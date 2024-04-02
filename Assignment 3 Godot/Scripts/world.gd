extends Node2D

var isPaused = false;

# Called when the node enters the scene tree for the first time.
func _ready():
	Input.set_mouse_mode(Input.MOUSE_MODE_CONFINED)
	
	if Telemetry.has_authenticated():
		_on_authenticated(true, Telemetry.get_session_id())

func _on_authenticated(success, info):
	if success:
		if info < 0:
			$CanvasLayer/SessionInfo.text = "Offline Session #" + str(info)
		else:
			$CanvasLayer/SessionInfo.text = "Server Session #" + str(info)
	else:
		$CanvasLayer/SessionInfo.text = str(info)

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	PauseMenu()

func PauseMenu():
	if Input.is_action_just_pressed("ui_cancel"):
		if get_tree().paused: # unpause the game
			get_tree().paused = false
			Input.set_mouse_mode(Input.MOUSE_MODE_CONFINED)
			$CanvasLayer/PauseMenu.visible = false
			
		elif not get_tree().paused: # pause the game
			get_tree().paused = true
			Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
			$CanvasLayer/PauseMenu.visible = true
			
