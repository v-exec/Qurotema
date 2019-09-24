# Log 1 - September 2019

This progress update, as the first one in the series, will encompass a lot of documentation from the 1 year+ of development Qurotema has undergone so far.

Qurotema is an experimental discovery audio-creation game, which builds on the larger narrative of [v-os](https://v-os.ca/qurotema).

## Qurotema

My aesthetic interest for dark environments, desolate deserts, epic monuments, and edgy achromatic imagery are the fundamental pillars for this project. My aim is to create an environment that embodies these ideas, and most importantly, communicates a dark, beautiful, and refined aesthetic.

Qurotema is a universe outside our own, where there is only the endless evolving black-oil sand, and the still liquid oceans. A violet sphere of light relatively orbits each entity at a quick speed.

The terrain slowly shifts between different states, flowing in waves, with texture morphing from slick, to grainy, to smooth. Aside from the dunes, there are a few structures made of black marble and smooth grey metal, dark rocks, and most notably, the Gates of Qurotema. These colossal gates loom in the distance, appearing and disappearing at random, watching over any visitors.

You are remotely accessing this world through experimental technologies that tie your consciousness to a not-so-material technological entity. You have a multi-layered heads-up display that appears in physical space, allowing you to interact with the world around you.

Sound is everywhere, and each action replies with feedback both in the form of visual changes in the terrain, and triggered audio/sonic parameter changes.

Monoliths with an alien language scattered around the landscape hold information to the nature of this universe, and reveal dark secrets of this world's creator.

## Design

Qurotema features zero explicit instructions. Players are left to figure everything out on their own, which I find extremely appealing, as _understanding_ the mechanics is, I would argue, a fascinating and incredibly amusing part of a game. Something I feel I am robbed of with every tutorial and hand-held set of instructions found in contemporary games.

How players move, the various movement options they have access to, how they interact with the environment, that is all left to them to discover. For this reason, interaction isn't particularly complex, usually limited to mouse button presses. This is so as to allow for some accessability, and not lock any players into frustration confusion as opposed to curious investigation.

Players are given minimal exposition in the beginning, explaining how they're embodying a non-physical entity, and how their task is to understand this universe and report back to the research laboratory they work for. Players must explore, find monoliths, and play with "instruments" to prompt additional story exposition from the laboratory.

Monoliths are marble rocks that contain holographic representations of a language: i-tema. This is a written-only language with very simple and consistent syntax. Players are given a translation of one monolith from the lab, to allow for those interested to decode and translate other monoliths and acquire a deeper understanding of the larger story, aside from messages recieved from the lab.

Instruments are musical installations of sorts with which the player can interact with to produce more clear-ended musical products for the larger world-soundscape.

Finally, a lot of emphasis is put on player movement. The player naturally glides over the surface of the dunes, but can also jump, fly, and teleport around the map. This freedom of movement drives home the non-physical form the player takes, as well as give more opportunities for the player to appreciate the environment from different perspectives.

All movement and interaction has an effect on the soundscape, meaning that as the player plays, all their actions are translated to audio. If the player chooses to focus on the audio (which they hopefully will) and reduce their actions in-game to triggers for audio, they would in a way create the music video for the music they produce.

## Tech

Originally, Qurotema was built in Unreal Engine 4. UE is known to be very beautiful, and features a graph environment for writing shaders. I am still not entirely comfortable with writing shaders in languages like HLSL, and given my specific desires for aesthetics, especially with the complex terrain, it made sense that UE would make this easier. This was true for graphics, but anyone who has had the displeasure of programming in UE without decent familiarity with C++ knows that it is not an easy feat. In 2018-2019, Unity released its own shader graph, and I remade Qurotema from scratch in Unity.

I am very happy with this decision, as I've been able to fine-tune everything very nicely.

Most notably, the movement system. At first, I attempted to use Unity's physics for player movement, but eventually opted to write my own system in order to keep the flow of movement as smooth as possible. Now, the player is forced to the ground unless they explicitly jump or run off a high enough cliff. To increase smoothness, the camera's position is always easing to the actual player position (which is also eased). That is combined with a camera shake to add movement to the scene and give everything a dynamic cinematic feel. Additionally, field of view changes as the player speeds up and slows down, as well as when they fly.

Interaction happens when the player holds the right click, displaying a user interface and a cursor - the catalyst for interaction - all in physical space. The UI is made of simple geometric elements on a few different layers with different easing ratios, to create a sense of depth when moving around. This UI is the communication channel between the player and their home universe, and it's meant to be clean, and embody the idea of a fictional user-interface from a futuristic era.

In the most recent version, I've created the first instrument: the strings. A set of spheres in a grid sits atop a black marble pedestal, and the player can drag from one sphere to another to create a string between them. Using their cursor to drag over this string, they make it vibrate and produce noise.

## Future

With all this being said, there is much to be done. My current concern is with the environment itself. The current terrain is not in its final state, and I would like to make it bigger, but am at the mercy of hardware limitations in regards to how high resolution a mesh can be. I have tried using Unity's terrain system, but its support for custom shaders is limited, and I am yet to find a solution. Perhaps a not-so-distant release of Unity will fix this issue.

Additionally, although audio is programatically being triggered, audio samples have not yet been produced. I must figure out the soundscape I want to construct and then create the assets necessary. I imagine a background system that has a chord progression (or perhaps multiple interchangeable ones?) that automatically cycles, and the current chord in the progression determines the pitches all sounds are locked to, to maintain some level of harmony.

The language, i-tema, has been created, but I need to create more vocabulary so it can be used in-game. Implementation should not be an issue. However, vital scenes like the introduction sequence and narrative exposition sequences are things that must be handled soon.

Thereafter, I hope to include additional instruments, more focus on the terrain shader, and other visual tweaks. Now, my focus is to design a sound profile for Qurotema, ensure the map is big enough and sufficiently varied, and produce the assets (marble surfaces, rocks, monoliths, water, etc.) to be implemented into the map. The current version includes a marble slab for the strings instrument with which I am fairly content, but it needs tweaking so it's higher resolution, and reflects the environment better.