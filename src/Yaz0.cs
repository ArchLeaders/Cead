using Cead.Interop;
using System.Buffers.Binary;
using System.Runtime.InteropServices;

namespace Cead;

public static unsafe partial class Yaz0
{
    [LibraryImport(CeadLib)] private static partial DataHandle Compress(byte* src, int src_len, uint data_alignment, int level);
    [LibraryImport(CeadLib)] private static partial void Decompress(byte* src, int src_len, byte* dst, int dst_len);

    public static DataHandle Compress(string file, int level = 7) => Compress(File.ReadAllBytes(file), level);
    public static DataHandle Compress(ReadOnlySpan<byte> src, int level = 7)
    {
        fixed (byte* srcPtr = src) {
            return Compress(srcPtr, src.Length, 0, level);
        }
    }

    public static Span<byte> Decompress(string file) => Decompress(File.ReadAllBytes(file));
    public static Span<byte> Decompress(ReadOnlySpan<byte> src)
    {
        Span<byte> dst = new byte[BinaryPrimitives.ReadUInt32BigEndian(src[0x04..0x08])];

        fixed (byte* src_ptr = src) {
            fixed (byte* dst_ptr = dst) {
                Decompress(src_ptr, src.Length, dst_ptr, dst.Length);
            }
        }

        return dst;
    }
}
