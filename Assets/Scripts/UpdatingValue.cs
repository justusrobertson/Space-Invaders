using UnityEngine;
using System.Collections;

public class UpdatingValue
{
	// The initialization value.
	public float OriginalValue { get; set; }
	
	// The current value.
	public float CurrentValue { get; set; }
	
	/// <summary>
	/// The constructor.
	/// </summary>
	/// <param name='val'>
	/// The initial value.
	/// </param>
	public UpdatingValue (float val)
	{
		// Initialize both values to val.
		OriginalValue = val;
		CurrentValue = val;
	}
	
	/// <summary>
	/// Updates the original value.
	/// </summary>
	public void IncreaseOriginalValue ()
	{
		OriginalValue++;
		CurrentValue++;
	}
}

