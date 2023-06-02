using UnityEngine;

namespace Kira.Building
{
    [CreateAssetMenu(fileName = "New Building", menuName = "Kira/Create Building")]
    public class BuildingData : ScriptableObject
    {
        [SerializeField] private new string name;
        [SerializeField] private BuildingCategory buildingCategory;

        /// <summary>
        /// Must always be a multiple of 2
        /// </summary>
        [SerializeField] private int tileSize = 1;

        public string Name => name;
        public BuildingCategory Category => buildingCategory;
        public int TileSize => tileSize;

        //TODO setup requirements
        //Resources needed
        //Buildings needed
    }
}