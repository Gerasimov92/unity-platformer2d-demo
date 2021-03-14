using UnityEngine;

public class RandomSelector : MonoBehaviour
{
    public int countToLeave = 1;

    private void Start()
    {
        int count = Random.Range(0, 2) * countToLeave;
        while (transform.childCount > count)
        {
            Transform childToDestroy = transform.GetChild(Random.Range(0, transform.childCount));
            DestroyImmediate(childToDestroy.gameObject);
        }
    }
}
