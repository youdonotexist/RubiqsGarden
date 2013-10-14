using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {

	// Use this for initialization
	public enum Direction {
		UP, DOWN, LEFT, RIGHT	
	};
	
	public GameObject laserObject = null;
	public LineRenderer line = null;
	
	private Vector2 direction = new Vector2(1.0f, 0.0f);
	public Direction eDirection = Direction.UP;
	
	
	void Start () {
		int rand = Random.Range(0, 4);
		if (rand == 0) {
			direction = new Vector2(-1.0f, 0.0f);
			eDirection = Direction.LEFT;
		}
		else if (rand == 1) {
			direction = new Vector2(1.0f, 0.0f);
			eDirection = Direction.RIGHT;	
		}
		else if (rand == 2) {
			direction = new Vector2(0.0f, 1.0f);
			eDirection = Direction.UP;	
		}
		else {
			direction = new Vector2(0.0f, -1.0f);
			eDirection = Direction.DOWN;
		}
				
		//direction = //new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
	}
	
	// Update is called once per frame
	void Update () {
		if (line.enabled == true) {
			Debug.DrawRay(transform.position, new Vector3(direction.x, 0.0f, direction.y));
			int layers = (1<<8) | (1<<9) | (1<<10) | (LayerMask.NameToLayer("LevelBounds") << 10);
			
			RaycastHit hitInfo = new RaycastHit();
			Vector3 direction3D = new Vector3(direction.x, 0.0f, direction.y);
			
			if (Physics.Raycast(new Ray(laserObject.transform.position, direction3D), out hitInfo, Mathf.Infinity, layers)) {
				Collider c = hitInfo.collider;
				
				line.SetPosition(0, transform.position);
				line.SetPosition(1, endLaser(c.transform.position));
				
				hitInfo.transform.SendMessage("HitByLaser", SendMessageOptions.DontRequireReceiver);
				
				if (c.gameObject.layer == 8) {
					//Debug.Log ("Player hit by Laser");	
					
				}
			}
			else {
			}
			//else {
			//	line.SetPosition(0, transform.position);
			//	line.SetPosition(1, transform.position + Vector3.Scale(direction, new Vector3(1000.0f, 1000.0f, 1000.0f)));
			//}
		}
	}
	
	Vector3 endLaser(Vector3 collidedPos) {
		if (eDirection == Direction.LEFT || eDirection == Direction.RIGHT) {
			return new Vector3 (collidedPos.x, 0.0f, transform.position.z);
		}
		else {
			return new Vector3 (transform.position.x, 0.0f, collidedPos.z);
		}
	}
	
	
}
