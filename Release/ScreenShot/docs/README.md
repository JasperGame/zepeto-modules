# ScreenShot Module Import Guide

[English](./README.md) | [Korean](./README_KR.md)

## Import Guide
1. Import the latest version of the screenshot module into your project.
2. Drag Zepeto ScreenShot Module/Prefab/ZepetoScreenShot_Horizontal.prefab into Scene.
         <img width="700" alt="image" src="./images/GuideImage1.png">
3. After adding the ZepetoPlayers component to the scene and implementing the character creation script, you can test the screenshot function by pressing the [▶︎(play)] button.

## Use Tips
#### You can adjust the position of BTN_ScreenShot inside the prefab.
- Double-click the prefab and select BTN_ScreenShot to reposition it.
         <img width="700" alt="image" src="./images/GuideImage2.png">

#### Screenshot module screen function
         <img width="700" alt="image" src="./images/GuideImage3.png">.  
⓵ This button captures the current screen.   
⓶ Selfie mode/3rd person mode conversion button.   
⓷ Background removal mode button. When activated, only ZEPETO characters will be captured.   
         <img width="700" alt="image" src="./images/GuideImage4.png">
⓸ Image save button. Save the captured image. It works inside the ZEPETO app.   
⓹ Image share button. Share captured images. It works inside the ZEPETO app.   
⓺ Feed upload button. It uploads captured images to the feed. It works inside the ZEPETO app.

#### When Selfie Mode is activated, character's face can follow the camera lens.
- Open the Layer Settings within the currently used Animator Controller and activate the IK Pass feature.
- If the Animator controller exists within a package file and cannot be modified, copy the file to the Assets location for use.
        <img width="700" alt="image" src="./images/GuideImage5.png">   
