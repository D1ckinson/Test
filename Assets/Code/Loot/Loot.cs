using Assets.Code.Data.Interfaces;
using Assets.Scripts.Factories;
using Assets.Scripts.Tools;
using UnityEngine;

namespace Assets.Scripts
{
    public class Loot : MonoBehaviour
    {
        [SerializeField][Min(1)] private int _value = 1;

        [field: SerializeField] public LootType LootType { get; private set; }

        private IValueContainer _valueContainer;

        public void Initialize(IValueContainer valueContainer)
        {
            _valueContainer = valueContainer.ThrowIfNull();
        }

        public void Collect()
        {
            _valueContainer.Add(_value);
            gameObject.SetActive(false);
        }
    }
}
