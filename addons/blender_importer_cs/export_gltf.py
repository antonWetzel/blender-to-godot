import bpy
import sys

i = 0
while sys.argv[i] != "--":
    i += 1

# Doc can be found here: https://docs.blender.org/api/current/bpy.ops.export_scene.html
bpy.ops.export_scene.gltf(filepath=sys.argv[i + 1],
                          export_apply=(sys.argv[i + 2] == "True"),
                          export_format='GLB')
