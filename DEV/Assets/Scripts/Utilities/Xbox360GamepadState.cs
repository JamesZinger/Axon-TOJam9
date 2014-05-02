using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;


// This class is based on the assumption that positive x faces right, and positive y faces up.
#region Controller Enums
namespace Xbox
{
	public enum Button : byte
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
		NONE = Byte.MaxValue
	};

	public enum Axis : byte
	{
		DPadX = 0,
		DPadY,
		LAnalogX,
		LAnalogY,
		RAnalogX,
		RAnalogY,
		TriggerR,
		TriggerL,
		NONE = Byte.MaxValue
	}
}
#endregion

public class Xbox360GamepadState
{
	#region Input class mappings

	public const string MAP_DPAD_X		= "DPad_XAxis_1";
	public const string MAP_DPAD_Y		= "DPad_YAxis_1";
	public const string MAP_LANALOG_X	= "L_XAxis_1";
	public const string MAP_LANALOG_Y	= "L_YAxis_1";
	public const string MAP_RANALOG_X	= "R_XAxis_1";
	public const string MAP_RANALOG_Y	= "R_YAxis_1";
	public const string MAP_TRIGGER_R   = "TriggersR_1";
	public const string MAP_TRIGGER_L   = "TriggersL_1";
	public const string MAP_A			= "A_1";
	public const string MAP_B			= "B_1";
	public const string MAP_X			= "X_1";
	public const string MAP_Y			= "Y_1";
	public const string MAP_BACK		= "Back_1";
	public const string MAP_START		= "Start_1";
	public const string MAP_LANALOG_BTN = "LS_1";
	public const string MAP_RANALOG_BTN = "RS_1";
	public const string MAP_LBUMPER	    = "LB_1";
	public const string MAP_RBUMPER		= "RB_1";
	//public const string MAP_AXIS_1		= "Axis_1";
	//public const string MAP_AXIS_2		= "Axis_2";
	//public const string MAP_AXIS_3		= "Axis_3";
	//public const string MAP_AXIS_4		= "Axis_4";
	//public const string MAP_AXIS_5		= "Axis_5";
	//public const string MAP_AXIS_6		= "Axis_6";
	//public const string MAP_AXIS_7		= "Axis_7";
	//public const string MAP_AXIS_8		= "Axis_8";
	//public const string MAP_AXIS_9		= "Axis_9";
	//public const string MAP_AXIS_10		= "Axis_10";

	#endregion

	#region Control axis/button values

	public Dictionary<Xbox.Axis, float> Axes
	{
		get { return axes; }
		private set { axes = value; }
	}

	public Dictionary<Xbox.Button, bool> Buttons
	{
		get { return buttons; }
		private set { buttons = value; }
	}

	private Dictionary<Xbox.Axis, float> PrevAxes
	{
		get { return prevAxes; }
		set { prevAxes = value; }
	}

	private Dictionary<Xbox.Button, bool> PrevButtons
	{
		get { return prevButtons; }
		set { prevButtons = value; }
	}

	private Dictionary<Xbox.Axis, float>	axes;
	private Dictionary<Xbox.Button, bool>	buttons;
	private Dictionary<Xbox.Axis, float>	prevAxes;
	private Dictionary<Xbox.Button, bool>	prevButtons;

	#endregion

	#region Constructor

	public Xbox360GamepadState()
	{
		Axes			= new Dictionary<Xbox.Axis,		float>		();
		PrevAxes		= new Dictionary<Xbox.Axis,		float>		();
		Buttons			= new Dictionary<Xbox.Button,	bool>		();
		PrevButtons		= new Dictionary<Xbox.Button,	bool>		();

		Axes.Add( Xbox.Axis.DPadX,		0f );				PrevAxes.Add( Xbox.Axis.DPadX,		0f );
		Axes.Add( Xbox.Axis.DPadY,		0f );				PrevAxes.Add( Xbox.Axis.DPadY,		0f );
		Axes.Add( Xbox.Axis.LAnalogX,	0f );				PrevAxes.Add( Xbox.Axis.LAnalogX,	0f );
		Axes.Add( Xbox.Axis.LAnalogY,	0f );				PrevAxes.Add( Xbox.Axis.LAnalogY,	0f );
		Axes.Add( Xbox.Axis.RAnalogX,	0f );				PrevAxes.Add( Xbox.Axis.RAnalogX,	0f );
		Axes.Add( Xbox.Axis.RAnalogY,	0f );				PrevAxes.Add( Xbox.Axis.RAnalogY,	0f );
		Axes.Add( Xbox.Axis.TriggerL,	0f );				PrevAxes.Add( Xbox.Axis.TriggerL,	0f );
		Axes.Add( Xbox.Axis.TriggerR,	0f );				PrevAxes.Add( Xbox.Axis.TriggerR,	0f );
		
		Buttons.Add( Xbox.Button.A,				false );	PrevButtons.Add( Xbox.Button.A,				false );
		Buttons.Add( Xbox.Button.B,				false );	PrevButtons.Add( Xbox.Button.B,				false );
		Buttons.Add( Xbox.Button.Back,			false );	PrevButtons.Add( Xbox.Button.Back,			false );
		Buttons.Add( Xbox.Button.LAnalogBtn,	false );	PrevButtons.Add( Xbox.Button.LAnalogBtn,	false );
		Buttons.Add( Xbox.Button.LBumper,		false );	PrevButtons.Add( Xbox.Button.LBumper,		false );
		Buttons.Add( Xbox.Button.RAnalogBtn,	false );	PrevButtons.Add( Xbox.Button.RAnalogBtn,	false );
		Buttons.Add( Xbox.Button.RBumper,		false );	PrevButtons.Add( Xbox.Button.RBumper,		false );
		Buttons.Add( Xbox.Button.Start,			false );	PrevButtons.Add( Xbox.Button.Start,			false );
		Buttons.Add( Xbox.Button.X,				false );	PrevButtons.Add( Xbox.Button.X,				false );
		Buttons.Add( Xbox.Button.Y,				false );	PrevButtons.Add( Xbox.Button.Y,				false );

	}

	static Xbox360GamepadState()
	{
		Instance = null;
	}

	#endregion

	#region Get the current control state

	public void UpdateState()
	{
		foreach ( Xbox.Axis key in Axes.Keys )
		{
			PrevAxes[ key ] = Axes[ key ];
		}

		foreach ( Xbox.Button key in Buttons.Keys )
		{
			PrevButtons[ key ] = Buttons[ key ];
		}


		
		// Read in the control axes
		Axes[ Xbox.Axis.DPadX ]    		  = Input.GetAxis( MAP_DPAD_X );
		Axes[ Xbox.Axis.DPadY ]    		  = Input.GetAxis( MAP_DPAD_Y );
		Axes[ Xbox.Axis.LAnalogX ] 		  = Input.GetAxis( MAP_LANALOG_X );
		Axes[ Xbox.Axis.LAnalogY ] 		  = Input.GetAxis( MAP_LANALOG_Y );
		Axes[ Xbox.Axis.RAnalogX ] 		  = Input.GetAxis( MAP_RANALOG_X );
		Axes[ Xbox.Axis.RAnalogY ] 		  = Input.GetAxis( MAP_RANALOG_Y );
		Axes[ Xbox.Axis.TriggerL ] 		  = Input.GetAxis( MAP_TRIGGER_L );
		Axes[ Xbox.Axis.TriggerR ] 		  = Input.GetAxis( MAP_TRIGGER_R );


		// Read in each of the buttons
		Buttons[ Xbox.Button.A ]          = Input.GetButton( MAP_A );
		Buttons[ Xbox.Button.B ]          = Input.GetButton( MAP_B );
		Buttons[ Xbox.Button.X ]          = Input.GetButton( MAP_X );
		Buttons[ Xbox.Button.Y ]          = Input.GetButton( MAP_Y );
		Buttons[ Xbox.Button.Back ]       = Input.GetButton( MAP_BACK );
		Buttons[ Xbox.Button.Start ]      = Input.GetButton( MAP_START );
		Buttons[ Xbox.Button.LAnalogBtn ] = Input.GetButton( MAP_LANALOG_BTN );
		Buttons[ Xbox.Button.RAnalogBtn ] = Input.GetButton( MAP_RANALOG_BTN );
		Buttons[ Xbox.Button.LBumper ]    = Input.GetButton( MAP_LBUMPER );
		Buttons[ Xbox.Button.RBumper ]    = Input.GetButton( MAP_RBUMPER );

	}

	#endregion

	#region ButtonDown / ButtonUp

	public bool IsButtonDown( Xbox.Button b )
	{
		if ( Buttons[ b ] == true && prevButtons[ b ] == false )
		{
			return true;
		}

		return false;
	}

	public bool IsButtonUp( Xbox.Button b )
	{
		if ( Buttons[ b ] == false && prevButtons[ b ] == true )
		{
			return true;
		}

		return false;
	}

	#endregion

	#region Axis Threshold Functions

	/// <summary>	Check if an axis is past a threshold and was not the check before. </summary>
	/// <remarks>	James, 2014-05-02. </remarks>
	/// <param name="Axis">		 	The axis to check. </param>
	/// <param name="Threshold">	The threashold between 0 and 1. </param>
	/// <returns>
	/// 	true if the axis is past the threshold and previously was not, otherwise false.
	/// </returns>
	public bool AxisJustPastThreshold( Xbox.Axis Axis, float Threshold )
	{
		float threshold = Mathf.Clamp( Threshold, -1f, 1f );

		if ( threshold > 0 )
		{
			if ( Axes[ Axis ] >= threshold && PrevAxes[ Axis ] < threshold )
				return true;
		}
		else
		{
			if ( Axes[ Axis ] < threshold && PrevAxes[ Axis ] > threshold )
				return true;
		}
		
		return false;
	}

	#endregion

	#region Singleton Stuff

	private static Xbox360GamepadState instance;

	public static Xbox360GamepadState Instance
	{
		get
		{
			if ( instance == null )
			{
				instance = new Xbox360GamepadState();
			}

			return instance;
		}
		private set { instance = value; }
	}


	#endregion

	public override string ToString()
	{

		StringBuilder sb = new StringBuilder();

		sb.AppendLine( "Xbox 360 Game pad State\n\n" );
		sb.AppendLine( "Axes\n" );

		foreach ( Xbox.Axis key in Axes.Keys )
		{
			sb.AppendLine( key.ToString() + ": \t\t" + Axes[ key ].ToString() );
		}

		sb.AppendLine( "" );
		sb.AppendLine( "Buttons\n" );

		foreach ( Xbox.Button key in Buttons.Keys )
		{
			sb.AppendLine( key.ToString() + ": \t\t" + Buttons[ key ].ToString() );
		}
		sb.AppendLine("");

		return sb.ToString();
	}
}
