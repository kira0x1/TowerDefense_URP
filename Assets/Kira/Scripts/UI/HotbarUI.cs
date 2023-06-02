using UnityEngine;

namespace Kira.UI
{
    public class HotbarUI : MonoBehaviour
    {
        [SerializeField]
        private PopupUI buildingsPopUp;

        public void OnBuildingsClicked()
        {
            buildingsPopUp.TogglePanel();
        }
    }
}