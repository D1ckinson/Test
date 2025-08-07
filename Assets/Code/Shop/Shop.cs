using Assets.Code.AbilitySystem.Ui;
using Assets.Scripts;
using Assets.Scripts.Tools;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Code.Shop
{
    public class Shop
    {
        private readonly PlayerData _playerData;
        private readonly ShopConfig _shopConfig;
        private readonly Canvas _canvas;

        public Shop(PlayerData playerData, ShopConfig shopConfig, Canvas canvas, TextButton buttonPrefab)
        {
            _playerData = playerData.ThrowIfNull();
            _shopConfig = shopConfig.ThrowIfNull();

            _canvas = Object.Instantiate(canvas);
        }

        public void Show()
        {

        }

        public void Hide()
        {

        }
    }

    public class ShopItemWindow : MonoBehaviour
    {

    }

    public class BuyButton : TextButton
    {
        [SerializeField] private TMP_Text _level;

        public void SetDescription(int level, int cost)
        {
            _level.text = level.ThrowIfNegative().ToString();
            SetText(cost.ThrowIfNegative().ToString());
        }
    }

    [CreateAssetMenu(menuName = "Game/ShopConfig")]
    public class ShopConfig : ScriptableObject
    {
        public Sprite Sprite;
        public List<ShopItem> ItemData;
    }

    [Serializable]
    public struct ShopItem
    {
        public Sprite Icon;
        public string Name;
        public string Description;
        public AbilityType AbilityType;
        public int[] Cost;
    }
}
