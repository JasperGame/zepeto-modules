# 기여하는 방법

[English](./HowToContribute.md) | [Korean](./HowToContribute_KR.md)

## ZEPETO Modules Project 기여를 환영합니다!
ZEPETO 월드 개발에 도움을 주는 ZEPETO Modules 제작에 기여해주셔서 진심으로 감사합니다.

다음과 같은 기여를 하실 수 있습니다.

- 버그 보고
- 기능 제안
- 모듈 추가 


## 버그 보고
프로젝트에서 버그나 문제를 발견하면 알려주세요! GitHub 리포지토리에서 이슈를 제출하여 버그를 보고할 수 있습니다. 버그를 보고할 때는 가능한 많은 정보를 제공해 주시기 바랍니다.

- 버그에 대한 명확한 설명
- 버그를 재현하는 단계
- 오류 메시지 또는 콘솔 출력
- 예상 동작 대 실제 동작
- 사용 중인 제페토 월드 버전 및 모듈 버전

## 기능 제안
새로운 기능이나 개선을 제안해 주세요! GitHub 리포지토리에서 새로운 이슈를 열어 기능 요청을 제출할 수 있습니다. 기능을 제안할 때는 다음을 포함해 주세요.

- 기능 또는 개선에 대한 명확한 설명
- 기능이나 개선이 유용하거나 가치 있는 이유
- 잠재적인 단점이나 제한 사항

## 모듈 추가
**귀하가 직접 제작한 모듈 추가를 환영합니다! 모듈을 추가하고자 하는 경우 다음과 같이 수행해 주세요.**

1. 귀하의 GitHub 계정으로 레포지토리를 포크합니다.
2. 포크한 레포지토리를 귀하의 로컬 머신에 클론합니다.
3. 변경 사항을 위한 새 브랜치를 만듭니다.
4. 변경 사항을 수행하고 커밋합니다.
5. 변경 사항을 귀하의 GitHub 포크에 푸시합니다.
6. 변경 사항을 우리 레포지토리에 병합하는 풀 리퀘스트를 엽니다.

**모듈을 추가하는 방법은 다음과 같습니다.**
 
1. 기존 모듈들을 참고하여 형식을 맞춰 제작해주세요.
2. Assets 폴더 내부에 Zepeto (공백이 제거된 Title명) Module 폴더를 추가합니다.
3. Assets/Version폴더를 만들고 폴더 내에 다음을 추가합니다.
    ``` cs
        public class Zepeto(공백이 제거된 Title명)Version
        {
            public static readonly string VERSION = "1.0.0";
        }
    ```
4. release폴더의 urlPath.json을 엽니다. 아래에 다음을 추가합니다.
     ``` json
        {
          "Title": "당신의 모듈 이름",
          "Description": "영어버전의 간략한 설명",
          "Description_ko": "한국어 버전의 간략한 설명",
          "DocsUrl": "사용한 API의 제페토 공식 Docs url, 없는경우 비워두세요",
          "LatestVersion": "1.0.0",
          "Dependencies": ["nothing"]
        },
    ```
5. release 폴더 내에 공백이 제거 된 Title 폴더를 추가합니다.
6. Preview.png 이미지를 추가합니다.
7. 제작한 모듈을 export해 v버전숫자.unitypackage로 저장합니다. (예: v1.0.1.unitypackage)
