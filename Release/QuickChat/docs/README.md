# How to use the quickChat Module in your Zepeto Project ?

The QuickChat module is the is used for sending in game predefined messages in a Zepeto Multiplayer world. 

In this documentation, you will learn how to add a QuickChat module in your existing zepeto Project. If you are just getting started as a developer in Zepeto, we suggest you to  check out our [beginner guide](https://docs.zepeto.me/studio-world/docs/welcome_zepeto_developers) first. 


## Adding the QuickChat Module into my project


- Open the **Zepeto Module Importer** by clicking on ZEPETO tab > Module Importer (1) in the Zepeto project where you will add the QuickChat functionality.
  
- Select the quick chat module in the list of module on the left tap of the module importer
- Click Import  to import the QuickChat Module. 
- If you did not set up the multiplay feature, make sure to set up the multiplay component first.

![Import the QuickChat file](https://github.com/naverz-LeGrandMAG/zepeto-modules/assets/131629767/a5466575-a0d6-4db0-bf53-fb415e9e8e89)


  
- Go to "*Zepeto QuickChat Module > UI > Prefabs*" and drag and drop the **QuickChat_Canvas_Horizontal** prefab into your scene if the world you are building has an horizontal orientation. Or use the **QuickChat_Canvas_Vertical** prefab if your world orientation is vertical.
  <img width="700" alt="image" src="./images/GuideImage1.png">   

- If your device has a big or smaller screen and the UI is not displayed properly in your world, You can adjust it size by tweaking the **reference resolution** within the Canvas scaler.
  
<img width="700" alt="image" src="./images/GuideImage2.png"> 

Feel free to customize the QuickChat Component/UI depending on your requirements.

This module works only in a multiplayer world, make sure to check out the multiplay component first before trying.


## Getting the list of preset messages

The list of preset quick messages is saved in the `messagesList` array. By using that variable, you can access the `QuickMessage` items which contain each an id (used for sending the quick message to the server) and a message (the content of what is sent to the server).

E.g: 
``` 
for (const messageItem of this.messageList)
{
  console.log(`messageId ${messageItem.id} and messageContent ${messageItem.message}`)
}
```
The code snippet above will will loop through the `this.messageList` the list of preset message and print their Id and their content.

Feel free to make a PR if you would like to add a interesting feature to this QuickChat Module.


