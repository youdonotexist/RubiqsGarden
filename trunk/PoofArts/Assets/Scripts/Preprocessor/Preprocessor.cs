using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Preprocessor : MonoBehaviour {
	public WorldScript worldScript = null;
	public GameObject player = null;
	
	public GameObject floorPrefab = null;
	public GameObject doorPrefab = null;
	
	public void ClearPlayerSpot() {
		int x = GetPlayerTileX();
		int y = GetPlayerTileY();
		
		GameObject playerTile = worldScript.getTileAtIndex(x, y);
		DestroyAndReplace(playerTile, floorPrefab);
	}
	
	public void AddDoor() {
		MinMaxTile("Door", 1, 1, doorPrefab, new string[] {"torch"}, true);
		PlayerControl player = GetPlayer();
		GameObject door = GetDoor();
		if (door != null) {
			Debug.Log ("Door Added");
			player.door = door;
			Door script = door.GetComponentInChildren<Door>();
			script.SetClosed();
			script.treasureNeeded.text = player.treasureNeeded.ToString();
		}
		else {
			Debug.Log ("ERROR NO DOOR CREATED");	
		}
		
	}
	
	public void ClearVisibleEnemies() {
		for(int x = 0; x <= worldScript.visibleWidth / 2; ++x)
		{
			for(int y = 0; y <= worldScript.visibleHeight / 2; ++y)
			{
				
				GameObject go = worldScript.getTileAtIndex(x, y);
				
				if (go.name.Contains("Floor_EvilOtto")) {
					removeChildren(go);
				}
				else if (go.name.Contains("Floor_Rocket1")) {
					removeChildren(go);	
				}
				else if (go.name.Contains("Laser")) {
					DestroyAndReplace(go, floorPrefab);
				}
			}
		}
	}
	
	public GameObject DestroyAndReplace(GameObject go, GameObject prefab) {
		
		if (prefab != null) {
			int x = Mathf.RoundToInt(go.transform.position.x);
			int y = Mathf.RoundToInt(go.transform.position.z);
			
			Vector3 oldPos = go.transform.position;
			
			Destroy(go);
			GameObject newObj = Instantiate(prefab, oldPos, Quaternion.identity) as GameObject; 	
			newObj.transform.parent = transform;
					
			if (newObj.name.Contains("Floor")) 	{
				Vector3 pos = newObj.transform.localPosition;
				pos.y = -0.5f;
				newObj.transform.localPosition = pos;
			}
					
			worldScript.worldTiles[x,y] = newObj;
			
			if (worldScript.isBorder(x, y)) {
				TileBase tb = newObj.GetComponent<TileBase>();
				tb.Hide();
				Debug.Log ("Hiding" + newObj.name);
			}
			
			return newObj;
		}
		else {
			Debug.Log ("PROCESSOR: FAILED REPLACING TILE WITH NULL PREFAB");	
			return null;
		}
	}
	
	public void removeChildren(GameObject go) {
		Transform[] ts = go.GetComponentsInChildren<Transform>();
		for (int i = 0; i < ts.Length; i++) {
			Transform t = ts[i];
			if (t.gameObject != go) {
				Destroy (t.gameObject);	
			}
		}
	}
	
	public void MinMaxTile(string name, int min, int max, GameObject prefab, string[] avoid, bool onlyVisible) {
		List<GameObject> torches = new List<GameObject>();
		
		for(int x = 0; x <= worldScript.visibleWidth + 1; ++x)
		{
			for(int y = 0; y <= worldScript.visibleHeight + 1; ++y)
			{
				if (x != 0 && x != worldScript.visibleWidth + 1 && y != 0 && y != worldScript.visibleWidth + 1) {
					GameObject go = worldScript.getTileAtIndex(x, y);
					if (go.name.Contains(name)) {
						torches.Add(go);	
					}
				}
			}
		}
		
		
		//Add Some More
		if (torches.Count < min) {
			int needed = min - torches.Count;
			int playerX = GetPlayerTileX();
			int playerY = GetPlayerTileY();
			
			
			while (torches.Count < min) {
				int x = Random.Range(0, worldScript.visibleWidth);
				int y = Random.Range(0, worldScript.visibleHeight);
				
				bool good = true;
				
				//Make sure there isn't a player here
				if (x == playerX && y == playerY) {
					good = false;
				}
				
				//Make Sure there isn't already on there
				GameObject go = worldScript.getTileAtIndex(x, y);
				if (go.name.Contains(name)) {
					good = false;
				}
				
				//Make sure it doesn't remove anything
				if (avoid != null && avoid.Length > 0) {
					for (int i = 0; i < avoid.Length; i++) {
						string a = avoid[i];
						if (go.name.Contains(a)) {
							good = false;
						}
					}
				}
				
				//Make sure it's visible
				if (onlyVisible) {
					if(worldScript.isBorder(x, y)) {
						good = false;
					}
				}
				
				if (good) {
					Debug.Log ("Adding " + name);
					GameObject newGo = DestroyAndReplace(go, prefab);
					torches.Add(newGo);
				}
			}
		}
		//Remove some
		else if (torches.Count > max) {
			
			while (torches.Count > max) {
				GameObject go = torches[0];
				DestroyAndReplace(go, floorPrefab);
				torches.RemoveAt (0);
			}
		}
	}
	
	public int GetPlayerTileX() {
		if (player == null) 
			player = GameObject.Find("Player");
			
		return Mathf.RoundToInt(player.transform.position.x);	
	}
	
	public int GetPlayerTileY() {
		if (player == null) 
			player = GameObject.Find("Player");
		
		return Mathf.RoundToInt(player.transform.position.z);
	}
	
	public PlayerControl GetPlayer() {
		if (player == null)	
			player = GameObject.Find("Player");
		
		return player.GetComponent<PlayerControl>();
	}
	
	public GameObject GetDoor() {
		
		GameObject door = GameObject.Find("Floor_Door(Clone)");
		return door;
	}
}
