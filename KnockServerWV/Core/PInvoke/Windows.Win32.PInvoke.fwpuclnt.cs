#pragma warning disable CS1591,CS1573,CS0465,CS0649,CS8019,CS1570,CS1584,CS1658,CS0436,CS8981,SYSLIB1092,CS3016
using global::System;
using global::System.Diagnostics;
using global::System.Diagnostics.CodeAnalysis;
using global::System.Runtime.CompilerServices;
using global::System.Runtime.InteropServices;
using global::System.Runtime.Versioning;
using KnockServerWV;
namespace Windows.Win32
{
    public static partial class PInvoke
    {       
        [DllImport("fwpuclnt.dll", ExactSpelling = true), DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        public static extern unsafe uint FwpmEngineOpen0([Optional] Foundation.PCWSTR serverName, uint authnService, [Optional] IntPtr authIdentity, [Optional] IntPtr session, FWPM_ENGINE_HANDLE* engineHandle);

        [DllImport("fwpuclnt.dll", ExactSpelling = true), DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        public static extern unsafe uint FwpmNetEventSubscribe4(
            FWPM_ENGINE_HANDLE engineHandle, 
            NetworkManagement.WindowsFilteringPlatform.FWPM_NET_EVENT_SUBSCRIPTION0* subscription, 
            [MarshalAs(UnmanagedType.FunctionPtr)] NetworkManagement.WindowsFilteringPlatform.FWPM_NET_EVENT_CALLBACK4 callback, 
            [Optional] void* context, 
            Foundation.HANDLE* eventsHandle);
        
        [DllImport("fwpuclnt.dll", ExactSpelling = true), DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        public static extern unsafe uint FwpmEngineSetOption0(FWPM_ENGINE_HANDLE engineHandle, NetworkManagement.WindowsFilteringPlatform.FWPM_ENGINE_OPTION option, NetworkManagement.WindowsFilteringPlatform.FWP_VALUE0* newValue);
        
        [DllImport("fwpuclnt.dll", ExactSpelling = true), DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        public static extern uint FwpmNetEventUnsubscribe0(FWPM_ENGINE_HANDLE engineHandle, Foundation.HANDLE eventsHandle);
        
        [DllImport("fwpuclnt.dll", ExactSpelling = true), DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        public static extern uint FwpmEngineClose0(FWPM_ENGINE_HANDLE engineHandle);
    }
}
