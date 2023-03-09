import { ZepetoScriptBehaviour } from 'ZEPETO.Script'
import {Text, Button, InputField, Slider} from 'UnityEngine.UI'
import {Object, WaitForSeconds, WaitUntil} from 'UnityEngine'
import {InventoryService} from "ZEPETO.Inventory";
import {BalanceListResponse, CurrencyService, CurrencyError} from "ZEPETO.Currency";
import {ProductRecord, ProductService} from "ZEPETO.Product";
import {ZepetoWorldMultiplay} from "ZEPETO.World";
import {RoomData, Room} from "ZEPETO.Multiplay";

export default class UIBalances extends ZepetoScriptBehaviour {
    @SerializeField() private possessionStarTxt : Text;
    @SerializeField() private possessionEnergyTxt : Text;
    @SerializeField() private possessionZemTxt : Text;
    @SerializeField() private possessionExpTxt : Text;
    @SerializeField() private possessionAmountExpTxt : Text;
    @SerializeField() private possessionLevelTxt : Text;
    @SerializeField() private expSlider : Slider;

    private _multiplay : ZepetoWorldMultiplay;
    private _room : Room
    private _myExp :number = 0;
    private _amountExp :number = 30;
    private _myLevel:number = 1;    

    private Start() {
        this.RefreshAllBalanceUI();
        this.RefreshExpUI();
        this._multiplay = Object.FindObjectOfType<ZepetoWorldMultiplay>();

        this._multiplay.RoomJoined += (room: Room) => {
            this._room = room;
            this.InitMessageHandler();
        }
    }
    
    private InitMessageHandler() {
        this._room.AddMessageHandler<BalanceSync>("SyncBalances", (message) => {
            this.RefreshAllBalanceUI();
        });
        ProductService.OnPurchaseCompleted.AddListener((product, response) => {
            this.RefreshAllBalanceUI();
        });
    }
    
    private RefreshAllBalanceUI(){
        this.StartCoroutine(this.RefreshBalanceUI());
        this.StartCoroutine(this.RefreshOfficialCurrencyUI());
    }
    
    private *RefreshBalanceUI(){
        const request = CurrencyService.GetUserCurrencyBalancesAsync();
        yield new WaitUntil(()=>request.keepWaiting == false);
        if(request.responseData.isSuccess) {
            this.possessionStarTxt.text = request.responseData.currencies?.ContainsKey(Currency.star) ? request.responseData.currencies?.get_Item(Currency.star).toString() :"0";
            this.possessionEnergyTxt.text = request.responseData.currencies?.ContainsKey(Currency.energy) ? request.responseData.currencies?.get_Item(Currency.energy).toString() :"0";
        }
    }
    
    private *RefreshOfficialCurrencyUI(){
        const request = CurrencyService.GetOfficialCurrencyBalanceAsync();
        yield new WaitUntil(()=>request.keepWaiting == false);
        this.possessionZemTxt.text = request.responseData.currency.quantity.toString();
    }
    
    public IncreaseExp(quantity:number){
        this._myExp += quantity;
        if(this._myExp >= this._amountExp){
            this._myLevel++;
            this._myExp -= this._amountExp;
            this.LevelUpReward();
        }
        this.RefreshExpUI();
    }
    
    private RefreshExpUI(){
        this.possessionExpTxt.text = this._myExp.toString();
        this.possessionLevelTxt.text = this._myLevel.toString();
        this.expSlider.value = this._myExp / this._amountExp;
    }
    
    private LevelUpReward(){
        // Get 5 stars for every level you raise
        const data = new RoomData();
        data.Add("currencyId", Currency.star);
        data.Add("quantity", 5);
        this._multiplay.Room?.Send("onCredit", data.GetObject());
    }
    
}



export interface BalanceSync {
    currencyId: string,
    quantity: number,
}

export interface InventorySync {
    productId: string,
    inventoryAction: InventoryAction,
}

export enum InventoryAction{
    Removed = -1,
    Used,
    Added,
}

export enum Currency{
    star = "star",
    energy = "energy",
}
