# Try to slide
This is my first game project created after watching youtube tutorial "Making a Simple Game in Unity - Unity C# Tutorial" by GameGrind from 2014 year. While I was moving forward in tutorial I started to understand some basic unity mechanics and started thinking how I can use them even right now and how I can diversify the game from tutorial using my ideas. How did it go? Let's see! 

## Game Description:
The goal of the game is to proceed through next levels getting the best scores with the lowest possible cost (cost of time and life). Player start in level selection area where he can go to first level. After each level he can acces to level scoreboard and to check and compare his score with other players scores. This feature allows player to assess his position and make a possible decision to try to pass the level once again with better result but with a risk of loosing life or ending the game.

## Project Description:
The goal of this project is to learn about basic Unity mechanics, basic interactions between Unity and C#  and to get used to Unity environment.

## Game Mechanics:
###### Main Menu:
Player starts in the main menu which I created by using GUI elements (GUI box, labels, buttons). At the moment I can only do it this way. Controlling each menu section by managing flags, for example:
```csharp
showOptionScreen = true;
```
- New Game - player is moved to name section, where he must input players name, game will check if the name field is not empty or if the player is already on any scoreboard (e.x. level scoreboard or overall scoreboard). 
If players name field is empty, player will be noticed that he need to input a name.
If players name is already on any scoreboard he will be notified that if he starts the game with that name, all scoreboard records will be deleted.
After name section, player is moved to level selection location.
- Continue Game - continue game with previous progress stored in PlayerPrefs. variables.
- Highscores - player has access to Overall Scoreboard here, which displays first 25 records from scoreboard.
- Options - in this section player can reset all scoreboards by pressing reset button.
- Quit - player can quit the game by pressing this button.

###### Level Selection location:
All level entrances are placed in this location. Player has access to each level, when player gets in range of entrance, level scoreboard will pop-up on the screen. If player already passed this level restart button will appear below the level scores and player will be able to procced through level once again trying to get better score. Whenever player finishes level and presses continue button he will be moved to level selection and next level will be unlocked.
There is also Scoreboard object in the middle of the scene, which displays overall scoreboard with best players. Player can compare his current score to other players and see how many points he is missing to the top players.

###### Level 1 - 8
- Player must get to Goal object before time runs out
- Level Score:
`level complete time in % * level time importance `
`coin count in % * level coin importance`
- Level Threats:
1. Traps: Physical (spike, floor, blade), Gas, Fire, Ice
1. Enemies
1. Falling down from map

###### Win/Lose
- Player wins when he gets to goal object, win screen with details is shown and player can press continue button to procced further.
After completing all eight levels player can submit his score using Scoreboard object in level selection.
- Player loses when time runs out, life is less or equal to 0.
Player can now view the scoreboard or press Quit button which move him to Main Menu.

## How to play
**Controls**  
- `W`/`Up Arrow` - move forward.
- `S`/`Down Arrow` - move backward.
- `A`/`Left Arrow` - move left.
- `D`/`Right Arrow` - move right.
- `E/SPACE` - enter level.

## Goals:
Goals at this time are just simply to try to learn, document my work, progress through this project and finish my first game!

## Download link
Download zip file from google drive, unzip package and run .exe file.  
[Google Drive Link - 20mb](https://drive.google.com/file/d/1kiW2mlkgiXp5ULNOwT17QpoJ2bwUDyWV/view?usp=sharing)
