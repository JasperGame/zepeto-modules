import { ZepetoScriptBehaviour } from 'ZEPETO.Script'
import { GetAllLeaderboardsResponse, GetRangeRankResponse, LeaderboardAPI, ResetRule } from "ZEPETO.Script.Leaderboard";
import { GameObject, Transform, WaitUntil, Coroutine } from "UnityEngine";
import { Button, Text } from "UnityEngine.UI";
import ITM_LeaderBoard from './ITM_LeaderBoard'

export default class LeaderboardManager extends ZepetoScriptBehaviour {

    public leaderboardId: string = "";
    public resetRule: ResetRule;

    @SerializeField() private contentsParent: Transform;
    @SerializeField() private myScoreGroup: GameObject;
    @SerializeField() private scoreGroupPrefab: GameObject;
    @SerializeField() private openBtn: Button;
    @SerializeField() private closeBtn: Button;
    @SerializeField() private seasonText: Text;

    private _startRank: number = 1;
    private _endRank: number = 10000; // Ranking information can be processed up to 10,000 cases at a time
    private _myBestScore: number;
    private _loadCoroutine: Coroutine;

    public static instance: LeaderboardManager;
    /* Singleton */
    private Awake() {
        if (LeaderboardManager.instance == null) {
            LeaderboardManager.instance = this;
        } else {
            return;
        }
    }
    private Start() {
        if (this.leaderboardId === "") {
            console.warn("error!  Leaderboard Id is empty");
            const message = "See the docs <color=blue><a>https://naverz-group.readme.io/studio-world/docs/leaderboard</a></color> for more information.";
            console.warn(message);
            return;
        }
        this.SetSeasonText();
        
        // Send based on 0 point when connecting
        this.SendScore(0);

        this.openBtn.onClick.AddListener(() => {
            console.log("CLick");
            this._loadCoroutine = this.StartCoroutine(this.LoadLeaderboard());
        });
        this.closeBtn.onClick.AddListener(() => {
            if (this._loadCoroutine != null) {
                this.StopCoroutine(this._loadCoroutine);
                this._loadCoroutine = null;
            }
        });
    }

    // Use Tip ! (If you reach a new record of 10)
    // LeaderBoardManager.instance.SendScore(10);
    public SendScore(score: number) {
        this._myBestScore = (score < this._myBestScore || this._myBestScore == 0) ? score : this._myBestScore;
        LeaderboardAPI.SetScore(this.leaderboardId, score,
            (result) => { console.log(`result.isSuccess: ${result.isSuccess}`); },
            (error) => { console.error(error); });

        return this._myBestScore;
    }

    private *LoadLeaderboard() {
        let isResponsed = false;
        let totalRankCount = 1;
        for (let i = 0; i < totalRankCount / 10; i++) {
            LeaderboardAPI.GetRangeRank(this.leaderboardId, i * 10 + 1, (i + 1) * 10, this.resetRule, false,
                (result) => {
                    this.OnResult(result);
                    totalRankCount = Number(result.rankInfo.totalRankCount);
                    isResponsed = true;
                },
                (error) => {
                    console.warn("GetRangeRank Error :" + error);
                    isResponsed = true;
                }
            );
            yield new WaitUntil(() => isResponsed);
            isResponsed = false;
        }
    }

    private OnResult(result: GetRangeRankResponse) {
        if (result.rankInfo.myRank) {
            // Set Group - My Score
            const myRank = result.rankInfo.myRank;
            const item: ITM_LeaderBoard = this.myScoreGroup.GetComponent<ITM_LeaderBoard>();
            this._myBestScore = myRank.score;
            item.SetGroup(myRank);
        }

        if (result.rankInfo.rankList) {
            const endCount = (result.rankInfo.rankList.length < this._endRank) ? result.rankInfo.rankList.length : this._endRank;
            for (let i = 0; i < endCount; ++i) {
                const rank = result.rankInfo.rankList[i];
                let item: ITM_LeaderBoard;
                if (this.contentsParent.childCount < result.rankInfo.rankList[i].rank) {
                    const newGroup: GameObject = GameObject.Instantiate(this.scoreGroupPrefab, this.contentsParent) as GameObject;
                    item = newGroup.GetComponent<ITM_LeaderBoard>();
                }
                else {
                    item = this.contentsParent.GetChild(result.rankInfo.rankList[i].rank - 1).GetComponent<ITM_LeaderBoard>();
                }
                item.SetGroup(rank);
            }
        }
    }

    private SetSeasonText() {
        LeaderboardAPI.GetAllLeaderboards(this.OnLoadAllLeaderboardsResult, this.OnError);
    }

    private OnLoadAllLeaderboardsResult(result: GetAllLeaderboardsResponse) {
        console.log(`result.isSuccess: ${result.isSuccess}`);
        const leaderboardManager = LeaderboardManager.instance;

        if (result.leaderboards) {
            for (let i = 0; i < result.leaderboards.length; ++i) {
                const leaderboard = result.leaderboards[i];
                console.log(`i: ${i}, id: ${leaderboard.id}, name: ${leaderboard.name} , resetRule: ${leaderboard.resetInfoList[0].resetRule}`);

                switch (leaderboard.resetInfoList[0].resetRule) {
                    case 1: //Day
                        leaderboardManager.seasonText.text = "End of Season : Every" + leaderboard.resetInfoList[0].hour, leaderboard.resetInfoList[0].min;
                        break;
                    case 2: // Week
                        leaderboardManager.seasonText.text = "End of Season : Every" + leaderboard.resetInfoList[0].weekDay, + leaderboard.resetInfoList[0].hour, leaderboard.resetInfoList[0].min;
                        break;
                    case 3: // Month
                        leaderboardManager.seasonText.text = "End of Season : Every" + leaderboard.resetInfoList[0].day, + leaderboard.resetInfoList[0].hour, leaderboard.resetInfoList[0].min;
                        break;
                    default:
                        leaderboardManager.seasonText.text = "No reset Rule";
                        break;
                }

            }
        }
    }

    private OnError(error: string) {
        console.error(error);
    }



}

