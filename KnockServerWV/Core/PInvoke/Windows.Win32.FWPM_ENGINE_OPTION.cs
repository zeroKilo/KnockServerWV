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
        public enum FWPM_ENGINE_OPTION
        {
            FWPM_ENGINE_COLLECT_NET_EVENTS = 0,
            FWPM_ENGINE_NET_EVENT_MATCH_ANY_KEYWORDS = 1,
            FWPM_ENGINE_NAME_CACHE = 2,
            FWPM_ENGINE_MONITOR_IPSEC_CONNECTIONS = 3,
            FWPM_ENGINE_PACKET_QUEUING = 4,
            FWPM_ENGINE_TXN_WATCHDOG_TIMEOUT_IN_MSEC = 5,
            FWPM_ENGINE_OPTION_MAX = 6,
        }
    }
}
