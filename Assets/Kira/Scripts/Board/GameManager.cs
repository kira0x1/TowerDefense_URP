using UnityEngine;

namespace Kira.Board
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private Vector2Int boardSize = new Vector2Int(11, 11);

        [SerializeField]
        private GameBoard board = default;

        [SerializeField]
        private GameTileContentFactory tileContentFactory = default;

        private void OnValidate()
        {
            if (boardSize.x < 2) boardSize.x = 2;
            if (boardSize.y < 2) boardSize.y = 2;
        }

        private void Awake()
        {
            board.Initialize(boardSize);
        }
    }
}