#pragma warning disable CS1591, CS1573, CS0465, CS0649, CS8019, CS1570, CS1584, CS1658, CS0436, CS8981, SYSLIB1092, CS3016
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
		public unsafe readonly partial struct PWSTR
			: IEquatable<PWSTR>
		{
			public readonly char* Value;

			public PWSTR(char* value) => this.Value = value;

			public PWSTR(IntPtr value) : this(unchecked((char*)value))
			{
			}

			public static implicit operator char*(PWSTR value) => value.Value;

			public static implicit operator PWSTR(char* value) => new PWSTR(value);

			public static bool operator ==(PWSTR left, PWSTR right) => left.Value == right.Value;

			public static bool operator !=(PWSTR left, PWSTR right) => !(left == right);

			public bool Equals(PWSTR other) => this.Value == other.Value;

			public override bool Equals(object obj) => obj is PWSTR other && this.Equals(other);

			public override int GetHashCode() => unchecked((int)this.Value);

			public override string ToString() => new PCWSTR(this.Value).ToString();

			public static implicit operator PCWSTR(PWSTR value) => new PCWSTR(value.Value);

			public int Length => new PCWSTR(this.Value).Length;
		}
	}
}
