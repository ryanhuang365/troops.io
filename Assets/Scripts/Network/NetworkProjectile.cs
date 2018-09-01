using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkProjectile : MonoBehaviour {








    private Vector3 position;
    private float rotation;
    public GameObject spawnOrigin = null;

    float syncTime;
    float syncDelay;
    float lastSynchronizationTime;
    Vector3 syncEndPosition;
    Vector3 syncStartPosition;
    private Vector2 fireDirection;
    private float angle;
    private float bulletSpeed;

    private float timer = 0;
    private float bulletLifeTime = 3f;

 


    // Use this for initialization
    void Start()
    {
        
        if (tag == "MyBullet")
        {
            gameObject.name = "MyBullet";
            angle = spawnOrigin.GetComponent<NetworkPlayer>().angle;
            bulletSpeed = spawnOrigin.GetComponent<NetworkPlayer>().bulletSpeed;
            fireDirection = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad) * bulletSpeed, Mathf.Cos(angle * Mathf.Deg2Rad) * bulletSpeed); 
            GetComponent<Rigidbody2D>().velocity = fireDirection;
        }
        else
        {
            
            gameObject.name = "NetworkBullet";
            tag = "NetworkBullet";
        }
    }

    private void OnTriggerEnter2D(Collider2D otherObject)
    {

        string name = otherObject.name;
        if (name == "NetworkTroop") {
            PhotonNetwork.Destroy(gameObject);
            spawnOrigin.GetComponent<TroopMovement>().spawnATroop();
            spawnOrigin.GetComponent<TroopMovement>().level += 1;
            
            
        }
    }

    private void Update()
    {
        if (tag == "MyBullet")
        {
            timer += Time.deltaTime;
            if (timer > bulletLifeTime) {
                timer = 0f;
                PhotonNetwork.Destroy(gameObject);
            }
            
        }
        else
        {
            ProjectileMovement();
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(GetComponent<Rigidbody2D>().position);
            stream.SendNext(GetComponent<Rigidbody2D>().velocity);
            stream.SendNext(GetComponent<Rigidbody2D>().rotation);
        }
        else
        {
            Vector2 syncPosition = (Vector2)stream.ReceiveNext();
            Vector2 syncVelocity = (Vector2)stream.ReceiveNext();
            rotation = (float)stream.ReceiveNext();

            syncTime = 0f;
            syncDelay = Time.time - lastSynchronizationTime;
            lastSynchronizationTime = Time.time;

            syncEndPosition = syncPosition + syncVelocity * syncDelay;
            syncStartPosition = GetComponent<Rigidbody2D>().position;
        }
    }
    void ProjectileMovement()
    {
        syncTime += Time.deltaTime;
        GetComponent<Rigidbody2D>().position = Vector2.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
        GetComponent<Rigidbody2D>().rotation = rotation;
    }

}
