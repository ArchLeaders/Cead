using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cead;

public partial class Byml
{
    public unsafe partial class Hash
    {
        [LibraryImport("Cead.lib", StringMarshalling = StringMarshalling.Utf8)] private static partial Byml HashGet(IntPtr hash, string key);
        [LibraryImport("Cead.lib", StringMarshalling = StringMarshalling.Utf8)] private static partial void HashSet(IntPtr hash, string key, Byml value);
        [LibraryImport("Cead.lib", StringMarshalling = StringMarshalling.Utf8)] private static partial void HashAdd(IntPtr hash, string key, Byml value);
        [LibraryImport("Cead.lib", StringMarshalling = StringMarshalling.Utf8)] private static partial void HashRemove(IntPtr hash, string key);
        [LibraryImport("Cead.lib", StringMarshalling = StringMarshalling.Utf8)][return: MarshalAs(UnmanagedType.Bool)] private static partial bool HashContains(IntPtr hash, string key);
        [LibraryImport("Cead.lib")] private static partial void HashClear(IntPtr hash);
        [LibraryImport("Cead.lib")] private static partial int HashLength(IntPtr hash);

        [LibraryImport("Cead.lib")] private static partial void HashCurrent(IntPtr iterator, out IntPtr key, out Byml value);
        [LibraryImport("Cead.lib")][return: MarshalAs(UnmanagedType.Bool)] private static partial bool HashAdvance(IntPtr hash, IntPtr iterator, out IntPtr next);
        [LibraryImport("Cead.lib")] private static partial IntPtr HashBegin(IntPtr hash);

        internal readonly IntPtr handle = IntPtr.Zero;

        public static implicit operator Hash(IntPtr ptr) => new(ptr);
        public Hash(nint handle) => this.handle = handle;

        public Byml this[string key] {
            get => HashGet(handle, key);
            set => HashSet(handle, key, value);
        }

        public int Length => HashLength(handle);

        public void Add(string key, Byml value) => HashAdd(handle, key, value);
        public void Remove(string key) => HashRemove(handle, key);
        public bool Contains(string key) => HashContains(handle, key);
        public void Clear() => HashClear(handle);

        /// <summary>Gets an enumerator for this span.</summary>
        public Enumerator GetEnumerator() => new(handle);

        /// <summary>Enumerates the elements of a <see cref="Span{T}"/>.</summary>
        public ref struct Enumerator
        {
            private readonly IntPtr _hash;
            private IntPtr _iterator;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal Enumerator(IntPtr hash)
            {
                _hash = hash;
                _iterator = IntPtr.Zero;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() => HashAdvance(_hash, _iterator, out _iterator);

            /// <summary>Gets the element at the current position of the enumerator.</summary>
            public KeyValuePair<string, Byml> Current {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get {
                    HashCurrent(_iterator, out IntPtr keyPtr, out Byml value);
                    return new(Marshal.PtrToStringUTF8(keyPtr)!, value);
                }
            }
        }
    }
}