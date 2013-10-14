using UnityEngine;
using System.Collections;

public class SFXPlayer : MonoBehaviour {
	
	public AudioClip[] clips;
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void PlayClip(string clipName)
	{
		for(int i = 0; i < clips.Length; ++i)
		{
			if(clips[i].name == clipName)
				audio.PlayOneShot(clips[i]);	
		}
	}
}
