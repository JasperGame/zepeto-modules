# Product Module Import Guide

[English](./README.md) | [Korean](./README_KR.md)

> Product API는 내 월드의 재화가 있는 경우에만 사용할 수 있습니다. 이 모듈에서는, 쉬운 이해를 돕기 위해 정규화 된 Currency와 Item Id를 사용했습니다. [[세팅하기]](https://docs.zepeto.me/studio-world/lang-ko/docs/zepeto_product#how-to-apply-a-sample-of-zepeto-product)

### Dependency
1. Product 모듈을 사용하기 전, PackagaManager를 통해 최신버전의 **ZEPETO.Product를 Import** 해주세요.
2. 서버코드의 모듈화를 위해 **Multiplay Component를 Import**해주세요.

### Import Guide
1. 최신버전을 내 프로젝트로 Import합니다.
2. World.multiplay > Index.ts파일에 다음과 같이 작성합니다.
3. Zepeto Product Module/Prefab/ZepetoProductCanvas.prefab을 내 씬으로 가져옵니다.
4. 월드 ID세팅과, 로그인, 스튜디오 내 정규화된 Currency와 Item을 모두 세팅 했는지 확인해주세요.
5. UI_Common_Btn의 버튼들을 눌러 동작을 확인합니다. (이 게임오브젝트는 테스트용임으로 실제 출시엔 버튼을 비활성 상태로 변경하세요.)

### Use Tip
- Level은 단순히 Star Currency의 증감 이해를 돕기위한 설정으로 저장되지 않는 데이터입니다. 실제 사용을 위해선 데이터베이스를 구축해보세요