import { Text } from "UnityEngine.UI";
import { ZepetoScriptBehaviour } from "ZEPETO.Script";
import Localization from "./Localization";

export default class ThaiFontApplier extends ZepetoScriptBehaviour {
    Start() {    
        this.gameObject.GetComponent<Text>().font = Localization.instance.GetDefaultThaiFont();
    }
}