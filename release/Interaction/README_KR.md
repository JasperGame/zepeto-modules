# Interaction Module Import Guide

[English](./README.md) | [Korean](./README_KR.md)

### Import Guide
1. 최신버전을 내 프로젝트로 import합니다.
2. Zepeto Interaction Module/Prefab/InteractionBench.prefab을 내 씬으로 가져옵니다.

### Use Tip
a. 다른 의자를 사용하고 싶으면 Prefab/SeatPos.prefab를 앉는 위치에 배치합니다.
b. 아이콘 모양을 변경할 수 있습니다.   
    b-1 , Resources/WorldEventIcons/Prefabs/PrefIconCanvas를 복제합니다.
    b-2 , 복제한 캔버스의 IconButton에 원하는 이미지로 교체합니다.
    b-3 , InteractionIcon에 PrefIconCanvas를 수정한 아이콘이 있는 프리팹으로 교체합니다.
c. 아이콘을 클릭했을 때 다른 행동을 하게 만들 수 있습니다.
    c-1 , 원하는 위치에 empty GameObject를 배치하고 isTrigger가 추가된 Collider를 추가합니다. 
    c-2 , InteractionIcon.ts 스크립트를 추가합니다. 
    c-3 , 행동 스크립트를 작성하고 해당 스크립트 내에 Interaction Icon의 OnClickEvent.AddListener(()=>) 에 추가합니다.
```typescript
    this._interactionIcon.OnClickEvent.AddListener(()=> {
        // when onclick interaction icon
        this._interactionIcon.HideIcon();
        
        // your interaction function
        this.DoInteraction();
    });
```