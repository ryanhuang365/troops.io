using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float moveSpeed = 3.0f;
    public Vector2 MoveVector { set; get; }
    public VirtualJoystick joystick;
    public Rigidbody2D rb;
    public float angle;

    private Rigidbody thisRigidBody;


	// Use this for initialization
	void Start () {

        
	}
	
	
	void FixedUpdate () {
        MoveVector = Input();

        Move();

	}
    private void Move() {

        rb.velocity = new Vector2((MoveVector.x * moveSpeed), (MoveVector.y * moveSpeed));
        //rb.rotation = angle ;
        rb.MoveRotation(angle * - Mathf.Rad2Deg);

    }
    private Vector2 Input() {   
        Vector2 dir = Vector2.zero;
        dir.x = joystick.Horizontal();
        dir.y = joystick.Vertical();
        if (dir.x != 0 && dir.y != 0)
        {
            
            angle = Mathf.Atan2(dir.x, dir.y);
        }

        if (dir.magnitude > 1) {
            dir.Normalize();
        }
        return dir;

    }

}
