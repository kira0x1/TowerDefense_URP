using UnityEngine;

namespace Kira.Building
{
    [CreateAssetMenu(fileName = "New Building", menuName = "Kira/Create Building")]
    public class BuildingData : ScriptableObject
    {
        [SerializeField] private new string name;
        [SerializeField] private Sprite icon;
        [SerializeField] private BuildingCategory buildingCategory;
        [SerializeField, TextArea] private string tooltip;

        /// <summary>
        /// Must always be a multiple of 2
        /// </summary>
        [SerializeField] private int tileSize = 1;

        public string Name => name;
        public BuildingCategory Category => buildingCategory;
        public string Tooltip => tooltip;
        public Sprite Icon => icon;
        public int TileSize => tileSize;

        //TODO setup requirements
        //Resources needed
        //Buildings needed
    }
}