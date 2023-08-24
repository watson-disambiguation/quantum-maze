using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject loadConcealer;
    public static TileManager instance;
    [SerializeField]
    private int width, height;
    [SerializeField]
    private int generationRadius = 5;
    [SerializeField]
    private float viewAngle,viewLimitFront, viewLimitSide, rotationThreshold;


    [SerializeField]
    private Tile tilePrefab;

    private int playerX;
    private int playerY;
    private float playerRotation;

    public Tile[,] tiles { get; private set; }

    public int Width()
    {
        return width;
    }

    public int Height()
    {
        return height;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    private void Start()
    {
        tiles = new Tile[width, height];
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                tiles[x, y] = Instantiate<Tile>(tilePrefab, transform);
            }
        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tiles[x, y].Init(x, y);
            }
        }
        
        InitialUpdateTiles();
    }

    private void Update()
    {
        int intX = Mathf.RoundToInt(Player.player.transform.position.x / 2f);
        int intY = Mathf.RoundToInt(Player.player.transform.position.y / 2f);
        float newPlayerRotation = Player.player.getRotation();
        bool isDifferentAngle = Mathf.Abs(newPlayerRotation - playerRotation) > rotationThreshold;
        if (playerX != intX || playerY != intY || isDifferentAngle)
        {
            playerRotation = Player.player.getRotation();
            playerX = intX;
            playerY = intY;
            UpdateTiles();
        }
    }
    
    // for testing use only!!!
    private void SetTilesTesting()
    {
        tiles[playerX,playerY].SetState(Tile.State.WallEndLeft);
        tiles[playerX,playerY].Collapse();
        tiles[playerX + 1,playerY].SetState(Tile.State.WallEndRight);
        tiles[playerX + 1, playerY].Collapse();
    }

    private void InitialUpdateTiles()
    {
        playerX = Mathf.RoundToInt(Player.player.transform.position.x / 2f);
        playerY = Mathf.RoundToInt(Player.player.transform.position.y / 2f);
        playerRotation = Player.player.getRotation();
        
        UpdateTiles();
        bool successful = false;
        while (!successful)
        {
            successful = CheckTiles();
            Debug.Log(successful);
            if (successful)
            {
                break;
            }
            Debug.Log("Failed, trying again");
            ResetTiles();
            UpdateTiles();
        }
        loadConcealer.SetActive(false);
    }

    private bool CheckTiles()
    {
        float maxDistance = 0;
        HashSet<Vector2Int> total = new();
        HashSet<Vector2Int> lastAdded = new();
        Vector2Int initialPos = new(playerX, playerY);
        lastAdded.Add(initialPos);
        total.UnionWith(lastAdded);

        while (lastAdded.Count > 0)
        {
            HashSet<Vector2Int> newlyAdded = new();
            foreach (var pos in lastAdded)
            {
                Vector2Int[] neighbours = tiles[pos.x, pos.y].getNeighbours();
                foreach (var neighbour in neighbours)
                {
                    Vector2Int neighbourConstrained = Constrain(neighbour);
                    bool toAdd = false;
                    if (tiles[neighbourConstrained.x, neighbourConstrained.y].state != Tile.State.Empty)
                    {
                        toAdd = total.Add(neighbourConstrained);
                    }

                    if (toAdd)
                    {
                        newlyAdded.Add(neighbourConstrained);
                    }
                    
                }
            }
            
            lastAdded = newlyAdded;
        }

        foreach (var pos in total)
        {
            float distance = (pos - initialPos).magnitude;
            if (distance > maxDistance)
            {
                maxDistance = distance;
            }
        }

        return maxDistance > viewLimitSide;

    }

    public Vector2Int Constrain(Vector2Int pos)
    {
        if (pos.x >= width)
        {
            return new(0, pos.y);
        }
        else if (pos.x < 0 && pos.y < height && pos.y >= 0)
        {
            return new(width - 1, pos.y);
        }
        else if (pos.y >= height)
        {
            return new(pos.x, 0);
        }
        else if (pos.y < 0)
        {
            return new(pos.x, height-1);
        }
        return new(pos.x, pos.y);
    }
    
    private void ResetTiles()
    {
        int leftBound = playerX - generationRadius;
        int rightBound = playerX + generationRadius;
        int bottomBound = playerY - generationRadius;
        int topBound = playerY + generationRadius;

        float playerRotationRadians = (playerRotation + 90) * Mathf.Deg2Rad;

        for (int x = leftBound; x <= rightBound; x++)
        {
            for(int y = bottomBound; y <= topBound; y++)
            {
                int loopX = x;
                if(x >= width)
                {
                    loopX = x - width;
                } 
                else if(x < 0)
                {
                    loopX = x + width;
                }
                int loopY = y;
                if (y >= height)
                {
                    loopY = y - height ;
                }
                else if (y < 0)
                {
                    loopY = y + height;
                }

                Tile currTile = tiles[loopX, loopY];
                currTile.SetState(Tile.State.Empty);
                currTile.Decollapse();
            }
        }
    }

    private void UpdateTiles()
    {

        int leftBound = playerX - generationRadius;
        int rightBound = playerX + generationRadius;
        int bottomBound = playerY - generationRadius;
        int topBound = playerY + generationRadius;

        
        float playerRotationRadians = (playerRotation + 90) * Mathf.Deg2Rad;

        for (int x = leftBound; x <= rightBound; x++)
        {
            for(int y = bottomBound; y <= topBound; y++)
            {
                bool toCollapse = false;
                Vector2 dirToTile = new Vector2(x-playerX, y-playerY);
                float magnitudeToTile = dirToTile.magnitude;
                Vector2 lightDirection = new Vector2(Mathf.Cos(playerRotationRadians), Mathf.Sin(playerRotationRadians));
                float angleFromLight = Vector2.Angle(lightDirection, dirToTile);
                if(angleFromLight < viewAngle / 2)
                {
                    toCollapse = magnitudeToTile < viewLimitFront;
                }
                else
                {
                    toCollapse = magnitudeToTile < viewLimitSide;
                }

                int loopX = x;
                if(x >= width)
                {
                    loopX = x - width;
                } 
                else if(x < 0)
                {
                    loopX = x + width;
                }
                int loopY = y;
                if (y >= height)
                {
                    loopY = y - height ;
                }
                else if (y < 0)
                {
                    loopY = y + height;
                }

                Tile currTile = tiles[loopX, loopY];
                if (!toCollapse)
                {
                    currTile.SetState(Tile.State.Empty);
                    currTile.Decollapse();
                }
                else if (!currTile.collapsed && toCollapse)
                {
                    generateTile(currTile);
                    currTile.Collapse();
                }

            }
        }
        
    }


    public void Generate(int seedX, int seedY, Tile.State seedState = Tile.State.Empty)
    {
        Debug.Log("Generating Field");
        Queue<Tile> queue = new Queue<Tile>();
        tiles[seedX,seedY].SetState(seedState);
        queue.Enqueue(tiles[seedX, seedY]);
        while (queue.Count > 0)
        {
            Tile currentTile = queue.Dequeue();
            if (currentTile.state == Tile.State.Empty)
            {
                generateTile(currentTile);
            }
            Vector2Int[] newTiles = currentTile.getNeighbours();
            foreach (Vector2Int tilePos in newTiles)
            {
                Tile newTile;
                if (tilePos.x >= width)
                {
                    newTile = tiles[0, tilePos.y];
                }
                else if (tilePos.x < 0 && tilePos.y < height && tilePos.y >= 0)
                {
                    newTile = tiles[width-1, tilePos.y];
                }
                else if (tilePos.y >= height)
                {
                    newTile = tiles[tilePos.x, 0];
                }
                else if (tilePos.y < 0)
                {
                    newTile = tiles[tilePos.x, height-1];
                }
                else
                {
                    newTile = tiles[tilePos.x, tilePos.y];
                }

                if (newTile.state == Tile.State.Empty)
                {
                    queue.Enqueue(newTile);
                }
            }
        }

    }
    public void generateTile(Tile tile)
    {
        var possibleStates = Enum.GetValues(typeof(Tile.State)).Cast<Tile.State>().ToList();
        possibleStates.Remove(Tile.State.Empty);
        int x = tile.x;
        int y = tile.y;

        Tile upTile;
        if (y+1 < height)
        {
            upTile = tiles[x, y +1];
        }
        else
        {
            upTile = tiles[x, 0];
        }
        if (upTile.state != Tile.State.Empty)
        {
            bool isOpen = Tile.OpenDirFromState(upTile.state)[1];
            foreach (var state in new List<Tile.State>(possibleStates))
            {
                if (isOpen != Tile.OpenDirFromState(state)[0])
                {
                    possibleStates.Remove(state);
                }
            }
            
        }

        Tile downTile;
        if (y-1 >= 0)
        {
            downTile = tiles[x, y - 1];
        }
        else
        {
            downTile= tiles[x, height-1];
        }
        if (downTile.state != Tile.State.Empty)
        {
            bool isOpen = Tile.OpenDirFromState(downTile.state)[0];
            foreach (var state in new List<Tile.State>(possibleStates))
            {
                if (isOpen != Tile.OpenDirFromState(state)[1])
                {
                    possibleStates.Remove(state);
                }
            }
            
        }
        Tile rightTile;
        if (x + 1 < width)
        {
            rightTile = tiles[x +1, y];
        }
        else
        {
            rightTile= tiles[0 , y];
        }
        if (rightTile.state != Tile.State.Empty)
        {
            bool isOpen = Tile.OpenDirFromState(rightTile.state)[2];
            foreach (var state in new List<Tile.State>(possibleStates))
            {
                if (isOpen != Tile.OpenDirFromState(state)[3])
                {
                    possibleStates.Remove(state);
                }
            }
            
        }
        Tile leftTile;
        if (x - 1 >= 0)
        {
            leftTile = tiles[x -1, y];
        }
        else
        {
            leftTile = tiles[width-1 , y];
        }
        if (leftTile.state != Tile.State.Empty)
        {
            bool isOpen = Tile.OpenDirFromState(leftTile.state)[3];
            foreach (var state in new List<Tile.State>(possibleStates))
            {
                if (isOpen != Tile.OpenDirFromState(state)[2])
                {
                    possibleStates.Remove(state);
                }
            }
            
        }
        int stateIndex = UnityEngine.Random.Range(0, possibleStates.Count);
        if(possibleStates.Count > 0)
        {
            tile.SetState(possibleStates[stateIndex]);
        }
        else
        {
            tile.SetState(Tile.State.Empty);
        }
        tile.Collapse();
        
        TravellerController.instance.DecideSpawn(tile);
    }
}
