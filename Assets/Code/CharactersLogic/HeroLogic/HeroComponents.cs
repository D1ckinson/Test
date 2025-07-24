using Assets.Code.AmplificationSystem;
using Assets.Scripts;
using Assets.Scripts.Movement;
using Assets.Scripts.Tools;
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
        public BuffContainer BuffContainer { get; private set; }//

        private void Awake()
        {
            CharacterMovement = GetComponent<CharacterMovement>();
            Health = GetComponent<Health>();
            LootCollector = GetComponent<LootCollector>();
            AbilityContainer = GetComponent<AbilityContainer>();
            BuffContainer = new();
        }
    }
}
