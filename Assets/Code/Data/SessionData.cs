using Assets.Code.CharactersLogic.HeroLogic;

namespace Assets.Scripts
{
    public class SessionData
    {
        public SessionData(HeroLevel level, HeroComponents heroComponents)
        {
            HeroLevel = level;
            HeroComponents = heroComponents;
        }

        public HeroLevel HeroLevel { get; }
        public HeroComponents HeroComponents { get; }
    }
}
