using UnityEngine;
using System.Collections;

public class EnemyMissile : MonoBehaviour
{	
	// Has the missile been fired?
	private bool hasFired;
	
	// A link to the game state.
	private GameState state;
	
	// Use this for initialization
	void Start () 
	{
		// The missile hasn't been fired yet...
		hasFired = false;
		
		// Grab the game state.
		state = Camera.main.GetComponent<GameState>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		// If the game is currently playing.
		if (state.CurrentState == GameState.State.Play)
		{
			// Restart the missile!
			if (!hasFired)	
			{
				rigidbody.AddForce(0, -500f, 0);
				hasFired = true;
			}
		}
		else
		{
			rigidbody.velocity = Vector3.zero;
			hasFired = false;
		}
	}
	
	// Called when we collide with another game object.
	void OnCollisionEnter(Collision collision)
	{
		// Check to see if we collided with the player.
		Player player = collision.gameObject.GetComponent<Player>();
		
		// If so...
		if (player != null)
		{
			player.Die ();
		}
		// Otherwise, if we collided with a barrier brick...
		else if (collision.gameObject.name.Equals("Brick"))
		{
			// Destroy the barrier!
			Destroy (collision.gameObject);	
		}
		
		// Destroy the missile.
		Destroy (gameObject);
	}
}