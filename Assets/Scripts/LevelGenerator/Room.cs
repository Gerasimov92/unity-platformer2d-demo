using UnityEngine;

public class Room : BaseRoom
{
    [SerializeField] private GameObject upDoor;
    [SerializeField] private GameObject rightDoor;
    [SerializeField] private GameObject downDoor;
    [SerializeField] private GameObject leftDoor;
    [SerializeField] private GameObject ladder;

    private void Awake()
    {
        ladder.SetActive(false);
    }

    public override void SetDoorState(Door door, bool closed)
    {
        if (!HasDoor(door))
            return;

        switch (door)
        {
            case Door.Up:
                upDoor.SetActive(closed);
                ladder.SetActive(!closed);
                break;
            case Door.Right:
                rightDoor.SetActive(closed);
                break;
            case Door.Down:
                downDoor.SetActive(closed);
                break;
            case Door.Left:
                leftDoor.SetActive(closed);
                break;
        }
    }

    public override bool HasDoor(Door door)
    {
        return door switch
        {
            Door.Up => upDoor != null,
            Door.Right => rightDoor != null,
            Door.Down => downDoor != null,
            Door.Left => leftDoor != null,
            _ => false
        };
    }
}
