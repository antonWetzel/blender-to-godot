# Blender to Godot

Importer for \*.blend files for Godot 3.4.

## Requirements

Blender must be accessible from the command line (Blender directory is added to path).

## Installation

Copy **addons/blender_importer/** into your project directory and enable the plugin ([Guide](https://docs.godotengine.org/en/stable/tutorials/plugins/editor/installing_plugins.html)).


## Options

| Name            | Value | Effect                              |
|-----------------|-------|-------------------------------------|
| Apply Modifiers | bool  | Export with Modifiers (Mirror, ...) |


## Versions
Language | Location
---------|--------
GD-Script| addons/blender-importer
C#   | addons/blender-importer-cs

There is no noticeable speed difference between versions, because all calculations are done by Godot or Blender.

## Limitations

Instances of the imported file are not updated until the scene is reopened.

## Inner Workings

The Blender file (\*.blend) is converted to a glTF 2.0 file (\*.glb) and then to a native Godot scene (\*.scn). The glTF file is deleted afterwards.

## Alternatives

https://github.com/V-Sekai/godot-blender
