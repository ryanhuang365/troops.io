using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkTroop : Photon.MonoBehaviour
{
    private Vector3 position;
    private float rotation;


    float syncTime;
    float syncDelay;
    float lastSynchronizationTime;
    Vector3 syncEndPosition;
    Vector3 syncStartPosition;
    public TroopMovement troopMovement;

    // Use this for initialization
    void Start () {
        
        
        
        if (GetComponentInParent<PhotonView>().isMine)
        {
            
            gameObject.name = "MyTroop";
            troopMovement = GetComponentInParent<TroopMovement>();
        }
        else
        {
            gameObject.name = "NetworkTroop";

        }
    }
    private void Update()
    {
        if (GetComponentInParent<PhotonView>().isMine)
        {

        }
        else
        {
            TroopMovement();
        }
    }

    private void OnTriggerEnter2D(Collider2D otherObject)   
    {
        if(gameObject.name == "MyTroop") { 
        string tag = otherObject.gameObject.tag;
            if (tag == "NetworkBullet") {
                Debug.Log("deleteasadsfa");
                PhotonNetwork.Destroy(gameObject);
                
                    troopMovement.level -= 1;
                    troopMovement.RefreshTroopArray();
                
            }
        }
    }
    /*public void DeleteThyself() {
        Debug.Log("deleted");
        PhotonNetwork.Destroy(gameObject);
        troopMovement.level -= 1;
        troopMovement.RefreshTroopArray();
        
    }*/



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
void TroopMovement() {
        syncTime += Time.deltaTime;
        GetComponent<Rigidbody2D>().position = Vector2.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
        GetComponent<Rigidbody2D>().rotation = rotation;
    }
    
    
}
