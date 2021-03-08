using UnityEngine;

public class LevelBorderChecker : MonoBehaviour
{
    [SerializeField] private UIController uiController;
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            uiController.ShowGameOverDialog();
        }
    }
}
