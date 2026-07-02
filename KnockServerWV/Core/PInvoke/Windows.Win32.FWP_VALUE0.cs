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
    namespace NetworkManagement.WindowsFilteringPlatform
    {
        public enum FWP_DATA_TYPE
        {
            FWP_EMPTY = 0,
            FWP_UINT8 = 1,
            FWP_UINT16 = 2,
            FWP_UINT32 = 3,
            FWP_UINT64 = 4,
            FWP_INT8 = 5,
            FWP_INT16 = 6,
            FWP_INT32 = 7,
            FWP_INT64 = 8,
            FWP_FLOAT = 9,
            FWP_DOUBLE = 10,
            FWP_BYTE_ARRAY16_TYPE = 11,
            FWP_BYTE_BLOB_TYPE = 12,
            FWP_SID = 13,
            FWP_SECURITY_DESCRIPTOR_TYPE = 14,
            FWP_TOKEN_INFORMATION_TYPE = 15,
            FWP_TOKEN_ACCESS_INFORMATION_TYPE = 16,
            FWP_UNICODE_STRING_TYPE = 17,
            FWP_BYTE_ARRAY6_TYPE = 18,
            FWP_SINGLE_DATA_TYPE_MAX = 255,
            FWP_V4_ADDR_MASK = 256,
            FWP_V6_ADDR_MASK = 257,
            FWP_RANGE_TYPE = 258,
            FWP_DATA_TYPE_MAX = 259,
        }

        public partial struct FWP_VALUE0
        {
            public FWP_DATA_TYPE type;

            public _Anonymous_e__Union u;

            [StructLayout(LayoutKind.Explicit)]
            public partial struct _Anonymous_e__Union
            {
                [FieldOffset(0)]
                public byte uint8;

                [FieldOffset(0)]
                public ushort uint16;

                [FieldOffset(0)]
                public uint uint32;

                [FieldOffset(0)]
                public unsafe ulong* uint64;

                [FieldOffset(0)]
                public sbyte int8;

                [FieldOffset(0)]
                public short int16;

                [FieldOffset(0)]
                public int int32;

                [FieldOffset(0)]
                public unsafe long* int64;

                [FieldOffset(0)]
                public float float32;

                [FieldOffset(0)]
                public unsafe double* double64;

                [FieldOffset(0)]
                public unsafe FWP_BYTE_ARRAY16* byteArray16;

                [FieldOffset(0)]
                public unsafe FWP_BYTE_BLOB* byteBlob;

                [FieldOffset(0)]
                public IntPtr sid;

                [FieldOffset(0)]
                public unsafe FWP_BYTE_BLOB* sd;

                [FieldOffset(0)]
                public IntPtr tokenInformation;

                [FieldOffset(0)]
                public IntPtr tokenAccessInformation;

                [FieldOffset(0)]
                public winmdroot.Foundation.PWSTR unicodeString;

                [FieldOffset(0)]
                public IntPtr byteArray6;
            }
        }
    }
}
