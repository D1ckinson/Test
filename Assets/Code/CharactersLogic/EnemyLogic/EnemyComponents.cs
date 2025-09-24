using Assets.Code.CharactersLogic.Movement.Direction_Sources.Interfaces;
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
    [RequireComponent(typeof(Animator))]
    public class EnemyComponents : MonoBehaviour
    {
        public ITargetDirectionTeller DirectionTeller { get; private set; }
        public CharacterMovement CharacterMovement { get; private set; }
        public Health Health { get; private set; }
        public CollisionDamage CollisionDamage { get; private set; }
        public DeathTriger DeathTriger { get; private set; }
        public CharacterType CharacterType { get; private set; } = CharacterType.Enemy1;


        private void Awake()
        {
            CharacterMovement = GetComponent<CharacterMovement>();
            Health = GetComponent<Health>();
            CollisionDamage = GetComponent<CollisionDamage>();
            DeathTriger = GetComponent<DeathTriger>();
        }

        public void Initialize(ITargetDirectionTeller directionTeller)
        {
            DirectionTeller = directionTeller.ThrowIfNull();
        }

        public void SetType(CharacterType characterType)
        {
            CharacterType = characterType.ThrowIfNull();
        }
    }
}
