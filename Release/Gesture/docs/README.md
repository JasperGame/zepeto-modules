# Gesture Module Import Guide

[English](./README.md) | [Korean](./README_KR.md)

## Import Guide
1. Import the latest version of the Gesture module into your project.
2. Drag ZepetoGestureModule/Prefab/ZepetoGesture_Horizontal.prefab into Scene.
     <img width="700" alt="image" src="./images/GuideImage1.png">
3. After adding the ZepetoPlayers component to the scene and implementing the character creation script, you can test the gesture function by pressing the [▶︎(play)] button.

## Use Tips
#### How to change button icon position
- Adjust the location of BTN_Gesture inside ZepetoGesture_Horizontal.prefab.   
         <img width="700" alt="image" src="./images/GuideImage2.png">

#### How to change the number of gesture loads
- You can control the number of gestures to be exposed by adjusting the number of Load Contents Count in the GestureLoader script inside ZepetoGesture_Horizontal.prefab.   
         <img width="700" alt="image" src="./images/GuideImage3.png">

#### Playing the gesture on loop
- You can activate/deactivate the looping setting for your gesture.
- You can set a wait time until the gesture start looping again.
  
<img width="700" alt="image" src="./images/GuideImage5.png">


#### How to sync multiplayer
- For multiplayer synchronization, after downloading [[Multiplay Component]](../../MultiplayComponent/)), check **Use Zepeto Gesture API** of ZepetoPlayersManager placed in the scene.   
         <img width="700" alt="image" src="./images/GuideImage4.png">
