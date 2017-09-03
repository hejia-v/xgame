# Resource checker

Resource checker is designed to help bring visibility to resource use in your scenes (ie what assets are using up memory,
which meshes are a bit too detailed, where are my materials and textures being used).

It should also be useful to check for redundant materials, textures you forget you're using, textures that can be compressed or reduced in size.

To use, just create an "Editor" folder within "Assets" in your project if you don't already have one, and drop ResourceChecker.cs in there.

In Unity you'll see a new option under "Window" -> "Resource Checker"

To use, once the window is open, just click 'Refresh' and it will list all active textures, materials and meshes in the scene

Textures
--------

- Textures are listed in descending memory size
- Click the texture name to open the asset in the project window
- The size of the texture, compression type and dimensions are show
- Click the 'X Mat' button to select all materials that use this texture in the scene
- Click the 'X GO' to select all game objects in the scene that use this texture

Materials
---------

- Click the material name to open the material asset in the project window
- Click the 'X GO' to select all game objects in the scene that use this material

Meshes
------
- Meshes are listed in descending vertex count
- Click the mesh name to open the mesh in the project window
- Click the 'X GO' to select all game objects in the scene that use this mesh

Its probably got a bunch of bugs and weird issues - feel free to help improve, fix up!

# UnityAssetCleaner

UnityAssetCleaner will remove unused assets, script and shader in your game and projects.

how to use
1. select menu/window/delete unused assets/unused by editor.
2. editor lists unused assets from your game.
    if you do not want to delete it, you can uncheck it. 
3. select "Exclude from project"
    export unused assets by unitypackage.

please use “menu/window/delete unused assets/cache clear” in some time. sometimes work incorrectly if not cache clear.

