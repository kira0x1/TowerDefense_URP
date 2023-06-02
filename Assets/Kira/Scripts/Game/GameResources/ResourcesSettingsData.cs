using UnityEngine;

namespace Kira
{
    [CreateAssetMenu(fileName = "New Resources Settings", menuName = "Kira/Create Resources Settings")]
    public class ResourcesSettingsData : ScriptableObject
    {
        public GameResourceData woodData;
        public GameResourceData foodData;

        public static ResourcesSettingsData Instance;
        private bool instanceSet;

        private void Awake()
        {
            Instance = this;
        }
    }
}