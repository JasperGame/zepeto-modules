import { Button, Text } from 'UnityEngine.UI';
import { ZepetoScriptBehaviour } from 'ZEPETO.Script';
import { WorldMultiplayChatContent} from 'ZEPETO.World';
import { GameObject } from 'UnityEngine';


//A function to initialize the button when it is created: messageItemGameObject is the game object on which we will add the click listener, messageItemText is the string we will use to change the content on the button and message if is the message id we will use to call the send the message to the server
export default class QuickMessageListItem extends ZepetoScriptBehaviour {

    Init(messageItemGameObject: GameObject, messageItemText:string, messageItemId:string)
    {

        //Check if the message text exist and update the button text
        if(messageItemText) {this.GetComponentInChildren<Text>().text = messageItemText}

        // Send the message to the server when the button is clicked and print the message on the server.
        messageItemGameObject.GetComponent<Button>().onClick.AddListener(() => {
            WorldMultiplayChatContent.instance.SendQuickMessage(messageItemId)
            console.log(`${ messageItemText } was sent`)
        })

    }
}