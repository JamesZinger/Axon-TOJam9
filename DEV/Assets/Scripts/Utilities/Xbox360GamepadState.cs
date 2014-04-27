using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;


// This class is based on the assumption that positive x faces right, and positive y faces up.
#region Controller Enums

public enum XboxButton : byte
{
	A = 0,
	B,
	X,
	Y,
	Back,
	Start,
	LAnalogBtn,
	RAnalogBtn,
	LBumper,
	RBumper,
	LTrigger,
	RTrigger,
	NONE = Byte.MaxValue
};

public enum XboxAxis
{
	DPad,
	LAnalog,
	RAnalog
}

#endregion

public class Xbox360GamepadState
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

	public Dictionary<XboxAxis, Vector2> Axies;
	public Dictionary<XboxButton, bool> Buttons;

	public Dictionary<XboxAxis, Vector2>	prevAxies;
	public Dictionary<XboxButton, bool>		prevButtons;

	#endregion

	#region Constructor

	public Xbox360GamepadState()
	{
		Axies = new Dictionary<XboxAxis, Vector2>();
		prevAxies = new Dictionary<XboxAxis, Vector2>();
		Buttons = new Dictionary<XboxButton,bool>();
		prevButtons = new Dictionary<XboxButton,bool>();

		Axies.Add(XboxAxis.DPad,	Vector2.zero);		prevAxies.Add(XboxAxis.DPad,	Vector2.zero);
		Axies.Add(XboxAxis.LAnalog, Vector2.zero);		prevAxies.Add(XboxAxis.LAnalog, Vector2.zero);
		Axies.Add(XboxAxis.RAnalog, Vector2.zero);		prevAxies.Add(XboxAxis.RAnalog, Vector2.zero);

		Buttons.Add(XboxButton.A,			false);		prevButtons.Add(XboxButton.A,			false);
		Buttons.Add(XboxButton.B,			false);		prevButtons.Add(XboxButton.B,			false);
		Buttons.Add(XboxButton.Back,		false);		prevButtons.Add(XboxButton.Back,		false);
		Buttons.Add(XboxButton.LAnalogBtn,	false);		prevButtons.Add(XboxButton.LAnalogBtn,	false);
		Buttons.Add(XboxButton.LBumper,		false);		prevButtons.Add(XboxButton.LBumper,		false);
		Buttons.Add(XboxButton.LTrigger,	false);		prevButtons.Add(XboxButton.LTrigger,	false);
		Buttons.Add(XboxButton.RAnalogBtn,	false);		prevButtons.Add(XboxButton.RAnalogBtn,	false);
		Buttons.Add(XboxButton.RBumper,		false);		prevButtons.Add(XboxButton.RBumper,		false);
		Buttons.Add(XboxButton.RTrigger,	false);		prevButtons.Add(XboxButton.RTrigger,	false);
		Buttons.Add(XboxButton.Start,		false);		prevButtons.Add(XboxButton.Start,		false);
		Buttons.Add(XboxButton.X,			false);		prevButtons.Add(XboxButton.X,			false);
		Buttons.Add(XboxButton.Y,			false);		prevButtons.Add(XboxButton.Y,			false);
		Buttons.Add(XboxButton.NONE,		false);		prevButtons.Add(XboxButton.NONE,		false);

	}

	#endregion

	#region Get the current control state

	public void UpdateState()
	{
		prevAxies[XboxAxis.DPad]		= Axies[XboxAxis.DPad];
		prevAxies[XboxAxis.LAnalog]		= Axies[XboxAxis.LAnalog];
		prevAxies[XboxAxis.RAnalog]		= Axies[XboxAxis.RAnalog];


		prevButtons[XboxButton.A]			= Buttons[XboxButton.A];
		prevButtons[XboxButton.B]			= Buttons[XboxButton.B];
		prevButtons[XboxButton.X]			= Buttons[XboxButton.X];
		prevButtons[XboxButton.Y]			= Buttons[XboxButton.Y];
		prevButtons[XboxButton.Back]		= Buttons[XboxButton.Back];
		prevButtons[XboxButton.Start]		= Buttons[XboxButton.Start];
		prevButtons[XboxButton.LAnalogBtn]	= Buttons[XboxButton.LAnalogBtn];
		prevButtons[XboxButton.RAnalogBtn]	= Buttons[XboxButton.RAnalogBtn];
		prevButtons[XboxButton.LBumper]		= Buttons[XboxButton.LBumper];
		prevButtons[XboxButton.RBumper]		= Buttons[XboxButton.RBumper];
		prevButtons[XboxButton.RTrigger]	= Buttons[XboxButton.RTrigger];
		prevButtons[XboxButton.LTrigger]	= Buttons[XboxButton.LTrigger];

		// Read in the control axes
		Axies[XboxAxis.DPad].Set(		 Input.GetAxis( MAP_DPAD_X ),    Input.GetAxis( MAP_DPAD_Y ) );
		Axies[XboxAxis.LAnalog].Set(	 Input.GetAxis( MAP_LANALOG_X ), Input.GetAxis( MAP_LANALOG_Y ) );
		Axies[XboxAxis.RAnalog].Set(	 Input.GetAxis( MAP_RANALOG_X ), Input.GetAxis( MAP_RANALOG_Y ) );
		Buttons[XboxButton.LTrigger]	= Input.GetAxis( MAP_TRIGGER ) > 0f;
		Buttons[XboxButton.RTrigger]	= Input.GetAxis( MAP_TRIGGER ) < 0f;

		// Read in each of the buttons
		Buttons[XboxButton.A]			= Input.GetButton( MAP_A );
		Buttons[XboxButton.B]			= Input.GetButton( MAP_B );
		Buttons[XboxButton.X]			= Input.GetButton( MAP_X );
		Buttons[XboxButton.Y]			= Input.GetButton( MAP_Y );
		Buttons[XboxButton.Back]		= Input.GetButton( MAP_BACK );
		Buttons[XboxButton.Start]		= Input.GetButton( MAP_START );
		Buttons[XboxButton.LAnalogBtn]	= Input.GetButton( MAP_LANALOG_BTN );
		Buttons[XboxButton.RAnalogBtn]	= Input.GetButton( MAP_RANALOG_BTN );
		Buttons[XboxButton.LBumper]		= Input.GetButton( MAP_LBUMPER );
		Buttons[XboxButton.RBumper]		= Input.GetButton( MAP_RBUMPER );

	}

	#endregion

	#region ButtonDown / ButtonUp

	public bool GetButtonDown( XboxButton b )
	{
		if ( Buttons[ b ] == true && prevButtons[ b ] == false )
		{
			return true;
		}

		return false;
	}

	public bool GetButtonUp( XboxButton b )
	{
		if ( Buttons[ b ] == false && prevButtons[ b ] == true )
		{
			return true;
		}

		return false;
	}

	#endregion
}
