using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System.Linq;
using System.IO;

namespace VRAdultFun
{
    partial class EmotionEngine : MVRScript
    {
        #region Initilation Variables
        //private static Atom debugUI;
        //private static UITextControl debugUIControl;
		private static bool allSetup = false;
		private static float tempFloat = 0.0f;
		private static float tempFloat2 = 0.0f;
		private static string currentLook = "";
		private static string currentBrow = "";
		private static string currentEye = "";
		private static string currentMouth = "";
        private static float debugClock = 0.0f;
		private static string debugString = "";
        private static string logLine;
        private static Atom aCube;
		private static FreeControllerV3 aCubeController;
        private static float saccadeClock = 0.0f;
		private static float saccadeRepeat = 0.0f;
        private static float eyeClock = 0.0f;
		private static float eyeCloseMaxMorph = 1.1f;
        private static float randomX = 0.0f;
        private static float randomY = 0.0f;
        private static string currentInterest = "Random";
        private static string prevInterest = "Random";
        private static float currentInterestLevel = 0.0f;
        private static float maxInterestLevel = 100.0f;
        private static float interestClock = 0.0f;
        private static float interestArousal = 0.0f;
        private static float interestValence = 0.0f;
        private static float interestMaxSmile = 0.65f;
        private static bool interestKissing = false;
        private static float pExtraversion = 80.0f; //Random.Range(20.0f,100.0f);//80.0f;
        private static float pAgreeableness = 60.0f; //Random.Range(40.0f,100.0f);//60.0f;
        private static float pStableness = 40.0f; //Random.Range(10.0f,100.0f);//40.0f;
        private static float peronalityAdjustH = 0.0f;
		private static float lastAdjustH = 0.0f;
		private static float lastAdjustV = 0.0f;
        private static float peronalityAdjustV = 0.0f;
        private static bool amGlancing = false;
        private static float glanceClock = 0.0f;
        private static float glanceTimeout = 0.0f;
        private static float velocityH = 0.0f;
        private static float velocityV = 0.0f;
        private static float eyesNonDirectClock = 0.0f;
        private static float eyesNonDirectAngle = 5.0f;
		private static float eyesDirectClock = 0.0f;
        private static float fuzzyLock = 1.0f;
        private static Vector3 refAngle;
		private static Vector3 oldEyePos;

		protected JSONStorableFloat uiExtraversion;
		protected JSONStorableFloat uiAgreeableness;
		protected JSONStorableFloat uiStableness;
		protected JSONStorableFloat uiBreatheSpeed;
		protected JSONStorableFloat uiBreatheRaiseMultiplier;
		protected JSONStorableFloat uiBreatheExpandMultiplier;
		protected JSONStorableFloat uiGazeSpeed;
		protected JSONStorableFloat uiGazeVariation;
		protected JSONStorableBool uiGazeAvoid;
		protected JSONStorableFloat uiGazeLookTime;
		protected JSONStorableFloat uiGazeAvoidTime;
		protected JSONStorableBool uiGazeGlance;
		protected JSONStorableFloat uiRollSpeed;
		protected JSONStorableFloat uiSaccadeSpeed;
		protected JSONStorableFloat uiSaccadeAmount;
		protected JSONStorableFloat uiSaccadeWanderMult;
		protected JSONStorableFloat uiBlinkSpeed;
		protected JSONStorableFloat uiEyeCloseMaxMorph;
		protected JSONStorableFloat uiArousalSpeed;
		protected JSONStorableFloat uiValenceSpeed;
		protected JSONStorableFloat uiMoodSpeed;
		protected JSONStorableFloat uiInterestSpeed;
		protected JSONStorableFloat uiInterestRate;
		protected JSONStorableBool uiDoHead;
		protected JSONStorableBool uiDoShoulders;
		protected JSONStorableFloat uiShoulderAmount;
		protected JSONStorableBool uiDoChest;
		protected JSONStorableFloat uiChestAmount;
		protected JSONStorableBool uiDoHands;
		protected JSONStorableBool uiConfigHead;
		protected JSONStorableBool uiUsePerson2;
		protected JSONStorableBool uiShowStats;
		protected JSONStorableBool uiDoKiss;
		protected JSONStorableFloat uiKissAmount;
		protected JSONStorableBool uiDoBlowjob;
		protected JSONStorableFloat uiBlowjobAmount;
		protected JSONStorableBool uiDoSex;
		protected JSONStorableFloat uiSexAmount;
		protected JSONStorableStringChooser uiFocusTarget;
		protected JSONStorableStringChooser uiObjectTarget;
		protected JSONStorableBool uiTargetLook;
		protected JSONStorableFloat uiPersonalSpace;
		protected JSONStorableFloat uiDirectGaze;
		protected JSONStorableFloat uiPeripheralGaze;
		protected JSONStorableFloat uiOutOfGaze;
		protected JSONStorableFloat uiCloseToFaceDist;
		protected JSONStorableFloat uiKissingDist;
		protected JSONStorableFloat uiInteractDist;
		protected JSONStorableFloat uiMaxMorphSmile;
		protected JSONStorableFloat uiEyeUpdate;
		protected JSONStorableFloat uiHeadInterest;
		protected JSONStorableFloat uiLHandInterest;
		protected JSONStorableFloat uiRHandInterest;
		protected JSONStorableFloat uiPenisInterest;
		protected JSONStorableFloat uiObjectInterest;
		protected JSONStorableBool uiSavePreset;
		protected JSONStorableBool uiLoadPreset;
		protected JSONStorableBool uiLoadDefaults;
		protected JSONStorableBool uiDoMorphs;
		

		private static Atom emTarget;
		private static string emTargetName;
		private static FreeControllerV3 emTargetController;
		private static float emTargetDistance;
		private static float emTargetDir;
		private static float emTargetHeadDir;
		//private static Vector3 emTargetPosPrev;
		private static float interestEMTarget;
		private static Vector3 lookAtPosition;
		
        private static float gAvoidance = Mathf.Clamp((((100.0f - pExtraversion) + (100.0f - pStableness)) / 2.0f), 30.0f, 100.0f);
        private static float gDuration = 10.0f * (Mathf.Clamp((pStableness / 5.0f) + ((100.0f - pAgreeableness) / 2.0f) + (pExtraversion / 5.0f), 0.0f, 100.0f) / 100.0f);
        private static float gFrequency = Mathf.Clamp((pStableness / 2.0f) + (pExtraversion / 2.0f), 0.0f, 100.0f);
        private static float gHeadSpeed = 1.75f;
		private static float adjustedSpeed = 0.0f;
        private static bool gDirectionCenter;
        private static bool gDirectionUp;
        private static bool gDirectionDown;
        private static bool gDirectionSide;
        private static float gHeadRoll = 0.0f;
        private static float gHeadRollTarget = 0.0f;
        private static float gAvoid = 0.0f;
        private static float gAvoidanceClock = 0.0f;
		private static float gAvoidingClock = 0.0f;
		private static string gAvoidInterest = "";
        private static float sexActionNeckX = 0.0f;

        private static string mainInterest = "RandomF";
        private static float mainValue = 0.0f;
        private static string secondInterest = "RandomF";
        private static float secondValue = 0.0f;
        private static string mainOld = "RandomF";
        private static string secondOld = "RandomF";
        private static bool mainSwitch = false;
        private static bool secondSwitch = false;
        private static float mainClock = 0.0f;
        private static float secondClock = 0.0f;

        private static StateMachine emotionSM;
        private static StateMachine lookSM;
        private static State lastLookState;
        private static StateMachine systemSM;
        private static StateMachine browSM;
        private static State lastBrowState;
        private static StateMachine mouthSM;
        private static State lastMouthState;
        private static StateMachine eyesSM;
        private static State lastEyesState;

        private static Atom person;
		private static bool personIsMale = false;
        private static Vector3 curEyePosition;
        private static Vector3 curEyeAngles;
        private static float eyeUpdateTime = 0.05f;
        private static float eyeUpdateClock = 0.0f;
        private static JSONStorable personEyes;
        private static JSONStorable personEyelids;
        private static FreeControllerV3 eyeController;
        private static FreeControllerV3 headActual;
        private static FreeControllerV3 headController;
		private static Vector3 headPrevPos;
        private static FreeControllerV3 neckController;
        private static FreeControllerV3 chestController;
        private static FreeControllerV3 lBreastController;
        private static FreeControllerV3 rBreastController;
        private static FreeControllerV3 pelvisController;
        private static FreeControllerV3 lHandController;
        private static FreeControllerV3 rHandController;
        private static FreeControllerV3 lShoulderController;
        private static FreeControllerV3 rShoulderController;
        private static FreeControllerV3 lArmController;
        private static FreeControllerV3 rArmController;
        private static FreeControllerV3 lElbowController;
        private static FreeControllerV3 rElbowController;
        private static FreeControllerV3 lFootController;
        private static FreeControllerV3 rFootController;

        private static bool lookAction = false;
        private static float lookVariation = 1.0f;
        private static float browVariation = 1.0f;
        private static float eyeVariation = 1.0f;
        private static float mouthVariation = 1.0f;
        private static float saccadeAmount = 0.0f;
		private static float shoulderUp = 0.0f;

		
		
		protected Rigidbody lipTrigger;
		private static float lipsTouchCount = 0.0f;
		protected Rigidbody vagTrigger;
		private static float vagTouchCount = 0.0f;
		private static bool lipsOnly = false;
		
        //Hand Morphs
        private static bool doHands = true;
        private static DAZMorph morphLHandFist;
        private static float mLHandFistValue = 0.0f;
        private static float mLHandFistTarget = 0.0f;
        private static DAZMorph morphRHandFist;
        private static float mRHandFistValue = 0.0f;
        private static float mRHandFistTarget = 0.0f;
        private static DAZMorph morphLHandStraighten;
        private static float mLHandStraightenValue = 0.0f;
        private static float mLHandStraightenTarget = 0.0f;
        private static DAZMorph morphRHandStraighten;
        private static float mRHandStraightenValue = 0.0f;
        private static float mRHandStraightenTarget = 0.0f;


        //Eye Brow Morphs
        private static bool morphBrowAction = false;
        private static DAZMorph morphExpExcitement;
        private static float mExcitementValue = 0.0f;
        private static float mExcitementTarget = 0.0f;
        private static DAZMorph morphBrowDown;
        private static float mBrowDownValue = 0.0f;
        private static float mBrowDownTarget = 0.0f;
        private static DAZMorph morphBrowUp;
        private static float mBrowUpValue = 0.0f;
        private static float mBrowUpTarget = 0.0f;
        private static DAZMorph morphBrowCenterUp;
        private static float mBrowCenterUpValue = 0.0f;
        private static float mBrowCenterUpTarget = 0.0f;
        private static DAZMorph morphBrowOuterUpLeft;
        private static float mBrowOuterUpLeftValue = 0.0f;
        private static float mBrowOuterUpLeftTarget = 0.0f;
        private static DAZMorph morphBrowOuterUpRight;
        private static float mBrowOuterUpRightValue = 0.0f;
        private static float mBrowOuterUpRightTarget = 0.0f;

        private static Vector3 saccadeOffset;
        //Eye Morphs
        private static bool morphEyeAction = false;
		private static string lookAwaySide = "left";
        private static DAZMorph morphEyesClosedLeft;
        private static float mEyesClosedLeftValue = 0.0f;
        private static float mEyesClosedLeftTarget = 0.0f;
        private static float mEyesClosedLeftBlinkTarget = 0.0f;
        private static DAZMorph morphEyesClosedRight;
        private static float mEyesClosedRightValue = 0.0f;
        private static float mEyesClosedRightTarget = 0.0f;
        private static float mEyesClosedRightBlinkTarget = 0.0f;
        private static DAZMorph morphEyesSquint;
        private static float mEyesSquintValue = 0.0f;
        private static float mEyesSquintTarget = 0.0f;

        //Mouth Morphs
        private static bool morphMouthAction = false;
        private static bool morphBlinking = false;
		private static float blinkTimer = 0.0f;
        private static DAZMorph morphNoseFlare;
        private static float mNoseFlareValue = 0.0f;
        private static float mNoseFlareTarget = 0.0f;
        private static DAZMorph morphExpSmileFullFace;
        private static float mSmileFullFaceValue = 0.0f;
        private static float mSmileFullFaceTarget = 0.0f;
        private static DAZMorph morphExpSmileOpenFullFace;
        private static float mSmileOpenFullFaceValue = 0.0f;
        private static float mSmileOpenFullFaceTarget = 0.0f;
        private static DAZMorph morphExpGlare;
        private static float mGlareValue = 0.0f;
        private static float mGlareTarget = 0.0f;
        private static DAZMorph morphExpHappy;
        private static float mHappyValue = 0.0f;
        private static float mHappyTarget = 0.0f;
        private static DAZMorph morphExpFlirting;
        private static float mFlirtingValue = 0.0f;
        private static float mFlirtingTarget = 0.0f;
        private static DAZMorph morphExpDeserveIt;
        private static float mDeserveItValue = 0.0f;
        private static float mDeserveItTarget = 0.0f;
        private static DAZMorph morphExpTakingIt;
        private static float mTakingItValue = 0.0f;
        private static float mTakingItTarget = 0.0f;
        private static DAZMorph morphMouthMouthOpen;
        private static float mMouthOpenValue = 0.0f;
        private static float mMouthOpenTarget = 0.0f;
        private static DAZMorph morphMouthMouthOpenWide;
        private static float mMouthOpenWideValue = 0.0f;
        private static float mMouthOpenWideTarget = 0.0f;
        private static DAZMorph morphMouthOpenWider;
        private static float mMouthOpenWiderValue = 0.0f;
        private static float mMouthOpenWiderTarget = 0.0f;
        private static DAZMorph morphMouthNarrow;
        private static float mMouthNarrowValue = 0.0f;
        private static float mMouthNarrowTarget = 0.0f;
        private static DAZMorph morphMouthSideLeft;
        private static float mMouthSideLeftValue = 0.0f;
        private static float mMouthSideLeftTarget = 0.0f;
        private static DAZMorph morphMouthSideRight;
        private static float mMouthSideRightValue = 0.0f;
        private static float mMouthSideRightTarget = 0.0f;
        private static DAZMorph morphMouthSmileSimpleLeft;
        private static float mSmileSimpleLeftValue = 0.0f;
        private static float mSmileSimpleLeftTarget = 0.0f;
        private static DAZMorph morphMouthSmileSimpleRight;
        private static float mSmileSimpleRightValue = 0.0f;
        private static float mSmileSimpleRightTarget = 0.0f;
        private static DAZMorph morphLipsLipsPucker;
        private static float mLipsPuckerValue = 0.0f;
        private static float mLipsPuckerTarget = 0.0f;
        private static DAZMorph morphLipsLipsPuckerWide;
        private static float mLipsPuckerWideValue = 0.0f;
        private static float mLipsPuckerWideTarget = 0.0f;
        private static DAZMorph morphLipsLipsClose;
        private static float mLipsCloseValue = 0.0f;
        private static float mLipsCloseTarget = 0.0f;
        private static DAZMorph morphVisF;
        private static float mVisFValue = 0.0f;
        private static float mVisFTarget = 0.0f;


        private static DAZMorph morphTongueInOut;
        private static float mTongueInOutValue = 1.0f;
        private static float mTongueInOutTarget = 1.0f;
        private static DAZMorph morphTongueSideSide;
        private static float mTongueSideSideValue = 0.0f;
        private static float mTongueSideSideTarget = 0.0f;
        private static DAZMorph morphTongueBendTip;
        private static float mTongueBendTipValue = 0.0f;
        private static float mTongueBendTipTarget = 0.0f;
        private static DAZMorph morphTongueLength;
        private static float mTongueTongueLengthValue = 0.0f;
        private static float mTongueTongueLengthTarget = 0.0f;

        private static DAZMorph morphRibCageSize;
        private static float mRibCageSizeOrig = 0.0f;
        private static float mRibCageSizeValue = 0.0f;
        private static float mRibCageSizeTarget = 0.0f;
        private static DAZMorph morphChestHeight;
        private static float mChestHeightOrig = 0.0f;
        private static float mChestHeightValue = 0.0f;
        private static float mChestHeightTarget = 0.0f;
        private static DAZMorph morphBreastHeight;
        private static float mBreastHeightOrig = 0.0f;
        private static float mBreastHeightValue = 0.0f;
        private static float mBreastHeightTarget = 0.0f;
        private static DAZMorph morphRibsDef;
        private static float mRibsDefOrig = 0.0f;
        private static float mRibsDefValue = 0.0f;
        private static float mRibsDefTarget = 0.0f;
        private static DAZMorph morphBreath;
        private static float mBreathOrig = 0.0f;
        private static float mBreathValue = 0.0f;
        private static float mBreathTarget = 0.0f;
        private static DAZMorph morphSternumDepth;
        private static float mSternumDepthOrig = 0.0f;
        private static float mSternumDepthValue = 0.0f;
        private static float mSternumDepthTarget = 0.0f;
        private static float breathClock = 0.0f;
        private static float breathHold = 0.0f;
        private static float breathCount = 0.0f;
        private static float breatheInSpeed = 1.1f;
        private static float breatheOutSpeed = 1.1f;
        private static float breathRate = Random.Range(0.2f, 0.3f);
        private static string breathState = "in";
        private static DAZMorph morphNipplesApply;
        private static float mNipplesApplyOrig = 0.0f;
        private static float mNipplesApplyValue = 0.0f;
        private static float mNipplesApplyTarget = 0.0f;
        private static DAZMorph morphDeepBulgeBellyBottom;
        private static float mDeepBulgeBellyBottomOrig = 0.0f;
        private static float mDeepBulgeBellyBottomValue = 0.0f;
        private static float mDeepBulgeBellyBottomTarget = 0.0f;
        private static DAZMorph morphDeepBulgeBellyMid;
        private static float mDeepBulgeBellyMidOrig = 0.0f;
        private static float mDeepBulgeBellyMidValue = 0.0f;
        private static float mDeepBulgeBellyMidTarget = 0.0f;
        private static DAZMorph morphDeepThroat;
        private static float mDeepThroatOrig = 0.0f;
        private static float mDeepThroatValue = 0.0f;
        private static float mDeepThroatTarget = 0.0f;
        private static DAZMorph morphBlowjobLips;
        private static float mBlowjobLipsOrig = 0.0f;
        private static float mBlowjobLipsValue = 0.0f;
        private static float mBlowjobLipsTarget = 0.0f;
        private static DAZMorph morphCheekSink;
        private static float mCheekSinkOrig = 0.0f;
        private static float mCheekSinkValue = 0.0f;
        private static float mCheekSinkTarget = 0.0f;
		
		


        private static bool usePerson2;
		private static Atom currentAtom;
		private static string currentAtomName;
        private static bool person2Usable;
        private static Atom person2;
		private static bool person2IsMale = false;

        private static Transform player;
        public static Transform playerVRLHand = SuperController.singleton.leftHand;
        public static Transform playerVRRHand = SuperController.singleton.rightHand;
        private static bool playerHandsUsable;
        private static FreeControllerV3 playerHeadController;
        private static FreeControllerV3 playerChestController;
        private static FreeControllerV3 playerLHandController;
        private static FreeControllerV3 playerRHandController;
        private static FreeControllerV3 playerPelvisController;
        private static FreeControllerV3 playerTipController;
        private static FreeControllerV3 playerTipBaseController;


        private static Vector3 playerFace;
        private static Vector3 playerFacePrev;
        private static Vector3 playerFaceRot;
        private static Vector3 playerFaceRotPrev;
        private static Vector3 playerChest;
        private static Vector3 playerLHand;
        private static Vector3 playerLHandPrev;
        private static Vector3 playerRHand;
        private static Vector3 playerRHandPrev;
        private static Vector3 playerPelvis;
        private static Vector3 playerTip;
        private static Vector3 playerTipPrev;
        private static Vector3 playerTipBase;
        private static Vector3 playerGround;
        private static Vector3 randomPointForward;
        private static Vector3 randomPointLeft;
        private static Vector3 randomPointRight;
        private static Vector3 randomPointUp;

        private static Transform personHeadTransform;
        private static float headToFaceRot;
        private static float playerHeadToFaceRot;
        private static float headToChestRot;
        private static float headToLHandRot;
        private static float headToRHandRot;
        private static float headToPelvisRot;
        private static float headToTipRot;

        private static Transform playerHeadTransform;
        private static float personChestToHead;
        private static float playerToHead;
        private static float playerToLBreast;
        private static float playerToRBreast;
        private static float playerToPelvis;
        private static float playerToLHand;
        private static float playerToRHand;
        private static float playerToPLHand;
        private static float playerToPRHand;
        private static float playerToLFoot;
        private static float playerToRFoot;
		private static bool playerLHandInteract;
		private static bool playerRHandInteract;
		private static bool playerHeadInteract;
		private static bool playerPenisInteract;

        private static float playerHeadToHead;
        private static float playerHeadToLHand;
        private static float playerHeadToRHand;
        private static float playerHeadToLBreast;
        private static float playerHeadToRBreast;
        private static float playerHeadToPelvis;

        private static Transform playerLHandTransform;
        private static float personChestToLHand;
        private static float playerLHandToHead;
        private static float playerLHandToLHand;
        private static float playerLHandToRHand;
        private static float playerLHandToLBreast;
        private static float playerLHandToRBreast;
        private static float playerLHandToPelvis;

        private static Transform playerRHandTransform;
        private static float personChestToRHand;
        private static float playerRHandToHead;
        private static float playerRHandToLHand;
        private static float playerRHandToRHand;
        private static float playerRHandToLBreast;
        private static float playerRHandToRBreast;
        private static float playerRHandToPelvis;

        private static float playerTipToHead;
        private static float playerTipToLHand;
        private static float playerTipToRHand;
        private static float playerTipToLBreast;
        private static float playerTipToRBreast;
        private static float playerTipToPelvis;

        private static float playerPelvisToHead;

        private static bool playerHeadMovement;
        private static bool playerLHandMovement;
        private static bool playerRHandMovement;
        private static bool playerTipMovement;
        private static bool emTargetMovement;

        private static float playerHeadTimeout;
        private static float playerLHandTimeout;
        private static float playerRHandTimeout;
        private static float playerTipTimeout;
        private static float emTargetTimeout;

        private static float minHeadMotion;
        private static float minHandMotion;
        private static float minTipMotion;

        private static float minFaceDistance;
        private static float closeFaceDistance;
        private static float personalSpaceDistance;
        private static float backgroundDistance;
        private static float interactionDistance;
		private static float kissingDistance = 0.285f;
		private static float kissingAngle = 0.0f;
		private static float kissingAmount = 1.0f;


        private static float lookDirectAngle;
        private static float lookPeripheralAngle;
        private static float lookNoAwarenessAngle;

        private static float movementMaxTimeout;
        private static float movementModifier;
        private static float movementFalloff;

        private static float playerInterest;
        private static float interestFace;
        private static float interestLHand;
        private static float interestRHand;
        private static float interestPelvis;
        private static float interestTip;
        private static float interestFaceBase = 60.0f;
        private static float interestLHandBase = 45.0f;
        private static float interestRHandBase = 45.0f;
        private static float interestPelvisBase = 20.0f;
        private static float interestTipBase = 30.0f;
		private static float interestEMTargetBase = 20.0f;

        private static float randomInterest;

        #endregion
        static void ObserveLipTrigger(object sender, TriggerEventArgs e)
        {
            //Do whatever you want here
			if (e.evtType == "Entered")
			{
				lipsTouchCount += 1.0f;
			}
			else
			{
				lipsTouchCount = Mathf.Clamp(lipsTouchCount - 1.0f,0.0f,100.0f);
			}
            //SuperController.LogMessage(e.evtType + "(" + lipsTouchCount + ") =" + e.collider.transform.parent.name);
        }
		
        static void ObserveVagTrigger(object sender, TriggerEventArgs e)
        {
            //Do whatever you want here
			if (e.evtType == "Entered")
			{
				vagTouchCount += 1.0f;
			}
			else
			{
				vagTouchCount = Mathf.Clamp(vagTouchCount - 1.0f,0.0f,100.0f);
			}
            //SuperController.LogMessage(e.evtType + "(" + lipsTouchCount + ") =" + e.collider.transform.parent.name);
        }
		
        public override void Init()
        {

            ////SuperController.LogError("public void OnPreLoad()");
            //emotionSM = new StateMachine();
            lookSM = new StateMachine();
            systemSM = new StateMachine();
            browSM = new StateMachine();
            mouthSM = new StateMachine();
            eyesSM = new StateMachine();

			lipTrigger = containingAtom.rigidbodies.First(rb => rb.name == "LipTrigger");
			lipTrigger.gameObject.AddComponent<TriggerCollide>().OnCollide += ObserveLipTrigger;

			vagTrigger = containingAtom.rigidbodies.First(rb => rb.name == "VaginaTrigger");
			vagTrigger.gameObject.AddComponent<TriggerCollide>().OnCollide += ObserveVagTrigger;
			
            minHeadMotion = 0.005f;
            minHandMotion = 0.015f;
            minTipMotion = 0.21f;

            minFaceDistance = 0.065f;
            closeFaceDistance = 0.075f;
            personalSpaceDistance = 0.85f;
            backgroundDistance = 1.2f;
            interactionDistance = 0.13f;

            movementMaxTimeout = 15.0f;
            movementModifier = 0.085f;
            movementFalloff = 0.11f;

			uiAgreeableness = new JSONStorableFloat("Personality Agreeableness", 50.0f, 1.0f, 99.0f, true, true);
			RegisterFloat(uiAgreeableness);
			CreateSlider(uiAgreeableness, false);

			uiExtraversion = new JSONStorableFloat("Personality Extraversion", 50.0f, 1.0f, 99.0f, true, true);
			RegisterFloat(uiExtraversion);
			CreateSlider(uiExtraversion, false);

			uiStableness = new JSONStorableFloat("Personality Stableness", 50.0f, 1.0f, 99.0f, true, true);
			RegisterFloat(uiStableness);
			CreateSlider(uiStableness, false);

			uiInterestSpeed = new JSONStorableFloat("Main Change Delay Mult", 1.0f, 0.0f, 5.0f, true, true);
			RegisterFloat(uiInterestSpeed);
			CreateSlider(uiInterestSpeed, false);

			uiArousalSpeed = new JSONStorableFloat("Arousal Speed Mult", 1.0f, 0.0f, 10.0f, true, true);
			RegisterFloat(uiArousalSpeed);
			CreateSlider(uiArousalSpeed, false);

			uiValenceSpeed = new JSONStorableFloat("Valence Speed Mult", 1.0f, 0.0f, 10.0f, true, true);
			RegisterFloat(uiValenceSpeed);
			CreateSlider(uiValenceSpeed, false);

			uiMoodSpeed = new JSONStorableFloat("Mood Degrade Speed Mult", 1.0f, 0.0f, 10.0f, true, true);
			RegisterFloat(uiMoodSpeed);
			CreateSlider(uiMoodSpeed, false);
			
			uiDoKiss = new JSONStorableBool("Auto Kissing", true);
			RegisterBool(uiDoKiss);
			CreateToggle(uiDoKiss, false);
			
			uiKissAmount = new JSONStorableFloat("Kissing Effect Mult", 1.0f, 0.0f, 1.5f, true, true);
			RegisterFloat(uiKissAmount);
			CreateSlider(uiKissAmount, false);

			uiDoBlowjob = new JSONStorableBool("Auto Blowjob", true);
			RegisterBool(uiDoBlowjob);
			CreateToggle(uiDoBlowjob, false);
			
			uiBlowjobAmount = new JSONStorableFloat("Blowjob Effect Mult", 0.7f, 0.0f, 1.5f, true, true);
			RegisterFloat(uiBlowjobAmount);
			CreateSlider(uiBlowjobAmount, false);

			uiDoSex = new JSONStorableBool("Auto Sex", true);
			RegisterBool(uiDoSex);
			CreateToggle(uiDoSex, false);

			uiSexAmount = new JSONStorableFloat("Sex Effect Mult", 0.75f, 0.0f, 1.5f, true, true);
			RegisterFloat(uiSexAmount);
			CreateSlider(uiSexAmount, false);
			
			uiGazeAvoid = new JSONStorableBool("Gaze Avoidance", true);
			RegisterBool(uiGazeAvoid);
			CreateToggle(uiGazeAvoid, false);

			uiGazeLookTime = new JSONStorableFloat("Avoidance Eye Contact Time", 1.0f, 0.0f, 5.0f, true, true);
			RegisterFloat(uiGazeLookTime);
			CreateSlider(uiGazeLookTime, false);

			uiGazeAvoidTime = new JSONStorableFloat("Avoidance Look Away Time", 1.0f, 0.0f, 5.0f, true, true);
			RegisterFloat(uiGazeAvoidTime);
			CreateSlider(uiGazeAvoidTime, false);

			uiGazeGlance = new JSONStorableBool("Gaze Glancing", true);
			RegisterBool(uiGazeGlance);
			CreateToggle(uiGazeGlance, false);

			uiGazeSpeed = new JSONStorableFloat("Gaze Speed Mult", 2.0f, 0.0f, 7.0f, true, true);
			RegisterFloat(uiGazeSpeed);
			CreateSlider(uiGazeSpeed, false);

			uiGazeVariation = new JSONStorableFloat("Gaze Variation Mult", 1.0f, 0.0f, 3.0f, true, true);
			RegisterFloat(uiGazeVariation);
			CreateSlider(uiGazeVariation, false);

			uiRollSpeed = new JSONStorableFloat("Gaze Tilt Speed Mult", 1.0f, 0.0f, 3.0f, true, true);
			RegisterFloat(uiRollSpeed);
			CreateSlider(uiRollSpeed, false);
			
			uiBreatheSpeed = new JSONStorableFloat("Breath Speed Mult", 1.0f, 0.0f, 3.0f, true, true);
			RegisterFloat(uiBreatheSpeed);
			CreateSlider(uiBreatheSpeed, false);

			uiBreatheRaiseMultiplier = new JSONStorableFloat("Breath Raise Mult", 0.7f, 0.0f, 3.0f, true, true);
			RegisterFloat(uiBreatheRaiseMultiplier);
			CreateSlider(uiBreatheRaiseMultiplier, false);

			uiBreatheExpandMultiplier = new JSONStorableFloat("Breath Expansion Mult", 0.7f, 0.0f, 3.0f, true, true);
			RegisterFloat(uiBreatheExpandMultiplier);
			CreateSlider(uiBreatheExpandMultiplier, false);

			uiBlinkSpeed = new JSONStorableFloat("Blink Delay Mult", 1.0f, 0.0f, 5.0f, true, true);
			RegisterFloat(uiBlinkSpeed);
			CreateSlider(uiBlinkSpeed, false);

			uiSaccadeSpeed = new JSONStorableFloat("Saccade Rate Mult", 1.0f, 0.0f, 5.0f, true, true);
			RegisterFloat(uiSaccadeSpeed);
			CreateSlider(uiSaccadeSpeed, false);
			
			uiSaccadeAmount = new JSONStorableFloat("Saccade Amount", 1.0f, 0.0f, 5.0f, true, true);
			RegisterFloat(uiSaccadeAmount);
			CreateSlider(uiSaccadeAmount, false);

			uiSaccadeWanderMult = new JSONStorableFloat("Saccade Max Dist Mult", 1.0f, 0.0f, 5.0f, true, true);
			RegisterFloat(uiSaccadeWanderMult);
			CreateSlider(uiSaccadeWanderMult, false);
			
			uiShowStats = new JSONStorableBool("Show Stats on Message Log", false);
			RegisterBool(uiShowStats);
			CreateToggle(uiShowStats, true);

			uiLoadDefaults = new JSONStorableBool("Load Defaults", false);
			RegisterBool(uiLoadDefaults);
			CreateToggle(uiLoadDefaults, true);

			uiLoadPreset = new JSONStorableBool("Load Preset", false);
			RegisterBool(uiLoadPreset);
			CreateToggle(uiLoadPreset, true);

			uiSavePreset = new JSONStorableBool("Save Preset", false);
			RegisterBool(uiSavePreset);
			CreateToggle(uiSavePreset, true);

			uiConfigHead = new JSONStorableBool("Auto Config Head", true);
			RegisterBool(uiConfigHead);
			CreateToggle(uiConfigHead, true);

			uiDoHead = new JSONStorableBool("Control Head", true);
			RegisterBool(uiDoHead);
			CreateToggle(uiDoHead, true);

			uiDoMorphs = new JSONStorableBool("Control Morphs", true);
			RegisterBool(uiDoMorphs);
			CreateToggle(uiDoMorphs, true);

			uiDoShoulders = new JSONStorableBool("Adjust Shoulders", true);
			RegisterBool(uiDoShoulders);
			CreateToggle(uiDoShoulders, true);

			uiShoulderAmount = new JSONStorableFloat("Shoulder Adjust Mult", 1.00f, 0.0f, 10.0f, true, true);
			RegisterFloat(uiShoulderAmount);
			CreateSlider(uiShoulderAmount, true);

			uiDoChest = new JSONStorableBool("Adjust Chest", true);
			RegisterBool(uiDoChest);
			CreateToggle(uiDoChest, true);

			uiChestAmount = new JSONStorableFloat("Chest Adjust Mult", 1.00f, 0.0f, 10.0f, true, true);
			RegisterFloat(uiChestAmount);
			CreateSlider(uiChestAmount, true);

			uiDoHands = new JSONStorableBool("Adjust Hands", true);
			RegisterBool(uiDoHands);
			CreateToggle(uiDoHands, true);

			uiUsePerson2 = new JSONStorableBool("Look at selected Person", false);
			RegisterBool(uiUsePerson2);
			CreateToggle(uiUsePerson2, true);
			
			List<string> targetChoices = new List<string>();
            foreach (string atomUID in SuperController.singleton.GetAtomUIDs())
            {
				currentAtom = SuperController.singleton.GetAtomByUid(atomUID);
                if (currentAtom != containingAtom && atomUID != null && currentAtom.type == "Person")
                {
					targetChoices.Add(atomUID);
				}
			}
			uiFocusTarget = new JSONStorableStringChooser("Target Selector", targetChoices, "None", "Choose Person");
			//RegisterArray(targetChoices);
			RegisterStringChooser(uiFocusTarget);
			UIDynamicPopup udp = CreatePopup(uiFocusTarget, true);

			List<string> objectChoices = new List<string>();
			objectChoices.Add("None");
            foreach (string atomUID in SuperController.singleton.GetAtomUIDs())
            {
				currentAtom = SuperController.singleton.GetAtomByUid(atomUID);
                if (atomUID != null)
                {
						objectChoices.Add(atomUID);
/*					if (currentAtom.type == "Person")
					{
						FreeControllerV3 tempController = currentAtom.GetStorableByID("headControl") as FreeControllerV3;
						if (tempController != null)
						{
							objectChoices.Add(atomUID);
						}
					}
					else
					{
					}
					*/
				}
			}
			uiObjectTarget = new JSONStorableStringChooser("Object Selector", objectChoices, "None", "Choose Object");
			//RegisterArray(objectChoices);
			RegisterStringChooser(uiObjectTarget);
			UIDynamicPopup udp2 = CreatePopup(uiObjectTarget, true);

			uiTargetLook = new JSONStorableBool("Object Can Look", false);
			RegisterBool(uiTargetLook);
			CreateToggle(uiTargetLook, true);

			uiMaxMorphSmile = new JSONStorableFloat("Max Smile (Morphs)", 0.6f, 0.0f, 1.0f, true, true);
			RegisterFloat(uiMaxMorphSmile);
			CreateSlider(uiMaxMorphSmile, true);
			
			uiEyeCloseMaxMorph = new JSONStorableFloat("Max Eye Close (Morphs)", 1.1f, 0.0f, 1.5f, true, true);
			RegisterFloat(uiEyeCloseMaxMorph);
			CreateSlider(uiEyeCloseMaxMorph, true);

			uiPersonalSpace = new JSONStorableFloat("Personal Space", 1.0f, 0.0f, 3.0f, true, true);
			RegisterFloat(uiPersonalSpace);
			CreateSlider(uiPersonalSpace, true);

			uiInteractDist = new JSONStorableFloat("Interaction Distance", 0.14f, 0.0f, 0.5f, true, true);
			RegisterFloat(uiInteractDist);
			CreateSlider(uiInteractDist, true);

			uiCloseToFaceDist = new JSONStorableFloat("Face Interact Dist", 0.13f, 0.0f, 0.5f, true, true);
			RegisterFloat(uiCloseToFaceDist);
			CreateSlider(uiCloseToFaceDist, true);

			uiKissingDist = new JSONStorableFloat("Kissing Dist", 0.27f, 0.0f, 0.5f, true, true);
			RegisterFloat(uiKissingDist);
			CreateSlider(uiKissingDist, true);

			uiDirectGaze = new JSONStorableFloat("Direct View Angle", 12.0f, 0.0f, 180.0f, true, true);
			RegisterFloat(uiDirectGaze);
			CreateSlider(uiDirectGaze, true);

			uiPeripheralGaze = new JSONStorableFloat("Peripheral View Angle", 45.0f, 0.0f, 180.0f, true, true);
			RegisterFloat(uiPeripheralGaze);
			CreateSlider(uiPeripheralGaze, true);

			uiOutOfGaze = new JSONStorableFloat("Out of View Angle", 90.0f, 0.0f, 180.0f, true, true);
			RegisterFloat(uiOutOfGaze);
			CreateSlider(uiOutOfGaze, true);

			uiObjectInterest = new JSONStorableFloat("Object Interest Mult", 1.0f, 0.0f, 3.0f, true, true);
			RegisterFloat(uiObjectInterest);
			CreateSlider(uiObjectInterest, true);

			uiHeadInterest = new JSONStorableFloat("Head Interest Mult", 1.0f, 0.0f, 3.0f, true, true);
			RegisterFloat(uiHeadInterest);
			CreateSlider(uiHeadInterest, true);

			uiLHandInterest = new JSONStorableFloat("LHand Interest Mult", 1.0f, 0.0f, 3.0f, true, true);
			RegisterFloat(uiLHandInterest);
			CreateSlider(uiLHandInterest, true);

			uiRHandInterest = new JSONStorableFloat("RHand Interest Mult", 1.0f, 0.0f, 3.0f, true, true);
			RegisterFloat(uiRHandInterest);
			CreateSlider(uiRHandInterest, true);

			uiPenisInterest = new JSONStorableFloat("Penis Interest Mult", 1.0f, 0.0f, 3.0f, true, true);
			RegisterFloat(uiPenisInterest);
			CreateSlider(uiPenisInterest, true);

			uiInterestRate = new JSONStorableFloat("Interest Rate Mult", 1.0f, 0.0f, 10.0f, true, true);
			RegisterFloat(uiInterestRate);
			CreateSlider(uiInterestRate, true);

			uiEyeUpdate = new JSONStorableFloat("Eye Pos Update Mult", 0.3f, 0.0f, 0.5f, true, true);
			RegisterFloat(uiEyeUpdate);
			CreateSlider(uiEyeUpdate, true);

			
            lookDirectAngle = uiDirectGaze.val;// * (playerHeadToHead / personalSpaceDistance);
            lookPeripheralAngle = uiPeripheralGaze.val;
            lookNoAwarenessAngle = uiOutOfGaze.val;

            usePerson2 = uiUsePerson2.val;

            //pre init
            player = CameraTarget.centerTarget.transform;
            playerInterest = 0.0f;
            randomInterest = 0.0f;
            interestFace = 0.0f;
            interestLHand = 0.0f;
            interestRHand = 0.0f;
            interestPelvis = 0.0f;
            interestTip = 0.0f;
			interestEMTarget = 0.0f;

            playerHeadTimeout = 0.0f;
            playerLHandTimeout = 0.0f;
            playerRHandTimeout = 0.0f;
            playerTipTimeout = 0.0f;
			emTargetTimeout = 0.0f;
			//emTargetPosPrev = new Vector3(0.0f,0.0f,0.0f);

            playerLHandMovement = false;
            playerRHandMovement = false;
            playerTipMovement = false;
            emTargetMovement = false;

            //fallbacks incase hands/person2 missing
            headToLHandRot = 180.0f;
            headToRHandRot = 180.0f;
            headToPelvisRot = 180.0f;
            headToTipRot = 180.0f;
            playerLHandToHead = backgroundDistance;
            playerRHandToHead = backgroundDistance;
            playerLHandToLHand = backgroundDistance;
            playerRHandToLHand = backgroundDistance;
            playerRHandToRHand = backgroundDistance;
            playerRHandToRHand = backgroundDistance;
            playerLHandToLBreast = backgroundDistance;
            playerRHandToLBreast = backgroundDistance;
            playerLHandToRBreast = backgroundDistance;
            playerRHandToRBreast = backgroundDistance;
            playerLHandToPelvis = backgroundDistance;
            playerRHandToPelvis = backgroundDistance;
            playerPelvisToHead = backgroundDistance;
            playerTipToHead = backgroundDistance;
            playerTipToLHand = backgroundDistance;
            playerTipToRHand = backgroundDistance;
            playerTipToLBreast = backgroundDistance;
            playerTipToRBreast = backgroundDistance;
            playerTipToPelvis = backgroundDistance;
			headPrevPos = new Vector3(0.0f,0.0f,0.0f);

        }

        public void Start()
               {
            ////SuperController.LogError("public void OnPostLoad()");
            //debugUI = Utils.GetAtom("debugText");
            //debugUIControl = debugUI.GetStorableByID("control") as UITextControl;
			//emTarget = SuperController.singleton.GetAtomByUid("EMTarget");
			
            person = containingAtom;//SuperController.singleton.GetAtomByUid("Person");
            if (person != null)
            {
				//SuperController.LogError("Person found");
                JSONStorable js = person.GetStorableByID("geometry");
                DAZCharacterSelector dcs = js as DAZCharacterSelector;
                GenerateDAZMorphsControlUI morphUI = dcs.morphsControlUI;

                headController = person.GetStorableByID("headControl") as FreeControllerV3;
                headActual = person.GetStorableByID("head") as FreeControllerV3;
                eyeController = person.GetStorableByID("eyeTargetControl") as FreeControllerV3;
                neckController = person.GetStorableByID("neckControl") as FreeControllerV3;
                chestController = person.GetStorableByID("chestControl") as FreeControllerV3;
                lBreastController = person.GetStorableByID("lNippleControl") as FreeControllerV3;
                rBreastController = person.GetStorableByID("rNippleControl") as FreeControllerV3;
                pelvisController = person.GetStorableByID("hipControl") as FreeControllerV3;
                lHandController = person.GetStorableByID("lHandControl") as FreeControllerV3;
                rHandController = person.GetStorableByID("rHandControl") as FreeControllerV3;
                lShoulderController = person.GetStorableByID("lShoulderControl") as FreeControllerV3;
                rShoulderController = person.GetStorableByID("rShoulderControl") as FreeControllerV3;
                lArmController = person.GetStorableByID("lArmControl") as FreeControllerV3;
                rArmController = person.GetStorableByID("rArmControl") as FreeControllerV3;
                lElbowController = person.GetStorableByID("lElbowControl") as FreeControllerV3;
                rElbowController = person.GetStorableByID("rElbowControl") as FreeControllerV3;
                lFootController = person.GetStorableByID("lFootControl") as FreeControllerV3;
                rFootController = person.GetStorableByID("rFootControl") as FreeControllerV3;
                refAngle = new Vector3(0.0f,-10.0f,-10.0f);
                if (morphUI != null)
                {
					//SuperController.LogError("Morph Controller Found");
                    morphLHandFist = morphUI.GetMorphByDisplayName("Left Hand Fist");
                    morphRHandFist = morphUI.GetMorphByDisplayName("Right Hand Fist");
                    morphLHandStraighten = morphUI.GetMorphByDisplayName("Left Hand Straighten");
                    morphRHandStraighten = morphUI.GetMorphByDisplayName("Right Hand Straighten");

                    morphBrowDown = morphUI.GetMorphByDisplayName("Brow Down");
                    morphBrowUp = morphUI.GetMorphByDisplayName("Brow Up");
                    morphBrowCenterUp = morphUI.GetMorphByDisplayName("Brow Inner Up");
                    morphBrowOuterUpLeft = morphUI.GetMorphByDisplayName("Brow Outer Up Left");
                    morphBrowOuterUpRight = morphUI.GetMorphByDisplayName("Brow Outer Up Right");

                    morphEyesClosedLeft = morphUI.GetMorphByDisplayName("Eyes Closed Left");
                    morphEyesClosedRight = morphUI.GetMorphByDisplayName("Eyes Closed Right");
                    morphEyesSquint = morphUI.GetMorphByDisplayName("Eyes Squint");

                    morphNoseFlare = morphUI.GetMorphByDisplayName("Nose ala relax");
                    morphExpSmileFullFace = morphUI.GetMorphByDisplayName("Smile Full Face");
                    morphExpSmileOpenFullFace = morphUI.GetMorphByDisplayName("Smile Open Full Face");
                    morphExpGlare = morphUI.GetMorphByDisplayName("Glare");
                    morphExpExcitement = morphUI.GetMorphByDisplayName("Concentrate");
                    morphExpHappy = morphUI.GetMorphByDisplayName("Happy");
                    morphExpFlirting = morphUI.GetMorphByDisplayName("Flirting");
                    morphExpDeserveIt = morphUI.GetMorphByDisplayName("Deserving It");
                    morphExpTakingIt = morphUI.GetMorphByDisplayName("Taking It");
                    morphMouthMouthOpen = morphUI.GetMorphByDisplayName("Mouth Open");
                    morphMouthMouthOpenWide = morphUI.GetMorphByDisplayName("Mouth Open Wide");
                    morphMouthOpenWider = morphUI.GetMorphByDisplayName("Mouth Open Wide 3");
                    morphMouthNarrow = morphUI.GetMorphByDisplayName("Mouth Narrow");
                    morphMouthSideLeft = morphUI.GetMorphByDisplayName("Mouth Side-Side Left");
                    morphMouthSideRight = morphUI.GetMorphByDisplayName("Mouth Side-Side Right");
                    morphMouthSmileSimpleLeft = morphUI.GetMorphByDisplayName("Mouth Smile Simple Left");
                    morphMouthSmileSimpleRight = morphUI.GetMorphByDisplayName("Mouth Smile Simple Right");
                    morphLipsLipsPucker = morphUI.GetMorphByDisplayName("Lips Pucker");
					if (morphLipsLipsPucker == null)
					{
						morphLipsLipsPucker = morphUI.GetMorphByDisplayName("W");
					}
                    morphLipsLipsPuckerWide = morphUI.GetMorphByDisplayName("Lips Pucker Wide");
                    morphLipsLipsClose = morphUI.GetMorphByDisplayName("Lips Close");
                    morphVisF = morphUI.GetMorphByDisplayName("F");

                    morphTongueInOut = morphUI.GetMorphByDisplayName("Tongue In-Out");
                    morphTongueSideSide = morphUI.GetMorphByDisplayName("Tongue Side-Side");
                    morphTongueBendTip = morphUI.GetMorphByDisplayName("Tongue Curl");
                    morphTongueLength = morphUI.GetMorphByDisplayName("Tongue Length");

                    morphRibCageSize = morphUI.GetMorphByDisplayName("Ribcage Size");
                    morphChestHeight = morphUI.GetMorphByDisplayName("Chest Height");
					if (morphChestHeight == null)
					{
						morphChestHeight = morphUI.GetMorphByDisplayName("Costal Angle Arched");
						mChestHeightOrig = morphChestHeight.morphValue;
						personIsMale = true;
					}
					else
					{
						mChestHeightOrig = morphChestHeight.morphValue;
					}
                    morphBreastHeight = morphUI.GetMorphByDisplayName("Breast Height");
					if (morphBreastHeight == null)
					{
						morphBreastHeight = morphUI.GetMorphByDisplayName("Pectorals Height");
						personIsMale = true;
					}
					morphBreath = morphUI.GetMorphByDisplayName("Breath1");
                    morphRibsDef = morphUI.GetMorphByDisplayName("Ribs Definition");
                    morphSternumDepth = morphUI.GetMorphByDisplayName("Sternum Depth");
                    morphNipplesApply = morphUI.GetMorphByDisplayName("Nipples Apply");
                    morphDeepBulgeBellyBottom = morphUI.GetMorphByDisplayName("deepbulge_bot");
                    morphDeepBulgeBellyMid = morphUI.GetMorphByDisplayName("deepbulge_mid");
                    morphDeepThroat = morphUI.GetMorphByDisplayName("deepthroat");
                    morphBlowjobLips = morphUI.GetMorphByDisplayName("Blowjob Lips");
                    morphCheekSink = morphUI.GetMorphByDisplayName("Cheeks Sink Lower");
					//SuperController.LogError("Morphs Loaded");
					
                }
            }
            playerHandsUsable = false;
            person2Usable = false;
		    //loadDefaults();

            if (playerVRLHand != null && playerVRRHand != null)
            {
				//SuperController.LogError("VR Hands Found");
                playerHandsUsable = true;
            }

            aCube = SuperController.singleton.GetAtomByUid("Cube");
			if (aCube != null)
			{
			aCubeController = aCube.GetStorableByID("control") as FreeControllerV3;
			}
            person2 = SuperController.singleton.GetAtomByUid(uiFocusTarget.val);
            if (person2 != null)
            {
                JSONStorable js = person2.GetStorableByID("geometry");
                DAZCharacterSelector dcs = js as DAZCharacterSelector;
                GenerateDAZMorphsControlUI morphUI = dcs.morphsControlUI;
				if (morphUI != null)
				{
					DAZMorph morphTemp = morphUI.GetMorphByDisplayName("Breast Height");
					person2IsMale = false;
					if (morphTemp == null)
					{
						person2IsMale = true;
					}
				}
				//SuperController.LogError("Person2 Found");
                person2Usable = true;
                playerHeadController = person2.GetStorableByID("headControl") as FreeControllerV3;
                playerChestController = person2.GetStorableByID("chestControl") as FreeControllerV3;
                playerLHandController = person2.GetStorableByID("lHandControl") as FreeControllerV3;
                playerRHandController = person2.GetStorableByID("rHandControl") as FreeControllerV3;
                playerPelvisController = person2.GetStorableByID("pelvisControl") as FreeControllerV3;
                playerTipController = person2.GetStorableByID("penisTipControl") as FreeControllerV3;
                playerTipBaseController = person2.GetStorableByID("penisBaseControl") as FreeControllerV3;
            }
			else
			{
				//SuperController.LogError("No Person2");
			}

            if (person == null || headController == null)
            {
                //SuperController.LogError("[EmotionEngine] Person not found");
                return;
            }
            if (player == null)
            {
                //SuperController.LogError("[EmotionEngine] Player not found");
                return;
            }

            if (usePerson2 && person2Usable)
            {
				//SuperController.LogError("Using Person2");
                playerFace = playerHeadController.followWhenOff.position;
                playerLHand = playerLHandController.followWhenOff.position;
                playerRHand = playerRHandController.followWhenOff.position;
                playerPelvis = playerPelvisController.followWhenOff.position;
                playerTip = playerTipController.followWhenOff.position;
            }
            else
            {
				//SuperController.LogError("Using Camera");
                playerFace = player.position;
                if (person2Usable && usePerson2)
                {
                    playerLHand = playerLHandController.followWhenOff.position;
                    playerRHand = playerRHandController.followWhenOff.position;
                    playerLHandTransform = playerLHandController.followWhenOff;
                    playerRHandTransform = playerRHandController.followWhenOff;
                }
                if (playerHandsUsable && usePerson2 == false)
                {
                    playerLHand = playerVRLHand.position;
                    playerRHand = playerVRRHand.position;
					playerLHandTransform = playerVRLHand;
					playerRHandTransform = playerVRRHand;
                }
                if (person2Usable == false && playerHandsUsable == false)
                {
                    playerLHand = playerFace;
                    playerRHand = playerFace;
                    playerLHandTransform = playerHeadTransform;
                    playerRHandTransform = playerHeadTransform;
                }
				playerPelvis = playerFace;
				playerTip = playerFace;
                if (person2Usable)
                {
                    playerPelvis = playerPelvisController.followWhenOff.position;
                    playerTip = playerTipController.followWhenOff.position;
                }
				else
				{
					uiUsePerson2.val = false;
				}
            }

            if (usePerson2 && person2 != null)
            {
                playerHeadTransform = playerHeadController.followWhenOff;
                closeFaceDistance = closeFaceDistance * 1.85f;
                person2Usable = true;
            }
            else
            {
                playerHeadTransform = player;
            }
			
			personEyes = containingAtom.GetStorableByID("Eyes");
			personEyelids = containingAtom.GetStorableByID("EyelidControl");
            lookAtPosition = eyeController.transform.TransformPoint(new Vector3(0.0f, 0.0f, 0.0f));
			//savePreset();
            systemSM.Switch(sUpdate);
        }

                     
        public void FixedUpdate()
        {
			//SuperController.singleton.ClearErrors();
			player = CameraTarget.centerTarget.transform;
			gHeadSpeed = 1.75f;
			usePerson2 = uiUsePerson2.val;
			oldEyePos = eyeController.transform.position;
			personalSpaceDistance = Mathf.Min(uiPersonalSpace.val,Mathf.Max(playerHeadToHead + 0.5f, closeFaceDistance*3.0f));
			backgroundDistance = personalSpaceDistance * 2.0f;
            lookDirectAngle = Mathf.Lerp(40.0f, uiDirectGaze.val,Mathf.Clamp(playerHeadToHead-kissingDistance,0.0f,personalSpaceDistance));// * (playerHeadToHead / personalSpaceDistance);
            lookPeripheralAngle = uiPeripheralGaze.val;
            lookNoAwarenessAngle = uiOutOfGaze.val;
			closeFaceDistance = uiCloseToFaceDist.val;
			interactionDistance = uiInteractDist.val;
			interestMaxSmile = uiMaxMorphSmile.val;
			doHands = uiDoHands.val;
			kissingDistance = uiKissingDist.val;
			eyeUpdateTime = uiEyeUpdate.val;
			currentAtomName = uiFocusTarget.val;
			kissingAmount = uiKissAmount.val;
			playerLHandInteract = false;
			playerRHandInteract = false;
			playerHeadInteract = false;
			playerPenisInteract = false;
			blinkTimer += Time.fixedDeltaTime;
			
			eyeCloseMaxMorph = uiEyeCloseMaxMorph.val;
			
			if (uiSavePreset.val)
			{
				savePreset();
			}
			if (uiLoadPreset.val)
			{
				loadPreset();
			}
			if (uiLoadDefaults.val)
			{
				loadDefaults();
			}
			
			personEyes.SetStringChooserParamValue("lookMode", "Target");
			personEyelids.SetBoolParamValue("blinkEnabled", false);
			containingAtom.GetStorableByID("AutoExpressions").SetBoolParamValue("enabled", false);
			//SuperController.LogError("Init");

			if (uiFocusTarget.val == "None")
			{
				uiUsePerson2.val = false;
			}
			if (uiObjectTarget.val == "None")
			{
				uiTargetLook.val = false;
			}
			
			if (uiObjectTarget.val != "None")
			{
				emTargetName = uiObjectTarget.val;
				emTarget = SuperController.singleton.GetAtomByUid(uiObjectTarget.val);
				if (emTarget.type == "Person")
				{
					emTargetController = emTarget.GetStorableByID("headControl") as FreeControllerV3;
				}
				else
				{
					emTargetController = emTarget.GetStorableByID("control") as FreeControllerV3;
				}
			}
			else
			{
				emTargetName = "None";
				emTarget = null;
				emTargetController = null;
			}
			
			currentAtom = SuperController.singleton.GetAtomByUid(currentAtomName);
			if (currentAtom != person2 && currentAtom != null)
			{
				person2 = currentAtom;
				systemSM.Switch(sReselectPerson2);
			}

			if (pAgreeableness != uiAgreeableness.val || pExtraversion != uiExtraversion.val || pStableness != uiStableness.val)
				{
				pAgreeableness = uiAgreeableness.val;
				pExtraversion = uiExtraversion.val;
				pStableness = uiStableness.val;
				if (pAgreeableness > 50 || pStableness > 50) { gDirectionCenter = true; } else { gDirectionCenter = false; }
				if (pExtraversion > 75 && pAgreeableness < 25) { gDirectionUp = true; } else { gDirectionUp = false; }
				if (pExtraversion < 25 || pAgreeableness < 50 || pStableness < 50) { gDirectionDown = true; } else { gDirectionDown = false; }
				if (pExtraversion > 50 || pAgreeableness > 50) { gDirectionSide = true; } else { gDirectionSide = false; }
				
				//gAvoidance = Mathf.Clamp((((100.0f - pExtraversion) + (100.0f - pStableness)) / 2.0f), 0.0f, 100.0f);
				gAvoidance = Mathf.Clamp((((pExtraversion) + (pStableness)) / 2.0f), 20.0f, 100.0f);
				gDuration = 5.0f * (Mathf.Clamp((pStableness / 5.0f) + ((100.0f - pAgreeableness) / 2.0f) + (pExtraversion / 5.0f), 0.0f, 100.0f) / 100.0f);
				gFrequency = Mathf.Clamp((pStableness / 2.0f) + (pExtraversion / 2.0f), 0.0f, 100.0f);
				}
			
			if (person != null)
			{
				if (uiConfigHead.val)
				{
					//SuperController.LogError("Config Head");
					//headController.currentPositionState = FreeControllerV3.PositionState.Off;
					headController.currentRotationState = FreeControllerV3.RotationState.On;
					headController.jointRotationDriveSpring = 3;
					headController.jointRotationDriveDamper = 1;
					headController.jointRotationDriveXTarget = 0.0f;
					headController.jointRotationDriveYTarget = 0.0f;
					headController.jointRotationDriveZTarget = 0.0f;
					headController.RBHoldRotationSpring = Mathf.Lerp(25,35,interestValence/10.0f);
					if (currentLook == "Kissing" || currentLook == "Sucking")
					{
						headController.RBHoldRotationSpring = 80;
					}
					headController.RBHoldRotationDamper = Mathf.Lerp(8,3,interestArousal/10.0f);
					if (morphTongueLength != null)
					{
						morphTongueLength.SetValue(0.08f);
					}

					//SuperController.LogError("Config Neck");
					//neckController.currentPositionState = FreeControllerV3.PositionState.Off;
					neckController.currentRotationState = FreeControllerV3.RotationState.On;
					neckController.jointRotationDriveSpring = 30;
					neckController.jointRotationDriveDamper = 40;
					neckController.jointRotationDriveXTarget = -10.0f;
					neckController.jointRotationDriveYTarget = 0.0f;
					neckController.jointRotationDriveZTarget = 0.0f;
					if (currentLook == "Sucking")
					{
						neckController.RBHoldRotationSpring = Mathf.Lerp(65,120,interestValence/10.0f);
						neckController.jointRotationDriveSpring = 60;
						neckController.jointRotationDriveDamper = 40;
						neckController.jointRotationDriveXTarget = Mathf.SmoothStep(-40.0f,00.0f, breathClock);
					}
					else
					{
						neckController.RBHoldRotationSpring = Mathf.Lerp(25,80,interestValence/10.0f);
						if (mainInterest == "Face" && playerHeadToHead < kissingDistance+0.15f)// && lipsTouchCount > 0.0f)
						{
							neckController.jointRotationDriveSpring = 60;
							if (playerHeadToHead > kissingDistance - 0.03f && playerHeadToHead < kissingDistance + 0.05f)
							{
								kissingAngle = Mathf.Clamp(kissingAngle - 0.3f,-40.0f,00.0f);
							}
							else
							{
								kissingAngle = Mathf.Clamp(kissingAngle + 0.1f,-40.0f,00.0f);
							}
							neckController.jointRotationDriveXTarget = kissingAngle;
						}
						else
						{
							if (kissingAngle < 0.0f)
							{
								kissingAngle = Mathf.Clamp(kissingAngle + 0.3f,-40.0f,00.0f);
								neckController.jointRotationDriveXTarget = kissingAngle;
							}

							if (currentLook == "Sex" && vagTouchCount > 0.0f && mainInterest == "Pelvis" && interestValence > 9.5f)
							{
								kissingAngle = Mathf.Clamp(kissingAngle + 0.05f,0.0f,20.0f);
							}
							else
							{
								kissingAngle = Mathf.Clamp(kissingAngle - 0.2f,0.0f,20.0f);
							}
							neckController.jointRotationDriveXTarget = kissingAngle;
						}
					}
					neckController.RBHoldRotationDamper = Mathf.Lerp(35,85,interestArousal/10.0f);
				}
				
				if (uiDoChest.val)
				{
					//SuperController.LogError("Do Chest");
					chestController.jointRotationDriveSpring = 400;
					chestController.jointRotationDriveDamper = 135;
					chestController.jointRotationDriveXTarget = (Mathf.SmoothStep(0.0f,Mathf.Lerp(0.0f,5.0f,interestArousal/10.0f), breathClock) * uiBreatheExpandMultiplier.val) * uiChestAmount.val;
					if (currentLook == "Sucking")
					{
						chestController.jointRotationDriveXTarget = 20.0f - (40.0f * breathClock);
					}
					Vector3 relative;
					relative = chestController.followWhenOff.InverseTransformDirection(headController.followWhenOff.forward);

					chestController.jointRotationDriveYTarget = 0.0f;
					chestController.jointRotationDriveZTarget = 0.0f;
				}
				
				if (uiDoShoulders.val)
				{
					//SuperController.LogError("Do Shoulders");
					//lShoulderController.RBHoldRotationSpring = 10;
					//lShoulderController.RBHoldRotationDamper = 1;
					tempFloat = 13.0f + ((5.0f * shoulderUp) + Mathf.Lerp(0.0f,10.0f,interestArousal/10.0f) + (breathClock * Mathf.Lerp(1.0f * uiBreatheExpandMultiplier.val, 3.0f * uiBreatheExpandMultiplier.val,interestArousal/10.0f))) * uiShoulderAmount.val;;
					tempFloat2 = -1.0f + (5.0f * (breathClock * Mathf.Lerp(0.5f, 1.0f,interestArousal/10.0f))) + (pExtraversion/20.0f);
					if (personIsMale)
					{
						tempFloat = tempFloat / 10.0f;
						tempFloat2 = 0.0f;
					}
					lShoulderController.jointRotationDriveSpring = Mathf.Lerp(70,120,((interestArousal + interestValence)/2.0f) / 10.0f);
					lShoulderController.jointRotationDriveDamper = Mathf.Lerp(45.0f,20.0f,((interestArousal + interestValence)/2.0f) / 10.0f);
					lShoulderController.jointRotationDriveXTarget = tempFloat;
					lShoulderController.jointRotationDriveYTarget = tempFloat2;//(headController.followWhenOff.eulerAngles.y + Mathf.Lerp(10.0f,20.0f,interestValence/10.0f) + (breathClock * Mathf.Lerp(1.0f * uiBreatheExpandMultiplier.val, 2.5f * uiBreatheExpandMultiplier.val,interestArousal/10.0f))) * uiShoulderAmount.val;
					//lShoulderController.jointRotationDriveYTarget += -sexActionNeckX * 2.0f;
					lShoulderController.jointRotationDriveZTarget = Mathf.Lerp(-10f, 10.0f,interestArousal/10.0f);//((-6.0f + (breathClock * 16.0f)) * uiBreatheExpandMultiplier.val) * uiShoulderAmount.val;//headController.followWhenOff.eulerAngles.y;
					//lShoulderController.jointRotationDriveZTarget += -sexActionNeckX * 2.0f;
					//rShoulderController.RBHoldRotationSpring = 10;
					//rShoulderController.RBHoldRotationDamper = 1;
					rShoulderController.jointRotationDriveSpring = Mathf.Lerp(70,120,((interestArousal + interestValence)/2.0f) / 10.0f);
					rShoulderController.jointRotationDriveDamper = Mathf.Lerp(45.0f,20.0f,((interestArousal + interestValence)/2.0f) / 10.0f);
					rShoulderController.jointRotationDriveXTarget = -tempFloat;
					rShoulderController.jointRotationDriveYTarget = -tempFloat2; //(neckController.followWhenOff.eulerAngles.y + Mathf.Lerp(10.0f,20.0f,interestValence/10.0f) + (breathClock * Mathf.Lerp(1.0f * uiBreatheExpandMultiplier.val, 2.5f * uiBreatheExpandMultiplier.val,interestArousal/10.0f))) * uiShoulderAmount.val;
					//rShoulderController.jointRotationDriveYTarget += -sexActionNeckX * 2.0f;
					rShoulderController.jointRotationDriveZTarget = Mathf.Lerp(-10f, 10.0f,interestArousal/10.0f);//((-6.0f + (breathClock * 16.0f)) * uiBreatheExpandMultiplier.val) * uiShoulderAmount.val;
					//rShoulderController.jointRotationDriveZTarget += -sexActionNeckX * 2.0f;
				}
				
				/*float [,] armArray = new float[10,37];
				//arms up
				armArray[0,0] = 100.0f;
				armArray[0,1] = 10.0f;
				armArray[0,2] = 100.0f;
				armArray[0,3] = -80.0f;
				armArray[0,4] = 0.0f;
				armArray[0,5] = 20.0f;
				
				armArray[0,7] = 100.0f;
				armArray[0,8] = 10.0f;
				armArray[0,9] = 100.0f;
				armArray[0,10]= 80.0f;
				armArray[0,11]= 0.0f;
				armArray[0,12]= -20.0f;
				
				armArray[0,13]= 200.0f;
				armArray[0,14]= 10.0f;
				armArray[0,15]= 200.0f;
				armArray[0,16]= -150.0f;
				armArray[0,17]= 0.0f;
				armArray[0,18]= 0.0f;

				armArray[0,19]= 200.0f;
				armArray[0,20]= 10.0f;
				armArray[0,21]= 200.0f;
				armArray[0,22]= 150.0f;
				armArray[0,23]= 0.0f;
				armArray[0,24]= 0.0f;

				armArray[0,25]= 10.0f;
				armArray[0,26]= 4.0f;
				armArray[0,27]= 10.0f;
				armArray[0,28]= 35.0f;
				armArray[0,29]= 0.0f;
				armArray[0,30]= 40.0f;
				
				armArray[0,31]= 10.0f;
				armArray[0,32]= 4.0f;
				armArray[0,33]= 10.0f;
				armArray[0,34]= -35.0f;
				armArray[0,35]= 0.0f;
				armArray[0,36]= 40.0f;
				
				int tempInt = 0;

				lArmController.jointRotationDriveSpring = armArray[tempInt,0];
				lArmController.jointRotationDriveDamper = armArray[tempInt,1];
				lArmController.jointRotationDriveMaxForce = armArray[tempInt,2];
				lArmController.jointRotationDriveXTarget = armArray[tempInt,3];
				lArmController.jointRotationDriveYTarget = armArray[tempInt,4];
				lArmController.jointRotationDriveZTarget = armArray[tempInt,5];
				
				rArmController.jointRotationDriveSpring = armArray[tempInt,6];
				rArmController.jointRotationDriveDamper = armArray[tempInt,7];
				rArmController.jointRotationDriveMaxForce = armArray[tempInt,8];
				rArmController.jointRotationDriveXTarget = armArray[tempInt,9];
				rArmController.jointRotationDriveYTarget = armArray[tempInt,10];
				rArmController.jointRotationDriveZTarget = armArray[tempInt,12];
				
				lElbowController.jointRotationDriveSpring = armArray[tempInt,13];
				lElbowController.jointRotationDriveDamper = armArray[tempInt,14];
				lElbowController.jointRotationDriveMaxForce = armArray[tempInt,15];
				lElbowController.jointRotationDriveXTarget = armArray[tempInt,16];
				lElbowController.jointRotationDriveYTarget = armArray[tempInt,17];
				lElbowController.jointRotationDriveZTarget = armArray[tempInt,18];
				
				rElbowController.jointRotationDriveSpring = armArray[tempInt,19];
				rElbowController.jointRotationDriveDamper = armArray[tempInt,20];
				rElbowController.jointRotationDriveMaxForce = armArray[tempInt,21];
				rElbowController.jointRotationDriveXTarget = armArray[tempInt,22];
				rElbowController.jointRotationDriveYTarget = armArray[tempInt,23];
				rElbowController.jointRotationDriveZTarget = armArray[tempInt,24];
				
				lHandController.jointRotationDriveSpring = armArray[tempInt,25];
				lHandController.jointRotationDriveDamper = armArray[tempInt,26];
				lHandController.jointRotationDriveMaxForce = armArray[tempInt,27];
				lHandController.jointRotationDriveXTarget = armArray[tempInt,28];
				lHandController.jointRotationDriveYTarget = armArray[tempInt,29];
				lHandController.jointRotationDriveZTarget = armArray[tempInt,30];
				
				rHandController.jointRotationDriveSpring = armArray[tempInt,31];
				rHandController.jointRotationDriveDamper = armArray[tempInt,32];
				rHandController.jointRotationDriveMaxForce = armArray[tempInt,33];
				rHandController.jointRotationDriveXTarget = armArray[tempInt,34];
				rHandController.jointRotationDriveYTarget = armArray[tempInt,35];
				rHandController.jointRotationDriveZTarget = armArray[tempInt,36];*/
			}
			
			
			//playerFace = player.position;
			if (usePerson2 && person2Usable)
			{
				playerHeadTransform = playerHeadController.followWhenOff;
				playerLHand = playerLHandController.followWhenOff.position;
				playerRHand = playerRHandController.followWhenOff.position;
				playerLHandTransform = playerLHandController.followWhenOff;
				playerRHandTransform = playerRHandController.followWhenOff;
			}
			else
			{
				playerHeadTransform = CameraTarget.centerTarget.transform;
				if (playerHandsUsable && usePerson2 == false)
				{
					playerLHand = playerVRLHand.position;
					playerRHand = playerVRRHand.position;
					playerLHandTransform = playerVRLHand;
					playerRHandTransform = playerVRRHand;
				}
				if (person2Usable == false && playerHandsUsable == false)
				{
					playerLHand = playerHeadTransform.position;
					playerRHand = playerHeadTransform.position;
					playerLHandTransform = playerHeadTransform;
					playerRHandTransform = playerHeadTransform;
				}
			}


			
			//emotionSM.OnUpdate();
            lookSM.OnUpdate();
            systemSM.OnUpdate();
            browSM.OnUpdate();
            mouthSM.OnUpdate();
            eyesSM.OnUpdate();
			//SuperController.LogError("State Machines Updated");
            string dbgHead = "";
            string dbgLHand = "";
            string dbgRHand = "";
            string dbgPenis = "";
            string dbgObject = "";
			
			if (aCubeController != null)
			{
            aCubeController.transform.position = lookAtPosition;
			}
            //neckController.jointRotationDriveSpring = 123.45f;
            //debugUIControl.SetValue = "We have Control";
            if (lastBrowState != browSM.CurrentState && browSM.CurrentState != null)
            {
                lastBrowState = browSM.CurrentState;
            }
            if (lastMouthState != mouthSM.CurrentState && mouthSM.CurrentState != null)
            {
                lastMouthState = mouthSM.CurrentState;
            }
            if (lastEyesState != eyesSM.CurrentState && eyesSM.CurrentState != null)
            {
                lastEyesState = eyesSM.CurrentState;
            }
            if (lastLookState != lookSM.CurrentState && lookSM.CurrentState != null)
            {
                lastLookState = lookSM.CurrentState;
            }
			//SuperController.LogError("Previous States Captured");
			
			
            interestArousal = Mathf.Clamp(interestArousal, 2.0f, 10.0f);
            interestValence = Mathf.Clamp(interestValence, 1.0f, 10.0f);
			if (currentLook == "Kissing" || currentLook == "Sucking" || currentLook == "Sex")
			{
				interestArousal += 0.1f;
				interestValence += 0.05f;
			}

			if (playerHeadToHead < personalSpaceDistance || playerLHandToHead < personalSpaceDistance || playerRHandToHead < personalSpaceDistance)
			{
				interestValence = Mathf.Clamp(interestValence, 3.5f, 10.0f);
			}
            breatheInSpeed = Mathf.Lerp(1.0f, 1.7f, interestArousal/10.0f) * uiBreatheSpeed.val;
            breatheOutSpeed = breatheInSpeed * Mathf.Lerp(0.5f,0.8f,interestArousal/10.0f);
            //breatheInSpeed = Mathf.Lerp(1.0f, 1.3f, interestArousal/10.0f) * uiBreatheSpeed.val;
            //breatheOutSpeed = breatheInSpeed * Mathf.Lerp(0.7f,1.5f,interestArousal/10.0f);
            if (interestKissing || playerTipToHead < interactionDistance || lipsTouchCount > 1.0f)
            {
                breatheInSpeed = Mathf.Lerp(1.5f, 2.0f,interestArousal / 10.0f) * uiBreatheSpeed.val;
                breatheOutSpeed = Mathf.Lerp(0.4f,1.5f,interestValence / 10.0f) * uiBreatheSpeed.val;
            }
            //breathing
            float breathingRate = (Random.Range(0.1f, 0.35f) + (interestArousal / 15.0f)) * uiBreatheSpeed.val;
            if (breathState == "in" && breathHold <= 0.0f && (lipsTouchCount < 1.0f || (lipsTouchCount > 0.0f && playerTipToHead > 0.077f)))
            {
                breathClock = Mathf.Min(breathClock + (Time.fixedDeltaTime * (breathingRate * breatheInSpeed)), 1.0f);
                if (breathClock == 1.0f)
                {
                    breathState = "out";
					breathHold = Mathf.Lerp(0.1f,0.03f,interestArousal/10.0f) * uiBreatheSpeed.val;
                }
            }
            if (breathState == "out" && breathHold <= 0.0f)
            {
                breathClock = Mathf.Max(breathClock - (Time.fixedDeltaTime * (breathingRate * breatheOutSpeed)), 0.0f);
                if (breathClock == 0.0f)
                {
                    breathState = "in";
                    breathCount += 1.0f;
					breathHold = Mathf.Lerp(0.3f,0.00f,Mathf.Clamp(interestArousal+3.0f,0.0f,10.0f)/10.0f) * uiBreatheSpeed.val;
                }
            }
			if (breathHold > 0.0f)
			{
				breathHold -= Time.fixedDeltaTime;
			}

			
			
			if (uiDoMorphs.val)
			{
				tempFloat = Mathf.Lerp(0.5f,1.0f,interestArousal/10.0f);
				if (morphRibCageSize != null)
				{
					morphRibCageSize.SetValue(Mathf.SmoothStep(0.0f, 0.23f * uiBreatheExpandMultiplier.val * tempFloat,breathClock));
				}
				if (morphChestHeight != null)
				{
					morphChestHeight.SetValue(Mathf.SmoothStep(mChestHeightOrig, mChestHeightOrig-(0.22f * uiBreatheRaiseMultiplier.val * tempFloat),breathClock));//Mathf.Min((breathClock * -0.365f) + 0.08f, 0.0f));
				}
				if (morphBreastHeight != null)
				{
					if (personIsMale)
					{
						morphBreastHeight.SetValue(Mathf.SmoothStep(0.0f, 0.5f * uiBreatheRaiseMultiplier.val * tempFloat,breathClock));//Mathf.Min((breathClock * -0.365f) + 0.08f, 0.0f));
					}
					else
					{
						morphBreastHeight.SetValue(Mathf.SmoothStep(0.0f, 1.0f * uiBreatheRaiseMultiplier.val * tempFloat,breathClock));//Mathf.Min((breathClock * -0.365f) + 0.08f, 0.0f));
					}
				}
				if (morphBreath != null)
				{
					morphBreath.SetValue(Mathf.SmoothStep(0.0f, Mathf.Clamp(1.0f - (0.5f * uiBreatheExpandMultiplier.val * tempFloat),0.0f,1.0f),breathClock));//Mathf.Min((breathClock * -0.365f) + 0.08f, 0.0f));
				}
				else
				{
					//morphRibsDef.SetValue(0.3f + (breathClock*0.7f) * 1.22f * uiBreatheExpandMultiplier.val * tempFloat);//Mathf.Min((breathClock * -0.365f) + 0.08f, 0.0f));
				}
				//morphRibsDef.SetValue(0.3f + (breathClock*0.7f) * 1.22f * uiBreatheExpandMultiplier.val * tempFloat);//Mathf.Min((breathClock * -0.365f) + 0.08f, 0.0f));
				if (morphSternumDepth != null)
				{
					morphSternumDepth.SetValue(Mathf.SmoothStep(0.0f, 0.21f * uiBreatheExpandMultiplier.val * tempFloat,breathClock));
				}
				if (morphNipplesApply != null)
				{
					morphNipplesApply.SetValue(interestArousal/13.0f);
				}
				
				
				//SuperController.LogError("Breathing Morphs done");
				if (morphDeepBulgeBellyBottom != null && morphDeepBulgeBellyMid != null && usePerson2 && uiDoSex.val)
				{
					tempFloat = Vector3.Distance(pelvisController.followWhenOff.position, playerTipController.followWhenOff.position);
					if ( tempFloat < 0.15f)
					{
						morphDeepBulgeBellyBottom.SetValue(Mathf.Clamp((1.0f - (tempFloat*6.666f)) * uiSexAmount.val,0.0f,1.0f));
					}
					if ( tempFloat < 0.065f)
					{
						morphDeepBulgeBellyMid.SetValue(Mathf.Clamp((1.0f - (tempFloat*15.384f)) * uiSexAmount.val,0.0f,1.0f) * 0.75f);
					}
				}
				
				
				if (morphDeepThroat != null && usePerson2 && uiDoBlowjob.val)
				{
					tempFloat = Vector3.Distance(headController.followWhenOff.position, playerTipController.followWhenOff.position);
					if ( tempFloat < 0.077f)
					{
						morphDeepThroat.SetValue(Mathf.Clamp((1.0f - (tempFloat*12.987f)) * uiBlowjobAmount.val,0.0f,1.0f));
					}
				}
				
				if (morphBlowjobLips != null && morphCheekSink != null && usePerson2 && playerTipToHead < playerHeadToHead && playerTipToHead < personalSpaceDistance/2.0f)
				{
					if (lipsTouchCount > 0.0f && uiDoBlowjob.val)
					{
						if (Mathf.Abs(Vector3.Angle(playerTipController.followWhenOff.position, headController.followWhenOff.position)) < lookDirectAngle)
						{
							mMouthOpenWiderTarget = 0.34f;
						}
						else
						{
							mMouthOpenWiderTarget = 0.0f;
						}
						if (mainInterest == "Tip")
						{
							mainInterest = "Pelvis";
						}
						tempFloat = Mathf.Clamp(Vector3.Distance(playerTipController.followWhenOff.position,playerTipPrev) * 9.0f,0.0f,1.0f);
						tempFloat2 = 0.0f - Mathf.Clamp(Vector3.Distance(headController.followWhenOff.position,headPrevPos) * 9.0f,0.0f,1.0f);
						if (tempFloat2 > tempFloat)
						{
							tempFloat = tempFloat2;
						}
						if (Vector3.Distance(personHeadTransform.position, playerTipController.followWhenOff.position) >= Vector3.Distance(personHeadTransform.position, playerTipPrev))// && Mathf.Abs(tempFloat) > 0.002f)
						{
							mBlowjobLipsTarget = Mathf.Clamp(mBlowjobLipsTarget + (tempFloat * uiBlowjobAmount.val),-0.2f,1.0f);
							mCheekSinkTarget = Mathf.Clamp(mBlowjobLipsTarget + 0.3f,0.0f,1.0f);
							if (mEyesClosedLeftValue < 0.0f && morphBlinking == false)
							{
								mEyesClosedLeftTarget = Mathf.Clamp(mEyesClosedLeftTarget + 0.1f,-0.2f,0.0f);
								mEyesClosedRightTarget = Mathf.Clamp(mEyesClosedRightTarget + 0.1f,-0.2f,0.0f);
							}
							mBrowUpTarget = Mathf.Clamp(mBrowUpTarget - 0.1f,0.0f,1.0f);
						}
						else
						{
							mBlowjobLipsTarget = Mathf.Clamp(mBlowjobLipsTarget - (tempFloat * uiBlowjobAmount.val * 2.0f),-0.2f,1.0f);
							mCheekSinkTarget = mBlowjobLipsTarget;
							mBrowUpTarget = Mathf.Clamp(mBrowUpTarget + 0.1f,0.0f,1.0f);
							if (mEyesClosedLeftValue < 0.3f && morphBlinking == false)
							{
								mEyesClosedLeftTarget = Mathf.Clamp(mEyesClosedLeftTarget - 0.2f,-0.5f,1.0f);
								mEyesClosedRightTarget = Mathf.Clamp(mEyesClosedRightTarget - 0.2f,-0.5f,1.0f);
							}
						}
					}
					else
					{
						mMouthOpenWiderTarget = 0.0f;
						mBlowjobLipsTarget = Mathf.Clamp(mBlowjobLipsTarget - 0.1f,0.0f,1.0f);
						mCheekSinkTarget = Mathf.Clamp(mCheekSinkTarget - 0.1f,0.0f,1.0f);
					}
					if (mBlowjobLipsTarget > mBlowjobLipsValue + 0.01f) { mBlowjobLipsValue = Mathf.Min(mBlowjobLipsValue + 0.3f, mBlowjobLipsTarget); }
					if (mBlowjobLipsTarget < mBlowjobLipsValue - 0.01f) { mBlowjobLipsValue = Mathf.Max(mBlowjobLipsValue - 0.3f, mBlowjobLipsTarget); }
					morphBlowjobLips.SetValue(Mathf.Clamp(mBlowjobLipsValue,0.0f,0.8f));
					if (mCheekSinkTarget > mCheekSinkValue + 0.01f) { mCheekSinkValue = Mathf.Min(mCheekSinkValue + 0.2f, mCheekSinkTarget); }
					if (mCheekSinkTarget < mCheekSinkValue - 0.01f) { mCheekSinkValue = Mathf.Max(mCheekSinkValue - 0.1f, mCheekSinkTarget); }
					morphCheekSink.SetValue(Mathf.Clamp(mCheekSinkValue*2.0f,0.0f,1.0f));
					playerTipPrev = playerTipController.followWhenOff.position;
					headPrevPos = headController.followWhenOff.position;
				}
				//SuperController.LogError("Sex Morphs Done");
				
				//Morph Controller
				if (doHands)
				{
					if (mLHandStraightenTarget > mLHandStraightenValue) { mLHandStraightenValue = Mathf.Min(mLHandStraightenValue + (mLHandStraightenTarget - mLHandStraightenValue) / 15.0f, mLHandStraightenTarget); }
					if (mLHandStraightenTarget < mLHandStraightenValue) { mLHandStraightenValue = Mathf.Max(mLHandStraightenValue - (mLHandStraightenValue - mLHandStraightenTarget) / 15.0f, mLHandStraightenTarget); }
					morphLHandStraighten.SetValue(Mathf.Clamp(mLHandStraightenValue + (interestValence / 100.0f), 0.0f, 1.0f));
					if (mRHandStraightenTarget > mRHandStraightenValue) { mRHandStraightenValue = Mathf.Min(mRHandStraightenValue + (mRHandStraightenTarget - mRHandStraightenValue) / 15.0f, mRHandStraightenTarget); }
					if (mRHandStraightenTarget < mRHandStraightenValue) { mRHandStraightenValue = Mathf.Max(mRHandStraightenValue - (mRHandStraightenValue - mRHandStraightenTarget) / 15.0f, mRHandStraightenTarget); }
					morphRHandStraighten.SetValue(Mathf.Clamp(mRHandStraightenValue + (interestValence / 100.0f), 0.0f, 1.0f));
					if (mLHandFistTarget > mLHandFistValue) { mLHandFistValue = Mathf.Min(mLHandFistValue + (mLHandFistTarget - mLHandFistValue) / 15.0f, mLHandFistTarget); }
					if (mLHandFistTarget < mLHandFistValue) { mLHandFistValue = Mathf.Max(mLHandFistValue - (mLHandFistValue - mLHandFistTarget) / 15.0f, mLHandFistTarget); }
					morphLHandFist.SetValue(Mathf.Clamp(mLHandFistValue + (interestArousal / 100.0f), 0.0f, 1.2f));
					if (mRHandFistTarget > mRHandFistValue) { mRHandFistValue = Mathf.Min(mRHandFistValue + (mRHandFistTarget - mRHandFistValue) / 15.0f, mRHandFistTarget); }
					if (mRHandFistTarget < mRHandFistValue) { mRHandFistValue = Mathf.Max(mRHandFistValue - (mRHandFistValue - mRHandFistTarget) / 15.0f, mRHandFistTarget); }
					morphRHandFist.SetValue(Mathf.Clamp(mRHandFistValue + (interestArousal / 100.0f), 0.0f, 1.2f));
					//SuperController.LogError("Hand Morphs Done");
				}

				if (mBrowUpTarget + (interestValence / 30.0f) > mBrowUpValue + 0.2f) { mBrowUpValue = Mathf.Min(mBrowUpValue + (0.006f * browVariation), mBrowUpTarget + (interestValence / 30.0f)); }
				if (mBrowUpTarget + (interestValence / 30.0f) < mBrowUpValue - 0.2f) { mBrowUpValue = Mathf.Max(mBrowUpValue - (0.006f * browVariation), mBrowUpTarget + (interestValence / 30.0f)); }
				morphBrowUp.SetValue(Mathf.SmoothStep(0.0f,1.0f,Round(Mathf.Clamp(mBrowUpValue, 0.0f, 1.0f))));
				
				if (mBrowDownTarget > mBrowDownValue + 0.02f) { mBrowDownValue = Mathf.Min(mBrowDownValue + (0.006f * browVariation), mBrowDownTarget); }
				if (mBrowDownTarget < mBrowDownValue - 0.02f) { mBrowDownValue = Mathf.Max(mBrowDownValue - (0.006f * browVariation), mBrowDownTarget); }
				morphBrowDown.SetValue(Mathf.SmoothStep(0.0f,1.0f,Round(mBrowDownValue)));
				if (mExcitementTarget > mExcitementValue + 0.1f) { mExcitementValue = Mathf.Min(mExcitementValue + (0.006f * browVariation), mExcitementTarget); }
				if (mExcitementTarget < mExcitementValue - 0.1f) { mExcitementValue = Mathf.Max(mExcitementValue - (0.008f * browVariation), mExcitementTarget); }
				morphExpExcitement.SetValue(Round(mExcitementValue));
				if (mBrowOuterUpLeftTarget > mBrowOuterUpLeftValue + 0.2f) { mBrowOuterUpLeftValue = Mathf.Min(mBrowOuterUpLeftValue + (0.03f * browVariation), mBrowOuterUpLeftTarget); }
				if (mBrowOuterUpLeftTarget < mBrowOuterUpLeftValue - 0.2f) { mBrowOuterUpLeftValue = Mathf.Max(mBrowOuterUpLeftValue - (0.012f * browVariation), mBrowOuterUpLeftTarget); }
				morphBrowOuterUpLeft.SetValue(Round(mBrowOuterUpLeftValue));
				if (mBrowOuterUpRightTarget > mBrowOuterUpRightValue + 0.2f) { mBrowOuterUpRightValue = Mathf.Min(mBrowOuterUpRightValue + (0.03f * browVariation), mBrowOuterUpRightTarget); }
				if (mBrowOuterUpRightTarget < mBrowOuterUpRightValue - 0.2f) { mBrowOuterUpRightValue = Mathf.Max(mBrowOuterUpRightValue - (0.012f * browVariation), mBrowOuterUpRightTarget); }
				morphBrowOuterUpRight.SetValue(Round(mBrowOuterUpRightValue));
				if (mBrowCenterUpTarget + (interestArousal / 20.0f) > mBrowCenterUpValue + 0.1) { mBrowCenterUpValue = Mathf.Min(mBrowCenterUpValue + (0.004f * browVariation), mBrowCenterUpTarget + (interestArousal / 20.0f)); }
				if (mBrowCenterUpTarget + (interestArousal / 20.0f) < mBrowCenterUpValue - 0.1) { mBrowCenterUpValue = Mathf.Max(mBrowCenterUpValue - (0.009f * browVariation), mBrowCenterUpTarget + (interestArousal / 20.0f)); }
				morphBrowCenterUp.SetValue(Round(Mathf.Clamp(mBrowCenterUpValue, -0.3f, 1.3f)));
				//SuperController.LogError("Brow Morphs Done");

				float lidLower = 0.25f;
				float lidRaise = 0.4f;
				tempFloat = Random.Range(-0.05f, 0.05f);
				tempFloat = Mathf.Clamp(1.25f - Mathf.Lerp(0.0f,0.1f,mEyesSquintValue),0.0f, eyeCloseMaxMorph);
				tempFloat2 = 0.0f;
				if (amGlancing)
				{
					tempFloat2 = -0.1f;
				}
				
				if (morphBlinking)
				{
					lidLower = 0.53f + Random.Range(-0.05f, 0.05f);;
					lidRaise = 0.1f + Random.Range(-0.05f, 0.05f);;
					if (mEyesClosedLeftTarget > mEyesClosedLeftValue) { mEyesClosedLeftValue = Mathf.Min(mEyesClosedLeftValue + lidLower, mEyesClosedLeftTarget); }
					if (mEyesClosedLeftTarget < mEyesClosedLeftValue) { mEyesClosedLeftValue = Mathf.Max(mEyesClosedLeftValue - lidRaise, mEyesClosedLeftTarget); }
					morphEyesClosedLeft.SetValue(Round(Mathf.Clamp(mEyesClosedLeftValue, -0.2f, tempFloat)));
					if (mEyesClosedRightTarget > mEyesClosedRightValue) { mEyesClosedRightValue = Mathf.Min(mEyesClosedRightValue + lidLower, mEyesClosedRightTarget); }
					if (mEyesClosedRightTarget < mEyesClosedRightValue) { mEyesClosedRightValue = Mathf.Max(mEyesClosedRightValue - lidRaise, mEyesClosedRightTarget); }
					morphEyesClosedRight.SetValue(Round(Mathf.Clamp(mEyesClosedRightValue, -0.2f, tempFloat)));
				}
				else
				{
					if (mEyesClosedLeftTarget + tempFloat2 > mEyesClosedLeftValue + 0.005f) { mEyesClosedLeftValue = Mathf.Min(mEyesClosedLeftValue + (lidLower * eyeVariation), mEyesClosedLeftTarget); }
					if (mEyesClosedLeftTarget + tempFloat2 < mEyesClosedLeftValue - 0.005f) { mEyesClosedLeftValue = Mathf.Max(mEyesClosedLeftValue - (lidRaise * eyeVariation), mEyesClosedLeftTarget); }
					morphEyesClosedLeft.SetValue(Mathf.SmoothStep(0.0f,1.0f,Round(Mathf.Clamp(mEyesClosedLeftValue, -0.5f, tempFloat))));
					if (mEyesClosedRightTarget + tempFloat2 > mEyesClosedRightValue + 0.005f) { mEyesClosedRightValue = Mathf.Min(mEyesClosedRightValue + (lidLower * eyeVariation), mEyesClosedRightTarget); }
					if (mEyesClosedRightTarget + tempFloat2 < mEyesClosedRightValue - 0.005f) { mEyesClosedRightValue = Mathf.Max(mEyesClosedRightValue - (lidRaise * eyeVariation), mEyesClosedRightTarget); }
					morphEyesClosedRight.SetValue(Mathf.SmoothStep(0.0f,1.0f,Round(Mathf.Clamp(mEyesClosedRightValue, -0.5f, tempFloat))));
				}

				if (morphBlinking && mEyesClosedRightValue <= 0.1f && mEyesClosedLeftValue <= 0.1f)
				{
					morphBlinking = false;
				}

				if (mEyesSquintTarget > mEyesSquintValue + 0.01) { mEyesSquintValue = Mathf.Min(mEyesSquintValue + (0.04f * eyeVariation), mEyesSquintTarget); }
				if (mEyesSquintTarget < mEyesSquintValue - 0.01) { mEyesSquintValue = Mathf.Max(mEyesSquintValue - (0.01f * eyeVariation), mEyesSquintTarget); }
				morphEyesSquint.SetValue(Round(Mathf.Clamp(mEyesSquintValue - mEyesClosedLeftValue, -0.2f, 1.0f)));

				tempFloat = Mathf.Lerp(0.15f,0.25f, interestArousal / 10.0f) * (breathClock) * uiBreatheExpandMultiplier.val;
				mNoseFlareTarget = 0.2f;
				if (breathState == "in")
				{
					tempFloat = (((interestArousal / 20.0f) * Mathf.Lerp(1.0f,0.0f,breathClock)) + Mathf.Lerp(0.0f,0.2f,((interestArousal+interestValence)/2.0f)/10.0f)) * uiBreatheExpandMultiplier.val;
					mNoseFlareTarget = -0.5f;
				}
				//tempFloat = Mathf.Clamp(tempFloat + (0.2f * (interestArousal/10.0f)),0.0f,1.0f) * uiBreatheExpandMultiplier.val;
				//SuperController.LogError("Eye Morphs Done");
				tempFloat2 = mouthVariation;
				if (mainInterest != "Face")
				{
					//mMouthOpenWideTarget = (Mathf.Clamp(interestArousal-6.0f,0.0f,10.0f)/10.0f)/2.0f;
				}
				else
				{
					//mMouthOpenWideTarget = 0.0f;
				}
				if (currentMouth == "Smile" || currentMouth == "Big Smile" || currentMouth == "Kissing" || currentMouth == "Sideways" || currentMouth == "Pout")
				{
					tempFloat = Mathf.Lerp(0.0f,Mathf.Lerp(0.02f, 0.10f,interestArousal/10.0f),(breathClock)) * uiBreatheExpandMultiplier.val;
					tempFloat2 = 0.25f;
					mNoseFlareTarget = Mathf.Clamp(mNoseFlareTarget * 5.0f,0.0f,0.5f);
					mMouthOpenWideTarget = 0.0f;
				}
				
				if (usePerson2 && person2Usable && currentLook == "Sex")
				{
					tempFloat = Mathf.Clamp(Vector3.Distance(playerTipController.followWhenOff.position,playerTipPrev) * 7.0f,0.0f,1.0f);
					//mMouthOpenWideTarget = Mathf.Clamp(mMouthOpenWideTarget + (tempFloat / 1.0f),0.0f,1.0f);
				}

				
				if (morphMouthMouthOpen != null)
				{
					if (mMouthOpenTarget + tempFloat > mMouthOpenValue + 0.2f) { mMouthOpenValue = Mathf.Min(mMouthOpenValue + (0.01f * tempFloat2), mMouthOpenTarget + tempFloat); }
					if (mMouthOpenTarget + tempFloat < mMouthOpenValue - 0.2f) { mMouthOpenValue = Mathf.Max(mMouthOpenValue - (0.05f * mouthVariation), mMouthOpenTarget + tempFloat); }
					if (currentMouth != "Big Smile" && currentMouth != "Sideways" && currentMouth != "Smile")// && currentMouth != "Closed")
					{
						morphMouthMouthOpen.SetValue(Mathf.SmoothStep(0.0f,1.0f,Round(Mathf.Clamp(mMouthOpenValue, 0.0f, 1.0f))));
					}
					else
					{
						morphMouthMouthOpen.SetValue(Mathf.SmoothStep(0.0f,1.0f,Round(Mathf.Clamp(mMouthOpenValue, 0.0f, 0.5f))));
					}
				}
				if (morphNoseFlare != null)
				{
					if (mNoseFlareTarget > mNoseFlareValue + 0.01f) { mNoseFlareValue = Mathf.Min(mNoseFlareValue + (0.05f * tempFloat2), mNoseFlareTarget); }
					if (mNoseFlareTarget < mNoseFlareValue - 0.01f) { mNoseFlareValue = Mathf.Max(mNoseFlareValue - (0.005f * mouthVariation), mNoseFlareTarget); }
					morphNoseFlare.SetValue(Round(Mathf.Clamp(mNoseFlareValue, -1.0f, 1.0f)));
				}
				if (morphMouthMouthOpenWide != null && morphMouthNarrow != null)
				{
					if (mMouthOpenWideTarget > mMouthOpenWideValue + 0.01f) { mMouthOpenWideValue = Mathf.Min(mMouthOpenWideValue + (0.002f), mMouthOpenWideTarget); }
					if (mMouthOpenWideTarget < mMouthOpenWideValue - 0.01f) { mMouthOpenWideValue = Mathf.Max(mMouthOpenWideValue - (0.0005f), mMouthOpenWideTarget); }
					morphMouthMouthOpenWide.SetValue(Mathf.SmoothStep(0.0f,1.0f,Round(mMouthOpenWideValue)));
					if (currentMouth == "Open")
					{
						mMouthNarrowTarget = Mathf.Clamp((mMouthOpenWideValue* 0.55f),0.0f,1.0f);
					}
					else
					{
						mMouthNarrowTarget = Mathf.Clamp((mMouthOpenWideValue* 0.55f) + (mMouthOpenValue * 0.55f),0.0f,1.0f);
					}
					if (mMouthNarrowTarget > mMouthNarrowValue + 0.01f) { mMouthNarrowValue = Mathf.Min(mMouthNarrowValue + (0.002f), mMouthNarrowTarget); }
					if (mMouthNarrowTarget < mMouthNarrowValue - 0.01f) { mMouthNarrowValue = Mathf.Max(mMouthNarrowValue - (0.0005f), mMouthNarrowTarget); }
					morphMouthNarrow.SetValue(Mathf.SmoothStep(0.0f,1.0f,Round(Mathf.Clamp(mMouthNarrowValue,0.0f,1.0f))));
				}
				if (morphMouthOpenWider != null && morphLipsLipsClose != null && usePerson2 && person2Usable)
				{
					if ((mainInterest == "Tip" || mainInterest == "Pelvis") && Vector3.Distance(playerTipController.followWhenOff.position,headController.followWhenOff.position) < closeFaceDistance * 1.3f && uiDoBlowjob.val)
					{
						mMouthOpenWiderTarget = 0.44f;
						mLipsCloseTarget = 1.1f;
					}
					else
					{
						mMouthOpenWiderTarget = 0.0f;
						if (currentLook != "Kissing")
						{
							mLipsCloseTarget = 0.0f;
						}
					}
					if (lipsTouchCount <= 1.0f)
					{
						mLipsCloseTarget = 0.0f;
					}					
					if (mMouthOpenWiderTarget > mMouthOpenWiderValue + 0.01f) { mMouthOpenWiderValue = Mathf.Min(mMouthOpenWiderValue + (0.02f), mMouthOpenWiderTarget); }
					if (mMouthOpenWiderTarget < mMouthOpenWiderValue - 0.01f) { mMouthOpenWiderValue = Mathf.Max(mMouthOpenWiderValue - (0.005f), mMouthOpenWiderTarget); }
					morphMouthOpenWider.SetValue(Mathf.SmoothStep(0.0f,1.0f,Round(mMouthOpenWiderValue)));
					if (mLipsCloseTarget > mLipsCloseValue + 0.01f) { mLipsCloseValue = Mathf.Min(mLipsCloseValue + (0.02f), mLipsCloseTarget); }
					if (mLipsCloseTarget < mLipsCloseValue - 0.01f) { mLipsCloseValue = Mathf.Max(mLipsCloseValue - (0.005f), mLipsCloseTarget); }
					morphLipsLipsClose.SetValue(Round(mLipsCloseValue));
				}
				if (morphLipsLipsPucker != null && morphLipsLipsPuckerWide != null)
				{
					if (mLipsPuckerTarget - tempFloat * 0.2f > mLipsPuckerValue + 0.2f) { mLipsPuckerValue = Mathf.Min(mLipsPuckerValue + (0.05f * mouthVariation), mLipsPuckerTarget - tempFloat * 0.2f); }
					if (mLipsPuckerTarget - tempFloat * 0.2f < mLipsPuckerValue - 0.2f) { mLipsPuckerValue = Mathf.Max(mLipsPuckerValue - (0.04f * mouthVariation), mLipsPuckerTarget - tempFloat * 0.2f); }
					morphLipsLipsPucker.SetValue(Mathf.SmoothStep(0.0f,1.0f,Round(mLipsPuckerValue)));
					if (mLipsPuckerWideTarget > mLipsPuckerWideValue + 0.1f) { mLipsPuckerWideValue = Mathf.Min(mLipsPuckerWideValue + (0.01f * mouthVariation), mLipsPuckerWideTarget); }
					if (mLipsPuckerWideTarget < mLipsPuckerWideValue - 0.1f) { mLipsPuckerWideValue = Mathf.Max(mLipsPuckerWideValue - (0.007f * mouthVariation), mLipsPuckerWideTarget); }
					morphLipsLipsPuckerWide.SetValue(Mathf.SmoothStep(0.0f,1.0f,Round(mLipsPuckerWideValue)));
				}
				if (morphExpFlirting != null)
				{
					if (mFlirtingTarget > mFlirtingValue + 0.01f) { mFlirtingValue = Mathf.Min(mFlirtingValue + (0.01f * mouthVariation), mFlirtingTarget); }
					if (mFlirtingTarget < mFlirtingValue - 0.01f) { mFlirtingValue = Mathf.Max(mFlirtingValue - (0.001f * mouthVariation), mFlirtingTarget); }
					morphExpFlirting.SetValue(Round(mFlirtingValue));
				}
				if (morphExpDeserveIt != null)
				{
					if (mDeserveItTarget > mDeserveItValue + 0.01f) { mDeserveItValue = Mathf.Min(mDeserveItValue + (0.05f * mouthVariation), mDeserveItTarget); }
					if (mDeserveItTarget < mDeserveItValue - 0.01f) { mDeserveItValue = Mathf.Max(mDeserveItValue - (0.02f * mouthVariation), mDeserveItTarget); }
					morphExpDeserveIt.SetValue(Round(mDeserveItValue));
				}
				if (morphExpTakingIt != null)
				{
					if (mTakingItTarget > mTakingItValue + 0.01f) { mTakingItValue = Mathf.Min(mTakingItValue + (0.05f * mouthVariation), mTakingItTarget); }
					if (mTakingItTarget < mTakingItValue - 0.01f) { mTakingItValue = Mathf.Max(mTakingItValue - (0.02f * mouthVariation), mTakingItTarget); }
					morphExpTakingIt.SetValue(Round(mTakingItValue));
				}
				if (morphExpHappy != null)
				{
					if (mHappyTarget > mHappyValue + 0.02f) { mHappyValue = Mathf.Min(mHappyValue + (0.0035f * mouthVariation), mHappyTarget); }
					if (mHappyTarget < mHappyValue - 0.02f) { mHappyValue = Mathf.Max(mHappyValue - (0.0011f * mouthVariation), mHappyTarget); }
					morphExpHappy.SetValue(Round(mHappyValue));
				}
				if (morphExpSmileFullFace != null && morphExpSmileOpenFullFace != null)
				{
					if (Mathf.Clamp((interestValence / 30.0f) + mSmileFullFaceTarget, 0.0f, interestMaxSmile) > mSmileFullFaceValue + 0.1f) { mSmileFullFaceValue = Mathf.Min(mSmileFullFaceValue + (0.0054f * mouthVariation), Mathf.Clamp((interestValence / 30.0f) + mSmileFullFaceTarget, 0.0f, interestMaxSmile)); }
					if (Mathf.Clamp((interestValence / 30.0f) + mSmileFullFaceTarget, 0.0f, interestMaxSmile) < mSmileFullFaceValue - 0.1f) { mSmileFullFaceValue = Mathf.Max(mSmileFullFaceValue - (0.003f * mouthVariation), Mathf.Clamp((interestValence / 30.0f) + mSmileFullFaceTarget, 0.0f, interestMaxSmile)); }
					morphExpSmileFullFace.SetValue((Round(mSmileFullFaceValue)));
					if (mSmileOpenFullFaceTarget > mSmileOpenFullFaceValue + 0.1f) { mSmileOpenFullFaceValue = Mathf.Min(mSmileOpenFullFaceValue + (0.0025f * mouthVariation), mSmileOpenFullFaceTarget); }
					if (mSmileOpenFullFaceTarget < mSmileOpenFullFaceValue - 0.1f) { mSmileOpenFullFaceValue = Mathf.Max(mSmileOpenFullFaceValue - (0.0002f * mouthVariation), mSmileOpenFullFaceTarget); }
					morphExpSmileOpenFullFace.SetValue(Mathf.SmoothStep(0.0f,1.0f,Round(mSmileOpenFullFaceValue)));
				}
				if (morphVisF != null)
				{
					if (mVisFTarget > mVisFValue + 0.03f) { mVisFValue = Mathf.Min(mVisFValue + (0.0003f * mouthVariation), mVisFTarget); }
					if (mVisFTarget < mVisFValue - 0.03f) { mVisFValue = Mathf.Max(mVisFValue - (0.0001f * mouthVariation), mVisFTarget); }
					morphVisF.SetValue(Round(mVisFValue));
				}

				if (morphMouthSmileSimpleLeft != null && morphMouthSmileSimpleRight != null)
				{
					if (Mathf.Clamp((interestValence / 50.0f) + mSmileSimpleLeftTarget, 0.0f, interestMaxSmile) > mSmileSimpleLeftValue + 0.02f) { mSmileSimpleLeftValue = Mathf.Min(mSmileSimpleLeftValue + (0.007f * mouthVariation), Mathf.Clamp((interestValence / 50.0f) + mSmileSimpleLeftTarget, 0.0f, interestMaxSmile)); }
					if (Mathf.Clamp((interestValence / 50.0f) + mSmileSimpleLeftTarget, 0.0f, interestMaxSmile) < mSmileSimpleLeftValue - 0.02f) { mSmileSimpleLeftValue = Mathf.Max(mSmileSimpleLeftValue - (0.001f * mouthVariation), Mathf.Clamp((interestValence / 50.0f) + mSmileSimpleLeftTarget, 0.0f, interestMaxSmile)); }
					morphMouthSmileSimpleLeft.SetValue(Round(mSmileSimpleLeftValue));
					if (Mathf.Clamp((interestValence / 50.0f) + mSmileSimpleRightTarget, 0.0f, interestMaxSmile) > mSmileSimpleRightValue + 0.02f) { mSmileSimpleRightValue = Mathf.Min(mSmileSimpleRightValue + (0.007f * mouthVariation), Mathf.Clamp((interestValence / 50.0f) + mSmileSimpleRightTarget, 0.0f, interestMaxSmile)); }
					if (Mathf.Clamp((interestValence / 50.0f) + mSmileSimpleRightTarget, 0.0f, interestMaxSmile) < mSmileSimpleRightValue - 0.02f) { mSmileSimpleRightValue = Mathf.Max(mSmileSimpleRightValue - (0.001f * mouthVariation), Mathf.Clamp((interestValence / 50.0f) + mSmileSimpleRightTarget, 0.0f, interestMaxSmile)); }
					morphMouthSmileSimpleRight.SetValue(Round(mSmileSimpleRightValue));
				}

				if (morphMouthSideLeft != null && morphMouthSideRight != null)
				{
					if (mMouthSideLeftTarget > mMouthSideLeftValue + 0.05f) { mMouthSideLeftValue = Mathf.Min(mMouthSideLeftValue + (0.015f * mouthVariation), mMouthSideLeftTarget); }
					if (mMouthSideLeftTarget < mMouthSideLeftValue - 0.05f) { mMouthSideLeftValue = Mathf.Max(mMouthSideLeftValue - (0.025f * mouthVariation), mMouthSideLeftTarget); }
					morphMouthSideLeft.SetValue(Round(mMouthSideLeftValue));
					if (mMouthSideRightTarget > mMouthSideRightValue + 0.05f) { mMouthSideRightValue = Mathf.Min(mMouthSideRightValue + (0.015f * mouthVariation), mMouthSideRightTarget); }
					if (mMouthSideRightTarget < mMouthSideRightValue - 0.05f) { mMouthSideRightValue = Mathf.Max(mMouthSideRightValue - (0.025f * mouthVariation), mMouthSideRightTarget); }
					morphMouthSideRight.SetValue(Round(mMouthSideRightValue));
				}
				//SuperController.LogError("Mouth Morphs Done");
				
				if (mTongueInOutTarget > mTongueInOutValue + 0.01f) { mTongueInOutValue = Mathf.Min(mTongueInOutValue + (0.08f * mouthVariation), mTongueInOutTarget); }
				if (mTongueInOutTarget < mTongueInOutValue - 0.01f) { mTongueInOutValue = Mathf.Max(mTongueInOutValue - (0.05f * mouthVariation), mTongueInOutTarget); }
				morphTongueInOut.SetValue(Round(mTongueInOutValue));
				if (mTongueSideSideTarget > mTongueSideSideValue + 0.01f) { mTongueSideSideValue = Mathf.Min(mTongueSideSideValue + (0.084f * mouthVariation), mTongueSideSideTarget); }
				if (mTongueSideSideTarget < mTongueSideSideValue - 0.01f) { mTongueSideSideValue = Mathf.Max(mTongueSideSideValue - (0.017f * mouthVariation), mTongueSideSideTarget); }
				morphTongueSideSide.SetValue(Round(mTongueSideSideValue));
				if (mTongueBendTipTarget > mTongueBendTipValue + 0.01f) { mTongueBendTipValue = Mathf.Min(mTongueBendTipValue + (0.08f * mouthVariation), mTongueBendTipTarget); }
				if (mTongueBendTipTarget < mTongueBendTipValue - 0.01f) { mTongueBendTipValue = Mathf.Max(mTongueBendTipValue - (0.06f * mouthVariation), mTongueBendTipTarget); }
				morphTongueBendTip.SetValue(Round(mTongueBendTipValue));
				//SuperController.LogError("Tongue Morphs Done");
			}

			
            if (saccadeClock <= 0.0f && mEyesClosedLeftValue < 0.7f && Random.Range(0.0f,100.0f) > 59.0f && interestKissing == false)
            {
				//SuperController.LogError("Saccade Start");
				tempFloat2 = Mathf.Clamp(Vector3.Distance(headController.followWhenOff.position, eyeController.transform.position) - (closeFaceDistance * 2.0f),0.0f,1.0f);
				tempFloat = saccadeAmount;//(saccadeAmount / Random.Range(0.8f,1.2f)) * uiSaccadeAmount.val * Mathf.Lerp(0.3f,1.0f,tempFloat2);
                float saccade = Random.Range(0.0f,Mathf.Lerp(30.0f,100.0f,tempFloat/30.0f));//Random.Range(0.0f, Mathf.Clamp(150.0f * (0.5f + (100.0f - pExtraversion)), 0.0f, 100.0f));
				debugString = " Random " + Round(saccade) + " Amount " + Round(tempFloat) + " ";
                //saccadeOffset = new Vector3(0.0f,0.0f,0.0f);
                float saccadeLength = (((5.0f + interestArousal) / (2.0f * (10.0f - interestValence))));// * ((2.0f * tempFloat + 20.0f) / 100.0f));// * Random.Range(0.5f, 1.5f);
                float saccadeRandom = Random.Range(-1.0f, 1.0f);
                saccadeClock = Mathf.Clamp(saccadeLength, 0.1f, 0.5f) * Mathf.Lerp(0.25f,1.5f,tempFloat/30.0f) / uiSaccadeSpeed.val;

				if (gAvoid == 1.0f)
				{
					tempFloat = tempFloat * 10.0f;
				}
				if ((Mathf.Abs(saccadeOffset.x) > 50.0f * uiSaccadeWanderMult.val || saccadeOffset.y > 10.0f * uiSaccadeWanderMult.val || saccadeOffset.y < Mathf.Lerp(-15.0f, -90.0f, interestArousal/10.0f) * uiSaccadeWanderMult.val) || Random.Range(0.0f,100.0f) > Mathf.Lerp(99975.0f,99999.0f,pExtraversion/100.0f) / 1000.0f)
				{
					if (gAvoid == 0.0f)
					{
						saccadeOffset = new Vector3(0.0f, 0.0f, 0.0f);
						debugString += "Offset Reset ";
					}
				}
                bool sChange = false;
                if (saccade <= 6.46f && playerHeadToHead > closeFaceDistance * 2.0f && sChange == false)
                {
                    //up right
                    saccadeOffset = new Vector3(saccadeOffset.x + Random.Range(tempFloat, 0.0f), saccadeOffset.y + Random.Range(0.0f, tempFloat / 200.0f), 0.0f);
                    sChange = true;
					debugString += " |Up Right ";
                }
                if (saccade <= 7.45f && playerHeadToHead > closeFaceDistance * 2.0f && sChange == false)
                {
                    //up left
                    saccadeOffset = new Vector3(saccadeOffset.x + Random.Range(-tempFloat, 0.0f), saccadeOffset.y - Random.Range(0.0f, tempFloat / 200.0f), 0.0f);
                    sChange = true;
					debugString += " |Up Left ";
                }
                if (saccade <= 7.79f && sChange == false)
                {
                    //down right
                    saccadeOffset = new Vector3(saccadeOffset.x + Random.Range(tempFloat*Random.Range(0.8f,1.2f), 0.0f), saccadeOffset.y + Random.Range(0.0f, -tempFloat*Random.Range(0.8f,1.2f)), 0.0f);
                    sChange = true;
					debugString += " |Down Right ";
                }
                if (saccade <= 7.89f && sChange == false)
                {
                    //down left
                    saccadeOffset = new Vector3(saccadeOffset.x + Random.Range(-tempFloat*Random.Range(0.8f,1.2f), 0.0f), saccadeOffset.y + Random.Range(0.0f, -tempFloat*Random.Range(0.8f,1.2f)), 0.0f);
                    sChange = true;
					debugString += " |Down Left ";
                }
                if ((saccade <= 15.54f || (playerHeadToHead < personalSpaceDistance / 2.0f && saccade <= 31.08f && saccadeOffset.x < 0.0f)) && sChange == false)
                {
                    //right
                    saccadeOffset = new Vector3(saccadeOffset.x + Random.Range(tempFloat, 0.0f), saccadeOffset.y, 0.0f);
                    sChange = true;
					debugString += " |Right ";
                }
                if ((saccade <= 16.8f || (playerHeadToHead < personalSpaceDistance / 2.0f && saccade <= 33.6f && saccadeOffset.x > 0.0f))  && sChange == false)
                {
                    //left
                    saccadeOffset = new Vector3(saccadeOffset.x + Random.Range(-tempFloat, 0.0f), saccadeOffset.y, 0.0f);
                    sChange = true;
					debugString += " |Left ";
                }
                if (saccade <= 17.69f && playerHeadToHead > personalSpaceDistance / 2.0f && sChange == false)
                {
                    //up
                    saccadeOffset = new Vector3(saccadeRandom, Random.Range(saccadeOffset.x, saccadeOffset.x + (tempFloat / 100.0f)), 0.0f);
                    sChange = true;
					debugString += " |Up ";
                }
                if ((saccade <= 20.38f || (saccade <= Mathf.Lerp(20.38f,30.0f,interestArousal/10.0f) && mainInterest == "Face")) && sChange == false)
                {
                    //down
                    saccadeOffset = new Vector3(saccadeOffset.x, Random.Range(0.0f, saccadeOffset.y-tempFloat*Random.Range(0.8f,1.2f)), 0.0f);
                    sChange = true;
					debugString += " |Down ";
                }
				if (Random.Range(0.0f,1000.0f * uiBlinkSpeed.val) < 15.0f && sChange)
				{
					if (tempFloat >= 5.0f * uiSaccadeAmount.val && eyeClock > (2.45f * uiBlinkSpeed.val))
					{
						if (currentEye != "Closed")
						{
						eyesSM.Switch(eBlink);
						eyeClock = 0.0f;
						debugString += " Small Blink ";
						}
					}
					else
					{
						if (tempFloat >= 10.0f * uiSaccadeAmount.val && eyeClock > (1.35f * uiBlinkSpeed.val))
						{
							if (currentEye != "Closed")
							{
							eyesSM.Switch(eBlink);
							eyeClock = 0.0f;
							debugString += " Big Blink";
							}
						}
					}
				}
				debugString += " Offset " + Round(Vector3.Distance(new Vector3(0.0f,0.0f,0.0f), saccadeOffset));
            }
            else
            {
                saccadeClock -= Time.fixedDeltaTime;
            }
            //saccadeOffset = new Vector3(eyeController.transform.rotation.x + saccadeOffset.x,eyeController.transform.rotation.y + saccadeOffset.y,eyeController.transform.rotation.z + saccadeOffset.z);

			//SuperController.LogError("Saccade Done");

            if (playerHandsUsable || person2Usable)
            {
                if (Vector3.Distance(playerLHand, playerLHandPrev) > minHandMotion)
                {
                    playerLHandMovement = true;
                    playerLHandTimeout = Mathf.Min(playerLHandTimeout + movementModifier, movementMaxTimeout);
                }
                else
                {
                    playerLHandTimeout = Mathf.Max(playerLHandTimeout - movementFalloff, 0.0f);
                    if (playerLHandTimeout == 0.0f)
                    {
                        playerLHandMovement = false;
                    }
                }
                if (Vector3.Distance(playerRHand, playerRHandPrev) > minHandMotion)
                {
                    playerRHandMovement = true;
                    playerRHandTimeout = Mathf.Min(playerRHandTimeout + movementModifier, movementMaxTimeout);
                }
                else
                {
                    playerRHandTimeout = Mathf.Max(playerRHandTimeout - movementFalloff, 0.0f);
                    if (playerRHandTimeout == 0.0f)
                    {
                        playerRHandMovement = false;
                    }
                }
            }
			
			if (emTargetName != "None")
			{
				//float temp = emTargetDistance
                /*if (Vector3.Distance(emTargetController.transform.position, emTargetPosPrev) > minHandMotion)
                {
					emTargetMovement = true;
					emTargetTimeout = Mathf.Min(emTargetTimeout + movementModifier, movementMaxTimeout);
				}
				else
				{
                    emTargetTimeout = Mathf.Max(emTargetTimeout - movementFalloff, 0.0f);
                    if (emTargetTimeout == 0.0f)
                    {
                        emTargetMovement = false;
                    }
				}*/
			}
			
			//SuperController.LogError("Movement Check done");
			
            if (amGlancing)
            {
                glanceClock += Time.fixedDeltaTime;
                if (glanceClock > Mathf.Clamp((100.0f - pExtraversion) / 10, 1.0f, 7.0f))
                {
                    glanceClock = 0.0f;
                    amGlancing = false;
					if (eyeClock >  1.5f * uiBlinkSpeed.val && currentEye != "Closed")
					{
						eyeClock = 0.0f;
						eyesSM.Switch(eBlink);
					}
                }
				//SuperController.LogError("amGlancing clock done");
            }

            if (playerHeadToHead < kissingDistance && interestKissing == false && uiDoKiss.val)
            {
                interestClock -= Time.fixedDeltaTime * 2.0f;
            }
            if (playerHeadToHead > kissingDistance && interestKissing)
            {
                lookSM.Switch(lPlayful);
                mouthSM.Switch(mClosed);
                interestKissing = false;
				mTongueInOutTarget = 1.0f;
				interestClock = 0.0f;
            }

            if (playerHandsUsable || person2Usable)
            {
                //interestLHand = interestLHandBase;
                //interestRHand = interestRHandBase;
			}
            //currentInterestLevel = Mathf.Max(currentInterestLevel - 0.12f, 0.0f);
            //interestFace = interestFaceBase;
			//SuperController.LogError("Interest Calc Start");
			
			interestLHand += (interestLHandBase / 1000.0f) * uiInterestRate.val;
			interestRHand += (interestRHandBase / 1000.0f) * uiInterestRate.val;
			interestFace += (interestFaceBase / 1000.0f) * uiInterestRate.val;
			interestEMTarget += (interestEMTargetBase / 1000.0f) * uiInterestRate.val;
			
			interestFace -= Mathf.Lerp(0.001f,0.004f,interestArousal/10.0f) * uiInterestRate.val;
			interestLHand -= Mathf.Lerp(0.001f,0.005f,interestValence/10.0f) * uiInterestRate.val;
			interestRHand -= Mathf.Lerp(0.001f,0.005f,interestValence/10.0f) * uiInterestRate.val;
			interestEMTarget -= Mathf.Lerp(0.003f,0.001f,interestValence/10.0f) * uiInterestRate.val;

            //Face
            if ((mainInterest == "Face" && mainOld == "Face") && mainClock > gDuration)
            {
                interestFace -= 0.1f * uiInterestRate.val;
				dbgHead += "-Timeout ";
            }
            if (playerHeadToFaceRot < lookDirectAngle)
            {
				if (interestFace < 50.0f)
				{
					interestFace += 0.08f * uiInterestRate.val;
					interestLHand -= 0.001f * uiInterestRate.val;
					interestRHand -= 0.001f * uiInterestRate.val;
				}
				else
				{
					interestFace += 0.01f * uiInterestRate.val;
				}
				interestArousal += 0.002f * uiArousalSpeed.val;
				if (interestValence > 5.0f)
				{
					interestArousal += 0.0004f * uiArousalSpeed.val;
				}
				interestValence += 0.003f * uiValenceSpeed.val;
				if (mainInterest == "Face"){fuzzyLock = 2.0f;}
				dbgHead += "+PLooking ";
            }
			else
			{
				interestFace -= 0.06f * uiInterestRate.val;
			}
			if (playerHeadToHead < kissingDistance && interestKissing == false && headToFaceRot < lookDirectAngle && playerHeadToFaceRot < lookDirectAngle && uiDoKiss.val)// && lipsTouchCount > 0.0f)
			{
				lookSM.Switch(lKissing);
				interestFace += 0.08f * uiInterestRate.val;
				interestArousal += 0.005f * uiArousalSpeed.val;
				interestValence += 0.007f * uiValenceSpeed.val;
				dbgHead += "+WantKiss ";
			}
			if (interestKissing)
			{
				interestFace += 0.3f * uiInterestRate.val;
				interestArousal += 0.015f * uiArousalSpeed.val;
				interestValence += 0.011f * uiValenceSpeed.val;
				dbgHead += "+Kissing ";
			}
            if (headToFaceRot < lookDirectAngle && playerHeadToFaceRot < lookDirectAngle && gAvoid == 0.0f && amGlancing == false && Vector3.Distance(new Vector3(0.0f,0.0f,0.0f),saccadeOffset) < 1.0f)
            {
				interestFace += 0.08f * uiInterestRate.val;
				interestArousal += 0.005f * uiArousalSpeed.val;
				interestValence += 0.007f * uiValenceSpeed.val;
				dbgHead += "+EyeToEye ";
			}
            if (headToFaceRot < lookDirectAngle && mainInterest == "Face")
            {
                interestFace += 0.04f * uiInterestRate.val;
				if (Vector3.Distance(new Vector3(0.0f,0.0f,0.0f),saccadeOffset) < 1.0f)
				{
					interestFace += 0.01f * uiInterestRate.val;
					interestArousal += 0.0005f * uiArousalSpeed.val;
				}
				else
				{
					interestArousal -= 0.001f * uiArousalSpeed.val;
				}
				interestValence += 0.0008f * uiValenceSpeed.val;
                if (mainInterest == "Face"){fuzzyLock = 1.5f;}
				dbgHead += "+LookAt ";
            }
			else
			{
				interestArousal -= 0.001f * uiArousalSpeed.val;
			}
            if (playerHeadToHead < closeFaceDistance * 3.0f)
            {
                interestFace += 0.03f * uiInterestRate.val;
                interestPelvis -= 0.1f * uiInterestRate.val;
                interestTip -= 0.2f * uiInterestRate.val;
				interestValence += 0.0001f * uiValenceSpeed.val;
				if (mainInterest == "Face"){fuzzyLock = 0.5f;}
				dbgHead += "+Close ";
            }
            if (playerHeadToHead < interactionDistance || playerHeadToLBreast < interactionDistance || playerHeadToRBreast < interactionDistance || playerHeadToPelvis < interactionDistance)
            {
                interestFace += 0.15f * uiInterestRate.val;
                interestArousal += 0.0002f * uiArousalSpeed.val;
                interestValence += 0.0002f * uiValenceSpeed.val;
                if (mainInterest == "Face"){fuzzyLock = 07.0f;}
				playerHeadInteract = true;
				dbgHead += "+Interact ";
            }
            if (playerHeadMovement && headToFaceRot < lookPeripheralAngle)
            {
                interestFace += 0.05f * uiInterestRate.val;
                //interestValence += 0.25f;
                if (mainInterest == "Face"){fuzzyLock = 5.5f;}
				dbgHead += "+Move ";
            }
            if (playerHeadMovement == false)
            {
                interestFace -= 0.007f * uiInterestRate.val;
				dbgHead += "-NoMove ";
            }
            if (Mathf.Abs(personChestToHead) > 80.0f)// && playerHeadToHead > personalSpaceDistance)
            {
                interestFace -= 0.02f * uiInterestRate.val;
                if (mainInterest == "Face"){fuzzyLock = 5.0f;}
				dbgHead += "-HighAngle ";
            }
            if (playerHeadToHead < personalSpaceDistance)
            {
                interestFace += 0.015f * uiInterestRate.val;
				dbgHead += "+PSpace ";
            }
            if (playerHeadToHead > personalSpaceDistance && playerHeadToHead < backgroundDistance)
            {
				if (interestFace < 41.0f)
				{
					interestFace += 0.01f * uiInterestRate.val;
					dbgHead += "+AwareDist ";
				}
				else
				{
					interestFace -= 0.01f * uiInterestRate.val;
					dbgHead += "-AwareDist ";
				}
            }
            if (playerHeadToHead > backgroundDistance)
            {
                interestFace -= 0.05f * uiInterestRate.val;
				dbgHead += "-FarOff ";
            }
            //interestFace -= (100.0f - pExtraversion) / 10.0f;
            //interestFace += (100.0f - pStableness) / 10.0f;

			//SuperController.LogError("Face Calc Done");
            if (playerHandsUsable || person2Usable)
            {

                bool interact = false;
                //Left Hand
                if ((mainInterest == "LHand" && mainOld == "LHand") && mainClock > gDuration)
                {
                    interestLHand -= 0.07f * uiInterestRate.val;
                    dbgLHand += "-Timeout ";
                }
                if (playerLHandToHead < closeFaceDistance)// && playerLHandToHead > interactionDistance)
                {
                    interestLHand += 0.02f * uiInterestRate.val;
					interestFace -= 0.01f * uiInterestRate.val;
                    interestArousal += 0.00005f * uiArousalSpeed.val;
                    interestValence += 0.00015f * uiValenceSpeed.val;
                    dbgLHand += "+FaceContact ";
                }
				if (playerHeadToFaceRot < lookDirectAngle*2.0 && headToFaceRot < lookDirectAngle && playerHeadToHead < personalSpaceDistance && interestLHand > 50.0f)
				{
					interestLHand -= 0.03f * uiInterestRate.val;
					dbgLHand += "-EyeToEye ";
				}
                if (playerLHandToHead < interactionDistance || playerLHandToLBreast < interactionDistance || playerLHandToRBreast < interactionDistance || playerLHandToPelvis < interactionDistance * 1.3f)
                {
					if (playerLHandMovement)
					{
                    interestLHand += 0.15f * uiInterestRate.val;
                    interestArousal += 0.0003f * uiArousalSpeed.val;
                    interestValence += 0.00055f * uiValenceSpeed.val;
					}
					else
					{
                    interestLHand -= 0.05f;
					}
                    interestArousal += 0.0001f * uiArousalSpeed.val;
					interestFace -= 0.01f * uiInterestRate.val;
					interestRHand -= 0.02f * uiInterestRate.val;
                    if (mainInterest == "LHand"){fuzzyLock = 3.5f;}
					gHeadSpeed = 0.5f;
					playerLHandInteract = true;
                    dbgLHand += "+Interact ";
                }
                if (playerLHandToLHand < interactionDistance)
                {
                    mLHandFistTarget = Random.Range(0.5f, 0.8f);
                }
                if (playerLHandToRHand < interactionDistance)
                {
                    mRHandFistTarget = Random.Range(0.5f, 0.8f);
                }
                if (playerLHandMovement && headToLHandRot < lookNoAwarenessAngle)
                {
                    interestLHand += 0.07f * (playerLHandTimeout / movementMaxTimeout) * uiInterestRate.val;
                    dbgLHand += "+Move ";
                    if (headToLHandRot > lookPeripheralAngle)
                    {
                        interestLHand += 0.01f * (playerLHandTimeout / movementMaxTimeout) * uiInterestRate.val;
                        dbgLHand += "+MoveNoVis ";
                    }
                }
                if (playerLHandMovement == false)
                {
                    interestLHand -= 0.035f * (1.0f - (playerLHandTimeout / movementMaxTimeout)) * uiInterestRate.val;
                    dbgLHand += "-NoMove ";
                }
                if (playerToPLHand < lookDirectAngle * 2.0f)
                {
                    interestLHand += 0.03f * uiInterestRate.val;
                    if (mainInterest == "LHand"){fuzzyLock = 0.5f;}
                    dbgLHand += "+PLookAt ";
                }
                if (headToLHandRot > lookNoAwarenessAngle)
                {
                    interestLHand -= 0.1f * uiInterestRate.val;
                    if (mainInterest == "LHand"){fuzzyLock = 5.0f;}
                    dbgLHand += "-HighAngle ";
                }
                if (playerLHandToHead < personalSpaceDistance)
                {
                    interestLHand += 0.005f * uiInterestRate.val;
                    dbgLHand += "+PSpace ";
                }
                if (mainInterest == "RHand")
                {
                    interestLHand -= 0.01f * uiInterestRate.val;
                    dbgLHand += "-OtherHand ";
                }
                if (mainOld == "LHand")
                {
                    interestLHand -= 0.02f * uiInterestRate.val;
                    dbgLHand += "-Repeat ";
                }
				
                if (playerLHandToHead > playerHeadToHead * 1.5f)
                {
                    interestLHand -= 0.02f * uiInterestRate.val;
                    dbgLHand += "-HeadCloser ";
                }
                if (playerLHandToHead > personalSpaceDistance && playerLHandToHead < backgroundDistance)
                {
					if (interestLHand < 40.0f)
					{
						interestLHand += 0.01f * uiInterestRate.val;
						dbgLHand += "+AwareDist ";
					}
					else
					{
						interestLHand -= 0.01f * uiInterestRate.val;
						dbgLHand += "-AwareDist ";
					}
                }
                if (playerLHandToHead > backgroundDistance)
                {
                    interestLHand -= 0.05f * uiInterestRate.val;
                    dbgLHand += "-FarOff ";
                }
                if (usePerson2 == false && playerHandsUsable == false)
                {
                    interestLHand -= 1000.0f;
                    dbgLHand += "-NoHands ";
                }
                //interestLHand -= ((100.0f - pStableness) / 1000.0f) * uiInterestRate.val;
                //interestLHand += ((100.0f - pExtraversion) / 1000.0f) * uiInterestRate.val;

				//SuperController.LogError("Left Hand Done");
                //Right Hand
                interact = false;
                if ((mainInterest == "RHand" && mainOld == "RHand") && mainClock > gDuration)
                {
                    interestRHand -= 0.07f * uiInterestRate.val;
                    dbgRHand += "-Timeout ";
                }
                if (playerRHandToHead < closeFaceDistance)// && playerRHandToHead > interactionDistance)
                {
                    interestRHand += 0.02f * uiInterestRate.val;
					interestFace -= 0.01f * uiInterestRate.val;
                    interestArousal += 0.00005f * uiArousalSpeed.val;
                    interestValence += 0.00015f * uiValenceSpeed.val;
                    dbgRHand += "+FaceContact ";
                }
				if (playerHeadToFaceRot < lookDirectAngle*2.0 && headToFaceRot < lookDirectAngle && playerHeadToHead < personalSpaceDistance && interestRHand > 40.0f)
				{
					interestRHand -= 0.03f * uiInterestRate.val;
					dbgRHand += "-EyeToEye ";
				}
                if (playerRHandToHead < interactionDistance || playerRHandToLBreast < interactionDistance || playerRHandToRBreast < interactionDistance || playerRHandToPelvis < interactionDistance * 1.3f)
                {
					if (playerRHandMovement)
					{
                    interestRHand += 0.15f * uiInterestRate.val;
                    interestArousal += 0.0003f * uiArousalSpeed.val;
                    interestValence += 0.00055f * uiValenceSpeed.val;
					}
					else
					{
                    interestRHand -= 0.05f * uiInterestRate.val;
					}
                    interestArousal += 0.0001f * uiArousalSpeed.val;
					interestFace -= 0.01f * uiInterestRate.val;
					interestRHand -= 0.02f * uiInterestRate.val;
                    if (mainInterest == "RHand"){fuzzyLock = 3.5f;}
					gHeadSpeed = 0.5f;
					playerRHandInteract = true;
                    dbgRHand += "+Interact ";
                }
                if (playerRHandToRHand < interactionDistance)
                {
                    mRHandFistTarget = Random.Range(0.5f, 0.8f);
                }
                if (playerRHandToRHand < interactionDistance)
                {
                    mRHandFistTarget = Random.Range(0.5f, 0.8f);
                }
                if (playerRHandMovement && headToRHandRot < lookNoAwarenessAngle)
                {
                    interestRHand += 0.07f * (playerRHandTimeout / movementMaxTimeout) * uiInterestRate.val;
                    dbgRHand += "+Move ";
                    if (headToRHandRot > lookPeripheralAngle)
                    {
                        interestRHand += 0.01f * (playerRHandTimeout / movementMaxTimeout) * uiInterestRate.val;
                        dbgRHand += "+MoveNoVis ";
                    }
                }
                if (playerRHandMovement == false)
                {
                    interestRHand -= 0.035f * (1.0f - (playerRHandTimeout / movementMaxTimeout)) * uiInterestRate.val;
                    dbgRHand += "-NoMove ";
                }
                if (playerToPRHand < lookDirectAngle * 2.0f)
                {
                    interestRHand += 0.03f * uiInterestRate.val;
                    if (mainInterest == "RHand"){fuzzyLock = 0.5f;}
                    dbgRHand += "+PLookAt ";
                }
                if (headToRHandRot > lookNoAwarenessAngle)
                {
                    interestRHand -= 0.1f * uiInterestRate.val;
                    if (mainInterest == "RHand"){fuzzyLock = 5.0f;}
                    dbgRHand += "-HighAngle ";
                }
                if (playerRHandToHead < personalSpaceDistance)
                {
                    interestRHand += 0.005f * uiInterestRate.val;
                    dbgRHand += "+PSpace ";
                }
                if (mainInterest == "RHand")
                {
                    interestRHand -= 0.01f * uiInterestRate.val;
                    dbgRHand += "-OtherHand ";
                }
                if (mainOld == "RHand")
                {
                    interestRHand -= 0.02f * uiInterestRate.val;
                    dbgRHand += "-Repeat ";
                }
				
                if (playerRHandToHead > playerHeadToHead * 1.5f)
                {
                    interestRHand -= 0.02f * uiInterestRate.val;
                    dbgRHand += "-HeadCloser ";
                }
                if (playerRHandToHead > personalSpaceDistance && playerRHandToHead < backgroundDistance)
                {
					if (interestRHand < 40.0f)
					{
						interestRHand += 0.01f * uiInterestRate.val;
						dbgRHand += "+AwareDist ";
					}
					else
					{
						interestRHand -= 0.01f * uiInterestRate.val;
						dbgRHand += "-AwareDist ";
					}
                }
                if (playerRHandToHead > backgroundDistance)
                {
                    interestRHand -= 0.05f * uiInterestRate.val;
                    dbgRHand += "-FarOff ";
                }
                if (usePerson2 == false && playerHandsUsable == false)
                {
                    interestRHand -= 1000.0f;
                    dbgRHand += "-NoHands ";
                }
                //interestRHand -= ((100.0f - pStableness) / 1000.0f) * uiInterestRate.val;
                //interestRHand += ((100.0f - pExtraversion) / 1000.0f) * uiInterestRate.val;

                if (interestLHand == interestRHand)
                {
                    if (playerLHandToHead < playerRHandToHead)
                    {
                        interestRHand -= 1.0f;
                    }
                    else
                    {
                        interestLHand -= 1.0f;
                    }
                }
				//SuperController.LogError("Right Hand Done");
            }
            if (person2Usable)
            {
                //pelvis
                //interestPelvis = interestPelvisBase;
                if ((mainInterest == "Pelvis" && mainOld == "Pelvis") && mainClock > gDuration)
                {
                    interestPelvis -= 0.02f * uiInterestRate.val;
					dbgPenis += "-Timeout ";
                }
                if (mainInterest == "Tip")
                {
                    interestPelvis -= 0.01f * uiInterestRate.val;
					dbgPenis += "-TipFirst ";
                }
                if (playerPelvisToHead < closeFaceDistance * 2.0f)
                {
                    interestPelvis += 0.02f * uiInterestRate.val;
                    //interestArousal += 2.0f;
					playerPenisInteract = true;
                    if (mainInterest == "Pelvis"){fuzzyLock = 0.5f;}
					dbgPenis += "+FaceInteract ";
                }
                if (playerHeadToHead < personalSpaceDistance / 2.0f && playerHeadToHead - 1.0f < playerPelvisToHead)
                {
                    interestPelvis -= 0.2f * uiInterestRate.val;
                    if (mainInterest == "Pelvis"){fuzzyLock = 2.5f;}
					dbgPenis += "-HeadCloser ";
                }
                if (playerTipToPelvis < interactionDistance * 1.0f && mainInterest != "Tip" && playerHeadToHead > personalSpaceDistance/2.0f)
                {
                    //interestArousal += 2.0f;
                    interestPelvis += 0.1f * uiInterestRate.val;
                    interestTip += 0.1f * uiInterestRate.val;
                    if (mainInterest == "Pelvis"){fuzzyLock = 2.5f;}
					dbgPenis += "+SexActPelvis ";
                }
                if (playerPelvisToHead < personalSpaceDistance)
                {
                    interestPelvis += 0.01f * uiInterestRate.val;
					dbgPenis += "+PSpace ";
                }
                if (playerPelvisToHead > personalSpaceDistance && playerPelvisToHead < backgroundDistance)
                {
                    interestPelvis -= 0.01f * uiInterestRate.val;
					dbgPenis += "-AwareDist ";
                }
                if (playerPelvisToHead > backgroundDistance)
                {
                    interestPelvis -= 0.05f * uiInterestRate.val;
					dbgPenis += "-FarOff ";
                }
				if (interestArousal < 6.0f)
				{
					interestPelvis -= 0.04f * uiInterestRate.val;
					dbgPenis += "-NotAroused ";
				}
                //interestPelvis -= ((100.0f - pAgreeableness) / 1000.0f) * uiInterestRate.val;
                //interestPelvis += ((pExtraversion - 50.0f) / 1000.0f) * uiInterestRate.val;

				//SuperController.LogError("Pelvis Done");
                //Penis
                //interestTip = interestTipBase;
                if ((mainInterest == "Penis" || mainOld == "Penis") && mainClock > gDuration && playerTipToHead > closeFaceDistance && playerTipToLBreast > closeFaceDistance && playerTipToRBreast > closeFaceDistance && playerTipToPelvis > closeFaceDistance * 3 && playerTipToLHand > interactionDistance && playerTipToRHand > interactionDistance)
                {
                    interestTip -= 0.02f * uiInterestRate.val;
					dbgPenis += "-Timeout ";
                }
                if (playerTipToHead < closeFaceDistance * 3.0f)
                {
                    //interestArousal += 2.0f;
                    interestTip += 1.0f * uiInterestRate.val;
                    if (mainInterest == "Penis"){fuzzyLock = 0.5f;}
					dbgPenis += "+CloseToFace ";
                }
                if (playerTipToHead < interactionDistance || playerTipToLBreast < interactionDistance || playerTipToRBreast < interactionDistance || playerTipToPelvis < interactionDistance)
                {
                    //interestArousal += 2.0f;
                    interestTip += 0.5f * uiInterestRate.val;
					interestArousal += 0.01f;
                    if (mainInterest == "Penis"){fuzzyLock = 0.25f;}
					playerPenisInteract = true;
					dbgPenis += "+Interact ";
                }
                if (playerTipMovement && headToFaceRot > lookPeripheralAngle)
                {
                    interestTip += 0.03f * uiInterestRate.val;
					dbgPenis += "+Move ";
                }
                if (playerTipMovement == false && playerTipToHead > closeFaceDistance)
                {
                    interestTip -= 0.01f * uiInterestRate.val;
                    if (mainInterest == "Penis"){fuzzyLock = 2.5f;}
					dbgPenis += "-NoMove ";
                }
                if (playerTipToHead < personalSpaceDistance)
                {
                    interestTip += 0.03f * uiInterestRate.val;
					dbgPenis += "+PSpace ";
                }
                if (playerTipToHead < playerLHandToHead && playerTipToHead < playerRHandToHead && (playerTipToPelvis < interactionDistance * 2 || playerTipToLBreast < interactionDistance || playerTipToRBreast < interactionDistance) && person2IsMale)
                {
                    interestTip += 2.0f * uiInterestRate.val;
                    interestPelvis += 0.3f * uiInterestRate.val;
                    interestLHand -= 0.03f * uiInterestRate.val;
                    interestRHand -= 0.03f * uiInterestRate.val;
					interestArousal += 0.03f;
                    if (mainInterest == "Penis"){fuzzyLock = 0.5f;}
					dbgPenis += "+SexAction ";
                }
                if (playerTipToHead > personalSpaceDistance && playerTipToHead < backgroundDistance)
                {
					if (interestTip < 40.5f)
					{
						interestTip += 0.01f * uiInterestRate.val;
						dbgPenis += "+AwareDist ";
					}
					else
					{
						interestTip -= 0.01f * uiInterestRate.val;
						dbgPenis += "-AwareDist ";
					}
                }
                if (playerTipToHead > backgroundDistance)
                {
                    interestTip -= 1.0f * uiInterestRate.val;
					dbgPenis += "-FarOff ";
                }
				if (playerHeadToFaceRot < lookDirectAngle*2.0f && playerTipToHead > closeFaceDistance)
				{
					interestTip -= 0.1f * uiInterestRate.val;
					dbgPenis += "-LookAtFace ";
				}
                if ((playerTipToHead < interactionDistance * 1.5f || playerTipToLHand < interactionDistance * 1.5f || playerTipToRHand < interactionDistance * 1.5f) && person2IsMale == true && playerHeadToHead > personalSpaceDistance/2.0f)
                {
                    interestTip += 1.0f * uiInterestRate.val;
                    interestPelvis += 0.3f * uiInterestRate.val;
					interestArousal += 0.03f;
                    if (mainInterest == "Penis"){fuzzyLock = 0.5f;}
					dbgPenis += "+SexInteract ";
                }
                else
                {
                    interestTip -= (pStableness) / 10.0f;
                }
				if (interestArousal < 7.0f && playerTipToHead > personalSpaceDistance)
				{
					interestTip -= 0.7f * uiInterestRate.val;
					dbgPenis += "-NotAroused ";
				}

				//SuperController.LogError("Penis Done");
                //interestTip += (pExtraversion - 50.0f) / 500.0f;
            }
			
			//interestEMTarget = interestEMTargetBase;
			if (emTargetName != "None")
			{
				
				if (emTargetMovement)
				{
					interestEMTarget += 0.07f * Mathf.Clamp(emTargetTimeout / movementMaxTimeout,0.0f,2.0f) * uiInterestRate.val;
					dbgObject += "+Move ";
				}
				if (emTargetDistance < personalSpaceDistance)
				{
					interestEMTarget += 0.04f * uiInterestRate.val;
					if (uiTargetLook.val == false)
					{
						interestEMTarget += 0.01f * uiInterestRate.val;
					}
					dbgObject += "+PSpace ";
				}
				else
				{
					interestEMTarget -= 0.1f * uiInterestRate.val;
				}
				if (emTargetDistance < closeFaceDistance * 2.0f)
				{
					interestEMTarget += 0.07f * uiInterestRate.val;
					if (uiTargetLook.val == false)
					{
						interestEMTarget += 0.03f * uiInterestRate.val;
					}
					dbgObject += "+Close ";
				}
				else
				{
					interestEMTarget -= 0.1f * uiInterestRate.val;
				}
				if (emTargetDir < lookPeripheralAngle)
				{
					interestEMTarget += 0.06f * uiInterestRate.val;
					dbgObject += "+Visible ";
				}
				else
				{
					interestEMTarget -= 0.01f * uiInterestRate.val;
				}
				if (emTargetHeadDir < lookDirectAngle && uiTargetLook.val)
				{
					interestEMTarget += 0.3f * uiInterestRate.val;
					if (emTargetDistance < personalSpaceDistance)
					{
						interestEMTarget += 0.2f * uiInterestRate.val;
					}
					dbgObject += "+Looking ";
				}
				else
				{
					interestEMTarget -= 0.01f * uiInterestRate.val;
				}
				if (mainOld == "Target")
				{
					interestEMTarget -= 0.03f * uiInterestRate.val;
					dbgObject += "+Previous ";
				}
				if (headToFaceRot < lookDirectAngle)
				{
					interestEMTarget -= 0.2f * uiInterestRate.val;
					dbgObject += "-LookingAtPlayer ";
					if (playerHeadToFaceRot < lookDirectAngle && amGlancing == false && gAvoid == 0.0f)
					{
						interestEMTarget -= 0.3f * uiInterestRate.val;
						dbgObject += "-EyeToEye ";
					}
				}
			}
			else
			{
				interestEMTarget = 0.0f;
			}
			//SuperController.LogError("Target Object Done");
			
			if (interestArousal > 6.0f && interestValence < 5.0f)
			{
				interestArousal -= 0.00025f / uiArousalSpeed.val;
			}
			if (interestValence > 8.0f && interestArousal < 6.0f)
			{
				interestValence -= 0.00065f / uiValenceSpeed.val;
			}
			
				interestFace = Mathf.Clamp(interestFace, (interestFaceBase/2.0f) * uiHeadInterest.val, (maxInterestLevel) * uiHeadInterest.val);
				interestLHand = Mathf.Clamp(interestLHand, (interestLHandBase/2.0f) * uiLHandInterest.val, (maxInterestLevel - 1.0f) * uiLHandInterest.val);
				interestRHand = Mathf.Clamp(interestRHand, (interestRHandBase/2.0f) * uiRHandInterest.val, (maxInterestLevel - 1.0f) * uiRHandInterest.val);
				interestPelvis = Mathf.Clamp(interestPelvis, (interestPelvisBase/2.0f) * uiPenisInterest.val, (maxInterestLevel + 1.0f) * uiPenisInterest.val);
				interestTip = Mathf.Clamp(interestTip, (interestTipBase/2.0f) * uiPenisInterest.val, (maxInterestLevel + 4.0f) * uiPenisInterest.val);
				interestEMTarget = Mathf.Clamp(interestEMTarget, (interestEMTargetBase/2.0f) * uiObjectInterest.val, (maxInterestLevel + 5.0f) * uiObjectInterest.val);
				
				
			//SuperController.LogError("Interest Calc Done");
			
            if (interestClock <= 0.0f && amGlancing == false)
            {

                float interestUpdate = currentInterestLevel;
                if (currentInterest == "Pelvis")
                {
                    interestUpdate = interestPelvis;
                    interestPelvis = interestPelvis - (25.0f * (mainClock / 5.0f));
                }
                if (currentInterest == "Tip")
                {
                    interestUpdate = interestTip;
                    interestTip = interestTip - (25.0f * (mainClock / 5.0f));
                }
                if (currentInterest == "RHand")
                {
                    interestUpdate = interestRHand;
                    interestRHand = interestRHand - (25.0f * (mainClock / 5.0f));
                }
                if (currentInterest == "LHand")
                {
                    interestUpdate = interestLHand;
                    interestLHand = interestLHand - (25.0f * (mainClock / 5.0f));
                }
                if (currentInterest == "Face" && interestKissing == false)
                {
                    interestUpdate = interestFace;
                    interestFace = interestFace - (10.0f * (mainClock / 5.0f));
                }
                if (currentInterest == "Target")
                {
                    interestUpdate = interestEMTarget;
                    interestEMTarget = interestEMTarget - (10.0f * (mainClock / 5.0f));
                }

				
				//SuperController.LogError("Main Interest Start");
                mainValue = 0.0f;
                secondValue = 0.0f;
				if (interestFace > secondValue || playerHeadToHead < closeFaceDistance * 1.75f)
                {
                    if (interestFace > mainValue || playerHeadToHead < closeFaceDistance * 1.75f)
                    {
						//if ((mainInterest != "RandomR" && mainInterest != "RandomL" && mainInterest != "RandomF") || interestValence > 5.0f)
						//{
							if (mainInterest != "Face")
							{
								mainOld = mainInterest;
								mainSwitch = true;
								//eyesSM.Switch(eFocus);
								//mouthSM.Switch(mBiteLip);
								if (currentLook == "Casual" || currentLook == "Bored" || currentLook == "DayDream")
								{
									if (interestValence < 6.0f)
									{
										mouthSM.Switch(mSmile);
									}
									else
									{
										mouthSM.Switch(mBigSmile);
									}
								}
							}
							mainInterest = "Face";
							mainValue = interestFace;

							if (playerHeadToHead < closeFaceDistance * 1.75f)
							{
								mainValue = interestFace + 30.0f;
							}
						//}
                    }
                    else
                    {
                        if (secondInterest != "Face" && secondOld != "Face")
                        {
                            secondOld = secondInterest;
                            secondSwitch = true;
                            //mouthSM.Switch(mSmile);
                        }
                        secondInterest = "Face";
                        secondValue = interestFace;
                    }
                }
                if (interestLHand > secondValue && (playerHandsUsable || (person2Usable && usePerson2)))
                {
                    if (interestLHand > mainValue)
                    {
                        if (mainInterest != "LHand")
                        {
                            mainOld = mainInterest;
                            mainSwitch = true;
                            if (morphBrowAction == false)
                            {
                                browSM.Switch(bRaised);
                            }
							if (Vector3.Angle(playerLHand - headController.followWhenOff.position, headController.followWhenOff.forward) > lookNoAwarenessAngle && playerLHandToHead > interactionDistance && currentEye != "Closed")
							{
								eyesSM.Switch(eClosed);
							}
                        }
                        mainInterest = "LHand";
                        mainValue = interestLHand;
                    }
                    else
                    {
						if (interestLHand > 30.0f)
						{
							if (secondInterest != "LHand" && secondOld != "LHand")
							{
								secondOld = secondInterest;
								secondSwitch = true;
								if (interestArousal < 5.0f && morphBrowAction == false && Random.Range(0.0f,100.0f) > 97.0f)
								{
									browSM.Switch(bOneRaise);
								}
							}
							secondInterest = "LHand";
							secondValue = interestLHand;
						}
                    }
                }
                if (interestRHand > secondValue && (playerHandsUsable || (person2Usable && usePerson2)))
                {
                    if (interestRHand > mainValue)
                    {
						if (interestRHand > 30.0f)
						{
							if (mainInterest != "RHand" && secondOld != "RHand")
							{
								mainOld = mainInterest;
								mainSwitch = true;
								if (morphBrowAction == false)
								{
									browSM.Switch(bRaised);
								}
								if (Vector3.Angle(playerRHand - headController.followWhenOff.position, headController.followWhenOff.forward) > lookNoAwarenessAngle && playerRHandToHead > interactionDistance && currentEye != "Closed")
								{
									eyesSM.Switch(eClosed);
								}
							}
							mainInterest = "RHand";
							mainValue = interestRHand;
						}
                    }
                    else
                    {
                        if (secondInterest != "RHand" && secondOld != "RHand")
                        {
                            secondOld = secondInterest;
                            secondSwitch = true;
                            if (interestArousal < 5.0f && morphBrowAction == false && Random.Range(0.0f,100.0f) > 97.0f)
                            {
                                browSM.Switch(bOneRaise);
                            }
                        }
                        secondInterest = "RHand";
                        secondValue = interestRHand;
                    }
                }
                if (interestPelvis > secondValue && person2Usable && usePerson2)
                {
                    if (interestPelvis > mainValue && playerHeadToHead > personalSpaceDistance/2.0f)
                    {
                        if (mainInterest != "Pelvis")
                        {
                            mainOld = mainInterest;
                            mainSwitch = true;
                            if (interestArousal > 5.0f && lookAction == false)
                            {
                                lookSM.Switch(lPlayful);
                            }
                        }
                        mainInterest = "Pelvis";
                        mainValue = interestPelvis;
                    }
                    else
                    {
                        if (secondInterest != "Pelvis" && secondOld != "Pelvis")
                        {
                            secondOld = secondInterest;
                            secondSwitch = true;
                            if (lookAction == false)
                            {
                                lookSM.Switch(lInquisitive);
                            }
                        }
                        secondInterest = "Pelvis";
                        secondValue = interestPelvis;
                    }
                }
                if ((interestTip > secondValue))// || ((playerTipToHead < closeFaceDistance || playerTipToPelvis < closeFaceDistance) && playerHeadToHead > closeFaceDistance * 2.0f)) && usePerson2)
                {
                    if (interestTip > mainValue && playerHeadToHead > personalSpaceDistance/2.0f)// || (playerTipToHead < closeFaceDistance || playerTipToPelvis < closeFaceDistance && playerHeadToHead > closeFaceDistance))
                    {
                        if (mainInterest != "Tip")
                        {
                            mainOld = mainInterest;
                            mainSwitch = true;
                            //lookSM.Switch(lIntense);
                        }
                        mainInterest = "Tip";
                        mainValue = interestTip;
                    }
                    else
                    {
                        if (secondInterest != "Tip")
                        {
                            secondOld = secondInterest;
                            secondSwitch = true;
                            //lookSM.Switch(lPlayful);
                        }
                        secondInterest = "Tip";
                        secondValue = interestTip;
                    }
                }
				
                if (interestEMTarget > secondValue && uiObjectTarget.val != "None")
                {
                    if (interestEMTarget > mainValue)
                    {
                        if (mainInterest != "Target")
                        {
                            mainOld = mainInterest;
                            mainSwitch = true;
                            if (interestArousal > 5.0f && lookAction == false)
                            {
                                lookSM.Switch(lPlayful);
                            }
                        }
                        mainInterest = "Target";
                        mainValue = interestEMTarget;
                    }
                    else
                    {
                        if (secondInterest != "Target" && secondOld != "Target")
                        {
                            secondOld = secondInterest;
                            secondSwitch = true;
                            if (lookAction == false)
                            {
                                lookSM.Switch(lInquisitive);
                            }
                        }
                        secondInterest = "Target";
                        secondValue = interestEMTarget;
                    }
                }
				
                if (mainSwitch)
                {
                    mainClock = 0.0f;
					saccadeOffset = new Vector3(0.0f, 0.0f, 0.0f);
                }
				
				if (playerLHandToHead < closeFaceDistance)
					{
						if (mainInterest != "LHand")
						{
							mainOld = mainInterest;
						}
						mainInterest = "LHand";
						mainValue = 100;
						secondOld = secondInterest;
						secondInterest = "Face";
						secondValue = 60;
					}
				if (playerRHandToHead < closeFaceDistance)
					{
						if (mainInterest != "RHand")
						{
							mainOld = mainInterest;
						}
						mainInterest = "RHand";
						mainValue = 100;
						secondOld = secondInterest;
						secondInterest = "Face";
						secondValue = 60;
					}
					
				if (secondInterest == "RandomR" || secondInterest == "RandomL" || secondInterest == "RandomF" || secondInterest == "RandomU")
				{
					if (Random.Range(0.0f,100.0f) > 85.0f)
					{
						if (Random.Range(0.0f,100.0f) > 90.0f)
						{
							secondInterest = "RandomF";
						}
						else
						{
							if (Random.Range(0.0f,100.0f) > 50.0f)
							{
							secondInterest = "RandomL";
							}
							else
							{
							secondInterest = "RandomR";
							}
						}
					}
				}
				
				if (secondInterest == "RHand" && interestRHand < 10.0f)
				{
					if (Random.Range(0.0f,100.0f) > 5.0f)
					{
						secondInterest = "RandomR";
					}
					else
					{
						secondInterest = "RandomF";
					}
				}
				
				if (secondInterest == "LHand" && interestLHand < 10.0f)
				{
					if (Random.Range(0.0f,100.0f) > 5.0f)
					{
						secondInterest = "RandomL";
					}
					else
					{
						secondInterest = "RandomF";
					}
				}
                string oldInterest = currentInterest;
				if ((mainOld == "RandomR" || mainOld == "RandomL" || mainOld == "RandomF") && mainInterest == "Face")
				{
					if (playerHeadToHead > personalSpaceDistance && interestValence < 5.0f && Random.Range(0.0f,100.0f) > 85.0f)
					{
						mainInterest = mainOld;
						mainOld = "Face";
					}
				}
				

				if (mainInterest != "Tip" && playerTipToHead < closeFaceDistance * 3.0f && interestArousal > 7.0f)
				{
					mainInterest = "Tip";
					interestClock = 0.0f;
				}
				
				if (vagTouchCount > 0.0f && currentLook == "Sex" && interestArousal > 9.5f && interestValence > 9.5f && mainInterest != "Face" && currentEye != "Closed")
				{
					mainOld = mainInterest;
					mainInterest = "Face";
				}
				
				//SuperController.LogError("Main interest mood and look");
                //currentInterestLevel = 0.0f;
                if (mainInterest == "Pelvis")
                {
                    interestArousal += (0.05f * (0.1f + (pExtraversion / 100.0f))) * uiArousalSpeed.val;
                    interestValence += (0.012f * (0.1f + (pAgreeableness / 100.0f))) * uiValenceSpeed.val;
                    currentInterest = "Pelvis";
                    currentInterestLevel = interestPelvis;
                    playerInterest += 2.0f;
                    if (lookAction == false)
                    {
                        if (interestArousal + interestValence < 5)
                        {
                            lookSM.SwitchRandom(new State[] {
                                            lCasual,
                                            lInquisitive
                                        });
                        }
                        else
                        {
                            if (interestArousal + interestValence > 15)
                            {
                                lookSM.SwitchRandom(new State[] {
                                                lCasual,
                                                lCasual,
                                                lInquisitive,
                                                lPlayful
                                            });
                            }
                            else
                            {
                                lookSM.SwitchRandom(new State[] {
                                                lIntense,
                                                lIntense,
                                                lPlayful,
                                                lPlayful,
                                                lInquisitive
                                            });
                            }
                        }
                    }
                }
                if (mainInterest == "Tip")
                {
                    interestArousal += (0.2f * (0.1f + (pExtraversion / 100.0f))) * uiArousalSpeed.val;
                    interestValence += (0.035f * (0.1f + (pAgreeableness / 100.0f))) * uiValenceSpeed.val;
                    currentInterest = "Tip";
                    currentInterestLevel = interestTip;
                    playerInterest += 5.0f;
                    if (lookAction == false)
                    {
                        if (playerTipToPelvis < interactionDistance * 1.5f && playerHeadToHead > personalSpaceDistance/2.0f && uiDoSex.val)
                        {
                            lookSM.Switch(lSex);
                        }
                        else
                        {
                            if (playerTipToHead < interactionDistance && uiDoBlowjob.val && lipsTouchCount > 0.0f)
                            {
                                lookSM.Switch(lSucking);
                            }
                            else
                            {
                                if (interestArousal + interestValence < 5)
                                {
                                    lookSM.SwitchRandom(new State[] {
                                                    lCasual,
                                                    lInquisitive
                                                });
                                }
                                else
                                {
                                    if (interestArousal + interestValence > 15)
                                    {
                                        lookSM.SwitchRandom(new State[] {
                                                        lCasual,
                                                        lCasual,
                                                        lInquisitive,
                                                        lPlayful
                                                    });
                                    }
                                    else
                                    {
                                        lookSM.SwitchRandom(new State[] {
                                                        lIntense,
                                                        lIntense,
                                                        lPlayful,
                                                        lPlayful,
                                                        lInquisitive
                                                    });
                                    }
                                }
                            }
                        }
                    }
                }
                if (mainInterest == "RHand")
                {
                    interestArousal += (0.005f * (0.1f + ((100.0f - pExtraversion) / 100.0f))) * uiArousalSpeed.val;
                    interestValence += (0.012f * (0.1f + (pAgreeableness / 100.0f))) * uiValenceSpeed.val;
                    currentInterest = "RHand";
                    currentInterestLevel = interestRHand;
                    playerInterest += 1.0f;
                    if (lookAction == false)
                    {
                        if (playerRHandToHead < interactionDistance || playerRHandToLBreast < interactionDistance || playerRHandToRBreast < interactionDistance || playerRHandToPelvis < interactionDistance * 1.3f)
                        {
                            lookSM.SwitchRandom(new State[] {
                                            lInquisitive,
                                            lFeel,
                                            lFeel,
                                            lFeel,
                                            lPlayful
                                        });
                        }
                        else
                        {
                            lookSM.SwitchRandom(new State[] {
                                            lInquisitive,
                                            lInquisitive,
                                            lCasual,
                                            lPlayful
                                        });
                        }
                    }
                }
                if (mainInterest == "LHand")
                {
                    interestArousal += (0.005f * (0.1f + ((100.0f - pExtraversion) / 100.0f))) * uiArousalSpeed.val;
                    interestValence += (0.012f * (0.1f + (pAgreeableness / 100.0f))) * uiValenceSpeed.val;
                    currentInterest = "LHand";
                    currentInterestLevel = interestLHand;
                    playerInterest += 1.0f;
                    if (lookAction == false)
                    {
                        if (playerLHandToHead < interactionDistance || playerLHandToLBreast < interactionDistance || playerLHandToRBreast < interactionDistance || playerLHandToPelvis < interactionDistance * 2.0f)
                        {
                            lookSM.SwitchRandom(new State[] {
                                            lInquisitive,
                                            lFeel,
                                            lFeel,
                                            lFeel,
                                            lPlayful
                                        });
                        }
                        else
                        {
                            lookSM.SwitchRandom(new State[] {
                                            lInquisitive,
                                            lInquisitive,
                                            lCasual,
                                            lPlayful
                                        });
                        }
                    }
                }
				tempFloat = 0.0f;
				if ((playerLHandToLBreast < interactionDistance || playerLHandToRBreast < interactionDistance || playerLHandToPelvis < interactionDistance * 1.3f) || (playerRHandToLBreast < interactionDistance || playerRHandToRBreast < interactionDistance || playerRHandToPelvis < interactionDistance * 1.3f))
					{
						tempFloat = 1.0f;
					}
				if (interestArousal > 9.5f && interestValence > 9.0f)
				{
					tempFloat = 1.0f;
				}
                if (mainInterest == "Face" || mainInterest == "Target")
                {
					if (playerHeadToHead < closeFaceDistance * 2.5f && mainInterest == "Face")
					{
						interestValence += (0.12f * (0.1f + (Mathf.Clamp(pAgreeableness + (interestArousal * 2.0f),0.0f,100.0f) / 100.0f))) * uiValenceSpeed.val;
					}
					else
					{
						interestValence += (0.1f * (0.1f + (Mathf.Clamp(pAgreeableness + (interestArousal * 2.0f),0.0f,100.0f) / 100.0f))) * uiValenceSpeed.val;
					}
					if (mainInterest == "Face")
					{
						interestArousal += (0.007f * (0.1f + ((100.0f - pExtraversion) / 100.0f))) * uiArousalSpeed.val;
						currentInterest = "Face";
						currentInterestLevel = interestFace;
					}
					else
					{
						currentInterest = "Target";
						currentInterestLevel = interestEMTarget;
					}
                    if (lookAction == false)
                    {
                        if (playerHeadToPelvis < interactionDistance && playerHeadMovement && mainInterest == "Face" && playerHeadToHead > personalSpaceDistance/2.0f && uiDoSex.val)
                        {
                            lookSM.Switch(lSex);
                        }
                        else
                        {
                            if (interestArousal + interestValence < 7.0f)
                            {
								if (playerHeadToFaceRot < lookDirectAngle * 2.0f)
								{
									lookSM.SwitchRandom(new State[] {
													//lBored,
													lCasual,
													lCasual,
													lCasual,
													lInquisitive,
													lInquisitive
												});
								}
								else
								{
									lookSM.SwitchRandom(new State[] {
													lBored,
													lDayDream,
													lDayDream,
													lCasual
												});
								}
                            }
                            else
                            {
                                if (interestArousal + interestValence < 15.0f)
                                {
                                    lookSM.SwitchRandom(new State[] {
                                                    lInquisitive,
                                                    lInquisitive,
                                                    lInquisitive,
                                                    lInquisitive,
                                                    lIntense,
                                                    lPlayful
                                                });
                                }
                                else
                                {
									if (tempFloat == 1.0f)
									{
										lookSM.Switch(lFeel);
									}
									else
									{
										lookSM.SwitchRandom(new State[] {
														lFeel,
														lPlayful,
														lPlayful,
														lPlayful,
														lIntense,
														lIntense
													});
									}
                                }
                            }
                        }
                    }
                    playerInterest += 0.5f;
                }


                if ((interestFace <= 40.0f && interestLHand <= 40.0f && interestRHand <= 40.0f && interestPelvis <= 40.0f && interestTip <= 40.0f && interestEMTarget <= 40.0f) || mainInterest == "RandomF" || mainInterest == "RandomU" || mainInterest == "RandomL" || mainInterest == "RandomR" || ((currentLook == "Bored" || currentLook == "DayDream") && Random.Range(0.0f,100.0f) > 80.0f && interestArousal < 5.0f))
                {
                    interestArousal -= (0.2f * (0.1f + ((100.0f - pStableness) / 100.0f))) / uiArousalSpeed.val;
                    interestValence -= (0.065f * (0.1f + ((100.0f - pAgreeableness) / 100.0f))) / uiValenceSpeed.val;
					if (lookAction && (currentLook == "Bored" || currentLook == "DayDream") && interestArousal > 5.5f)
					{
                        lookSM.SwitchRandom(new State[] {
                                        lPlayful,
                                        lInquisitive,
                                        lCasual,
                                    });
					}
					else
					{
						if (lookAction == false && playerHeadToHead > personalSpaceDistance && interestValence < 5.0f)
						{
							lookSM.SwitchRandom(new State[] {
											lBored,
											lBored,
											lBored,
											lCasual,
											lDayDream,
										});
						}
					}
                    if ((mainInterest == "RandomF" || mainInterest == "RandomU" || mainInterest == "RandomL" || mainInterest == "RandomR") && Random.Range(0.0f,100.0f) > 10.0f * uiInterestSpeed.val)
                    {
                        currentInterest = mainInterest;
                    }
                    else
                    {
						if (Random.Range(0.0f, 100.0f) < 10.0f + (interestFace / 10.0f))
						{
							currentInterest = "Face";
						}
						else
						{
							playerInterest -= 10.5f;
							if (Random.Range(0.0f, 100.0f) < 5.0f)
							{
								currentInterest = "RandomF";
							}
							else
							{
								if (Random.Range(0.0f, 100.0f) < 10.0f)
								{
									currentInterest = "RandomU";
								}
								else
								{
									if (Random.Range(0.0f, 100.0f) > 50.0f)
									{
										currentInterest = "RandomR";
									}
									else
									{
										currentInterest = "RandomL";
									}
								}
							}
						}
						if (mainInterest != currentInterest)
						{
							//mainOld = mainInterest;
						}
						if (secondInterest != currentInterest)
						{
							//secondOld = secondInterest;
						}
						//mainInterest = currentInterest;
						//secondInterest = currentInterest;
						currentInterestLevel = 50.0f;
                    }
                }
                if (currentInterest != oldInterest && eyeClock > (1.45f * uiBlinkSpeed.val) && amGlancing == false && currentEye != "Closed")
                {
					if (morphEyeAction == false)
					{
						eyesSM.Switch(eBlink);
						eyeClock = 0.0f;
					}
                }
                if (playerHeadToHead < kissingDistance && interestKissing == false && headToFaceRot < lookDirectAngle * 2.0f && playerHeadToFaceRot < lookDirectAngle && uiDoKiss.val && lipsTouchCount > 0.0f)
                {
                    lookSM.Switch(lKissing);
                }
                float clockBase = Random.Range(2.0f, 7.0f);
                if (mainSwitch || secondSwitch)
                {
                    clockBase = Random.Range(4.0f, 10.0f);
                }
				if ((mainInterest == "Tip" || mainInterest == "Pelvis") && (playerTipToHead < closeFaceDistance || playerTipToPelvis < interactionDistance))
				{
					clockBase = Random.Range(1.0f, 2.0f);
				}
                interestClock = Mathf.Clamp(clockBase * (pStableness / 50.0f), 2.0f, 12.0f) * uiInterestSpeed.val;
				//SuperController.LogError("Main Interest Done");
            }
            else
            {
                interestClock -= Time.fixedDeltaTime;
				//SuperController.LogError("Interest Clock Counting");
            }
			
			
			if (playerHeadToFaceRot < lookDirectAngle && (playerHeadToHead < personalSpaceDistance || pExtraversion < 35.0f) && uiGazeAvoid.val)
			{
				//SuperController.LogError("Gaze Avoid start");
				if (mainInterest != "Face" && gAvoid == 0.0f && amGlancing == false && playerHeadToHead < personalSpaceDistance / 2.0f)
				{
					interestClock -= Time.fixedDeltaTime;
				}
				//SuperController.singleton.ClearMessages();
				//SuperController.LogMessage("Eye Contact (" + (gAvoidance / 4.0) + "/" + ((100.0f-gAvoidance) / 10.0f) + ")" + gAvoidanceClock + "/" + gAvoidingClock + "/" + gAvoid, false);
				if (gAvoidingClock > 0.0f && gAvoid == 1.0f)
				{
					tempFloat = 1.0f;
					if (currentLook == "bored")
					{
						tempFloat = 0.3f;
					}
					if (currentLook == "casual")
					{
						tempFloat = 0.65f;
					}
					if (currentLook == "intense")
					{
						tempFloat = 1.35f;
					}
					if ( mEyesClosedLeftValue > 0.5f)
					{
						tempFloat = 0.1f;
					}
					if ( playerHeadToFaceRot < lookDirectAngle)
					{
						tempFloat = Mathf.Max(tempFloat - 0.2f,0.1f);
					}
					gAvoidingClock -= Time.fixedDeltaTime * (tempFloat * Mathf.Lerp(0.3f, 1.3f,interestArousal / 10.0f));
				}
				else
				{
					if (gAvoidingClock <= 0.0f && gAvoid == 1.0f)
					{
						gAvoid = 0.0f;
						gAvoidanceClock = 0.0f;
						gAvoidingClock = 0.0f;
						if (eyeClock > 0.7f * uiBlinkSpeed.val && currentEye != "Closed")
						{
							eyesSM.Switch(eBlink);
							eyeClock = 0.0f;
						}
						if (mainInterest != gAvoidInterest)
						{
							//mainOld = mainInterest;
						}
						mainInterest = gAvoidInterest;
						interestClock = 0.0f;
					}
				}
				if (gAvoidanceClock < Mathf.Lerp(1.5f * uiGazeLookTime.val,13.0f * uiGazeLookTime.val,pExtraversion/100.0f) && gAvoid == 0.0f)
				{
					if (playerHeadToHead <= backgroundDistance && amGlancing == false && mEyesClosedLeftTarget < 0.5f && currentLook != "Intense" && interestValence < 9.5f)
					{
						if (mainInterest == "Face" && Vector3.Distance(new Vector3(0.0f,0.0f,0.0f),saccadeOffset) < 15.0f * uiSaccadeAmount.val)
						{
							gAvoidanceClock += Time.fixedDeltaTime * (2.0f * (1.0f-(interestArousal / 10.0f)));
						}
					}
					if ((amGlancing == true || Vector3.Distance(new Vector3(0.0f,0.0f,0.0f),saccadeOffset) > 15.0f * uiSaccadeAmount.val || mEyesClosedLeftTarget > 0.5f) && gAvoidanceClock > 0.0f)
					{
						gAvoidanceClock -= Time.fixedDeltaTime / 2.0f;
					}
				}
				else
				{
					if (gAvoidanceClock >= Mathf.Lerp(1.5f * uiGazeLookTime.val,13.0f * uiGazeLookTime.val,pExtraversion/100.0f) && gAvoid == 0.0f && Random.Range(0.0f,100.0f) > Mathf.Lerp(99.0f,99.99f,interestArousal/10.0f))
					{
						gAvoid = 1.0f;
						gAvoidInterest = mainInterest;
						gAvoidingClock = Mathf.Lerp(4.0f * uiGazeAvoidTime.val, 0.5f * uiGazeAvoidTime.val, interestValence/10.0f) * Mathf.Lerp(2.0f,0.75f,pExtraversion/100.0f);
						//eyesSM.Switch(eClosed);
						if (morphMouthAction == false)
						{
							mouthSM.SwitchRandom(new State[] {
										mSmile,
										mSmile,
										mSmile,
										mSmirk,
										mSideways
									});
						}
						//mouthSM.Switch(mBigSmile);
						if (secondInterest == "RandomL" || secondInterest == "RandomR")
						{
							if (secondInterest == "RandomL")
							{
								if (lookAwaySide == "right")
								{
									eyesSM.Switch(eBlink);
									eyeClock = 0.0f;
								}
								lookAwaySide = "left";
							}
							else
							{
								if (lookAwaySide == "left")
								{
									eyesSM.Switch(eBlink);
									eyeClock = 0.0f;
								}
								lookAwaySide = "right";
							}
						}
						else
						{
							if (Random.Range(0.0f, Mathf.Lerp(35.0f, 100.0f, interestValence/10.0f)) < 32.0f)
							{
								if (lookAwaySide == "right")
								{
									eyesSM.Switch(eBlink);
									eyeClock = 0.0f;
								}
								lookAwaySide = "left";
							}
							else
							{
								if (lookAwaySide == "left")
								{
									eyesSM.Switch(eBlink);
									eyeClock = 0.0f;
								}
								lookAwaySide = "right";
							}
						}
					}
				}
				//SuperController.LogError("Gaze Avoid Done");
			}
			else
			{
				gAvoid = 0.0f;
				gAvoidanceClock = 0.0f;
				gAvoidingClock = 0.0f;
			}
			
			if (((playerLHandToPelvis < interactionDistance * 1.5f) || (playerRHandToPelvis < interactionDistance * 1.5f) || playerHeadToPelvis < interactionDistance * 1.5f) && vagTouchCount > 0)
			{
				interestArousal += 0.15f;
				if (interestArousal > 6.0f)
				{
					if (currentLook != "Sex")
					{
						lookSM.Switch(lSex);
					}
				}
				else
				{
					if (currentLook != "Feel")
					{
						lookSM.Switch(lFeel);
					}
				}

			}
			
			if ((playerLHandToHead < closeFaceDistance || playerRHandToHead < closeFaceDistance) && lipsTouchCount > 0.0f)
			{
				lipsOnly = true;
				if (currentMouth != "Kissing")
				{
					mouthSM.Switch(mKiss);
					
				}
			}
			else
			{
				lipsOnly = false;
			}

			//SuperController.LogError("Checking for face Idle");
			if (morphBrowAction == false && morphEyeAction == false && morphMouthAction == false && Random.Range(0.0f,100.0f) > Mathf.Lerp(99.99f,98.0f,interestArousal/10.0f) && currentLook != "Sex" && currentLook != "Sucking" && currentLook != "Kissing")
			{
				if (amGlancing == false && gAvoid == 0.0f && playerPenisInteract && playerHeadToHead > personalSpaceDistance/2.0f)
				{
					if (playerTipToHead < interactionDistance && lipsTouchCount > 0.0f)
					{
						if (uiDoBlowjob.val)
						{
							lookSM.Switch(lSucking);
						}
					}
					else
					{
						if (uiDoSex.val)
						{
							lookSM.Switch(lSex);
						}
					}
				}
				else
				{
					if (interestArousal < 5.0f)
					{
						if (interestValence < 5.0f)
						{
							lookSM.SwitchRandom(new State[] {
											lCasual,
											lCasual,
											lCasual,
											lCasual,
											lInquisitive,
											lInquisitive,
											lInquisitive,
											lBored,
											lBored,
											lDayDream,
											lDayDream,
											lDayDream,
										});
						}
						else
						{
							lookSM.SwitchRandom(new State[] {
											lCasual,
											lCasual,
											lCasual,
											lCasual,
											lInquisitive,
											lInquisitive,
											lInquisitive,
											lInquisitive,
											lInquisitive,
											lPlayful,
										});
						}
					}
					else
					{
						if (interestValence < 5.0f)
						{
							lookSM.SwitchRandom(new State[] {
											lCasual,
											lCasual,
											lInquisitive,
											lInquisitive,
											lInquisitive,
											lPlayful,
											lPlayful,
											lPlayful,
											lPlayful,
										});
						}
						else
						{
							if (interestArousal > 8.0f && interestValence > 8.0f && playerHeadToFaceRot > lookDirectAngle)
							{
								lookSM.SwitchRandom(new State[] {
											lFeel,
											lFeel,
											lPlayful
										});
							}
							else
							{
								lookSM.SwitchRandom(new State[] {
											lFeel,
											lInquisitive,
											lInquisitive,
											lInquisitive,
											lInquisitive,
											lPlayful,
											lPlayful,
											lPlayful,
											lPlayful,
											lPlayful,
											lIntense,
											lIntense
										});
							}
						}
					}
				}
				
			}
			
			//SuperController.LogError("Checking for motion interact");
			if (morphMouthAction == false && interestArousal >= 8.5f && interestValence > 6.0f && currentLook != "Feel" && currentLook != "Sex" && currentLook != "Sucking" && currentLook != "Kissing")
			{
				if ((playerLHandInteract && playerLHandMovement) || (playerRHandInteract && playerRHandMovement) || (playerHeadInteract && playerHeadMovement) || (playerPenisInteract && playerTipMovement))
				{
					lookSM.Switch(lFeel);
				}
			}

			//SuperController.LogError("Checking for Blowjob look timeout");
            if (lookAction == false)
            {
                if (playerTipToHead < interactionDistance * 1.05f && uiDoBlowjob.val)
                {
                    lookSM.Switch(lSucking);
					if (mainInterest != "Tip" && mainInterest != "Pelvis")
					{
						mainInterest = "Tip";
						interestTip = 100.0f;
					}
                }
            }

			//SuperController.LogError("Calculating mood decay");
            if (currentInterestLevel > 70.0f)
            {
                playerInterest += (currentInterestLevel - 70.0f) / 500.0f;
            }
            else
            {
                playerInterest -= 0.05f;
            }
            playerInterest = Mathf.Clamp(playerInterest, 0.0f, 100.0f);


            interestArousal = Mathf.Clamp(interestArousal - (0.002f * uiMoodSpeed.val), 3.0f, 10.0f);
			if (playerHeadToHead > personalSpaceDistance)
			{
				interestValence = Mathf.Clamp(interestValence - (0.0081f * uiMoodSpeed.val), 2.0f, 10.0f);
			}
			else
			{
				interestValence = Mathf.Clamp(interestValence - (0.0043f * uiMoodSpeed.val), 2.0f, 10.0f);
			}


			//SuperController.LogError("Look Position start");
            if (currentInterest == "RandomF")
            {
                //headController.transform.LookAt(randomPointForward);
                lookAtPosition = randomPointForward;
                eyeController.transform.position = randomPointForward;
            }
            if (currentInterest == "Target")
            {
                //headController.transform.LookAt(randomPointForward);
				if (emTargetName == "[CameraRig]")
				{
					lookAtPosition = CameraTarget.centerTarget.transform.position;
					eyeController.transform.position = CameraTarget.centerTarget.transform.position;
				}
				else
				{
					lookAtPosition = emTargetController.transform.position;
					eyeController.transform.position = emTargetController.transform.position;
				}
            }
            if (currentInterest == "RandomL")
            {
                //headController.transform.LookAt(randomPointLeft);
                lookAtPosition = randomPointLeft;
                eyeController.transform.position = randomPointLeft;
            }
            if (currentInterest == "RandomR")
            {
                //headController.transform.LookAt(randomPointRight);
                lookAtPosition = randomPointRight;
                eyeController.transform.position = randomPointRight;
            }
            if (currentInterest == "RandomU")
            {
                //headController.transform.LookAt(randomPointUp);
                lookAtPosition = randomPointUp;
                eyeController.transform.position = randomPointUp;
            }
            if (currentInterest == "Face")
            {
				if (playerHeadToHead > personalSpaceDistance)
				{
					if (usePerson2 && person2Usable)
					{
						lookAtPosition = playerHeadTransform.TransformPoint(new Vector3(0.0f, 0.04f, 0.07f));
					}
					else
					{
						lookAtPosition = playerHeadTransform.position;
					}
				}
				else
				{
					if (usePerson2 && person2Usable)
					{
						lookAtPosition = playerHeadTransform.TransformPoint(new Vector3(0.0f, Mathf.Lerp(0.0f,0.04f,Mathf.Clamp((playerHeadToHead-kissingDistance)/personalSpaceDistance,0.0f,1.0f)), 0.07f));
					}
					else
					{
						lookAtPosition = playerHeadTransform.TransformPoint(new Vector3(0.0f, -0.02f, 0.0f));
					}
				}
				fuzzyLock = 1.0f;
            }
            if (currentInterest == "Pelvis" && usePerson2 && person2Usable)
            {
                lookAtPosition = playerPelvisController.followWhenOff.TransformPoint(new Vector3(0.0f, 0.0f, 0.0f));
                fuzzyLock = 2.0f;
            }
            if (currentInterest == "LHand" && (usePerson2 || playerHandsUsable))
            {
				if (playerLHandToHead < closeFaceDistance)
				{
					lookAtPosition = playerLHandTransform.TransformPoint(new Vector3(0.2f, 0.0f, 0.2f));
				}
				else
				{
					lookAtPosition = playerLHandTransform.TransformPoint(new Vector3(-0.1f, 0.0f, 0.0f));
				}
                fuzzyLock = 5.0f;
            }
			
            if (currentInterest == "RHand" && (usePerson2 || playerHandsUsable))
            {
				if (playerRHandToHead < closeFaceDistance)
				{
					lookAtPosition = playerRHandTransform.TransformPoint(new Vector3(-0.2f, 0.0f, 0.2f));
				}
				else
				{
					lookAtPosition = playerRHandTransform.TransformPoint(new Vector3(0.1f, 0.0f, 0.0f));
				}
                fuzzyLock = 5.0f;
            }
            if (currentInterest == "Tip" && usePerson2 && person2Usable)
            {
                if (playerTipToHead < personalSpaceDistance)// && Random.Range(0.0f,100.0f) > pExtraversion + 25.0f)
                {
                    if (Mathf.Abs(headToFaceRot) <= lookNoAwarenessAngle && interestValence > 8.0f && lipsTouchCount > 1.0f)
                    {
                        lookAtPosition = playerHeadTransform.TransformPoint(new Vector3(0.0f, 0.04f, 0.07f));
                        fuzzyLock = 3.0f;
						interestValence -= 0.5f;
                    }
                    else
                    {
						if (playerTipToHead > 0.08f || lipsTouchCount <= 1.0f)
						{
							lookAtPosition = playerTipController.followWhenOff.TransformPoint(new Vector3(0.0f, 0.02f, 0.0f));
						}
						else
						{
							lookAtPosition = playerTipBaseController.followWhenOff.TransformPoint(new Vector3(0.0f, 0.1f, -0.3f));
						}
                    }
                }
                else
                {
					lookAtPosition = playerTipController.followWhenOff.position;
                    fuzzyLock = 2.0f;
                }
            }
            if (secondClock <= 0.0f && amGlancing)
            {
                amGlancing = false;
            }
			
			if (interestKissing && uiDoKiss.val)
			{
				tempFloat = 0.0f;
				if (usePerson2 == false || person2Usable == false)
				{
					tempFloat = 0.0f;
				}
				lookAtPosition = playerHeadTransform.TransformPoint(new Vector3(0.0f,0.00f,0.07f));
				if (gHeadRoll > 10.0f)
				{
					//lookAtPosition = playerHeadTransform.TransformPoint(new Vector3(0.01f, 0.00f, tempFloat));
				}
				if (gHeadRoll < -10.0f)
				{
					//lookAtPosition = playerHeadTransform.TransformPoint(new Vector3(-0.01f, 0.00f, tempFloat));
				}
				fuzzyLock = 0.0f;
				if (eyesSM.CurrentState != eClosed)
				{
					//eyesSM.Switch(eClosed);
				}
			}
			//SuperController.LogError("Look Position Done");
		
			
			if (playerHeadToHead > personalSpaceDistance && playerInterest <= 10.0f && amGlancing == false && interestKissing == false)
			{
				//gAvoid = 1.0f;
			}
			
			//SuperController.LogError("BlowJob / Sex Check");
			if (amGlancing == false && gAvoid == 0.0f && playerPenisInteract && lookAction == false && playerHeadToHead > personalSpaceDistance/2.0f)
			{
				if (playerTipToHead < interactionDistance)
				{
					if (uiDoBlowjob.val && currentLook != "Sucking")
					{
						lookSM.Switch(lSucking);
					}
				}
				else
				{
					if (uiDoSex.val && currentLook != "Sex")
					{
						lookSM.Switch(lSex);
					}
				}
			}

			tempFloat = 1.0f;
			if (amGlancing == true)
			{
				tempFloat = 0.0f;
				saccadeOffset = new Vector3(0.0f, 0.0f, 0.0f);
			}
			if (glanceTimeout >= 0.0f)
			{
				glanceTimeout -= Time.fixedDeltaTime;
			}
			//SuperController.LogError("Eye Position start");
            //			if (((Random.Range(0.0f,100000.0f) / 1000.0f > Mathf.Clamp(100.00f - ((100.0f-pStableness)/100.0f),99.15f,99.899f) || secondClock > 0.0f) && mainClock > Mathf.Clamp(6.0f * ((100.0f-pExtraversion)/100),1.5f,5.0f) && playerHeadToHead > closeFaceDistance) || amGlancing == true)
            if (((Random.Range(0.0f, 100.0f) > Mathf.Lerp(50.2f,96.95f,pStableness/100.0f) || (gAvoid == 1.0f && Random.Range(0.0f, 100.0f) > Mathf.Lerp(20.2f,56.95f,pStableness/100.0f))) || secondClock > 0.0f || amGlancing == true) && (glanceTimeout <= 0.0f || amGlancing == true) && uiGazeGlance.val)// && mainInterest == mainOld && secondInterest != "RandomL" && secondInterest != "RandomR")
            //if (1.0f == 1.0f)
			{
				//SuperController.LogError("Glancing at Second");
				if (mainInterest == secondInterest)
				{
					if (lookAwaySide == "Left")
					{
						eyeController.transform.position = randomPointLeft;
						eyeController.transform.rotation = chestController.followWhenOff.rotation;
						eyeController.transform.Translate(0.0f, -1.0f, 0.0f);
						amGlancing = true;
					}
					else
					{
						eyeController.transform.position = randomPointRight;
						eyeController.transform.rotation = chestController.followWhenOff.rotation;
						eyeController.transform.Translate(0.0f, -1.0f, 0.0f);
						amGlancing = true;
					}
				}
				
                if (secondInterest == "RandomL")
                {
                    //lookAtPosition = playerPelvisController.transform.TransformPoint(new Vector3(0.0f,0.0f,0.0f));
                    eyeController.transform.position = randomPointLeft;
					eyeController.transform.rotation = chestController.followWhenOff.rotation;
					eyeController.transform.Translate(0.0f, -1.0f, 0.0f);
                    amGlancing = true;
                    //fuzzyLock = 3.5f;
                }
                if (secondInterest == "RandomR")
                {
                    //lookAtPosition = playerPelvisController.transform.TransformPoint(new Vector3(0.0f,0.0f,0.0f));
                    eyeController.transform.position = randomPointRight;
					eyeController.transform.rotation = chestController.followWhenOff.rotation;
					eyeController.transform.Translate(0.0f, -1.0f, 0.0f);
                    amGlancing = true;
                    //fuzzyLock = 3.5f;
                }
                if (secondInterest == "RandomU")
                {
                    //lookAtPosition = playerPelvisController.transform.TransformPoint(new Vector3(0.0f,0.0f,0.0f));
                    eyeController.transform.position = playerPelvisController.followWhenOff.position;
                    amGlancing = true;
                    //fuzzyLock = 3.5f;
                }
                if (secondInterest == "RandomF")
                {
                    //lookAtPosition = playerPelvisController.transform.TransformPoint(new Vector3(0.0f,0.0f,0.0f));
                    eyeController.transform.position = randomPointForward;
					eyeController.transform.rotation = chestController.followWhenOff.rotation;
					eyeController.transform.Translate(0.0f, -1.0f, 0.0f);
                    amGlancing = true;
                    //fuzzyLock = 3.5f;
                }
				if (secondInterest == secondOld && secondInterest != "RandomF")
				{
                    eyeController.transform.position = randomPointForward;
                    amGlancing = true;
				}
                if (secondInterest == "Face")
                {
                    //if (gAvoid == 0.0f)
                    //{
                    //lookAtPosition = playerHeadController.transform.TransformPoint(new Vector3(0.0f,0.0f,0.0f));
                    //}

					if (person2Usable)
					{
						eyeController.transform.position = playerHeadTransform.TransformPoint(new Vector3(0.0f, 0.04f, 0.07f));
						eyeController.transform.rotation = playerHeadTransform.rotation;
						eyeController.transform.Translate(0.0f, 0.03f, 0.07f);
					}
					else
					{
						eyeController.transform.position = playerHeadTransform.TransformPoint(new Vector3(0.0f, 0.00f, 0.00f));
					}
                    amGlancing = true;
                    //fuzzyLock = 1.5f;
                }
                if ((secondInterest == "LHand" && interestLHand > 30.0f) || (secondInterest == "RandomL" && interestLHand > 10.0f))
                {
                    //lookAtPosition = playerLHandController.transform.TransformPoint(new Vector3(0.0f,0.0f,0.0f));
					eyeController.transform.position = playerLHandTransform.TransformPoint(new Vector3(-0.1f, 0.0f, 0.0f));

                    amGlancing = true;
                    //fuzzyLock = 2.5f;
                }
                if ((secondInterest == "RHand" && interestRHand > 30.0f) || (secondInterest == "RandomR" && interestRHand > 10.0f))
                {
                    //lookAtPosition = playerRHandController.transform.TransformPoint(new Vector3(0.0f,0.0f,0.0f));
                    eyeController.transform.position = playerRHandTransform.TransformPoint(new Vector3(0.1f, 0.0f, 0.0f));

                    amGlancing = true;
                    //fuzzyLock = 2.5f;
                }
                if (secondInterest == "Pelvis" && usePerson2)
                {
                    //lookAtPosition = playerPelvisController.transform.TransformPoint(new Vector3(0.0f,0.0f,0.0f));
                    eyeController.transform.position = playerPelvis;
                    eyeController.transform.rotation = chestController.followWhenOff.rotation;
                    eyeController.transform.Translate(0.0f, 0.0f, 1.0f);
                    amGlancing = true;
                    //fuzzyLock = 3.5f;
                }
                if (secondInterest == "Tip" && headToTipRot < lookNoAwarenessAngle && usePerson2)
                {
                    //lookAtPosition = playerTipController.transform.TransformPoint(new Vector3(0.0f,0.0f,0.0f));
                    eyeController.transform.position = playerTip;
                    eyeController.transform.rotation = chestController.followWhenOff.rotation;
                    eyeController.transform.Translate(0.0f, 0.0f, 1.0f);
                    amGlancing = true;
                    //fuzzyLock = 3.5f;
                }
				if (gAvoid == 1.0f)
				{
					//secondClock = 0.0f;//Time.fixedDeltaTime;
					glanceTimeout = 0.0f;
					if (gAvoidInterest == "Face")
					{
						if (person2Usable)
						{
							eyeController.transform.position = playerHeadTransform.TransformPoint(new Vector3(0.0f, 0.04f, 0.07f));
							eyeController.transform.rotation = playerHeadTransform.rotation;
							eyeController.transform.Translate(0.0f, 0.03f, 0.07f);
						}
						else
						{
							eyeController.transform.position = playerHeadTransform.position;
						}
						amGlancing = true;
					}
					if (gAvoidInterest == "Face")
					{
						if (person2Usable)
						{
							eyeController.transform.position = playerHeadTransform.TransformPoint(new Vector3(0.0f, 0.04f, 0.07f));
							eyeController.transform.rotation = playerHeadTransform.rotation;
							eyeController.transform.Translate(0.0f, 0.03f, 0.07f);
						}
						else
						{
							eyeController.transform.position = playerHeadTransform.position;
						}
						amGlancing = true;
					}
					if (gAvoidInterest == "LHand")
					{
						eyeController.transform.position = playerLHandTransform.TransformPoint(new Vector3(-0.1f, 0.0f, 0.0f));
						amGlancing = true;
					}
					if (gAvoidInterest == "RHand")
					{
						eyeController.transform.position = playerRHandTransform.TransformPoint(new Vector3(0.1f, 0.0f, 0.0f));
						amGlancing = true;
					}
					if ((gAvoidInterest == "Pelvis" || gAvoidInterest == "Tip") && usePerson2)
					{
						eyeController.transform.position = playerPelvis;
						amGlancing = true;
					}
				}
                if (secondClock <= 0.0f && amGlancing)
                {
                    secondClock = Random.Range(0.35f, 0.65f + (1.0f * ((100.0f - pExtraversion) / 100.0f)));
                    ////LogError("Glancing");
                }
                if (amGlancing == false)
                {
                    secondClock = 0.0f;
                }
                else
                {
					if (tempFloat == 1.0f && eyeClock > (1.25f * uiBlinkSpeed.val) && currentEye != "Closed")
					{
						eyesSM.Switch(eBlink);
						eyeClock = 0.0f;
					}
					gHeadSpeed = 2.0f;
					glanceTimeout = Mathf.Lerp(2.0f,10.0f,pExtraversion/100.0f);
                }
            }
            else
            {
				//SuperController.LogError("Looking at main");
                gHeadSpeed = 1.25f;
                if (mainInterest == "Face" && (gAvoid != 1.0f || Random.Range(0.0f, 1.00f) < interestArousal / 10.0f))
                {
					if (person2Usable)
					{
						eyeController.transform.position = playerHeadTransform.TransformPoint(new Vector3(0.0f, 0.04f, 0.07f));
					}
					else
					{
						eyeController.transform.position = playerHeadTransform.position;
					}
                    eyeController.transform.rotation = playerHeadTransform.rotation;

                }
                if (mainInterest == "Pelvis" && usePerson2 && person2Usable)
                {
                    if (playerTipToHead < closeFaceDistance || headToTipRot > 45.0f)
                    {
						eyeController.transform.position = playerPelvisController.followWhenOff.position;
                        if (playerTipToHead < interactionDistance || headToTipRot > 45.0f)
                        {
							if (mainInterest == "Face" || mainOld == "Face" || secondInterest == "Face" || secondOld == "Face")
							{
								eyeController.transform.rotation = playerHeadTransform.rotation;
								eyeController.transform.position = playerHeadTransform.TransformPoint(new Vector3(0.0f, 0.04f, 0.07f));
							}
							if (mainInterest == "LHand" || mainOld == "LHand" || secondInterest == "LHand" || secondOld == "LHand")
							{
								eyeController.transform.rotation = playerLHandTransform.rotation;
								eyeController.transform.position = playerLHandTransform.TransformPoint(new Vector3(-0.1f, 0.0f, 0.0f));
							}
							if (mainInterest == "RHand" || mainOld == "RHand" || secondInterest == "RHand" || secondOld == "RHand")
							{
								eyeController.transform.rotation = playerLHandTransform.rotation;
								eyeController.transform.position = playerRHandTransform.TransformPoint(new Vector3(0.1f, 0.0f, 0.0f));
							}
                        }
                        else
                        {
                            eyeController.transform.position = playerPelvis;
                        }
                    }
                    else
                    {
                        eyeController.transform.position = playerPelvis;
                    }
                    
                }
                if (mainInterest == "LHand" && (usePerson2 || playerHandsUsable))
                {
                    //if (playerLHandToHead < closeFaceDistance)
                    //{
                    //    eyeController.transform.position = playerHeadTransform.position;
                    //    eyeController.transform.rotation = playerHeadTransform.rotation;
                    //    eyeController.transform.Translate(0.0f, 0.03f, 0.07f);
                    //}
                    //else
                    //{
                        eyeController.transform.position = playerLHandTransform.TransformPoint(new Vector3(-0.1f, 0.0f, 0.0f));
                        eyeController.transform.rotation = playerLHandTransform.rotation;
                        //eyeController.transform.Translate(-0.1f, 0.0f, 0.0f);
                    //}
                }
                if (mainInterest == "RHand" && (usePerson2 || playerHandsUsable))
                {
                    //if (playerRHandToHead < minFaceDistance)
                    //{
                    //    eyeController.transform.position = playerHeadTransform.position;
                    //    eyeController.transform.rotation = playerHeadTransform.rotation;
                    //    eyeController.transform.Translate(0.0f, 0.03f, 0.07f);
                    //}
                    //else
                    //{
                        eyeController.transform.position = playerRHandTransform.TransformPoint(new Vector3(0.1f, 0.0f, 0.0f));
                        eyeController.transform.rotation = playerRHandTransform.rotation;
                        //eyeController.transform.Translate(0.1f, 0.0f, 0.0f);
                    //}
                }
                if (mainInterest == "Tip" && usePerson2 && person2Usable)
                {
                    if (playerTipToHead < closeFaceDistance || headToTipRot > 45.0f)
                    {
                        if (playerTipToHead < interactionDistance || headToTipRot > 45.0f)
                        {
                            eyeController.transform.rotation = playerHeadTransform.rotation;
                            eyeController.transform.position = playerHeadTransform.TransformPoint(new Vector3(0.0f, 0.04f, 0.07f));
                        }
                        else
                        {
                            eyeController.transform.position = playerTip;
                            //eyeController.transform.rotation = playerTipController.followWhenOff.rotation;
                            //eyeController.transform.Translate(0.0f, 0.05f, -0.17f);
                        }
                    }
                    else
                    {
                        eyeController.transform.position = playerTip;
                    }
                }
            }
				
            Vector3 centrePoint = chestController.followWhenOff.position;
            Vector3 eyePoint = eyeController.transform.position;
            float c2eActual = Vector3.Distance(centrePoint, eyePoint);
            centrePoint.y = 0.0f;
            eyePoint.y = 0.0f;
            float c2eDist = Vector3.Distance(centrePoint, eyePoint);
            if (c2eDist < closeFaceDistance * 3.0f)
            {
                eyeController.transform.position = eyeController.transform.position + (chestController.followWhenOff.forward * 0.1f);
            }

            secondClock = Mathf.Clamp(secondClock - Time.fixedDeltaTime, 0.0f, 10.0f);
            mainClock += Time.fixedDeltaTime;
            if (mainClock > 7.0f && mainSwitch)
            {
                mainSwitch = false;
            }
            if (secondClock <= 0.0f && secondSwitch)
            {
                secondSwitch = false;
            }


			
			if ((playerLHandInteract && playerRHandInteract) || playerHeadInteract)
			{
				if (playerHeadToFaceRot > lookDirectAngle && playerHeadToHead > closeFaceDistance * 1.2f)// && (playerLHandMovement || playerRHandMovement))
				{
					if (currentLook != "Intense")
					{
						gAvoid = 1.0f;
					}
				}
			}
			//SuperController.LogError("Eye Position Done");
			
			if (gAvoid == 1.0f && interestKissing == false)
			{
				//SuperController.LogError("Doing avoidance");
				//interestArousal += 0.0006f;
				//SuperController.LogMessage("Avoiding" + gAvoidance, false);
				//saccadeOffset = new Vector3(0.0f, 0.0f, 0.0f);
				if (lookAwaySide == "right")
				{
					//mainOld = mainInterest;
					mainInterest = "RandomR";
					lookAtPosition = randomPointRight;//playerRHandTransform.TransformPoint(new Vector3(0.0f, 0.0f, 0.0f));
					fuzzyLock = 2.0f;
					if (amGlancing == false)
					{
						eyeController.transform.position = randomPointRight;//playerRHandTransform.position;
					}
					gHeadRollTarget = Random.Range(-25.0f,-5.0f);
				}
				else
				{
					//mainOld = mainInterest;
					mainInterest = "RandomL";
					lookAtPosition = randomPointLeft;//playerLHandTransform.TransformPoint(new Vector3(0.0f, 0.0f, 0.0f));
					fuzzyLock = 2.0f;
					if (amGlancing == false)
					{
						eyeController.transform.position = randomPointLeft;//playerLHandTransform.position;
					}
					gHeadRollTarget = Random.Range(25.0f,5.0f);
				}
			}
			
			if (playerLHandToHead < closeFaceDistance || playerRHandToHead < closeFaceDistance)
				{
				eyeController.transform.position = playerHeadTransform.TransformPoint(new Vector3(0.0f, 0.04f, 0.07f));
                eyeController.transform.rotation = playerHeadTransform.rotation;
				}

			if (mEyesClosedLeftValue > 0.9f && morphBlinking == false)
			{
				//eyeController.transform.position = headController.transform.TransformPoint(new Vector3(0.0f, 0.00f, 1.0f));
			}
			
			if (Vector3.Distance(headController.followWhenOff.position, eyeController.transform.position) < closeFaceDistance * 2.7f && currentEye != "Closed" && (mainInterest == "LHand" || mainInterest == "RHand"))
			{
				eyesSM.Switch(eClosed);
			}

			if (interestKissing && uiDoKiss.val)
			{
				fuzzyLock = 0.0f;
				gHeadSpeed = 0.2f;
			}
			
			if ((playerHeadToHead <= personalSpaceDistance / 2.0f && mainInterest == "Face") || currentLook == "Intense")
			{
				//fuzzyLock = 0.25f;
				//gHeadSpeed = 0.7f;
				//peronalityAdjustH = peronalityAdjustH * 0.995f;
				//peronalityAdjustV = peronalityAdjustV * 0.995f;
			}
			//SuperController.LogError("Interaction checks Done");

			
			if (playerHeadToHead <= Mathf.Max(personalSpaceDistance / 3.0f,closeFaceDistance*2.0f) && currentLook != "Sucking" && currentLook != "Kissing" && mainInterest == "Face")
			{
				tempFloat = playerHeadController.followWhenOff.eulerAngles.z - headController.followWhenOff.eulerAngles.z;
				if (tempFloat > 180.0f){tempFloat -= 360.0f;}
				if (tempFloat < -180.0f){tempFloat += 360.0f;}

				gHeadRollTarget = Mathf.Clamp(tempFloat + Random.Range(-25.0f,25.0f),-40.0f,40.0f);
			}
			//SuperController.LogError("Blink calculation");
			
            eyeController.transform.Translate(saccadeOffset * (Vector3.Distance(headController.followWhenOff.position, eyeController.transform.position) / 100.0f));
			
			tempFloat = Vector3.Distance(oldEyePos, eyeController.transform.position);
		    if ((tempFloat > 0.02f && eyeClock > (1.65f * uiBlinkSpeed.val)) || (tempFloat > 0.3f && eyeClock > (0.35f * uiBlinkSpeed.val)))
			{
				if (currentEye != "Closed" && morphEyeAction == false && interestKissing == false)
				{
                    eyesSM.Switch(eBlink);
                    eyeClock = 0.0f;
				}
			}
			
            if (interestKissing == false)
            {
                //if (((Random.Range(0.0f, (((15.0f - (10.0f - interestArousal)) / (2.0f * (interestArousal))) * (1.0f / Random.Range(1.333f, 2.5f))) * Time.fixedDeltaTime) / 300.0f) - (Mathf.Max(eyeClock - 1.0f, 0.0f) / 5000000.0f) <= 0.0f) && eyeClock > 1.0f)
                //if (((Random.Range(0.0f,(1.0f / Random.Range(1.333f,3.5f)) * Time.fixedDeltaTime) / 10.0f) - (Mathf.Max(eyeClock-1.0f,0.0f) / 5000000.0f) <= 0.0f) && eyeClock > 0.5f)
                if (eyeClock > 2.0f * uiBlinkSpeed.val && Random.Range(0.0f,100.0f) > Mathf.Lerp(99.4f - (interestValence / 100.0f),99.7f - (interestArousal / 100.0f),pExtraversion)) 
				{
                    eyeClock = 0.0f;
                    eyesSM.Switch(eBlink);
                }
            }
			if (eyeClock > 0.3f && morphBlinking && currentEye != "Closed")
			{
				morphBlinking = false;
				eyesSM.Switch(eOpen);
				eyeClock = 0.0f;
			}
			eyeClock += Time.fixedDeltaTime;

            //eyeController.transform.Translate(randomX,randomY,0.0f);
            if (eyeUpdateClock >= eyeUpdateTime || (mainInterest == "Face" && playerHeadMovement && playerHeadToHead > personalSpaceDistance / 2.0f) || (mainInterest == "LHand" && playerLHandMovement) || (mainInterest == "RHand" && playerRHandMovement))
            {
                curEyePosition = eyeController.transform.position;
                curEyeAngles = eyeController.transform.eulerAngles;
                eyeUpdateClock = 0.0f;
            }
            else
            {
                eyeUpdateClock += Time.fixedDeltaTime;
                eyeController.transform.position = curEyePosition;
                eyeController.transform.eulerAngles = curEyeAngles;
            }
			
			//SuperController.LogError("Calculating Head Rotation");
            Transform head = headController.transform;
            Transform reference = chestController.followWhenOff;

            Vector3 actualDir = reference.InverseTransformDirection(head.forward);
            Vector3 targetDir = lookAtPosition - head.position;
            targetDir.Normalize();
            targetDir = reference.InverseTransformDirection(targetDir);
            Vector2 actualDirH = new Vector2(actualDir.x, actualDir.z);
            Vector2 targetDirH = new Vector2(targetDir.x, targetDir.z);
            Vector2 actualDirV = new Vector2(actualDirH.magnitude, actualDir.y);
            Vector2 targetDirV = new Vector2(targetDirH.magnitude, targetDir.y);
            actualDirH.Normalize();
            targetDirH.Normalize();
            actualDirV.Normalize();
            targetDirV.Normalize();
            float actualH = Mathf.Atan2(actualDirH.x, actualDirH.y);
            float targetH = Mathf.Atan2(targetDirH.x, targetDirH.y);
            float actualV = Mathf.Atan2(actualDirV.y, actualDirV.x);
            float targetV = Mathf.Atan2(targetDirV.y, targetDirV.x);


            float headToEyeController = Mathf.Abs(Vector3.Angle(eyeController.transform.position - headController.followWhenOff.position, headController.followWhenOff.forward));
            if (headToEyeController > eyesNonDirectAngle)
            {
                eyesNonDirectClock += Time.fixedDeltaTime * Mathf.Lerp(1.5f, 6.0f, interestValence / 10.0f);
            }
            else
            {
                eyesNonDirectClock = 0.0f;
            }
			if (headToEyeController > 70.0f && amGlancing == false)
			{
				//eyesSM.Switch(eClosed);
			}


            if ((10.0f - ((100.0f - pExtraversion) / 10.0f) - eyesNonDirectClock < 0.0f || currentLook == "Intense") && interestKissing == false)// || playerHeadToHead < closeFaceDistance * 2.0f)
            {
				peronalityAdjustH = peronalityAdjustH * 0.995f;
				peronalityAdjustV = peronalityAdjustV * 0.995f;
                fuzzyLock = fuzzyLock * 0.95f;
				//eyesNonDirectClock = 0.0f;
            }
			
			if (currentLook == "Sucking")
			{
				peronalityAdjustH = 0.0f;
				peronalityAdjustV = 0.0f;
                fuzzyLock = 0.0f;
			}
			
			if (mainInterest == "Tip")
			{
				fuzzyLock = 0.0f;
				gHeadSpeed = gHeadSpeed / 5.0f;
			}
			
            // adjust angles
			
			if (uiDoHead.val)
			{
				
				//SuperController.LogError("Adjusting head rotation");
				peronalityAdjustH = Mathf.Clamp(peronalityAdjustH,-30.0f * Mathf.Deg2Rad,30.0f * Mathf.Deg2Rad);
				peronalityAdjustV = Mathf.Clamp(peronalityAdjustV,-20.0f * Mathf.Deg2Rad,20.0f * Mathf.Deg2Rad);
				
				tempFloat = peronalityAdjustH;
				if (Mathf.Abs(peronalityAdjustH - lastAdjustH) > 10.0f * Mathf.Deg2Rad)
				{
					if (lastAdjustH < peronalityAdjustH)
					{
						//peronalityAdjustH = lastAdjustH + 10.0f * Mathf.Deg2Rad;
					}
					else
					{
						//peronalityAdjustH = lastAdjustH - 10.0f * Mathf.Deg2Rad;
					}
				}
				lastAdjustH = tempFloat;
				lastAdjustV = peronalityAdjustV;
				tempFloat2 = Mathf.Clamp(playerHeadToHead - 0.1f, 0.0f, 1.0f);
				tempFloat = Mathf.Lerp(60.00f,64.0f, tempFloat2);
				targetH = Mathf.Clamp(targetH + (peronalityAdjustH * uiGazeVariation.val), -tempFloat * Mathf.Deg2Rad, tempFloat * Mathf.Deg2Rad);
				tempFloat = Mathf.Lerp(75.00f,79.0f, tempFloat2);
				targetV = Mathf.Clamp(targetV + (peronalityAdjustV * uiGazeVariation.val), -tempFloat * Mathf.Deg2Rad, (tempFloat*1.25f) * Mathf.Deg2Rad);
				adjustedSpeed = Mathf.Clamp((Mathf.Lerp(gHeadSpeed * 1.00f,gHeadSpeed*2.0f,((interestArousal+interestValence)/2.0f)/10.0f)) / uiGazeSpeed.val,0.1f, 30.0f); // - 0.5f + (closeFaceDistance / playerHeadToHead)
				if (gAvoid == 1.0f || (currentLook == "Sex" && playerHeadToHead < personalSpaceDistance / 2.0f))
				{
					adjustedSpeed = adjustedSpeed * Mathf.Lerp(3.0f,10.0f,pExtraversion/100.0f);
				}
				if (mainInterest == "RandomR" || mainInterest == "RandomL" || mainInterest == "RandomU" || mainInterest == "RandomF")
				{
					adjustedSpeed = adjustedSpeed * 2.0f;
				}
				if (lipsTouchCount > 0.0f && playerHeadToHead > kissingDistance * 2.0f && (playerLHandToHead < closeFaceDistance || playerRHandToHead < closeFaceDistance))
				{
					adjustedSpeed = adjustedSpeed * 10.0f;
				}
				/*
				if (Mathf.Abs(actualH - targetH) < (5.0f * (pAgreeableness / 100.0f) * fuzzyLock) * Mathf.Deg2Rad)// || (Mathf.Abs(actualH - targetH) > 65.0f * Mathf.Deg2Rad && Mathf.Abs(actualH) < Mathf.Abs(targetH)))// || eyesSM.CurrentState == eClosed)
				{
					targetH = actualH + (velocityH * 0.5f);
				}
				if (Mathf.Abs(actualV - targetV) < (2.0f * (pExtraversion / 100.0f) * fuzzyLock) * Mathf.Deg2Rad)// || (Mathf.Abs(actualV - targetV) > 55.0f * Mathf.Deg2Rad && Mathf.Abs(actualV) < Mathf.Abs(targetV)))// || eyesSM.CurrentState == eClosed)
				{
					targetV = actualV + (velocityV * 0.5f);
				}*/
				if (Mathf.Abs(actualH - targetH) > (5.0f * (pAgreeableness / 100.0f) * fuzzyLock) * Mathf.Deg2Rad)// || (Mathf.Abs(actualH - targetH) > 65.0f * Mathf.Deg2Rad && Mathf.Abs(actualH) < Mathf.Abs(targetH)))// || eyesSM.CurrentState == eClosed)
				{
					actualH = Mathf.SmoothDamp(actualH, targetH, ref velocityH, adjustedSpeed, Mathf.Infinity, Time.fixedDeltaTime);
				}
				if (Mathf.Abs(actualV - targetV) > (2.0f * (pExtraversion / 100.0f) * fuzzyLock) * Mathf.Deg2Rad)// || (Mathf.Abs(actualV - targetV) > 55.0f * Mathf.Deg2Rad && Mathf.Abs(actualV) < Mathf.Abs(targetV)))// || eyesSM.CurrentState == eClosed)
				{
					actualV = Mathf.SmoothDamp(actualV, targetV, ref velocityV, adjustedSpeed, Mathf.Infinity, Time.fixedDeltaTime);
				}
				
				Mathf.Clamp(actualH,-65.0f * Mathf.Rad2Deg,65.0f * Mathf.Rad2Deg);
				Mathf.Clamp(actualV,-80.0f * Mathf.Rad2Deg,80.0f * Mathf.Rad2Deg);
				
				

				// recombine
				actualDir = RecombineDirection(actualH, actualV);
				targetDir = RecombineDirection(targetH, targetV);
				actualDir = reference.TransformDirection(actualDir);

				//head.eulerAngles.z = curRotation.z;
				head.transform.LookAt(head.transform.position + actualDir, headController.followWhenOff.position - chestController.followWhenOff.position);
				
				if ((peronalityAdjustH < 0.0f && gHeadRollTarget < 0.0f) || (peronalityAdjustH > 0.0f && gHeadRollTarget > 0.0f))
				{
					//gHeadRollTarget = -gHeadRollTarget;
				}
				tempFloat = (400.0f / uiRollSpeed.val) / (Mathf.Max(Mathf.Abs(gHeadRollTarget - gHeadRoll), 1.0f) / 10.0f);
				if (interestKissing || currentMouth == "Sucking")
				{
					tempFloat = 100.0f / uiRollSpeed.val;
				}
				if (gAvoid == 1.0f)
				{
					tempFloat = tempFloat * 2.0f;
				}
				if (gHeadRoll < gHeadRollTarget)
				{
					gHeadRoll = Mathf.Min(gHeadRoll + (((gHeadRollTarget) - gHeadRoll) / tempFloat), 45.0f);
				}
				if (gHeadRoll > gHeadRollTarget)
				{
					gHeadRoll = Mathf.Max(gHeadRoll - ((gHeadRoll - (gHeadRollTarget)) / tempFloat), -45.0f); // * (interestValence/10.0f)
				}
				
				

				// apply roll
				Vector3 eulerAngles = head.transform.localEulerAngles;
				eulerAngles.z += gHeadRoll;
				head.transform.localEulerAngles = eulerAngles;

				//SuperController.LogError("Adjusting neck rotation");

				eulerAngles = head.transform.eulerAngles;
				Vector3 forwardEuler = chestController.transform.eulerAngles;
				Vector3 difference = forwardEuler - eulerAngles;

				//neckController.transform.localEulerAngles = difference;
				//neckController.transform.eulerAngles = eulerAngles;
				//Vector3 newDir = Vector3.RotateTowards(neckController.transform.forward, headController.followWhenOff.forward, Mathf.Lerp(0.1f,2.5f,interestValence/10.0f) / 1000.0f, 0.0f);
				//neckController.transform.rotation = Quaternion.LookRotation(newDir);
				//neckController.transform.Rotate((sexActionNeckX / 2.0f) * Time.deltaTime,0.0f,0.0f);
				Vector3 newDir = Vector3.RotateTowards(neckController.transform.forward, headController.followWhenOff.forward, Mathf.Lerp(1.1f,70.5f,interestValence/10.0f) / 100.0f, 0.0f);
				neckController.transform.rotation = Quaternion.LookRotation(newDir);
				
				neckController.transform.eulerAngles = eulerAngles;
				eulerAngles = neckController.transform.localEulerAngles;
				//eulerAngles.z = eulerAngles.z / 2.0f;
				//eulerAngles.x = eulerAngles.x / 2.0f;
				//eulerAngles.x += sexActionNeckX / 5.0f;
				eulerAngles.z -= gHeadRoll * Mathf.Lerp(0.6f,0.2f,interestValence/10.0f);
				neckController.transform.localEulerAngles = eulerAngles;
				//eulerAngles = neckController.transform.eulerAngles;
				//neckController.transform.eulerAngles = eulerAngles;
				
			   // eulerAngles = headController.transform.eulerAngles;
			   // eulerAngles.x += sexActionNeckX / 5.0f;
			   // headController.transform.eulerAngles = eulerAngles;
			}
			else
			{
				peronalityAdjustH = 0.0f;
				peronalityAdjustV = 0.0f;
				gHeadRoll = 0.0f;
			}

				if ((playerLHandToHead < closeFaceDistance && playerLHandMovement) || (playerRHandToHead < closeFaceDistance && playerRHandMovement))
					{
						if (eyeClock > (2.75f * uiBlinkSpeed.val) && morphBlinking == false && currentEye != "Closed")
						{
						eyesSM.Switch(eClosed);
						}
					}
			
			if (morphMouthAction == false)
			{
				currentMouth = "Idle";
			}
			if (morphEyeAction == false)
			{
				currentEye = "Idle";
			}
			if (morphBrowAction == false)
			{
				currentBrow = "Idle";
			}
			if (lookAction == false)
			{
				currentLook = "Idle";
			}
			
			if (emTargetName != "None")
			{
				//emTargetPosPrev = emTargetController.transform.position;
			}
			//SuperController.LogError("Fixed Update Complete");
			
			
			if (uiShowStats.val)
			{
			//SuperController.LogError("Doing Message Stats");
			SuperController.singleton.ClearMessages();
			Vector3 tempAngles = playerHeadTransform.eulerAngles;
			SuperController.LogMessage("Current Emotion :" + currentLook, false);
			SuperController.LogMessage("Brow State :" + currentBrow, false);
			SuperController.LogMessage("Eye State :" + currentEye, false);
			SuperController.LogMessage("Mouth State :" + currentMouth, false);
			SuperController.LogMessage("", false);
			SuperController.LogMessage("Arousal : " + Round(interestArousal), false);
			SuperController.LogMessage("Happiness : " + Round(interestValence), false);
			SuperController.LogMessage("", false);
			SuperController.LogMessage("Interest Clock : " + Round(interestClock), false);
			SuperController.LogMessage("Main Interest : " + mainInterest + "(" + mainOld + ")", false);
			SuperController.LogMessage("Second Interest : " + secondInterest + "(" + secondOld + ")", false);
			SuperController.LogMessage("Player Interest : " + Round(playerInterest), false);
			SuperController.LogMessage("", false);
			SuperController.LogMessage("Face (" + Round(playerHeadToHead) + "): " + Round(interestFace), false);
			SuperController.LogMessage("Inf : " + dbgHead, false);
			SuperController.LogMessage("LHand (" + Round(playerLHandToHead) + "): " + Round(interestLHand), false);
			SuperController.LogMessage("Inf : " + dbgLHand, false);
			SuperController.LogMessage("RHand (" + Round(playerRHandToHead) + "): " + Round(interestRHand), false);
			SuperController.LogMessage("Inf : " + dbgRHand, false);
			SuperController.LogMessage("Pelvis (" + Round(playerPelvisToHead) + "): " + Round(interestPelvis), false);
			SuperController.LogMessage("  Tip (" + Round(playerTipToHead) + "): " + Round(interestTip), false);
			SuperController.LogMessage("Inf : " + dbgPenis, false);
			SuperController.LogMessage("Target (" + Round(emTargetDistance) + "): " + Round(interestEMTarget), false);
			SuperController.LogMessage("Inf : " + dbgObject, false);
			SuperController.LogMessage("", false);
			SuperController.LogMessage("Penis to Pelvis Distance : " + Round(playerTipToPelvis), false);
			SuperController.LogMessage("Eye Contact  -Buildup:" + Round(gAvoidanceClock) + " -TimeOut:" + Round(gAvoidingClock), false);
			SuperController.LogMessage("Person Face->Eye Angle : " + Round(headToEyeController), false);
			SuperController.LogMessage("Person->Player Face Angle : " + Round(headToFaceRot), false);
			SuperController.LogMessage("Player->Person Face Angle : " + Round(playerHeadToFaceRot), false);
			SuperController.LogMessage("Eye/Saccade Clock : " + Round(eyeClock) + "/" + Round(saccadeClock), false);
			SuperController.LogMessage("Saccade : " + debugString, false);
			if (morphBlinking)
			{
				SuperController.LogMessage("Eye State : Blinking", false);
			}
			else
			{
				if (mEyesClosedLeftValue > 0.8f)
				{
					SuperController.LogMessage("Eye State : Closed", false);
				}
				else
				{
					SuperController.LogMessage("Eye State : Open", false);
				}
			}
			if (amGlancing)
			{
				SuperController.LogMessage("Gaze : Glancing", false);
			}
			else
			{
				if (gAvoid == 1.0f)
				{
					SuperController.LogMessage("Gaze : Avoiding " + gAvoidInterest, false);
				}
				else
				{
					if (headToEyeController > lookDirectAngle)
					{
						SuperController.LogMessage("Gaze : Indirect", false);
					}
					else
					{
						SuperController.LogMessage("Gaze : Direct", false);
					}
				}
			}
			SuperController.LogMessage("Gaze Variation Horizontal : " + Round(peronalityAdjustH * Mathf.Rad2Deg), false);
			SuperController.LogMessage("Gaze Variation Vertical : " + Round(peronalityAdjustV * Mathf.Rad2Deg), false);
			SuperController.LogMessage("Gaze Head Roll : " + Round(gHeadRoll) + "(" + Round(gHeadRollTarget) + ")", false);
			SuperController.LogMessage("Gaze Neck Adjust : " + Round(sexActionNeckX), false);
			SuperController.LogMessage("Gaze Fuzzy Lock : " + fuzzyLock, false);
			SuperController.LogMessage("Target Vert : " + Round(targetV * Mathf.Rad2Deg) + "| Hor : " + Round(targetH * Mathf.Rad2Deg) + "| Speed : " + Round(adjustedSpeed), false);
			SuperController.LogMessage("Actual Vert : " + Round(actualV * Mathf.Rad2Deg) + "| Hor : " + Round(actualH * Mathf.Rad2Deg) + "| H Speed : " + Round(velocityH) + "| V Speed : " + Round(velocityV), false);
			SuperController.LogMessage("", false);
			SuperController.LogMessage("Lips Touch Count : " + lipsTouchCount, false);
			SuperController.LogMessage("V Touch Count : " + vagTouchCount, false);
			SuperController.LogMessage("", false);
			SuperController.LogMessage("Flirt Morph : " + Round(mFlirtingValue), false);
			SuperController.LogMessage("Happy Morph : " + Round(mHappyValue), false);
			SuperController.LogMessage("Excitement Morph : " + Round(mExcitementValue), false);
			SuperController.LogMessage("Glare Morph : " + Round(mGlareValue), false);
			SuperController.LogMessage("Brow Center Up Morph : " + Round(mBrowCenterUpValue), false);
			SuperController.LogMessage("Brow Down Morph : " + Round(mBrowDownValue), false);
			SuperController.LogMessage("Brow Outer Up Left Morph : " + Round(mBrowOuterUpLeftValue), false);
			SuperController.LogMessage("Brow Outer Up Right Morph : " + Round(mBrowOuterUpRightValue), false);
			SuperController.LogMessage("Brow All Up Morph : " + Round(mBrowUpValue), false);
			SuperController.LogMessage("Left Eye Closed Morph : " + Round(mEyesClosedLeftValue), false);
			SuperController.LogMessage("Right Eye Closed Morph : " + Round(mEyesClosedRightValue), false);
			SuperController.LogMessage("Eyes Squint Morph : " + Round(mEyesSquintValue), false);
			SuperController.LogMessage("Smile Full Face Morph : " + Round(mSmileFullFaceValue), false);
			SuperController.LogMessage("Smile Open Full Face Morph : " + Round(mSmileOpenFullFaceValue), false);
			SuperController.LogMessage("Smile Simple Left Morph : " + Round(mSmileSimpleLeftValue), false);
			SuperController.LogMessage("Smile Simple Right Morph : " + Round(mSmileSimpleRightValue), false);
			SuperController.LogMessage("Mouth Narrow Morph : " + Round(mMouthNarrowValue), false);
			SuperController.LogMessage("Mouth Open Morph : " + Round(mMouthOpenValue), false);
			SuperController.LogMessage("Mouth Open Wide Morph : " + Round(mMouthOpenWideValue), false);
			SuperController.LogMessage("Mouth Side Left Morph : " + Round(mMouthSideLeftValue), false);
			SuperController.LogMessage("Mouth Side Right Morph : " + Round(mMouthSideRightValue), false);
			SuperController.LogMessage("Lips Pucker Morph : " + Round(mLipsPuckerValue), false);
			SuperController.LogMessage("Lips Pucker Wide Morph : " + Round(mLipsPuckerWideValue), false);
			SuperController.LogMessage("Cheeks Sink Morph : " + Round(mCheekSinkValue), false);
			SuperController.LogMessage("Ten Strip Blowjob Lips Morph : " + Round(mBlowjobLipsValue), false);
			SuperController.LogMessage("Ten Strip Deserving It Morph : " + Round(mDeserveItValue), false);
			SuperController.LogMessage("Ten Strip Taking It Morph : " + Round(mTakingItValue), false);
			
			}
        }

        //System States
        private static State sUpdate = new SUpdate();
        private static State sUpdatePerson = new SUpdatePerson();
        private static State sUpdatePlayer = new SUpdatePlayer();
        private static State sUpdatePlayerHands = new SUpdatePlayerHands();
        private static State sUpdatePerson2 = new SUpdatePerson2();
		private static State sReselectPerson2 = new SReselectPerson2();

        //Emotion States
        //private static State emHappy = new EMHappy();
        //private static State emPleased = new EMPleased();
        //private static State emDistracted = new EMDistracted();
        //private static State emWaiting = new EMWaiting();
        //private static State emBored = new EMBored();

        //Reaction States
        //private static State rExcited = new RExcited();
        //private static State rSuprised = new RSuprised();
        //private static State rConcentrate = new RConcentrate();
        //private static State rFrustrated = new RFrustrated();
        //private static State rAnticipate = new RAnticipate();
        //private static State rShy = new RShy();

        //Attention States
        private static State lIntense = new LIntense();
        private static State lInquisitive = new LInquisitive();
        private static State lCasual = new LCasual();
        private static State lBored = new LBored();
        private static State lDayDream = new LDayDream();
        private static State lPlayful = new LPlayful();
        private static State lFeel = new LFeel();
        private static State lKissing = new LKissing();
        private static State lSucking = new LSucking();
        private static State lSex = new LSex();


        //Brow States
        private static State bRaised = new BRaised();
        private static State bLowered = new BLowered();
        private static State bConcentrate = new BConcentrate();
        private static State bOneRaise = new BOneRaise();
        private static State bApprehensive = new BApprehensive();

        //Eye States
        private static State eBlink = new EBlink();
        private static State eOpen = new EOpen();
        private static State eClosed = new EClosed();
        private static State eFocus = new EFocus();
        private static State eSquint = new ESquint();
        private static State eWide = new EWide();
        private static State eWink = new EWink();

        //Mouth States
        private static State mOpen = new MOpen();
        private static State mClosed = new MClosed();
        private static State mBiteLip = new MBiteLip();
        private static State mSmile = new MSmile();
        private static State mBigSmile = new MBigSmile();
        private static State mSmirk = new MSmirk();
        private static State mSideways = new MSideways();
        private static State mKiss = new MKiss();
        private static State mSuck = new MSuck();
        private static State mJoy = new MJoy();

        private class SUpdate : State
        {
            public override void OnEnter()
            {
                Duration = 0.05f;
                //Vector3 perp = Vector3.Cross(chestController.followWhenOff.eulerAngles, refAngle);
                //float dir = Vector3.Dot(perp, chestController.followWhenOff.up);
				tempFloat = Mathf.Abs(Vector3.Angle(refAngle, chestController.followWhenOff.forward));
                bool resetDir = false;
                if (Mathf.Abs(tempFloat) > 60.0f || allSetup == false)
                {
                    resetDir = true;
                    refAngle = chestController.followWhenOff.forward;
                }

                if (Random.Range(0.0f, 100.0f) > 98.5f || resetDir)
                {
                    //generate random points
                    float rad = Random.Range(-15.0f, 15.0f) * Mathf.Deg2Rad;
                    Vector3 position = chestController.followWhenOff.right * Mathf.Sin(rad) + chestController.followWhenOff.forward * Mathf.Cos(rad);
                    randomPointForward = chestController.followWhenOff.position + (chestController.followWhenOff.right * Random.Range(-3.0f, 3.0f)) + (chestController.followWhenOff.up * Random.Range(2.0f, -1.5f)) + (chestController.followWhenOff.forward * 5.0f);
                }
                if (Random.Range(0.0f, 100.0f) > 98.5f || resetDir)
                {
                    //generate random points
                    float rad = Random.Range(15.0f, 65.0f) * Mathf.Deg2Rad;
                    Vector3 position = chestController.followWhenOff.right * Mathf.Sin(rad) + chestController.followWhenOff.up * Mathf.Cos(rad);
                    //randomPointLeft = chestController.followWhenOff.position + position * Random.Range(1.0f, 4.0f);
					randomPointLeft = chestController.followWhenOff.position + (chestController.followWhenOff.right * (-1.0f * Random.Range(1.75f, 7.0f))) + (chestController.followWhenOff.up * Random.Range(0.0f, -1.5f)) + (chestController.followWhenOff.forward * 2.0f);
                    //randomPointLeft = chestController.followWhenOff.position - (chestController.followWhenOff.right * 30.0f) - (chestController.followWhenOff.up * 7.0f) + (chestController.followWhenOff.forward * 30.0f);
                }
                if (Random.Range(0.0f, 100.0f) > 98.5f || resetDir)
                {
                    //generate random points
                    float rad = Random.Range(15.0f, 65.0f) * Mathf.Deg2Rad;
                    Vector3 position = chestController.followWhenOff.right * Mathf.Sin(rad) + chestController.followWhenOff.forward * Mathf.Cos(rad);
                    //randomPointRight = chestController.followWhenOff.position + position * Random.Range(1.0f, 4.0f) - (chestController.followWhenOff.up * 2.0f) + (chestController.followWhenOff.forward * 3.0f);
                    randomPointRight = chestController.followWhenOff.position + (chestController.followWhenOff.right * (1.0f * Random.Range(1.75f, 7.0f))) + (chestController.followWhenOff.up * Random.Range(0.0f, -1.5f)) + (chestController.followWhenOff.forward * 2.0f);
                }
                if (Random.Range(0.0f, 100.0f) > 98.5f || resetDir)
                {
                    //generate random points
                    float rad = Random.Range(0.0f, -5.0f) * Mathf.Deg2Rad;
                    Vector3 position = chestController.followWhenOff.up * Mathf.Sin(rad) + chestController.followWhenOff.forward * Mathf.Cos(rad);
                    randomPointUp = chestController.followWhenOff.position + (chestController.followWhenOff.right * Random.Range(-2.0f, 2.0f)) + (chestController.followWhenOff.up * Random.Range(-3.0f, 1.0f)) + (chestController.followWhenOff.forward * 7.0f);
                }

            }

            public override void OnTimeout()
            {
                systemSM.Switch(sUpdatePerson);
            }
        }

        private class SUpdatePerson : State
        {
            public override void OnEnter()
            {
                Duration = 0.05f;
                //Person to Player/Person2 update
                personHeadTransform = headController.followWhenOff; //headController.transform;

				if (usePerson2 && person2Usable)
				{
					headToFaceRot = Mathf.Abs(Vector3.Angle(playerHeadTransform.TransformPoint(new Vector3(0.0f, 0.04f, 0.07f)) - personHeadTransform.TransformPoint(new Vector3(0.0f, 0.04f, 0.07f)), personHeadTransform.forward));
					playerHeadToFaceRot = Mathf.Abs(Vector3.Angle(personHeadTransform.TransformPoint(new Vector3(0.0f, 0.04f, 0.07f)) - playerHeadTransform.TransformPoint(new Vector3(0.0f, 0.04f, 0.07f)), playerHeadTransform.forward));
				}
				else
				{
					headToFaceRot = Mathf.Abs(Vector3.Angle(playerHeadTransform.TransformPoint(new Vector3(0.0f, 0.00f, 0.00f)) - personHeadTransform.TransformPoint(new Vector3(0.0f, 0.04f, 0.07f)), personHeadTransform.forward));
					playerHeadToFaceRot = Mathf.Abs(Vector3.Angle(personHeadTransform.TransformPoint(new Vector3(0.0f, 0.04f, 0.07f)) - playerHeadTransform.TransformPoint(new Vector3(0.0f, 0.00f, 0.00f)), playerHeadTransform.forward));
				}
                headToChestRot = Mathf.Abs(Vector3.Angle(playerChest - personHeadTransform.TransformPoint(new Vector3(0.0f, 0.04f, 0.07f)), personHeadTransform.forward));
				if (emTargetName == "[CameraRig]")
				{
					emTargetDir = Mathf.Abs(Vector3.Angle(CameraTarget.centerTarget.transform.position - personHeadTransform.position, personHeadTransform.forward));
					emTargetHeadDir = Mathf.Abs(Vector3.Angle(personHeadTransform.position - CameraTarget.centerTarget.transform.position, CameraTarget.centerTarget.transform.forward));
					emTargetDistance = Vector3.Distance(personHeadTransform.position, CameraTarget.centerTarget.transform.position);
				}
				else
				{
					if (emTarget != null)
					{
						emTargetDir = Mathf.Abs(Vector3.Angle(emTargetController.transform.position - personHeadTransform.position, personHeadTransform.forward));
						emTargetHeadDir = Mathf.Abs(Vector3.Angle(personHeadTransform.position - emTargetController.transform.position, emTargetController.transform.forward));
						emTargetDistance = Vector3.Distance(personHeadTransform.position, emTargetController.transform.position);
					}
				}
				
				
                if (Vector3.Distance(playerFace, playerFacePrev) > minHeadMotion || Vector3.Angle(playerFaceRot, playerFaceRotPrev) > 5.0f)
                {
                    playerHeadMovement = true;
                    playerHeadTimeout = Mathf.Min(playerHeadTimeout + movementModifier, movementMaxTimeout);
                }
                else
                {
                    playerHeadTimeout = Mathf.Max(playerHeadTimeout - movementFalloff, 0.0f);
                    if (playerHeadTimeout == 0.0f)
                    {
                        playerHeadMovement = false;
                    }
                }
                if (playerHandsUsable || person2Usable)
                {
                    headToLHandRot = Mathf.Abs(Vector3.Angle(playerLHand - personHeadTransform.position, personHeadTransform.forward));
                    headToRHandRot = Mathf.Abs(Vector3.Angle(playerRHand - personHeadTransform.position, personHeadTransform.forward));
                }
                if (person2Usable)
                {
                    headToPelvisRot = Mathf.Abs(Vector3.Angle(playerPelvis - personHeadTransform.position, personHeadTransform.forward));
                    headToTipRot = Mathf.Abs(Vector3.Angle(playerTip - personHeadTransform.position, personHeadTransform.forward));
                }
            }
            public override void OnTimeout()
            {
                systemSM.Switch(sUpdatePerson2);
            }
        }
		
        private class SUpdatePerson2 : State
        {
            public override void OnEnter()
            {
                Duration = 0.05f;
                if (usePerson2 && person2Usable)
                {
                    playerFacePrev = playerFace;
                    playerFace = playerHeadTransform.position;
                    playerFaceRotPrev = playerFaceRot;
                    playerFaceRot = playerHeadTransform.eulerAngles;
                    playerChest = playerChestController.followWhenOff.position;
                    playerLHandPrev = playerLHand;
                    playerLHand = playerLHandController.followWhenOff.position;
                    playerRHandPrev = playerRHand;
                    playerRHand = playerRHandController.followWhenOff.position;
                    playerPelvis = playerPelvisController.followWhenOff.position;
                    //playerTipPrev = playerTip;
                    playerTip = playerTipController.followWhenOff.position;
                    playerTipBase = playerTipBaseController.followWhenOff.position;
                }
                else
                {
                    playerFacePrev = playerFace;
                    playerFace = player.TransformPoint(new Vector3(-0.3f, 0.0f, 0.0f));
                    playerFaceRotPrev = playerFaceRot;
                    playerFaceRot = player.eulerAngles;
                    playerChest = new Vector3(playerFace.x, playerFace.y - 1.0f, playerFace.z); //REDO THIS
                    playerLHandPrev = playerLHand;
                    playerRHandPrev = playerRHand;
                    if (person2Usable && playerHandsUsable == false)
                    {
                        playerLHand = playerLHandController.followWhenOff.position;
                        playerRHand = playerRHandController.followWhenOff.position;
                    }
                    if (playerHandsUsable && (usePerson2 == false || person2Usable == false))
                    {
                        playerLHand = playerVRLHand.position;
                        playerRHand = playerVRRHand.position;
                    }
                    if (person2Usable == false && playerHandsUsable == false)
                    {
                        playerLHand = playerFace;
                        playerRHand = playerFace;
                    }
                    if (person2Usable && usePerson2)
                    {
                        playerPelvis = playerPelvisController.followWhenOff.position;
                        //playerTipPrev = playerTip;
                        playerTip = playerTipController.followWhenOff.position;
                        playerTipBase = playerTipBaseController.followWhenOff.position;
                    }
					else
					{
                        playerPelvis = playerFace;
                        //playerTipPrev = playerTip;
                        playerTip = playerFace;
                        playerTipBase = playerFace;
					}
                }
                playerGround = new Vector3(playerFace.x, 0.0f, playerFace.z);
            }
            public override void OnTimeout()
            {
                systemSM.Switch(sUpdatePlayer);
				allSetup = true;
            }
        }
		
        private class SUpdatePlayer : State
        {
            public override void OnEnter()
            {
                Duration = 0.05f;
                //Player/Person2 to Person update
                if (usePerson2)
                {
                    playerHeadTransform = playerHeadController.followWhenOff;
					person2Usable = true;
                }
                else
                {
                    playerHeadTransform = CameraTarget.centerTarget.transform;
					person2Usable = false;
                }

                personChestToHead = Mathf.Abs(Vector3.Angle(playerFace - chestController.followWhenOff.position, chestController.followWhenOff.forward));

                playerToHead = Mathf.Abs(Vector3.Angle(headController.followWhenOff.position - playerFace, playerHeadTransform.forward));
                playerToPelvis = Mathf.Abs(Vector3.Angle(pelvisController.followWhenOff.position - playerFace, playerHeadTransform.forward));
                playerToLHand = Mathf.Abs(Vector3.Angle(lHandController.followWhenOff.position - playerFace, playerHeadTransform.forward));
                playerToRHand = Mathf.Abs(Vector3.Angle(rHandController.followWhenOff.position - playerFace, playerHeadTransform.forward));
                playerToLFoot = Mathf.Abs(Vector3.Angle(lFootController.followWhenOff.position - playerFace, playerHeadTransform.forward));
                playerToRFoot = Mathf.Abs(Vector3.Angle(rFootController.followWhenOff.position - playerFace, playerHeadTransform.forward));

                playerHeadToHead = Vector3.Distance(headController.followWhenOff.position, playerFace);
                playerHeadToLHand = Vector3.Distance(lHandController.followWhenOff.position, playerFace);
                playerHeadToRHand = Vector3.Distance(rHandController.followWhenOff.position, playerFace);
				if (lBreastController != null)
				{
					playerHeadToLBreast = Vector3.Distance(lBreastController.followWhenOff.position, playerFace);
					playerHeadToRBreast = Vector3.Distance(rBreastController.followWhenOff.position, playerFace);
					playerToLBreast = Mathf.Abs(Vector3.Angle(lBreastController.followWhenOff.position - playerFace, playerHeadTransform.forward));
					playerToRBreast = Mathf.Abs(Vector3.Angle(rBreastController.followWhenOff.position - playerFace, playerHeadTransform.forward));
				}
				else
				{
					playerHeadToLBreast = 999.0f;
					playerHeadToRBreast = 999.0f;
					playerToLBreast = 180.0f;
					playerToRBreast = 180.0f;
				}
                playerHeadToPelvis = Vector3.Distance(pelvisController.followWhenOff.position, playerFace);
                if (person2Usable)
                {
                    playerPelvisToHead = Vector3.Distance(headController.followWhenOff.position, playerPelvis);
					//playerTipToHeadLast = playerTipToHead;
					if (lBreastController != null)
					{
						playerTipToHead = Vector3.Distance(headController.followWhenOff.position, playerTip);
						playerTipToLHand = Vector3.Distance(lHandController.followWhenOff.position, playerTip);
						playerTipToRHand = Vector3.Distance(rHandController.followWhenOff.position, playerTip);
						if (lBreastController != null)
						{
							playerTipToLBreast = Vector3.Distance(lBreastController.followWhenOff.position, playerTip);
							playerTipToRBreast = Vector3.Distance(rBreastController.followWhenOff.position, playerTip);
						}
						else
						{
							playerTipToLBreast = 999.0f;
							playerTipToRBreast = 999.0f;
						}
						playerTipToPelvis = Vector3.Distance(pelvisController.followWhenOff.position, playerTip);
						if (Vector3.Distance(playerTip, playerTipPrev) > minTipMotion / 2.0f)
						{
							playerTipMovement = true;
							playerTipTimeout = Mathf.Min(playerTipTimeout + movementModifier, movementMaxTimeout);
						}
						else
						{
							playerTipTimeout = Mathf.Max(playerTipTimeout - movementFalloff, 0.0f);
							if (playerTipTimeout == 0.0f)
							{
								playerTipMovement = false;
							}
						}
					}
					else
					{
						playerTipToHead = 999.0f;
						playerTipToLHand = 999.0f;
						playerTipToRHand = 999.0f;
						playerTipToLBreast = 999.0f;
						playerTipToRBreast = 999.0f;
						playerTipToPelvis = 999.0f;
						playerTipMovement = false;
						playerTipTimeout = 0.0f;
					}
                }
				else
				{
                    playerPelvisToHead = 999.0f;
                    playerTipToHead = 999.0f;
                    playerTipToLHand = 999.0f;
                    playerTipToRHand = 999.0f;
                    playerTipToLBreast = 999.0f;
                    playerTipToRBreast = 999.0f;
                    playerTipToPelvis = 999.0f;
					playerTipMovement = false;
					playerTipTimeout = 0.0f;
				}
            }
            public override void OnTimeout()
            {
                systemSM.Switch(sUpdatePlayerHands);
            }
        }

        private class SUpdatePlayerHands : State
        {
            public override void OnEnter()
            {
                Duration = 0.05f;
                //Player/Person2 hands to Person update

                if (playerHandsUsable || (person2Usable && usePerson2))
                {
                    if (playerHandsUsable)
                    {
                        playerLHandTransform = playerVRLHand;
                        playerRHandTransform = playerVRRHand;
                    }
                    if (usePerson2 || (playerHandsUsable == false && person2Usable))
                    {
                        playerLHandTransform = playerLHandController.followWhenOff;
                        playerRHandTransform = playerRHandController.followWhenOff;
                    }
                    if (person2Usable == false && playerHandsUsable == false)
                    {
                        playerLHandTransform = playerHeadTransform;
                        playerRHandTransform = playerHeadTransform;
                    }
                    personChestToLHand = Mathf.Abs(Vector3.Angle(playerLHandTransform.position - chestController.followWhenOff.position, chestController.followWhenOff.forward));
                    personChestToRHand = Mathf.Abs(Vector3.Angle(playerRHandTransform.position - chestController.followWhenOff.position, chestController.followWhenOff.forward));

                    playerToPLHand = Mathf.Abs(Vector3.Angle(playerLHandTransform.position - playerHeadTransform.position, playerHeadTransform.forward));
                    playerToPRHand = Mathf.Abs(Vector3.Angle(playerRHandTransform.position - playerHeadTransform.position, playerHeadTransform.forward));
                    playerLHandToHead = Vector3.Distance(headController.followWhenOff.position, playerLHandTransform.TransformPoint(new Vector3(-0.1f,0.0f,0.0f)));
                    playerRHandToHead = Vector3.Distance(headController.followWhenOff.position, playerRHandTransform.TransformPoint(new Vector3(0.1f,0.0f,0.0f)));
                    playerLHandToLHand = Vector3.Distance(lHandController.followWhenOff.position, playerLHandTransform.TransformPoint(new Vector3(-0.1f,0.0f,0.0f)));
                    playerRHandToLHand = Vector3.Distance(lHandController.followWhenOff.position, playerRHandTransform.TransformPoint(new Vector3(0.1f,0.0f,0.0f)));
                    playerLHandToRHand = Vector3.Distance(rHandController.followWhenOff.position, playerLHandTransform.TransformPoint(new Vector3(-0.1f,0.0f,0.0f)));
                    playerRHandToRHand = Vector3.Distance(rHandController.followWhenOff.position, playerRHandTransform.TransformPoint(new Vector3(0.1f,0.0f,0.0f)));
					if (lBreastController != null)
					{
						playerLHandToLBreast = Vector3.Distance(lBreastController.followWhenOff.position, playerLHandTransform.TransformPoint(new Vector3(-0.1f,0.0f,0.0f)));
						playerRHandToLBreast = Vector3.Distance(lBreastController.followWhenOff.position, playerRHandTransform.TransformPoint(new Vector3(0.1f,0.0f,0.0f)));
						playerLHandToRBreast = Vector3.Distance(rBreastController.followWhenOff.position, playerLHandTransform.TransformPoint(new Vector3(-0.1f,0.0f,0.0f)));
						playerRHandToRBreast = Vector3.Distance(rBreastController.followWhenOff.position, playerRHandTransform.TransformPoint(new Vector3(0.1f,0.0f,0.0f)));
					}
					else
					{
						playerLHandToLBreast = 999.0f;
						playerRHandToLBreast = 999.0f;
						playerLHandToRBreast = 999.0f;
						playerRHandToRBreast = 999.0f;
					}
                    playerLHandToPelvis = Vector3.Distance(pelvisController.followWhenOff.position, playerLHandTransform.TransformPoint(new Vector3(-0.1f,0.0f,0.0f)));
                    playerRHandToPelvis = Vector3.Distance(pelvisController.followWhenOff.position, playerRHandTransform.TransformPoint(new Vector3(0.1f,0.0f,0.0f)));
                    if (person2Usable == false && playerHandsUsable == false)
                    {
						playerToPLHand = 180.0f;
						playerToPRHand = 180.0f;
						playerLHandToHead = 999.0f;
						playerRHandToHead = 999.0f;
						playerLHandToLHand = 999.0f;
						playerRHandToLHand = 999.0f;
						playerLHandToRHand = 999.0f;
						playerRHandToRHand = 999.0f;
						playerLHandToLBreast = 999.0f;
						playerRHandToLBreast = 999.0f;
						playerLHandToRBreast = 999.0f;
						playerRHandToRBreast = 999.0f;
						playerLHandToPelvis = 999.0f;
						playerRHandToPelvis = 999.0f;
                    }
                }
            }
            public override void OnTimeout()
            {
                systemSM.Switch(sUpdate);
            }
        }
		
		
        private class SReselectPerson2 : State
        {
            public override void OnEnter()
            {
				Duration = 0.1f;
				person2Usable = false;
				
				if (person2 != null)
				{
					JSONStorable js = person2.GetStorableByID("geometry");
					DAZCharacterSelector dcs = js as DAZCharacterSelector;
					GenerateDAZMorphsControlUI morphUI = dcs.morphsControlUI;
					if (morphUI != null)
					{
						DAZMorph morphTemp = morphUI.GetMorphByDisplayName("Breast Height");
						person2IsMale = false;
						if (morphTemp == null)
						{
							person2IsMale = true;
						}
					}
					person2Usable = true;
					playerHeadController = person2.GetStorableByID("headControl") as FreeControllerV3;
					playerChestController = person2.GetStorableByID("chestControl") as FreeControllerV3;
					playerLHandController = person2.GetStorableByID("lHandControl") as FreeControllerV3;
					playerRHandController = person2.GetStorableByID("rHandControl") as FreeControllerV3;
					playerPelvisController = person2.GetStorableByID("pelvisControl") as FreeControllerV3;
					playerTipController = person2.GetStorableByID("penisTipControl") as FreeControllerV3;
					playerTipBaseController = person2.GetStorableByID("penisBaseControl") as FreeControllerV3;
					////SuperController.LogError("New Person " + currentAtomName + " Found");
				}
				else
				{
					//SuperController.LogError("New Person " + currentAtomName + " Not Found");
				}
				
				if (usePerson2 && person2Usable)
				{
					playerFace = playerHeadController.followWhenOff.position;
					playerLHand = playerLHandController.followWhenOff.position;
					playerRHand = playerRHandController.followWhenOff.position;
					playerPelvis = playerPelvisController.followWhenOff.position;
					playerTip = playerTipController.followWhenOff.position;
				}
				else
				{
					playerFace = player.position;
					if (person2Usable && usePerson2)
					{
						playerLHand = playerLHandController.followWhenOff.position;
						playerRHand = playerRHandController.followWhenOff.position;
						playerLHandTransform = playerLHandController.followWhenOff;
						playerRHandTransform = playerRHandController.followWhenOff;
					}
					if (playerHandsUsable && usePerson2 == false)
					{
						playerLHand = playerVRLHand.position;
						playerRHand = playerVRRHand.position;
						playerLHandTransform = playerVRLHand;
						playerRHandTransform = playerVRRHand;
					}
					if (person2Usable == false && playerHandsUsable == false)
					{
						playerLHand = playerFace;
						playerRHand = playerFace;
						playerLHandTransform = playerHeadTransform;
						playerRHandTransform = playerHeadTransform;
					}
					playerPelvis = playerFace;
					playerTip = playerFace;
					if (person2Usable)
					{
						playerPelvis = playerPelvisController.followWhenOff.position;
						playerTip = playerTipController.followWhenOff.position;
					}
					else
					{
						//uiUsePerson2.val = false;
					}
				}
				
				if (usePerson2 && person2 != null)
				{
					playerHeadTransform = playerHeadController.followWhenOff;
					closeFaceDistance = closeFaceDistance * 1.85f;
				}
				else
				{
					playerHeadTransform = player;
				}
			}
            public override void OnTimeout()
            {
                systemSM.Switch(sUpdate);
            }		
		}
		
        private Vector3 RecombineDirection(float angleH, float angleV)
        {
            float cosV = Mathf.Cos(angleV);
            return new Vector3(
                Mathf.Sin(angleH) * cosV,
                Mathf.Sin(angleV),
                Mathf.Cos(angleH) * cosV
            );
        }
		
		private void loadDefaults()
		{
			SimpleJSON.JSONNode loadedSettings = new SimpleJSON.JSONClass();
			loadedSettings=SuperController.singleton.LoadJSON("E-Motion_Defaults.json");
			if (loadedSettings != null)
			{
				uiExtraversion.val = loadedSettings["Extraversion"].AsFloat;
				uiAgreeableness.val = loadedSettings["Agreeableness"].AsFloat;
				uiStableness.val = loadedSettings["Stableness"].AsFloat;
				uiBreatheSpeed.val = loadedSettings["Breathing Speed"].AsFloat;
				uiBreatheExpandMultiplier.val = loadedSettings["Breathe Morph Multiplier"].AsFloat;
				uiGazeVariation.val = loadedSettings["Gaze Angle Variation"].AsFloat;
				uiGazeAvoid.val = loadedSettings["Gaze Avoidance Enable"].AsBool;
				uiGazeLookTime.val = loadedSettings["Gaze Look At Time"].AsFloat;
				uiGazeAvoidTime.val = loadedSettings["Gaze Avoid Look Time"].AsFloat;
				uiGazeGlance.val = loadedSettings["Gaze Glance Enable"].AsBool;
				uiRollSpeed.val = loadedSettings["Gaze Head Roll Speed"].AsFloat;
				uiSaccadeSpeed.val = loadedSettings["Eye Saccade Frequency"].AsFloat;
				uiSaccadeAmount.val = loadedSettings["Eye Saccade Movement Scale"].AsFloat;
				uiSaccadeWanderMult.val = loadedSettings["Eye Saccade Max Dist from Target Scale"].AsFloat;
				uiBlinkSpeed.val = loadedSettings["Eye Blink Delay Scale"].AsFloat;
				uiArousalSpeed.val = loadedSettings["Mood Arousal Scale"].AsFloat;
				uiValenceSpeed.val = loadedSettings["Mood Valence Scale"].AsFloat;
				uiMoodSpeed.val = loadedSettings["Mood Change Scale"].AsFloat;
				uiInterestSpeed.val = loadedSettings["Main Interest Switch Delay Scale"].AsFloat;
				uiInterestRate.val = loadedSettings["Global Interest Rate Scale"].AsFloat;
				uiDoHead.val = loadedSettings["Control Head and Neck Movements"].AsBool;
				uiDoMorphs.val = loadedSettings["Control Breath and Expression Morphs"].AsBool;
				uiDoShoulders.val = loadedSettings["Control Shoulder Movements"].AsBool;
				uiDoChest.val = loadedSettings["Control Chest Movement"].AsBool;
				uiChestAmount.val = loadedSettings["Chest Movement Scale"].AsFloat;
				uiDoHands.val = loadedSettings["Control Hand Morphs"].AsBool;
				uiConfigHead.val = loadedSettings["Automatically Configure Head and Neck physics"].AsBool;
				uiUsePerson2.val = loadedSettings["Look At Target Person"].AsBool;
				uiShowStats.val = loadedSettings["Show Debug Info on Message Log"].AsBool;
				uiDoKiss.val = loadedSettings["Kissing Enable"].AsBool;
				uiKissAmount.val = loadedSettings["Kissing Morph Scale"].AsFloat;
				uiDoBlowjob.val = loadedSettings["Blowjob Enable"].AsBool;
				uiBlowjobAmount.val = loadedSettings["Blowjob Morph Scale"].AsFloat;
				uiDoSex.val = loadedSettings["Sex Enable"].AsBool;
				uiSexAmount.val = loadedSettings["Sex Morph Scale"].AsFloat;
				uiFocusTarget.val = loadedSettings["Current Focus Target"];
				uiObjectTarget.val = loadedSettings["Current Object Target"];
				uiTargetLook.val = loadedSettings["Object View Direction Enable"].AsBool;
				uiPersonalSpace.val = loadedSettings["Personal Space Distance"].AsFloat;
				uiDirectGaze.val = loadedSettings["Direct Viewing Angle"].AsFloat;
				uiPeripheralGaze.val = loadedSettings["Peripheral Viewing Angle"].AsFloat;
				uiOutOfGaze.val = loadedSettings["Maximum Viewing Angle"].AsFloat;
				uiCloseToFaceDist.val = loadedSettings["Face Interaction Distance"].AsFloat;
				uiKissingDist.val = loadedSettings["Kissing Activation Distance"].AsFloat;
				uiInteractDist.val = loadedSettings["General Interaction Distance"].AsFloat;
				uiMaxMorphSmile.val = loadedSettings["Maximum Allowed Value For Smile Morphs"].AsFloat;
				uiEyeCloseMaxMorph.val = loadedSettings["Maximum Amount To Close Eyes"].AsFloat;
				uiEyeUpdate.val = loadedSettings["Minimum Time Between Eye Target Movements Excl Saccades"].AsFloat;
				uiHeadInterest.val = loadedSettings["Target Head Interest Rate Scale"].AsFloat;
				uiLHandInterest.val = loadedSettings["Target Left Hand Interest Rate Scale"].AsFloat;
				uiRHandInterest.val = loadedSettings["Target Right Hand Interest Rate Scale"].AsFloat;
				uiPenisInterest.val = loadedSettings["Target Pelvis Interest Rate Scale"].AsFloat;
				uiObjectInterest.val = loadedSettings["Target Object Interest Rate Scale"].AsFloat;
				if (uiFocusTarget.val != "None" && uiUsePerson2.val)
				{
					person2 = SuperController.singleton.GetAtomByUid(uiFocusTarget.val);
					if (person2 != null)
					{
					systemSM.Switch(sReselectPerson2);
					}
				}
				uiLoadDefaults.val = false;
				SuperController.LogError("E-Motion Defaults Loaded successfully!");
			}
		}
		
		private void loadPreset()
		{
			SimpleJSON.JSONNode loadedSettings = new SimpleJSON.JSONClass();
			loadedSettings=SuperController.singleton.LoadJSON("E-Motion_Preset.json");
			uiExtraversion.val = loadedSettings["Extraversion"].AsFloat;
			uiAgreeableness.val = loadedSettings["Agreeableness"].AsFloat;
			uiStableness.val = loadedSettings["Stableness"].AsFloat;
			uiBreatheSpeed.val = loadedSettings["Breathing Speed"].AsFloat;
			uiBreatheExpandMultiplier.val = loadedSettings["Breathe Morph Multiplier"].AsFloat;
			uiGazeVariation.val = loadedSettings["Gaze Angle Variation"].AsFloat;
			uiGazeAvoid.val = loadedSettings["Gaze Avoidance Enable"].AsBool;
			uiGazeLookTime.val = loadedSettings["Gaze Look At Time"].AsFloat;
			uiGazeAvoidTime.val = loadedSettings["Gaze Avoid Look Time"].AsFloat;
			uiGazeGlance.val = loadedSettings["Gaze Glance Enable"].AsBool;
			uiRollSpeed.val = loadedSettings["Gaze Head Roll Speed"].AsFloat;
			uiSaccadeSpeed.val = loadedSettings["Eye Saccade Frequency"].AsFloat;
			uiSaccadeAmount.val = loadedSettings["Eye Saccade Movement Scale"].AsFloat;
			uiSaccadeWanderMult.val = loadedSettings["Eye Saccade Max Dist from Target Scale"].AsFloat;
			uiBlinkSpeed.val = loadedSettings["Eye Blink Delay Scale"].AsFloat;
			uiArousalSpeed.val = loadedSettings["Mood Arousal Scale"].AsFloat;
			uiValenceSpeed.val = loadedSettings["Mood Valence Scale"].AsFloat;
			uiMoodSpeed.val = loadedSettings["Mood Change Scale"].AsFloat;
			uiInterestSpeed.val = loadedSettings["Main Interest Switch Delay Scale"].AsFloat;
			uiInterestRate.val = loadedSettings["Global Interest Rate Scale"].AsFloat;
			uiDoHead.val = loadedSettings["Control Head and Neck Movements"].AsBool;
			uiDoMorphs.val = loadedSettings["Control Breath and Expression Morphs"].AsBool;
			uiDoShoulders.val = loadedSettings["Control Shoulder Movements"].AsBool;
			uiDoChest.val = loadedSettings["Control Chest Movement"].AsBool;
			uiChestAmount.val = loadedSettings["Chest Movement Scale"].AsFloat;
			uiDoHands.val = loadedSettings["Control Hand Morphs"].AsBool;
			uiConfigHead.val = loadedSettings["Automatically Configure Head and Neck physics"].AsBool;
			uiUsePerson2.val = loadedSettings["Look At Target Person"].AsBool;
			uiShowStats.val = loadedSettings["Show Debug Info on Message Log"].AsBool;
			uiDoKiss.val = loadedSettings["Kissing Enable"].AsBool;
			uiKissAmount.val = loadedSettings["Kissing Morph Scale"].AsFloat;
			uiDoBlowjob.val = loadedSettings["Blowjob Enable"].AsBool;
			uiBlowjobAmount.val = loadedSettings["Blowjob Morph Scale"].AsFloat;
			uiDoSex.val = loadedSettings["Sex Enable"].AsBool;
			uiSexAmount.val = loadedSettings["Sex Morph Scale"].AsFloat;
			uiFocusTarget.val = loadedSettings["Current Focus Target"];
			uiObjectTarget.val = loadedSettings["Current Object Target"];
			uiTargetLook.val = loadedSettings["Object View Direction Enable"].AsBool;
			uiPersonalSpace.val = loadedSettings["Personal Space Distance"].AsFloat;
			uiDirectGaze.val = loadedSettings["Direct Viewing Angle"].AsFloat;
			uiPeripheralGaze.val = loadedSettings["Peripheral Viewing Angle"].AsFloat;
			uiOutOfGaze.val = loadedSettings["Maximum Viewing Angle"].AsFloat;
			uiCloseToFaceDist.val = loadedSettings["Face Interaction Distance"].AsFloat;
			uiKissingDist.val = loadedSettings["Kissing Activation Distance"].AsFloat;
			uiInteractDist.val = loadedSettings["General Interaction Distance"].AsFloat;
			uiMaxMorphSmile.val = loadedSettings["Maximum Allowed Value For Smile Morphs"].AsFloat;
			uiEyeCloseMaxMorph.val = loadedSettings["Maximum Amount To Close Eyes"].AsFloat;
			uiEyeUpdate.val = loadedSettings["Minimum Time Between Eye Target Movements Excl Saccades"].AsFloat;
			uiHeadInterest.val = loadedSettings["Target Head Interest Rate Scale"].AsFloat;
			uiLHandInterest.val = loadedSettings["Target Left Hand Interest Rate Scale"].AsFloat;
			uiRHandInterest.val = loadedSettings["Target Right Hand Interest Rate Scale"].AsFloat;
			uiPenisInterest.val = loadedSettings["Target Pelvis Interest Rate Scale"].AsFloat;
			uiObjectInterest.val = loadedSettings["Target Object Interest Rate Scale"].AsFloat;
			uiLoadPreset.val = false;
			if (uiObjectTarget.val != "None")
			{
				emTargetName = uiObjectTarget.val;
				emTarget = SuperController.singleton.GetAtomByUid(uiObjectTarget.val);
				if (emTarget != null)
				{
					if (emTarget.type == "Person")
					{
						emTargetController = emTarget.GetStorableByID("headControl") as FreeControllerV3;
					}
					else
					{
						emTargetController = emTarget.GetStorableByID("control") as FreeControllerV3;
					}
				}
			}
			else
			{
				emTargetName = "None";
				emTarget = null;
				emTargetController = null;
			}
			if (uiFocusTarget.val != "None" && uiUsePerson2.val)
			{
				person2 = SuperController.singleton.GetAtomByUid(uiFocusTarget.val);
				if (person2 != null)
				{
				systemSM.Switch(sReselectPerson2);
				}
			}
			SuperController.LogError("E-Motion Setup Loaded successfully!");
		}

		public void savePreset()
		{
			SimpleJSON.JSONClass mySettings = new SimpleJSON.JSONClass();

			//Add some data                        
			if (uiShowStats.val){mySettings["Show Debug Info on Message Log"] = "True";}else{mySettings["Show Debug Info on Message Log"] = "False";}
			if (uiConfigHead.val){mySettings["Automatically Configure Head and Neck physics"] = "True";}else{mySettings["Automatically Configure Head and Neck physics"] = "False";}
			if (uiDoHead.val){mySettings["Control Head and Neck Movements"] = "True";}else{mySettings["Control Head and Neck Movements"] = "False";}
			if (uiDoHead.val){mySettings["Control Breath and Expression Morphs"] = "True";}else{mySettings["Control Breath and Expression Morphs"] = "False";}
			if (uiDoShoulders.val){mySettings["Control Shoulder Movements"] = "True";}else{mySettings["Control Shoulder Movements"] = "False";}
			mySettings.Add("Shoulder Movement Scale", new SimpleJSON.JSONData(uiShoulderAmount.val));
			if (uiDoChest.val){mySettings["Control Chest Movement"] = "True";}else{mySettings["Control Chest Movement"] = "False";}
			mySettings.Add("Chest Movement Scale", new SimpleJSON.JSONData(uiChestAmount.val));
			if (uiDoHands.val){mySettings["Control Hand Morphs"] = "True";}else{mySettings["Control Hand Morphs"] = "False";}
			if (uiUsePerson2.val){mySettings["Look At Target Person"] = "True";}else{mySettings["Look At Target Person"] = "False";}
			mySettings.Add("Current Focus Target", new SimpleJSON.JSONData(uiFocusTarget.val));
			mySettings.Add("Current Object Target", new SimpleJSON.JSONData(uiObjectTarget.val));
			if (uiTargetLook.val){mySettings["Object View Direction Enable"] = "True";}else{mySettings["Object View Direction Enable"] = "False";}
			if (uiDoKiss.val){mySettings["Kissing Enable"] = "True";}else{mySettings["Kissing Enable"] = "False";}
			mySettings.Add("Kissing Activation Distance", new SimpleJSON.JSONData(uiKissingDist.val));
			mySettings.Add("Kissing Morph Scale", new SimpleJSON.JSONData(uiKissAmount.val));
			if (uiDoBlowjob.val){mySettings["Blowjob Enable"] = "True";}else{mySettings["Blowjob Enable"] = "False";}
			mySettings.Add("Blowjob Morph Scale", new SimpleJSON.JSONData(uiBlowjobAmount.val));
			if (uiDoSex.val){mySettings["Sex Enable"] = "True";}else{mySettings["Sex Enable"] = "False";}
			mySettings.Add("Sex Morph Scale", new SimpleJSON.JSONData(uiSexAmount.val));
			mySettings.Add("Maximum Allowed Value For Smile Morphs", new SimpleJSON.JSONData(uiMaxMorphSmile.val));
			mySettings.Add("Maximum Amount To Close Eyes", new SimpleJSON.JSONData(uiEyeCloseMaxMorph.val));
			mySettings.Add("Minimum Time Between Eye Target Movements Excl Saccades", new SimpleJSON.JSONData(uiEyeUpdate.val));
			mySettings.Add("Main Interest Switch Delay Scale", new SimpleJSON.JSONData(uiInterestSpeed.val));
			mySettings.Add("Global Interest Rate Scale", new SimpleJSON.JSONData(uiInterestRate.val));
			mySettings.Add("Extraversion", new SimpleJSON.JSONData(uiExtraversion.val));
			mySettings.Add("Agreeableness", new SimpleJSON.JSONData(uiAgreeableness.val));
			mySettings.Add("Stableness", new SimpleJSON.JSONData(uiStableness.val));
			mySettings.Add("Mood Arousal Scale", new SimpleJSON.JSONData(uiArousalSpeed.val));
			mySettings.Add("Mood Valence Scale", new SimpleJSON.JSONData(uiValenceSpeed.val));
			mySettings.Add("Mood Change Scale", new SimpleJSON.JSONData(uiMoodSpeed.val));
			mySettings.Add("Breathing Speed", new SimpleJSON.JSONData(uiBreatheSpeed.val));
			mySettings.Add("Breathe Morph Multiplier", new SimpleJSON.JSONData(uiBreatheExpandMultiplier.val));
			mySettings.Add("Gaze Speed", new SimpleJSON.JSONData(uiGazeSpeed.val));
			mySettings.Add("Gaze Angle Variation", new SimpleJSON.JSONData(uiGazeVariation.val));
			if (uiGazeAvoid.val){mySettings["Gaze Avoidance Enable"] = "True";}else{mySettings["Gaze Avoidance Enable"] = "False";}
			mySettings.Add("Gaze Look At Time", new SimpleJSON.JSONData(uiGazeLookTime.val));
			mySettings.Add("Gaze Avoid Look Time", new SimpleJSON.JSONData(uiGazeAvoidTime.val));
			if (uiGazeGlance.val){mySettings["Gaze Glance Enable"] = "True";}else{mySettings["Gaze Glance Enable"] = "False";}
			mySettings.Add("Gaze Head Roll Speed", new SimpleJSON.JSONData(uiRollSpeed.val));
			mySettings.Add("Eye Saccade Frequency", new SimpleJSON.JSONData(uiSaccadeSpeed.val));
			mySettings.Add("Eye Saccade Movement Scale", new SimpleJSON.JSONData(uiSaccadeAmount.val));
			mySettings.Add("Eye Saccade Max Dist from Target Scale", new SimpleJSON.JSONData(uiSaccadeWanderMult.val));
			mySettings.Add("Eye Blink Delay Scale", new SimpleJSON.JSONData(uiBlinkSpeed.val));
			mySettings.Add("Direct Viewing Angle", new SimpleJSON.JSONData(uiDirectGaze.val));
			mySettings.Add("Peripheral Viewing Angle", new SimpleJSON.JSONData(uiPeripheralGaze.val));
			mySettings.Add("Maximum Viewing Angle", new SimpleJSON.JSONData(uiOutOfGaze.val));
			mySettings.Add("Personal Space Distance", new SimpleJSON.JSONData(uiPersonalSpace.val));
			mySettings.Add("Face Interaction Distance", new SimpleJSON.JSONData(uiCloseToFaceDist.val));
			mySettings.Add("General Interaction Distance", new SimpleJSON.JSONData(uiInteractDist.val));
			mySettings.Add("Target Head Interest Rate Scale", new SimpleJSON.JSONData(uiHeadInterest.val));
			mySettings.Add("Target Left Hand Interest Rate Scale", new SimpleJSON.JSONData(uiLHandInterest.val));
			mySettings.Add("Target Right Hand Interest Rate Scale", new SimpleJSON.JSONData(uiRHandInterest.val));
			mySettings.Add("Target Pelvis Interest Rate Scale", new SimpleJSON.JSONData(uiPenisInterest.val));
			mySettings.Add("Target Object Interest Rate Scale", new SimpleJSON.JSONData(uiObjectInterest.val));
			SuperController.singleton.SaveJSON(mySettings, "E-Motion_Preset.json");
			uiSavePreset.val = false;
			SuperController.LogError("E-Motion Setup Saved to file successfully!");
		}
	
		public static float Round(float value)
		{
			return Mathf.Round(value * 100.0f) / 100.0f;
		}
    }
}