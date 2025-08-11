﻿using Assets.Code.AbilitySystem.Ui;
using Assets.Code.Tools;
using Assets.Scripts.Tools;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Ui.LevelUp
{
    public class LevelUpButton : UiButton
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _stats;

        public void SetText(string text)
        {
            _stats.text = text;
        }

        public void SetDescription(string name, Sprite image, List<string> stats)
        {
            _name.text = name.ThrowIfNullOrEmpty();
            _image.sprite = image.ThrowIfNull();
            _stats.text = stats.ThrowIfNullOrEmpty().ToWrapText();
        }
    }
}
