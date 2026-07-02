using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.NetworkManagement.WindowsFilteringPlatform;
using NetFwTypeLib;

namespace KnockServerWV
{
    public static class Server
    {
        public class KnockUser
        {
            public bool isIP4 = true;
            public uint IP4;
            public DateTime Updated;
            public bool Authenticated = false;
            public List<uint> knocks = new List<uint>();

            public KnockUser(uint ip4)
            {
                IP4 = ip4;
                Updated = DateTime.Now;
            }
        }

        //global
        public static readonly object _sync = new object();
        public static bool isRunning = false;
        public static Dictionary<string, string> config = new Dictionary<string, string>();
        public static List<string> controlledRules = new List<string>();
        public static List<PortWatcher> watchers = new List<PortWatcher>();
        public static Dictionary<uint, KnockUser> knockUsers = new Dictionary<uint, KnockUser>();
        private static List<uint> knockUpdates = new List<uint>();

        //firewall event handling
        private static unsafe PInvoke.FWPM_ENGINE_HANDLE* filterEngineHandle;
        private static unsafe HANDLE* filterEventHandle;
        private static unsafe FWP_VALUE0* filterOption0;
        private static unsafe FWP_VALUE0* filterOption1;
        private static unsafe FWP_VALUE0* filterOption2;
        private static unsafe FWPM_NET_EVENT_SUBSCRIPTION0* eventSubscription;
        private static FWPM_NET_EVENT_CALLBACK4 eventHandler;

        //firewall management
        public static int[] currentKnockSequence;
        public static int maxUserTimeout = 60;
        public static int basePort = 60;
        public static int range = 60;
        public static List<string> currentIPs = new List<string>();
        public static List<uint[]> ip4knocks = new List<uint[]>();
        private static INetFwPolicy2 fwMgr;

        public static void Start()
        {
            if (isRunning)
                return;
            isRunning = true;
            int basePort = int.Parse(config["baseport"]);
            int range = int.Parse(config["range"]);
            watchers.Clear();
            for (int i = 0; i < range; i++)
                watchers.Add(new PortWatcher((ushort)(basePort + i)));
            Logger.Log("Knockserver started");
        }

        public static void Stop()
        {
            if (!isRunning)
                return;
            isRunning = false;
            foreach (PortWatcher watcher in watchers)
                watcher.Stop();
            watchers.Clear();
            Logger.Log("Knockserver stopped");
        }

        public static void LoadConfig(string pathConfig, string pathRules)
        {
            config.Clear();
            string[] lines = File.ReadAllLines(pathConfig);
            foreach (string line in lines)
            {
                string[] parts = line.Split('=');
                if (parts.Length == 2)
                    config.Add(parts[0].ToLower().Trim(), parts[1].Trim());
            }
            controlledRules.Clear();
            lines = File.ReadAllLines(pathRules);
            foreach (string line in lines)
                if (line.Trim() != "")
                {
                    string s = line.ToLower().Trim();
                    controlledRules.Add(s);
                    Logger.Log("Added controlled rule " + s);
                }
            maxUserTimeout = int.Parse(config["usertimeout"]);
            Logger.maxEntries = int.Parse(config["maxlogdisplay"]);
            currentIPs.Add("127.0.0.1");
            fwMgr = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
            UpdateFirewallRules();
        }

        public static int[] MakeCurrentKnockSequence()
        {
            lock (_sync)
            {
                List<int> result = new List<int>();
                DateTime d = DateTime.Now;
                int a = d.Day;
                int b = (int)d.DayOfWeek;
                int c = d.DayOfYear;
                int salt1 = int.Parse(config["salt1"]);
                int salt2 = int.Parse(config["salt2"]);
                basePort = int.Parse(config["baseport"]);
                range = int.Parse(config["range"]);
                int length = int.Parse(config["length"]);
                int seed = (b + salt1) * (c + a + salt2);
                Random rnd = new Random(seed);
                for (int i = 0; i < length; i++)
                {
                    int tmp = basePort + rnd.Next() % range;
                    while (result.Contains(tmp))
                        tmp = basePort + rnd.Next() % range;
                    result.Add(tmp);
                }
                currentKnockSequence = result.ToArray();
                return currentKnockSequence;
            }
        }

        public static void Update()
        {
            bool needsRuleUpdate = false;
            lock (_sync)
            {
                foreach (uint[] knock in ip4knocks)
                {
                    uint ip = knock[0];
                    if (!knockUsers.ContainsKey(ip))
                    {
                        Logger.Log("New user from " + Ip4ToString(ip), false);
                        knockUsers.Add(ip, new KnockUser(ip));
                    }
                    KnockUser u = knockUsers[ip];
                    u.Updated = DateTime.Now;
                    if (!u.Authenticated)
                    {
                        uint port = knock[1];
                        if (u.knocks.Count > 0 && u.knocks[u.knocks.Count - 1] == port)
                            return;
                        Logger.Log("KNOCK from " + Ip4ToString(ip) + " to local port " + port);
                        u.knocks.Add(port);
                        while (u.knocks.Count > currentKnockSequence.Length)
                            u.knocks.RemoveAt(0);
                        for (int i = 0; i < u.knocks.Count; i++)
                            if (u.knocks[i] != currentKnockSequence[i])
                            {
                                u.knocks = new List<uint>();
                                break;
                            }
                        if (u.knocks.Count == currentKnockSequence.Length && !u.Authenticated)
                        {
                            u.Authenticated = true;
                            string ips = Ip4ToString(ip);
                            Logger.Log("Authenticated user from IP " + ips);
                            if (!currentIPs.Contains(ips))
                                currentIPs.Add(ips);
                            needsRuleUpdate = true;
                        }
                    }
                }
                ip4knocks.Clear();
                DateTime now = DateTime.Now;
                if (knockUpdates.Count > 0)
                {
                    foreach (uint ip in knockUpdates)
                        if (knockUsers.ContainsKey(ip))
                            knockUsers[ip].Updated = now;
                        else
                            Logger.Log("Cant update user with ip " + Ip4ToString(ip), false);
                    Logger.Log("Processed " + knockUpdates.Count + " knock updates", false);
                    knockUpdates.Clear();
                }
                for (int i = 0; i < knockUsers.Keys.Count; i++)
                {
                    uint key = knockUsers.Keys.ToArray()[i];
                    KnockUser u = knockUsers[key];
                    TimeSpan t = now - u.Updated;
                    if (t.TotalSeconds > maxUserTimeout)
                    {
                        u.knocks.Clear();
                        u.Authenticated = false;
                        knockUsers.Remove(key);
                        string ip = Ip4ToString(u.IP4);
                        Logger.Log("Removed user with IP " + ip);
                        if (currentIPs.Contains(ip))
                            currentIPs.Remove(ip);
                        needsRuleUpdate = true;
                        i--;
                    }
                }
            }
            if (needsRuleUpdate)
                UpdateFirewallRules();
        }

        public static void UpdateIP4Knock(uint ip)
        { 
            lock(_sync)
            {
                knockUpdates.Add(ip);
            }
        }

        public static void StartFilterEngine()
        {
            lock (_sync)
            {
                unsafe
                {
                    uint result;
                    filterEngineHandle = (PInvoke.FWPM_ENGINE_HANDLE*)AllocMemZeroed(sizeof(PInvoke.FWPM_ENGINE_HANDLE));
                    if ((result = PInvoke.FwpmEngineOpen0(null, 10, IntPtr.Zero, IntPtr.Zero, filterEngineHandle)) != 0)
                    {
                        Logger.Log("Error: Failed to open filter engine handle " + result.ToString("X8"));
                        return;
                    }
                    else
                        Logger.Log("Opened filter engine handle");

                    filterOption0 = MakeNewUIntValue(1);
                    if ((result = PInvoke.FwpmEngineSetOption0(*filterEngineHandle, FWPM_ENGINE_OPTION.FWPM_ENGINE_COLLECT_NET_EVENTS, filterOption0)) != 0)
                    {
                        Logger.Log("Error: Failed to set option FWPM_ENGINE_COLLECT_NET_EVENTS " + result.ToString("X8"));
                        return;
                    }
                    else
                        Logger.Log("Set option FWPM_ENGINE_COLLECT_NET_EVENTS");

                    filterOption1 = MakeNewUIntValue(4);
                    if ((result = PInvoke.FwpmEngineSetOption0(*filterEngineHandle, FWPM_ENGINE_OPTION.FWPM_ENGINE_NET_EVENT_MATCH_ANY_KEYWORDS, filterOption1)) != 0)
                    {
                        Logger.Log("Error: Failed to set option FWPM_ENGINE_NET_EVENT_MATCH_ANY_KEYWORDS " + result.ToString("X8"));
                        return;
                    }
                    else
                        Logger.Log("Set option FWPM_ENGINE_NET_EVENT_MATCH_ANY_KEYWORDS");

                    filterOption2 = MakeNewUIntValue(1);
                    if ((result = PInvoke.FwpmEngineSetOption0(*filterEngineHandle, FWPM_ENGINE_OPTION.FWPM_ENGINE_MONITOR_IPSEC_CONNECTIONS, filterOption2)) != 0)
                    {
                        Logger.Log("Error: Failed to set option FWPM_ENGINE_MONITOR_IPSEC_CONNECTIONS " + result.ToString("X8"));
                        return;
                    }
                    else
                        Logger.Log("Set option FWPM_ENGINE_MONITOR_IPSEC_CONNECTIONS");

                    filterEventHandle = (HANDLE*)AllocMemZeroed(sizeof(HANDLE));
                    eventSubscription = (FWPM_NET_EVENT_SUBSCRIPTION0*)AllocMemZeroed(sizeof(FWPM_NET_EVENT_SUBSCRIPTION0));
                    eventHandler = FilterCallback;
                    if ((result = PInvoke.FwpmNetEventSubscribe4(*filterEngineHandle, eventSubscription, eventHandler, null, filterEventHandle)) != 0)
                    {
                        Logger.Log("Error: Failed to register event handler " + result.ToString("X8"));
                        return;
                    }
                    else
                        Logger.Log("Registered event handler");
                }
            }
        }

        private static unsafe void FilterCallback(void* context, FWPM_NET_EVENT5* e)
        {
            Logger.Log("Received event type " + e->type + " flags " + e->header.flags.ToString("X8"), false);
            if (CheckFlags(e->header.flags) &&
                e->type == FWPM_NET_EVENT_TYPE.FWPM_NET_EVENT_TYPE_CLASSIFY_DROP &&
                e->header.ipVersion == FWP_IP_VERSION.FWP_IP_VERSION_V4 &&
                e->header.localPort >= basePort &&
                e->header.localPort < basePort + range)
            {
                lock (_sync)
                {
                    ip4knocks.Add(new uint[] { e->header.remoteAddrV4, e->header.localPort });
                }
            }
        }

        private static void UpdateFirewallRules()
        {
            lock (_sync)
            {
                string ips = String.Join(",", currentIPs.ToArray());
                Logger.Log("Updating firewall rules with IPs: " + ips, false);
                for(int i = 0; i < controlledRules.Count; i++)
                {
                    string rule = controlledRules[i];
                    Logger.Log("Updating firewall rule " + rule, false);
                    INetFwRule2 fwRule = null;
                    foreach (INetFwRule2 r in fwMgr.Rules)
                        if (r.Name.ToLower() == rule)
                        {
                            fwRule = r;
                            break;
                        }
                    if (fwRule == null)
                    {
                        Logger.Log("Did not found rule " + rule + ", removed it from list");
                        controlledRules.RemoveAt(i);
                        i--;
                    }
                    else
                        fwRule.RemoteAddresses = ips;
                }
                Logger.Log("Updated firewall rules");
            }
        }

        private static IntPtr AllocMemZeroed(int cb)
        {
            IntPtr result = Marshal.AllocHGlobal(cb);
            Extension.RtlZeroMemory(result, new IntPtr(cb));
            Logger.Log("Mem allocated at " + result.ToInt64().ToString("X16") + " size " + cb.ToString("X8"), false);
            return result;
        }

        private static unsafe FWP_VALUE0* MakeNewUIntValue(uint u)
        {
            FWP_VALUE0* result = (FWP_VALUE0*)AllocMemZeroed(sizeof(FWP_VALUE0));
            result->type = FWP_DATA_TYPE.FWP_UINT32;
            result->u.uint32 = u;
            return result;
        }

        public static string Ip4ToString(uint ip)
        {
            byte[] b = BitConverter.GetBytes(ip);
            return b[3] + "." + b[2] + "." + b[1] + "." + b[0];
        }

        private static bool CheckFlags(uint f)
        {
            if ((f & (uint)FWPM_NET_EVENT_FLAG.IP_VERSION_SET) == 0)
                return false;
            if ((f & (uint)FWPM_NET_EVENT_FLAG.REMOTE_ADDR_SET) == 0)
                return false;
            if ((f & (uint)FWPM_NET_EVENT_FLAG.LOCAL_PORT_SET) == 0)
                return false;
            return true;
        }
    }
}