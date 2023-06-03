using UnityEngine;

namespace Kira.UI
{
    public class HotbarUI : MonoBehaviour
    {
        [SerializeField]
        private BuildMenuUI buildingsPopUp;

        public void OnBuildingsClicked()
        {
            buildingsPopUp.TogglePanel();
        }
    }
}