using UnityEngine.Networking;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public int maxCantPlayers;
    public int cantPlayers;

    void Awake()
    {
        if (instance == null)
            instance = this;
        
        else if (instance != this)
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);

        cantPlayers = maxCantPlayers;
    }
}
