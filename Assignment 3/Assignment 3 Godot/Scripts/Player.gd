extends CharacterBody2D

# constants
const MAX_SPEED = 600.0 # units per tick
const MAX_WALK_SPEED = 300.0 # units per tick
const ACCELERATION = 3000.0 # units per tick ^2
const FRICTION = 3000.0 # units per tick ^2
const AIR_RESISTANCE = 1000.0 # units per tick ^2
const JUMP_VELOCITY = 750.0
const SWING_SPEED = 700.0

var direction = 0

# bools
var canSwing = false
var timeSlow = false
var canMove = true
var inputJumpBuffered = false
var isOnCoyoteFloor = true
var isOnCoyoteWallOnly = false

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
	coyoteFloor()
	jumpBuffer()
	jump(delta)
	move(delta)
	coyoteWall()
	wallJump()
	#wallSlide(delta)
	move_and_slide()
	queue_redraw()

func sword(): # Handle Sword Dash
	if Input.is_action_just_released("SwingSword") and canSwing:
		var swingDirection = get_local_mouse_position().normalized()
		if is_on_floor(): # flatten swing if on ground
			swingDirection.y = 0
			swingDirection.x = sign(swingDirection.x)
		velocity = swingDirection * SWING_SPEED
		move_and_collide(swingDirection)
		canSwing = false
	
	if Input.is_action_pressed("SwingSword") and canSwing and not is_on_floor():
		Engine.time_scale = 0.1
	else:
		Engine.time_scale = 1
	
	if isOnCoyoteFloor or isOnCoyoteWallOnly:
		canSwing = true

func jump(delta): # Handle Jump
	if inputJumpBuffered and isOnCoyoteFloor:
		inputJumpBuffered = false
		isOnCoyoteFloor = false
		velocity += Vector2.UP * JUMP_VELOCITY

func jumpBuffer():
	if Input.is_action_just_pressed("jump"):
		inputJumpBuffered = true
		$JumpBufferTimer.start()
	
	if $JumpBufferTimer.is_stopped():
		inputJumpBuffered = false

func coyoteFloor(): # coyote time logic
	if is_on_floor():
		isOnCoyoteFloor = true
		$CoyoteTimer.start()
	
	if $CoyoteTimer.is_stopped():
		isOnCoyoteFloor = false

func wallJump(): # Handle Wall Jump
	if inputJumpBuffered and isOnCoyoteWallOnly:
		isOnCoyoteWallOnly = false
		velocity += (Vector2.UP + Vector2.RIGHT * get_wall_normal().x).normalized() * JUMP_VELOCITY
		
		canMove = false
		$WallJumpInputSupresstionTimer.start()
	
	if $WallJumpInputSupresstionTimer.is_stopped():
		canMove = true

func coyoteWall(): # wall coyote time logic
	if is_on_wall_only():
		isOnCoyoteWallOnly = true
		$WallCoyoteTimer.start()
	
	if is_on_floor():
		isOnCoyoteWallOnly = false
	
	if $WallCoyoteTimer.is_stopped():
		isOnCoyoteWallOnly = false

func wallSlide(delta): # not functional
	if is_on_wall_only() and get_wall_normal().x * direction < 0:
		velocity.y = velocity.y * 10 * delta * abs(get_wall_normal().x)

func move(delta): # Get the input direction and handle the movement/deceleration
	direction = Input.get_axis("move_left", "move_right")
	
	if direction and canMove:
		if is_on_floor():
			if Input.is_action_pressed("Sprint"):
				runMove(delta)
			else:
				walkMove(delta)
		else:
			airMove(delta)
	elif is_on_floor():
		groundFriction(delta)
	else:
		airResistance(delta)

func walkMove(delta): # walking
	velocity.x = move_toward(velocity.x, direction * MAX_WALK_SPEED, ACCELERATION * delta)

func runMove(delta): # running
	velocity.x = move_toward(velocity.x, direction * MAX_SPEED, ACCELERATION * delta)

func airMove(delta): # air control
	if velocity.x * direction < MAX_SPEED:
		velocity.x = move_toward(velocity.x, direction * MAX_SPEED, ACCELERATION * delta)

func groundFriction(delta): # ground slow down
	velocity.x = move_toward(velocity.x, 0, FRICTION * delta)

func airResistance(delta): # air slow down
	velocity.x = move_toward(velocity.x, 0, AIR_RESISTANCE * delta)

