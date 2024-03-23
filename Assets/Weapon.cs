using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage;

    public Camera cam;

    public float fireRate;//so dan ban tren 1s

    private float nextFire;

    [Header("VFX")]
    public GameObject hitVFX;

    [Header("Ammo")]
    public int mag = 5;//so bang dan
    public int ammo = 30;//so luong dan bat dau
    public int maxAmmo = 30;//so dan trong moi bang dan

    [Header("UI")]
    public TextMeshProUGUI magText;
    public TextMeshProUGUI ammoText;

    private void Update()
    {
        //
        if(nextFire > 0)
        {
            nextFire -= Time.deltaTime;
        }

        if(Input.GetButton("Fire1") && nextFire <= 0 && ammo > 0)
        {
            nextFire = 1 / fireRate;

            ammo--;

            magText.text = mag.ToString();
            ammoText.text = ammo + "/" + maxAmmo.ToString();

            Fire();
        }
        //
        //
        if (Input.GetKeyDown(KeyCode.R) && ammo < maxAmmo)
        {
            ReLoad();
        }
        //
    }

    void ReLoad()
    {
        if(mag > 0)
        {
            mag--;

            ammo = maxAmmo;
        }

        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + maxAmmo.ToString();
    }

    void Fire()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        RaycastHit hit;

        //ktra va cham voi vat gi do
        if(Physics.Raycast(ray.origin, ray.direction, out hit, 100f))
        {
            //sinh ra vfx tai noi trung dan
            PhotonNetwork.Instantiate(hitVFX.name, hit.point, Quaternion.identity);

            //ktra va cham vat co health
            if (hit.transform.gameObject.GetComponent<Health>())
            {
                //bien doi doi tuong tro choi lay thanh phan PhotonView, chay method TakeDamage()
                //RpcTarget.All: muc tieu la tat ca moi nguoi tren sever.
                hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage",RpcTarget.All, damage);
            }
        }
    }
}
