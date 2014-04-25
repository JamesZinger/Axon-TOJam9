using UnityEngine;
using System.Collections;
using System;

public class GameControls : MonoBehaviour
{

	private Xbox360GamepadState Controller;

	#region Input Events

	public delegate void	JumpHandeler();
	public event			JumpHandeler JumpButton;

	public delegate void	SlideHandeler();
	public event			SlideHandeler SlideButton;

	public delegate void	PauseHandeler();
	public event			PauseHandeler PauseButton;

	public delegate void	UseItemHandeler();
	public event			UseItemHandeler UseItemButton;

	public delegate void	UseShortcutHandeler();
	public event			UseShortcutHandeler UseShortcutButton;

	public delegate void	MenuUpHandeler();
	public event			MenuUpHandeler MenuUpButton;

	public delegate void	MenuDownHandeler();
	public event			MenuDownHandeler MenuDownButton;

	public delegate void	MenuConfirmHandeler();
	public event			MenuConfirmHandeler MenuConfirmButton;

	#endregion

	void Update()
	{
		// Update the controller
		//Controller = Controller.GetState();

		if ( CheckJumpControls() )
			if ( JumpButton.GetInvocationList().Length != 0 )
				JumpButton();

		if ( CheckSlideControls() )
			if ( SlideButton.GetInvocationList().Length != 0 )
				SlideButton();

		if ( CheckPauseControls() )
			if ( PauseButton.GetInvocationList().Length != 0 )
				PauseButton();

		if ( CheckMenuUpControls() )
			if ( MenuUpButton.GetInvocationList().Length != 0 )
				MenuUpButton();

		if ( CheckMenuDownControls() )
			if ( MenuDownButton.GetInvocationList().Length != 0 )
				MenuDownButton();

		if ( CheckMenuConfirmControls() )
			if ( MenuConfirmButton.GetInvocationList().Length != 0 )
				MenuConfirmButton();

		if ( CheckUseItemControls() )
			if ( UseItemButton.GetInvocationList().Length != 0 )
				UseItemButton();

		if ( CheckUseShortcutControls() )
			if ( UseShortcutButton.GetInvocationList().Length != 0 )
				UseShortcutButton();

	}

	#region Check Controls

	private bool CheckJumpControls()
	{
		if ( Input.GetKeyDown( KeyCode.Space ) )
			return true;

		if ( Input.GetKeyDown( KeyCode.W ) )
			return true;

		if ( Input.GetKeyDown( KeyCode.UpArrow ) )
			return true;

		return false;
	}

	private bool CheckSlideControls()
	{
		if ( Input.GetKeyDown( KeyCode.LeftControl ) )
			return true;

		if ( Input.GetKeyDown( KeyCode.S ) )
			return true;

		if ( Input.GetKeyDown( KeyCode.DownArrow ) )
			return true;

		return false;
	}

	private bool CheckPauseControls()
	{
		if ( Input.GetKeyDown( KeyCode.P ) )
			return true;

		return false;
	}

	private bool CheckMenuUpControls()
	{
		if ( Input.GetKeyDown( KeyCode.UpArrow ) )
			return true;

		if ( Input.GetKeyDown( KeyCode.W ) )
			return true;

		return false;
	}

	private bool CheckMenuDownControls()
	{
		if ( Input.GetKeyDown( KeyCode.DownArrow ) )
			return true;

		if ( Input.GetKeyDown( KeyCode.S ) )
			return true;

		return false;
	}

	private bool CheckMenuConfirmControls()
	{
		if ( Input.GetKeyDown( KeyCode.Return ) )
			return true;

		return false;
	}

	private bool CheckUseItemControls()
	{
		if ( Input.GetKeyDown( KeyCode.E ) )
			return true;

		return false;
	}

	private bool CheckUseShortcutControls()
	{
		if ( Input.GetKeyDown( KeyCode.F ) )
			return true;

		return false;
	}

	#endregion
}
