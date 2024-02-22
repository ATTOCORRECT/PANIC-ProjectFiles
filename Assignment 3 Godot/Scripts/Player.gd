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

# vectors
var cameraOriginalPosition = Vector2.ZERO
var mouseOriginalPosition = Vector2.ZERO
var cameraPositionDelta = Vector2.ZERO
var swingDirection = Vector2.ZERO

# bools
var canSwing = false
var timeSlow = false
var canMove = true
var inputJumpBuffered = false
var isOnCoyoteFloor = true
var isOnCoyoteWallOnly = false
var isSprinting = false

#music variables 
var impactSoundHasPlayed

# Get the gravity from the project settings to be synced with RigidBody nodes.
var gravity = ProjectSettings.get_setting("physics/2d/default_gravity")

#getting the animation player
@onready var animation = $AnimationPlayer
@onready var sprite = $Sprite2D

func _draw(): # debug line stuff
	if Input.is_action_pressed("SwingSword") and canSwing:
		draw_line(Vector2.ZERO,swingDirection * 100,Color.WHITE,1)

func _physics_process(delta): # physics update
	# Add the gravity.
	velocity.y += gravity * delta
	

	
	#buffers
	coyoteFloor()
	coyoteWall()
	jumpBuffer()
	
	#movement
	sword()
	move(delta)
	jump(delta)
	wallJump()
	#wallSlide(delta)
	
	move_and_slide()
	
	if not isOnCoyoteFloor:
		if canSwing:
			animation.play("fall")
		
	elif isOnCoyoteFloor:
		
		if direction and Input.is_action_pressed("Sprint"):
			animation.play("sprint")
		elif direction:
			animation.play("walk")
		else:
			animation.play("idle")
	
	# flip sprite
	if Input.is_action_pressed("move_left"):
		sprite.flip_h = true
	elif Input.is_action_pressed("move_right"):
		sprite.flip_h = false
	
	queue_redraw() # debug lines

func sword(): # Handle Sword Dash
	if Input.is_action_just_pressed("SwingSword"):
		cameraOriginalPosition = $PlayerCamera.get_screen_center_position()
		mouseOriginalPosition = get_global_mouse_position()
	cameraPositionDelta = $PlayerCamera.get_screen_center_position() - cameraOriginalPosition
	swingDirection = (get_global_mouse_position() - (mouseOriginalPosition + cameraPositionDelta)).normalized()
	
	if Input.is_action_just_released("SwingSword") and canSwing:
		if is_on_floor(): # flatten swing if on ground
			swingDirection.y = 0
			swingDirection.x = sign(swingDirection.x)
		velocity = swingDirection * SWING_SPEED
		move_and_collide(swingDirection)
		canSwing = false
		
		animation.play("dash")
		animation.queue("fall")
	
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
		velocity.y = -JUMP_VELOCITY

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
		
		#placeholder animation for when this is working
		animation.play("wall_slide")

func move(delta): # Get the input direction and handle the movement/deceleration
	direction = Input.get_axis("move_left", "move_right")
	
	if direction and canMove:
		if is_on_floor():
			if Input.is_action_pressed("Sprint"):
				walkMove(delta, MAX_SPEED) # running
			else:
				walkMove(delta, MAX_WALK_SPEED) # walking
		else:
			airMove(delta)
	elif is_on_floor():
		groundFriction(delta)
	else:
		airResistance(delta)

func walkMove(delta, maxSpeed): # walking & running
	velocity.x = move_toward(velocity.x, direction * maxSpeed, ACCELERATION * delta)
	velocity.y = velocity.x / get_floor_normal().y * get_floor_normal().x * -1 +100 

func airMove(delta): # air control
	if velocity.x * direction < MAX_SPEED:
		velocity.x = move_toward(velocity.x, direction * MAX_SPEED, ACCELERATION * delta)

func groundFriction(delta): # ground slow down
	velocity.x = move_toward(velocity.x, 0, FRICTION * delta)

func airResistance(delta): # air slow down
	velocity.x = move_toward(velocity.x, 0, AIR_RESISTANCE * delta)
	
func playerImpactSound(delta): ##playing the player impact sound
	if is_on_floor:
		$PlayerImpactSoundPlayer.play()
		impactSoundHasPlayed = true

