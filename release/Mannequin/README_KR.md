# Mannequin Module Import Guide

[English](./README.md) | [Korean](./README_KR.md)

>  <img width="30" alt="image" src="https://user-images.githubusercontent.com/123578202/224616952-9a0217d6-0da8-4146-8164-ed796edcf1f9.png"> 마네킹 API는 내가 직접 제작한 아이템이 있는 경우에만 사용할 수 있습니다. [[아이템 만들기]](https://studio.zepeto.me/ko/console/items/create)

### Dependency
1. 마네킹을 사용하기 전, PackagaManager를 통해 최신버전의 **ZEPETO.Mannequin을 Import** 해주세요.
2. 입어보기 동기화를 위해 먼저, **Multiplay Component를 Import**해주세요.

### Import Guide
1. 최신버전을 내 프로젝트로 Import합니다.
2. World.multiplay > Index.ts파일에 다음과 같이 작성합니다. (다른 모듈을 더 추가했다면 아래 이미지보다 더 많은 모듈이 있을 수 있습니다.)
  ![Mannequin_img1](https://user-images.githubusercontent.com/123578202/224619045-73ada633-aa6b-4bd8-b4a3-f0d7488cdea8.png)
3. Zepeto Mannequin Module/Prefab/MannequinManager.prefab을 내 씬으로 가져옵니다.
4. Prefab 폴더를 열어 원하는 타입의 마네킹을 씬 안으로 드래그하고, 내가 제작한 아이템의 Id를 붙여 넣어줍니다. (공백이 없어야 합니다.)
    <img width="386" alt="image" src="https://user-images.githubusercontent.com/123578202/224619245-23532e76-77f3-4708-8e92-2a67493e04a1.png">


### Use Tip
- 마네킹 오브젝트의 Clollider 사이즈를 조정해 원하는 범위에서 마네킹을 인터렉션 할 수 있습니다.
