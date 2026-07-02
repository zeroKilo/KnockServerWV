using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows.Win32
{
    public static partial class PInvoke
    {
        public unsafe readonly partial struct FWPM_ENGINE_HANDLE : IEquatable<FWPM_ENGINE_HANDLE>
        {
            public readonly void* Value;

            public FWPM_ENGINE_HANDLE(void* value) => this.Value = value;

            public FWPM_ENGINE_HANDLE(IntPtr value) : this(unchecked((void*)value))
            {
            }

            public static FWPM_ENGINE_HANDLE Null => default;

            public bool IsNull => Value == default;

            public static implicit operator void*(FWPM_ENGINE_HANDLE value) => value.Value;

            public static explicit operator FWPM_ENGINE_HANDLE(void* value) => new FWPM_ENGINE_HANDLE(value);

            public static bool operator ==(FWPM_ENGINE_HANDLE left, FWPM_ENGINE_HANDLE right) => left.Value == right.Value;

            public static bool operator !=(FWPM_ENGINE_HANDLE left, FWPM_ENGINE_HANDLE right) => !(left == right);

            public bool Equals(FWPM_ENGINE_HANDLE other) => this.Value == other.Value;

            public override bool Equals(object obj) => obj is FWPM_ENGINE_HANDLE other && this.Equals(other);

            public override int GetHashCode() => unchecked((int)this.Value);

            public static implicit operator IntPtr(FWPM_ENGINE_HANDLE value) => new IntPtr(value.Value);

            public static explicit operator FWPM_ENGINE_HANDLE(IntPtr value) => new FWPM_ENGINE_HANDLE((void*)value.ToPointer());

            public static explicit operator FWPM_ENGINE_HANDLE(UIntPtr value) => new FWPM_ENGINE_HANDLE((void*)value.ToPointer());
        }
    }
}
