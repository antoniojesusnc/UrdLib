using System;

namespace Urd.Utils
{
    public static class NumbersExtension
    {
        public static int RoundToInt(this double value) => (int)Math.Round(value);
        
        public static string ZeroOneToPercentage(this float value)
        {
            return (value * 100).ToString("#0.0");
        } 
    }
}