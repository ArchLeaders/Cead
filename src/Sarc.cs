#pragma warning disable CA1419 // Provide a parameterless constructor that is as visible as the containing type for concrete types derived from 'System.Runtime.InteropServices.SafeHandle'

using Cead.Interop;
using Microsoft.Win32.SafeHandles;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Cead;

public enum Endianness
{
    Big, Little
}

public enum Mode
{
    Legacy, New
}

public unsafe partial class Sarc : SafeHandleMinusOneIsInvalid
{
    [LibraryImport(CeadLib)] private static partial Sarc SarcFromBinary(byte* src, int src_len);
    [LibraryImport(CeadLib)] private static partial DataHandle SarcToBinary(IntPtr writer);
    [LibraryImport(CeadLib)] private static partial int GetNumFiles(IntPtr sarc);
    [LibraryImport(CeadLib)] private static partial int GetFileMapCount(IntPtr writer);
    [LibraryImport(CeadLib)] private static partial Endianness GetEndianness(IntPtr sarc);
    [LibraryImport(CeadLib)] private static partial Endianness SetEndianness(IntPtr writer, Endianness endianess);
    [LibraryImport(CeadLib, StringMarshalling = StringMarshalling.Utf8)][return: MarshalAs(UnmanagedType.Bool)] private static partial bool GetFile(IntPtr sarc, string name, out byte* dst, out int dst_len);
    [LibraryImport(CeadLib)] private static partial IntPtr NewSarcWriter(Endianness endian, Mode mode);
    [LibraryImport(CeadLib)] private static partial IntPtr GetSarcWriter(IntPtr sarc);
    [LibraryImport(CeadLib)] private static partial void SetWriterMode(IntPtr writer, Mode mode);
    [LibraryImport(CeadLib, StringMarshalling = StringMarshalling.Utf8)][return: MarshalAs(UnmanagedType.Bool)] private static partial bool SarcWriterGet(IntPtr writer, string name, out byte* dst, out int dst_len);
    [LibraryImport(CeadLib, StringMarshalling = StringMarshalling.Utf8)] private static partial void AddSarcFile(IntPtr writer, string name, byte* src, int src_len);
    [LibraryImport(CeadLib, StringMarshalling = StringMarshalling.Utf8)] private static partial void RemoveSarcFile(IntPtr writer, string name);
    [LibraryImport(CeadLib)] private static partial void ClearSarcFiles(IntPtr writer);

    [LibraryImport(CeadLib)] private static partial void SarcCurrent(IntPtr iterator, out byte* key_ptr, out byte* dst, out int dst_len);
    [LibraryImport(CeadLib)][return: MarshalAs(UnmanagedType.Bool)] private static partial bool SarcAdvance(IntPtr hash, IntPtr iterator, out IntPtr next);
    [LibraryImport(CeadLib)][return: MarshalAs(UnmanagedType.Bool)] private static partial bool FreeSarc(IntPtr sarc, IntPtr writer);

    private Sarc() : base(true) { }
    private Sarc(IntPtr _handle) : base(true) => handle = _handle;
    public Sarc(Endianness endian = Endianness.Little, Mode mode = Mode.New) : base(true)
    {
        _writer = NewSarcWriter(endian, mode);
    }

    public Span<byte> this[string key] {
        get {
            bool success = _writer > -1 ? SarcWriterGet((nint)_writer, key, out byte* ptr, out int len) : GetFile(handle, key, out ptr, out len);
            if (!success) {
                throw new KeyNotFoundException($"Could not find a file with the name '{key}'");
            }

            return new(ptr, len);
        }
    }

    public static Sarc FromBinary(byte[] data) => FromBinary(data.AsSpan());
    public static Sarc FromBinary(ReadOnlySpan<byte> data)
    {
        fixed (byte* ptr = data) {
            return SarcFromBinary(ptr, data.Length);
        }
    }

    public DataHandle ToBinary()
    {
        return SarcToBinary(Writer);
    }

    public void ToBinary(string file)
    {
        using DataHandle dataHandle = SarcToBinary(Writer);
        using FileStream fs = File.Create(file);
        fs.Write(dataHandle.AsSpan());
    }

    public int Count => _writer > -1 ? GetFileMapCount((nint)_writer) : GetNumFiles(handle);
    public Endianness Endian {
        get => GetEndianness(handle);
        set => SetEndianness(Writer, value);
    }

    public void Add(string name, byte[] data) => Add(name, data.AsSpan());
    public void Add(string name, ReadOnlySpan<byte> data)
    {
        fixed (byte* ptr = data) {
            AddSarcFile(Writer, name, ptr, data.Length);
        }
    }

    public void Remove(string name)
    {
        RemoveSarcFile(Writer, name);
    }

    public void Clear()
    {
        ClearSarcFiles(Writer);
    }

    public Enumerator GetEnumerator() => new(this);
    public ref struct Enumerator
    {
        private readonly Sarc _handle;
        private IntPtr _iterator;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Enumerator(Sarc handle)
        {
            _handle = handle;
            _iterator = IntPtr.Zero;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext() => SarcAdvance(_handle.Writer, _iterator, out _iterator);

        /// <summary>Gets the element at the current position of the enumerator.</summary>
        public KeyValuePair<string, SarcFile> Current {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                SarcCurrent(_iterator, out byte* keyPtr, out byte* dst, out int dstLen);
                return new(Utf8StringMarshaller.ConvertToManaged(keyPtr)!, new(dst, dstLen));
            }
        }
    }

    public readonly struct SarcFile
    {
        private readonly byte* _ptr;
        private readonly int _len;

        public SarcFile(byte* ptr, int len)
        {
            _ptr = ptr;
            _len = len;
        }

        public static implicit operator Span<byte>(SarcFile sarcFile) => sarcFile.AsSpan();
        public Span<byte> AsSpan()
        {
            return new(_ptr, _len);
        }
    }

    private IntPtr _writer = -1;
    private IntPtr Writer {
        get {
            if (_writer <= -1) {
                _writer = GetSarcWriter(handle);
            }

            return _writer;
        }
    }

    public override bool IsInvalid { get; }

    protected override bool ReleaseHandle()
    {
        return FreeSarc(handle, _writer);
    }
}
