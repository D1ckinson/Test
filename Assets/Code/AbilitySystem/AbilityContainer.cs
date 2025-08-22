using Assets.Code.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Code.AbilitySystem
{
    public class AbilityContainer : MonoBehaviour
    {
        private Dictionary<AbilityType, Ability> _abilities;

        public IEnumerable<AbilityType> MaxedAbilities => _abilities.Values.Where(ability => ability.IsMaxed).Select(ability => ability.Type);

        private void Awake()
        {
            _abilities = new();
        }

        public void Run()
        {
            _abilities.ForEachValues(_ability => _ability.Run());
        }

        public void Stop()
        {
            _abilities.ForEachValues(_abilities => _abilities.Stop());
        }

        public void Add(Ability ability)
        {
            ability.ThrowIfNull();
            _abilities.ContainsKey(ability.Type).ThrowIfTrue(new ArgumentException());

            _abilities.Add(ability.Type, ability);
        }

        public void RemoveAll()
        {
            _abilities.Clear();
        }

        public void Upgrade(AbilityType abilityType)
        {
            _abilities.GetValueOrThrow(abilityType.ThrowIfNull()).LevelUp();
        }

        public bool HasAbility(AbilityType abilityType)
        {
            return _abilities.ContainsKey(abilityType.ThrowIfNull());
        }

        public int GetAbilityLevel(AbilityType abilityType)
        {
            _abilities.TryGetValue(abilityType.ThrowIfNull(), out Ability ability);

            return ability.IsNull() ? Constants.Zero : ability.Level;
        }
    }
}
