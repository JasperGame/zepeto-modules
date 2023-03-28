# LeaderBoard Module Import Guide

[English](./README.md) | [Korean](./README_KR.md)

## Import Guide
1. Import the latest version of the LeaderBoard module into your project.   

     ​
2. From the Assets/Zepeto LeaderBoard Module/Prefab folder, add a prefab by dragging it to the scene.
     <img width="700" alt="image" src="./images/GuideImage1.png">   
- ZepetoLeaderBoard_Horizontal.prefab : Horizontal    
- ZepetoLeaderBoard_Vertical.prefab : Vertical    
     > **Note**: To check whether the screen mode of the current world is horizontal or vertical, check Orientation in Open World Setting. Vertical is portrait mode, Horizontal is landscape mode.   
         <img width="700" alt="image" src="./Image/GuideImage2.png">   

3. If the ZEPETO character is not used in the scene, add an EventSystem.
     > **Note**: ZepetoPlayers creates an Event System at runtime to ensure that the leaderboard buttons are activated properly. Without the Event System, the leaderboard button does not activate properly.   
         <img width="700" alt="image" src="./Image/GuideImage3.png">   

4. Refer to the following guide to complete the leaderboard setup and copy the leaderboard ID. [[Leaderboard Guide]](https://docs.zepeto.me/studio-world/docs/leaderboard)   
     <img width="700" alt="image" src="./images/GuideImage4.png">    
​
5. Paste the copied leaderboard ID into the LeaderboardId input field of the LeaderboardPanel LeaderboardManager script of the Prefab added in the Scene. Also, for the Reset Rule, select the same Leaderboard Reset Rule.   
     <img width="700" alt="image" src="./images/GuideImage5.png">   
​

## Use Tips
- To record a player's score, please refer to the script below. Use the following scripts at the end of the game, at the point of earning coins, etc.
     ```typescript
     if(finish)
         LeaderBoardManager.instance.SendScore(this.TimeRecord);
     ```
