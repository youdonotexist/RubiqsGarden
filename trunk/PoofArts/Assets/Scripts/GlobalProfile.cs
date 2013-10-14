using UnityEngine;
using System.Collections;

public class GlobalProfile : MonoBehaviour 
{
	public int score = 0;

	// Use this for initialization
	void Start () 
	{
		DontDestroyOnLoad(gameObject);
		Application.LoadLevel("SplashScreen");
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
