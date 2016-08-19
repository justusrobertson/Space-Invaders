using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
	{
		// The object has exploded! Queue its death sequence.
		StartCoroutine (Kill ());
	}
	
	/// <summary>
	/// Kill the game object.
	/// </summary>
	IEnumerator Kill ()
	{
		// Wait for the explosion to take place.
		yield return new WaitForSeconds(.5f);
		
		// Destroy the explosion!
		Destroy (gameObject);
	}
}
