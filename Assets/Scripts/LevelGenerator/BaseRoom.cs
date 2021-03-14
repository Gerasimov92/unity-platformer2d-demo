using UnityEngine;

public abstract class BaseRoom : MonoBehaviour
{
    public abstract void SetDoorState(Door door, bool closed);
    public abstract bool HasDoor(Door door);
}
