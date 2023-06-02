using UnityEngine;

namespace Kira
{
    public struct GameResource
    {
        public string name;
        public Sprite icon;
        public int amount;

        public GameResource(string name, Sprite icon, int amount = 0)
        {
            this.name = name;
            this.icon = icon;
            this.amount = amount;
        }
    }
}