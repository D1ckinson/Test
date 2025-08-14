using Assets.Code.Data;
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

    public DeathWindow Initialize()
    {
        _earnedText.SetText(UIText.EarnedText);
        _yourTimeText.SetText(UIText.YourTimeText);
        _minutesText.SetText(UIText.MinutesText);
        _continueText.SetText(UIText.ContinueText);
        _menuText.SetText(UIText.MenuText);

        return this;
    }
}
