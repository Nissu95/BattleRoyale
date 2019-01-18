using UnityEngine.UI;
using UnityEngine;

public class CantPlayersTxt : MonoBehaviour
{
    [SerializeField] Text cantPlayersTxt;

    private void Update()
    {
        cantPlayersTxt.text = "Players: " + GameManager.instance.cantPlayers;
    }

}
