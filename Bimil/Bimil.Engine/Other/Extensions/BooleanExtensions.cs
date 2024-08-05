namespace Bimil.Engine.Other.Extensions
{
    public static class BooleanExtensions
    {
        public static bool IsNullOrTrue(bool? value)
        {
            return value == null || value == true;
        }

        public static bool IsNullOrFalse(bool? value)
        {
            return value == null || value == false;
        }
    }
}