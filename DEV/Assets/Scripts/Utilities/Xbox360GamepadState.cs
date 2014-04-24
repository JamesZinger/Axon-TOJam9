using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;


// This class is based on the assumption that positive x faces right, and positive y faces up.

public struct Xbox360GamepadState
{
	#region Input class mappings

	public static string MAP_DPAD_X		= "DPadHorizontal";
	public static string MAP_DPAD_Y		= "DPadVertical";
	public static string MAP_LANALOG_X	= "LSHorizontal";
	public static string MAP_LANALOG_Y	= "LSVertical";
	public static string MAP_RANALOG_X	= "RSHorizontal";
	public static string MAP_RANALOG_Y	= "RSVertical";
	public static string MAP_TRIGGER	= "Trigger";
	public static string MAP_A			= "A";
	public static string MAP_B			= "B";
	public static string MAP_X			= "X";
	public static string MAP_Y			= "Y";
	public static string MAP_BACK		= "Back";
	public static string MAP_START		= "Start";
	public static string MAP_LANALOG_BTN = "LSButton";
	public static string MAP_RANALOG_BTN = "RSButton";
	public static string MAP_LBUMPER	= "LBumper";
	public static string MAP_RBUMPER	= "RBumper";

	#endregion

	#region Control axis/button values

	// Axes
	public Vector2	DPad;
	public Vector2	LAnalog;
	public Vector2	RAnalog;
	
	// Buttons
	public bool		A;
	public bool		B;
	public bool		X;
	public bool		Y;
	public bool		Back;
	public bool		Start;
	public bool		LAnalogBtn;
	public bool		RAnalogBtn;
	public bool		LBumper;
	public bool		RBumper;
	public bool		LTrigger;
	public bool		RTrigger;

	#endregion

	#region Constructor

	//public Xbox360GamepadState()
	//{
	//    DPad		 = Vector2.zero;
	//    LAnalog		 = Vector2.zero;
	//    RAnalog		 = Vector2.zero;
	//    A			 = false;
	//    B			 = false;
	//    X			 = false;
	//    Y			 = false;
	//    Back		 = false;
	//    Start		 = false;
	//    LAnalogBtn	 = false;
	//    RAnalogBtn	 = false;
	//    LBumper		 = false;
	//    RBumper		 = false;
	//    LTrigger	 = false;
	//    RTrigger	 = false;
	//}

	#endregion

	#region Get the current control state

	public Xbox360GamepadState GetState()
	{
		// Read in the control axes
		DPad.Set(	  Input.GetAxis( MAP_DPAD_X ),    Input.GetAxis( MAP_DPAD_Y ) );
		LAnalog.Set(  Input.GetAxis( MAP_LANALOG_X ), Input.GetAxis( MAP_LANALOG_Y ) );
		RAnalog.Set(  Input.GetAxis( MAP_RANALOG_X ), Input.GetAxis( MAP_RANALOG_Y ) );
		LTrigger	= Input.GetAxis( MAP_TRIGGER ) > 0f;
		RTrigger	= Input.GetAxis( MAP_TRIGGER ) < 0f;

		// Read in each of the buttons
		A			= Input.GetButton( MAP_A );
		B			= Input.GetButton( MAP_B );
		X			= Input.GetButton( MAP_X );
		Y			= Input.GetButton( MAP_Y );
		Back		= Input.GetButton( MAP_BACK );
		Start		= Input.GetButton( MAP_START );
		LAnalogBtn	= Input.GetButton( MAP_LANALOG_BTN );
		RAnalogBtn	= Input.GetButton( MAP_RANALOG_BTN );
		LBumper		= Input.GetButton( MAP_LBUMPER );
		RBumper		= Input.GetButton( MAP_RBUMPER );

		// Return the refreshed control state
		return this;
	}

	#endregion
}
