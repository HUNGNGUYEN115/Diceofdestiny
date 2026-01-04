# Dice Game

**Description:**  
The game is about rolling the dice, using spirit cards, and showing the Dice × Multiplier Equation.

## Game Setup

1. Place all necessary assets in the scene (dice, cards, canvas including TextMeshPro).
2. Implement the dice rolling function by using random torque and force. Disable rolling when the dice is already rolling (`Rolledice.cs`).
3. Detect the rolled face by using a trigger with the plane and get the name of the opposite face (`FaceDetector.cs`).
4. Implement the spirit cards definitions using ScriptableObjects (`CardData.cs`).
5. Implement the script to add data to cards (name, usage text, card number, VFX, sound effects) (`Cards.cs`).
6. Create `CardManager.cs` to check conditions and activate spirit cards based on the dice result.
7. Create `Point.cs` to show the Multiplier Equation in the UI with animation (using TMP Animator component) and a History Panel.
   
## Unity Version

6000.3.2f1

## Third-party Assets 

Using the tool for Unity called TMP Animator to animate the UI within numbers, the tool is free and allowed to use in personal project
github link: https://github.com/Luca3317/TMPEffects.git

All  assets is free to use in the personal project
- Dice model: Unity assets store
- Cards model: icht.io
- vfx : Unity assets store
- all audio clip: Pixabay
