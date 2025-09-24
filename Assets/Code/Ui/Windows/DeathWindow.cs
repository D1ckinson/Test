using Assets.Code.Data;
using Assets.Code.Tools;
using Assets.Code.Ui.Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathWindow : BaseWindow
{
    [SerializeField] private TMP_Text _earnedText;
    [SerializeField] private TMP_Text _yourTimeText;
    [SerializeField] private TMP_Text _minutesText;
    [SerializeField] private TMP_Text _continueText;
    [SerializeField] private TMP_Text _menuText;

    [field: SerializeField] public TMP_Text CoinsQuantity { get; private set; }
    [field: SerializeField] public TMP_Text MinutesQuantity { get; private set; }
    [field: SerializeField] public Button ContinueForAddButton { get; private set; }
    [field: SerializeField] public Button BackToMenuButton { get; private set; }

    private void Awake()
    {
        ContinueForAddButton.Subscribe(Disable);
        BackToMenuButton.Subscribe(Disable);
    }

    private void OnDestroy()
    {
        ContinueForAddButton.Unsubscribe(Disable);
        BackToMenuButton.Unsubscribe(Disable);
    }

    public DeathWindow Initialize()
    {
        _earnedText.SetText(UIText.Earned);
        _yourTimeText.SetText(UIText.YourTime);
        _minutesText.SetText(UIText.Minutes);
        _continueText.SetText(UIText.Continue);
        _menuText.SetText(UIText.MenuText);

        return this;
    }
}
