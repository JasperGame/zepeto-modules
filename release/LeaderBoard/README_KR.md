# LeaderBoard Module Import Guide

[English](./README.md) | [Korean](./README_KR.md)

### Import Guide
1. 최신버전을 내 프로젝트로 import합니다.
2. Zepeto LeaderBoard Module/Prefab/ZepetoLeaderBoard_Horizontal.prefab을 내 씬으로 가져옵니다.
3. Product Settings > Zepeto > Zepeto Leaderboard에서 리더보드를 제작하고 리더보드 ID를 복사합니다. [[참고]](https://docs.zepeto.me/studio-world/lang-ko/docs/leaderboard)
4. ZepetoLeaderBoard_Horizontal프리팹의 LeaderBoardPanel 오브젝트를 열어 Leaderboard Id에 내 리더보드 Id를 붙여넣습니다.

### Use Tip
- 아래 스크립트를 처럼 점수를 저장할 수 있습니다. 게임이 끝나는 시점, 코인을 획득한 시점 등에서 다른 스크립트에서 사용하세요.
```typescript
if(finish)
    LeaderBoardManager.instance.SendScore(this.TimeRecord);
```