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

	public enum Trigger : byte
	{
		Right = 0,
		Left,
		NONE = Byte.MaxValue
	}

	public enum Axis : byte
	{
		DPad = 0,
		LAnalog,
		RAnalog,
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

	public Dictionary<Xbox.Axis, Vector2> Axes
	{
		get { return axes; }
		private set { axes = value; }
	}

	public Dictionary<Xbox.Button, bool> Buttons
	{
		get { return buttons; }
		private set { buttons = value; }
	}

	public Dictionary<Xbox.Trigger, float> Triggers
	{
		get { return triggers; }
		private set { triggers = value; }
	}

	public Dictionary<Xbox.Axis, Vector2> PrevAxes
	{
		get { return prevAxes; }
		private set { prevAxes = value; }
	}

	public Dictionary<Xbox.Button, bool> PrevButtons
	{
		get { return prevButtons; }
		private set { prevButtons = value; }
	}

	public Dictionary<Xbox.Trigger, float> PrevTrggers
	{
		get { return prevTriggers; }
		private set { prevTriggers = value; }
	}

	public float[] DebugAxes
	{
		get { return debugAxes; }
		private set { debugAxes = value; }
	}

	private Dictionary<Xbox.Axis, Vector2>	axes;
	private Dictionary<Xbox.Button, bool>	buttons;
	private Dictionary<Xbox.Trigger, float> triggers;
	private Dictionary<Xbox.Axis, Vector2>	prevAxes;
	private Dictionary<Xbox.Button, bool>	prevButtons;
	private Dictionary<Xbox.Trigger, float> prevTriggers;
	private float[]							debugAxes;
	
	#endregion

	#region Constructor

	public Xbox360GamepadState()
	{
		Axes			= new Dictionary<Xbox.Axis,		Vector2>	();
		PrevAxes		= new Dictionary<Xbox.Axis,		Vector2>	();
		Buttons			= new Dictionary<Xbox.Button,	bool>		();
		PrevButtons		= new Dictionary<Xbox.Button,	bool>		();
		Triggers		= new Dictionary<Xbox.Trigger,	float>		();
		PrevTrggers		= new Dictionary<Xbox.Trigger,	float>		();

		Axes.Add( Xbox.Axis.DPad,		Vector2.zero );		PrevAxes.Add( Xbox.Axis.DPad,		Vector2.zero );
		Axes.Add( Xbox.Axis.LAnalog,	Vector2.zero );		PrevAxes.Add( Xbox.Axis.LAnalog,	Vector2.zero );
		Axes.Add( Xbox.Axis.RAnalog,	Vector2.zero );		PrevAxes.Add( Xbox.Axis.RAnalog,	Vector2.zero );
		
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

		Triggers.Add( Xbox.Trigger.Left,			0f );	PrevTrggers.Add( Xbox.Trigger.Left,				0f );
		Triggers.Add( Xbox.Trigger.Right,			0f );	PrevTrggers.Add( Xbox.Trigger.Right,			0f );

		DebugAxes = new float[ 10 ];
		for ( int i = 0; i < 10; i++ )
		{
			DebugAxes[ i ] = 0f;
		}
	}

	#endregion

	#region Get the current control state

	public void UpdateState()
	{
		foreach ( Xbox.Axis key in Axes.Keys )
		{
			PrevAxes[ key ] = Axes[ key ];
		}

		foreach ( Xbox.Trigger key in Triggers.Keys )
		{
			PrevTrggers[ key ] = Triggers[ key ];
		}

		foreach ( Xbox.Button key in Buttons.Keys )
		{
			PrevButtons[ key ] = Buttons[ key ];
		}


		
		// Read in the control axes
		Axes[ Xbox.Axis.DPad ]    = new Vector2( Input.GetAxis( MAP_DPAD_X )   , Input.GetAxis( MAP_DPAD_Y ) );
		Axes[ Xbox.Axis.LAnalog ] = new Vector2( Input.GetAxis( MAP_LANALOG_X ), Input.GetAxis( MAP_LANALOG_Y ) );
		Axes[ Xbox.Axis.RAnalog ] = new Vector2( Input.GetAxis( MAP_RANALOG_X ), Input.GetAxis( MAP_RANALOG_Y ) );

		Triggers[ Xbox.Trigger.Right ]	= Input.GetAxis( MAP_TRIGGER_R );
		Triggers[ Xbox.Trigger.Left ]	= Input.GetAxis( MAP_TRIGGER_L );

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

		for ( int i = 0; i < DebugAxes.Length; i++ )
		{
			DebugAxes[ i ] = Input.GetAxis( "Axis_" + ( i + 1 ) );
		}

	}

	#endregion

	#region ButtonDown / ButtonUp

	public bool GetButtonDown( Xbox.Button b )
	{
		if ( Buttons[ b ] == true && prevButtons[ b ] == false )
		{
			return true;
		}

		return false;
	}

	public bool GetButtonUp( Xbox.Button b )
	{
		if ( Buttons[ b ] == false && prevButtons[ b ] == true )
		{
			return true;
		}

		return false;
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
