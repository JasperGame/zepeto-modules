import { ZepetoScriptBehaviour } from 'ZEPETO.Script';
import { LocalPlayer, CharacterState, ZepetoCharacter, ZepetoPlayers} from 'ZEPETO.Character.Controller';
import { OfficialContentType, ZepetoWorldContent, Content } from 'ZEPETO.World';
import { Button } from 'UnityEngine.UI';
import { Object, GameObject, Transform, AnimationClip, WaitForSeconds, Coroutine } from 'UnityEngine';
import Thumbnail from './Thumbnail';

export default class GestureLoader extends ZepetoScriptBehaviour {

    @HideInInspector() public contents: Content[] = [];
    @HideInInspector() public thumbnails: GameObject[] = [];
    @HideInInspector() public gestureLoop: Coroutine;
    @HideInInspector() public animation: AnimationClip = null;

    @SerializeField() private _loadContentsCount: number = 100;
    @SerializeField() private _contentsParent: Transform;
    @SerializeField() private _prefThumb: GameObject;

    private _myCharacter: ZepetoCharacter ;
    
    // Loop setting
    @Header("Looping Setting") 
    @Tooltip("Activate/Deactivate the looping feature") public isLooping: boolean;
    @Tooltip("Waiting time until you run the gesture again") @SerializeField() private _waitTimeBeforeRepeat: number; // Waiting time before repeating a loop

    Start() {
        ZepetoPlayers.instance.OnAddedLocalPlayer.AddListener(() => {
            // In order to take a thumbnail with my character, You need to request the content after the character is created.
            this._myCharacter = ZepetoPlayers.instance.LocalPlayer.zepetoPlayer.character;
            this.ContentRequest();            
        });
    }

    // 1. Receive content from the server
    private ContentRequest() {
        // All Type Request
        ZepetoWorldContent.RequestOfficialContentList(OfficialContentType.All, contents => {
            this.contents = contents;
            for (let i = 0; i < this._loadContentsCount; i++) {
                if (!this.contents[i].IsDownloadedThumbnail) {
                    // Take a thumbnail photo using my character
                    this.contents[i].DownloadThumbnail(this._myCharacter,() =>{
                        this.CreateThumbnailObjcet(this.contents[i]);
                    });
                } else {
                    this.CreateThumbnailObjcet(this.contents[i]);
                }
            }
        });
    }

    // 2. Creating Thumbnail Objects
    private CreateThumbnailObjcet(content: Content) {
        const newThumb: GameObject = GameObject.Instantiate(this._prefThumb, this._contentsParent) as GameObject;
        newThumb.GetComponent<Thumbnail>().content = content;

        // Button Listener for each thumbnail
        newThumb.GetComponent<Button>().onClick.AddListener(() => {  
            this.LoadAnimation(content);
        });
        this.thumbnails.push(newThumb);
    }

    // 3. Loading Animation
    private LoadAnimation(content: Content ) {
        // Verify animation load
        if (!content.IsDownloadedAnimation) {
            // If the animation has not been downloaded, download it.
            content.DownloadAnimation(() => {
                // play animation clip
                this.runAnimation(content.AnimationClip, content.Keywords)
            });    
        } else {
            this.runAnimation(content.AnimationClip, content.Keywords)
        }
    }
       
    // A function to run an animation, 
    private runAnimation(animation: AnimationClip, type: OfficialContentType[] )
    {        
        //if there is another gesture running, stop the coroutine and cancel the gesture
        if(this.gestureLoop)
        {
            this.StopCoroutine(this.gestureLoop)
        }
        this._myCharacter.CancelGesture()

        // check if isLooping is true and it is not a pose
        if(this.isLooping === true && this._checkIfGestureIsPose(type) === false)
        {            
            this.gestureLoop = this.StartCoroutine(this.setGestureLoop(animation))
        }
        //When the isLooping is false
        else{
            //If there is another animation running, cancel it
            this._myCharacter.SetGesture(animation)
        }   
    }

    private _checkIfGestureIsPose(type: OfficialContentType[])
    {
        return type.some( item => item === OfficialContentType.Pose || item === OfficialContentType.GesturePose )
    }

    // A coroutine for running the Gesture in loop
    public *setGestureLoop(animation: AnimationClip)
    {        
        while(true){
            
            if(this._myCharacter.CurrentState === CharacterState.Idle && animation)
            {
                this._myCharacter.SetGesture(animation)
                yield new WaitForSeconds(animation.length + this._waitTimeBeforeRepeat)
            }
            else{
                yield null;
            }
        }
    }
}