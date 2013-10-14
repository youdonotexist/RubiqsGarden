using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{

	public Texture open;
	public Texture closed;
	
	public TextMesh treasureNeeded;
	
	bool isOpen = false;
	
	public void SetOpen() {
		renderer.material.mainTexture = open;
		
		if (isOpen == false) {
			SFXPlayer audio = GameObject.Find("SFXPlayer").GetComponent<SFXPlayer>();
			GameObject.Find("SFXPlayer").SendMessage("PlayClip", "door_open");	
			this.transform.parent.name = "Open_Floor_Door";
			this.gameObject.layer = LayerMask.NameToLayer("Default");
		}
			
		
		isOpen = true;
	}
	
	public void SetClosed() {
		renderer.material.mainTexture = closed;
		isOpen = false;
	}
	
	public bool IsOpen() {
		return isOpen;
	}	
	
	public void OnTriggerEnter(Collider c) {
		Debug.Log ("Door Trigger");
		if (c.name.Equals("Player")) {
			if (isOpen) {
				WorldScript worldScript = GameObject.Find("World").GetComponent<WorldScript>();
				int currentLevel = worldScript.levelIndex;
				Application.LoadLevel("Level" + (currentLevel + 1)); 
			}
		}
	}
	
	
	
}

