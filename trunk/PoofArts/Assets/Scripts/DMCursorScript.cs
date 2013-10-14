using UnityEngine;
using System.Collections;

public class DMCursorScript : MonoBehaviour 
{
	public PlayerControl player;
	public WorldScript world;
	
	public enum ControlScheme
	{
		FollowPlayer,
		FollowMouse
	};
	public ControlScheme controls = ControlScheme.FollowPlayer;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch(controls)
		{
		case ControlScheme.FollowPlayer:			
			FollowPlayerControls();
			break;
			
		case ControlScheme.FollowMouse:
			FollowMouseControls();
			break;
			
		default:
			break;
		};
	}
	
	void FollowPlayerControls()
	{
		transform.position = player.transform.position;
		
		//Shift Controls		
		if (player.isPlayerMoving == false) {
			if(Input.GetKeyDown(KeyCode.LeftArrow))
			{	
				world.ShiftLeft(Mathf.RoundToInt(transform.position.z));
			}
			if(Input.GetKeyDown(KeyCode.RightArrow))
			{
				world.ShiftRight(Mathf.RoundToInt(transform.position.z));
			}
			if(Input.GetKeyDown(KeyCode.UpArrow))
			{
				world.ShiftUp(Mathf.RoundToInt(transform.position.x));
			}			
			if(Input.GetKeyDown(KeyCode.DownArrow))
			{
				world.ShiftDown(Mathf.RoundToInt(transform.position.x));
			}	
		}
	}
	
	Vector3 mouseDownPos;
	void FollowMouseControls()
	{
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.y = 1;
		
		mousePos.x = Mathf.Clamp(mousePos.x, 1, 8);
		mousePos.z = Mathf.Clamp(mousePos.z, 1, 8);
		
		
		transform.position = mousePos;
		
		if(Input.GetMouseButtonDown(0))
		{
			mouseDownPos = mousePos;	
		}
		
		if(Input.GetMouseButton(0))
		{
			Vector3 diff = mousePos - mouseDownPos;
			bool horiz = (Mathf.Abs(diff.x) >= Mathf.Abs(diff.z));
			if(horiz)
			{
				if(diff.x > 1.0f)
				{
					mouseDownPos.x += 1.0f;
					world.ShiftRight(Mathf.RoundToInt(transform.position.z));
				}
				else if(diff.x < -1.0f)
				{
					mouseDownPos.x -= 1.0f;
					world.ShiftLeft(Mathf.RoundToInt(transform.position.z));
				}
			}
			else
			{
				if(diff.z > 1.0f)
				{
					mouseDownPos.z += 1.0f;	
					world.ShiftUp(Mathf.RoundToInt(transform.position.x));
				}
				else if(diff.z < -1.0f)
				{
					mouseDownPos.z -= 1.0f;
					world.ShiftDown(Mathf.RoundToInt(transform.position.x));
				}
			}
		}
	}

}
