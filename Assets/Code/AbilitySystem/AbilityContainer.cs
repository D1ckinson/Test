using Assets.Code.AbilitySystem;
using Assets.Code.Tools;
using Assets.Scripts.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Code
{
    public class AbilityContainer : MonoBehaviour
    {
        private Dictionary<AbilityType, Ability> _abilities;

        public IEnumerable<AbilityType> MaxedAbilities => _abilities.Values.Where(ability => ability.IsMaxed).Select(ability => ability.Type);

        private void Awake()
        {
            _abilities = new();
        }

        private void Update()
        {
            foreach (Ability ability in _abilities.Values)
            {
                ability.Update();
            }
        }

        public void Add(Ability ability)
        {
            _abilities.Add(ability.ThrowIfNull().Type, ability);
        }

        public void Upgrade(AbilityType abilityType)
        {
            _abilities.GetValueOrThrow(abilityType).LevelUp();
        }

        public bool HasUpgrade(Enum type)
        {
            return _abilities.ContainsKey((AbilityType)type.ThrowIfNull());
        }

        public int GetAbilityLevel(AbilityType abilityType)
        {
            return _abilities[abilityType.ThrowIfNull()].Level;
        }

        public bool IsMaxed(AbilityType abilityType)
        {
            return _abilities[abilityType].IsMaxed;
        }
    }
}
