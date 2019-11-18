# Log 5 - November 2019

These last few weeks, I've focused almost exclusively on audio work for Qurotema - a fitting time investment given that the sound component of the game is integral to the experience. A reminder of the audio; various player actions all produce different sounds, working collectively to build a soundscape made from player input, all sitting on a randomly arranged ambient track.

## Audio Production

To begin with, the audio needed to be produced. A sound / set of sounds were chosen for each player action, and two sets of these sounds were created for low and high energy levels (determined by the amount of player actions taken over a period of time). The idea is to create some sonic variation for the player, and to try to make the overall excitement of the sound reflect the player's degree of activity. In addition to this set of dynamic sounds, there are also more ambient sounds that still change depending on low / high energy modes, but are not triggered by player action and simply serve the purpose of providing a persistent soundscape. This is a foundation for the dynamic sounds, as well as a sort of persistent sonic activity, avoiding a barren soundscape from ever happening.

The sounds produced were made to fit with the nature of their "source". That is to say, for instance, the vertical lines that appear when the player is using the teleportation mode create a short, regular, almost percussive noise that is consistent with their visual nature: a regular repeated spawning of these vertical lines. Another example is how teleportation sounds like a windy explosion, and walking / sprinting sounds like a muffled drum track.

For some sounds, I decided to mix the C major and E minor chords into one. This makes it slightly more difficult to associate a clear emotion to the chord as there is some level of increaed sonic dissonance, but this is very effective in making the soundtrack come off as more complex and mysterious, and hopefully more fun to 'explore'.

## Audio Implementation

To make the audio sets scalable with easy addition of new sounds, I've created two systems for the two primary forms of audio.

1. The simpler of the two is the *ambience system*. This is for the ambient sounds that come in and out of play, and is made with a procedurally generated collection of objects, all of which are children to the camera, and each containing an audio source. These objects are controlled by a manager script that ensures they are all activated at the same time for musical synchronization, activates appropriate ones at appropriate player actions, and switches them from chord to chord.

2. The more complex system is the *dynamics system*. This is for the player-produced sounds, and is also made with a procedurally generated collection of objects, all of which are children to the camera, and each contain two audio sources. Each of these audio sources is for the different levels of energy: low or high.

To be more specific, player actions can have a series of consequences on audio. They can:

1. Play a single one-shot of a random sound from a set.
2. Play a single one-shot of a select sound from a set.
3. Fade in a looping clip.
4. Fade out a looping clip.

Each of these 4 actions must also ensure that the proper set of clips is being played. Two different sets of sounds were created for each of the two energy levels, meaning that when at high-energy, only high-energy clips should be played. This is handled by the script on each object, which, when asked to play a sound, checks what the energy level is before playing anything. In the event that the energy level is high, the low energy level clip is automatically slowly faded out, and the high energy clip is automatically faded in.

In summary, this system has made it very easy for me to implement audio samples, and thankfully sped up the technical side of audio work. Upon requesting a sound to be played, it dynamically handles fading it in, fading out any other samples from that set of sounds, and so forth.

During the implementation process, when experiencing the sounds in-game, I felt that some didn't work very well - either they didn't fit the visual, weren't properly mixed, or simply needed additional retouching. This system made it simple for me to iterate on them at a fast pace.

[With all that being said, here is a video that showcases the soundtrack and the sound system it's built on.](https://www.youtube.com/watch?v=eLQaBD16Xns&feature=youtu.be)

## New Instrument

During the audio implementation phase, I had an idea to make one of the sounds play different notes depending on how far up or down the camera is looking - essentially creating a vertical piano roll. The idea was to give the player some ability to directly influence the sound and write their own melody, as opposed to simply triggering sounds on and off. Of course, I quickly saw that a visual support was necessary to show players this set of horizontal slices.

![rings](https://raw.githubusercontent.com/v-exec/Qurotema/master/documentation/new/rings.png)

After creating the visual aid, I decided this would in fact work best as a standalone instrument as it had some visual flair, wouldn't crowd the already busy player UI, and perhaps most importantly, stayed consistent with the idea that player actions result in ambient sonic consequences, whereas instruments in the world are the ones meant to be direct forms of musical creation. Keeping this consistency intact is important as the lack of instructions only makes it harder for the player to grasp how exactly they're affecting the game world.

[So with that, it appears I have another instrument in Qurotema!](https://www.youtube.com/watch?v=6FCTKaSj4KQ&feature=youtu.be)

## Next Week

I've made large strides with this update and I really feel the game coming together. Next week I'd like to begin finalizing what we already have in preparation for the final task - tying the story progression to player actions. So, I plan on refining a couple of sounds, perhaps adding another instrument, and adding the language projections to the monoliths. Each of these tasks is fairly easy, so together I believe they will provide a good amount of work. The week after that will simply be a matter of producing a more deliberate map, and tying player actions to the existing story system.