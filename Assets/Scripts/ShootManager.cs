using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootManager : Photon.MonoBehaviour {


    
    [PunRPC]
    void FireBullet(Vector2 startPos, float angle, PhotonMessageInfo pmi)
    {
        GameObject bullet = (GameObject)Instantiate(Resources.Load("Projectile"), startPos, new Quaternion(0, 0, angle, 0));
        Vector2 fireDirection = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad) * pmi.photonView.gameObject.GetComponentInParent<NetworkPlayer>().bulletSpeed, Mathf.Cos(angle * Mathf.Deg2Rad) * pmi.photonView.gameObject.GetComponentInParent<NetworkPlayer>().bulletSpeed);
        bullet.GetComponent<Rigidbody2D>().velocity = fireDirection;


        bullet.GetComponent<Projectile>().spawnOrigin = pmi.photonView.gameObject.GetComponentInParent<TroopMovement>().gameObject;
        if (pmi.photonView.isMine)
        {
            bullet.tag = "MyBullet";
        }
        else
        {
            bullet.tag = "NetworkBullet";
        }
    }
}
