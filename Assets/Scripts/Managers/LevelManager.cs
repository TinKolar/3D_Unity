using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.SearchService;
#endif

public class LevelManager : MonoBehaviour
{
    Door d;
    bool doorOpened = false;
    Transform victoryPosition;
    VFXManager vFXManager;
    GameManager gameManager;
    HUD hud;
    int winObjects = 1;
    bool win = false;

    [SerializeField] public int levelToUnlock;

    private void Start()
    {

        GameObject door = GameObject.FindGameObjectWithTag("Door");
        if (door != null)
        {
            d = door.GetComponent<Door>();
            if (d == null)
                Debug.LogWarning("Door GameObject found, but no 'Door' component attached.");
        }
        else
        {
            Debug.LogWarning("No GameObject with tag 'Door' found.");
        }

        GameObject obj = GameObject.FindGameObjectWithTag("winObject");
        if (obj != null)
        {
            victoryPosition = obj.GetComponent<winObject>().transform; 
            if (victoryPosition == null)
                Debug.LogWarning("GameObject with tag 'winObject' found, but no 'winObject' component attached.");
        }
        else
        {
            Debug.LogWarning("No GameObject with tag 'winObject' found.");
        }

        vFXManager = VFXManager.instance;
        if (vFXManager == null)
        {
            Debug.LogWarning("VFXManager.instance is null.");
        }

        GameObject hudObj = GameObject.FindGameObjectWithTag("HUD");
        if (hudObj != null)
        {
            hud = hudObj.GetComponent<HUD>();
            if (hud == null)
                Debug.LogWarning("GameObject with tag 'HUD' found, but no 'HUD' component attached.");
        }
        else
        {
            Debug.LogWarning("No GameObject with tag 'HUD' found.");
        }

        winObjects = GameObject.FindGameObjectsWithTag("winObject").Length;
        gameManager = GameManager.Instance;

    }


    private void Update()
    {
        if (!doorOpened && AreAllEnemiesDead())
        {
            d.OpenDoor();
            doorOpened = true;
        }
        winObjects = GameObject.FindGameObjectsWithTag("winObject").Length;

        if (winObjects == 0 && win == false)
        {
            StartCoroutine(HandleVictory());
        }
    }

    private bool AreAllEnemiesDead()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("KeyEnemy");
        return enemies.Length == 0;
    }

    IEnumerator HandleVictory()
    {
        win = true;

        vFXManager.PlaySound(vFXManager.victory, Camera.main.transform);
        yield return new WaitForSeconds(vFXManager.victory.length);

        if (levelToUnlock == 2)
        {
            gameManager.levelTwoUnlocked = true;
        }
        else if (levelToUnlock == 3)
        {
            gameManager.levelThreeUnlocked = true;
        }

        if (hud != null)
            hud.OnVictory();

        yield return null;
    }

}
