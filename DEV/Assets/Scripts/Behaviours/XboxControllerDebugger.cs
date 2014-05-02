using UnityEngine;
using System.Collections;

public class XboxControllerDebugger : MonoBehaviour
{

	Xbox360GamepadState state;

	// Use this for initialization
	void Start ()
	{
		state = new Xbox360GamepadState();
		string[] joysticks = Input.GetJoystickNames();
		foreach ( string joystick in joysticks )
		{
			Debug.Log( "Joystick: " + joystick );
			
		}
	}

	// Update is called once per frame
	void Update ()
	{
		state.UpdateState();
		//Debug.Log( state.ToString() );
		
		foreach (Xbox.Axis key in state.Axes.Keys)
		{
			if ( state.Axes[ key ] != Vector2.zero )
				Debug.Log( "Axis " + key.ToString() + ": " + state.Axes[ key ].ToString() );
		}

		foreach ( Xbox.Button key in state.Buttons.Keys )
		{
			if ( state.GetButtonDown( key ) )
				Debug.Log( "Button " + key.ToString() + ": " + state.Buttons[ key ].ToString() );
		}

		foreach ( Xbox.Trigger key in state.Triggers.Keys )
		{
			if ( state.Triggers[ key ] != 0f )
				Debug.Log( "Trigger " + key.ToString() + ": " + state.Triggers[ key ].ToString() );
			
		}
	}
}
