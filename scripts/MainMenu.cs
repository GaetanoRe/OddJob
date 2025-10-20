using Godot;
using System;

public partial class MainMenu : Control
{
	[Export]
	public PackedScene mainScene;

	public void OnPlayButtonPressed()
	{
		GetTree().ChangeSceneToPacked(mainScene);
	}
	
	public void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}
}
