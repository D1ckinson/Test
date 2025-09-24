using Assets.Code.Tools;
using UnityEngine;

namespace Assets.Code.Ui.Windows
{
    public class BaseWindow : MonoBehaviour
    {
        protected void Disable()
        {
            this.SetActive(false);
        }
    }
}
