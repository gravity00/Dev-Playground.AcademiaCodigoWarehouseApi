namespace System
{
    public static class StringExtensions
    {
        public static bool EqualsOrdinalIgnoreCase(this string value, string other){
            if(value == null){
                return other == null;
            }

            return value.Equals(other, StringComparison.OrdinalIgnoreCase);
        }
    }
}