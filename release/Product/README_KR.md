# Product Module Import Guide

[English](./README.md) | [Korean](./README_KR.md)

> Product API는 내 월드의 재화가 있는 경우에만 사용할 수 있습니다. 이 모듈에서는, 쉬운 이해를 돕기 위해 정규화 된 Currency와 Item Id를 사용했습니다. [[세팅하기]](https://docs.zepeto.me/studio-world/lang-ko/docs/zepeto_product#how-to-apply-a-sample-of-zepeto-product)


### Dependency
1. Product 모듈을 사용하기 전, PackagaManager를 통해 최신버전의 **ZEPETO.Product를 Import** 해주세요.
2. 서버코드의 모듈화를 위해 **Multiplay Component를 Import**해주세요.

### Import Guide
1. 최신버전을 내 프로젝트로 Import합니다.   
 ![product_img1](https://user-images.githubusercontent.com/123578202/224613221-35e22f74-841a-46fc-9388-943cf5ebd153.png)
2. World.multiplay > Index.ts파일에 다음과 같이 추가합니다.(다른 모듈을 더 추가했다면 아래 이미지보다 더 많은 모듈이 있을 수 있습니다.)
 ![product_img2](https://user-images.githubusercontent.com/123578202/224613036-803cf372-92c6-4bd8-90ae-39537180cf4e.png)
3. Zepeto Product Module/Prefab/ZepetoProductCanvas.prefab을 내 씬으로 가져옵니다.
4. 월드 ID세팅과, 로그인, 스튜디오 내 정규화된 Currency와 Item을 모두 세팅 했는지 확인해주세요.   
  4-1. 정상적으로 세팅이 되었다면 에디터 상단 ZEPETO > Setting > Product에 다음과 같이 표시됩니다. 각 썸네일에 아이템 아이콘을 등록하면 됩니다.
    <img width="500" alt="product_img4" src="https://user-images.githubusercontent.com/123578202/224614665-80d54306-83bd-40c4-b0db-21932f126a61.png">   
  
5. UI_Common_Btn의 버튼들을 눌러 동작을 확인합니다. (이 버튼들은 테스트용 입니다. 실제 출시엔 버튼을 비활성 상태로 변경하세요.)   
6. 서버가 항상 켜져있어야 합니다. 간혹 오류가 발생한다면 서버를 재실행 해주세요    
 ![product_img3](https://user-images.githubusercontent.com/123578202/224615407-f0c2efc2-c6d8-4403-878c-a0929eed9fa2.png)


### Use Tip
- Level은 단순히 Star Currency의 증감 이해를 돕기위한 설정으로 저장되지 않는 데이터입니다. 실제 사용을 위해선 데이터베이스를 구축해보세요.
