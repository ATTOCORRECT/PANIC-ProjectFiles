extends Node2D


# Called when the node enters the scene tree for the first time.
func _on_authenticated(success, info):
	if success:
		if (info < 0):
			$SessionInfo.text = "Offline Session" + str(info)
		else:
			$SessionInfo.text = "Server Session #" + str(info)
	else:
		$SessionInfo.text = info
	
func _ready():
	if Telemetry.has_authenticated():
		_on_authenticated(true, Telemetry.get_session_id())
	else:
		Telemetry.authenticated.connect(_on_authenticated)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass


func _on_quit_button_up():
	get_tree().quit()


func _on_play_button_up():
	Telemetry.set_section("Game Running")
	get_tree().change_scene_to_file("res://world.tscn")
