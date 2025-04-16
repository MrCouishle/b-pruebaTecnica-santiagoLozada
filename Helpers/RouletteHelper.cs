namespace Helpers
{
    public static class RouletteHelper
    {
        public static ColorEnums GetColor(int number)
        {
            if (number == 0)
            {
                return ColorEnums.Green;
            }

            int[] reds = [1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36];
            return reds.Contains(number) ? ColorEnums.Red : ColorEnums.Black;
        }
    }
}
