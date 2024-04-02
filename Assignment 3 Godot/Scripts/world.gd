extends Node2D

var isPaused = false;

# Called when the node enters the scene tree for the first time.
func _ready():
	Input.set_mouse_mode(Input.MOUSE_MODE_CONFINED)

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	PauseMenu()
	pass
	
func PauseMenu():
	if Input.is_action_just_pressed("ui_cancel"):
		if get_tree().paused: # unpause the game
			get_tree().paused = false
			Input.set_mouse_mode(Input.MOUSE_MODE_CONFINED)
			
		elif not get_tree().paused: # pause the game
			get_tree().paused = true
			Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
