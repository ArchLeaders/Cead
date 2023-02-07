using Cead.Interop;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cead;

public partial class Byml
{
    public partial class Array
    {
        [LibraryImport("Cead.lib")] private static unsafe partial Byml ArrayGet(IntPtr vector, int index);
        [LibraryImport("Cead.lib")] private static unsafe partial void ArraySet(IntPtr vector, int index, Byml value);
        [LibraryImport("Cead.lib", StringMarshalling = StringMarshalling.Utf8)] private static partial void ArrayAdd(IntPtr hash, string key, Byml value);
        [LibraryImport("Cead.lib", StringMarshalling = StringMarshalling.Utf8)] private static partial void ArrayRemove(IntPtr hash, string key);
        [LibraryImport("Cead.lib", StringMarshalling = StringMarshalling.Utf8)][return: MarshalAs(UnmanagedType.Bool)] private static partial bool ArrayContains(IntPtr hash, string key);
        [LibraryImport("Cead.lib")] private static unsafe partial void ArrayClear(IntPtr vector);
        [LibraryImport("Cead.lib")] private static unsafe partial int ArrayLength(IntPtr vector);
        [LibraryImport("Cead.lib")] private static partial Byml ArrayCurrent(IntPtr array, int index);

        public static implicit operator Array(IntPtr ptr) => new(ptr);
        internal unsafe Array(IntPtr handle) => this.handle = handle;

        private readonly IntPtr handle = IntPtr.Zero;

        public Byml this[int index] {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ArrayGet(handle, index);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => ArraySet(handle, index, value);
        }

        public int Length => ArrayLength(handle);

        public void Add() => ArrayClear(handle);
        public void Remove() => ArrayClear(handle);
        public void Clear() => ArrayClear(handle);

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
    }
}