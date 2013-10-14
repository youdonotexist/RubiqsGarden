using UnityEngine;
using System.Collections;

public class LaserTile : TileBase
{/*
	public override void Hide() {
		//gameObject.renderer.enabled = false;
		//if (gameObject.collider != null)
		//	gameObject.collider.enabled = false;
		Collider[] cs = gameObject.GetComponentsInChildren<Collider>();
		Renderer[] rs = gameObject.GetComponentsInChildren<Renderer>();
		LineRenderer[] ls = gameObject.GetComponentsInChildren<LineRenderer>();
		
		foreach(Collider c in cs) {
			c.enabled = false;	
		}
		
		foreach(Renderer r in rs) {
			r.enabled = false;	
		}
		
		foreach(LineRenderer l in ls) {
			l.enabled = false;	
		}
		
		
	}
	
	public override void Show() {
		Collider[] cs = gameObject.GetComponentsInChildren<Collider>();
		Renderer[] rs = gameObject.GetComponentsInChildren<Renderer>();
		LineRenderer[] ls = gameObject.GetComponentsInChildren<LineRenderer>();
		
		foreach(Collider c in cs) {
			c.enabled = true;	
		}
		
		foreach(Renderer r in rs) {
			r.enabled = true;	
		}
		
		foreach(LineRenderer l in ls) {
			l.enabled = true;	
		}
	}
	
	public override void OnBorder() {
		Hide ();
	}*/
}

