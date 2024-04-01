using Photon.Pun;
using Photon.Pun.UtilityScripts;
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

    [Header("Animaton")]
    public Animation anim;
    public AnimationClip reload;

    [Header("Recoil Setting")]//rung khi ban
    //[Range(0f, 1f)]
    //public float recoilpercent = 0.3f;
    [Range(0, 2)]
    public float recoverPercent = 0.7f;

    [Space]
    public float recoilUp = 1f;
    public float recoilBack = 0f;

    private Vector3 originalPosition;
    private Vector3 recoilVelocity = Vector3.zero;

    private float recoilLength;
    private float recoverLength;

    private bool recoiling;
    //private bool recovering;

    private void Start()
    {
        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + maxAmmo.ToString();

        originalPosition = transform.localPosition;

        recoilLength = 0;
        recoverLength = 1 / fireRate * recoverPercent;
    }

    private void Update()
    {
        //
        if(nextFire > 0)
        {
            nextFire -= Time.deltaTime;
        }

        if(Input.GetButton("Fire1") && nextFire <= 0 && ammo > 0 && anim.isPlaying == false)
        {
            nextFire = 1 / fireRate;

            ammo--;

            Fire();
        }
        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + maxAmmo.ToString();
        //
        //
        if (Input.GetKeyDown(KeyCode.R) && ammo < maxAmmo && mag > 0)
        {
            ReLoad();
        }
        //

        if (recoiling)
        {
            Recoil();
        }

        //if (recovering)
        //{
        //    Recovering();
        //}
    }

    void ReLoad()
    {
        anim.Play(reload.name);

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
        recoiling = true;
        //recovering = false;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        RaycastHit hit;

        //PhotonNetwork.LocalPlayer.AddScore(1);

        //ktra va cham voi vat gi do
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f))
        {
            PhotonNetwork.LocalPlayer.AddScore(1);//them score
            //sinh ra vfx tai noi trung dan
            PhotonNetwork.Instantiate(hitVFX.name, hit.point, Quaternion.identity);

            //ktra va cham vat co health
            if (hit.transform.gameObject.GetComponent<Health>())
            {
                //add score
                PhotonNetwork.LocalPlayer.AddScore(damage);
                if(damage >= hit.transform.gameObject.GetComponent<Health>().health)
                {
                    //kill
                    RoomManager.instance.kills++;
                    RoomManager.instance.SetHashes();
                    PhotonNetwork.LocalPlayer.AddScore(100);
                }

                //bien doi doi tuong tro choi lay thanh phan PhotonView, chay method TakeDamage()
                //RpcTarget.All: muc tieu la tat ca moi nguoi tren sever.
                hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage",RpcTarget.All, damage);
            }
        }
    }

    void Recoil()
    {
        Vector3 finalPosition = new Vector3(originalPosition.x ,originalPosition.y + recoilUp ,originalPosition.z - recoilBack);

        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosition, ref recoilVelocity, recoilLength);

        if(transform.localPosition == finalPosition)
        {
            recoiling = false;
            //recovering = false;
        }
    }
    
    //void Recovering()
    //{
    //    Vector3 finalPosition = originalPosition;

    //    transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosition, ref recoilVelocity, recoverLength);

    //    if(transform.localPosition == finalPosition)
    //    {
    //        recoiling = false;
    //        recovering = false;
    //    }
    //}
}
