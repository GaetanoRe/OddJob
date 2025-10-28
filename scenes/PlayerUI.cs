using Godot;
using System;

public partial class PlayerUI : Control
{
	
	[Export]
	public Player player;
	
	[Export]
	public Label interactLabel;
	

	public override void _Process(double delta){
		interactLabel.Visible = player.canInteract;
	}
	
}
