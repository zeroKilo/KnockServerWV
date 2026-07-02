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
        [UnmanagedFunctionPointerAttribute(CallingConvention.Winapi)]
        public unsafe delegate void FWPM_NET_EVENT_CALLBACK4(void* context, FWPM_NET_EVENT5* e);
    }
}
