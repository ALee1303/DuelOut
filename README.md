# DuelOut
DuelOut is a 2 player version of Breakout game made in Monogame.

![png](https://i.imgur.com/KPKvd0E.png)

## Overview
------------
This was one of the final project done during my study in Columbia College. The project was created in 5 week sprint.

First week consisted of coming up with game idea and project goal.

Second week goal was to discover technical debt and possible fallback.

Third week was when the working concept was due.

By week four, playable demo was expected so that the last week could be spend to addressing bug fixes for last week presentation.

## Objective
------------
As an adeptation from a breakout game that required two player, the biggest feature that stood out from the original was the color-coded balls.

Each player started at the opposite position and broke the bricks in the middle to battle each other. The broken bricks spawned items which traveled in different direction depending on the ball's color. Also, when player's paddle collide with the ball, the color changed according to the player's team.

In order to get over the dependencies issue, I used strategy patterns with conditions spawning from simple enum that represented each team's color. This way, instantiation of each objects were the only dependencies created by the introduced team color variable.

## Description
---------------
This project includes running build of the game and also a Monogame Visual Studio project for the game with the needed scripts. In order to save development time  custom Monogame Library and Breakout Content was provided by [Jeff Meyers](https://github.com/dsp56001/MonogameBreakOut/tree/master/BreakoutTest/MonoGameLibrary).

## Getting Started
These instructions will help you deploy the running build or Monagame Visual Studio project on your hard drive.

### Requirements
* Visual Studio 2015 or 2017
* Monogame 3.0

### Running the Release
After cloning the repository, run FinalDuelOut.exe inside Release folder.

### Running the C# Project
After Cloning the repository, run the FinalDuelOut.sln

## How to Play
--------------
Red and Blue player start from the opposite side and must break the brick in the middle to win score. Players must also stay alive by reacting to the falling debrie. When players have used up all the ball, they must wait for the ball to regenerate, which is when no ball is at play and player's score is lower.

### Controls
* __Blue Player__:
  * Left: Left Arrow
  * Right: Right Arrow
  * Launch: Down
* __Red Player__:
  * Left: Z
  * Right: C
  * Launch: X
  
### Rules
* Each players start with their own ball with its color.
* Ball of opposite color hit by paddle will change the ball's color.
* When all balls are lost, a new ball will be spawned to player with lower score.
* Each ball lost costs 1 point.
* Each brick rewards you with one point.
* Bricks spawn items towards or away from the scored player, depending on the type of item.
  * **Purple**: Spawns a ball for the scored player on the paddle or on the location of the brick.
  * **Yellow**: Randomly spawns __Zombie or Ballon Baby__ towards the scored player.
  * **Red**: Spawn __Poop__ towards opponent direction.
* There are three items. Each items have different effect:
  * **Poop**: looses one life when hit.
  * **Zombie Baby**: looses one life when hit.
  * **Ballon Baby**: looses one life _from guilt (No actual babies were hurt in the process)_ when dropped by spawned player.
* Player who stays alive longer wins.
* When all blocks are cleared, player with higher point wins.

## Acknowledgements
________________
* Jeff Meyers for resources and libraries
* [Gloobus Studio](https://gloobus.net/baby-zombie/) for Zombie Baby texture.
* [Comodo777](https://www.dreamstime.com/comodo777_info) for Ballon Baby texture.
* Google image for Poop texture.
* Inspired by [Breakout](https://en.wikipedia.org/wiki/Breakout_(video_game))
