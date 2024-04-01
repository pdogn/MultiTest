using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using TMPro;
using Photon.Pun.UtilityScripts;

public class LeaderBoard : MonoBehaviour
{
    public GameObject playersHolder;

    [Header("Options")]
    public float refreshRate = 1f;

    [Header("UI")]
    public GameObject[] slots;
    [Space]
    public TextMeshProUGUI[] scoretext;
    public TextMeshProUGUI[] nameText;
    public TextMeshProUGUI[] kdText;

    private void Start()
    {
        InvokeRepeating(nameof(Refresh), 1f, refreshRate);
    }

    public void Refresh()
    {
        foreach (var slot in slots)
        {
            slot.SetActive(false);
        }

        var sortedPlayerList = (from player in PhotonNetwork.PlayerList 
                                orderby player.GetScore()
                                descending 
                                select player).ToList();

        int i = 0;
        foreach(var player in sortedPlayerList)
        {
            slots[i].SetActive(true);

            if (player.NickName == "")
                player.NickName = "unnamed";


            nameText[i].text = player.NickName;
            scoretext[i].text = player.GetScore().ToString();

            if (player.CustomProperties["kills"] != null)
            {
                kdText[i].text = player.CustomProperties["kills"] 
                                + "/" + player.CustomProperties["deaths"];
            }
            else
            {
                kdText[i].text = "0/0";
            }

            i++;
        }
    }

    private void Update()
    {
        playersHolder.SetActive(Input.GetKey(KeyCode.Tab));
    }
}
