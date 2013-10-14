using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {
	
	public Texture[] walk_up;
	public Texture[] walk_down;
	public Texture[] walk_left;
	public Texture[] walk_right;
	
	
	public int[] durations;
	
	public Texture[] currentAnimation = null;
	int currentFrame;
	int frameDelay;
	
	public enum ANIM {
		WALK_UP, 
		WALK_DOWN,
		WALK_LEFT,
		WALK_RIGHT,
		IDLE_UP,
		IDLE_DOWN,
		IDLE_LEFT,
		IDLE_RIGHT
	}
	
	ANIM animation = ANIM.WALK_DOWN;
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if(frameDelay >= durations[currentFrame])
		{
			++currentFrame;
			//if(currentFrame == frames.Length) currentFrame = 0;
			frameDelay = 0;
			
			//renderer.material.mainTexture = frames[currentFrame];
		}
		frameDelay += (int)(Time.deltaTime * 1000);
	}
	
	//public void 
}
