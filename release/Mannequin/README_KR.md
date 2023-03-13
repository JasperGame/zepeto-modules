# Mannequin Module Import Guide

[English](./README.md) | [Korean](./README_KR.md)

> 마네킹 API는 내가 직접 제작한 아이템이 있는 경우에만 사용할 수 있습니다. [[아이템 만들기]](https://studio.zepeto.me/ko/console/items/create)

### Dependency
1. 마네킹을 사용하기 전, PackagaManager를 통해 최신버전의 **ZEPETO.Mannequin을 Import** 해주세요.
2. 입어보기 동기화를 위해 **Multiplay Component를 Import**해주세요.

### Import Guide
1. 최신버전을 내 프로젝트로 Import합니다.
2. World.multiplay > Index.ts파일에 다음과 같이 작성합니다.
3. Zepeto Mannequin Module/Prefab/MannequinManager.prefab을 내 씬으로 가져옵니다.
4. Prefab 폴더를 열어 원하는 타입의 마네킹을 씬 안으로 드래그하고, 내가 제작한 아이템의 Id를 붙여 넣어줍니다.

### Use Tip
- Clollider의 사이즈를 조정해 원하는 범위에서 마네킹을 인터렉션 할 수 있습니다.