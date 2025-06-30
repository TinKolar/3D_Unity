using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera cutsceneCamera;
    public Camera gameplayCamera;

    public void SwitchToGameplayCamera()
    {
        cutsceneCamera.enabled = false;
        gameplayCamera.enabled = true;
    }
}
