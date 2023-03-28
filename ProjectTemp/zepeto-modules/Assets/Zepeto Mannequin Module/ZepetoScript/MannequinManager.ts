import { ZepetoScriptBehaviour } from 'ZEPETO.Script'
import { Room } from 'ZEPETO.Multiplay';
import {
    ClothesPreviewer,
    ItemContent, ItemContentsRequest,
    Mannequin,
    MannequinComponent,
    MannequinInteractable,
    MannequinPreviewer
} from "ZEPETO.Mannequin";
import {ZepetoContext} from "Zepeto";
import {ZepetoPlayer, ZepetoPlayers} from "ZEPETO.Character.Controller";
import {GameObject, Object, Canvas, WaitForSecondsRealtime, LayerMask} from "UnityEngine";
import {Player} from "ZEPETO.Multiplay.Schema";
import { ZepetoWorldMultiplay } from 'ZEPETO.World';

class CharacterItem{
    id:string;
    property:string;
}

class ChangedItem{
    sessionId:string;
    characterItems:CharacterItem[];
}

export default class MannequinManager extends ZepetoScriptBehaviour {
    private MESSAGE_TYPE = {
        OnChangedItem:"OnChangedItem",
        SyncChangedItem:"SyncChangedItem",
        CheckChangedItem:"CheckChangedItem"
    }
    
    private multiplay:ZepetoWorldMultiplay;
    private _room: Room;
    private _previewer: MannequinPreviewer
    private _context : ZepetoContext

    private _userZepetoContexts:Map<string,ZepetoContext> = new Map<string, ZepetoContext>();
    private _currentMannequinComponent:MannequinComponent = null;
    private _selectMannequinComponent: MannequinComponent = null;
    private _isOpenMannequinUI: boolean = false;
    
    private _basicClothString = "basicCloth" as const;

    Start() {
        this.multiplay = Object.FindObjectOfType<ZepetoWorldMultiplay>();
        
        Mannequin.OnOpenedShopUI.AddListener((item) => {
            //when you click OpenShopButton
            this.OpenedShopUI(item)
        });
        Mannequin.OnClosedShopUI.AddListener(() => {
            //when you click CloseShopButton
            this.ClosedShopUI()
        });
        Mannequin.OnSelectedItem.AddListener((itemcontent:ItemContent,select:boolean)=>{
            //아이템이 선택되었을 때 행동
        });

        ZepetoPlayers.instance.OnAddedLocalPlayer.AddListener(() => {
            const myPlayer = ZepetoPlayers.instance.LocalPlayer.zepetoPlayer;

            //mannequin
            const character = myPlayer.character;
            character.gameObject.AddComponent<MannequinInteractable>();

            console.log("local _context set");
            this._context = character.Context;

            const mannequins = Object.FindObjectsOfType<MannequinComponent>()
            mannequins.forEach(mannequin => {
                // when you enter mannequin collider
                mannequin.onActive.AddListener(contents =>{
                    if(this._currentMannequinComponent != null &&
                        this._currentMannequinComponent == mannequin)
                        return;

                    if(contents == null || contents.length == 0)
                    {
                        console.log("No mannequin data");
                        return;
                    }

                    if(this._isOpenMannequinUI) {
                        this.BreakMannequin();
                    }

                    this._selectMannequinComponent = mannequin;
                    Mannequin.OpenUI( contents );
                    console.log("onActive");
                });

                // [Option] when you leave mannequin collider
                mannequin.onCancel.AddListener( () =>
                {
                    if(this._currentMannequinComponent == null ||
                        this._currentMannequinComponent != mannequin)
                        return;
                    
                    this.BreakMannequin();
                    console.log("onCancel");
                });

                let iconCanvas = mannequin.gameObject.GetComponentInChildren<Canvas>(true);
                if(iconCanvas != null){
                    iconCanvas.gameObject.layer = LayerMask.NameToLayer("UI");
                }
            });
        });

        ZepetoPlayers.instance.OnAddedPlayer.AddListener((sessionId: string) => {
            if (ZepetoPlayers.instance.GetPlayer(sessionId).isLocalPlayer) {
                return;
            }
            const userContext = ZepetoPlayers.instance.GetPlayer(sessionId).character.Context;
            this._userZepetoContexts.set(sessionId,userContext);
            this._room.Send(this.MESSAGE_TYPE.CheckChangedItem,sessionId);
        });
        this.multiplay.RoomJoined += (room: Room) => {
            this._room = room;
            this.AddMessageHandler();
        };
    }

    private AddMessageHandler(){
        // [Option] Synchronize each player's clothes
        this._room.AddMessageHandler<ChangedItem>(this.MESSAGE_TYPE.SyncChangedItem, message => {
            console.log("syncChangedItem");
            if (message == null) {
                return;
            }

            if(false == this._userZepetoContexts.has(message.sessionId)){
                return;
            }

            let itemContents:ItemContent[] = [];

            for (const characterItem of message.characterItems) {
                let itemContent:ItemContent = new ItemContent();
                itemContent.id = characterItem.id;
                itemContent.property = parseInt(characterItem.property);

                if(itemContent.id == this._basicClothString)
                    itemContent.id = "";

                itemContents.push(itemContent);
            }
            let clothesPreviewer:ClothesPreviewer = new ClothesPreviewer(this._userZepetoContexts.get(message.sessionId),itemContents);
            clothesPreviewer.PreviewContents();
        });
    }

    private ClosedShopUI(){
        this._currentMannequinComponent = null;
        this._isOpenMannequinUI = false;
    }

    private OpenedShopUI(items : ItemContent[]) {
        this._isOpenMannequinUI = true;
        this._currentMannequinComponent = this._selectMannequinComponent;
        
        this._previewer = new MannequinPreviewer( this._context, items );
        this._previewer.OnChanged.AddListener((changeValues)=>{
            let characterItems:CharacterItem[] = [];

            for (const changeValue of changeValues) {
                let characterItem:CharacterItem = new CharacterItem();
                characterItem.id =   changeValue.id;
                characterItem.property = changeValue.property.toString();

                if(characterItem.id == "")
                    characterItem.id = this._basicClothString;

                characterItems.push(characterItem);
            }
            this._room.Send(this.MESSAGE_TYPE.OnChangedItem, characterItems);
        });

        this._previewer.PreviewContents();
    }

    public BreakMannequin(){
        Mannequin.CloseUI();
        if (this._previewer){
            this._previewer.ResetContents();
            this._previewer = null;
        }

        this._currentMannequinComponent = null;
        this._isOpenMannequinUI = false;
    }

}