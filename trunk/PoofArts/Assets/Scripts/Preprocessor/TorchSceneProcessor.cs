using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("SceneProcessors/TorchSceneProcessor")]
public class TorchSceneProcessor : Preprocessor {
	public GameObject torchPrefab = null;
	
	
	public void process(WorldScript world) {
		ClearPlayerSpot();
		ClearVisibleEnemies();
		MinMaxTile("Torch", 1, 1, torchPrefab, null, true);
		AddDoor();
	}
}
