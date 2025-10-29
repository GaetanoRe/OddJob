using Godot;
using System;

public partial class Player : CharacterBody3D
{

	[Export]
	public float speed = 5f;

	[Export]
	public float jumpVel = 50f;

	[Export]
	public float lookSensitivity = 0.06f;

	[Export]
	public bool isRunning;

	[Export]
	public Node3D camRoot;

	[Export]
	public Camera3D camera;

	[Export]
	public InteractCast interactCast;

	public bool canInteract = false;


	private float walkSpeed = 5f;
	
	private Vector3 charVelocity;

	private Vector3 wishDirection;

	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _Input(InputEvent @event)
	{
		if(@event.IsActionPressed("interact"))
		{
			if (canInteract)
			{
				var interactable = interactCast.GetCurrentArea();
				if(interactable is InteractableArea ia)
				{
					ia.Interact(this);
				}
			}
		}
		if (@event is InputEventMouseButton)
		{
			Input.MouseMode = Input.MouseModeEnum.Captured;
		}
		else if (@event.IsActionPressed("ui_cancel"))
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}
		
		if(Input.MouseMode == Input.MouseModeEnum.Captured)
		{
			if(@event is InputEventMouseMotion mouseMotion)
			{
				camRoot.RotateY(-mouseMotion.Relative.X * lookSensitivity);
				camera.RotateX(-mouseMotion.Relative.Y * lookSensitivity);
				float xRotation = Mathf.Clamp(camera.Rotation.X, Mathf.DegToRad(-90), Mathf.DegToRad(90));
				camera.Rotation = new Vector3(xRotation, camera.Rotation.Y, camera.Rotation.Z);
			}
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		var inputDirection = Input.GetVector("move_left", "move_right", "move_forward", "move_backward").Normalized();

		wishDirection = camRoot.GlobalTransform.Basis * new Vector3(inputDirection.X, 0, inputDirection.Y);

		if (IsOnFloor())
		{
			_HandleGroundPhysics();
		}
		else
		{
			_HandleAirPhysics(delta, Velocity.Y);
		}
		MoveAndSlide();
	}

	private void OnRayEnteredInteractable(Area3D area){
		canInteract = true;
	}

	private void OnRayExitedInteractable(Area3D area){
		canInteract = false;
		
	}


	private void _HandleAirPhysics(double delta, float currVelocity)
	{
		currVelocity -= (float) ProjectSettings.GetSetting("physics/3d/default_gravity") * (float)delta;
		Velocity = new Vector3(Velocity.X, currVelocity, Velocity.Z);
	}
	
	private void _HandleGroundPhysics()
	{
		if(isRunning){
			speed = walkSpeed * 2;
		}
		else{
			speed = walkSpeed;
		}
		charVelocity.X = wishDirection.X * speed;
		charVelocity.Z = wishDirection.Z * speed;
		if(Input.IsActionPressed("jump")){
			charVelocity.Y = jumpVel;
		}
		else{
			charVelocity.Y = 0;
		}

		if(Input.IsActionPressed("run_forward")){
			isRunning = true;
		}
		else{
			isRunning = false;
		}
		Velocity = charVelocity;
	}


}
