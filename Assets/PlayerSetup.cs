using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    public Movement movement;

    public GameObject Camera;

    public void IsLocalPlayer()
    {
        movement.enabled = true;
        Camera.SetActive(true);
    }
}
