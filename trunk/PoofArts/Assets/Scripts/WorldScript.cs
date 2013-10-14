using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ShiftEventType
{
	LeftShift,
	RightShift,
	UpShift,
	DownShift
}

public class ShiftEvent
{
	public ShiftEvent(ShiftEventType pType, int pIndex)
	{	
		type = pType;
		index = pIndex;
		shiftAmt = 0.0f;			
		started = false;
	}
	
	public ShiftEventType type;
	int index;
	float shiftAmt;		
	bool started;
	
	GameObject target = null;
	bool playerOnBorder = false;
	
	public bool UpdateEvent(WorldScript worldScript)
	{
		GameObject player = GameObject.Find("Player");
		int pX = Mathf.RoundToInt(player.transform.position.x);
		int pZ = Mathf.RoundToInt(player.transform.position.z);
		
		float oldShiftAmt = shiftAmt;
		shiftAmt += 4.0f * Time.deltaTime;	
		shiftAmt = Mathf.Clamp(shiftAmt, 0.0f, 1.0f);
		
		float offset = shiftAmt - oldShiftAmt;		
		Vector3 shiftVec = Vector3.zero;
		
		GameObject first = null;
		
		switch(type)
		{
		case ShiftEventType.LeftShift:			
			
			//Confirm that this shift should still happen				
			if(started == false)
			{				
				GameObject nextTile = worldScript.getTileAtIndex(pX + 1, pZ);
				
				if((nextTile.name.StartsWith("Floor") == false || nextTile.name.StartsWith("Floor_Door")) && pX == 1)
					return false;				
			}
			
			shiftVec = new Vector3(-offset, 0, 0);
			ShiftPlayer(shiftVec, worldScript);
			
			if(started == false)
			{	
				//Start the shift
				GameObject backup = worldScript.worldTiles[0, index];
				for(int x = 0; x < WorldScript.worldWidth - 1; ++x)
				{
					worldScript.worldTiles[x,index] = worldScript.worldTiles[x+1,index];
				}
				worldScript.worldTiles[WorldScript.worldWidth - 1, index] = backup;
				worldScript.worldTiles[WorldScript.worldWidth - 1, index].transform.Translate(WorldScript.worldWidth, 0, 0, Space.World);
				started = true;				
			}
			for(int x = 0; x < WorldScript.worldWidth; ++x)
			{
				worldScript.worldTiles[x,index].transform.Translate(shiftVec, Space.World);
			}
			break;
			
		case ShiftEventType.RightShift:
						
			//Confirm that this shift should still happen				
			if(started == false)
			{				
				GameObject nextTile = worldScript.getTileAtIndex(pX - 1, pZ);
				if((nextTile.name.StartsWith("Floor") == false || nextTile.name.StartsWith("Floor_Door")) && pX == worldScript.visibleWidth)
					return false;				
			}
			
			shiftVec = new Vector3(offset, 0, 0);			
			ShiftPlayer(shiftVec, worldScript);
			
			if(started == false)
			{
				
				//Start the shift				
				GameObject backup = worldScript.worldTiles[WorldScript.worldWidth - 1, index];			
				for(int x = WorldScript.worldWidth - 1; x > 0; --x)
				{
					worldScript.worldTiles[x,index] = worldScript.worldTiles[x-1,index];
				}			
				worldScript.worldTiles[0, index] = backup;	
				worldScript.worldTiles[0, index].transform.Translate(-WorldScript.worldWidth, 0, 0, Space.World);
				started = true;
			}
			for(int x = 0; x < WorldScript.worldWidth; ++x)
			{
				worldScript.worldTiles[x,index].transform.Translate(shiftVec, Space.World);
			}				
			break;
			
		case ShiftEventType.UpShift:			
			
			//Confirm that this shift should still happen				
			if(started == false)
			{				
				GameObject nextTile = worldScript.getTileAtIndex(pX, pZ - 1);
				if((nextTile.name.StartsWith("Floor") == false || nextTile.name.StartsWith("Floor_Door")) && pZ == worldScript.visibleHeight)
					return false;				
			}
			
			shiftVec = new Vector3(0, 0, offset);
			ShiftPlayer(shiftVec, worldScript);
			
			if(started == false)
			{				
				//Start the shift
				GameObject backup = worldScript.worldTiles[index, WorldScript.worldHeight-1];			
				for(int y = WorldScript.worldHeight - 1; y > 0; --y)
				{
					worldScript.worldTiles[index,y] = worldScript.worldTiles[index,y-1];
				}			
				worldScript.worldTiles[index, 0] = backup;	
				worldScript.worldTiles[index, 0].transform.Translate(0, 0, -WorldScript.worldHeight, Space.World);
				started = true;
			}
			for(int y = 0; y < WorldScript.worldHeight; ++y)
			{
				worldScript.worldTiles[index,y].transform.Translate(shiftVec, Space.World);
			}
			break;
			
		case ShiftEventType.DownShift:
			
			//Confirm that this shift should still happen				
			if(started == false)
			{				
				GameObject nextTile = worldScript.getTileAtIndex(pX, pZ + 1);
				if((nextTile.name.StartsWith("Floor") == false || nextTile.name.StartsWith("Floor_Door")) && pZ == 1)
					return false;				
			}
			
			shiftVec = new Vector3(0, 0, -offset);
			ShiftPlayer(shiftVec, worldScript);
			
			if(started == false)
			{
				//Start the shift
				GameObject backup = worldScript.worldTiles[index, 0];
				for(int y = 0; y < WorldScript.worldHeight - 1; ++y)
				{
					worldScript.worldTiles[index,y] = worldScript.worldTiles[index,y+1];
				}
				worldScript.worldTiles[index, WorldScript.worldHeight-1] = backup;	
				worldScript.worldTiles[index, WorldScript.worldHeight-1].transform.Translate(0, 0, WorldScript.worldHeight, Space.World);				
				started = true;
			}
			
			for(int y = 0; y < WorldScript.worldHeight; ++y)
			{
				worldScript.worldTiles[index,y].transform.Translate(shiftVec, Space.World);
			}
			break;
			
		default:
			break;
		}
			
		
		if(shiftAmt >= 1.0f)
		{			
			return false;
		}
		
		return true;
	}
	
	public bool isShifting() {
		return started;	
	}
	
	public void ShiftPlayer(Vector3 offset, WorldScript worldScript) {
		GameObject player = GameObject.Find("Player");
		if (target == null) {
			Vector3 shiftNorm = offset.normalized;
			
			target = worldScript.getTileAtIndex(Mathf.RoundToInt(player.transform.position.x), 
												Mathf.RoundToInt(player.transform.position.z));
			
			Vector3 dir = shiftNorm + (target.transform.position);
			
			playerOnBorder = worldScript.isBorder(Mathf.RoundToInt(dir.x), Mathf.RoundToInt(dir.z));
		}
		
		if (playerOnBorder == false) {
			player.transform.Translate(offset, Space.World);
		}
	}
}

public class WorldScript : MonoBehaviour 
{
	public int visibleWidth = 8;
	public int visibleHeight = 8;
	
	public static int worldWidth = 16;
	public static int worldHeight = 16;
	
	public GameObject[] tiles;
	public float[] tileWeights;
	float sumOfWeights;
	
	public GameObject[,] worldTiles;	
	
	float shiftAmt = 0.0f;	
	
	public bool useTorches = false;
	public float ambientLightLevel = 1.0f;
	public float fullLight = 8.0f;
	
	public int levelIndex = 1;
	
	Preprocessor processor = null;
	
	
	List<ShiftEvent> shiftEventList = new List<ShiftEvent>();
	
	// Use this for initialization
	void Start ()
	{
		//Tile weights
		if(tiles.Length != tileWeights.Length)
		{
			Debug.Log("ERROR NOT GOOD BAD THING! - Needs a weight for every tile.");
		}
		sumOfWeights = 0;
		foreach(float w in tileWeights)
		{
			sumOfWeights += w;
		}
		
		//Create a temporary world
		worldTiles = new GameObject[worldWidth, worldHeight];				
		for(int x = 0; x < worldWidth; ++x)
		{
			for(int y = 0; y < worldHeight; ++y)
			{
				int tile = RandomTile();
				GameObject newObj = Instantiate(tiles[tile], new Vector3(x, 0, y), Quaternion.identity) as GameObject; 	
				newObj.transform.parent = transform;
				
				if (newObj.name.Contains("Floor")) 	{
					Vector3 pos = newObj.transform.localPosition;
					pos.y = -0.5f;
					newObj.transform.localPosition = pos;
				}
				
				//newObj.renderer.material.color = new Color(Random.value, Random.value, Random.value, 1.0f);
				worldTiles[x,y] = newObj;
			}
		}
		
		gameObject.SendMessage("process", this, SendMessageOptions.DontRequireReceiver);
		
		ProcessVisibility();
		
	}
	
	int RandomTile()
	{
		float val = Random.value * sumOfWeights;
		float runningSum = 0;
		for(int i = 0; i < tileWeights.Length; ++i)
		{
			runningSum += tileWeights[i];	
			if(val <= runningSum)
				return i;
		}		
		
		return 0;
	}
	
	// Update is called once per frame
	int index = 0;
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Alpha1))
			index = 0;
		if(Input.GetKeyDown(KeyCode.Alpha2))
			index = 1;
		if(Input.GetKeyDown(KeyCode.Alpha3))
			index = 2;
		if(Input.GetKeyDown(KeyCode.Alpha4))
			index = 3;
		if(Input.GetKeyDown(KeyCode.Alpha5))
			index = 4;
		if(Input.GetKeyDown(KeyCode.Alpha6))
			index = 5;
		if(Input.GetKeyDown(KeyCode.Alpha7))
			index = 6;
		if(Input.GetKeyDown(KeyCode.Alpha8))
			index = 7;		
		
		if(shiftEventList.Count > 0 && shiftEventList[0].UpdateEvent(this) == false)
		{
			shiftEventList.RemoveAt(0); 
		}		
		
		if (useTorches) {
			int torchCount = GetTorchCount();
			ambientLightLevel = Mathf.Max(Mathf.Min(((float)torchCount) / fullLight, 0.3f), 0.05f);
		}
		else {
			ambientLightLevel = 1.0f;
		}
		
		//Debug.Log ("Ambient Light: " + ambientLightLevel);
		
		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, new Color(ambientLightLevel, ambientLightLevel, ambientLightLevel), Time.deltaTime);
		
		ProcessVisibility();
	}	
	
	public void ProcessVisibility()
	{
		//Create a temporary world
		for(int x = 0; x < worldWidth; ++x)
		{
			for(int y = 0; y < worldHeight; ++y)
			{
				TileBase tb = worldTiles[x,y].GetComponent<TileBase>();
				tb.Hide();
				//worldTiles[x,y].renderer.enabled = false;	
				//worldTiles[x,y].SetActiveRecursively(false);
			}
		}
		
		//Make some visible
		for(int x = 0; x <= visibleWidth + 1; ++x)
		{
			for(int y = 0; y <= visibleHeight + 1; ++y)
			{
				if (x != 0 && x != visibleWidth + 1 && y != 0 && y != visibleWidth + 1) {
					TileBase tb = worldTiles[x,y].GetComponent<TileBase>();
					tb.Show();
				}
				else {
					TileBase tb = worldTiles[x,y].GetComponent<TileBase>();
					tb.OnBorder();
				}
				//worldTiles[x,y].renderer.enabled = true;	
				//worldTiles[x,y].SetActiveRecursively(true);
			}
		}
	}
	
	public void ChangeTo(int x, int y, int tile)
	{
		Vector3 oldPos = worldTiles[x,y].transform.position;
		Destroy(worldTiles[x,y].gameObject);
		
		GameObject newObj = Instantiate(tiles[tile], oldPos, Quaternion.identity) as GameObject; 	
		newObj.transform.parent = transform;
		
		if (newObj.name.Contains("Floor")) 
		{
			Vector3 pos = newObj.transform.localPosition;
			pos.y = -0.5f;
			newObj.transform.localPosition = pos;
		}
		else
		{
			Vector3 pos = newObj.transform.localPosition;
			pos.y = 0;
			newObj.transform.localPosition = pos;
		}
		
		worldTiles[x,y] = newObj;
	}
	
	public void ShiftLeft(int row)
	{				
		if(shiftEventList.Count < 2)
			shiftEventList.Add(new ShiftEvent(ShiftEventType.LeftShift, row));
	}
	
	public void ShiftRight(int row)
	{	
		if(shiftEventList.Count < 2)
			shiftEventList.Add(new ShiftEvent(ShiftEventType.RightShift, row));	
	}
	
	
	public void ShiftDown(int col)
	{			
		if(shiftEventList.Count < 2)
			shiftEventList.Add(new ShiftEvent(ShiftEventType.DownShift, col));	
	}
	
	public void ShiftUp(int col)
	{	
		if(shiftEventList.Count < 2)
			shiftEventList.Add(new ShiftEvent(ShiftEventType.UpShift, col));	
	}
	
	public GameObject getTileAtIndex(int x, int y) {
			return worldTiles[x, y];
	}
	
	public bool isMoving() {
		return shiftEventList.Count > 0 && shiftEventList[0].isShifting();
				
	}
	
	public bool hasEvents() {
		return shiftEventList.Count > 0;	
	}
	
	public bool isBorder(int x, int y) {
		return x == 0 || x == visibleWidth + 1 || y == 0 || y == visibleHeight + 1;
	}
	
	public int GetTorchCount() {
		int count = 0;
		for(int x = 0; x <= visibleWidth + 1; ++x)
		{
			for(int y = 0; y <= visibleHeight + 1; ++y)
			{
				if (x != 0 && x != visibleWidth + 1 && y != 0 && y != visibleWidth + 1) {
					GameObject go = worldTiles[x, y];
					if(go != null)
					{
						Light l = go.GetComponentInChildren<Light>();
						if (l != null)
							count++;
					}
				}
			}
		}
		
		Debug.Log ("Light Count: " + count);
		
		return count;
	}
}
