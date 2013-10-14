using UnityEngine;
using System.Collections;

public class MovingFloor : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.P)) {
			Vector3 pos = gameObject.transform.position;
			pos.x += -1.0f;
			gameObject.transform.position = pos;
		}
	}
}
