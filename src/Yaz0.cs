using Cead.Interop;
using System.Buffers.Binary;
using System.Runtime.InteropServices;

namespace Cead;

public static unsafe partial class Yaz0
{
    [LibraryImport("Cead.lib")]
    internal static unsafe partial void Compress(byte* src, int src_len, out VectorSafeHandle dst_handle, out byte* dst, out int dst_len, uint data_alignment, int level);

    [LibraryImport("Cead.lib")]
    internal static unsafe partial void Decompress(byte* src, int src_len, byte* dst, int dst_len);

    [LibraryImport("Cead.lib")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool FreeVector(IntPtr vector_ptr);

    public static unsafe Span<byte> Compress(string file, out VectorSafeHandle handle, int level = 7) => Compress(File.ReadAllBytes(file), out handle, level);
    public static unsafe Span<byte> Compress(ReadOnlySpan<byte> src, out VectorSafeHandle handle, int level = 7)
    {
        DllManager.Load();

        fixed (byte* srcPtr = src) {
            Compress(srcPtr, src.Length, out handle, out byte* dst, out int dstLen, 0, level);
            return new(dst, dstLen);
        }
    }

    public static unsafe Span<byte> Decompress(string file) => Decompress(File.ReadAllBytes(file));
    public static unsafe Span<byte> Decompress(ReadOnlySpan<byte> src)
    {
        DllManager.Load();

        Span<byte> dst = new byte[BinaryPrimitives.ReadUInt32BigEndian(src[0x04..0x08])];

        fixed (byte* src_ptr = src) {
            fixed (byte* dst_ptr = dst) {
                Decompress(src_ptr, src.Length, dst_ptr, dst.Length);
            }
        }

        return dst;
    }
}
