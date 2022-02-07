using System.Diagnostics;
using Godot;
using Godot.Collections;

namespace BlenderImporter {

	[Tool]
	class Importer : EditorImportPlugin {

		enum Preset {
			Default,
			Max,
		}

		public override string GetImporterName() { return "antonWetzel.blenderImporter.cs"; }

		public override string GetVisibleName() { return "Blender Importer CS"; }

		public override Array GetRecognizedExtensions() { return new Array() { "blend" }; }

		public override string GetSaveExtension() { return "scn"; }

		public override string GetResourceType() { return "PackedScene"; }

		public override bool GetOptionVisibility(string option, Dictionary options) { return true; }

		public override int GetPresetCount() { return (int)Preset.Max; }

		public override string GetPresetName(int preset) {
			switch ((Preset)preset) {
			case Preset.Default: return "Default";
			default: return "Unknown";
			}
		}

		public override int GetImportOrder() { return (int)ImportOrder.Scene; }

		public override Array GetImportOptions(int preset) {
			switch ((Preset)preset) {
			case Preset.Default:
				return new Array() {
					new Dictionary() {
						{"name", "apply_modifiers"},
						{"default_value", true},
					}
				};
			default:
				return new Array() { };
			}
		}

		public override int Import(
			string sourceFile,
			string savePath,
			Godot.Collections.Dictionary options,
			Godot.Collections.Array platformVariants,
			Godot.Collections.Array genFiles
		) {
			var param = " " + options["apply_modifiers"];
			var tempPath = sourceFile.Replace(".blend", ".glb");
			var globalPath = ProjectSettings.GlobalizePath(sourceFile);
			var globalBasePath = globalPath.Replace(".blend", "");
			var pythonFile = ProjectSettings.GlobalizePath("res://addons/blender_importer_cs/export_gltf.py");
			var proc = new Process {
				StartInfo = new ProcessStartInfo {
					UseShellExecute = false,
					RedirectStandardOutput = true,
					CreateNoWindow = true,
					FileName = "blender",
					Arguments = globalPath +
						" --background" +
						" --python " + pythonFile +
						" -- " + globalBasePath + param,
				}
			};
			try {
				proc.Start();
				while (!proc.StandardOutput.EndOfStream) {
					proc.StandardOutput.ReadLine();
				}
			} catch (System.Exception e) {
				GD.PushWarning(e.Message);
				GD.PushWarning("Is blender avaible in path?");
				return (int)Error.Bug;
			}
			var temp = new Godot.PackedSceneGLTF().ImportGltfScene(tempPath);
			var packedScene = new Godot.PackedScene();
			packedScene.Pack(temp);
			new Godot.Directory().Remove(tempPath);
			return (int)ResourceSaver.Save(savePath + "." + GetSaveExtension(), packedScene);

		}
	}
}
