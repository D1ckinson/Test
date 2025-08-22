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
        public static string Earned { get; internal set; }
        public static string YourTime { get; internal set; }
        public static string Minutes { get; internal set; }
        public static string Continue { get; internal set; }
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
            Earned = "Заработано";
            YourTime = "Ваше время";
            Minutes = "Минут";
            Continue = "Продолжить";
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
            Earned = "Earned";
            YourTime = "Your time";
            Minutes = "Minutes";
            Continue = "Continue";
            MenuText = "Menu";
        }

        private static void FillTr()
        {

        }
    }
}
