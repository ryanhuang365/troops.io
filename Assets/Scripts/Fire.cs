using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Fire : Photon.MonoBehaviour, IPointerDownHandler {
    private Image fireButton;
    public GameObject projectile;
    public GameObject origin;

    public NetworkPlayer networkPlayer;
    public GameObject Player;
    public TroopMovement troopList;
    float fireRate = 2f;
    private float timer = 0;
    private bool fireAvailable;
    public GameObject ShootManager;
    

    public List<GameObject> projectiles;
    // Use this for initialization
    void Start()
    {
        
        origin = GetComponentInParent<TroopMovement>().gameObject; ;
        fireButton = GetComponent<Image>();

    }
    public virtual void OnPointerDown(PointerEventData ped)
    {
        Vector2 pos;
        if (fireAvailable && RectTransformUtility.ScreenPointToLocalPointInRectangle(fireButton.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {

            //Vector2 fireDirection = new Vector2(Mathf.Sin(networkPlayer.angle) * networkPlayer.bulletSpeed, Mathf.Cos(networkPlayer.angle) * networkPlayer.bulletSpeed);
            //GameObject proj = PhotonNetwork.Instantiate(projectile.name, new Vector2(networkPlayer.gameObject.GetComponent<Rigidbody2D>().position.x, networkPlayer.gameObject.GetComponent<Rigidbody2D>().position.y), Quaternion.Euler(0,0,networkPlayer.angle), 0);
            ShootManager.GetComponent<PhotonView>().RPC("FireBullet", PhotonTargets.AllViaServer, new Vector2(networkPlayer.gameObject.GetComponent<Rigidbody2D>().position.x, networkPlayer.gameObject.GetComponent<Rigidbody2D>().position.y), networkPlayer.angle);

            foreach (GameObject troop in troopList.troops) {
                Debug.Log(1);
                //GameObject troopProj = PhotonNetwork.Instantiate(projectile.name, new Vector2(troop.transform.position.x, troop.transform.position.y), Quaternion.Euler(0, 0, networkPlayer.angle), 0);
                ShootManager.GetComponent<PhotonView>().RPC("FireBullet", PhotonTargets.AllViaServer, new Vector2(troop.GetComponent<Rigidbody2D>().position.x, troop.GetComponent<Rigidbody2D>().position.y), networkPlayer.angle);

            }
            fireAvailable = false;
        }
    }
    
	
	// Update is called once per frame
	void Update () {
        if (!fireAvailable)
        {
            timer += Time.deltaTime;
            if (timer >= fireRate) {
                fireAvailable = true;
                timer = 0f;
            }
        }
	}
    /*[PunRPC]
    void FireBullet(Vector2 startPos, float angle)
    {
        GameObject bullet = (GameObject)Instantiate(Resources.Load("Projectile"), startPos, new Quaternion(0,0,angle,0));
        Vector2 fireDirection = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad) * networkPlayer.bulletSpeed, Mathf.Cos(angle * Mathf.Deg2Rad) * networkPlayer.bulletSpeed);
        bullet.GetComponent<Rigidbody2D>().velocity = fireDirection;
        
        
        bullet.GetComponent<Projectile>().spawnOrigin = origin;
        if (Player.GetComponent<PhotonView>().isMine)
        {
            bullet.tag = "MyBullet";
        }
        else {
            bullet.tag = "NetworkBullet";
        }
    }*/
}
