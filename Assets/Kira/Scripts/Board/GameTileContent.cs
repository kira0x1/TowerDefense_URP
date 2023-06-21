using UnityEngine;

namespace Kira.Board
{
    public enum GameTileContentType
    {
        Empty,
        Destination
    }

    public class GameTileContent : MonoBehaviour
    {
        [SerializeField]
        private GameTileContentType type = default;
        public GameTileContentType Type => type;

        private GameTileContentFactory originFactory;

        public GameTileContentFactory OriginFactory
        {
            get => originFactory;
            set
            {
                Debug.Assert(originFactory == null, "Null Origin Factory!!");
                originFactory = value;
            }
        }

        public void Recycle()
        {
            originFactory.Reclaim(this);
        }
    }
}