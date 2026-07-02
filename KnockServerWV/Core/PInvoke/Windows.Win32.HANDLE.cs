#pragma warning disable CS1591,CS1573,CS0465,CS0649,CS8019,CS1570,CS1584,CS1658,CS0436,CS8981,SYSLIB1092,CS3016
using global::System;
using global::System.Diagnostics;
using global::System.Diagnostics.CodeAnalysis;
using global::System.Runtime.CompilerServices;
using global::System.Runtime.InteropServices;
using global::System.Runtime.Versioning;
using winmdroot = global::Windows.Win32;
namespace Windows.Win32
{
    namespace Foundation
    {
        public unsafe partial struct HANDLE
            : IEquatable<HANDLE>
        {
            public IntPtr Value;

            public HANDLE(IntPtr value)
            {
                Value = value;
            }

            public static HANDLE Null => default;

            public bool IsNull => Value == default;

            public static bool operator ==(HANDLE left, HANDLE right) => left.Value == right.Value;

            public static bool operator !=(HANDLE left, HANDLE right) => !(left == right);

            public bool Equals(HANDLE other) => this.Value == other.Value;

            public override bool Equals(object obj) => obj is HANDLE other && this.Equals(other);

            public override int GetHashCode() => unchecked((int)this.Value);
        }
    }
}
