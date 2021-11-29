# Random Tilemap Generator

This is a random tilemap generator that supports multiple tile type.

This script's parameters can be modified from the Unity Editor GUI.

# Instructions:

Load the scene "c-tilemap-20.unity".

Drag & Drop tiles to the "Tiles" field on the Tilemap object.

Other parameters were kept from the original script.

# Scripts

**[Random Tile Generator](Assets/Scripts/RandomTileGenerator.cs)** - This is the main generator algorithm.

The script generates a random int array with numbers from 0 to 'number of tiles to choose from'.

The script can smooth the tilemap for better results.

See notes for more details.

<br/>

**[Tilemap Generator](Assets/Scripts/TilemapGenerator.cs)** - This script is meant to be used with the tilemap object.

The script contains all the parameters needed to use the algrorithm.

After the built of a tilemap int array, this script is also building the tilemap on the screen.

See notes for more details.

# External Links

**Itch.io** - https://littlegamers2021.itch.io/tilemap-generator
