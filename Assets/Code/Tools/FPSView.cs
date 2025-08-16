using TMPro;
using UnityEngine;

namespace Assets.Code.Tools
{
    public class FPSView : BaseWindow
    {
        private const float One = 1f;
        private const string WholeNumberFormat = "0";

        [SerializeField] private TMP_Text _QuantityText;

        private void Update()
        {
            string fps = (One / Time.unscaledDeltaTime).ToString(WholeNumberFormat);
            _QuantityText.SetText(fps);
        }
    }
}
