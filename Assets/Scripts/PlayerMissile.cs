using UnityEngine;
using System.Collections;

public class PlayerMissile : MonoBehaviour 
{
	// Has the missile been fired?
	private bool hasFired;
	
	// The player script.
	private Player player;
	
	// The enemies script.
	private Enemies enemies;
	
	// Use this for initialization
	void Start () 
	{		
		// The missile hasn't been fired yet...
		hasFired = false;
		
		// Find and set the player script.
		player = GameObject.Find("Tank").GetComponent<Player>();
		
		// Find and set the enemies script.
		enemies = Camera.main.GetComponent<Enemies>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		// If the game is currently playing.
		if (player.state.CurrentState == GameState.State.Play)
		{
			// Restart the missile!
			if (!hasFired)	
			{
				rigidbody.AddForce(0, 1750f, 0);
				hasFired = true;
			}
		}
		else
		{
			rigidbody.velocity = Vector3.zero;	
			hasFired = false;
		}
	}
	
	// Called when we collide with another object.
	void OnCollisionEnter(Collision collision)
	{
		// Check to see if we have collided with an enemy.
		Invader invader = collision.gameObject.GetComponent<Invader>();
		
		// If so...
		if (invader != null)
		{
			// Create an explosion!
			Instantiate (player.explosion, invader.transform.position, Quaternion.identity);
			
			// Tell the enemies script to remove the invader from the game.
			enemies.KillEnemy (invader);
		}
		// Otherwise, if we collided with a barrier brick...
		else if (collision.gameObject.name.Equals("Brick"))
		{
			// ...destroy the brick.
			Destroy (collision.gameObject);	
		}
		
		// Update the player script to know it can shoot again.
		player.HasShot = false;
		
		// Destroy the missile.
		Destroy (gameObject);
	}
}
