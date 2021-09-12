using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerNameInput : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        string playerName = this.GetComponent<TextMeshProUGUI>().text;

        if (playerName != null)
        {
            GameManager.instance.playerNameText = playerName;
        }
    }
}
