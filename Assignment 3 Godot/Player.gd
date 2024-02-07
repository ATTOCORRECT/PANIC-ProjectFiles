extends CharacterBody2D

# constants
const MAX_SPEED = 500.0 # units per tick
const ACCELERATION = 5000.0 # units per tick ^2
const FRICTION = 3000.0 # units per tick ^2
const JUMP_VELOCITY = -750.0
const SWING_SPEED = 700.0

var canSwing = false

# Get the gravity from the project settings to be synced with RigidBody nodes.
var gravity = ProjectSettings.get_setting("physics/2d/default_gravity")

func _draw():
	if Input.is_action_pressed("SwingSword") and canSwing:
		draw_line(Vector2.ZERO,get_local_mouse_position().normalized() * 100,Color.WHITE,1)

func _physics_process(delta): # physics update
	# Add the gravity.
	if not is_on_floor():
		velocity.y += gravity * delta
	
	sword()
	jump(delta)
	move(delta)
	move_and_slide()
	queue_redraw()

func sword():
	if Input.is_action_just_released("SwingSword") and canSwing:
		var swingDirection = get_local_mouse_position().normalized()
		if is_on_floor(): # flatten swing if on ground
			swingDirection.y = 0
			swingDirection.x = sign(swingDirection.x)
		velocity = swingDirection * SWING_SPEED
		position += swingDirection * 20
		canSwing = false
	
	if Input.is_action_pressed("SwingSword") and canSwing and not is_on_floor():
		Engine.time_scale = 0.1
	else:
		Engine.time_scale = 1
		
	if is_on_floor() or is_on_wall():
		canSwing = true

func jump(delta): # Handle Jump
	if Input.is_action_just_pressed("jump") and is_on_floor():
		velocity.y = JUMP_VELOCITY

func move(delta): # Get the input direction and handle the movement/deceleration
	var direction = Input.get_axis("move_left", "move_right")
	if direction and abs(velocity.x) < MAX_SPEED:
		velocity.x = move_toward(velocity.x, direction * MAX_SPEED, ACCELERATION * delta)
	elif is_on_floor():
		velocity.x = move_toward(velocity.x, 0, FRICTION * delta)
