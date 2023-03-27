# Mannequin Module Import Guide

[English](./README.md) | [Korean](./README_KR.md)

## Dependencies
1. To use the mannequin function, install the latest version of ZEPETO.Mannequin Package through PackagaManager.
2. Import the Multiplay Component through the Module Importer to synchronize the try-on.
3. The Mannequin API can only be used when you have an item that you have created yourself. If this is your first time creating an item, you must first create the item and complete the screening process. [[Create item]](https://studio.zepeto.me/console/items/create)

## Import Guide
1. Import the latest version of the Mannequin module into your project.
2. Write the following in the World.multiplay/Index.ts file.
       > **Note**: You may have more modules than the image below if you have added other modules.
         <img width="700" alt="image" src="./Image/GuideImage1.png">
3. Drag and drop the Zepeto Mannequin Module/Prefab/MannequinManager.prefab into the scene.
4. Within the Zepeto Mannequin Module/Prefab folder, place the desired type of mannequin in the desired location within the scene.
         <img width="700" alt="image" src="./Image/GuideImage2.png">
5. Add the id of the item you created to the ids of the Mannequin Component.
     > **Note**: Please check the [[Mannequin Guide]](https://docs.zepeto.me/studio-world/docs/zepeto_mannequin) on how to check the mannequin type and item ID.
         <img width="700" alt="image" src="./Image/GuideImage3.png">
6. Add the ZepetoPlayers component to the scene and implement the character creation script.
7. You can test the mannequin function by turning on the multiplayer server at the top and pressing the [▶︎(play)] button.


## Use Tips
- You can interact with the mannequin within the desired range by adjusting the collider size of the mannequin object.
         <img width="700" alt="image" src="./Image/GuideImage4.png">
