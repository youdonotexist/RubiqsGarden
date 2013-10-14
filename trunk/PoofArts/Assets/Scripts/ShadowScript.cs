using UnityEngine;
using System.Collections;

public class ShadowScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = transform.parent.position + new Vector3(0, -0.02f, -0.3f);
	}
}
