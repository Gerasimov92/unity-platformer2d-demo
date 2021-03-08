using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private CanvasGroup gameOverDialog;

    void Start()
    {
        SetCanvasGroupEnabled(gameOverDialog, false);
    }

    public void ShowGameOverDialog()
    {
        SetCanvasGroupEnabled(gameOverDialog, true);
    }

    private void SetCanvasGroupEnabled(CanvasGroup group, bool enabled)
    {
        group.alpha = (enabled ? 1.0f : 0.0f);
        group.interactable = enabled;
        group.blocksRaycasts = enabled;
    }
}
