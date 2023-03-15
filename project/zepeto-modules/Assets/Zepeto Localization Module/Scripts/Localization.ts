import { Application, Font, GameObject, Resources, SystemLanguage, TextAsset} from 'UnityEngine';
import { Text } from "UnityEngine.UI";
import { ZepetoScriptBehaviour } from 'ZEPETO.Script';
import { UnityEvent } from "UnityEngine.Events";

export enum LanguageOption { "DeviceLanguage", "kr", "en", "jp", "zh-sc-gl", "zh-sc-zai", "th", "id", "ru", "it", "pt", "es", "vi", "zh-hant", "fr", "tr" }

export default class Localization extends ZepetoScriptBehaviour {
    public localSheet: TextAsset;
    public language: LanguageOption = LanguageOption.DeviceLanguage;

    private readonly _languageOptionChanged:UnityEvent = new UnityEvent();
    private readonly _localizedTextMap = new Map<string, string>();

    private _defaultThaiFont: Font;
    private _originFont: Font;
    private _currentLanguageKey: string;

    public get languageOptionChanged(): UnityEvent { return this._languageOptionChanged; }
    public get currentLanguageKey(): string { return this._currentLanguageKey; }
    
    private static _instance: Localization = null;
    public static get instance(): Localization {
        if (this._instance === null) {
            this._instance = GameObject.FindObjectOfType<Localization>();
            if (this._instance === null) {
                this._instance = new GameObject(Localization.name).AddComponent<Localization>();
            }
        }
        return this._instance;
    }

    Awake() {
        if (Localization._instance !== null && Localization._instance !== this) {
            GameObject.Destroy(this.gameObject);
        } else {
            Localization._instance = this;
            GameObject.DontDestroyOnLoad(this.gameObject);
        }
        this._defaultThaiFont = Resources.Load("NotoSansThai-Regular") as Font;
        this.SetLanguage();
    }

    public ChangeLanguageOption(targetLanguage: LanguageOption) {
        this.language = targetLanguage;
        this.SetLanguage();
        this._languageOptionChanged.Invoke();
    }

    public ApplyLocalization(text: Text, key: string) {
        if (this.CheckThai()) {
            text.font = this._defaultThaiFont;
        }
        text.text = this.GetLocalizedText(key);
    }

    public ApplyLocalizationWithParam(text: Text, key: string, args: string[]) {
        if (this.CheckThai()) {
            text.font = this._defaultThaiFont;
        }
        text.text = this.GetLocalizedTextWithParam(key, args);
    }

    /** Returns localized text value from give key stirng */
    public GetLocalizedText(key: string): string {
        // if value is invalid,
        if (!this._localizedTextMap.has(key)) {
            console.warn("[Localization]: Invalid Value");
            return "Not Translated Yet";
        }
        return this._localizedTextMap.get(key);
    }

    /** Returns localized text value from give key stirng */
    public GetLocalizedTextWithParam(key: string, args: string[]): string {
        return this.GetLocalizedText(key).replace(/{(\d+)}/g, (match, index) => args[index] || '');
    }

    public GetDefaultThaiFont(): Font {
        return this._defaultThaiFont;
    }

    public CheckThai(): boolean {
        return this._currentLanguageKey === LanguageOption[LanguageOption.th].toString();
    }

    private SetLanguage() {

        if (this.language == 0) {
            this.SetLanguageFromSystemLanguage();
            console.log(`[Localization] Current Language is set to ${this._currentLanguageKey} by System Language`);
        } else {
            this._currentLanguageKey = LanguageOption[this.language].toString();
            console.log(`[Localization] Current Language is set to ${this._currentLanguageKey}, LanguageOption: ${LanguageOption[this.language].toString()}, this.language: ${this.language}`);
        }

        // Parse the common module localization CSV.
        const textFile = Resources.Load("ZW_LOCALIZATION_COMMON") as TextAsset;
        this.SetLocalizationMapFromCSV(this._localizedTextMap, textFile, this._currentLanguageKey);

        // Parse the content module localization CSV.
        if (!this.localSheet) {
            console.warn("[Localization]: There's no text file to read.");
        } else {
            this.SetLocalizationMapFromCSV(this._localizedTextMap, this.localSheet, this._currentLanguageKey);
        }        
    }

    private SetLanguageFromSystemLanguage() {
        switch (Application.systemLanguage) {
            case SystemLanguage.Korean:
                this._currentLanguageKey = LanguageOption[LanguageOption.kr].toString();
                break;
            case SystemLanguage.English:
                this._currentLanguageKey = LanguageOption[LanguageOption.en].toString();
                break;
            case SystemLanguage.Japanese:
                this._currentLanguageKey = LanguageOption[LanguageOption.jp].toString();
                break;
            case SystemLanguage.Chinese:
                this._currentLanguageKey = LanguageOption[LanguageOption['zh-sc-gl']].toString();
                break;
            case SystemLanguage.ChineseSimplified:
                this._currentLanguageKey = LanguageOption[LanguageOption['zh-sc-gl']].toString();
                break;
            case SystemLanguage.Thai:
                this._currentLanguageKey = LanguageOption[LanguageOption.th].toString();
                break;
            case SystemLanguage.Indonesian:
                this._currentLanguageKey = LanguageOption[LanguageOption.id].toString();
                break;
            case SystemLanguage.Russian:
                this._currentLanguageKey = LanguageOption[LanguageOption.ru].toString();
                break;
            case SystemLanguage.Italian:
                this._currentLanguageKey = LanguageOption[LanguageOption.it].toString();
                break;
            case SystemLanguage.Portuguese:
                this._currentLanguageKey = LanguageOption[LanguageOption.pt].toString();
                break;
            case SystemLanguage.Spanish:
                this._currentLanguageKey = LanguageOption[LanguageOption.es].toString();
                break;
            case SystemLanguage.Vietnamese:
                this._currentLanguageKey = LanguageOption[LanguageOption.vi].toString();
                break;
            case SystemLanguage.ChineseTraditional:
                this._currentLanguageKey = LanguageOption[LanguageOption['zh-hant']].toString();
                break;
            case SystemLanguage.French:
                this._currentLanguageKey = LanguageOption[LanguageOption.fr].toString();
                break;
            case SystemLanguage.Turkish:
                this._currentLanguageKey = LanguageOption[LanguageOption.tr].toString();
                break;
            // case SystemLanguage.Arabic:
            //     this._currentLanguageKey = LanguageOption[LanguageOption.ar].toString();
            //     break;
            default:
                this._currentLanguageKey = LanguageOption[LanguageOption.en].toString();
                break;
        }
    }

    private SetLocalizationMapFromCSV(targetMap: Map<string, string>, targetTextAsset: TextAsset, targetLanguageKey: string) {
        const csv: string = targetTextAsset.ToString();
        const lines: string[] = csv.split("\n");
        const headers: string[] = lines[0].split(",");
        const targetLanguageIndex: number = headers.findIndex(header => header.trim() === targetLanguageKey);

        if (targetLanguageIndex === -1) {
            console.error(`[Localization]: Error occured during parsing CSV file. The target language key "${targetLanguageKey}" was not found.`);
            return;
        }

        // In Google Spreadsheet, the last line does not include \r\n, so we add it as follows.
        const lastLine: string = lines[lines.length - 1];
        if (lastLine.length > 0 && !lastLine.endsWith("\r\n")) {
            lines[lines.length - 1] += "\r\n";
        }

        for (let i = 1; i < lines.length; i++) {
            // In Google Spreadsheet, if a comma is included in a cell, the cell is enclosed in double 
            if (lines[i].length === 0) {
                continue;
            }
            const columns: string[] = lines[i].match(/(".*?"|[^",\r\n]+)(?=[,\r\n])/g).map(column => column.replace(/"/g, ''));

            const keyColumn = columns[0];
            if (!keyColumn) {
                console.error(`[Localization]: Error occured during parsing CSV file. Error caused by the first column of the file.`);
                continue;
            }
            const key: string = keyColumn.trim();

            const valueColumn = columns[targetLanguageIndex];
            if (!valueColumn) {
                console.error(`[Localization]: Error occured during parsing CSV file. Error casued by key: ${key}, valueColumn: ${valueColumn}, targetLanguageIndex: ${targetLanguageIndex}}, lines[i].length: ${lines[i].length}, columns.length: ${columns.length}`);
                continue;
            }
            const value: string = valueColumn.trim().replace(/<br>/gi, '\n');
            if (!value) {
                console.error(`[Localization]: Error occured during parsing CSV file. Error casued by key: ${key}, valueColumn: ${valueColumn}, targetLanguageIndex: ${targetLanguageIndex}}, value: ${value}, lines[i].length: ${lines[i].length}, columns.length: ${columns.length}`);
                continue;
            }
            targetMap.set(key, value);
        }
    }
}
