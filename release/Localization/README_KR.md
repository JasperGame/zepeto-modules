# Localization Module Import Guide

[English](./README.md) | [Korean](./README_KR.md)

### Import Guide
1. 최신버전을 내 프로젝트로 import합니다.
2. Zepeto Gesture Module/Prefab/ZepetoGesture_Horizontal.prefab을 내 씬으로 가져옵니다.

### Use Tip
- 프리팹 내부의 BTN_Gesture의 위치를 조절할 수 있습니다.
- 프리팹 내부의 GestureLoader스크립트의 Count 수를 조절해 보이는 제스쳐의 개수를 조절할 수 있습니다.
    <img width="572" alt="image" src="https://user-images.githubusercontent.com/123578202/224653731-480eeb67-b1df-4511-b90a-e2f409f80875.png">
- 멀티플레이 동기화를 위해서는 [[동기화 컴포넌트]](../MultiplayComponent/)를 다운로드 한 후, 씬에 배치한 ZepetoPlayersManager의 Use Zepeto Gesture API를 체크하면 제스쳐가 동기화 됩니다.    
    <img width="576" alt="image" src="https://user-images.githubusercontent.com/123578202/224652925-d1efb4df-ee6a-4342-8e9e-ce2420718204.png">

