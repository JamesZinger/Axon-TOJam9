using UnityEngine;
using System.Collections;

public class MenuControls : MonoBehaviour
{
	#region Event Delegates and Objects

	public delegate void MenuUpHandeler();
	public event			MenuUpHandeler MenuUpButton;

	public delegate void MenuDownHandeler();
	public event			MenuDownHandeler MenuDownButton;

	public delegate void MenuConfirmHandeler();
	public event			MenuConfirmHandeler MenuConfirmButton;

	#endregion

	// Use this for initialization
	void Awake () 
	{
		Menu.Instance.Controls = this;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( CheckMenuUpControls() )
			if ( MenuUpButton != null )
				MenuUpButton();

		if ( CheckMenuDownControls() )
			if ( MenuDownButton != null )
				MenuDownButton();

		if ( CheckMenuConfirmControls() )
			if ( MenuConfirmButton != null )
				MenuConfirmButton();
	}

	#region Key check functions

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

	#endregion
}
