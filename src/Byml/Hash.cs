#pragma warning disable CA1419 // Provide a parameterless constructor that is as visible as the containing type for concrete types derived from 'System.Runtime.InteropServices.SafeHandle'

using Cead.Handles;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Cead;

public partial class Byml
{
    public unsafe partial class Hash : BymlHandle
    {
        [LibraryImport(CeadLib, StringMarshalling = StringMarshalling.Utf8)] private static partial Byml HashGet(IntPtr hash, string key);
        [LibraryImport(CeadLib, StringMarshalling = StringMarshalling.Utf8)] private static partial void HashSet(IntPtr hash, string key, Byml value);
        [LibraryImport(CeadLib, StringMarshalling = StringMarshalling.Utf8)] private static partial void HashAdd(IntPtr hash, string key, Byml value);
        [LibraryImport(CeadLib, StringMarshalling = StringMarshalling.Utf8)] private static partial void HashRemove(IntPtr hash, string key);
        [LibraryImport(CeadLib, StringMarshalling = StringMarshalling.Utf8)][return: MarshalAs(UnmanagedType.Bool)] private static partial bool HashContainsKey(IntPtr hash, string key);
        [LibraryImport(CeadLib)] private static partial void HashClear(IntPtr hash);
        [LibraryImport(CeadLib)] private static partial int HashLength(IntPtr hash);
        [LibraryImport(CeadLib)] private static partial void HashCurrent(IntPtr iterator, out byte* key, out Byml value);
        [LibraryImport(CeadLib)][return: MarshalAs(UnmanagedType.Bool)] private static partial bool HashAdvance(IntPtr hash, IntPtr iterator, out IntPtr next);
        [LibraryImport(CeadLib)] private static partial IntPtr BuildEmptyHash();
        [LibraryImport(CeadLib)][return: MarshalAs(UnmanagedType.Bool)] private static partial bool FreeHash(IntPtr hash);

        public int Length => HashLength(handle);

        public Byml this[string key] {
            get => HashGet(handle, key);
            set => HashSet(handle, key, value);
        }

        public void Add(string key, Byml value) => HashAdd(handle, key, value);
        public void Remove(string key) => HashRemove(handle, key);
        public bool ContainsKey(string key) => HashContainsKey(handle, key);
        public void Clear() => HashClear(handle);

        public Hash(IntPtr _handle) : base(true) => handle = _handle;

        public Hash() : this(BuildEmptyHash()) { }

        public static implicit operator Hash(Dictionary<string, Byml> values) => new(values);
        public Hash(Dictionary<string, Byml> values) : this(BuildEmptyHash())
        {
            foreach ((var key, var value) in values) {
                HashAdd(handle, key, value);
            }
        }

        public Enumerator GetEnumerator() => new(handle);
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
                    HashCurrent(_iterator, out byte* keyPtr, out Byml value);
                    return new(Utf8StringMarshaller.ConvertToManaged(keyPtr)!, value);
                }
            }
        }

        protected override bool ReleaseHandle()
        {
            return _isChild || FreeHash(handle);
        }
    }
}