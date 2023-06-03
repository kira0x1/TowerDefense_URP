using System.Collections.Generic;
using UnityEngine;

namespace Kira.Building
{
    public class BuildingSettings : MonoBehaviour
    {
        [SerializeField]
        private List<BuildingData> buildings = new List<BuildingData>();

        public List<BuildingData> GetBuildings()
        {
            return buildings;
        }
    }
}