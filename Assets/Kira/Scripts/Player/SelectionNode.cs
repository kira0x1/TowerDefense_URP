using UnityEngine;

namespace Kira
{
    public struct SelectionNode
    {
        public int x;
        public int y;

        public Vector3 position;

        public SelectionNode(Vector3 position, int x, int y)
        {
            this.position = position;
            this.x = x;
            this.y = y;
        }
    }
}