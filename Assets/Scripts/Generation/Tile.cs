using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    public bool collapsed
    {
        get;
        private set;
    }
    public bool occupied = false;
    public int x { get; private set; }
    public int y { get; private set; }

    private bool[] openDirs = new bool[4]; 

    public enum State
    {
        Wall2WayHorizontal = 0,
        Wall2WayVertical = 1,
        Wall3WayDown = 2,
        Wall3WayLeft = 3,
        Wall3WayRight = 4,
        Wall3WayUp = 5,
        Wall4Way = 6,
        WallEndBottom = 7,
        WallEndTop = 8,
        WallEndRight = 9,
        WallEndLeft = 10,
        WallTurnDownRight = 11,
        WallTurnUpRight = 12,
        WallTurnUpLeft = 13,
        WallTurnDownLeft = 14,
        Empty = 15
    }

    public State state { get; private set; }

    [SerializeField]
    public FloorTile[] tiles = new FloorTile[9];
    public void Init(int x, int y, State initialState = State.Empty)
    {
        collapsed = false;
        this.x = x; 
        this.y = y;
        this.transform.position = new Vector3(2*x, 0, 2*y);
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                int index = 3 * i + j;
                FloorTile currentTile = tiles[index];
                currentTile.transform.position = this.transform.position + new Vector3((i-1) * TileManager.instance.Width() * 2, 0, (j-1) * TileManager.instance.Height() * 2);
            }
        }
        SetState(initialState);
    }

    public void SetState(State newState)
    {
        if(state == newState)
        {
            return;
        }
        state = newState;
        foreach(FloorTile tile in tiles)
        {
            tile.SetState(newState);
        }
        openDirs = OpenDirFromState(state);
    }

    public Vector2Int[] getNeighbours()
    {
        Vector2Int up = new Vector2Int(x, y+1);
        Vector2Int down = new Vector2Int(x, y - 1);
        Vector2Int right = new Vector2Int(x+1, y);
        Vector2Int left = new Vector2Int(x-1, y);
        List<Vector2Int> list = new List<Vector2Int>();
        if (openDirs[0])
        {
            list.Add(up);
        }
        if (openDirs[1])
        {
            list.Add(down);
        }
        if (openDirs[2])
        {
            list.Add(left);
        }
        if (openDirs[3])
        {
            list.Add(right);
        }
        return list.ToArray();
    }

    public static bool[] OpenDirFromState(State state)
    {
        switch (state)
        {
            case State.Empty:
                return  new bool[] { false, false, false, false };
            case State.Wall2WayHorizontal:
                return new bool[] { false, false, true, true };
            case State.Wall2WayVertical:
                return new bool[] { true, true, false, false };
            case State.Wall3WayDown:
                return new bool[] { true, false, true, true };
            case State.Wall3WayLeft:
                return new bool[] { true, true, false, true };
            case State.Wall3WayRight:
                return new bool[] { true, true, true, false };
            case State.Wall3WayUp:
                return new bool[] { false, true, true, true };
            case State.Wall4Way:
                return new bool[] { true, true, true, true };
            case State.WallEndBottom:
                return new bool[] { true, false, false, false };
            case State.WallEndTop:
                return new bool[] { false, true, false, false };
            case State.WallEndLeft:
                return new bool[] { false, false, false, true };
            case State.WallEndRight:
                return new bool[] { false, false, true, false };
            case State.WallTurnDownLeft:
                return new bool[] { false, true, true, false };
            case State.WallTurnDownRight:
                return new bool[] { false, true, false, true };
            case State.WallTurnUpLeft:
                return new bool[] { true, false, true, false };
            case State.WallTurnUpRight:
                return new bool[] { true, false, false, true };
            default:
                return null;
        }
    }

    public void Collapse()
    {
        collapsed = true;
    }

    public void Decollapse()
    {
        collapsed = false;
        if (occupied)
        {
            TravellerController.instance.Despawn();
        }

        occupied = false;
    }
}
