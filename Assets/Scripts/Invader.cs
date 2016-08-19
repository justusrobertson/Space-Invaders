using UnityEngine;
using System.Collections;

public class Invader : MonoBehaviour 
{	
	// Used for deciding whether to fire.
	private int fireProbability;
	
	// A link to the enemies script.
	// This script is used to coordinate all bad guys.
	private Enemies enemies;
	
	// Use this for initialization
	void Start () 
	{			
		// Initialize the pool of numbers used to decide whether to fire.
		fireProbability = 1000;
		
		// Grab the enemies script from the Main Camera.
		enemies = Camera.mainCamera.GetComponent<Enemies>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		// If the game is currently playing.
		if (enemies.state.CurrentState == GameState.State.Play)
		{
			// Decide whether the enemy should shoot.
			if (Random.Range (0, fireProbability) == 87) Shoot();	
		}
	}
	
	/// <summary>
	/// Update the enemy's position.
	/// </summary>
	public void Move (Enemies.Direction direction)
	{
		// If the swarm is moving right...
		if (direction == Enemies.Direction.Right) iTween.MoveTo(gameObject, transform.position + (enemies.MovementAmount.CurrentValue * Vector3.right), enemies.state.InvaderDelay.CurrentValue);	
		
		// If the swarm is moving left...
		if (direction == Enemies.Direction.Left) iTween.MoveTo(gameObject, transform.position + (enemies.MovementAmount.CurrentValue * Vector3.left), enemies.state.InvaderDelay.CurrentValue);
		
		// If the swarm is moving down...
		if (direction == Enemies.Direction.Down) iTween.MoveTo(gameObject, new Vector3(transform.position.x, transform.position.y - .75f, transform.position.z), enemies.state.InvaderDelay.CurrentValue);
		
		// Animate the enemy.
		renderer.material.SetTextureOffset("_MainTex", new Vector2 (Mathf.Abs (0.5f - renderer.material.mainTextureOffset.x), 0));
	}
	
	/// <summary>
	/// Checks to see if the invader is at the screen's edge.
	/// </summary>
	/// <returns>
	/// Whether or not the invader is at the screen's edge.
	/// </returns>
	public bool AtScreenEdge ()
	{
		// Are we at the edge of the screen? If so, return true.
		if (transform.position.x >= 13f || transform.position.x <= -13f) return true;
		
		// If not, return false!
		return false;
	}
	
	/// <summary>
	/// Fire a missile at the player.
	/// </summary>
	private void Shoot ()
	{
		// Create a new instance of the enemy missile prefab.
		Instantiate (enemies.missile, transform.position, Quaternion.identity);	
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
	}
}
