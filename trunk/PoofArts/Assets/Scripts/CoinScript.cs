using UnityEngine;
using System.Collections;

public class CoinScript : MonoBehaviour
{
	
	Vector3 target;
	
	// Use this for initialization
	void Start ()
	{
		target = new Vector3(4.5f, 5, 9.0f);//Camera.main.ViewportToWorldPoint (new Vector3 (0.5f, 0.0f, -0.8f));
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position = Vector3.MoveTowards (transform.position, target, 15.0f * Time.deltaTime);		
		if (transform.position == target) 
		{
			Destroy (gameObject);	
		}
			
	}
}
