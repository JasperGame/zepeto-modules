import {ZepetoScriptBehaviour} from 'ZEPETO.Script'
import {Button, Text} from 'UnityEngine.UI'
import {WaitUntil, GameObject} from 'UnityEngine'
import {CurrencyService} from "ZEPETO.Currency";
import {CurrencyPackageUnitRecord, ProductRecord, ProductService, ProductType} from "ZEPETO.Product";
import ITM_ShopProduct from './ITM_ShopProduct'

export default class UIShop extends ZepetoScriptBehaviour {
    @SerializeField() private possessionZemTxt : Text;
    @SerializeField() private shopProducts : GameObject[];
    
    private _products : ProductRecord[];
    
    private Start() {
        this.StartCoroutine(this.RefreshZemUI());
        this.StartCoroutine(this.RefreshProducts());
        
        ProductService.OnPurchaseCompleted.AddListener((productRecord, response)=>{
            this.StartCoroutine(this.RefreshZemUI());
        });
    }
    
    private * RefreshZemUI(){
        const request = CurrencyService.GetOfficialCurrencyBalanceAsync();
        
        yield new WaitUntil(()=>request.keepWaiting == false);
        if(request.responseData.isSuccess) {
            this.possessionZemTxt.text = request.responseData.currency.quantity.toString();
        }
    }
    
    private * RefreshProducts(){
        const request = ProductService.GetProductsAsync();
        yield new WaitUntil(()=>request.keepWaiting == false);

        if (!request.responseData || !request.responseData.isSuccess) {
            console.warn("Refresh Products Failed");
            console.warn("See the Product docs <color=blue><a>https://naverz-group.readme.io/studio-world/docs/zepeto_product</a></color> for more information.");
            return;
        }

        let currencyPackageIndex = 0;
        for (const product of request.responseData.products || []) {
            if (product.ProductType === ProductType.CurrencyPackage && currencyPackageIndex < this.shopProducts.length) {
                this.shopProducts[currencyPackageIndex].GetComponent<ITM_ShopProduct>().RefreshProduct(product);
                currencyPackageIndex++;
            }
        }
    }

}
