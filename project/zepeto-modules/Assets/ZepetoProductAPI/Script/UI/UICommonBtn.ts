import {ZepetoScriptBehaviour} from 'ZEPETO.Script'
import {Button, Text} from 'UnityEngine.UI'
import {GameObject, Object, WaitUntil, WaitForSeconds} from 'UnityEngine'
import {ProductRecord, ProductService, ProductType, PurchaseType} from "ZEPETO.Product";
import {ZepetoWorldMultiplay} from "ZEPETO.World";
import {Room, RoomData} from "ZEPETO.Multiplay";
import UIBallances, {BalanceSync, Currency, InventoryAction, InventorySync} from "./UIBalances";

export default class UICommonBtn extends ZepetoScriptBehaviour {
    @SerializeField() private gainBalanceBtn: Button;
    @SerializeField() private useBalanceBtn: Button;
    @SerializeField() private increaseExpBtn: Button;
    @SerializeField() private acquireRandomItemBtn: Button;
    @SerializeField() private purchaseImmediatelyBtn: Button;
    @SerializeField() private purchaseOfficialUIBtn: Button;
    @SerializeField() private purchaseItemPackageBtn: Button;
    @SerializeField() private informationPref: GameObject;

    private _itemsCache: ProductRecord[] = [];
    private _itemsPackageCache: ProductRecord[] = []
    private _multiplay: ZepetoWorldMultiplay;
    private _room : Room;
    private _uiBallances: UIBallances;

    private Start() {
        this._multiplay = Object.FindObjectOfType<ZepetoWorldMultiplay>();
        this._uiBallances = Object.FindObjectOfType<UIBallances>();

        // button Interval
        let allBtns : Button[] = this.GetComponentsInChildren<Button>();
        allBtns.forEach(btn => btn.onClick.AddListener(() => this.StartCoroutine(this.BtnInterval(btn))));
        
        this.StartCoroutine(this.LoadAllItems());

        this._multiplay.RoomJoined += (room: Room) => {
            this._room = room;
            this.InitMessageHandler();
        }
    }
    
    private InitMessageHandler() {
        //button listener
        this.gainBalanceBtn.onClick.AddListener(() =>this.OnClickGainBalance(Currency.energy, 1));
        this.useBalanceBtn.onClick.AddListener(() => this.OnClickUseBalance(Currency.energy, 1));
        this.increaseExpBtn.onClick.AddListener(() => this.OnClickIncreaseExp());
        this.acquireRandomItemBtn.onClick.AddListener(() => this.OnClickAcquireRandomItem());
        //sell items with id called potion1.
        this.purchaseImmediatelyBtn.onClick.AddListener(() => this.StartCoroutine(this.OnClickPurchaseItemImmediately("potion1")));
        this.purchaseOfficialUIBtn.onClick.AddListener(() => {
            //The first non-consumable item is sold through the official ui.
            const nonConsumableItem = this._itemsCache.find(ir => ir.PurchaseType === PurchaseType.NonConsumable);
            if (nonConsumableItem) {
                this.OnClickPurchaseItem(nonConsumableItem);
            }
            else{
                this.OpenInformation(`Non-consumable product does not exist.`);
            }
        });
        this.purchaseItemPackageBtn.onClick.AddListener(() => {
            const packageItem = this._itemsPackageCache[0];
            if (packageItem) {
                this.OnClickPurchaseItem(packageItem);
            }
            else{
                this.OpenInformation(`Item package product does not exist.`);
            }
        });

        
        // log message handler
        this._room.AddMessageHandler<BalanceSync>("SyncBalances", (message) => {
            this.OpenInformation(`${message.currencyId} a Increase or decrease: ${message.quantity}`);
        });
        this._multiplay.Room.AddMessageHandler<InventorySync>("SyncInventories", (message) => {
            this.OpenInformation(`${message.productId} has been ${InventoryAction[message.inventoryAction]} in the inventory.`);
            // item use sample
            /*if(message.inventoryAction == InventoryAction.Used){
                if(message.productId == "potion1"){
                    console.log("potion use!");
                }
            }*/
        });
        this._room.AddMessageHandler<string>("DebitError", (message) => {
            this.OpenInformation(message);
        });
        ProductService.OnPurchaseCompleted.AddListener((product, response) => {
            this.OpenInformation(`${response.productId} Purchase Completed`);
        });
        ProductService.OnPurchaseFailed.AddListener((product, response) => {
            this.OpenInformation(response.message);
        });
    }

    private* LoadAllItems() {
        const request = ProductService.GetProductsAsync();
        yield new WaitUntil(() => request.keepWaiting == false);
        if (request.responseData.isSuccess) {
            this._itemsCache = [];
            request.responseData.products.forEach((pr) => {
                if (pr.ProductType == ProductType.Item) {
                    this._itemsCache.push(pr);
                }
                if (pr.ProductType == ProductType.ItemPackage) {
                    this._itemsPackageCache.push(pr);
                }
            });

            if (this._itemsCache.length == 0) {
                console.warn("no Item information");
                return;
            }
        }
        else{
            console.warn("Product Load Failed");
        }
    }
    
    private OpenInformation(message:string){
        const inforObj = GameObject.Instantiate(this.informationPref,this.transform.parent) as GameObject;
        inforObj.GetComponentInChildren<Text>().text = message;
    }

    private OnClickGainBalance(currencyId: string, quantity: number) {
        const data = new RoomData();
        data.Add("currencyId", currencyId);
        data.Add("quantity", quantity);
        this._multiplay.Room?.Send("onCredit", data.GetObject());
    }

    private OnClickUseBalance(currencyId: string, quantity: number) {
        const data = new RoomData();
        data.Add("currencyId", currencyId);
        data.Add("quantity", quantity);
        this._multiplay.Room?.Send("onDebit", data.GetObject());
    }
    
    private OnClickIncreaseExp() {
        //This is just a test code that you don't save.
        this._uiBallances.IncreaseExp(10);
    }

    private OnClickAcquireRandomItem() {
        if (this._itemsCache.length == 0) {
            console.warn("Item cache has not yet been loaded.");
            return;
        }
                   
        // Choose one of the consumable items.
        const consumableItem: ProductRecord[] = [];
        this._itemsCache.forEach((pr)=>{
           if(pr.PurchaseType == PurchaseType.Consumable)
               consumableItem.push(pr);
        });
        
        const randNum = Math.floor(Math.random() * consumableItem.length);
        const randItem = consumableItem[randNum];

        const data = new RoomData();
        data.Add("productId", randItem.productId);
        data.Add("quantity", 1);
        this._multiplay.Room?.Send("onAddInventory", data.GetObject());
    }

    // an immediate purchase
    private* OnClickPurchaseItemImmediately(productId: string) {
        const request = ProductService.PurchaseProductAsync(productId);
        yield new WaitUntil(() => request.keepWaiting == false);
        if (request.responseData.isSuccess) {
            // is purchase success
        } else {
            // is purchase fail
        }
    }
    
    // open offical ui
    private OnClickPurchaseItem(productRecord: ProductRecord) {
        ProductService.OpenPurchaseUI(productRecord);
    }
    
    private * BtnInterval(btn:Button){
        btn.interactable = false;
        yield new WaitForSeconds(0.2);

        btn.interactable = true;
    }
}