using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour {
	public PackedSprite sprite = null;
	Vector2 walkDir = new Vector2(0.0f, 0.0f);
	
	const float _focusDist = 0.25f;
	public float walkSpeed = 40.0f;
	
	public GameObject _focus = null;
	public GameObject _textBox = null;
	//public SpriteText _spriteText = null;
	
	public bool _interacting = false;
	
	//public RoomEvents currentRoom = null;
	public GameObject _heldObject = null;
	
	public Vector3 destination = Vector3.zero;
	public Vector3 start = Vector3.zero;
	public bool isPlayerMoving = false;
	
	public int lives = 100;
	
	public int treasure = 0;
	public GameObject treasureNum;
	public GameObject treasureNumShadow;
	
	public int treasureNeeded = 3;
	
	public enum MoveDirection {UP, DOWN, LEFT, RIGHT};
	public MoveDirection moveDirection = MoveDirection.UP;
	
	public WorldScript worldScript = null;
	
	public GameObject torch;
	public bool useTorch = false;
	
	public GameObject door = null;
	
	float ouchTime = 0.0f;
	
	List<MoveEvent> moveEventList = new List<MoveEvent>();
	
	//public Interaction _currentInteraction;
	
	// Use this for initialization
	void Start () {
		//sprite = GetComponent<Sprite>();
		gameObject.name = "Player";
		sprite = gameObject.GetComponent<PackedSprite>();
		
		torch = transform.FindChild("Torch").gameObject;
		
		torch.active = useTorch;
	}
	
	// Update is called once per frame
	void FixedUpdate() {
		gameObject.rigidbody.AddForce(0.0f, 0.0f, 0.0f);	
	}
	
	public bool IsDoorOpen() {
		return treasure >= treasureNeeded;	
	}
	
	bool died = false;
	float diedTimer = 3.0f;
	void Update () {
		
		if((lives < 0 && died == false) || Input.GetKeyDown(KeyCode.R))
		{
			lives = 0;
			GameObject.Find("SFXPlayer").SendMessage("PlayClip", "Die");
			renderer.enabled = false;
			died = true;
		}
		
		if(died)
		{
			diedTimer -= Time.deltaTime;
			if(diedTimer < 0.0f)
				Application.LoadLevel(Application.loadedLevel);
			return;
		}
		
		//if (_interacting == false) {
			normal();
		//}
		//else {
			//interacting();	
		//}
		
		//GameObject.Find("Lives").guiText.text = "Health: " + lives;
		
		GameObject healthBar = GameObject.Find("HealthBar");
		Vector3 scale = healthBar.transform.localScale;
		scale.x = (lives / 100.0f);
		healthBar.transform.localScale = scale;
		
		ouchTime -= Time.deltaTime;
		if(ouchTime <= 0.0f) renderer.material.color = Color.white;
		
		treasureNum.guiText.text = treasure.ToString();
		treasureNumShadow.guiText.text = treasure.ToString();
		
		if (treasure == treasureNeeded) {
			if (door != null) {
				Door doorScript = door.GetComponentInChildren<Door>();
				doorScript.SetOpen();
			}
		}
		
		if(treasure >= treasureNeeded)
			GameObject.Find("FindTheDoor").guiText.enabled = true;
		
		if (door != null) {
			Door doorScript = door.GetComponentInChildren<Door>();
			doorScript.treasureNeeded.text = Mathf.Max (treasureNeeded - treasure, 0).ToString();
		}
		
		
	}
	
	void HitByLaser()
	{
		GameObject.Find("SFXPlayer").SendMessage("PlayClip", "Hit");
		renderer.material.color = Color.red;
		ouchTime = 0.1f;
		--lives;
	}
	
	void HitByRocket()
	{
		GameObject.Find("SFXPlayer").SendMessage("PlayClip", "Hit");
		renderer.material.color = Color.red;
		ouchTime = 0.1f;
		lives -= 20;
	}
	
	void HitByOtto()
	{
		GameObject.Find("SFXPlayer").SendMessage("PlayClip", "Hit");
		renderer.material.color = Color.red;
		ouchTime = 0.1f;
		lives -= 20;	
	}
	
	void HitByBomb()
	{
		GameObject.Find("SFXPlayer").SendMessage("PlayClip", "Hit");
		renderer.material.color = Color.red;
		ouchTime = 0.1f;
		lives -= 20;	
	}
		
	void normal() {
		// Movement
		Vector2 moveDir = new Vector2(0.0f, 0.0f);
		float speed = 1.0f;
		
		if (Input.GetKeyDown(KeyCode.A)) {
			moveDir.x = -speed;
		} else if (Input.GetKeyDown(KeyCode.D)) {
			moveDir.x = speed;
		}
		else {
			moveDir.x = 0.0f;
		}
		
		if (Mathf.Abs(moveDir.x) == 0.0f) {
			if (Input.GetKeyDown(KeyCode.W)) {
				moveDir.y = speed;
			} else if (Input.GetKeyDown(KeyCode.S)) {
				moveDir.y = -speed;
			}
			else {
				moveDir.y = 0.0f;
			}
		}
		
		bool moveHorizontal = Mathf.Abs(moveDir.x) > 0.0f;
		bool moveVertical = Mathf.Abs(moveDir.y) > 0.0f;
		
		if ((moveHorizontal || moveVertical) && moveEventList.Count < 2)
			moveEventList.Add(new MoveEvent(moveDir, this));
		
		if (moveEventList.Count > 0 && moveEventList[0].UpdateEvent() == false) {
			moveEventList.RemoveAt(0);	
		}
		
			if (moveEventList.Count == 0) {
				moveDir = Vector3.zero;
				if (Input.GetKey(KeyCode.A)) {
					moveDir.x = -speed;
				} else if (Input.GetKey(KeyCode.D)) {
					moveDir.x = speed;
				}
				else {
					moveDir.x = 0.0f;
				}
				
				if (Mathf.Abs(moveDir.x) == 0.0f)
				{
					if (Input.GetKey(KeyCode.W)) {
						moveDir.y = speed;
					} else if (Input.GetKey(KeyCode.S)) {
						moveDir.y = -speed;
					}
					else {
						moveDir.y = 0.0f;	
					}
				}
				
				moveHorizontal = Mathf.Abs(moveDir.x) > 0.0f;
				moveVertical = Mathf.Abs(moveDir.y) > 0.0f;
				
				if ((moveHorizontal || moveVertical) && moveEventList.Count < 2)
					moveEventList.Add(new MoveEvent(moveDir, this));
			
				if (moveEventList.Count > 0 && moveEventList[0].UpdateEvent() == false) {
					moveEventList.RemoveAt(0);
				}
			}
		
		//If I'm off the board for any reason, put me back on it
		if(transform.position.x < 1.0f)
		{
			Vector3 pos = transform.position;
			pos.x = 1.0f;
			transform.position = pos;
			moveEventList.Clear();
		}
		else if(transform.position.x > 8)
		{
			Vector3 pos = transform.position;
			pos.x = 8;
			transform.position = pos;
			moveEventList.Clear();
		}
		else if(transform.position.z < 1.0f)
		{
			Vector3 pos = transform.position;
			pos.z = 1.0f;
			transform.position = pos;
			moveEventList.Clear();
		}
		else if(transform.position.z > 8)
		{
			Vector3 pos = transform.position;
			pos.z = 8;
			transform.position = pos;
			moveEventList.Clear();
		}
		
		if (moveEventList.Count == 0) {
			UVAnimation anim = sprite.GetCurAnim();
			if (anim.name == "Walk Up")
				sprite.DoAnim("Idle Up");
			else if (anim.name == "Walk Down")
				sprite.DoAnim("Idle Down");
			else if (anim.name== "Walk Left")
				sprite.DoAnim("Idle Left");
		}
		
			
		//if (isPlayerMoving == false && moveVertical == false && moveHorizontal == false) {
			
		//}
		//}
		
		//Interaction
		
		/*if (Input.GetKeyDown(KeyCode.Space)) {
			SphereCollider collider = (SphereCollider) _focus.collider;
			Collider[] objs = Physics.OverlapSphere(_focus.transform.position, collider.radius, 1 << 8);
			if (objs.Length > 0) {
				GameObject obj = objs[0].gameObject;
				Interaction i = obj.GetComponent<Interaction>();
				Interactable ib = obj.GetComponent<Interactable>();
				Door d = obj.GetComponent<Door>();
				
				if (i != null && d == null) {
					if (ib != null)
						ib._interacting = true;
					setFacing();
					showInteraction(i, false);	
				}
			}
		}*/
	}
		
	/*void interacting() {
		if (_currentInteraction != null) {
			if (Input.GetKeyDown(KeyCode.Space)) {
				if (_currentInteraction._currentInteraction < _currentInteraction._interactions.Length) {
					_currentInteraction._text.Text = _currentInteraction._interactions[_currentInteraction._currentInteraction++];
				}
				else {
					_currentInteraction._currentInteraction = 0;
					_currentInteraction._text.transform.parent.gameObject.SetActiveRecursively(false);
					
					if (_currentInteraction._callback)
						_currentInteraction._callback.finishText();
					
					if (_currentInteraction.collider) {
						_currentInteraction.collider.enabled = false;
						StartCoroutine(_currentInteraction.enableCollider());
					}
					
					finishInteraction();
				}
			}
		}
	}
	
	public void startInteraction() {
		_interacting = true;	
	}
	
	/*public void showInteraction(Interaction i, bool fromTrigger) {
		_interacting = true;
		_currentInteraction = i;
		i.startInteraction(this, _spriteText, fromTrigger);
	}
	
	public void finishInteraction() {
		_interacting = false;
		if (_currentInteraction) {
			_currentInteraction._currentInteraction = 0;
			_currentInteraction._interactions = null;
			_currentInteraction = null;
		}
	}*/
	
	public void setFacing(Vector2 face) {
		walkDir = face;
		setFacing();
	}
	
	public Vector2 getFacing() {
		return walkDir;	
	}
	
	public void setFacing() {
		if (walkDir.x > 0.0f) {
			sprite.DoAnim("Idle Left");
			//Vector3 localScale = transform.localScale;
			//localScale.x = walkDir.x;
			//transform.localScale = localScale;
			//_focus.transform.position = transform.position + new Vector3(walkDir.x * _focusDist, transform.position.y, walkDir.y * _focusDist);
		}
		else if (walkDir.x < 0.0f) {
			sprite.DoAnim("Idle Left");	
			//Vector3 localScale = transform.localScale;
			//localScale.x = walkDir.x;
			//transform.localScale = localScale;
			//_focus.transform.position = transform.position + new Vector3(walkDir.x * _focusDist, transform.position.y, walkDir.y * _focusDist);
		}
		else if (walkDir.y > 0.0f) {
			sprite.DoAnim("Idle Up");	
			//_focus.transform.position = transform.position + new Vector3(walkDir.x * _focusDist, transform.position.y, walkDir.y * _focusDist);
		}
		else if (walkDir.y < 0.0f) {
			sprite.DoAnim("Idle Down");	
			//_focus.transform.position = transform.position + new Vector3(walkDir.x * _focusDist, transform.position.y, walkDir.y * _focusDist);
		}	
	}
}

public class MoveEvent {
	public MoveEvent(Vector2 moveDirection, PlayerControl go)
	{	
		moveDir = moveDirection;
		player = go;
		world = GameObject.Find("World").GetComponent<WorldScript>();
	}
	
	bool started;
	Vector2 moveDir;
	PlayerControl player = null;
	GameObject finalTarget = null;
	WorldScript world = null;
	
	bool stuck = false;
	float stuckElapsed = 0.0f;
	
	public bool UpdateEvent()
	{
		GameObject tile = world.getTileAtIndex(Mathf.RoundToInt(player.transform.position.x), Mathf.RoundToInt(player.transform.position.z));
		if(tile.name.StartsWith("Floor") == false && tile.name.StartsWith("Open_Floor_Door") == false)
		{
			return false;
		}
		
		bool moveHorizontal = Mathf.Abs(moveDir.x) > 0.0f;
		bool moveVertical = Mathf.Abs(moveDir.y) > 0.0f;
		
		if (moveHorizontal) {
			PackedSprite sprite = player.sprite;
			
			if (moveDir.x > 0.0f) {
				sprite.DoAnim("Walk Left");
			}
			else {
				sprite.DoAnim("Walk Left");
			}
				
			Vector3 localScale = player.transform.localScale;
			localScale.x = moveDir.x;
			player.transform.localScale = localScale;
			
			//walkDir = moveDir;	
		}
		else if (moveVertical) {
			PackedSprite sprite = player.sprite;
			
			if (moveDir.y > 0.0f) {
				sprite.DoAnim("Walk Up");
			}
			else {
				sprite.DoAnim("Walk Down");
			}
			
			//walkDir = moveDir;	
		}
		
		if (finalTarget == null) {
			finalTarget = world.getTileAtIndex(Mathf.RoundToInt(player.transform.position.x) + Mathf.RoundToInt(moveDir.x), Mathf.RoundToInt(player.transform.position.z) + Mathf.RoundToInt(moveDir.y));	
			//finalTarget.renderer.material.color = Color.red;
		}
		
		if (finalTarget != null) {
			//If it's the floor
			if (finalTarget.layer == LayerMask.NameToLayer("Pit")) {
				//Debug.Log ("Dead.");
				stuck = true;
				//return false;
			}
			else if (finalTarget.layer == LayerMask.NameToLayer("Obstacle") && finalTarget.name.StartsWith("Open_Floor_Door") == false) {
				player.setFacing(moveDir);
				return false;
			}
			else if (world.isBorder(Mathf.RoundToInt(finalTarget.transform.position.x), Mathf.RoundToInt(finalTarget.transform.position.z))) {
				player.setFacing(moveDir);
				return false;	
			}
		}
		else {
			return false;	
		}
		
		player.transform.position = Vector3.MoveTowards(player.transform.position, finalTarget.transform.position, player.walkSpeed * Time.deltaTime);
		float dist = Vector3.Distance(player.transform.position, finalTarget.transform.position);
		
		if (dist < 0.01f) {
			if (stuck == true) {				
				
				player.setFacing(moveDir);
				stuckElapsed += Time.deltaTime;
				if (stuckElapsed > 1.0f) {
					stuck = false;
					stuckElapsed = 0.0f;
					player.setFacing(moveDir);
					return false;	
				}
					
			}
			else {
				return false;	
			}
			//finalTarget.renderer.material.color = Color.white;
		}
		
		return true;
	}
	
	public bool isShifting() {
		return started;	
	}
	
	public Vector3 currentDestination() {
		return Vector3.zero;	
	}
}
