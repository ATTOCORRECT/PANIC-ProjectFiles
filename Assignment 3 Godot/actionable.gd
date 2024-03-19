extends Area2D

@export var dialogue_resources: DialogueResource
@export var dialogue_start: String = "Test_Node_Title"

func action() -> void:
	DialogueManager.show_example_dialogue_balloon(dialogue_resources, dialogue_start)
