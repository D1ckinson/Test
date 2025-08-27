using Assets.Code.Tools;
using Assets.Scripts;
using Assets.Scripts.Movement;
using UnityEngine;

namespace Assets.Code.CharactersLogic.EnemyLogic
{
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
        public CharacterType CharacterType { get; private set; } = CharacterType.Enemy;

        private void Awake()
        {
            CharacterMovement = GetComponent<CharacterMovement>();
            Health = GetComponent<Health>();
            CollisionDamage = GetComponent<CollisionDamage>();
            DeathTriger = GetComponent<DeathTriger>();
        }

        public void Initialize(DirectionTeller directionTeller)
        {
            DirectionTeller = directionTeller.ThrowIfNull();
        }

        public void SetType(CharacterType characterType)
        {
            CharacterType = characterType.ThrowIfNull();
        }
    }
}
