using System;
using System.Linq;

namespace promoCodeApp.Helpers
{
    public static class PromoUtil
    {
        public static string GenerateCode()
        {
            const int length = 6;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}