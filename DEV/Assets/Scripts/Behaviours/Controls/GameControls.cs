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

	public delegate void	StopSlideHandeler();
	public event			StopSlideHandeler StopSlideButton;

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

	void Awale()
	{
		Game.Instance.Controls = this;
	}

	void Update()
	{
		// Update the controller
		//Controller = Controller.GetState();

		if ( CheckJumpControls() )
		{ 
			Debug.Log("Jump Button");
			if ( JumpButton != null )
				JumpButton();
		}

		if ( CheckSlideControls() )
			if ( SlideButton != null )
				SlideButton();

		if ( CheckStopSlideControls())
			if (StopSlideButton != null )
				StopSlideButton();

		if ( CheckPauseControls() )
			if ( PauseButton != null )
				PauseButton();

		if ( CheckMenuUpControls() )
			if ( MenuUpButton != null )
				MenuUpButton();

		if ( CheckMenuDownControls() )
			if ( MenuDownButton != null )
				MenuDownButton();

		if ( CheckMenuConfirmControls() )
			if ( MenuConfirmButton != null )
				MenuConfirmButton();

		if ( CheckUseItemControls() )
			if ( UseItemButton != null )
				UseItemButton();

		if ( CheckUseShortcutControls() )
			if ( UseShortcutButton != null )
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

	private bool CheckStopSlideControls()
	{
		if ( Input.GetKeyUp( KeyCode.LeftControl ) )
			return true;

		if ( Input.GetKeyUp( KeyCode.S ) )
			return true;

		if ( Input.GetKeyUp( KeyCode.DownArrow ) )
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
