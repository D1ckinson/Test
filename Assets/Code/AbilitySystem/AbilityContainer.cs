using Assets.Scripts.Tools;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    public class AbilityContainer : MonoBehaviour
    {
        private Dictionary<AbilityType, Ability> _abilities;

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
            ability.ThrowIfNull();
            _abilities.Add(ability.Type, ability);
        }

        public List<AbilityType> GetMaxedAbilities()
        {
            List<AbilityType> abilityTypes = new();

            foreach (Ability ability in _abilities.Values)
            {
                if (ability.IsMaxed)
                {
                    abilityTypes.Add(ability.Type);
                }
            }

            return abilityTypes;
        }

        public void Upgrade(AbilityType abilityType)
        {
            _abilities[abilityType.ThrowIfNull()].LevelUp();
        }

        public bool HasAbility(AbilityType abilityType)
        {
            return _abilities.ContainsKey(abilityType.ThrowIfNull());
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
