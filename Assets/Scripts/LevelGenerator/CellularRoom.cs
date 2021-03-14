using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class CellularRoom : BaseRoom
{
    [SerializeField] private Tile tile;
    private const int Width = 10;
    private const int Height = 10;
    private readonly bool[,] _walls = new bool[Width, Height];
    private Tilemap _tilemap;

    private void Awake()
    {
        _tilemap = GetComponentInChildren<Tilemap>();
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (x == 0 || y == 0 || x == Width - 1 || y == Height - 1)
                    SetWall(x, y, true);
                else
                    SetWall(x, y, Random.Range(0, 2) == 1);
            }
        }

        UpdateTilemap();
    }

    public override void SetDoorState(Door door, bool closed)
    {
        if (!closed)
        {
            MakePath(door);
            Smooth(5);
            UpdateTilemap();
        }
    }

    public override bool HasDoor(Door door)
    {
        return true;
    }

    private void UpdateTilemap()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                _tilemap.SetTile(new Vector3Int(x - Width / 2, y - Height / 2, 0), IsWall(x, y) ? tile : null);
            }
        }
    }

    private bool IsWall(int x, int y)
    {
        if (x < 0 || y < 0 || x >= Width || y >= Height)
            return true;
        return _walls[x, y];
    }

    private void SetWall(int x, int y, bool flag)
    {
        if (x < 0 || y < 0 || x >= Width || y >= Height)
            throw new Exception("Map coordinates out of range.");

        _walls[x, y] = flag;
    }

    private int GetNeighborCount(int x, int y)
    {
        int count = 0;
        for (int dy = -1; dy <= 1; dy++)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                if (IsWall(x + dx, y + dy))
                    ++count;
            }
        }
        return count;
    }

    private void Smooth(int min)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                int count = GetNeighborCount(x, y);
                SetWall(x, y, count > min);
            }
        }
    }

    private void MakePath(Door door)
    {
        int startX;
        int startY;
        int endX;
        int endY;
        bool isVertical;

        switch (door)
        {
            case Door.Up:
                startX = Width / 2;
                startY = 1;
                endX = Width / 2;
                endY = Height - 1;
                isVertical = true;
                break;

            case Door.Right:
                startX = Width / 2;
                startY = 2;
                endX = Width - 1;
                endY = 2;
                isVertical = false;
                break;

            case Door.Down:
                startX = Width / 2;
                startY = 0;
                endX = Width / 2;
                endY = 5;
                isVertical = true;
                break;

            case Door.Left:
                startX = 0;
                startY = 2;
                endX = Width / 2;
                endY = 2;
                isVertical = false;
                break;

            default:
                return;
        }

        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                if (isVertical)
                {
                    SetWall(x - 1, y, false);
                    SetWall(x, y, false);
                    SetWall(x + 1, y, false);
                }
                else
                {
                    SetWall(x, y - 1, false);
                    SetWall(x, y, false);
                    SetWall(x, y + 1, false);
                }
            }
        }
    }
}
