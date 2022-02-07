tool
extends EditorImportPlugin

enum Preset {
	Default
}

func get_importer_name():
	return "antonWetzel.blenderImporter"

func get_visible_name():
	return "Blender Importer"

func get_recognized_extensions():
	return ["blend"]

func get_save_extension():
	return "scn"

func get_resource_type():
	return "PackedScene"

func get_option_visibility(option, options):
	return true

func get_preset_count():
	return Preset.size()

func get_preset_name(preset):
	match preset:
		Preset.Default:
			return "Default"
		_:
			return "Unknown"

func get_import_order():
	return IMPORT_ORDER_SCENE

func get_import_options(preset):
	match preset:
		Preset.Default:
			return [{
				"name": "apply_modifiers",
				"default_value": true,
			}]
		_:
			return []

func import(source_file, save_path, options, platform_variants, gen_files):
	var temp_path = source_file.replace(".blend", ".glb")
	var global_path = ProjectSettings.globalize_path(source_file)
	var global_path_base = global_path.replace(".blend", "");
	var python_file = ProjectSettings.globalize_path("res://addons/blender_importer/export_gltf.py")
	var ret = OS.execute("blender", [
		global_path,
		"--background",
		"--python", python_file,
		"--",
		global_path_base,
		options["apply_modifiers"],
	])
	if ret != OK:
		push_warning("Is blender avaible in path?")
		return ERR_BUG
	var temp = PackedSceneGLTF.new().import_gltf_scene(temp_path)
	var packed_scene = PackedScene.new()
	packed_scene.pack(temp)
	Directory.new().remove(temp_path)
	return ResourceSaver.save(save_path + "." + get_save_extension(), packed_scene)
