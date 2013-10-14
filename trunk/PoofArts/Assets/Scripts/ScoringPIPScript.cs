using UnityEngine;
using System.Collections;

public class ScoringPIPScript : MonoBehaviour 
{
	public float timeToFade = 1.0f;
	public float fadeStart = 0.5f;
	public float ySpeed = 0.1f;
	float interp = 0.0f;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		//Interpolate
		interp += Time.deltaTime;
		if(interp > timeToFade)
		{
			Destroy(gameObject);
		}	
		
		if(interp > fadeStart)
		{
			Color color = guiText.material.color;
			color.a = (1.0f - fadeStart) - (interp - fadeStart);
			guiText.material.color = color;
		}
		
		transform.Translate(Vector3.up * ySpeed * Time.deltaTime, Space.World);
			
		//Clone the text to all child shadows
		foreach(Transform t in transform)
		{
			if(t.guiText)
			{
				t.guiText.text = guiText.text;
				
				if(interp > fadeStart)
				{
					Color color = t.guiText.material.color;
					color.a = (1.0f - fadeStart) - (interp - fadeStart);
					t.guiText.material.color = color;
				}
			}
		}
	}
}
