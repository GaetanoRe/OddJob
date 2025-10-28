using Godot;
using System;

public partial class InteractableTest : StaticBody3D, IInteractable
{
	public void OnFocus(){
		GD.Print($"Focused on {this.Name}");
	}

	public void OnUnFocus(){
		GD.Print($"UnFocused on {this.Name}");
	}

	public void OnInteract(Node3D investigator){
		GD.Print("I have Interacted with this thing!!!");
	}
	
	public string GetPrompt(){
		return "A Prompt";
	}
}
