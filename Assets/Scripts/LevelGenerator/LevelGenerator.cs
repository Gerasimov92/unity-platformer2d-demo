using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public int floorCount;
    public int floorLength;
    public int totalRoomCount;

    public Room[] roomPrefabs;
    public Room startingRoom;

    private Room[,] _spawnedRooms;

    private void Start()
    {
        _spawnedRooms = new Room[floorLength, floorCount];
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

        Room newRoom = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)]);

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

    private bool ConnectToSomething(Room room, Vector2Int p)
    {
        int maxX = _spawnedRooms.GetLength(0) - 1;
        int maxY = _spawnedRooms.GetLength(1) - 1;

        List<Vector2Int> neighbours = new List<Vector2Int>();

        if (room.UpDoor != null && p.y < maxY && _spawnedRooms[p.x, p.y + 1]?.DownDoor != null) neighbours.Add(Vector2Int.up);
        if (room.DownDoor != null && p.y > 0 && _spawnedRooms[p.x, p.y - 1]?.UpDoor != null) neighbours.Add(Vector2Int.down);
        if (room.RightDoor != null && p.x < maxX && _spawnedRooms[p.x + 1, p.y]?.LeftDoor != null) neighbours.Add(Vector2Int.right);
        if (room.LeftDoor != null && p.x > 0 && _spawnedRooms[p.x - 1, p.y]?.RightDoor != null) neighbours.Add(Vector2Int.left);

        if (neighbours.Count == 0) return false;

        Vector2Int selectedDirection = neighbours[Random.Range(0, neighbours.Count)];
        Room selectedRoom = _spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y];

        if(selectedDirection == Vector2Int.up)
        {
            room.SetDoorState(Room.Door.Up, false);
            selectedRoom.SetDoorState(Room.Door.Down,false);
        }
        else if (selectedDirection == Vector2Int.down)
        {
            room.SetDoorState(Room.Door.Down, false);
            selectedRoom.SetDoorState(Room.Door.Up,false);
        }
        else if (selectedDirection == Vector2Int.right)
        {
            room.SetDoorState(Room.Door.Right, false);
            selectedRoom.SetDoorState(Room.Door.Left,false);
        }
        else if (selectedDirection == Vector2Int.left)
        {
            room.SetDoorState(Room.Door.Left, false);
            selectedRoom.SetDoorState(Room.Door.Right,false);
        }

        return true;
    }
}
