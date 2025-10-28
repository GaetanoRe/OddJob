using Godot;
using System;

public partial class InteractableArea : Area3D
{
	[Export]
	public string prompt = "Interact";

	[Export]
	public NodePath targetPath;

	private Node _target;

	[Signal] public delegate void FocusEnteredEventHandler(Area3D self);
	[Signal] public delegate void FocusExitedEventHandler(Area3D self);
	[Signal] public delegate void InteractedEventHandler(Node3D investigator);

	public override void _Ready(){
		if(string.IsNullOrEmpty(targetPath)){
			_target = Owner;
		}
		else{
			_target = GetNodeOrNull(targetPath) ?? Owner;
		}
		AddToGroup("interactable");
	}

	public void OnFocus(){
		EmitSignal(SignalName.FocusEntered, this);
		if(_target is IInteractable it) it.OnFocus();
	}

	public void OnUnFocus(){
		EmitSignal(SignalName.FocusExited, this);
		if(_target is IInteractable it) it.OnUnFocus();
	}

	public void Interact(Node3D investigator){
		EmitSignal(SignalName.Interacted, investigator);
		if(_target is IInteractable it) it.OnInteract(investigator);
	}

	public string GetPrompt() => (_target as IInteractable)?.GetPrompt() ?? prompt;
}

public interface IInteractable{
	void OnFocus();
	void OnUnFocus();
	void OnInteract(Node3D investigator);
	string GetPrompt();
}
