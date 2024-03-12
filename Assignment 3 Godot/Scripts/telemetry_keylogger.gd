extends Node2D


# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

func _input(event):
	if event is InputEventKey:
		if event.is_pressed():
			Telemetry.log_event("keyLogger", "Key Pressed", {event = event.as_text()})
		if event.is_released():
			Telemetry.log_event("keyLogger", "Key Released", {event = event.as_text()})
	
	if event is InputEventMouseButton:
		if event.is_pressed():
			Telemetry.log_event("keyLogger", "Mouse Pressed", {event = event.as_text()})
		if event.is_released():
			Telemetry.log_event("keyLogger", "Mouse Released", {event = event.as_text()})
