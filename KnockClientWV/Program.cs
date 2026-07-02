using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace KnockClientWV
{
    internal class Program
    {
        public static Dictionary<string, string> config = new Dictionary<string, string>();
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("config.txt");
            foreach (string line in lines)
            {
                string[] parts = line.Split('=');
                if (parts.Length == 2)
                    config.Add(parts[0].ToLower().Trim(), parts[1].Trim());
            }
            int[] seq = GetCurrentKnockSequence();
            Console.Write("Knocking sequence:");
            foreach (int i in seq)
                Console.Write(" " + i);
            Console.WriteLine();
            IPAddress addr = IPAddress.Parse(config["target"]);
            for (int i = 0; i < seq.Length; i++)
            {
                try
                {
                    Console.Write("Knock on " + config["target"] + ":" + seq[i]);
                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    var result = socket.BeginConnect(addr, seq[i], null, null);
                    result.AsyncWaitHandle.WaitOne(100, true);
                    if (socket.Connected)
                    {
                        socket.EndConnect(result);
                        Console.WriteLine(" Answered");
                        Thread.Sleep(100);
                    }
                    else
                    {
                        socket.Close();
                        Console.WriteLine(" Not answered");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" Not answered\n" + ex);
                }
            }
        }



        static int[] GetCurrentKnockSequence()
        {
            List<int> result = new List<int>();
            DateTime d = DateTime.Now;
            int a = d.Day;
            int b = (int)d.DayOfWeek;
            int c = d.DayOfYear;
            int salt1 = int.Parse(config["salt1"]);
            int salt2 = int.Parse(config["salt2"]);
            int basePort = int.Parse(config["baseport"]);
            int range = int.Parse(config["range"]);
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
            return result.ToArray();
        }
    }
}
