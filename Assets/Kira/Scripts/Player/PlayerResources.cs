using UnityEngine;

namespace Kira
{
    public class PlayerResources : MonoBehaviour
    {
        private GameResource woodResource;
        private GameResource foodResource;

        private void Awa()
        {
            woodResource = ResourcesSettingsData.Instance.woodData.CreateResource(100);
            foodResource = ResourcesSettingsData.Instance.woodData.CreateResource(50);
            Debug.Log(woodResource.amount);
        }
    }
}