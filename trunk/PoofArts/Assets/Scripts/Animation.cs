using UnityEngine;
using System.Collections;

public class Animation : MonoBehaviour {
	
	public Texture[] frames;
	public int[] durations;
	
	int currentFrame;
	int frameDelay;
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if(frameDelay >= durations[currentFrame])
		{
			++currentFrame;
			if(currentFrame == frames.Length) currentFrame = 0;
			frameDelay = 0;
			
			renderer.material.mainTexture = frames[currentFrame];
		}
		frameDelay += (int)(Time.deltaTime * 1000);
	}
}
