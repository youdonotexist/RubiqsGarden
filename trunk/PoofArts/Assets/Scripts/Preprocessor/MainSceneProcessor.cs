using UnityEngine;
using System.Collections;


[AddComponentMenu("SceneProcessors/MainSceneProcessor")]
public class MainSceneProcessor : Preprocessor {

	public void process(WorldScript world) {
		worldScript = world;
		
		Debug.Log ("Processing..");
		ClearPlayerSpot();
		ClearVisibleEnemies();
		AddDoor();
	}
}
