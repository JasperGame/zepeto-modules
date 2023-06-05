# How to use the quickChat Module in your Zepeto Project ?

The QuickChat module is the is used for sending in game predefined messages in a Zepeto Multiplayer world. 

In this documentation, you will learn how to add a QuickChat module in your existing zepeto Project. If you are just getting started as a developer in Zepeto, we suggest you to  check out our [beginner guide](https://docs.zepeto.me/studio-world/docs/welcome_zepeto_developers) first. 


## Adding the QuickChat Module into my project


- Open the **Zepeto Module Importer** by clicking on ZEPETO tab > Module Importer (1) in the Zepeto project where you will add the QuickChat functionality.
  
- Select the quick chat module in the list of module on the left tap of the module importer
- Click Import  to import the QuickChat Module. 
- If you did not set up the multiplay feature, make sure that while importing the module you also import the `Zepeto Multiplay Component` and the `World.multiplay` (note the QuickChat feature works only in a multiplay world). In case you already have a multiplay or you are planning to add one on your own, feel free to uncheck the `Zepeto Multiplay Component` and the `World.multiplay` before clicking the import button at the bottom.

![Import the QuickChat file](https://github.com/naverz-LeGrandMAG/zepeto-modules/assets/131629767/a5466575-a0d6-4db0-bf53-fb415e9e8e89)

![Uncheck the Zepeto Multiplay Component & World.Multiplay](https://github.com/naverz-LeGrandMAG/zepeto-modules/assets/131629767/0d544f6d-8313-4cc6-8509-34ff53389839)
  
- Drag and drop the **QuickChat Canvas** prefab located at "*Zepeto QuickChat Module > UI > Prefabs*" into your scene. 
![Drag and drop QuickChat Canvas](https://github.com/naverz-LeGrandMAG/zepeto-modules/assets/131629767/5f52e476-ff23-42f8-9a18-7efe3fe4320b)

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


