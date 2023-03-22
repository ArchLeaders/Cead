using Microsoft.Win32.SafeHandles;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cead;

public partial class Byml
{
    public unsafe partial class Array : SafeHandleMinusOneIsInvalid
    {
        [LibraryImport(CeadLib)] private static partial Byml ArrayGet(IntPtr vector, int index);
        [LibraryImport(CeadLib)] private static partial void ArraySet(IntPtr vector, int index, Byml value);
        [LibraryImport(CeadLib, StringMarshalling = StringMarshalling.Utf8)] private static partial void ArrayAdd(IntPtr hash, Byml value);
        [LibraryImport(CeadLib, StringMarshalling = StringMarshalling.Utf8)] private static partial void ArrayRemove(IntPtr hash, int index);
        [LibraryImport(CeadLib)] private static partial void ArrayClear(IntPtr vector);
        [LibraryImport(CeadLib)] private static partial int ArrayLength(IntPtr vector);
        [LibraryImport(CeadLib)] private static partial Byml ArrayCurrent(IntPtr array, int index);
        [LibraryImport(CeadLib)] private static partial IntPtr BuildEmptyArray();
        [LibraryImport(CeadLib)][return: MarshalAs(UnmanagedType.Bool)] private static partial bool FreeArray(IntPtr array);

        public int Length => ArrayLength(handle);

        public Byml this[int index] {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ArrayGet(handle, index);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => ArraySet(handle, index, value);
        }

        public void Add(Byml node) => ArrayAdd(handle, node);
        public void Remove(int index) => ArrayRemove(handle, index);
        public void Clear() => ArrayClear(handle);

        public Array(IntPtr _handle) : base(true) => handle = _handle;

        public Array() : this(BuildEmptyArray()) { }

        public static implicit operator Array(Byml[] values) => new(values);
        public Array(params Byml[] values) : this(BuildEmptyArray())
        {
            for (int i = 0; i < values.Length; i++) {
                ArrayAdd(handle, values[i]);
            }
        }

        public Enumerator GetEnumerator() => new(handle, Length);
        public ref struct Enumerator
        {
            private readonly IntPtr _array;
            private readonly int _length;
            private int _index;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal Enumerator(IntPtr array, int length)
            {
                _array = array;
                _length = length;
                _index = -1;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                _index++;
                if (_index < _length) {
                    return true;
                }

                return false;
            }

            public Byml Current {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => ArrayCurrent(_array, _index);
            }
        }

        protected override bool ReleaseHandle()
        {
            return FreeArray(handle);
        }
    }
}