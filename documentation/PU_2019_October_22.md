# Log 3 - October 2019

This week, I focused on finishing assets for Qurotema, so next week I can focus on more mechanical facets of the game.

## Terrain Shader Rewrite

First came a rewrite of the shader for the terrain. There are three reasons for this.

First of all, the terrain itself was not perfectly conceptualized in the first place. To create intricate pattenrs through reflection (a desired effect to give depth and higher detail/complex visuals), a normal map is generated using noise, which in turn affects how light is reflected. Because the terrain itself is a big object, these textures need to be scaled appropriately. This is one of the reasons why I decided to use generative textures as opposed to premade ones, as generated textures can not only be seamless, but more easily manipulated to hide repetitive patterns in their tiling.

The issue with the first iteration of the shader was how it attempted to be minimal in its usage of separate UV maps. UV, in this case, refers to how a texture is wrapped around a mesh. I had attempted to use only 2 UV maps to lay out multiple normal maps, but this ended up making the normal map generation too convoluted in trying to account for how some normal maps looked better at different sizes. Not to mention, this caused certain maps to be compressed so heavily that they all looked the same, simply producing a uniform grain on the terrain. In short, I ended up increasing the number of UV maps to give me better control over how I scale each normal map.

The second reason for this rewrite was to add dynamic blending between different normal maps. The terrain is the primary visual element in the grand majority of Qurotema, and to keep it from going visually stale, I wanted to create a series of visual "presets" of sorts, and have the terrain blend between them over time. In addition, this would further express how Qurotema is an alien world.

So, now the terrain has 4 unique normal maps, and 2 smoothness maps for visual variety, all of which are being blended through dynamically.

Finally, the third reason is a removal of certain features of the terrain. Namely, replacing vertex displacement with a normal map that produces a similar effect. Vertex displacement, although interesting, is limited to the resolution of the vertices of the mesh. To keep performance acceptable and simultaneously make the terrain area big enough, vertices are in fact fairly far apart. Normal maps that interpolate between those vertices are what allow for the terrain to appear smoother than it actually is. Vertex displacement unfortunately ruins this effect, however, by making the true resolution of the terrain easier to notice. Another problem that this has avoided is how objects like rocks and monoliths, which are entirely static, seem like they float in the terrain if the terrain moves. By keeping the terrain geometrically static, it makes it easier for other game objects to appear like they have a more realistic physical presence.

Another effect I was experimenting with were feedback effects through emission textures. The idea being that certain actions from the player should provoke a specific visual effect, like glowing dots or lines appearing on the terrain. I did this initially by making a copy of the terrain, elevating it slightly, and doing some alpha masking to create that effect. This was costly in terms of performance, and I figured I might as well implement an emission map on the standard terrain shader itself.

With that, the terrain is done! Here are some examples of what it can look like:

![terrain1](https://raw.githubusercontent.com/v-exec/Qurotema/master/documentation/new/terrain1.png)
![terrain2](https://raw.githubusercontent.com/v-exec/Qurotema/master/documentation/new/terrain2.png)
![terrain3](https://raw.githubusercontent.com/v-exec/Qurotema/master/documentation/new/terrain3.png)
![terrain4](https://raw.githubusercontent.com/v-exec/Qurotema/master/documentation/new/terrain4.png)
![terrain5](https://raw.githubusercontent.com/v-exec/Qurotema/master/documentation/new/terrain5.png)
![terrain6](https://raw.githubusercontent.com/v-exec/Qurotema/master/documentation/new/terrain6.png)
![terrain7](https://raw.githubusercontent.com/v-exec/Qurotema/master/documentation/new/terrain7.png)
![terrain8](https://raw.githubusercontent.com/v-exec/Qurotema/master/documentation/new/terrain8.png)
![terrain9](https://raw.githubusercontent.com/v-exec/Qurotema/master/documentation/new/terrain9.png)

## Clouds Removal

In the last update, I put a lot of focus on the clouds and skybox. Despite that time and effort, I've decided that the clouds detract from the atmosphere more so than add to it. Qurotema is a fairly dark environment, and bright white clouds contrast too starkly in style. If they're dark, then they lack contrast and blend in awkwardly into the starry sky. Additionally, the terrain is already a busy visual, adding clouds to the sky makes the environment look too busy. For that reason, I've only kept the starry sky, and removed the clouds. As of right now, I do not intend to bring the clouds back, and feel like I'll leave the sky as it is and call it done as well!

## Monolith and Rock Production + Material Retexturing

A large focus for this week was producing the monolith and rocks found on the map. The monoliths are for the purpose of narrative exposition through holographic text in Qurotema's fictional language, and rocks are simply for visual variety.

I began with the rocks, but once again aimed for an alien material that does not match standard characteristics of the rocks we have on Earth. To create this, I made a degraded material that's rough on the surface, but reveals a smoother, more reflective surface underneath. The inspiration came from the rough texture of shards of charcoal. I mapped it to a heavily geometric mesh so as to avoid the pain of scultping a set of rocks, but also to keep the terrain from getting too visually busy with high-detail geometry.

![rockTest](https://raw.githubusercontent.com/v-exec/Qurotema/master/documentation/new/rockTest.png)
![rock](https://raw.githubusercontent.com/v-exec/Qurotema/master/documentation/new/rock.jpg)

Next came the monolith. I wanted to use a material similar to that of the platform under the strings instrument, a black marble, and a clearcoat metal. Using the same material significes that the monoliths, like the platform under the strings instrument, originate from the same place.

While experimenting with the monolith's material, I came across a rocky pattern that significantly influenced the roughness of the material. The marble was originally very reflective, but this new approach made it quite rough in most areas. Once I imported it into Unity, it worked far better with the environment for two big reasons.

![monolith](https://raw.githubusercontent.com/v-exec/Qurotema/master/documentation/new/mono.png)

First of all, reflectiveness makes it more difficult for the texture itself to be seen, and I wanted the black marble to be easily recognizeable.

![reflective marble example](https://raw.githubusercontent.com/v-exec/Qurotema/master/documentation/new/reflectiveMarbleExample.png)

Second of all, the terrain is already incredibly reflective. Adding more reflective objects in the environment only makes them compete with the aesthetic of the terrain. Making the monolith more rough provides a beautiful contrast, with the terrain and monolith working in complementary fashion rather than competitive.

I updated the marble on the strings platform to match with this new texture, and it worked very nicely!

![newTextures](https://raw.githubusercontent.com/v-exec/Qurotema/master/documentation/new/newTextures.png)

## Next Week

As the primary assets have been produced already, next week will be dedicated towards the mechanics of Qurotema. More explicitly, I'd like to begin working on the introduction sequence - this is the text that shows up in the very beginning of the game, giving some narrative exposition, and introducing the Qurotema universe.

I'd also like to work on producing audio samples and beginning their implementation in various systems (walking, running, jumping, look direction, strings instrument interaction, UI usage, etc.). I suspect the audio work will require a lot of iteration, so it's important I begin sooner rather than later.