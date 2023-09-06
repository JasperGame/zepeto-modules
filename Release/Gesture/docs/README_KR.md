# Gesture Module Import Guide

[English](./README.md) | [Korean](./README_KR.md)

## Import Guide
1. Gesture 모듈 최신버전을 내 프로젝트로 import합니다.
2. ZepetoGestureModule/Prefab/Gesture_Canvas_Horizontal.prefab (또는 월드의 화면 방향에 따라 Gesture_Canvas_Vertical.prefab)을 드래그해서 Scene으로 가져옵니다.
    <img width="700" alt="image" src="./images/GuideImage1.png">
3.  Scene에 ZepetoPlayers 컴포넌트를 추가하고, 캐릭터 생성 스크립트를 구현한 후 [▶︎(play)]버튼을 눌러 제스쳐 기능을 테스트할 수 있습니다.  

## Use Tip
#### 버튼 아이콘 위치를 변경하는 방법
- Gesture_Canvas_Horizontal.prefab (또는 Gesture_Canvas_Vertical.prefab) 내부의 BTN_Gesture 위치를 조정합니다.   
         <img width="700" alt="image" src="./images/GuideImage2.png">

#### 제스쳐 로드 개수를 변경하는 방법
- ZepetoGesture_Horizontal.prefab 내부의 GestureLoader스크립트의 Load Contents Count 수를 조절해 노출될 제스쳐의 개수를 조절할 수 있습니다.   
        <img width="700" alt="image" src="./images/GuideImage3.png">   
#### 제스처를 반복 재생 하기
- 제스처의 반복 설정을 활성화/비활성화할 수 있습니다.
- 제스처가 다시 반복 재생되기까지의 대기 시간을 설정할 수 있습니다.
- 포즈 아닌 제스처에만 반복 설정 가능합니다.

- <img width="700" alt="image" src="./images/GuideImage5.png">

#### 포즈 지속 시간 설정
- 포즈 제스처를 사용할 때 캐릭터가 포즈를 취하는 시간(초)을 설정할 수 있습니다. 기본값은 0초입니다. 포즈 지속 시간이 설정되지 않으면 기본값으로 설정됩니다.
  
- 이 기능은 포즈 제스처에만 작동합니다.
  
  <img width="700" alt="image" src="./images/GuideImage6.png">

#### 화면 회전 기능이 있는 경우
- 만약 화면 회전 기능이 있는 경우, 제스처 모듈이 제공하는 Custom Touchpad Controller를 사용하세요. (**ZepetoGesture > Prefab > Custom Touchpad Controller**). 이 Custom Prefab은 화면 회전 후에 터치패드를 사용하여 제스처를 취소할 수 있게 해줍니다.
- 해야 할 일은 이러한 프리팹을 ZepetoPlayers 게임 오브젝트 내에 있는 Zepeto Player 스크립트를 대체하기 위해 끌어다 놓는 것뿐입니다.
-  <img width="700" alt="image" src="./images/GuideImage7.png">
   
#### 멀티플레이 동기화 하는 방법
- 멀티플레이 동기화를 위해서는 [[Multiplay Component]](../../MultiplayComponent/)를 다운로드 한 후, 씬에 배치한 ZepetoPlayersManager의 **Use Zepeto Gesture API**를 체크합니다.   
        <img width="700" alt="image" src="./images/GuideImage4.png"> 
