using System;
using YG;

namespace Assets.Code.Data
{
    public static class UIText
    {
        public static string LevelMaxed { get; private set; }
        public static string Level { get; private set; }
        public static string Play { get; private set; }
        public static string Shop { get; private set; }
        public static string Upgrade { get; private set; }
        public static string Leaderboard { get; private set; }
        public static string EarnedText { get; internal set; }
        public static string YourTimeText { get; internal set; }
        public static string MinutesText { get; internal set; }
        public static string ContinueText { get; internal set; }
        public static string MenuText { get; internal set; }

        static UIText()
        {
            switch (YG2.envir.language)
            {
                case "ru":
                    FillRu();
                    break;
                case "en":
                    FillEn();
                    break;
                case "tr":
                    FillTr();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private static void FillRu()
        {
            LevelMaxed = "Макс";
            Level = "Ур";
            Play = "Играть";
            Shop = "Магазин";
            Upgrade = "Улучшить";
            Leaderboard = "Таблица лидеров";
            EarnedText="";
            YourTimeText="Ваше время";
            MinutesText="Минут";
            ContinueText="Продолжить";
            MenuText = "Меню";
        }

        private static void FillEn()
        {
            LevelMaxed = "Max";
            Level = "Lvl";
            Play = "Play";
            Shop = "Shop";
            Upgrade = "Upgrade";
            Leaderboard = "Leaderboard";
            EarnedText = "Earned";
            YourTimeText = "Your time";
            MinutesText = "Minutes";
            ContinueText = "Continue";
            MenuText = "Menu";
        }

        private static void FillTr()
        {

        }
    }
}
