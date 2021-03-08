using UnityEngine;

public class Room : MonoBehaviour
{
    public enum Door
    {
        Up,
        Right,
        Down,
        Left
    }

    public GameObject UpDoor;
    public GameObject RightDoor;
    public GameObject DownDoor;
    public GameObject LeftDoor;
    public GameObject Ladder;

    private void Awake()
    {
        Ladder.SetActive(false);
    }

    public void SetDoorState(Door door, bool closed)
    {
        switch (door)
        {
            case Door.Up:
                UpDoor.SetActive(closed);
                Ladder.SetActive(!closed);
                break;
            case Door.Right:
                RightDoor.SetActive(closed);
                break;
            case Door.Down:
                DownDoor.SetActive(closed);
                break;
            case Door.Left:
                LeftDoor.SetActive(closed);
                break;
        }
    }
}
