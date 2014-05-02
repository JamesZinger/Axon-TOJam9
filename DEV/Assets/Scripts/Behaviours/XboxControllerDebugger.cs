using UnityEngine;
using System.Collections;

public class XboxControllerDebugger : MonoBehaviour
{

	Xbox360GamepadState state;

	// Use this for initialization
	void Start ()
	{
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
		
		foreach (Xbox.Axis key in state.Axes.Keys)
		{
			if ( state.Axes[ key ] != 0 )
				Debug.Log( "Axis " + key.ToString() + ": " + state.Axes[ key ].ToString() );
		}

		foreach ( Xbox.Button key in state.Buttons.Keys )
		{
			if ( state.IsButtonDown( key ) )
				Debug.Log( "Button " + key.ToString() + ": " + state.Buttons[ key ].ToString() );
		}

	}
}
