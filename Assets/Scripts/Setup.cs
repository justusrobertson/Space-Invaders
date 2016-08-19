using UnityEngine;
using System.Collections;

public class Setup : MonoBehaviour 
{
	// Link to game state.
	private GameState state;
	
	// Links to game objects.
	public GameObject bottomLine;
	public GameObject player;
	public GameObject barricade;
	
	// Colors.
	private Color tealArcade = new Color(0, 231, 255, 1);
	private Color greenArcade = new Color(32, 255, 32, 1);
	
	// Use this for initialization
	void Start () 
	{
		// Grab the game state script.
		state = Camera.main.GetComponent<GameState>();
		
		// Lock the cursor.
		Screen.lockCursor = false;
		
		// Hide the cursor.
		Screen.showCursor = true;
		
		// Choose a color.
		Color myColor = this.tealArcade;
		
		// Color the bottom line.
		bottomLine.renderer.material.color = myColor;
		
		// Color the player.
		player.renderer.material.color = myColor;
		
		// Get all the blocks in the barricade.
		Transform[] allChildren = barricade.GetComponentsInChildren<Transform>();
		
		// For each block in the barricade...
		foreach (Transform child in allChildren)
		{
			// ...if its being rendered, color it.
			if (child.renderer) child.renderer.material.color = myColor;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		CheckCursor ();
	}
	
	/// <summary>
	/// Checks to see if the user has issued a cursor command.
	/// </summary>
	private void CheckCursor ()
	{
		// If the user presses escape...
		if (Input.GetKeyDown (KeyCode.Escape))
		{
			// ...toggle the cursor lock and visibility.
			Screen.lockCursor = !Screen.lockCursor;
			Screen.showCursor = !Screen.showCursor;
			
			if (state.CurrentState == GameState.State.Play) state.CurrentState = GameState.State.Pause;
			else if (state.CurrentState == GameState.State.Pause) state.CurrentState = GameState.State.Play;
		}
		
		if (!Screen.showCursor) Screen.lockCursor = true;
	}
}
