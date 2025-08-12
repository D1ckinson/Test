using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.State_Machine
{
    public class TextButton : MonoBehaviour
    {
        [field: SerializeField] public Button Button { get; private set; }
        [field: SerializeField] public TMP_Text Text { get; private set; }
    }
}
