using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour 
{
	// The states in which the game can be.
	public enum State
	{
		Play,
		Pause,
		GameOver
	};
	
	// The current state of the game.
	public State CurrentState { get; set; }
	
	// The last time the invaders moved.
	public float LastUpdate { get; set; }
	
	// The delay between movements performed by the invaders.
	public UpdatingValue InvaderDelay { get; set; }
	
	// The number of enemies left on the screen.
	public UpdatingValue EnemiesLeft { get; set; }
	
	// The range of world positions encapsulated by the game board.
	public Range<float> BoardPositions { get; set; }
	
	// The enemy starting position.
	public float TopPosition { get; set; }
	
	// Use this for initialization
	void Awake () 
	{
		//Initialize the current state.
		CurrentState = State.Pause;
		
		// Initialize the last update time to now.
		LastUpdate = Time.time;
		
		// Initialize the delay between enemy movement.
		InvaderDelay = new UpdatingValue (1);
		
		// Initialize the remaining enemies.
		EnemiesLeft = new UpdatingValue (0);
		
		// Set the size of the game board.
		BoardPositions = new Range<float>(-13f, 13f);
		
		// Set the enemy starting position.
		TopPosition = 13f;
	}
		
	/// <summary>
	/// Updates the speed once an enemy is shot.
	/// </summary>
	public void KillEnemy()
	{
		// Updates the enemy counter.
		EnemiesLeft.CurrentValue--;
		
		// Updates the speed at which the enemies transition.
		InvaderDelay.CurrentValue = InvaderDelay.OriginalValue * ((EnemiesLeft.CurrentValue + 2) / EnemiesLeft.OriginalValue);
	}
}
