using Godot;
using System;

public partial class InteractCast : RayCast3D
{

	[Export] public string InteractableGroup = "interactable";

	private Area3D _last;

	[Signal] public delegate void RayEnteredInteractableEventHandler(Area3D area);
	[Signal] public delegate void RayExitedInteractableEventHandler(Area3D area);

	public override void _Ready(){
		CollideWithAreas = true;
		CollideWithBodies = false;
		HitFromInside = false;
		Enabled = true;
		ExcludeParent = true;
	}

	public override void _PhysicsProcess(double delta){
		Area3D current = null;

		if(IsColliding()){
			var col = GetCollider();
			if(col is Area3D area && area.IsInGroup(InteractableGroup)){
				current = area;
			}
		}

		if(current != null && current != _last){
			EmitSignal(SignalName.RayEnteredInteractable, current);
		}

		if(_last != null && _last != current){
			EmitSignal(SignalName.RayExitedInteractable, _last);
		}

		
	}
	
	public Area3D GetCurrentArea() => _last;
}
