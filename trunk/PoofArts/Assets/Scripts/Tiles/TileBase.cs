using UnityEngine;
using System.Collections;

public class TileBase : MonoBehaviour
{
	public bool isLaser = false;
	
	
	public void Hide() {
		//gameObject.renderer.enabled = false;
		//if (gameObject.collider != null)
		//	gameObject.collider.enabled = false;
		Collider[] cs = gameObject.GetComponentsInChildren<Collider>();
		Renderer[] rs = gameObject.GetComponentsInChildren<Renderer>();
		LineRenderer[] ls = gameObject.GetComponentsInChildren<LineRenderer>();
		Light[] lgs = gameObject.GetComponentsInChildren<Light>();
		
		foreach(Collider c in cs) {
			c.enabled = false;	
		}
		
		foreach(Renderer r in rs) {
			r.enabled = false;	
		}
		
		foreach(LineRenderer l in ls) {
			l.enabled = false;	
		}
		
		foreach (Light l in lgs) {
			l.enabled = false;	
		}
		
		
	}
	
	public void Show() {
		Collider[] cs = gameObject.GetComponentsInChildren<Collider>();
		Renderer[] rs = gameObject.GetComponentsInChildren<Renderer>();
		LineRenderer[] ls = gameObject.GetComponentsInChildren<LineRenderer>();
		Light[] lgs = gameObject.GetComponentsInChildren<Light>();
		
		foreach(Collider c in cs) {
			c.enabled = true;	
		}
		
		foreach(Renderer r in rs) {
			r.enabled = true;	
		}
		
		foreach(LineRenderer l in ls) {
			l.enabled = true;	
		}
		
		foreach (Light l in lgs) {
			l.enabled = true;	
		}
	}
	
	public void OnBorder() {
		if(isLaser)
			Hide ();
		else
			Show();
	}
}

