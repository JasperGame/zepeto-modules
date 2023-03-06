import { ZepetoScriptBehaviour } from 'ZEPETO.Script'
import { Image, Text } from 'UnityEngine.UI'
import { Texture, Texture2D, Sprite, Rect, Vector2, GameObject } from 'UnityEngine'
import { ZepetoWorldHelper, Users } from 'ZEPETO.World'
import { GetRangeRankResponse, LeaderboardAPI, ResetRule, Rank } from "ZEPETO.Script.Leaderboard";


export default class ITM_LeaderBoard extends ZepetoScriptBehaviour {
    @SerializeField() private playerImage: Image;
    @SerializeField() private playerRankText: Text;
    @SerializeField() private playerNameText: Text;
    @SerializeField() private playerScoreText: Text;

    private _rankCache : Rank;
    
    public SetGroup(rank : Rank){
        if(this._rankCache?.member != rank.member){
            if(rank.name != null) {
                // Set player's ProfileTexture
                ZepetoWorldHelper.GetProfileTexture(rank.member, (texture: Texture) => {
                    if(texture!=null)
                        this.playerImage.sprite = this.GetSprite(texture);
                }, (error) => {
                    console.warn("GetProfileTexture Error : " + error);
                });
            }
            else{
                console.warn(rank.rank+" member null : " + rank.name);
            }
            this.playerNameText.text = rank.name ?? "withdrawn user.";
        }
        // Set player's rank, and score
        this.playerRankText.text = rank.rank.toString();
        this.playerScoreText.text = rank.score.toString();
        
        this._rankCache = rank;
    }

    GetSprite(texture:Texture){
        let rect:Rect = new Rect(0, 0, texture.width, texture.height);
        return Sprite.Create(texture as Texture2D, rect, new Vector2(0.5, 0.5));
    }

}