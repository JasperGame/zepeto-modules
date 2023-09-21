import { ZepetoScriptBehaviour } from 'ZEPETO.Script';
import { ZepetoWorldContent, QuickMessage } from 'ZEPETO.World';
import {GameObject, Transform} from 'UnityEngine'
import QuickMessageListItem from './QuickMessageListItem';


export default class QuickChatLoader extends ZepetoScriptBehaviour {
    

    //Messages list is an array of QuickMessages retrieved from Zepeto's DB.
    @HideInInspector() public messagesList:QuickMessage[] = [];

    @SerializeField() private messageItemPrefab: GameObject;
    @SerializeField() private _contentsParent: Transform;

    @SerializeField() private openButton: GameObject;
    @SerializeField() private panelParent: GameObject;




    Start() {

        this.ReceiveChatList();
    }

    Update(){
        // Hide the QuickChat button when the parent content's parent panel is visible
        if (this.panelParent.activeSelf === true) { this.openButton.SetActive(false)}

        // Unhide the QuickChat button when the parent panel is hidden
        else{
            this.openButton.SetActive(true)
        }
    }

    //A method to load all the quickChat messages preset from Zepeto's DB.
    private ReceiveChatList()
    {
        ZepetoWorldContent.GetQuickMessageList(quickMessageList => {
            quickMessageList.forEach((quickMessage: QuickMessage, index: number, array: QuickMessage[]) => {
                this.CreateMessage(quickMessage)
            });
        }, err => {
            //log an error message if there is an error while trying to retrieve the message.
            console.log(`QuickMessage Error: ${err}`);
        });
        
    }

    // A method to create individual message GameObjects (with text and button)
    private CreateMessage(quickMessageItem:QuickMessage)
    {   
        // Add the created QuickMessageItem to the messageLists
        this.messagesList.push(quickMessageItem)


        // Create an instance of the messageItemPrefab
        const newMessage: GameObject = GameObject.Instantiate(this.messageItemPrefab, this._contentsParent) as GameObject;
        
        // Initialize the messageItem
        const messageItem: QuickMessageListItem = newMessage.GetComponent<QuickMessageListItem>();
        messageItem.Init(newMessage, quickMessageItem.message, quickMessageItem.id)
    }

}