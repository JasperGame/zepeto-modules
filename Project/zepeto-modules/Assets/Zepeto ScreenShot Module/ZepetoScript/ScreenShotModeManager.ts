import {Animator, Camera, GameObject, Object, HumanBodyBones, Quaternion, Renderer, Transform, Vector3} from 'UnityEngine';
import { ZepetoPlayer, ZepetoPlayers, ZepetoCharacter } from 'ZEPETO.Character.Controller';
import {ZepetoScriptBehaviour} from 'ZEPETO.Script'
import IKController from './IKController';
import ScreenShotController from './ScreenShotController';
import SelfieCamera from './SelfieCamera';

export default class ScreenShotModeManager extends ZepetoScriptBehaviour {
    
    private iKController: IKController;
    
    public screenShotController: GameObject;
    private screenShot: ScreenShotController;
    
    public selfieCameraPrefab: GameObject;
    private selfieCamera: Camera;
    private zepetoCamera: Camera;

    public additionalRunSpeedInSelfieMode: number = 0;
    public additionalWalkSpeedInSelfieMode: number = 0;
    
    @HideInInspector() public localPlayer: ZepetoPlayer;
    @HideInInspector() public myCharacter: ZepetoCharacter;

    // Data
    private playerLayer: number = 21;

    Start() {
        this.screenShot = this.screenShotController.GetComponent<ScreenShotController>();
        
        // Caching objects related to the Zepeto Local player
        ZepetoPlayers.instance.OnAddedLocalPlayer.AddListener(() => {
            this.localPlayer = ZepetoPlayers.instance.LocalPlayer.zepetoPlayer;
            this.zepetoCamera = ZepetoPlayers.instance.LocalPlayer.zepetoCamera.camera;
            this.myCharacter = this.localPlayer.character;

            if(this.localPlayer.character.gameObject.layer != this.playerLayer) {
                this.localPlayer.character.GetComponentsInChildren<Transform>().forEach((characterObj) => {
                    characterObj.gameObject.layer = this.playerLayer;
                });
            }            
        });
    }

    // Proceed with the specified settings when entering screenshot mode. 
    public StartScreenShotMode() {
        // 1. IK Settings
        this.selfieCamera = GameObject.Instantiate<GameObject>(this.selfieCameraPrefab).GetComponent<Camera>();

        let character = ZepetoPlayers.instance.LocalPlayer.zepetoPlayer.character;
        let target = this.selfieCamera;
  
        // 2. SelfieCamera setting
        let selfieCamera: SelfieCamera = target.GetComponent<SelfieCamera>();
        selfieCamera.InitSetting(character.gameObject.transform);
        
        let playerAnimator = this.localPlayer.character.gameObject.GetComponentInChildren<Animator>();
        this.iKController = playerAnimator.gameObject.AddComponent<IKController>();
        this.iKController.SetTarget(target.transform);

        // 3. Initialize the zepetoCamera
        this.SetZepetoCameraMode();
    }

   
    // Initialize the camera settings when exiting the screenshot mode.
    public ExitScreenShotMode(isThirdPersonView: boolean) {
        if(this.selfieCamera != null) {
            GameObject.Destroy(this.selfieCamera.gameObject);
        }

        if(!isThirdPersonView) {
            this.SetIKActive(false);
            this.zepetoCamera.gameObject.SetActive(true);
        }
    }

    public GetPlayerLayer(): number {
        return this.playerLayer;
    }
    // Return Selfie Camera
    public GetSelfieCamera(): Camera {
        return this.selfieCamera;
    }
    // Return ZEPETO Camera
    public GetZepetoCamera(): Camera {
        return this.zepetoCamera;
    }

    // Decide whether to enable the selfie camera
    public SetSelfieCameraActive(active: boolean) {
        this.selfieCamera.gameObject.SetActive(active);
    }

    // Decide whether to apply IKPass
     public SetIKActive(active: boolean) {
         this.iKController.SetIKWeightActive(active);
    }

    // Decide whether to use MoveTurn Anim
    public SetMoveturnActive(active: boolean) {
        this.myCharacter.MotionV2.useMoveTurn = active;
    }

    // Increases run speed
    public AddRunSpeed(addSpeed: number) {
        this.myCharacter.additionalRunSpeed = addSpeed;
    }

    // Increases walk speed
    public AddWalkSpeed(addSpeed: number) {
        this.myCharacter.additionalWalkSpeed = addSpeed;
    }

    // Functions for camera setup
    SetSelfieCameraMode() {
        //Disable the existing ZEPETO Camera
        this.zepetoCamera.gameObject.SetActive(false);
        // Enable Selfie Camera
        this.SetSelfieCameraActive(true);
        // Enabling IKPass for Selfie Pose Settings
        this.SetIKActive(true);
        // Disable Moveturn Animation
        this.SetMoveturnActive(false);
        //Add speed
        this.AddRunSpeed(this.additionalRunSpeedInSelfieMode);
        this.AddWalkSpeed(this.additionalWalkSpeedInSelfieMode);
        //Change the camera for screenshots to the selfie camera
        this.screenShot.SetScreenShotCamera(this.selfieCamera);
    }

    SetZepetoCameraMode() {
        //Activate the current ZEPETO camera
        this.zepetoCamera.gameObject.SetActive(true);
        // Disable Selfie Camera
        this.SetSelfieCameraActive(false);
        //Disable IKPass to stop posing for selfies
        this.SetIKActive(false);
        // Enable Moveturn Animation
        this.SetMoveturnActive(true);
        //Add speed
        this.AddRunSpeed(this.additionalRunSpeedInSelfieMode *(-1));
        this.AddWalkSpeed(this.additionalWalkSpeedInSelfieMode * (-1));
        //Change the active camera to the ZEPETO camera
        this.screenShot.SetScreenShotCamera(this.zepetoCamera);

    }
}