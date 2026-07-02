using System;
using System.Security.Cryptography;
using System.Text;

namespace KnockServerWV
{
    public class LogEntry
    {
        public string text;
        public string[] parts;
        public byte[] hash;
        public DateTime time;
        public bool display;
        public LogEntry(string s, bool disp = true)
        {
            text = s;
            parts = s.Trim().Split(' ');
            time = DateTime.Now;
            hash = GetHash(s);
            display = disp;
        }

        public byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public bool Equals(LogEntry obj)
        {
            for (int i = 0; i < hash.Length; i++)
                if (hash[i] != obj.hash[i])
                    return false;
            return true;
        }
    }

}
