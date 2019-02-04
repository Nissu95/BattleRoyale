using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetCantPlayers : MonoBehaviour
{
    private void Update()
    {
        GameManager.instance.cantPlayers = GameManager.instance.maxCantPlayers;
    }
}
