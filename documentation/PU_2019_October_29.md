# Log 4 - October 2019

This week, I established the audio style of Qurotema, along with producing final features in the movement system, user interfacing, and graphics. Additionally, I created a preliminary version of the story sequencing system, allowing for narrative text to appear in sequences for story development, accompanied with custom actions like making new gameobjects appear.

## Movement Modes & User Interfacing

From an early point in Qurotema's development, I've wanted multiple modes of movement, with each affecting the world in some different visual way. My justification for this is that Qurotema's story is about gaining an unknown power, and unleashing a foreign force through this power's acquisition. It's a story about unintentionally releasing an unknown through growth and evolution, mimicking technological and philosophical concerns in the contemporary world, especially in relation to automation and advanced artificial intelligence.

The movement systems are a way to enforce the player's notion of power growth. Each of these modes also features a visual influence on the environment in escalating degrees, and a different user interface for aesthetic variance and clearer differentiation between the modes. For the sake of clarity, these modes are called "control", "movement", and "flight" respectively.

In "control", the primary mode, the player can interact with their environment through a floating spherical cursor that stays near the player. They use this cursor to activate monoliths so they can see the text they contain, as well as interact with instruments, like creating and strumming the strings on the Strings instrument.

In "movement", the player can teleport at the location of their cursor. This time, the "cursor" is a set of vertical markers at the center of the player's vision. These markers, being placed wherever the player looks, have a bigger physical presence than the cursor in "control" and produce more audio, further enforcing the idea that the player is influencing the environment.

In "flight", the player hovers far above the terrain, moves at high speeds, and creates a large field of moving emissive textures where they look. In this mode, players don't simply add to the environment, but affect it directly, once again aiming to produce a sense of environmental mastery.

[In this video](https://www.youtube.com/watch?v=Pta2KjvQ6hE&feature=youtu.be) are the three modes of movement, along with their visual effects and user interfaces.

## Storytelling

What ties Qurotema together is the explicit narrative. It's important for me to contextualize the player, even if vagely, so that Qurotema does not come off as a tech-demo, but more of a crafted experience.

Therefore, I've created a system that allows a sequence of strings to be written onto the screen, and each of the pieces of text can also be accompanied with custom scripted events.

Thus far, I've created the introductory sequence where the player's controls are locked until 2 of the pieces of text have been shown. The sun comes out, the gates that it hid behind disappear, and the skybox comes into view (all scripted events).

Here is a [video of the introduction sequence](https://youtu.be/MYkhZOYoMvU).

For now, I am happy with the text, though I might think of a fancier way of displaying it later on if I feel it doesn't match the style.

## Audio

An incredibly important component for Qurotema's aesthetic and experience is audio. This week I experimented with different styles, and produced what I believe to be a solid foundation, and a plan for how to organize Qurotema's audio system.

Qurotema's audio aims to be atmospheric, creepy, dynamic, and epic. The system that governs this audio needs to divide musical components up into different sources, using the player's actions with the world as triggers for audio samples and effects (some musical and explicit, others subtle and ambient).

To do this, I am producing a set of tracks and samples that work together as an arrangement. Each track/sample set is assigned to different actions, making the player the catalyst for the audio.

These triggers are:

Looking at the sun.
Looking at the gates.
Walking / running / player speed.
Jumping.
Flying.
Teleporting.
Using the UI mode.
Looking at a monolith.
Instrument interactions.

In addition to these sound triggers, for the purpose of using audio to provide dynamic range to the game's narrative, Qurotema features the following musical states:

Idle
Low Energy
High Energy
Monumental

Each of these states features differences in instrumentation and arrangement, but nonetheless blend between one another seamlessly, both in terms of mixing, as well as composition. The same actions from the player produce different results, depending on the state the game is in.

Idle is a state achieved through minimal player input, and the default theme of the game. Walking, jumping, and minimal sprinting switch to the low energy state. Flying, teleporting, continuous sprinting, rapid camera movement, and repeated interactions eventually lead to the high energy state. Finally, the monumental state is temporarily triggered exclusively when milestone actions have been completed, like interacting with an instrument or monolith for enough time to trigger progress in the story.

For consistency, all states share the same key (C major) and usually follow the same bassline (CM, and Em). That is, the root of the chosen scale, and the third of the chosen scale. In a standard western 7-tone scale, the relationship between the root and the third is fairly consonant, making it easy to stay on either of the two notes for a prolonged time. Simultaneously, the third holds a bit of tension when related to the fundamental, meaning that it still produces a slight sense of discomfort, which is my goal in making the Qurotema universe feel a bit evil, cold, and unwelcoming. The third moving to the fundamental provides a sense of relief, but the idea is to keep that shortlived and switch back to the third fairly quickly.

The idle state is, simply put, ambient music. It is calm, drone-y, with too slow of a tempo to feel a discernable beat and anticipate changes in chords. Every so often, the signature theme plays, in octaves: third, second, and fundamental. All the while, the melody floats above it, sang by a choir-like set of voices. This state uniquely features a vocal chant that resembles that of a choir in a cathedral. No other states include this sound, rewarding players who stop and listen, and making the theme of Qurotema accessible but uncommon during gameplay, considering that players will likely not be standing still very often.

The low energy state is composed of the standard drone ambience with quiet hints of ambient percussion. Player actions trigger simple, low volume samples and simple changes to audio effects.

The high energy state is composed of the standard drone ambience, along with some bassline variations, hints of passing melodies, and ambient percussion. Player actions trigger a different set of more complex, medium-to-high volume samples and complex changes to audio effects. This is the state players will spend most of their time in.

The monumental state is composed of a different drone ambience, an alternative bassline, and a strong melody. Player actions trigger the same samples as in the low energy state to allow for the difference in style to shine through whilst maintaining a link to what the player is already sonically familiar with.

I've produced 3 sets of audio that showcase some of what I've spoken about here.

[Vocal chanting experiment](https://soundcloud.com/v_exec/qurotema-sound-test/s-ejTv3).

[Signature theme](https://soundcloud.com/v_exec/qurotema-theme/s-fn9Rb), the piano is temporary and only used to experiment.

[Atmospheric background track](https://soundcloud.com/v_exec/qurotema-atmoshperic-background/s-FnstP).

## Next Week

Next week, I'd like to produce a larger set of audio to fully fill the possible variations of audio combinations in the system outlined above. If I have time, I'd also like to create the preliminary system for triggering this audio, so we can get the samples into Unity as quickly as possible.