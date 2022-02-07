using System;
using Godot;

namespace BlenderImporter {

	[Tool]
	class Plugin : EditorPlugin {

		Importer importer;

		public override void _EnterTree() {
			importer = new Importer();
			AddImportPlugin(importer);
		}

		public override void _ExitTree() {
			RemoveImportPlugin(importer);
			importer = null;
		}
	}
}
