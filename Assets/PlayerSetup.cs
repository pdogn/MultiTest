using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviour
{
    public Movement movement;

    public GameObject Camera;

    public string nickName;

    public TextMeshPro nicknameText;

    public void IsLocalPlayer()
    {
        movement.enabled = true;
        Camera.SetActive(true);
    }

    [PunRPC]
    public void SetNickname(string _name)
    {
        nickName = _name;

        nicknameText.text = nickName;
    }
}
