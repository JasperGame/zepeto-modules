import { Button, Text } from "UnityEngine.UI";
import { ZepetoScriptBehaviour } from "ZEPETO.Script";
import Localization, { LanguageOption } from "./Localization";

const _numberOfLanguage:number = 15;
export default class LanguageChanger extends ZepetoScriptBehaviour {
    public nextButton: Button;
    public prevButton: Button;
    public currentLangKeyText: Text;
    
    private _currentLanguageKeyIndex: number = 0;

    Start() {
        this.UpdateCurrnetLangKeyText();
        this.GetCurrentLanguageKeyIndex();
        Localization.instance.languageOptionChanged.AddListener(()=> {
            this.UpdateCurrnetLangKeyText();
        });
        this.nextButton.onClick.AddListener(()=>this.NextLanguage());
        this.prevButton.onClick.AddListener(()=>this.PrevLanguage());
    }
    
    public NextLanguage() {
        this._currentLanguageKeyIndex++;
        if (this._currentLanguageKeyIndex > _numberOfLanguage) {
            this._currentLanguageKeyIndex = 0;
        }
        
        Localization.instance.ChangeLanguageOption(this._currentLanguageKeyIndex);
     }

    public PrevLanguage() {
        this._currentLanguageKeyIndex--;
        if (this._currentLanguageKeyIndex < 0) {
            this._currentLanguageKeyIndex = _numberOfLanguage;
        }
        
        Localization.instance.ChangeLanguageOption(this._currentLanguageKeyIndex);
    }

    private GetCurrentLanguageKeyIndex() {
        const key: string = Localization.instance.currentLanguageKey;
        let targetIndex: number = 0;
        for (let i = 0; i < _numberOfLanguage; i++) {
            if (LanguageOption[i] == key) {
                targetIndex = i;
            }
        }
        this._currentLanguageKeyIndex = targetIndex;
    }

    private UpdateCurrnetLangKeyText() {
        this.currentLangKeyText.text = Localization.instance.currentLanguageKey;
    }
}
