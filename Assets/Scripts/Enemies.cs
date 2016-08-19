using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemies : MonoBehaviour 
{	
	// Enumerates the directions in which the invaders can move.
	public enum Direction
	{
		Right,
		Left,
		Down
	};
	
	// Tracks which movement sound was last played.
	private int sound;
	
	// The set of all living invaders.
	private HashSet<Invader> invaders;
	
	// How much each enemy should move at a given time.
	public UpdatingValue MovementAmount { get; set; }
	
	// The current direction in which the invaders are moving.
	public Direction currentDirection;
	
	// The last direction in which the invaders were moving.
	public Direction lastDirection;
	
	// Links to the three invader prefabs.
	public Transform invader1;
	public Transform invader2;
	public Transform invader3;
	
	// Links to the missile prefab.
	public Transform missile;
	
	// Links to the game's state.
	public GameState state;
	
	// A link to the sound manager.
	public Sounds sounds;
	
	// Use this for initialization
	void Start () 
	{			
		// Set the sound counter to the first movement sound.
		sound = 2;
		
		// Initialize the set of invaders.
		invaders = new HashSet<Invader> ();
		
		// Initialize the amount of space the invaders cover with each move.
		MovementAmount = new UpdatingValue (.5f);
		
		// Initialize the current movement direction.
		currentDirection = Direction.Right;
		
		// Initialize the last movement direction.
		lastDirection = Direction.Right;
		
		// Initialize the invaders on the game board.
		Initialize ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		// If the game is currently playing.
		if (state.CurrentState == GameState.State.Play)
		{
			// Check to see if it's time to move.
			if (state.LastUpdate + state.InvaderDelay.CurrentValue < Time.time) Move ();
		}
	}
	
	/// <summary>
	/// Initializes the invaders to their starting positions.
	/// </summary>
	private void Initialize ()
	{
		// Calculate the size of the game board.
		float totalSize = Mathf.Abs (state.BoardPositions.Minimum) + Mathf.Abs (state.BoardPositions.Maximum);
		
		// Store the space between adjacent invaders.
		float gapSize = .75f;
		
		// Calculate the size of a row in the invader swarm.
		float rowSize = (invader1.renderer.bounds.size.x * 11) + (gapSize * 10);
		
		// Calculate the difference between the swarm length and the board width.
		float difference = totalSize - rowSize;
		
		// Calculate the size of the gap between the row and the side of the screen.
		float sideGap = difference / 2;
		
		// Store the world position of the top row.
		float yPos = state.TopPosition;
		
		// Iterate through six rows of invaders.
		for (int i = 0; i < 6; i++)
		{
			// Iterate from the leftmost to rightmost invader.
			for (float xPos = state.BoardPositions.Minimum + sideGap + (invader1.renderer.bounds.size.x / 1.5f); xPos <= state.BoardPositions.Maximum - sideGap; xPos = xPos + invader1.renderer.bounds.size.x + gapSize)
			{
				// Create a transform for the invader.
				Transform invader;
				
				// Decide what type of invader to create, based on what row we are on.
				if (i < 2) invader = invader1;
				else if (i < 4) invader = invader2;
				else invader = invader3;
				
				// Create the invader in the game world and add its Invader script to our invaders collection.
				invaders.Add ((Instantiate (invader, new Vector3 (xPos, yPos, 0), Quaternion.identity) as Transform).GetComponent<Invader> ());
				
				// Let the game state know an enemy has been created.
				state.EnemiesLeft.IncreaseOriginalValue ();
			}
			
			// Move the y position down one row.
			yPos = yPos - invader1.renderer.bounds.size.y - (gapSize * 1.5f);
		}
	}
	
	/// <summary>
	/// Move the invaders.
	/// </summary>
	private void Move ()
	{		
		// If the swarm is not moving down...
		if (currentDirection != Direction.Down)
		{
			// ...iterate through each enemy...
			foreach (Invader invader in invaders)
			{
				// ...if the enemy is at the edge of the screen...
				if (invader.AtScreenEdge ())
				{
					// ...store the direction we are currently moving in.
					lastDirection = currentDirection;
					
					// Move everyone down!
					currentDirection = Direction.Down;
					
					// Exit loop.
					break;
				}
			}
		}
		else
		{
			// Otherwise, tell the swarm to move in the opposite direction.
			if (lastDirection == Direction.Right) currentDirection = Direction.Left;
			else currentDirection = Direction.Right;
		}
		
		// Move each invader on screen.
		foreach (Invader invader in invaders) invader.Move (currentDirection);
		
		// Update the movement time.
		state.LastUpdate = Time.time;
		
		// Play the movement sound.
		Beep ();
	}
	
	/// <summary>
	/// Plays the next moving sound.
	/// </summary>
	private void Beep ()
	{
		// Play the current moving sound.
		sounds.PlaySound (sound);
		
		// Update to the next sound.
		sound++;
		
		// If we passed the last sound, cycle back to the first.
		if (sound > 5) sound = 2;
	}
	
	/// <summary>
	/// Removes an enemy from the game.
	/// </summary>
	/// <param name='invader'>
	/// The Invader script of the enemy to remove.
	/// </param>
	public void KillEnemy (Invader invader)
	{
		// Remove the invader from our invaders collection.
		invaders.Remove (invader);
		
		// Destroy the enemy game object.
		Destroy (invader.gameObject);
		
		// Play the exploding enemy sound.
		sounds.PlaySound (1);	
		
		// Let the game state know an enemy has been destroyed.
		state.KillEnemy ();
		
		// Increase the space that each invader covers.
		MovementAmount.CurrentValue = MovementAmount.OriginalValue + (1f / state.EnemiesLeft.CurrentValue);
		
		// If that was the last invader, the player wins!
		if (state.EnemiesLeft.CurrentValue == 0) state.CurrentState = GameState.State.GameOver;
	}
}
