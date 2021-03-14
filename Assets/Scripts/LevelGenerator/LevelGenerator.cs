using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public int floorCount;
    public int floorLength;
    public int totalRoomCount;

    public BaseRoom[] roomPrefabs;
    public BaseRoom startingRoom;

    private BaseRoom[,] _spawnedRooms;

    private void Start()
    {
        _spawnedRooms = new BaseRoom[floorLength, floorCount];
        _spawnedRooms[0, 0] = startingRoom;

        for (int i = 0; i < totalRoomCount; i++)
        {
            SpawnRoom();
        }
    }

    private void SpawnRoom()
    {
        HashSet<Vector2Int> vacantPlaces = new HashSet<Vector2Int>();
        for (int x = 0; x < _spawnedRooms.GetLength(0); x++)
        {
            for (int y = 0; y < _spawnedRooms.GetLength(1); y++)
            {
                if (_spawnedRooms[x, y] == null) continue;

                int maxX = _spawnedRooms.GetLength(0) - 1;
                int maxY = _spawnedRooms.GetLength(1) - 1;

                if (x > 0 && _spawnedRooms[x - 1, y] == null) vacantPlaces.Add(new Vector2Int(x - 1, y));
                if (y > 0 && _spawnedRooms[x, y - 1] == null) vacantPlaces.Add(new Vector2Int(x, y - 1));
                if (x < maxX && _spawnedRooms[x + 1, y] == null) vacantPlaces.Add(new Vector2Int(x + 1, y));
                if (y < maxY && _spawnedRooms[x, y + 1] == null) vacantPlaces.Add(new Vector2Int(x, y + 1));
            }
        }

        BaseRoom newRoom = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)]);

        int limit = 500;
        while (limit-- > 0)
        {
            Vector2Int position = vacantPlaces.ElementAt(Random.Range(0, vacantPlaces.Count));

            if (ConnectToSomething(newRoom, position))
            {
                newRoom.transform.position = new Vector3(position.x * 10, position.y * 10, 0);
                _spawnedRooms[position.x, position.y] = newRoom;
                return;
            }
        }

        Destroy(newRoom.gameObject);
    }

    private bool ConnectToSomething(BaseRoom room, Vector2Int p)
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();

        BaseRoom neighbourRoom = GetRoomAtPosition(p.x, p.y + 1);
        if (room.HasDoor(Door.Up) && neighbourRoom != null && neighbourRoom.HasDoor(Door.Down))
            neighbours.Add(Vector2Int.up);

        neighbourRoom = GetRoomAtPosition(p.x, p.y - 1);
        if (room.HasDoor(Door.Down) && neighbourRoom != null && neighbourRoom.HasDoor(Door.Up))
            neighbours.Add(Vector2Int.down);

        neighbourRoom = GetRoomAtPosition(p.x + 1, p.y);
        if (room.HasDoor(Door.Right) && neighbourRoom != null && neighbourRoom.HasDoor(Door.Left))
            neighbours.Add(Vector2Int.right);

        neighbourRoom = GetRoomAtPosition(p.x - 1, p.y);
        if (room.HasDoor(Door.Left) && neighbourRoom != null && neighbourRoom.HasDoor(Door.Right))
            neighbours.Add(Vector2Int.left);

        if (neighbours.Count == 0) return false;

        Vector2Int selectedDirection = neighbours[Random.Range(0, neighbours.Count)];
        BaseRoom selectedRoom = GetRoomAtPosition(p.x + selectedDirection.x, p.y + selectedDirection.y);

        if(selectedDirection == Vector2Int.up)
        {
            room.SetDoorState(Door.Up, false);
            selectedRoom.SetDoorState(Door.Down,false);
        }
        else if (selectedDirection == Vector2Int.down)
        {
            room.SetDoorState(Door.Down, false);
            selectedRoom.SetDoorState(Door.Up,false);
        }
        else if (selectedDirection == Vector2Int.right)
        {
            room.SetDoorState(Door.Right, false);
            selectedRoom.SetDoorState(Door.Left,false);
        }
        else if (selectedDirection == Vector2Int.left)
        {
            room.SetDoorState(Door.Left, false);
            selectedRoom.SetDoorState(Door.Right,false);
        }

        return true;
    }

    private BaseRoom GetRoomAtPosition(int x, int y)
    {
        if (x < 0 || y < 0 || x >= _spawnedRooms.GetLength(0) || y >= _spawnedRooms.GetLength(1))
            return null;
        return _spawnedRooms[x, y];
    }
}
