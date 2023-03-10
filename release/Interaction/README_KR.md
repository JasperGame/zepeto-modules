# Interaction Import Guide

[English](./README.md) | [Korean](./README_KR.md)

1. Zepeto Import Manager를 통해 최신버전의 Interaction 샘플을 Import 합니다.
2. Prefab 폴더를 열어 InteractionBench를 씬 안으로 드래그 합니다. 추가 된 의자는 주변으로 가면 앉을 수 있는 아이콘이 생기고, 아이콘을 클릭했을 때 제페토 캐릭터가 앉을 수 있게 됩니다.
3. 원하는 의자를 사용해 아이콘을 띄우고 싶으면 Prefab 폴더를 열어 SeatPos를 적절한 공간에 배치합니다.
4. 아이콘 모양을 변경할 수 있습니다.
    4-1 , Resources/WorldEventIcons/Prefabs/PrefIconCanvas를 복제합니다.
    4-2 , 복제한 캔버스의 IconButton에 원하는 이미지로 교체합니다.
    4-3 , InteractionIcon에 PrefIconCanvas를 수정한 아이콘이 있는 프리팹으로 교체합니다.
5. 아이콘을 클릭했을 때 다른 행동을 하게 만들 수 있습니다.
    5-1 , 원하는 위치에 empty GameObject를 배치하고 isTrigger가 추가된 Collider를 추가합니다. 
    5-2 , InteractionIcon.ts 스크립트를 추가합니다. 
    5-3 , 행동 스크립트를 작성하고 해당 스크립트 내에 Interaction Icon의 OnClickEvent.AddListener(()=>) 에 추가합니다.