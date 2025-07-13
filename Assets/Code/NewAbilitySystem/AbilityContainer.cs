using Assets.Scripts.Tools;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    public class AbilityContainer : MonoBehaviour
    {
        private List<Ability> _abilities;

        private void Awake()
        {
            _abilities = new();
        }

        private void Update()
        {
            _abilities.ForEach(ability => ability.Update());
        }

        public void Add(Ability ability)
        {
            _abilities.Add(ability.ThrowIfNull());
        }
    }
}
