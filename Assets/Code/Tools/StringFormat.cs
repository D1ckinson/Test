namespace Assets.Code.Tools
{
    public static class StringFormat
    {
        // Числовые форматы
        public const string WholeNumber = "0";
        public const string OneDecimal = "0.0";
        public const string TwoDecimals = "0.00";
        public const string Currency = "C2"; // $12.34
        public const string Percentage = "P1"; // 12.3%

        // Дата/время
        public const string ShortDate = "d"; // 10/25/2023
        public const string LongDate = "D"; // Tuesday, October 25, 2023
        public const string TimeOnly = "t"; // 2:45 PM

        // Специальные
        public const string FileSize = "0.## MB";
        public const string Scientific = "0.##E+0";
    }
}
