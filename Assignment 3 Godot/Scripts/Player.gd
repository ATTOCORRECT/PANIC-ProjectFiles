extends CharacterBody2D

# constants
const MAX_SPEED = 600.0 # units per tick
const MAX_WALK_SPEED = 300.0 # units per tick
const ACCELERATION = 5000.0 # units per tick ^2
const FRICTION = 5000.0 # units per tick ^2
const AIR_RESISTANCE = 1000.0 # units per tick ^2
const JUMP_VELOCITY = 750.0
const SWING_SPEED = 800.0
const SWING_DEADZONE = 75

var direction = 0

var swingMagnitude = 0

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
var sydneyMode = false   # makes the movement weird
var wasOnFloor = false #to log singular collisions with the floor

# anim state
var animationState = "default"

# music variables 
var impactSoundHasPlayed

# Get the gravity from the project settings to be synced with RigidBody nodes.
var gravity = ProjectSettings.get_setting("physics/2d/default_gravity")

# getting the animation player
@onready var animation = $AnimationPlayer
@onready var sprite = $Sprite2D

#getting the area2d that finds dialogue boxes
@onready var actionable_finder: Area2D = $ActionableFinder

func _draw(): # debug line stuff
	if Input.is_action_pressed("swing_sword") and canSwing:
		if not swingMagnitude > SWING_DEADZONE:
			draw_arc(Vector2.ZERO, 40, 0, 360, 128, Color.RED, 1)
			draw_arc(Vector2.ZERO, swingMagnitude / SWING_DEADZONE * 40, 0, 360, 128, Color.WHITE, 1)
		else:
			draw_line(swingDirection * 40, swingDirection * 100,Color.WHITE, 3)
			draw_arc(Vector2.ZERO, 40, 0, 360, 128, Color.WHITE, 3)

func _physics_process(delta): # physics update
	
	if is_on_floor():
		animationState = "idle"
	
	if !is_on_floor() and canSwing:
		animationState = "fall"
	
	# Add the gravity.
	velocity.y += gravity * delta
	
	# buffers
	coyoteFloor()
	coyoteWall()
	jumpBuffer()
	
	# movement
	sword()
	move(delta)
	jump(delta)
	wallJump()
	wallSlide(delta)
	move_and_slide()
	
	queue_redraw() # debug lines

func _process(delta):
	playerImpactSound()
	walkingSound()
	
	# Animations
	
	match animationState:
		"walk":
			animation.play("walk")
		"sprint":
			animation.play("sprint")
		"dash":
			animationState = ""
			animation.play("dash")
			animation.queue("fall")
		"fall":
			animation.play("fall")
		"wall_slide":
			animation.play("wall_slide")
		"idle":
			animation.play("idle")
		_:
			pass
	

		# flip sprite
	if Input.is_action_pressed("move_left"):
		sprite.flip_h = true
	elif Input.is_action_pressed("move_right"):
		sprite.flip_h = false


func sword(): # Handle Sword Dash
	
	# swing controll handling
	var swingVector = Vector2.ZERO
	if sydneyMode:
		if Input.is_action_just_pressed("swing_sword"):
			resetMousePosition()
			cameraOriginalPosition = $PlayerCamera.get_screen_center_position()
			mouseOriginalPosition = get_global_mouse_position()
			
		cameraPositionDelta = $PlayerCamera.get_screen_center_position() - cameraOriginalPosition
		swingVector = (get_global_mouse_position() - (mouseOriginalPosition + cameraPositionDelta))
		
	else:
		swingVector = get_local_mouse_position()
	swingMagnitude = swingVector.length()
	swingDirection = swingVector.normalized()
	
	# physics
	if Input.is_action_just_released("swing_sword") and canSwing: # successful sword swing
		canSwing = false
		
		if swingMagnitude > SWING_DEADZONE :
			
			if is_on_floor(): # flatten swing if on ground
				swingDirection.y = 0
				swingDirection.x = sign(swingDirection.x)
			
			Telemetry.log_event("", "Sword Swing Release", {position = position, 
			timeSlow = Engine.time_scale < 1, swingDirection = swingDirection, 
			onFloor = is_on_floor() })
			
			velocity = swingDirection * SWING_SPEED 
			move_and_collide(swingDirection * SWING_SPEED / 50)
			
			animationState = "dash"
	
	if Input.is_action_just_pressed("swing_sword") and canSwing:
		
		Telemetry.log_event("", "Sword Swing Start", {position = position, 
		timeSlow = Engine.time_scale < 1, 
		onFloor = is_on_floor() })
	
	if Input.is_action_pressed("swing_sword") and canSwing and not is_on_floor():
		Engine.time_scale = 0.1
	elif Engine.time_scale == 0.1:
		Engine.time_scale = 1
	
	if isOnCoyoteFloor:
		canSwing = true
		pass

func jump(delta): # Handle Jump
	if inputJumpBuffered and isOnCoyoteFloor: # successful jump
		Telemetry.log_event("", "Jump", {position = position})
		
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
	if inputJumpBuffered and isOnCoyoteWallOnly: # successful wall jump
		Telemetry.log_event("", "Wall Jump", {position = position})
		
		animationState = "fall"
		
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

func wallSlide(delta):
	if is_on_wall_only() and get_wall_normal().x * direction < 0: # succesfull wall slide
		if velocity.y > 0: #only when going down
			animationState = "wall_slide"
			
			velocity.y -= gravity * delta
			velocity.y = lerpf(velocity.y, 0, 10 * delta)

func move(delta): # Get the input direction and handle the movement/deceleration
	direction = Input.get_axis("move_left", "move_right")
	
	if direction and canMove:
		if is_on_floor():
			if Input.is_action_pressed("sprint"):
				animationState = "sprint"
				walkMove(delta, MAX_SPEED) # running
			else:
				animationState = "walk"
				walkMove(delta, MAX_WALK_SPEED) # walking
		else:
			airMove(delta)
	elif is_on_floor():
		groundFriction(delta)
	else:
		airResistance(delta)
	
	# movement telemetry
	if Input.is_action_just_pressed("move_left"):
		Telemetry.log_event("", "Move Left", 
		{position = position,
		canMove = canMove, 
		direction = direction,
		onFloor = is_on_floor(),
		isSprinting = Input.is_action_pressed("sprint") && is_on_floor()})
	
	if Input.is_action_just_pressed("move_right"):
		Telemetry.log_event("", "Move Right", 
		{position = position,
		canMove = canMove, 
		direction = direction,
		onFloor = is_on_floor(),
		isSprinting = Input.is_action_pressed("sprint") && is_on_floor()})
	
	if Input.is_action_just_released("move_left"):
		Telemetry.log_event("", "Stop Moving Left", 
		{position = position,
		canMove = canMove, 
		direction = direction,
		onFloor = is_on_floor(),
		isSprinting = Input.is_action_pressed("sprint") && is_on_floor()})
	
	if Input.is_action_just_released("move_right"):
		Telemetry.log_event("", "Stop Moving Right", 
		{position = position,
		canMove = canMove, 
		direction = direction,
		onFloor = is_on_floor(),
		isSprinting = Input.is_action_pressed("sprint") && is_on_floor()})

func walkMove(delta, maxSpeed): # walking & running
	velocity.x = move_toward(velocity.x, direction * maxSpeed, ACCELERATION * delta)
	velocity.y = velocity.x / get_floor_normal().y * get_floor_normal().x * -1 +100 

func airMove(delta): # air control
	if velocity.x * direction < MAX_WALK_SPEED:
		velocity.x = move_toward(velocity.x, direction * MAX_SPEED, ACCELERATION * delta)

func groundFriction(delta): # ground slow down
	velocity.x = move_toward(velocity.x, 0, FRICTION * delta)

func airResistance(delta): # air slow down
	velocity.x = move_toward(velocity.x, 0, AIR_RESISTANCE * delta)
	
func playerImpactSound(): ##playing the player impact sound
	if !wasOnFloor and isOnCoyoteFloor:
		$PlayerSoundPlayers/ImpactSound.play()
		pass
	wasOnFloor = isOnCoyoteFloor

func walkingSound():
	##$PlayerSoundPlayers/WalkingSound.play()
	pass
	
func _unhandled_input(_event: InputEvent) -> void:
	if Input.is_action_just_pressed("ui_accept"):
		var actionables = actionable_finder.get_overlapping_areas()
		if actionables.size() > 0:
			actionables[0].action()
			return

func resetMousePosition():
	var screenMiddle = get_viewport().size / 2
	get_viewport().warp_mouse(screenMiddle)
