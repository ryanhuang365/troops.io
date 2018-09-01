using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : Photon.MonoBehaviour {
    public GameObject myCamera;
    public GameObject Canvas;
    public GameObject SpawnTroop;
    public GameObject JoystickBack;
    public GameObject LevelText;

    bool isAlive = true;

    public float bulletSpeed = 10;



    float syncTime;
    float syncDelay;
    float lastSynchronizationTime;
    Vector3 syncEndPosition;
    Vector3 syncStartPosition;
    float rotation;
    public float moveSpeed = 3.0f;
    public Vector2 MoveVector { set; get; }
    public VirtualJoystick joystick;
    public Rigidbody2D rb;
    public float angle;
    public GameObject shootManager;

    void Start()
    {
        
        if (photonView.isMine)
        {
            //shootManager.SetActive(true);
            gameObject.name = "Me";
            myCamera.SetActive(true);
            GetComponent<TroopMovement>().enabled = true;
            /*SpawnTroop.SetActive(true);
            JoystickBack.SetActive(true);
            LevelText.SetActive(true);*/
            Canvas.SetActive(true);
        }
        else
        {
            gameObject.name = "NetworkPlayer";
        }

    }
    void Update()
    {
        if (photonView.isMine)
        {
            InputMovement();
        }
        else
        {
            SyncedMovement();
        }
    }
    public void InputMovement()
    {

        MoveVector = Input();

        Move();

    }
    private void Move()
    {

        rb.velocity = new Vector2((MoveVector.x * moveSpeed), (MoveVector.y * moveSpeed));
        rb.MoveRotation(-angle);

    }
    private Vector2 Input()
    {
        Vector2 dir = Vector2.zero;
        dir.x = joystick.Horizontal();
        dir.y = joystick.Vertical();
        if (dir.x != 0 && dir.y != 0)
        {

            angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        }

        if (dir.magnitude > 1)
        {
            dir.Normalize();
        }
        return dir;

    }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting) {
            stream.SendNext(GetComponent<Rigidbody2D>().position);
            stream.SendNext(GetComponent<Rigidbody2D>().velocity);
            stream.SendNext(GetComponent<Rigidbody2D>().rotation);
        }
        else {
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
    private void SyncedMovement()
    {
        syncTime += Time.deltaTime;
        GetComponent<Rigidbody2D>().position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
        GetComponent<Rigidbody2D>().rotation = rotation;
    }
}
