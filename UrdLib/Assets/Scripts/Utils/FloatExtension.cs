namespace Urd.Utils
{
    public static class FloatExtension
    {
        public static string ZeroOneToPercentage(this float value)
        {
            return (value * 100).ToString("#0.0");
        } 
    }
}