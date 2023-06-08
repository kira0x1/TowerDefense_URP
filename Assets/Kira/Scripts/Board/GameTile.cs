using UnityEngine;

namespace Kira.Board
{
    public class GameTile : MonoBehaviour
    {
        [SerializeField]
        private Transform arrow = default;

        private GameTile north, east, south, west, nextOnPath;
        private int distance;

        public bool IsAlternative { get; set; }

        public bool HasPath => distance != int.MaxValue;

        public GameTile GrowPathNorth() => GrowPathTo(north);
        public GameTile GrowPathEast() => GrowPathTo(east);
        public GameTile GrowPathSouth() => GrowPathTo(south);
        public GameTile GrowPathWest() => GrowPathTo(west);


        private static readonly Quaternion NorthRotation = Quaternion.Euler(90f, 0f, 0f);
        private static readonly Quaternion EastRotation = Quaternion.Euler(90f, 90f, 0f);
        private static readonly Quaternion SouthRotation = Quaternion.Euler(90f, 180f, 0f);
        private static readonly Quaternion WestRotation = Quaternion.Euler(90f, 270f, 0f);

        public void ShowPath()
        {
            if (distance == 0)
            {
                arrow.gameObject.SetActive(false);
                return;
            }

            arrow.gameObject.SetActive(true);

            arrow.localRotation =
                nextOnPath == north ? NorthRotation :
                nextOnPath == east ? EastRotation :
                nextOnPath == south ? SouthRotation : WestRotation;
        }

        private GameTile GrowPathTo(GameTile neighbour)
        {
            Debug.Assert(HasPath, "Missing Path!");

            if (neighbour == null || neighbour.HasPath)
            {
                return null;
            }

            neighbour.distance = distance + 1;
            neighbour.nextOnPath = this;
            return neighbour;
        }

        public void ClearPath()
        {
            distance = int.MaxValue;
            nextOnPath = null;
        }

        public void BecomeDestination()
        {
            distance = 0;
            nextOnPath = null;
        }


        public static void MakeEastWestNeighbours(GameTile east, GameTile west)
        {
            Debug.Assert(west.east == null && east.west == null, "Redefine neighbours!");

            west.east = east;
            east.west = west;
        }

        public static void MakeNorthSouthNeighbours(GameTile north, GameTile south)
        {
            Debug.Assert(south.north == null && north.south == null, "Redefine neighbours!");

            south.north = north;
            north.south = south;
        }
    }
}