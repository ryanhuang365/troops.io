using UnityEngine;

public class CameraRotationLock : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
