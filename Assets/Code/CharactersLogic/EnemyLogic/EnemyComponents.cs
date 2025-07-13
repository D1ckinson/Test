using Assets.Code.CharactersLogic;
using Assets.Scripts.Movement;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(DirectionTeller))]
    [RequireComponent(typeof(CharacterMovement))]
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(CollisionDamage))]
    [RequireComponent(typeof(DeathTriger))]
    public class EnemyComponents : MonoBehaviour
    {
        public DirectionTeller DirectionTeller { get; private set; }
        public CharacterMovement CharacterMovement { get; private set; }
        public Health Health { get; private set; }
        public CollisionDamage CollisionDamage { get; private set; }
        public DeathTriger DeathTriger { get; private set; }

        private void Awake()
        {
            DirectionTeller = GetComponent<DirectionTeller>();
            CharacterMovement = GetComponent<CharacterMovement>();
            Health = GetComponent<Health>();
            CollisionDamage = GetComponent<CollisionDamage>();
            DeathTriger = GetComponent<DeathTriger>();
        }
    }
}
