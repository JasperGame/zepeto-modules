import {ZepetoScriptBehaviour} from 'ZEPETO.Script'
import {Button, Text, ToggleGroup} from 'UnityEngine.UI'
import {GameObject, Object, Sprite, Transform, WaitUntil} from 'UnityEngine'
import {InventoryRecord, InventoryService} from "ZEPETO.Inventory";
import {CurrencyService} from "ZEPETO.Currency";
import {ProductRecord, ProductService, PurchaseType} from "ZEPETO.Product";
import {ZepetoWorldMultiplay} from "ZEPETO.World";
import {Room, RoomData} from "ZEPETO.Multiplay";
import ITM_Inventory from './ITM_Inventory';
import {BalanceSync, InventorySync, Currency} from "./UIBalances";

export default class UIInventory extends ZepetoScriptBehaviour {
    @SerializeField() private usedSlotNumTxt : Text;
    @SerializeField() private possessionStarTxt : Text;
    @SerializeField() private useBtn : Button;
    
    @SerializeField() private contentParent : Transform;
    @SerializeField() private prefItem : GameObject;
    @SerializeField() private itemImage : Sprite[];
    
    private _inventoryCache: InventoryRecord[];
    private _productCache: Map<string, ProductRecord> = new Map<string, ProductRecord>();
    private _multiplay : ZepetoWorldMultiplay;
    private _room : Room;

    private Start() {
        this._multiplay = Object.FindObjectOfType<ZepetoWorldMultiplay>();
        this._multiplay.RoomJoined += (room: Room) => {
            this._room = room;
            this.InitMessageHandler();
        }
        
        this.StartCoroutine(this.LoadAllItems());
    }
    
    private InitMessageHandler(){
        ProductService.OnPurchaseCompleted.AddListener((product, response) => {
            this.StartCoroutine(this.RefreshInventoryUI());
            this.StartCoroutine(this.RefreshBalanceUI());
        });
        this._room.AddMessageHandler<InventorySync>("SyncInventories", (message) => {
            this.StartCoroutine(this.RefreshInventoryUI());
        });
        this._room.AddMessageHandler<BalanceSync>("SyncBalances", (message) => {
            this.StartCoroutine(this.RefreshBalanceUI());
        });
        this.useBtn.onClick.AddListener(()=> this.OnClickUseInventoryItem());
    }

    private* LoadAllItems() {
        const request = ProductService.GetProductsAsync();
        yield new WaitUntil(() => request.keepWaiting == false);
        if (request.responseData.isSuccess) {
            request.responseData.products.forEach((pr) => {
                this._productCache.set(pr.productId,pr);
            });
        }

        this.StartCoroutine(this.RefreshInventoryUI());
        this.StartCoroutine(this.RefreshBalanceUI());
    }
    
    private * RefreshInventoryUI(){
        const request = InventoryService.GetListAsync();
        yield new WaitUntil(()=>request.keepWaiting == false);
        if(request.responseData.isSuccess) {
            const items: InventoryRecord[] = request.responseData.products;
            
            items.forEach((ir, index) => {
                // If there are zero consumable items, delete them from the inventory.
                if (ir.quantity <= 0 && this._productCache.get(ir.productId).PurchaseType == PurchaseType.Consumable) {
                    // remove inventory
                    const data = new RoomData();
                    data.Add("productId", ir.productId);
                    this._multiplay.Room?.Send("onRemoveInventory", data.GetObject());
                    return;
                }

            });

            // If the value matches the previously received value, do not update it.
            if (this._inventoryCache === items) 
                return;
            else if (items != null && this._inventoryCache?.length == items.length) 
                this.UpdateInventory(items);
            else
                this.CreateInventory(items);

            this.usedSlotNumTxt.text = items.length.toString();
            this._inventoryCache = items;
        }
    }
    
    private UpdateInventory(items:InventoryRecord[]){
        const itemScripts = this.contentParent.GetComponentsInChildren<ITM_Inventory>();
        items.forEach((ir)=>{
            itemScripts.forEach((itemScript)=>{
                if(itemScript.itemRecord.productId == ir.productId) {            
                    const isShowQuantity:boolean = this._productCache.get(ir.productId).PurchaseType == PurchaseType.Consumable;
                    itemScript.RefreshItem(ir, isShowQuantity);
                    return;
                }
            });
        });
    }
    
    private CreateInventory(items :InventoryRecord[]){
        this.contentParent.GetComponentsInChildren<ITM_Inventory>().forEach((child)=>{
            GameObject.Destroy(child.gameObject);
        });

        // Sort by Create Order (descending order)
        items.sort((a, b) => b.createdAt.getTime() - a.createdAt.getTime());
        
        items.forEach((ir, index) => {
            const itemObj = Object.Instantiate(this.prefItem, this.contentParent) as GameObject;

            const itemScript = itemObj.GetComponent<ITM_Inventory>();
            this.itemImage.forEach((s, index) => {
                // Import by name comparison from image resources.
                if (s.name == ir.productId) {
                    itemScript.itemImage.sprite = this.itemImage[index];
                    return;
                }
            });
            // Non-consumable items do not display numbers.
            const isShowQuantity:boolean = this._productCache.get(ir.productId).PurchaseType == PurchaseType.Consumable;
            itemScript.RefreshItem(ir, isShowQuantity);
            itemScript.isOn(index == 0);
        });
    }
    
    private *RefreshBalanceUI(){
        const request = CurrencyService.GetUserCurrencyBalancesAsync();
        yield new WaitUntil(()=>request.keepWaiting == false);
        if(request.responseData.isSuccess) {
            this.possessionStarTxt.text = request.responseData.currencies.ContainsKey(Currency.star) ? request.responseData.currencies.get_Item(Currency.star).toString() : "0";
        }
        else{
            console.log(request.responseData.ErrorCode);
        }
    }
    

    private OnClickUseInventoryItem(){
        const toggleGroup = this.contentParent.GetComponent<ToggleGroup>();
        const item = toggleGroup.GetFirstActiveToggle()?.GetComponent<ITM_Inventory>().itemRecord;
        
        if(item == null){
            console.warn("no have item data");
            return;
        }
        if(this._multiplay.Room == null){
            console.warn("server disconnect");
            return;
        }

        const data = new RoomData();
        data.Add("productId", item.productId);
        data.Add("quantity", 1);
        this._multiplay.Room?.Send("onUseInventory", data.GetObject());
    }

}
