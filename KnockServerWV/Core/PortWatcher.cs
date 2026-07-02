using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace KnockServerWV
{
    public class PortWatcher
    {
        public ushort port;
        private bool isRunning = false;
        private TcpListener host;
        private readonly object _sync = new object();

        public PortWatcher(ushort port)
        {
            this.port = port;
            try
            {
                host = new TcpListener(IPAddress.Any, port);
                host.Start();
                new Thread(tMain).Start();
            }
            catch (Exception e)
            {
                Logger.Log("Error in constructor of portwatcher " + port + " : " + e.Message, false);
            }
        }

        public void Stop()
        {
            try
            {
                SetRunning(false);
                if (host != null)
                    host.Stop();
            }
            catch { }
        }

        public bool IsRunning()
        {
            bool result = false;
            lock (_sync)
            {
                result = isRunning;
            }
            return result;
        }

        private void SetRunning(bool value)
        {
            lock (_sync)
            {
                isRunning = value;
            }
        }

        private void tMain(object obj)
        {
            SetRunning(true);
            while (IsRunning())
            {
                try
                {
                    TcpClient client = host.AcceptTcpClient();
                    IPEndPoint rep = client.Client.RemoteEndPoint as IPEndPoint;
                    byte[] buff = rep.Address.GetAddressBytes();
                    byte[] tmp = new byte[buff.Length];
                    for(int i  = 0; i < tmp.Length; i++)
                        tmp[i] = buff[buff.Length - i - 1];
                    if (buff.Length == 4)
                        Server.UpdateIP4Knock(BitConverter.ToUInt32(tmp, 0));
                    else
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (byte b in buff)
                            sb.Append("X2");
                        Logger.Log("Non IP4: " + sb.ToString());
                    }
                    client.Close();
                }
                catch (Exception e)
                {
                    Logger.Log("Error in tMain of portwatcher " + port + " : " + e.Message, false);
                }
            }
        }
    }

}
