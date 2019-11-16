using System;
using System.Security.Cryptography;
using System.Text;

namespace Scrooge.Exchange.Connectors
{
    public static class Util
    {
        public static string GetHexString(byte[] bytes)
        {
            var builder = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes)
            {
                builder.Append($"{b:x2}");
            }
            return builder.ToString();
        }

        public static byte[] Sign(string secret, string content)
        {
            var signedBytes = new HMACSHA256(Encoding.UTF8.GetBytes(secret))
                .ComputeHash(Encoding.UTF8.GetBytes(content));
            return signedBytes;
        }

        private static readonly long EpocTicks = new DateTime(1970, 1, 1).Ticks;
        public static long GetCurrentMilliseconds()
        {
            var unixTime = (DateTime.UtcNow.AddSeconds(-1).Ticks - EpocTicks) / TimeSpan.TicksPerMillisecond;
            return unixTime;
        }
    }
}
