import { ZepetoScriptBehaviour } from 'ZEPETO.Script'
import {Image, Text, Toggle, ToggleGroup} from 'UnityEngine.UI'
import {GameObject} from 'UnityEngine'
import {InventoryRecord} from "ZEPETO.Inventory";

export default class ITM_Inventory extends ZepetoScriptBehaviour {
    public itemRecord:InventoryRecord;
    public itemImage:Image;

    @SerializeField() private quantityTxt :Text;
    @SerializeField() private isOnUIObj : GameObject;
    
    private Start() {
        this.GetComponent<Toggle>().group = this.transform.parent.GetComponent<ToggleGroup>();
    }
    
    public RefreshItem(ir:InventoryRecord, isShowQuantity:boolean = true){
        this.itemRecord = ir;
        this.quantityTxt.text = isShowQuantity ? this.itemRecord.quantity.toString() : "";
    }
    
    public isOn(boolean:boolean){
        this.GetComponent<Toggle>().isOn = boolean;
        this.isOnUIObj.SetActive(boolean);
    }

}