using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TroopMovement : MonoBehaviour {

    public List<GameObject> troops;
    Transform player;
    public GameObject troop;
    public float angleToSpawn = 0;
    public float angleIncrement = 60;
    public Text levelText;
    public int level = 0;


    // Use this for initialization
    void Start () {
        player = transform;
        RefreshTroopArray();
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        
            Vector2 playerPosition = player.position;
            if (troops.Count != 0)
            {
                for (int i = 0; i < troops.Count; i++)
                {



                    Vector2 troopPosition = new Vector2(troops[i].transform.position.x, troops[i].transform.position.y);
                    troops[i].transform.rotation = player.rotation;
                    Vector2 velocity = new Vector2(troopPosition.x - playerPosition.x, troopPosition.y - playerPosition.y);
                    troops[i].GetComponent<Rigidbody2D>().velocity = -velocity;

                }
            }
        
	}
    public void RefreshTroopArray() {
        troops.Clear();
        foreach (Transform troop in player) {
            if (troop.tag == "Troop") {
                
                troops.Add(troop.gameObject);
            }
        }
        levelText.text = "Level: " + level;
    }
    public void spawnATroop()
    {

            float hyp = troop.GetComponent<CircleCollider2D>().radius + player.gameObject.GetComponent<CircleCollider2D>().radius;
            float opp = Mathf.Sin(angleToSpawn) * hyp;
            float adj = Mathf.Cos(angleToSpawn) * hyp;
            Vector3 spawnPos = new Vector3(player.position.x + adj, player.position.y + opp, 0);
            GameObject myTroop = PhotonNetwork.Instantiate(troop.name, spawnPos, player.rotation, 0);
            myTroop.transform.parent = player;
            
            RefreshTroopArray();
            angleToSpawn += angleIncrement;
            if (angleToSpawn >= 360f)
            {
                angleToSpawn -= 360f;
            }
            level++;
            levelText.text = "Level: " + level;
        
    }


}
