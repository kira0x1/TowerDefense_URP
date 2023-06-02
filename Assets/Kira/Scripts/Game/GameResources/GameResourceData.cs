using UnityEngine;

namespace Kira
{
    [CreateAssetMenu(fileName = "New Resource", menuName = "Kira/Create Resource")]
    public class GameResourceData : ScriptableObject
    {
        [SerializeField] private new string name;
        [SerializeField] private Sprite icon;

        public GameResource CreateResource(int amount = 0)
        {
            return new GameResource(name, icon, amount);
        }
    }
}