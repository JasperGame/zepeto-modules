import { ZepetoScriptBehaviour } from 'ZEPETO.Script'
import {ProductRecord, ProductService} from 'ZEPETO.Product'
import {Button, Image, Text} from "UnityEngine.UI";

export default class ITM_ShopProduct extends ZepetoScriptBehaviour {
    public productRecord:ProductRecord;
    
    @SerializeField() private currencyQuantityTxt :Text
    @SerializeField() private requireZemTxt : Text;
    @SerializeField() private purchaseBtn : Button;
    
    public RefreshProduct(product : ProductRecord){
        this.productRecord = product;
        
        if(this.productRecord!= null) {
            this.currencyQuantityTxt.text =this.productRecord.currencyPackageUnits[0]?.quantity.toString();
            this.requireZemTxt.text = this.productRecord.price.toString();

            // remove past cache listeners
            this.purchaseBtn.onClick.RemoveAllListeners();
            // purchaseBtn addlistner
            this.purchaseBtn.onClick.AddListener(()=>{
                // official UI popup open
                ProductService.OpenPurchaseUI(product);
            });
        }
    }

}