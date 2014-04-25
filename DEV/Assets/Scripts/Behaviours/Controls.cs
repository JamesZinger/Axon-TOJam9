using UnityEngine;
using System.Collections;
using System;

public class Controls : MonoBehaviour
{

	Xbox360GamepadState ControllerState;

	#region Input Events

	public delegate void	JumpHandeler ( EventArgs e );
	public event			JumpHandeler Jump;

	public delegate void	CrouchHandeler ( EventArgs e );
	public event			CrouchHandeler Crouch;

	public delegate void	PauseHandeler ( EventArgs e );
	public event			PauseHandeler Pause;

	#endregion


}
