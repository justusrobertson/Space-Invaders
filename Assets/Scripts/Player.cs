using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{	
	// The speed at which the player should move.
	private int speed;
	
	// Tracks whether there is a player missile on the board.
	public bool HasShot { get; set; }
	
	// The game state.
	// Set this in the editor.
	public GameState state;
	
	// A link to the missile transform.
	public Transform missile;
	
	// A link to the explosion transform.
	public Transform explosion;
	
	// A link to the sound manager.
	public Sounds sounds;
	
	// Use this for initialization
	void Start ()
	{
		// The player has not yet fired when the game begins.
		HasShot = false;	
		
		// Set the player's speed.
		speed = 10;
	}
	
	// Update is called once per frame
	void Update () 
	{
		// If the game is currently playing.
		if (state.CurrentState == GameState.State.Play)
		{
			CheckInput ();
			CheckBoundary ();
		}
	}
	
	/// <summary>
	/// Checks for and responds to user input.
	/// </summary>
	private void CheckInput ()
	{
		if (Input.GetButton("Left"))
		{
			transform.Translate(speed * Vector3.left * Time.deltaTime);
		}
		
		if (Input.GetButton("Right"))
		{
			transform.Translate(speed * Vector3.right * Time.deltaTime);
		}
		
		if (Input.GetButtonDown ("Fire"))
		{
			FireMissile();	
		}
	}
	
	/// <summary>
	/// Check to see if the player has reached the board's boudnary.
	/// </summary>
	private void CheckBoundary ()
	{
		if (transform.position.x > state.BoardPositions.Maximum) transform.position = new Vector3(state.BoardPositions.Maximum, transform.position.y, transform.position.z);
		if (transform.position.x < state.BoardPositions.Minimum) transform.position = new Vector3(state.BoardPositions.Minimum, transform.position.y, transform.position.z);
	}
		
	/// <summary>
	/// Fires a missile.
	/// </summary>
	private void FireMissile()
	{
		// Only one missile can be onscreen at a time...
		if (!HasShot)
		{
			// Record that a missile has been fired.
			HasShot = true;
			
			// Play the missile sound.
			sounds.PlaySound(0);
			
			// Create a new instance of the missile prefab at the current position.
			Instantiate (missile, new Vector3 (transform.position.x, transform.position.y + renderer.bounds.size.y, 0), Quaternion.identity);
		}
	}
	
	/// <summary>
	/// The tank was hit!
	/// </summary>
	public void Die ()
	{
		// Create an explosion!
		Instantiate (explosion, transform.position, Quaternion.identity);
		
		// Destroy the tank
		Destroy (gameObject);
		
		// Play the explosion sound.
		sounds.PlaySound(6);		
		
		// Game over, man. Game over!
		state.CurrentState = GameState.State.GameOver;
	}
}