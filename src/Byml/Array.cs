using Cead.Interop;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cead;

public partial class Byml
{
    public unsafe partial class Array : SafeHandle, IBymlObject
    {
        [LibraryImport("Cead.lib")] private static partial Byml ArrayGet(IntPtr vector, int index);
        [LibraryImport("Cead.lib")] private static partial void ArraySet(IntPtr vector, int index, Byml value);
        [LibraryImport("Cead.lib", StringMarshalling = StringMarshalling.Utf8)] private static partial void ArrayAdd(IntPtr hash, Byml value);
        [LibraryImport("Cead.lib", StringMarshalling = StringMarshalling.Utf8)] private static partial void ArrayRemove(IntPtr hash, int index);
        [LibraryImport("Cead.lib")] private static partial void ArrayClear(IntPtr vector);
        [LibraryImport("Cead.lib")] private static partial int ArrayLength(IntPtr vector);
        [LibraryImport("Cead.lib")] private static partial Byml ArrayCurrent(IntPtr array, int index);
        [LibraryImport("Cead.lib")] private static partial IntPtr BuildEmptyArray();
        [LibraryImport("Cead.lib")] private static partial IntPtr BuildArray(IntPtr* value, int value_len);

        public override bool IsInvalid { get; }
        public int Length => ArrayLength(handle);
        public bool IsOwner { get; set; } = true;

        public Byml this[int index] {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ArrayGet(handle, index);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => ArraySet(handle, index, value);
        }

        public void Add(Byml node) => ArrayAdd(handle, node);
        public void Remove(int index) => ArrayRemove(handle, index);
        public void Clear() => ArrayClear(handle);

        public Array() : base(BuildEmptyArray(), true) { }

        public static implicit operator Array(Byml[] value) => new(value);
        public Array(params Byml[] value) : base(BuildEmptyArray(), true)
        {
            for (int i = 0; i < value.Length; i++) {
                ArrayAdd(handle, value[i]);
            }
        }

        public static implicit operator Array(IntPtr[] value) => new(value);
        public Array(params IntPtr[] array) : base(BuildArray(array), true) { }
        private static IntPtr BuildArray(IntPtr[] value)
        {
            fixed(IntPtr* ptr = value) {
                return BuildArray(ptr, value.Length);
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
            // Only dispose the resource if the Array
            // does not have a parent (owns itself)
            if (IsOwner) {
                PtrHandle.FreePtr(handle);
            }

            return true;
        }
    }
}