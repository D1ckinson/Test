using System;
using System.Collections.Generic;
using YG;

namespace Assets.Code.Data
{
    public static class UIText
    {
        private const string Russian = "ru";
        private const string English = "en";
        private const string Turkish = "tr";

        private static Dictionary<AbilityType, string> _abilityName;

        public static string LevelMax { get; private set; }
        public static string Level { get; private set; }
        public static string Play { get; private set; }
        public static string Shop { get; private set; }
        public static string Upgrade { get; private set; }
        public static string Leaderboard { get; private set; }
        public static string Earned { get; private set; }
        public static string YourTime { get; private set; }
        public static string Minutes { get; private set; }
        public static string Continue { get; private set; }
        public static string MenuText { get; private set; }
        public static string PersonalBest { get; private set; }

        public static IReadOnlyDictionary<AbilityType, string> AbilityName => _abilityName;

        static UIText()
        {
            switch (YG2.envir.language)
            {
                case Russian:
                    FillRu();
                    break;
                case English:
                    FillEn();
                    break;
                case Turkish:
                    FillTr();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private static void FillRu()
        {
            LevelMax = "Макс";
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
            PersonalBest = "Личный рекорд";

            _abilityName = new()
            {
                [AbilityType.SwordStrike] = "Удар мечом",
                [AbilityType.GhostSwords] = "Призрачные мечи"
            };
        }

        private static void FillEn()
        {
            LevelMax = "Max";
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
            PersonalBest = "Personal best";

            _abilityName = new()
            {
                [AbilityType.SwordStrike] = "Sword strike",
                [AbilityType.GhostSwords] = "Ghost Swords"
            };
        }

        private static void FillTr()
        {

        }
    }
}
