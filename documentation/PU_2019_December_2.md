# Log 6 - December 2019

This log marks the final log in this documentation series. Since the last update, I have created a new instrument alongside a time-tracking system, made numerous fixes and optimizations to various game elements, added the language to the monoliths on the map, and tied all the story elements together from start to finish. This log will focus on these last few changes, and will be complemented by a report which discusses the larger topics of this project, found in the documentation folder.

Before I get into the discussion, here is a [link](https://www.youtube.com/watch?v=pqO7EeNAWfk&feature=youtu.be) to a full playthrough of _Qurotema_. If you'd like to discover the game's world for yourself, make sure to play the [build](https://v-os.ca/media/downloads/qurotema/qurotema_windows.zip) (Windows only) first. And be warned, the build is not yet optimized, so depending on your machine it may run quite poorly.

## Sequencer Instrument

I wanted 3 instruments in the game, and felt that I had 2 melodic instruments, and it might be fun to add an instrument that's focused more on percussion. A common format for percussion is a sequencer: a set of blocks where the x axis typically represents a certain meter of music, and the y axis represents a certain sample. The other two instruments try to do something interesting with an existing concept. The strings have the player create strings between nodes in 3D space, and the length of the string determines its pitch. I find this interesting because it allows the player to have an influence on the notes they have access to, as opposed to being limited to a preset collection of notes. The other instrument, the rings, takes a piano's keys and flips them on their side. This visualization is fairly close to how I picture music in my head, and the fact that it can be played in 360 degrees reimagines the layout of a piano.

The point being that I wanted the sequencer instrument to also try something new. Seeing as how the other instruments required the player to use the 'cursor' to interact with them, I decided that something where the player could accidentally trigger a note in the sequencer instrument would be useful. It could help players who have not yet been able to interact with the other instruments understand that there are musical instruments in this game, hopefully motivating them to explore further.

To do this, I made a grid of these blocks, and each block can be triggered simply by walking over it. Then, to make things more interesting, the typically linear nature of a sequencer was broken up by making each block fairly flush with the uneven terrain. This way, a player can easily walk over one and activate it by accident, providing them with immediate visual feedback, and eventually audio feedback when that sample is played. The way the blocks are laid out on the terrain also makes it a bit more challenging to create a drum pattern without making a mistake, since not all blocks are visible and a player can easily make a miscalculation. This is intentional, as these mistakes can be fun discoveries in new drum patterns, and since it's all synchronized to the music, nothing ever really sounds bad.

The only issue that remains at the moment is the movement system itself sometimes clipping the player on the side of a block when they want to walk on it, making the process of setting up a drum pattern somewhat annoying.

I'm also unsure as to whether a block should deactivate automatically after some time, to prevent the same player-made pattern to play forever if they don't turn it off. It can become annoying to hear the same drum beat repeatedly.

## Language

A narrative tool beyond that of the text in the game is the fictional language of _Qurotema_. This language is called i-tema, and it's a language I constructed while studying generative syntax with the purpose of making a minimal syntax model where words have no morphological variations, and where the same word can act as noun, adjective/adverb, and verb. I am in the process of writing about this language on my [website](https://v-os.ca), so I wouldn't go into detail in this log as it's also not particularly relevant to the game design.

Text in this language appears in front of the monoliths once they're interacted with (click the 'eye' of the monolith). The phrases that appear are legitimate sentences, with legitimate vocabulary, using a legitimate writing system. This means the language _can_ be understood. Naturally, players will not understand this language as I highly doubt anyone will have read the documentation on my site, let alone internalized it enough to read i-tema intuitively. That being said, however, a big purpose of i-tema appearing in this game is to motivate investigation outside the game itself. Nearly all of my products these days are in some ways narratively connected, and a lot of those connections are documented on my site. So, by leaving an unanswered question like that in _Qurotema_, it can encourage players to seek answers out on my site.

Obviously, I think an _extremely_ small minority of players will do this. It's a lot of work, with a narrative payoff that may not be worth it to most people. Still, I think there's something conceptually beautiful about not faking a fictional language, and giving even players who do not wish to research further a sense of depth to this game world.

The narrative revealed through these monoliths, in short, is a set of simple sentences (to ease translation) that speak to the player. They come from the entity that created _Qurotema_, and attempt to pursuade the player character to continue further into the world, and eventually 'free' the creator.

## Story Progression

To contextualize the player, text gives the player vague instructions on what to do, and notifies them of when an event has happened. To progress, the player needs to find all three instruments, and play a certain number of notes on each. Once this is done, the gates appear, and the player must walk to them to trigger the ending sequence.

Mechanically, this is really simple. And of course, the complexity comes in figuring out where these instruments are in the first place, and how to use them. As in the playthrough I created, the game can be beat in less than 10 minutes. But, I imagine that a player who has no idea how to play could easily take an hour (if they were patient and interested enough).

The problem at the moment is the awkwardness of the narrative. The beginning is fine, but as the game progresses it feels increasingly difficult to find it believable. Essentially, the text is coming from some sort of HQ as hinted in the beginning, and over time they lose 'connection' with the player. The text (what represents HQ), however, is visually the same throughout the game. This sense of connection loss is entirely invisible, and I think that makes it feel off.

There's also the fact that the ending feels cheap and incomplete - which it admittedly is. I would love to put some more time into it and write something with a larger narrative impact. In-narrative, the player is effectively releasing an 'evil god' into the universe, and that dramatic event is not communicated at all.

## Optimization Struggles

The final step was to build the game, and I encountered more issues than I expected during this process. After doing general optimizations - better lightmapping, tigher reflection probes, lower shadowmap resolutions - an issue remained. I was getting some tearing. Enabling v-sync fixed this, but introduced an extremely annoying regular stutter. After many hours of trying to figure this out, browsing forums, and rebuilding the project dozens of times, the most reliable solution was to build the game without v-sync in windowed mode. This seemed to eliminate tearing, while keeping the game fairly stutter free.

Still, this is not a good fix, and I will need to continue tackling this problem to make sure the game is playable with different screen settings, and hopefully on a decent number of machines.

## What's Next?

This being the final log, I think it's important to discuss what the plans for this game are. For one, I do think there's still work to be done in terms of polish.

The monoliths' 'animations' (text appearing and lighting up) were rushed to some degree, and contrast unpleasantly with the otherwise ultra-smooth visuals. So, I'd love to make those fade in or something of the sort to make sure they match the aesthetic of the game. The same goes for the story progression, which at times feels disconnected from the gameplay, especially towards the ending.

Additionally, as I've mentioned, performance is not stellar, but certainly has the potential to be.

Finally, I think _Qurotema_ needs testing from other players. As I've spoken about in the earlier logs, I want to emphasize the joy of discovery for both the environment and the actual gameplay mechanics. If players are entirely incapable of finding some of these mechanics, perhaps narrative hints are necessary, or something integrated into the world? These are the types of questions I think would be easier to answer if I have people play _Qurotema_ and see what confuses them, what brings them joy, and where exactly their curiosity leads them.