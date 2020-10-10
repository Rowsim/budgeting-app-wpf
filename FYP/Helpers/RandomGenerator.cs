using System;
using System.IO;

namespace FYP.Helpers
{
    public static class RandomGenerator
    {
        private static readonly Random random = new Random();

        internal static string GetRandomStringFromPath()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", "");
            return path;
        }

        internal static double GetRandomDouble()
        {
            return random.NextDouble();
        }

        internal static int GetRandomIntMaxValue(int max = 9999999)
        {
            return random.Next(max);
        }

        internal static string GenerateAccountId(string name)
        {
            if (name.Length >= 4)
            {
                name = name.Remove(3);
            }
            name += GetRandomIntMaxValue(999999);
            return name;
        }

        internal static DateTime GenerateRandomDate(int rangeDays = 365)
        {
            DateTime result = DateTime.Today;
            rangeDays = GetRandomIntMaxValue(rangeDays);
            if (rangeDays % 2 == 0)
            {
                rangeDays *= -1;
            }
            result = result.AddDays(rangeDays);
            return result;
        }
    }
}