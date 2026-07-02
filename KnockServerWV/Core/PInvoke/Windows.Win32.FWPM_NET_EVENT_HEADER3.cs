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
        public enum FWP_IP_VERSION
        {
            FWP_IP_VERSION_V4 = 0,
            FWP_IP_VERSION_V6 = 1,
            FWP_IP_VERSION_NONE = 2,
            FWP_IP_VERSION_MAX = 3,
        }
        public enum FWP_AF
        {
            FWP_AF_INET = 0,
            FWP_AF_INET6 = 1,
            FWP_AF_ETHER = 2,
            FWP_AF_NONE = 3,
        }

        public partial struct FWP_BYTE_BLOB
        {
            public uint size;

            public unsafe byte* data;
        }

        public partial struct __byte_16
        {
            private const int SpanLength = 16;

            public unsafe fixed byte Value[SpanLength];

            public unsafe ref byte this[int index] => ref Value[index];
        }

        public partial struct FWP_BYTE_ARRAY16
        {
            public __byte_16 byteArray16;
        }

        public partial struct FWPM_NET_EVENT_HEADER3
        {
            public System.Runtime.InteropServices.ComTypes.FILETIME timeStamp;

            public uint flags;

            public FWP_IP_VERSION ipVersion;

            public byte ipProtocol;

            public _Anonymous1_e__Union Anonymous1;

            public _Anonymous2_e__Union Anonymous2;

            public ushort localPort;

            public ushort remotePort;

            public uint scopeId;

            public FWP_BYTE_BLOB appId;

            public IntPtr userId;

            public FWP_AF addressFamily;

            public IntPtr packageSid;

            public Foundation.PWSTR enterpriseId;

            public ulong policyFlags;

            public FWP_BYTE_BLOB effectiveName;

            public uint localAddrV4 => Anonymous1.localAddrV4;

            public FWP_BYTE_ARRAY16 localAddrV6 => Anonymous1.localAddrV6;

            public uint remoteAddrV4 => Anonymous2.remoteAddrV4;

            public FWP_BYTE_ARRAY16 remoteAddrV6 => Anonymous2.remoteAddrV6;

            [StructLayout(LayoutKind.Explicit)]
            public partial struct _Anonymous1_e__Union
            {
                [FieldOffset(0)]
                public uint localAddrV4;

                [FieldOffset(0)]
                public FWP_BYTE_ARRAY16 localAddrV6;
            }

            [StructLayout(LayoutKind.Explicit)]
            public partial struct _Anonymous2_e__Union
            {
                [FieldOffset(0)]
                public uint remoteAddrV4;
                
                [FieldOffset(0)]
                public FWP_BYTE_ARRAY16 remoteAddrV6;
            }
        }
    }
}
