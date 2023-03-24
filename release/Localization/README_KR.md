# Localization Module Import Guide

[English](./README.md) | [Korean](./README_KR.md)

## Import Guide
1. Localization 모듈 최신버전을 내 프로젝트로 import합니다.
2. ZepetoLocalizationModule/SampleTextFile/ZW_LOCALIZATION_SAMPLE.csv 문서 안에 Key값과 각 언어별 번역 텍스트 내용을 입력한 뒤에 월드내에서 사용하게 되면 자동으로 다국어로 변환이 되어 사용할 수 있습니다. 
    <img width="700" alt="image" src="./Image/GuideImage1.png">   
    > **Note**: csv 파일은 스프레드시트 또는 엑셀, 구글 스프레드시트 등의 프로그램으로 수정할 수 있습니다. 
3. Scene 내 'Localization'이라는 이름의 게임오브젝트를 생성하고, Localization.ts를 붙여줍니다. Localization은 싱글톤 매니저 형태로, 하나만 존재해야 합니다. Local Sheet에 번역문과 키를 담고 있는 csv파일을 넣어줍니다.   
    <img width="700" alt="image" src="./Image/GuideImage2.png">   
4. Text 컴포넌트가 붙어있는 각 게임 오브젝트들에 TextLocalizer.ts를 붙여줍니다. Localization Key 입력칸에 번역이 적용되어서 나올 Key를 정확하게 입력합니다. [▶︎(play)]버튼을 눌러 확인하면 번역 텍스트가 적용되어 변경되는것을 확인할 수 있습니다.   
    <img width="700" alt="image" src="./Image/GuideImage3.png">   


## Use Tip
#### 런타임 언어 변경 방법
- 샘플로 제공된 Prefab폴더에 위치한 LanguageChanger와 LocalizationSample을 확인해보세요.

#### 스크립트로 텍스트 접근하는 방법
- 아래와 같은 방법으로 제페토 스크립트로를 통해 직접 텍스트에 접근할 수 있습니다.
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
            // You have {0} heart left. 
            this._text.text = this._localization.GetLocalizedTextWithParam(localizationKeys.SAMPLE_TEXT_WITH_PARAMS, ["10"]);
            // result: You have 10 heart left.
        }
    }
    ```

#### 서식 텍스트 무시하기
- Disable Rich Text를 체크해서 서식이 있는 텍스트 포맷을 무시할 수 있습니다.

