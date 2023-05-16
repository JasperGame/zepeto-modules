# How to use the quickChat Module in your Zepeto Project ?

The QuickChat module is the is used for sending in game predefined messages in a Zepeto Multiplayer world. 

In this documentation, you will learn how to add a QuickChat module in your existing zepeto Project. If you are just getting started as a developer in Zepeto, we suggest you to  check out our [beginner guide](https://docs.zepeto.me/studio-item/lang-ko/docs) first. 


## Adding the QuickChat Module into my project


- Open the **Zepeto Module Importer** in the Zepeto project where you will add the QuickChat functionality.
  
- Import the QuickChat Module. while importing the module make sure that you also import the multiplay component as this functionality only works in a Multiplayer world. In case you already have a multiplay/ you are planning to add one on your own, feel free to uncheck the multiplay component.
  
- Drag and drop the **QuickChat Canvas** prefab located at "*Zepeto QuickChat Module > UI > Prefabs*" into your scene. 

Fee free to customize the QuickChat Component/UI depending on your requirements.

This module works only in a multiplay world, make sure to check out the multiplay component first before trying.


## Getting the list of preset messages

The list of preset quick messages is saved in the the `messagesList` array. By using that variable, you can access the `QuickMessage` items which contain each an id (used for sending the quick message to the server) and a message (the content of what is sent to the server).


Feel free to make a PR if you would like to add a interesting feature to this QuickChat Module ^^


