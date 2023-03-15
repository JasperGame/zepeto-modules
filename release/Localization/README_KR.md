# Localization Module Import Guide

[English](./README.md) | [Korean](./README_KR.md)

### Import Guide
1. 최신버전을 내 프로젝트로 import합니다.
2. 스프레드 시트 또는 엑셀을 이용해서 번역문과 Key값을 만들 수 있습니다. 기본으로 제공된 SampleTextFile/ZW_LOCALIZATION_SAMPLE.csv 파일에 필요한 key값을 추가해보세요.   
    <img width="400" alt="image" src="https://user-images.githubusercontent.com/123578202/225220567-202d7bf9-38e2-4594-8af6-6fedf09f40a3.png"> 
3. 씬 내 'Localization'이라는 이름의 게임오브젝트를 생성하고, Localization.ts를 붙여줍니다. Localization은 싱글톤 매니저 형태로, 하나만 존재해야 합니다. Local Sheet에 번역문과 키를 담고 있는 csv파일을 넣어줍니다.   
    <img width="400" alt="image" src="https://user-images.githubusercontent.com/123578202/225218798-2d08bd5e-4347-4293-a4a1-3385974b20c9.png">   
4. Text 컴포넌트가 붙어있는 각 게임 오브젝트들에 TextLocalizer.ts를 붙여줍니다. Localization Key를 정확하게 입력합니다.
    <img width="400" alt="image" src="https://user-images.githubusercontent.com/123578202/225219301-89c4255d-6d2c-44fb-bed4-d0f17742a539.png">


### Use Tip
- 샘플로 제공돤 Prefab폴더에 위치한 LanguageChanger와 LocalizationSample을 확인해보세요.
- 아래와 같은 방법으로 제페토 스크립트로도 접근할 수 있습니다.
```typescript
export default class Sample extends ZepetoScriptBehaviour {
    private _localization: Localization;
    private _text: Text;
 
    Start() {   
        this._localization = Localization.Instance;
    }
 
    SetText() {
        this._text.text = this._localization.GetLocalizedText(localizationKeys.SAMPLE_TEXT);
        // result: This is sample text
    }
 
    SetTextWithParams() {
        this._text.text = this._localization.GetLocalizedTextWithParam(localizationKeys.SAMPLE_TEXT_WITH_PARAMS, ["1"]);
        // result: You have 1 heart left.
    }
}
```

