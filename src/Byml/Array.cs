using Cead.Interop;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cead;

public partial class Byml
{
    public partial class Array : SafeHandle
    {
        [LibraryImport("Cead.lib")] private static unsafe partial Byml ArrayGet(IntPtr vector, int index);
        [LibraryImport("Cead.lib")] private static unsafe partial void ArraySet(IntPtr vector, int index, Byml value);
        [LibraryImport("Cead.lib")] private static unsafe partial void Clear(IntPtr vector);
        [LibraryImport("Cead.lib")] private static unsafe partial int Length(IntPtr vector);

        internal unsafe Array(IntPtr handle) : base(handle, true) { }

        public Byml this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ArrayGet(handle, index);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => ArraySet(handle, index, value);
        }

        public int Length => Size(handle);

        public override bool IsInvalid { get; }

        /// <summary>Gets an enumerator for this span.</summary>
        public Enumerator GetEnumerator() => new(this);

        /// <summary>Enumerates the elements of a <see cref="Span{T}"/>.</summary>
        public ref struct Enumerator
        {
            /// <summary>The span being enumerated.</summary>
            private readonly Array _vector;
            /// <summary>The next index to yield.</summary>
            private int _index;

            /// <summary>Initialize the enumerator.</summary>
            /// <param name="vector">The span to enumerate.</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal Enumerator(Array vector)
            {
                _vector = vector;
                _index = -1;
            }

            /// <summary>Advances the enumerator to the next element of the span.</summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                int index = _index + 1;
                if (index < _vector.Length)
                {
                    _index = index;
                    return true;
                }

                return false;
            }

            /// <summary>Gets the element at the current position of the enumerator.</summary>
            public Byml Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _vector[_index];
            }
        }

        /// <summary>
        /// Clears the contents of this span.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            Clear(handle);
        }

        protected override bool ReleaseHandle()
        {
            PtrHandle.FreePtr(handle);
            return true;
        }
    }
}