# Headbutt Game

A physics-driven fighting game called
HEADBUTT. The objective of the game is for two players to knock each other off
the stage with a headbutt attack. The game emphasizes fun, engagement, and
social interaction, with no downtime and interesting decisions for the player. The
game mechanics include simple movesets, a 2D stage with islands, and a life
system. The game intends to provide players with a lighthearted, odd,
and comedic experience.

Inspired by games that include odd fighting mechanics or silly physics
such as include Divekick (2013), Gang Beasts (2017), and QWOP (2008).

![screenshot](https://cdn.discordapp.com/attachments/1149428611116441600/1229552118570946581/Screenshot_2024-04-15_170444.png?ex=67b3b1a5&is=67b26025&hm=82fac645955d1febece40d1fd081b0cafc0263af563e4a4a62e7a9920f266300&)

## Goals

HEADBUTT is all about fun and engagement. It offers a competitive play
experience with no downtime, keeping you on your toes and fully engaged. The
game also instills social interaction by having two players face each other, which
can provide a sense of pride after defeating the opponent. An additional goal of
the game is to generate interesting decisions for the player. They can make
tactical decisions, where each input will impact the fight's outcome. Decisions are
discernable but double-edged; for example, if players attack too early and whiff,
resulting in punishment. Adding to the fun aspect, seeing something unexpected
or unwanted can be comedic, especially when sharing it with a friend.

## Mechanics

Two players face off on a 2D stage. The stage features a large floating island with
two smaller islands on the left and right sides. Players can jump to the islands. If a
player falls off the stage, they lose a life. By default, players have three lives.
The players' move set includes headbutting and jumping. Getting hit by a headbutt
applies force to the player and pushes them. The more the player is hit, the
stronger the force. The player can fall off the stage without being careful.
If the two players don't land hits on each other for more than 10 seconds, a 10-
second countdown appears. If the countdown reaches 0, both players lose a life.
This mechanic is inspired by the Cranked game mode in Call of Duty and intends
to urge the players to go on the offensive.

The game-winner is decided by whoever still has lives remaining.

## Formal elements

The player interaction pattern in this game is one-on-one, with the objective of
knocking the other player off the stage. The rules are a mix of explicit and implicit,
and players can engage in bluffing and games which are outside the rules. The
resources available to players are lives and time, and the outcome is that one
player will win, and the other will lose.

## Dramatic elements

The game lacks a story or premise, but one idea that was considered was that the
player character is on a drug-fuelled psychosis and believes their head is going to
explode. They see the opponent player, who is also going through the same
predicament, and believe them to be the cause of the problem. And so, they slam
their heads at each other and try to make the other disappear. This story would
play in an opening cutscene.

## Dynamic elements

The moveset in this game is very simple, but as is common in fighting games,
players may discover an optimal strategy for which the game designer didn't
HEADBUTT Game Design Report3account. Techniques or exploits may exist that provide an additional degree of
mechanics. An example of this is "wavedashing" in Super Smash Bros. Melee.
Although no playtesting was carried out due to time constraints, it could bring
insights into unexpected player behaviour.

## Aesthetics

The game uses free low poly style assets from the Unity Asset Store and
animations from Mixamo. The only aesthetic justification that is present is for the
player's character models. There is no reason that the player models are ninjas,
but that model was chosen because their heads are enormous, which helps the
attack mechanic. Aesthetics, in general, were not important to this game. If there
were more time, then the aesthetics would support the drug-fuelled psychosis
idea, making the world and characters more psychedelic and abstract (see Juice
Galaxy (2023) for inspiration). For example, the skybox could be changed from a
clear blue sky to a weird-looking fractal pattern. An additional graphical effect I
wanted to add was a hit effect and trail behind the player so that attacking and
getting pushed around would look more aesthetically pleasing and provide
feedback. The game also lacks audio due to time constraints, but if there were
audio, it would sound goofy and cartoonish to support the silly nature of the game.

## Technology

The game heavily uses Unity's animation and physics engine. There was one
feature that couldn't be implemented due to bugs: a ragdoll system, where if a
player was in the air for long enough, they would ragdoll. Although it didn't work
due to some problems relating to the Animator, Rigidbodies and colliders, it would
see usage if there were a multi-height platform stage. The game's island stage
doesn't benefit from having ragdoll physics. The game's low poly graphics mean
that any modern computer hardware could run the game. A place for improvement
would be adding controller support, as there is only keyboard input when both
players must share a keyboard. The Unity game engine did not impede any of the
features implemented or wanted to implement and was a good choice, in
particular, because of the plentiful amount of assets and resources available.
