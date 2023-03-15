import { InputField, Text } from "UnityEngine.UI";
import { ZepetoScriptBehaviour } from "ZEPETO.Script";
import Localization, { LanguageOption } from "./Localization";

export default class InputFieldLocalizer extends ZepetoScriptBehaviour {
    public localizationKey: string;
    public disableRichText: boolean;

    private _isInitialized: boolean = false;
    private _inputField: InputField;
    private _uiText: Text;
    private _localizedText: string;

    public get localizedText() {
        if (!this._localizedText) {
            this._localizedText = "Not localized yet";
        }
        return this._localizedText;
    }

    Awake() {
        this._inputField = this.gameObject.GetComponent<InputField>();
        this._uiText = this._inputField.textComponent;
    }

    OnEnable() {
        if (this._isInitialized) {
            this.UpdateInputFieldPlaceholder();
        }
    }

    Start() {
        if (!this.localizationKey || this.localizationKey == "") {
            this._isInitialized = false;
            console.error(`[TextLocalizer] Text Localizer is not initialized. Enter localization key first.(${this.gameObject.name})`);
            return;
        } else {
            this._isInitialized = true;
        }

        // When UI text doesn't exist
        if (!this.gameObject.GetComponent<InputField>()) {
            console.error(`[TextLocalizer] Text Localizer is not initialized. There's no UI InputField. (${this.gameObject.name})`);
            return;
        }

        this.UpdateInputFieldPlaceholder();
        Localization.instance.languageOptionChanged.AddListener(()=> {
            this.UpdateInputFieldPlaceholder();
        });
    }

    private UpdateInputFieldPlaceholder() {
        if (Localization.instance.CheckThai()) {
            this._uiText.font = Localization.instance.GetDefaultThaiFont();
        }

        if (this.disableRichText) {
            this._uiText.supportRichText = false;
        }

        const localizedText: string = Localization.instance.GetLocalizedText(this.localizationKey);
        this._inputField.text = localizedText;
        this._localizedText = localizedText;
    }
}