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
        public enum FWPM_NET_EVENT_TYPE
        {
            FWPM_NET_EVENT_TYPE_IKEEXT_MM_FAILURE = 0,
            FWPM_NET_EVENT_TYPE_IKEEXT_QM_FAILURE = 1,
            FWPM_NET_EVENT_TYPE_IKEEXT_EM_FAILURE = 2,
            FWPM_NET_EVENT_TYPE_CLASSIFY_DROP = 3,
            FWPM_NET_EVENT_TYPE_IPSEC_KERNEL_DROP = 4,
            FWPM_NET_EVENT_TYPE_IPSEC_DOSP_DROP = 5,
            FWPM_NET_EVENT_TYPE_CLASSIFY_ALLOW = 6,
            FWPM_NET_EVENT_TYPE_CAPABILITY_DROP = 7,
            FWPM_NET_EVENT_TYPE_CAPABILITY_ALLOW = 8,
            FWPM_NET_EVENT_TYPE_CLASSIFY_DROP_MAC = 9,
            FWPM_NET_EVENT_TYPE_LPM_PACKET_ARRIVAL = 10,
            FWPM_NET_EVENT_TYPE_MAX = 11,
        }

        public enum FWPM_NET_EVENT_FLAG
        {
            IP_PROTOCOL_SET = 0x00000001,
            LOCAL_ADDR_SET = 0x00000002,
            REMOTE_ADDR_SET = 0x00000004,
            LOCAL_PORT_SET = 0x00000008,
            REMOTE_PORT_SET = 0x00000010,
            APP_ID_SET = 0x00000020,
            USER_ID_SET = 0x00000040,
            SCOPE_ID_SET = 0x00000080,
            IP_VERSION_SET = 0x00000100,
            REAUTH_REASON_SET = 0x00000200,
            PACKAGE_ID_SET = 0x00000400,
            ENTERPRISE_ID_SET = 0x00000800,
            POLICY_FLAGS_SET = 0x00001000,
            EFFECTIVE_NAME_SET = 0x00002000,
        }

        public partial struct FWPM_NET_EVENT5
        {
            public FWPM_NET_EVENT_HEADER3 header;

            public FWPM_NET_EVENT_TYPE type;

            public _Anonymous_e__Union u;

            [StructLayout(LayoutKind.Explicit)]
            public partial struct _Anonymous_e__Union
            {
                [FieldOffset(0)]
                public unsafe void* ikeMmFailure;

                [FieldOffset(0)]
                public unsafe void* ikeQmFailure;

                [FieldOffset(0)]
                public unsafe void* ikeEmFailure;

                [FieldOffset(0)]
                public unsafe void* classifyDrop;

                [FieldOffset(0)]
                public unsafe void* ipsecDrop;

                [FieldOffset(0)]
                public unsafe void* idpDrop;

                [FieldOffset(0)]
                public unsafe void* classifyAllow;

                [FieldOffset(0)]
                public unsafe void* capabilityDrop;

                [FieldOffset(0)]
                public unsafe void* capabilityAllow;

                [FieldOffset(0)]
                public unsafe void* classifyDropMac;

                [FieldOffset(0)]
                public unsafe void* lpmPacketArrival;
            }
        }
    }
}
