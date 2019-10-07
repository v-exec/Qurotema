# Log 2 - October 2019

This week, my primary goal was to iron out unfinished parts of the project, especially those of graphical nature, that ultimately impact the overall aesthetic.

## Terrain and Water

First came the terrain shader, which is composed of 3 main parts.

First, a gradient noise map is created and used to displace the height of the terrain. The UV map that maps this noise is then displaced in a single axis to create the illusion of waves shifting the surface of the terrain. Additional parameters are moved around using perlin noise to give it more life.

Second, a set of noise maps is blended together and then mapped to the smoothness of the material. This produces the sparkly specks of light on the terrain. Because some spots are extremely reflective, and others not as much, the contrast in light creates dots of bright reflected light.

Finally, and perhaps the most integral component, is the generation of the normal map. Once again, a set of noise maps is blended together to create a normal map. This map is especially important because it changes the direction light is reflected in, creating complex reflections with a combination of probe-based reflection (for distant objects outside the player's view), as well as screenspace reflections (for more dynamic visuals that don't necessarily follow the laws of physics, but still look cool).

I also made sure to allow for expansion on the 2nd and 3rd parts of the terrain shader. The plan is to create a few different 'profiles' that are blended between, as opposed to having a single and static normal map or a single smoothness map.

I added water that functions in a similar way, though the intersection of terrain and water meshes created problems for the movement system I had made. Because I keep the player glued to the ground to create a sense of gliding over the terrain, the player also stays glued to the water and at times this overrides the terrain and the player goes straight through it instead of climbing up the terrain and off the water. This has prompted me to rethink a bit of the movement system, and create a check for whether there are surfaces higher than the water (or any other collider, for that matter), so they're prioritized.

## Skybox

The terrain and water didn't give me too much trouble, but the skybox, which I assumed would be a quick job, took far longer than I anticipaed. My supervisor for this project suggested that I "find a degree of criticality in the documentation", so it is my hope that they will be happy to learn about my struggles in regards to the sky.

The plan was to create a series of clouds that would rotate in different directions, and be more visible the closer they are to the sun. My assumption was that Unity's skybox system would allow for this. I was incorrect.

Unity's skybox is fairly limited, and because I'm using my own implementation of a "sun", I'm not using Unity's procedural skybox either. Not that it would matter, but this suggested I should do things more manually. Upon further research, I learned that what I wanted to do was a "sky dome", where a hemisphere around the map with a texture is rotated to create a sense of cloud movement. This made sense, but Unity had no integrated support for this. Not only that, but I had a lot of trouble finding good images online. The fact of the matter was, I needed clouds with a transparent background so they could blend into eachother in cool ways. Also, I created a starry night skybox using [Spacescape](https://github.com/petrocket/spacescape), and I wanted that to be visible through the clouds as they became more transparent.

So, I created my own mesh in Blender, made some clouds for testing, and mapped them to the 3D hemisphere. It looked _okay_ in Unity, but I had hoped that some built-in blending mode would make the clouds appear more vibrant and opaque towards the sun, and vice-versa. This was not the case, and I had to write my own shader for this. I added a property that forces the alpha of the cloud texture to be clipped at a different threshold, meaning that clouds could now fade in and out in cool smokey patterns.

In the process of creating the close-to-sun-transparency effect, I ended up with some accidentally interesting results by clipping the clouds too abruptly.

![clipAccident](https://github.com/v-exec/Qurotema/tree/master/documentation/new.clipAccident.png)

Although I didn't keep this effect, I did like how odd it looked, and might decide to bring it back in a later version.

Finally, I wanted to also slowly rotate the starry night skybox. Even this was painful, as Unity's new implementation of skyboxes in the High Definition Render Pipeline is still lacking in documentation. After looking around, I had to go through their source code alongside their not-so-complete HDRP reference, and find out how to access and modify the skybox's rotation property with _very specific_, undocumented syntax.

The result is this:

![skyboxExample](https://github.com/v-exec/Qurotema/tree/master/documentation/new.skyboxExample.png)

The clouds are still the test clouds, but even they look alright with the current setup.

## Post Processing

A noteworthy mention is a new development in HDRP's post processing system. Through another unrelated project, I learned that Unity is yet to (officially) implement custom screen-space shader creation in the HD pipeline. This means that if I do wish to create custom screenspace effects, I'd need to switch to the standard rendering pipeline, which would in turn mean recreating things like the terrain shader and atmospheric effects. Thankfully, I do not plan to do this for _Qurotema_, but this has proved a challenge in my other project.

## Next Week

What awaits next week is working with the map itself. I want to finalize the actual terrain, and create a system to keep the player inside the limitations of the map without making them feel too limited, and hiding the borders of the map itself.