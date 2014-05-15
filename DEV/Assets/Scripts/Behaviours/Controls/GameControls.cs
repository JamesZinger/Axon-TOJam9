using UnityEngine;
using System.Collections;
using System;

public class GameControls : MonoBehaviour
{

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

	#endregion

	Xbox360GamepadState controller = Xbox360GamepadState.Instance;

	private bool isLoaded;

	void Awake()
	{
		Game.Instance.Controls = this;
		isLoaded = false;

	}

	void Update()
	{
		if ( !isLoaded )
		{
			isLoaded = true;
			return;
		}

		if ( CheckJumpControls() )
		{ 
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

		if ( CheckUseItemControls() )
			if ( UseItemButton != null )
				UseItemButton();

		if ( CheckUseShortcutControls() )
			if ( UseShortcutButton != null )
				UseShortcutButton();

		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();

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

		if ( controller.IsButtonDown(Xbox.Button.A) == true)
			return true;

		return false;
	}

	private bool CheckSlideControls()
	{
		if ( Input.GetKeyDown( KeyCode.S ) )
			return true;

		if ( Input.GetKeyDown( KeyCode.DownArrow ) )
			return true;

		if ( controller.IsButtonDown( Xbox.Button.B ) )
			return true;

		return false;
	}

	private bool CheckStopSlideControls()
	{
		if ( Input.GetKeyUp( KeyCode.S ) )
			return true;

		if ( Input.GetKeyUp( KeyCode.DownArrow ) )
			return true;

		if ( controller.IsButtonUp( Xbox.Button.B ) )
			return true;

		return false;
	}

	private bool CheckPauseControls()
	{
		if ( Input.GetKeyDown( KeyCode.P ) )
			return true;

		if ( controller.IsButtonDown( Xbox.Button.Start ) )
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
		if ( Input.GetKeyDown( KeyCode.Z ) )
			return true;

		if (Input.GetKeyDown(KeyCode.RightControl))
			return true;

		if ( Input.GetKeyDown( KeyCode.LeftControl ) )
			return true;

		if ( controller.IsButtonDown(Xbox.Button.X))
			return true;

		return false;
	}

	private bool CheckUseShortcutControls()
	{
		if ( Input.GetKeyDown( KeyCode.Q ) )
			return true;

		if ( controller.IsButtonDown(Xbox.Button.Start))
			return true;

		return false;
	}

	#endregion
}
