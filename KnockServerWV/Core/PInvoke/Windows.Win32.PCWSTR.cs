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
		public unsafe readonly partial struct PCWSTR
			: IEquatable<PCWSTR>
		{
			public readonly char* Value;

			public PCWSTR(char* value) => this.Value = value;

			public static explicit operator char*(PCWSTR value) => value.Value;

			public static implicit operator PCWSTR(char* value) => new PCWSTR(value);

			public bool Equals(PCWSTR other) => this.Value == other.Value;

			public override bool Equals(object obj) => obj is PCWSTR other && this.Equals(other);

			public override int GetHashCode() => unchecked((int)this.Value);

			public int Length
			{
				get
				{
					char* p = this.Value;
					if (p == null)
						return 0;
					while (*p != '\0')
						p++;
					return checked((int)(p - this.Value));
				}
			}
		}
	}
}
