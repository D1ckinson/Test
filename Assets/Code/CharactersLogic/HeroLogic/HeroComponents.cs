using Assets.Scripts;
using Assets.Scripts.Movement;
using UnityEngine;

namespace Assets.Code.CharactersLogic.HeroLogic
{
    [RequireComponent(typeof(CharacterMovement))]
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(LootCollector))]
    [RequireComponent(typeof(AbilityContainer))]
    public class HeroComponents : MonoBehaviour
    {
        public CharacterMovement CharacterMovement { get; private set; }
        public Health Health { get; private set; }
        public LootCollector LootCollector { get; private set; }
        public AbilityContainer AbilityContainer { get; private set; }

        private void Awake()
        {
            CharacterMovement = GetComponent<CharacterMovement>();
            Health = GetComponent<Health>();
            LootCollector = GetComponent<LootCollector>();
            AbilityContainer = GetComponent<AbilityContainer>();
        }
    }
}
