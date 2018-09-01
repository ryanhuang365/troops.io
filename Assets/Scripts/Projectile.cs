using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Photon.MonoBehaviour {

    public GameObject spawnOrigin = null;

    private void OnTriggerEnter2D(Collider2D otherObject)
    {
        if (spawnOrigin.GetComponent<PhotonView>().isMine)
        {
            if (otherObject.name == "NetworkTroop")
            {
                Hit();
            }
        }
    }
    [PunRPC] void Hit() {
        Destroy(gameObject);
        if (spawnOrigin.GetComponent<PhotonView>().isMine)
        {
            GetComponent<PhotonView>().RPC("Hit", PhotonTargets.Others);
            
        }
    }
}
