# LeaderBoard Module Import Guide

[English](./README.md) | [Korean](./README_KR.md)

### Import Guide
1. 최신버전을 내 프로젝트로 import합니다.
2. Zepeto LeaderBoard Module/Prefab/ZepetoLeaderBoard_Horizontal.prefab을 내 씬으로 가져옵니다.
3. Product Settings > Zepeto > Zepeto Leaderboard에서 리더보드를 제작하고 리더보드 ID를 복사합니다. [[참고]](https://docs.zepeto.me/studio-world/lang-ko/docs/leaderboard)
    <img width="500" alt="image" src="https://user-images.githubusercontent.com/123578202/224616316-43df4b9e-2fed-48e3-9680-d834382263ca.png">
4. ZepetoLeaderBoard_Horizontal프리팹의 LeaderBoardPanel 오브젝트를 열어 Leaderboard Id에 내 리더보드 Id를 붙여넣고, ResetRule을 설정합니다.   
    <img width="500" alt="image" src="https://user-images.githubusercontent.com/123578202/224616120-df75fa70-e6d2-45f3-8986-a801927c8600.png">


### Use Tip
- 아래 스크립트를 처럼 점수를 저장할 수 있습니다. 게임이 끝나는 시점, 코인을 획득한 시점 등에서 다른 스크립트에서 사용하세요.
```typescript
if(finish)
    LeaderBoardManager.instance.SendScore(this.TimeRecord);
```
