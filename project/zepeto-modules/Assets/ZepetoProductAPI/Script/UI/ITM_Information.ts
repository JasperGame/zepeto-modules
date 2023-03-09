import { ZepetoScriptBehaviour } from 'ZEPETO.Script'
import {GameObject, WaitForSeconds} from "UnityEngine";

export default class ITM_Information extends ZepetoScriptBehaviour {
    public destroyDelay:number = 0.7;

    private Start() {    
        this.StartCoroutine(this.DestoryInformation());
    }
    
    private * DestoryInformation(){
        yield new WaitForSeconds(this.destroyDelay);
        GameObject.Destroy(this.gameObject);
    }
    

}