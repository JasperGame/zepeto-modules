import { Text } from "UnityEngine.UI";
import { ZepetoScriptBehaviour } from "ZEPETO.Script";
import Localization, { LanguageOption } from "./Localization";

export default class ThaiFontApplierSelective extends ZepetoScriptBehaviour {
    Start() {    
        const uiText = this.gameObject.GetComponent<Text>();
        if (Localization.instance.currentLanguageKey == LanguageOption[LanguageOption.th].toString()) {
            uiText.font = Localization.instance.GetDefaultThaiFont();
        }
    }
}