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
        }

        private static void FillEn()
        {
            LevelMaxed = "Max";
            Level = "Lvl";
            Play = "Play";
            Shop = "Shop";
            Upgrade = "Upgrade";
            Leaderboard = "Leaderboard";
        }

        private static void FillTr()
        {

        }
    }
}
