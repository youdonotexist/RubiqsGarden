using UnityEngine;
using System.Collections;

public class UISceneScript : MonoBehaviour {
	
	public string nextScreen;
	
	float timer = 1.0f;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer -= Time.deltaTime;
		
		if(timer <= 0.0f)
			if(Input.anyKeyDown) OnMouseDown();
	}
	
	void OnMouseDown()
	{
		if(nextScreen == "SplashScreen")			
			GameObject.Find("GlobalProfile").GetComponent<GlobalProfile>().score = 0;
		Application.LoadLevel(nextScreen);	
	}
}
