using UnityEngine;
using Random = UnityEngine.Random;

namespace VRAdultFun
{
    partial class EmotionEngine : MVRScript
    {
        #region Initilation Variables
        //private static Atom debugUI;
        //private static UITextControl debugUIControl;
		private static bool allSetup = false;
		private static float tempFloat = 0.0f;
		private static string currentLook = "";
        private static float debugClock = 0.0f;
        private static string logLine;
        private static Atom aCube;
        private static float saccadeClock = 0.0f;
		private static float saccadeRepeat = 0.0f;
        private static float eyeClock = 0.0f;
        private static float randomX = 0.0f;
        private static float randomY = 0.0f;
        private static string currentInterest = "Random";
        private static string prevInterest = "Random";
        private static float currentInterestLevel = 0.0f;
        private static float maxInterestLevel = 100.0f;
        private static float interestClock = 0.0f;
        private static float interestArousal = 0.0f;
        private static float interestValence = 0.0f;
        private static float interestMaxSmile = 0.55f;
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
		protected JSONStorableFloat uiGazeSpeed;
		protected JSONStorableFloat uiGazeVariation;
		protected JSONStorableFloat uiSaccadeSpeed;
		protected JSONStorableFloat uiBlinkSpeed;
		protected JSONStorableFloat uiArousalSpeed;
		protected JSONStorableFloat uiValenceSpeed;
		protected JSONStorableFloat uiInterestSpeed;

		
        private static float gAvoidance = Mathf.Clamp((((100.0f - pExtraversion) + (100.0f - pStableness)) / 2.0f), 30.0f, 100.0f);
        private static float gDuration = 10.0f * (Mathf.Clamp((pStableness / 5.0f) + ((100.0f - pAgreeableness) / 2.0f) + (pExtraversion / 5.0f), 0.0f, 100.0f) / 100.0f);
        private static float gFrequency = Mathf.Clamp((pStableness / 2.0f) + (pExtraversion / 2.0f), 0.0f, 100.0f);
        private static float gHeadSpeed = 1.75f;
        private static bool gDirectionCenter;
        private static bool gDirectionUp;
        private static bool gDirectionDown;
        private static bool gDirectionSide;
        private static float gHeadRoll = 0.0f;
        private static float gHeadRollTarget = 0.0f;
        private static float gAvoid = 0.0f;
        private static float gAvoidanceClock = 0.0f;
		private static float gAvoidingClock = 0.0f;
        private static float sexActionNeckX = 0.0f;

        private string mainInterest = "RandomF";
        private float mainValue = 0.0f;
        private string secondInterest = "RandomF";
        private float secondValue = 0.0f;
        private string mainOld = "RandomF";
        private string secondOld = "RandomF";
        private bool mainSwitch = false;
        private bool secondSwitch = false;
        private float mainClock = 0.0f;
        private float secondClock = 0.0f;

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
        private static Vector3 curEyePosition;
        private static Vector3 curEyeAngles;
        private static float eyeUpdateTime = 0.05f;
        private static float eyeUpdateClock = 0.0f;
        private static FreeControllerV3 eyeController;
        private static FreeControllerV3 headActual;
        private static FreeControllerV3 headController;
        private static FreeControllerV3 neckController;
        private static FreeControllerV3 chestController;
        private static FreeControllerV3 lBreastController;
        private static FreeControllerV3 rBreastController;
        private static FreeControllerV3 pelvisController;
        private static FreeControllerV3 lHandController;
        private static FreeControllerV3 rHandController;
        private static FreeControllerV3 lArmController;
        private static FreeControllerV3 rArmController;
        private static FreeControllerV3 lFootController;
        private static FreeControllerV3 rFootController;

        private static bool lookAction = false;
        private static float lookVariation = 1.0f;
        private static float browVariation = 1.0f;
        private static float eyeVariation = 1.0f;
        private static float mouthVariation = 1.0f;
        private static float saccadeAmount = 0.0f;

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
        private static DAZMorph morphMouthMouthOpen;
        private static float mMouthOpenValue = 0.0f;
        private static float mMouthOpenTarget = 0.0f;
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
        private static DAZMorph morphVisF;
        private static float mVisFValue = 0.0f;
        private static float mVisFTarget = 0.0f;


        private static DAZMorph morphTongueInOut;
        private static float mTongueInOutValue = 0.0f;
        private static float mTongueInOutTarget = 0.0f;
        private static DAZMorph morphTongueSideSide;
        private static float mTongueSideSideValue = 0.0f;
        private static float mTongueSideSideTarget = 0.0f;
        private static DAZMorph morphTongueBendTip;
        private static float mTongueBendTipValue = 0.0f;
        private static float mTongueBendTipTarget = 0.0f;

        private static DAZMorph morphRibCageSize;
        private static float mRibCageSizeOrig = 0.0f;
        private static float mRibCageSizeValue = 0.0f;
        private static float mRibCageSizeTarget = 0.0f;
        private static DAZMorph morphChestHeight;
        private static float mChestHeightOrig = 0.0f;
        private static float mChestHeightValue = 0.0f;
        private static float mChestHeightTarget = 0.0f;
        private static DAZMorph morphSternumDepth;
        private static float mSternumDepthOrig = 0.0f;
        private static float mSternumDepthValue = 0.0f;
        private static float mSternumDepthTarget = 0.0f;
        private static float breathClock = 0.0f;
        private static float breathCount = 0.0f;
        private static float breatheInSpeed = 1.1f;
        private static float breatheOutSpeed = 1.1f;
        private static float breathRate = Random.Range(0.2f, 0.3f);
        private static string breathState = "in";


        private static bool usePerson2;
        private static bool person2Usable;
        private static Atom person2;

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

        private static float playerHeadTimeout;
        private static float playerLHandTimeout;
        private static float playerRHandTimeout;
        private static float playerTipTimeout;

        private static float minHeadMotion;
        private static float minHandMotion;
        private static float minTipMotion;

        private static float minFaceDistance;
        private static float closeFaceDistance;
        private static float personalSpaceDistance;
        private static float backgroundDistance;
        private static float interactionDistance;


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

        private static float randomInterest;

        #endregion

        public override void Init()
        {

            //SuperController.LogError("public void OnPreLoad()");
            //emotionSM = new StateMachine();
            lookSM = new StateMachine();
            systemSM = new StateMachine();
            browSM = new StateMachine();
            mouthSM = new StateMachine();
            eyesSM = new StateMachine();


            minHeadMotion = 0.005f;
            minHandMotion = 0.015f;
            minTipMotion = 0.21f;

            minFaceDistance = 0.175f;
            closeFaceDistance = 0.075f;
            personalSpaceDistance = 0.85f;
            backgroundDistance = 1.2f;
            interactionDistance = 0.13f;

            movementMaxTimeout = 15.0f;
            movementModifier = 0.025f;
            movementFalloff = 0.11f;

			uiAgreeableness = new JSONStorableFloat("Personality Agreeableness", pAgreeableness, 10.0f, 100.0f, true, true);
			RegisterFloat(uiAgreeableness);
			CreateSlider(uiAgreeableness, false);

			uiExtraversion = new JSONStorableFloat("Personality Extraversion", pExtraversion, 10.0f, 100.0f, true, true);
			RegisterFloat(uiExtraversion);
			CreateSlider(uiExtraversion, false);

			uiStableness = new JSONStorableFloat("Personality Stableness", pStableness, 10.0f, 100.0f, true, true);
			RegisterFloat(uiStableness);
			CreateSlider(uiStableness, false);
			
			uiBreatheSpeed = new JSONStorableFloat("Breath Speed Multiplier", 1.0f, 0.0f, 3.0f, true, true);
			RegisterFloat(uiBreatheSpeed);
			CreateSlider(uiBreatheSpeed, false);

			uiGazeSpeed = new JSONStorableFloat("Gaze Speed Multiplier", 1.0f, 0.0f, 3.0f, true, true);
			RegisterFloat(uiGazeSpeed);
			CreateSlider(uiGazeSpeed, false);

			uiGazeVariation = new JSONStorableFloat("Gaze Variation Multiplier", 1.0f, 0.0f, 3.0f, true, true);
			RegisterFloat(uiGazeVariation);
			CreateSlider(uiGazeVariation, false);

			
			uiBlinkSpeed = new JSONStorableFloat("Blink Speed Multiplier", 1.0f, 0.0f, 2.0f, true, true);
			RegisterFloat(uiBlinkSpeed);
			CreateSlider(uiBlinkSpeed, false);

			uiSaccadeSpeed = new JSONStorableFloat("Saccade Speed Multiplier", 1.0f, 0.0f, 3.0f, true, true);
			RegisterFloat(uiSaccadeSpeed);
			CreateSlider(uiSaccadeSpeed, false);
			
			uiArousalSpeed = new JSONStorableFloat("Arousal Speed Multiplier", 1.0f, 0.0f, 10.0f, true, true);
			RegisterFloat(uiArousalSpeed);
			CreateSlider(uiArousalSpeed, false);

			uiValenceSpeed = new JSONStorableFloat("Valence Speed Multiplier", 1.0f, 0.0f, 10.0f, true, true);
			RegisterFloat(uiValenceSpeed);
			CreateSlider(uiValenceSpeed, false);

			uiInterestSpeed = new JSONStorableFloat("Interest Change Speed Multiplier", 1.0f, 0.0f, 3.0f, true, true);
			RegisterFloat(uiInterestSpeed);
			CreateSlider(uiInterestSpeed, false);
			
            lookDirectAngle = 9.0f;// * (playerHeadToHead / personalSpaceDistance);
            lookPeripheralAngle = 20.0f;
            lookNoAwarenessAngle = 60.0f;

            usePerson2 = true;

            //pre init
            player = CameraTarget.centerTarget.transform;
            playerInterest = 0.0f;
            randomInterest = 0.0f;
            interestFace = 0.0f;
            interestLHand = 0.0f;
            interestRHand = 0.0f;
            interestPelvis = 0.0f;
            interestTip = 0.0f;

            playerHeadTimeout = 0.0f;
            playerLHandTimeout = 0.0f;
            playerRHandTimeout = 0.0f;
            playerTipTimeout = 0.0f;

            playerLHandMovement = false;
            playerRHandMovement = false;
            playerTipMovement = false;

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

        }

        public void Start()
               {
            //SuperController.LogError("public void OnPostLoad()");
            //debugUI = Utils.GetAtom("debugText");
            //debugUIControl = debugUI.GetStorableByID("control") as UITextControl;

            person = SuperController.singleton.GetAtomByUid("Person");
            if (person != null)
            {
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
                lArmController = person.GetStorableByID("lShoulderControl") as FreeControllerV3;
                rArmController = person.GetStorableByID("rShoulderControl") as FreeControllerV3;
                lFootController = person.GetStorableByID("lFootControl") as FreeControllerV3;
                rFootController = person.GetStorableByID("rFootControl") as FreeControllerV3;
                refAngle = new Vector3(0.0f,-10.0f,-10.0f);
                if (morphUI != null)
                {
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

                    morphExpSmileFullFace = morphUI.GetMorphByDisplayName("Smile Full Face");
                    morphExpSmileOpenFullFace = morphUI.GetMorphByDisplayName("Smile Open Full Face");
                    morphExpGlare = morphUI.GetMorphByDisplayName("Glare");
                    morphExpExcitement = morphUI.GetMorphByDisplayName("Excitement");
                    morphExpHappy = morphUI.GetMorphByDisplayName("Happy");
                    morphExpFlirting = morphUI.GetMorphByDisplayName("Flirting");
                    morphMouthMouthOpen = morphUI.GetMorphByDisplayName("Mouth Open");
                    morphMouthSideLeft = morphUI.GetMorphByDisplayName("Mouth Side-Side Left");
                    morphMouthSideRight = morphUI.GetMorphByDisplayName("Mouth Side-Side Right");
                    morphMouthSmileSimpleLeft = morphUI.GetMorphByDisplayName("Mouth Smile Simple Left");
                    morphMouthSmileSimpleRight = morphUI.GetMorphByDisplayName("Mouth Smile Simple Right");
                    morphLipsLipsPucker = morphUI.GetMorphByDisplayName("Lips Pucker");
                    morphLipsLipsPuckerWide = morphUI.GetMorphByDisplayName("Lips Pucker Wide");
                    morphVisF = morphUI.GetMorphByDisplayName("F");

                    morphTongueInOut = morphUI.GetMorphByDisplayName("Tongue In-Out");
                    morphTongueSideSide = morphUI.GetMorphByDisplayName("Tongue Side-Side");
                    morphTongueBendTip = morphUI.GetMorphByDisplayName("Tongue Curl");

                    morphRibCageSize = morphUI.GetMorphByDisplayName("Ribcage Size");
                    mRibCageSizeOrig = morphRibCageSize.morphValue;
                    morphChestHeight = morphUI.GetMorphByDisplayName("Chest Height");
                    mChestHeightOrig = morphChestHeight.morphValue;
                    morphSternumDepth = morphUI.GetMorphByDisplayName("Sternum Depth");
                    mSternumDepthOrig = morphSternumDepth.morphValue;
                }
            }

            if (playerVRLHand != null && playerVRRHand != null)
            {
                playerHandsUsable = true;
            }

            //aCube = SuperController.singleton.GetAtomByUid("Cube");
            person2 = SuperController.singleton.GetAtomByUid("Person#2");
            if (person2 != null)
            {
                person2Usable = true;
                playerHeadController = person2.GetStorableByID("headControl") as FreeControllerV3;
                playerChestController = person2.GetStorableByID("chestControl") as FreeControllerV3;
                playerLHandController = person2.GetStorableByID("lHandControl") as FreeControllerV3;
                playerRHandController = person2.GetStorableByID("rHandControl") as FreeControllerV3;
                playerPelvisController = person2.GetStorableByID("pelvisControl") as FreeControllerV3;
                playerTipController = person2.GetStorableByID("penisTipControl") as FreeControllerV3;
                playerTipBaseController = person2.GetStorableByID("penisBaseControl") as FreeControllerV3;
            }

            if (person == null || headController == null)
            {
                SuperController.LogError("[EmotionEngine] Person not found");
                return;
            }
            if (player == null)
            {
                SuperController.LogError("[EmotionEngine] Player not found");
                return;
            }

            if (usePerson2 && person2Usable)
            {
                playerFace = playerHeadController.transform.position;
                playerLHand = playerLHandController.transform.position;
                playerRHand = playerRHandController.transform.position;
                playerPelvis = playerPelvisController.transform.position;
                playerTip = playerTipController.transform.position;
            }
            else
            {
                playerFace = player.position;
                if (person2Usable && (usePerson2 || playerHandsUsable == false))
                {
                    playerLHand = playerLHandController.transform.position;
                    playerRHand = playerRHandController.transform.position;
                }
                if (playerHandsUsable && usePerson2 == false)
                {
                    playerLHand = playerVRLHand.position;
                    playerRHand = playerVRRHand.position;
                }
                if (person2Usable == false && playerHandsUsable == false)
                {
                    playerLHand = playerFace;
                    playerRHand = playerFace;
                }
                if (person2Usable)
                {
                    playerPelvis = playerPelvisController.transform.position;
                    playerTip = playerTipController.transform.position;
                }
            }
            playerHandsUsable = false;
            person2Usable = false;

            if (usePerson2 && person2 != null)
            {
                playerHeadTransform = playerHeadController.transform;
                closeFaceDistance = closeFaceDistance * 1.85f;
                person2Usable = true;
            }
            else
            {
                playerHeadTransform = player;
            }
            if (playerVRLHand != null && playerVRRHand != null)
            {
                playerLHandTransform = playerVRLHand;
                playerRHandTransform = playerVRRHand;
                playerHandsUsable = true;
            }
            else
            {
                if (person2 != null)
                {
                    playerLHandTransform = playerLHandController.transform;
                    playerRHandTransform = playerRHandController.transform;
                }
                else
                {
                    playerLHandTransform = playerHeadTransform;
                    playerRHandTransform = playerHeadTransform;
                }
            }
            systemSM.Switch(sUpdate);
        }

                     
        public void FixedUpdate()
        {
			gHeadSpeed = 1.75f;
			oldEyePos = eyeController.transform.position;
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
				gDuration = 10.0f * (Mathf.Clamp((pStableness / 5.0f) + ((100.0f - pAgreeableness) / 2.0f) + (pExtraversion / 5.0f), 0.0f, 100.0f) / 100.0f);
				gFrequency = Mathf.Clamp((pStableness / 2.0f) + (pExtraversion / 2.0f), 0.0f, 100.0f);
				}
			
			
			//emotionSM.OnUpdate();
            lookSM.OnUpdate();
            systemSM.OnUpdate();
            browSM.OnUpdate();
            mouthSM.OnUpdate();
            eyesSM.OnUpdate();
            string dbgHead = "";
            string dbgLHand = "";
            string dbgRHand = "";
            string dbgPenis = "";
            //aCube.transform.position = randomPointForward;
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
            interestArousal = Mathf.Clamp(interestArousal, 3.0f, 10.0f);
            interestValence = Mathf.Clamp(interestValence, 2.0f, 10.0f);
			if (playerHeadToHead < personalSpaceDistance || playerLHandToHead < personalSpaceDistance || playerRHandToHead < personalSpaceDistance)
			{
				interestValence = Mathf.Clamp(interestValence, 4.5f, 10.0f);
			}
            breatheInSpeed = Mathf.Lerp(1.0f, 1.3f, interestValence/10.0f) * uiBreatheSpeed.val;
            breatheOutSpeed = Mathf.Lerp(0.6f, 1.2f, interestArousal/10.0f) * uiBreatheSpeed.val;
            if (interestKissing || playerTipToHead < interactionDistance)
            {
                breatheInSpeed = 1.5f * uiBreatheSpeed.val;
                breatheOutSpeed = 0.4f * uiBreatheSpeed.val;
            }
            //breathing
            float breathingRate = Random.Range(0.2f, 0.3f) + (interestArousal / 15.0f) * uiBreatheSpeed.val;
            if (breathState == "in")
            {
                breathClock = Mathf.Min(breathClock + (Time.fixedDeltaTime * (breathingRate * breatheInSpeed)), 1.0f);
                if (breathClock == 1.0f)
                {
                    breathState = "out";
                }
            }
            if (breathState == "out")
            {
                breathClock = Mathf.Max(breathClock - (Time.fixedDeltaTime * (breathingRate * breatheOutSpeed)), 0.0f);
                if (breathClock == 0.0f)
                {
                    breathState = "in";
                    breathCount += 1.0f;
                }
            }

            morphRibCageSize.SetValue(breathClock * 0.13f);
            morphChestHeight.SetValue(Mathf.Min((breathClock * -0.135f) + 0.02f, 0.0f));
            morphSternumDepth.SetValue(breathClock * 0.11f);

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
            }

            if (mBrowUpTarget + (interestValence / 30.0f) > mBrowUpValue + 0.02f) { mBrowUpValue = Mathf.Min(mBrowUpValue + (0.003f * browVariation), mBrowUpTarget + (interestValence / 30.0f)); }
            if (mBrowUpTarget + (interestValence / 30.0f) < mBrowUpValue - 0.02f) { mBrowUpValue = Mathf.Max(mBrowUpValue - (0.006f * browVariation), mBrowUpTarget + (interestValence / 30.0f)); }
            morphBrowUp.SetValue(Mathf.Clamp(mBrowUpValue, 0.0f, 1.0f));
            if (mBrowDownTarget > mBrowDownValue + 0.02f) { mBrowDownValue = Mathf.Min(mBrowDownValue + (0.003f * browVariation), mBrowDownTarget); }
            if (mBrowDownTarget < mBrowDownValue - 0.02f) { mBrowDownValue = Mathf.Max(mBrowDownValue - (0.006f * browVariation), mBrowDownTarget); }
            morphBrowDown.SetValue(mBrowDownValue);
            if (mExcitementTarget > mExcitementValue + 0.01f) { mExcitementValue = Mathf.Min(mExcitementValue + (0.006f * browVariation), mExcitementTarget); }
            if (mExcitementTarget < mExcitementValue - 0.01f) { mExcitementValue = Mathf.Max(mExcitementValue - (0.008f * browVariation), mExcitementTarget); }
            morphExpExcitement.SetValue(mExcitementValue);
            if (mBrowOuterUpLeftTarget > mBrowOuterUpLeftValue + 0.02f) { mBrowOuterUpLeftValue = Mathf.Min(mBrowOuterUpLeftValue + (0.03f * browVariation), mBrowOuterUpLeftTarget); }
            if (mBrowOuterUpLeftTarget < mBrowOuterUpLeftValue - 0.02f) { mBrowOuterUpLeftValue = Mathf.Max(mBrowOuterUpLeftValue - (0.012f * browVariation), mBrowOuterUpLeftTarget); }
            morphBrowOuterUpLeft.SetValue(mBrowOuterUpLeftValue);
            if (mBrowOuterUpRightTarget > mBrowOuterUpRightValue + 0.02f) { mBrowOuterUpRightValue = Mathf.Min(mBrowOuterUpRightValue + (0.03f * browVariation), mBrowOuterUpRightTarget); }
            if (mBrowOuterUpRightTarget < mBrowOuterUpRightValue - 0.02f) { mBrowOuterUpRightValue = Mathf.Max(mBrowOuterUpRightValue - (0.012f * browVariation), mBrowOuterUpRightTarget); }
            morphBrowOuterUpRight.SetValue(mBrowOuterUpRightValue);
            if (mBrowCenterUpTarget + (interestArousal / 20.0f) > mBrowCenterUpValue + 0.03) { mBrowCenterUpValue = Mathf.Min(mBrowCenterUpValue + (0.002f * browVariation), mBrowCenterUpTarget + (interestArousal / 20.0f)); }
            if (mBrowCenterUpTarget + (interestArousal / 20.0f) < mBrowCenterUpValue - 0.03) { mBrowCenterUpValue = Mathf.Max(mBrowCenterUpValue - (0.001f * browVariation), mBrowCenterUpTarget + (interestArousal / 20.0f)); }
            morphBrowCenterUp.SetValue(Mathf.Clamp(mBrowCenterUpValue, 0.0f, 1.0f));

            float lidLower = 0.15f;
            float lidRaise = 0.07f;
            if (morphBlinking)
            {
				tempFloat = Random.Range(-0.05f, 0.05f);
                lidLower = 0.53f + tempFloat;
                lidRaise = 0.1f + tempFloat;
				tempFloat = 1.05f - Mathf.Lerp(0.0f,0.1f,mEyesSquintValue);
                if (mEyesClosedLeftTarget > mEyesClosedLeftValue) { mEyesClosedLeftValue = Mathf.Min(mEyesClosedLeftValue + lidLower, mEyesClosedLeftTarget); }
                if (mEyesClosedLeftTarget < mEyesClosedLeftValue) { mEyesClosedLeftValue = Mathf.Max(mEyesClosedLeftValue - lidRaise, mEyesClosedLeftTarget); }
                morphEyesClosedLeft.SetValue(Mathf.Clamp(mEyesClosedLeftValue, 0.0f, tempFloat));
                if (mEyesClosedRightTarget > mEyesClosedRightValue) { mEyesClosedRightValue = Mathf.Min(mEyesClosedRightValue + lidLower, mEyesClosedRightTarget); }
                if (mEyesClosedRightTarget < mEyesClosedRightValue) { mEyesClosedRightValue = Mathf.Max(mEyesClosedRightValue - lidRaise, mEyesClosedRightTarget); }
                morphEyesClosedRight.SetValue(Mathf.Clamp(mEyesClosedRightValue, 0.0f, tempFloat));
            }
            else
            {
                if (mEyesClosedLeftTarget > mEyesClosedLeftValue) { mEyesClosedLeftValue = Mathf.Min(mEyesClosedLeftValue + (lidLower * eyeVariation), mEyesClosedLeftTarget); }
                if (mEyesClosedLeftTarget < mEyesClosedLeftValue) { mEyesClosedLeftValue = Mathf.Max(mEyesClosedLeftValue - (lidRaise * eyeVariation), mEyesClosedLeftTarget); }
                morphEyesClosedLeft.SetValue(Mathf.Clamp(mEyesClosedLeftValue, 0.0f, tempFloat));
                if (mEyesClosedRightTarget > mEyesClosedRightValue) { mEyesClosedRightValue = Mathf.Min(mEyesClosedRightValue + (lidLower * eyeVariation), mEyesClosedRightTarget); }
                if (mEyesClosedRightTarget < mEyesClosedRightValue) { mEyesClosedRightValue = Mathf.Max(mEyesClosedRightValue - (lidRaise * eyeVariation), mEyesClosedRightTarget); }
                morphEyesClosedRight.SetValue(Mathf.Clamp(mEyesClosedRightValue, 0.0f, tempFloat));
            }

            if (morphBlinking && mEyesClosedRightValue <= 0.0f && mEyesClosedLeftValue <= 0.0f)
            {
                morphBlinking = false;
            }

            if (mEyesSquintTarget + (interestArousal / 100.0f) > mEyesSquintValue + 0.05) { mEyesSquintValue = Mathf.Min(mEyesSquintValue + (0.04f * eyeVariation), mEyesSquintTarget + (interestArousal / 100.0f)); }
            if (mEyesSquintTarget + (interestArousal / 100.0f) < mEyesSquintValue - 0.05) { mEyesSquintValue = Mathf.Max(mEyesSquintValue - (0.01f * eyeVariation), mEyesSquintTarget + (interestArousal / 100.0f)); }
            morphEyesSquint.SetValue(Mathf.Clamp(mEyesSquintValue, 0.0f, 1.0f));


            if (mMouthOpenTarget > mMouthOpenValue) { mMouthOpenValue = Mathf.Min(mMouthOpenValue + (0.007f * mouthVariation), mMouthOpenTarget); }
            if (mMouthOpenTarget < mMouthOpenValue) { mMouthOpenValue = Mathf.Max(mMouthOpenValue - (0.07f * mouthVariation), mMouthOpenTarget); }
            morphMouthMouthOpen.SetValue(Mathf.Clamp(mMouthOpenValue + (interestArousal / 100.0f), 0.0f, 1.0f));
            if (mLipsPuckerTarget > mLipsPuckerValue) { mLipsPuckerValue = Mathf.Min(mLipsPuckerValue + (0.01f * mouthVariation), mLipsPuckerTarget); }
            if (mLipsPuckerTarget < mLipsPuckerValue) { mLipsPuckerValue = Mathf.Max(mLipsPuckerValue - (0.007f * mouthVariation), mLipsPuckerTarget); }
            morphLipsLipsPucker.SetValue(mLipsPuckerValue);
            if (mLipsPuckerWideTarget > mLipsPuckerWideValue) { mLipsPuckerWideValue = Mathf.Min(mLipsPuckerWideValue + (0.01f * mouthVariation), mLipsPuckerWideTarget); }
            if (mLipsPuckerWideTarget < mLipsPuckerWideValue) { mLipsPuckerWideValue = Mathf.Max(mLipsPuckerWideValue - (0.007f * mouthVariation), mLipsPuckerWideTarget); }
            morphLipsLipsPuckerWide.SetValue(mLipsPuckerWideValue);
            if (mFlirtingTarget > mFlirtingValue + 0.01f) { mFlirtingValue = Mathf.Min(mFlirtingValue + (0.01f * mouthVariation), mFlirtingTarget); }
            if (mFlirtingTarget < mFlirtingValue - 0.01f) { mFlirtingValue = Mathf.Max(mFlirtingValue - (0.001f * mouthVariation), mFlirtingTarget); }
            morphExpFlirting.SetValue(mFlirtingValue);
            if (mHappyTarget > mHappyValue + 0.01f) { mHappyValue = Mathf.Min(mHappyValue + (0.0035f * mouthVariation), mHappyTarget); }
            if (mHappyTarget < mHappyValue - 0.01f) { mHappyValue = Mathf.Max(mHappyValue - (0.0011f * mouthVariation), mHappyTarget); }
            morphExpHappy.SetValue(mHappyValue);
            if (Mathf.Clamp((interestValence / 30.0f) + mSmileFullFaceTarget, 0.0f, interestMaxSmile) > mSmileFullFaceValue + 0.01f) { mSmileFullFaceValue = Mathf.Min(mSmileFullFaceValue + (0.0054f * mouthVariation), Mathf.Clamp((interestValence / 30.0f) + mSmileFullFaceTarget, 0.0f, interestMaxSmile)); }
            if (Mathf.Clamp((interestValence / 30.0f) + mSmileFullFaceTarget, 0.0f, interestMaxSmile) < mSmileFullFaceValue - 0.01f) { mSmileFullFaceValue = Mathf.Max(mSmileFullFaceValue - (0.003f * mouthVariation), Mathf.Clamp((interestValence / 30.0f) + mSmileFullFaceTarget, 0.0f, interestMaxSmile)); }
            morphExpSmileFullFace.SetValue(mSmileFullFaceValue);
            if (mSmileOpenFullFaceTarget > mSmileOpenFullFaceValue + 0.01f) { mSmileOpenFullFaceValue = Mathf.Min(mSmileOpenFullFaceValue + (0.0025f * mouthVariation), mSmileOpenFullFaceTarget); }
            if (mSmileOpenFullFaceTarget < mSmileOpenFullFaceValue - 0.01f) { mSmileOpenFullFaceValue = Mathf.Max(mSmileOpenFullFaceValue - (0.0002f * mouthVariation), mSmileOpenFullFaceTarget); }
            morphExpSmileOpenFullFace.SetValue(mSmileOpenFullFaceValue);

            if (mVisFTarget > mVisFValue + 0.01f) { mVisFValue = Mathf.Min(mVisFValue + (0.0003f * mouthVariation), mVisFTarget); }
            if (mVisFTarget < mVisFValue - 0.01f) { mVisFValue = Mathf.Max(mVisFValue - (0.0001f * mouthVariation), mVisFTarget); }
            morphExpSmileOpenFullFace.SetValue(mVisFValue);

            if (Mathf.Clamp((interestValence / 50.0f) + mSmileSimpleLeftTarget, 0.0f, interestMaxSmile) > mSmileSimpleLeftValue + 0.02f) { mSmileSimpleLeftValue = Mathf.Min(mSmileSimpleLeftValue + (0.007f * mouthVariation), Mathf.Clamp((interestValence / 50.0f) + mSmileSimpleLeftTarget, 0.0f, interestMaxSmile)); }
            if (Mathf.Clamp((interestValence / 50.0f) + mSmileSimpleLeftTarget, 0.0f, interestMaxSmile) < mSmileSimpleLeftValue - 0.02f) { mSmileSimpleLeftValue = Mathf.Max(mSmileSimpleLeftValue - (0.001f * mouthVariation), Mathf.Clamp((interestValence / 50.0f) + mSmileSimpleLeftTarget, 0.0f, interestMaxSmile)); }
            morphMouthSmileSimpleLeft.SetValue(mSmileSimpleLeftValue);
            if (Mathf.Clamp((interestValence / 50.0f) + mSmileSimpleRightTarget, 0.0f, interestMaxSmile) > mSmileSimpleRightValue + 0.02f) { mSmileSimpleRightValue = Mathf.Min(mSmileSimpleRightValue + (0.007f * mouthVariation), Mathf.Clamp((interestValence / 50.0f) + mSmileSimpleRightTarget, 0.0f, interestMaxSmile)); }
            if (Mathf.Clamp((interestValence / 50.0f) + mSmileSimpleRightTarget, 0.0f, interestMaxSmile) < mSmileSimpleRightValue - 0.02f) { mSmileSimpleRightValue = Mathf.Max(mSmileSimpleRightValue - (0.001f * mouthVariation), Mathf.Clamp((interestValence / 50.0f) + mSmileSimpleRightTarget, 0.0f, interestMaxSmile)); }
            morphMouthSmileSimpleRight.SetValue(mSmileSimpleRightValue);

            if (mMouthSideLeftTarget > mMouthSideLeftValue + 0.01f) { mMouthSideLeftValue = Mathf.Min(mMouthSideLeftValue + (0.005f * mouthVariation), mMouthSideLeftTarget); }
            if (mMouthSideLeftTarget < mMouthSideLeftValue - 0.01f) { mMouthSideLeftValue = Mathf.Max(mMouthSideLeftValue - (0.015f * mouthVariation), mMouthSideLeftTarget); }
            morphMouthSideLeft.SetValue(mMouthSideLeftValue);
            if (mMouthSideRightTarget > mMouthSideRightValue + 0.01f) { mMouthSideRightValue = Mathf.Min(mMouthSideRightValue + (0.005f * mouthVariation), mMouthSideRightTarget); }
            if (mMouthSideRightTarget < mMouthSideRightValue - 0.01f) { mMouthSideRightValue = Mathf.Max(mMouthSideRightValue - (0.015f * mouthVariation), mMouthSideRightTarget); }
            morphMouthSideRight.SetValue(mMouthSideRightValue);
			
            if (mTongueInOutTarget > mTongueInOutValue) { mTongueInOutValue = Mathf.Min(mTongueInOutValue + (0.04f * mouthVariation), mTongueInOutTarget); }
            if (mTongueInOutTarget < mTongueInOutValue) { mTongueInOutValue = Mathf.Max(mTongueInOutValue - (0.07f * mouthVariation), mTongueInOutTarget); }
            morphTongueInOut.SetValue(mTongueInOutValue);
            if (mTongueSideSideTarget > mTongueSideSideValue) { mTongueSideSideValue = Mathf.Min(mTongueSideSideValue + (0.034f * mouthVariation), mTongueSideSideTarget); }
            if (mTongueSideSideTarget < mTongueSideSideValue) { mTongueSideSideValue = Mathf.Max(mTongueSideSideValue - (0.017f * mouthVariation), mTongueSideSideTarget); }
            morphTongueSideSide.SetValue(mTongueSideSideValue);
            if (mTongueBendTipTarget > mTongueBendTipValue) { mTongueBendTipValue = Mathf.Min(mTongueBendTipValue + (0.08f * mouthVariation), mTongueBendTipTarget); }
            if (mTongueBendTipTarget < mTongueBendTipValue) { mTongueBendTipValue = Mathf.Max(mTongueBendTipValue - (0.06f * mouthVariation), mTongueBendTipTarget); }
            morphTongueBendTip.SetValue(mTongueBendTipValue);


            if (interestKissing == false)
            {
                eyeClock += Time.fixedDeltaTime * (0.5f * uiBlinkSpeed.val);
                if (((Random.Range(0.0f, (((15.0f - (10.0f - interestArousal)) / (2.0f * (interestArousal))) * (1.0f / Random.Range(1.333f, 2.5f))) * Time.fixedDeltaTime) / 300.0f) - (Mathf.Max(eyeClock - 1.0f, 0.0f) / 5000000.0f) <= 0.0f) && eyeClock > 0.05f)
                //if (((Random.Range(0.0f,(1.0f / Random.Range(1.333f,3.5f)) * Time.fixedDeltaTime) / 10.0f) - (Mathf.Max(eyeClock-1.0f,0.0f) / 5000000.0f) <= 0.0f) && eyeClock > 0.5f)
                {
                    eyesSM.Switch(eBlink);
                    eyeClock = 0.0f;
                }
            }


            if ((saccadeClock <= 0.0f) && eyesSM.CurrentState != eClosed)
            {

                float saccade = Random.Range(0.0f, Mathf.Clamp(150.0f * (0.5f + (100.0f - pExtraversion)), 0.0f, 100.0f));
                //saccadeOffset = new Vector3(0.0f,0.0f,0.0f);
                float saccadeLength = (((5.0f + interestValence) / (2.0f * (10.0f - interestValence))) * ((2.0f * saccadeAmount + 20.0f) / 100.0f)) * Random.Range(1.0f, 4.0f);
                float saccadeRandom = Random.Range(-1.0f, 1.0f);
                saccadeClock = Mathf.Clamp(saccadeLength, 0.005f / uiSaccadeSpeed.val, 0.25f / uiSaccadeSpeed.val);

                bool sChange = false;
                if (saccade <= 20.38f || (saccade <= 30.0f && mainInterest == "Face"))
                {
                    //down
                    saccadeOffset = new Vector3(saccadeRandom, Random.Range(0.0f, -saccadeAmount), 0.0f);
                    sChange = true;
                }
                if (saccade <= 17.69f && playerHeadToHead > closeFaceDistance * 2.0f)
                {
                    //up
                    saccadeOffset = new Vector3(saccadeRandom, Random.Range(0.0f, saccadeAmount / 700), 0.0f);
                    sChange = true;
                }
                if (saccade <= 16.8f || (playerHeadToHead < closeFaceDistance * 2.0f && saccade <= 33.6f))
                {
                    //left
                    saccadeOffset = new Vector3(Random.Range(-saccadeAmount, saccadeAmount), saccadeRandom, 0.0f);
                    sChange = true;
                }
                if (saccade <= 15.54f || (playerHeadToHead < closeFaceDistance * 2.0f && saccade <= 31.08f))
                {
                    //right
                    saccadeOffset = new Vector3(Random.Range(saccadeAmount, 0.0f), saccadeRandom, 0.0f);
                    sChange = true;
                }
                if (saccade <= 7.89f)
                {
                    //down left
                    saccadeOffset = new Vector3(Random.Range(-saccadeAmount, 0.0f), Random.Range(0.0f, -saccadeAmount), 0.0f);
                    sChange = true;
                }
                if (saccade <= 7.79f)
                {
                    //down right
                    saccadeOffset = new Vector3(Random.Range(saccadeAmount, 0.0f), Random.Range(0.0f, -saccadeAmount), 0.0f);
                    sChange = true;
                }
                if (saccade <= 7.45f && playerHeadToHead > closeFaceDistance * 2.0f)
                {
                    //up left
                    saccadeOffset = new Vector3(Random.Range(-saccadeAmount, 0.0f), Random.Range(0.0f, saccadeAmount / 700), 0.0f);
                    sChange = true;
                }
                if (saccade <= 6.46f && playerHeadToHead > closeFaceDistance * 2.0f)
                {
                    //up right
                    saccadeOffset = new Vector3(Random.Range(saccadeAmount, 0.0f), Random.Range(0.0f, saccadeAmount / 700), 0.0f);
                    sChange = true;
                }
                if (saccadeAmount >= 2.8f && eyeClock > 0.75f && sChange)
                {
                    eyesSM.Switch(eBlink);
                    eyeClock = 0.0f;
                }
                else
                {
                    if (saccadeAmount >= 5.0f && eyeClock > 0.8f && sChange)
                    {
                        eyesSM.Switch(eBlink);
                        eyeClock = 0.0f;
                    }
                }
            }
            else
            {
                saccadeClock -= Time.fixedDeltaTime;
            }
            //saccadeOffset = new Vector3(eyeController.transform.rotation.x + saccadeOffset.x,eyeController.transform.rotation.y + saccadeOffset.y,eyeController.transform.rotation.z + saccadeOffset.z);


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

            if (amGlancing)
            {
                glanceClock += Time.fixedDeltaTime;
                if (glanceClock > Mathf.Clamp((100.0f - pExtraversion) / 10, 1.0f, 7.0f))
                {
                    glanceClock = 0.0f;
                    amGlancing = false;
                }
            }

            if (playerHeadToHead < closeFaceDistance && interestKissing == false)
            {
                interestClock = interestClock / 10.0f;
            }
            if (playerHeadToHead > closeFaceDistance && interestKissing)
            {
                lookSM.Switch(lIntense);
                mouthSM.Switch(mClosed);
                interestKissing = false;
            }

            if (playerHandsUsable || person2Usable)
            {
                interestLHand = interestLHandBase;
                interestRHand = interestRHandBase;
			}
            //currentInterestLevel = Mathf.Max(currentInterestLevel - 0.12f, 0.0f);
            interestFace = interestFaceBase;

            //Face
            if ((mainInterest == "Face" || mainOld == "Face") && mainClock > gDuration)
            {
                interestFace -= 20.0f;
				dbgHead += "-Timeout ";
            }
            if (playerHeadToFaceRot < lookDirectAngle && playerHeadToHead < backgroundDistance)
            {
                interestFace += 30.0f;
				interestValence += 0.0005f;
				interestLHand -= 10.0f;
				interestRHand -= 10.0f;
				if (mainInterest == "Face"){fuzzyLock = 10.0f;}
				dbgHead += "+PlayerLooking ";
            }
            if (headToFaceRot < lookDirectAngle)
            {
                interestFace += 10.0f;
				interestValence += 0.0005f;
                if (mainInterest == "Face"){fuzzyLock = 5.5f;}
				dbgHead += "+LookingAt ";
            }
            if (playerHeadToHead < closeFaceDistance)
            {
                interestFace += 50.0f;
				interestValence += 0.001f;
				if (mainInterest == "Face"){fuzzyLock = 0.5f;}
				dbgHead += "+Close ";
            }
            if (playerHeadToHead < interactionDistance || playerHeadToLBreast < interactionDistance || playerHeadToRBreast < interactionDistance || playerHeadToPelvis < interactionDistance)
            {
                interestFace += 50.0f;
                interestArousal += 0.002f;
                interestValence += 0.0002f;
                if (mainInterest == "Face"){fuzzyLock = 07.0f;}
				dbgHead += "+Interacting ";
            }
            if (playerHeadMovement && headToFaceRot < lookPeripheralAngle)
            {
                interestFace += 30.0f;
                //interestValence += 0.25f;
                if (mainInterest == "Face"){fuzzyLock = 5.5f;}
				dbgHead += "+Move ";
            }
            if (playerHeadMovement == false)
            {
                interestFace -= 10.0f;
				dbgHead += "-NoMove ";
            }
            if (personChestToHead > 70.0f && playerHeadToHead > personalSpaceDistance)
            {
                interestFace -= 100.0f;
                if (mainInterest == "Face"){fuzzyLock = 25.0f;}
				dbgHead += "-HighAngle ";
            }
            if (playerHeadToHead < personalSpaceDistance)
            {
                interestFace += 10.0f;
				dbgHead += "+PSpace ";
            }
            if (playerHeadToHead > personalSpaceDistance && playerHeadToHead < backgroundDistance)
            {
                interestFace -= 20.0f;
				dbgHead += "-AwareDist ";
            }
            if (playerHeadToHead > backgroundDistance)
            {
                interestFace -= 50.0f;
				dbgHead += "-FarOff ";
            }
            //interestFace -= (100.0f - pExtraversion) / 10.0f;
            //interestFace += (100.0f - pStableness) / 10.0f;

            if (playerHandsUsable || person2Usable)
            {

                bool interact = false;
                //Left Hand
                if ((mainInterest == "LHand" || mainOld == "LHand") && mainClock > gDuration)
                {
                    interestLHand -= 20.0f;
                    dbgLHand += "-LookTimeout ";
                }
                if (playerLHandToHead < closeFaceDistance)// && playerLHandToHead > interactionDistance)
                {
                    interestLHand += 100.0f;
					interestFace -= 3.0f;
                    interestArousal += 0.005f;
                    interestValence += 0.0025f;
                    dbgLHand += "+FaceContact ";
                }
				if (playerHeadToFaceRot < lookDirectAngle*2.0 && headToFaceRot < lookDirectAngle)
				{
					interestRHand -= 50.0f;
					dbgLHand += "-EyeToEye ";
				}
                if (playerLHandToHead < interactionDistance || playerLHandToLBreast < interactionDistance || playerLHandToRBreast < interactionDistance || playerLHandToPelvis < interactionDistance * 1.3f)
                {
					if (playerLHandMovement)
					{
                    interestArousal += 0.01f;
                    interestValence += 0.0055f;
                    interestLHand += 40.0f;
					}
					else
					{
                    interestLHand -= 10.0f;
					}
					interestFace -= 3.0f;
					interestRHand -= 20.0f;
                    if (mainInterest == "LHand"){fuzzyLock = 3.5f;}
					gHeadSpeed = 0.5f;
                    dbgLHand += "+Interacting ";
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
                    interestLHand += 50.0f * (playerLHandTimeout / movementMaxTimeout);
                    dbgLHand += "+Move ";
                    if (headToLHandRot > lookPeripheralAngle)
                    {
                        interestLHand += 50.0f * (playerLHandTimeout / movementMaxTimeout);
                        dbgLHand += "+MoveNoVis ";
                    }
                }
                if (playerLHandMovement == false)
                {
                    interestLHand -= 5.0f * (1.0f - (playerLHandTimeout / movementMaxTimeout));
                    dbgLHand += "-NoMove ";
                }
                if (playerToPLHand < lookDirectAngle * 1.0f)
                {
                    interestLHand += 30.0f;
                    if (mainInterest == "LHand"){fuzzyLock = 0.5f;}
                    dbgLHand += "+LookingAt ";
                }
                if (headToLHandRot > 90.0f)
                {
                    interestLHand -= 60.0f;
                    if (mainInterest == "LHand"){fuzzyLock = 25.0f;}
                    dbgLHand += "-HighAngle ";
                }
                if (playerLHandToHead < personalSpaceDistance)
                {
                    interestLHand += 10.0f;
                    dbgLHand += "+PSpace ";
                }
                if (mainInterest == "RHand")
                {
                    interestLHand -= 30.0f;
                    dbgLHand += "-OtherHand ";
                }
                if (secondOld == "LHand" || mainOld == "RHand")
                {
                    interestLHand -= 20.0f;
                    dbgLHand += "-Repeat ";
                }
				
                if (playerLHandToHead > playerHeadToHead * 1.5f)
                {
                    interestLHand -= 20.0f;
                    dbgLHand += "-HeadCloser ";
                }
                if (playerLHandToHead > personalSpaceDistance && playerLHandToHead < backgroundDistance)
                {
                    interestLHand += 10.0f;
                    dbgLHand += "+AwareDist ";
                }
                if (playerLHandToHead > backgroundDistance)
                {
                    interestLHand -= 50.0f;
                    dbgLHand += "-FarOff ";
                }
                interestLHand -= (100.0f - pStableness) / 10.0f;
                interestLHand += (100.0f - pExtraversion) / 10.0f;

                //Right Hand
                interact = false;
                if ((mainInterest == "RHand" || mainOld == "RHand") && mainClock > gDuration)
                {
                    interestRHand -= 20.0f;
					dbgRHand += "-LookTimeout ";
                }
                if (playerRHandToHead < closeFaceDistance)// && playerRHandToHead > interactionDistance)
                {
                    interestRHand += 100.0f;
					interestFace -= 3.0f;
                    interestArousal += 0.005f;
                    interestValence += 0.0025f;
					dbgRHand += "+FaceContact ";
                }
				if (playerHeadToFaceRot < lookDirectAngle*2.0f && headToFaceRot < lookDirectAngle)
				{
					interestLHand -= 50.0f;
					dbgRHand += "-EyeToEye ";
				}
                if (mainInterest == "LHand")
                {
                    interestRHand -= 30.0f;
                    dbgRHand += "-OtherHand ";
                }
                if (secondOld == "RHand" || mainOld == "LHand")
                {
                    interestRHand -= 20.0f;
                    dbgRHand += "-Repeat ";
                }
                if (playerRHandToHead < interactionDistance || playerRHandToLBreast < interactionDistance || playerRHandToRBreast < interactionDistance || playerRHandToPelvis < interactionDistance * 2.0f)
                {
                    interestRHand += 40.0f;
					interestFace -= 3.0f;
					interestLHand -= 20.0f;
					gHeadSpeed = 0.5f;
 					if (playerRHandMovement)
					{
                    interestArousal += 0.01f;
                    interestValence += 0.0055f;
					}
                   if (mainInterest == "RHand"){fuzzyLock = 3.5f;}
				   dbgRHand += "+Interacting ";
                }
                if (playerRHandToLHand < interactionDistance)
                {
                    mLHandFistTarget = Random.Range(0.5f, 0.8f);
                }
                if (playerRHandToRHand < interactionDistance)
                {
                    mRHandFistTarget = Random.Range(0.5f, 0.8f);
                }
                if (playerRHandMovement && headToRHandRot < lookNoAwarenessAngle)
                {
                    interestRHand += 50.0f * (playerLHandTimeout / movementMaxTimeout);
					dbgRHand += "+Move ";
                    if (headToRHandRot > lookPeripheralAngle)
                    {
                        interestRHand += 50.0f * (playerLHandTimeout / movementMaxTimeout);
						dbgRHand += "+MoveNoVis ";
                    }
                }
                if (playerRHandMovement == false)
                {
                    interestRHand -= 5.0f * (1.0f - (playerLHandTimeout / movementMaxTimeout));
					dbgRHand += "-NoMove ";
                }
                if (playerToPRHand < lookDirectAngle * 1.0f)
                {
                    interestRHand += 30.0f;
                    if (mainInterest == "RHand"){fuzzyLock = 0.5f;}
					dbgRHand += "+LookingAt ";
                }
                if (headToRHandRot > 90)
                {
                    interestRHand -= 60.0f;
                    if (mainInterest == "RHand"){fuzzyLock = 25.0f;}
					dbgRHand += "-HighAngle ";
                }
                if (playerRHandToHead < personalSpaceDistance)
                {
                    interestRHand += 10.0f;
					dbgRHand += "+PSpace ";
                }
                if (playerRHandToHead > playerHeadToHead * 1.5f)
                {
                    interestRHand -= 20.0f;
					dbgRHand += "-HeadCloser ";
                }
                if (playerRHandToHead > personalSpaceDistance && playerRHandToHead < backgroundDistance)
                {
                    interestRHand += 10.0f;
					dbgRHand += "+AwareDist ";
                }
                if (playerRHandToHead > backgroundDistance)
                {
                    interestRHand -= 50.0f;
					dbgRHand += "-FarOff ";
                }
                interestRHand -= (100.0f - pStableness) / 10.0f;
                interestRHand += (100.0f - pExtraversion) / 10.0f;

                if (interestLHand == interestRHand)
                {
                    if (playerLHandToHead < playerRHandToHead)
                    {
                        interestRHand -= 5.0f;
                    }
                    else
                    {
                        interestLHand -= 5.0f;
                    }
                }
            }
            if (person2Usable)
            {
                //pelvis
                interestPelvis = interestPelvisBase;
                if ((mainInterest == "Pelvis" || mainOld == "Pelvis") && mainClock > gDuration)
                {
                    interestPelvis -= 20.0f;
                }
                if (mainInterest == "Tip")
                {
                    interestPelvis -= 10.0f;
                }
                if (playerPelvisToHead < closeFaceDistance)
                {
                    interestPelvis += 20.0f;
                    //interestArousal += 2.0f;
                    if (mainInterest == "Pelvis"){fuzzyLock = 0.5f;}
                }
                if (playerHeadToHead < personalSpaceDistance / 2.0f && playerHeadToHead - 1.0f < playerPelvisToHead)
                {
                    interestPelvis -= 30.0f;
                    if (mainInterest == "Pelvis"){fuzzyLock = 2.5f;}
                }
                if (playerTipToPelvis < interactionDistance * 1.5f && mainInterest != "Tip")
                {
                    //interestArousal += 2.0f;
                    interestPelvis += 60.0f;
                    interestTip += 60.0f;
                    if (mainInterest == "Pelvis"){fuzzyLock = 2.5f;}
                }
                if (playerPelvisToHead < personalSpaceDistance)
                {
                    interestPelvis += 10.0f;
                }
                if (playerPelvisToHead > personalSpaceDistance && playerPelvisToHead < backgroundDistance)
                {
                    interestPelvis += 10.0f;
                }
                if (playerPelvisToHead > backgroundDistance)
                {
                    interestPelvis -= 50.0f;
                }
				if (interestArousal < 8.0f)
				{
					interestPelvis -= 100.0f;
				}
                interestPelvis -= (100.0f - pAgreeableness) / 10.0f;
                interestPelvis += (pExtraversion - 50.0f) / 5.0f;

                //Penis
                interestTip = interestTipBase;
                if ((mainInterest == "Penis" || mainOld == "Penis") && mainClock > gDuration && playerTipToHead > closeFaceDistance && playerTipToLBreast > closeFaceDistance && playerTipToRBreast > closeFaceDistance && playerTipToPelvis > closeFaceDistance * 3 && playerTipToLHand > interactionDistance && playerTipToRHand > interactionDistance)
                {
                    interestTip -= 20.0f;
					dbgPenis += "-Timeout ";
                }
                if (playerTipToHead < closeFaceDistance)
                {
                    //interestArousal += 2.0f;
                    interestTip += 100.0f;
                    if (mainInterest == "Penis"){fuzzyLock = 0.5f;}
					dbgPenis += "+CloseToFace ";
                }
                if (playerTipToHead < interactionDistance || playerTipToLBreast < interactionDistance || playerTipToRBreast < interactionDistance || playerTipToPelvis < interactionDistance)
                {
                    //interestArousal += 2.0f;
                    interestTip += 40.0f;
                    if (mainInterest == "Penis"){fuzzyLock = 0.25f;}
					dbgPenis += "+Interacting ";
                }
                if (playerTipMovement && headToFaceRot > lookPeripheralAngle)
                {
                    interestTip += 30.0f;
					dbgPenis += "+Move ";
                }
                if (playerTipMovement == false && playerTipToHead > closeFaceDistance)
                {
                    interestTip -= 10.0f;
                    if (mainInterest == "Penis"){fuzzyLock = 2.5f;}
					dbgPenis += "-NoMove ";
                }
                if (playerTipToHead < personalSpaceDistance)
                {
                    interestTip += 20.0f;
					dbgPenis += "+PSpace ";
                }
                if (playerTipToHead < playerLHandToHead && playerTipToHead < playerRHandToHead && (playerTipToPelvis < interactionDistance * 2 || playerTipToLBreast < interactionDistance || playerTipToRBreast < interactionDistance))
                {
                    interestTip += 20.0f;
                    if (mainInterest == "Penis"){fuzzyLock = 0.5f;}
					dbgPenis += "+SexAction ";
                }
                if (playerTipToHead > personalSpaceDistance && playerTipToHead < backgroundDistance)
                {
                    interestTip -= 10.0f;
					dbgPenis += "-AwareDist ";
                }
                if (playerTipToHead > backgroundDistance)
                {
                    interestTip -= 50.0f;
					dbgPenis += "-FarOff ";
                }
				if (playerHeadToFaceRot < lookDirectAngle*2.0f)
				{
					interestTip -= 50.0f;
					dbgPenis += "-LookAtFace ";
				}
                if (playerTipToHead < interactionDistance * 1.5f || playerTipToLHand < interactionDistance * 1.5f || playerTipToRHand < interactionDistance * 1.5f)
                {
                    interestTip += 10.0f;
                    if (mainInterest == "Penis"){fuzzyLock = 0.5f;}
					dbgPenis += "+SexInteract ";
                }
                else
                {
                    interestTip -= (pStableness) / 10.0f;
                }
				if (interestArousal < 7.0f)
				{
					interestTip -= 100.0f;
					dbgPenis += "-NotAroused ";
				}

                interestTip += (pExtraversion - 50.0f) / 5.0f;
            }

			
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
                if (currentInterest == "Face")
                {
                    interestUpdate = interestFace;
                    interestFace = interestFace - (10.0f * (mainClock / 5.0f));
                }

                mainValue = 0.0f;
                secondValue = 0.0f;
                if (interestFace > secondValue || playerHeadToHead < closeFaceDistance * 1.75f)
                {
                    if (interestFace > mainValue || playerHeadToHead < closeFaceDistance * 1.75f)
                    {
                        if (mainInterest != "Face")
                        {
                            mainOld = mainInterest;
                            mainSwitch = true;
                            //mouthSM.Switch(mBiteLip);
                        }
                        mainInterest = "Face";
                        mainValue = interestFace;

                        if (playerHeadToHead < closeFaceDistance * 1.5f)
                        {
                            mainValue = 150.0f;
                        }
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
                if (interestLHand > secondValue && (playerHandsUsable || person2Usable))
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
                        }
                        mainInterest = "LHand";
                        mainValue = interestLHand;
                    }
                    else
                    {
						if (interestLHand > 50.0f)
						{
							if (secondInterest != "LHand" && secondOld != "LHand")
							{
								secondOld = secondInterest;
								secondSwitch = true;
								if (interestArousal < 5.0f && morphBrowAction == false && Random.Range(0.0f,100.0f) > 50.0f)
								{
									browSM.Switch(bOneRaise);
								}
							}
							secondInterest = "LHand";
							secondValue = interestLHand;
						}
                    }
                }
                if (interestRHand > secondValue && (playerHandsUsable || person2Usable))
                {
                    if (interestRHand > mainValue)
                    {
						if (interestRHand > 50.0f)
						{
							if (mainInterest != "RHand" && secondOld != "RHand")
							{
								mainOld = mainInterest;
								mainSwitch = true;
								if (morphBrowAction == false)
								{
									browSM.Switch(bRaised);
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
                            if (interestArousal < 5.0f && morphBrowAction == false && Random.Range(0.0f,100.0f) > 50.0f)
                            {
                                browSM.Switch(bOneRaise);
                            }
                        }
                        secondInterest = "RHand";
                        secondValue = interestRHand;
                    }
                }
                if (interestPelvis > secondValue && person2Usable)
                {
                    if (interestPelvis > mainValue)
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
                if ((interestTip > secondValue || (playerTipToHead < closeFaceDistance || playerTipToPelvis < closeFaceDistance) && person2Usable && playerHeadToHead > closeFaceDistance))
                {
                    if (interestTip > mainValue || (playerTipToHead < closeFaceDistance || playerTipToPelvis < closeFaceDistance && playerHeadToHead > closeFaceDistance))
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
                if (mainSwitch)
                {
                    mainClock = 0.0f;
                }
				
				if (playerLHandToHead < closeFaceDistance)
					{
					mainInterest = "LHand";
					mainValue = 100;
					secondInterest = "Face";
					secondValue = 80;
					}
				if (playerRHandToHead < closeFaceDistance)
					{
					mainInterest = "RHand";
					mainValue = 100;
					secondInterest = "Face";
					secondValue = 80;
					}
				if (secondInterest == "RHand" && interestRHand < 50.0f)
				{
					if (Random.Range(0.0f,100.0f) > 15.0f)
					{
						secondInterest = "RandomR";
					}
					else
					{
						secondInterest = "RandomF";
					}
				}
				if (secondInterest == "LHand" && interestLHand < 50.0f)
				{
					if (Random.Range(0.0f,100.0f) > 15.0f)
					{
						secondInterest = "RandomL";
					}
					else
					{
						secondInterest = "RandomF";
					}
				}
				
				if (secondInterest == "RandomR" || secondInterest == "RandomL" || secondInterest == "RandomF" || secondInterest == "RandomU")
				{
					if (Random.Range(0.0f,100.0f) > 55.0f)
					{
						if (Random.Range(0.0f,100.0f) > 70.0f)
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
					
                string oldInterest = currentInterest;
                //currentInterestLevel = 0.0f;
                if (mainInterest == "Pelvis")
                {
                    interestArousal += (0.1f * (0.1f + (pExtraversion / 100.0f))) * uiArousalSpeed.val;
                    interestValence += (0.025f * (0.1f + (pAgreeableness / 100.0f))) * uiValenceSpeed.val;
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
                    interestValence += (0.075f * (0.1f + (pAgreeableness / 100.0f))) * uiValenceSpeed.val;
                    currentInterest = "Tip";
                    currentInterestLevel = interestTip;
                    playerInterest += 5.0f;
                    if (lookAction == false)
                    {
                        if (playerTipToPelvis < interactionDistance * 1.5f)
                        {
                            lookSM.Switch(lSex);
                        }
                        else
                        {
                            if (playerTipToHead < interactionDistance * 1.2f)
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
                    interestArousal += (0.1f * (0.1f + ((100.0f - pExtraversion) / 100.0f))) * uiArousalSpeed.val;
                    interestValence += (0.025f * (0.1f + (pAgreeableness / 100.0f))) * uiValenceSpeed.val;
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
                    interestArousal += (0.1f * (0.1f + ((100.0f - pExtraversion) / 100.0f))) * uiArousalSpeed.val;
                    interestValence += (0.025f * (0.1f + (pAgreeableness / 100.0f))) * uiValenceSpeed.val;
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
				if ((playerLHandToHead < interactionDistance || playerLHandToLBreast < interactionDistance || playerLHandToRBreast < interactionDistance || playerLHandToPelvis < interactionDistance * 1.3f) || (playerRHandToHead < interactionDistance || playerRHandToLBreast < interactionDistance || playerRHandToRBreast < interactionDistance || playerRHandToPelvis < interactionDistance * 1.3f))
					{
						tempFloat = 1.0f;
					}
                if (mainInterest == "Face")
                {
                    interestArousal += (0.07f * (0.1f + ((100.0f - pExtraversion) / 100.0f))) * uiArousalSpeed.val;
					if (playerHeadToHead < closeFaceDistance * 1.5f)
					{
						interestValence += (0.75f * (0.1f + (Mathf.Clamp(pAgreeableness + (interestArousal * 2.0f),0.0f,100.0f) / 100.0f))) * uiValenceSpeed.val;
					}
					else
					{
						interestValence += (0.15f * (0.1f + (Mathf.Clamp(pAgreeableness + (interestArousal * 2.0f),0.0f,100.0f) / 100.0f))) * uiValenceSpeed.val;
					}
                    currentInterest = "Face";
                    currentInterestLevel = interestFace;
                    if (lookAction == false)
                    {
                        if (playerHeadToPelvis < interactionDistance && playerHeadMovement)
                        {
                            lookSM.Switch(lSex);
                        }
                        else
                        {
                            if (interestArousal + interestValence < 7)
                            {
								if (playerHeadToFaceRot < lookDirectAngle * 2.0f)
								{
									lookSM.SwitchRandom(new State[] {
													lCasual,
													lInquisitive
												});
								}
								else
								{
									lookSM.SwitchRandom(new State[] {
													//lBored,
													lCasual,
													lCasual,
													lCasual
												});
								}
                            }
                            else
                            {
                                if (interestArousal + interestValence > 15)
                                {
                                    lookSM.SwitchRandom(new State[] {
                                                    lIntense,
                                                    lIntense,
                                                    lPlayful
                                                });
                                }
                                else
                                {
									if (tempFloat == 1.0f && Random.Range(0.0f,100.0f) > 50.0f)
									{
										lookSM.SwitchRandom(new State[] {
														lFeel,
														lFeel,
														lPlayful,
														lInquisitive,
														lFeel
													});
									}
									else
									{
										lookSM.SwitchRandom(new State[] {
														lCasual,
														lCasual,
														lPlayful,
														lInquisitive,
														lInquisitive
													});
									}
                                }
                            }
                        }
                    }
                    playerInterest += 0.5f;
                }


                if ((interestFace <= 40.0f && interestLHand <= 40.0f && interestRHand <= 40.0f && interestPelvis <= 40.0f && interestTip <= 40.0f) || mainInterest == "RandomF" || mainInterest == "RandomU" || mainInterest == "RandomL" || mainInterest == "RandomR" || currentLook == "Bored" || currentLook == "DayDream")
                {
                    interestArousal -= (0.07f * (0.1f + ((100.0f - pStableness) / 100.0f))) * uiArousalSpeed.val;
                    interestValence -= (0.025f * (0.1f + ((100.0f - pAgreeableness) / 100.0f))) * uiValenceSpeed.val;
					if (lookAction && (currentLook == "Bored" || currentLook == "DayDream") && interestArousal > 4.5f)
					{
                        lookSM.SwitchRandom(new State[] {
                                        lPlayful,
                                        lInquisitive,
                                        lCasual,
                                    });
					}
					else
					{
						if (lookAction == false && playerHeadToHead > personalSpaceDistance)
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
                    if (mainInterest == "RandomF" || mainInterest == "RandomU" || mainInterest == "RandomL" || mainInterest == "RandomR")
                    {
                        currentInterest = mainInterest;
                    }
                    else
                    {
                        if (Random.Range(0.0f, 100.0f) > 20.0f)
                        {
                            currentInterest = "RandomF";
                        }
                        else
                        {
                            if (Random.Range(0.0f, 100.0f) > 10.0f)
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
					mainInterest = currentInterest;
					secondInterest = currentInterest;
                    currentInterestLevel = 40.0f;
                    playerInterest -= 0.5f;
                }
                if (currentInterest != oldInterest && eyeClock > 0.25f && amGlancing == false)
                {
                    eyesSM.Switch(eBlink);
                    eyeClock = 0.0f;
                }
                if (playerHeadToHead < closeFaceDistance && interestKissing == false && headToFaceRot < lookDirectAngle * 5.0f)
                {
                    lookSM.Switch(lKissing);
                }
                float clockBase = Random.Range(1.0f, 3.0f);
                if (mainSwitch || secondSwitch)
                {
                    clockBase = Random.Range(3.0f, 5.0f);
                }
                interestClock = Mathf.Clamp(clockBase * (pStableness / 100.0f), 1.0f, 4.0f);
            }
            else
            {
                interestClock -= Time.fixedDeltaTime * uiInterestSpeed.val;
            }

            if (lookAction == false)
            {
                if (playerTipToHead < interactionDistance * 1.55f)
                {
                    lookSM.Switch(lSucking);
                }
            }

            if (currentInterestLevel > 70.0f)
            {
                playerInterest += (currentInterestLevel - 70.0f) / 500.0f;
            }
            else
            {
                playerInterest -= 0.05f;
            }
            playerInterest = Mathf.Clamp(playerInterest, 0.0f, 100.0f);


            interestArousal = Mathf.Clamp(interestArousal - (0.0035f * uiArousalSpeed.val), 3.0f, 10.0f);
			if (playerHeadToHead > personalSpaceDistance)
			{
				interestValence = Mathf.Clamp(interestValence - (0.0041f * uiValenceSpeed.val), 2.0f, 10.0f);
			}
			else
			{
				interestValence = Mathf.Clamp(interestValence - (0.0023f * uiValenceSpeed.val), 2.0f, 10.0f);
			}

            Vector3 lookAtPosition = eyeController.transform.TransformPoint(new Vector3(0.0f, 0.0f, 0.0f));

            if (currentInterest == "RandomF")
            {
                //headController.transform.LookAt(randomPointForward);
                lookAtPosition = randomPointForward;
                eyeController.transform.position = randomPointForward;
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
                if (interestKissing)
                {
                    lookAtPosition = playerHeadTransform.TransformPoint(new Vector3(0.0f, 0.02f, 0.0f));
                    fuzzyLock = 0.0f;
                }
                else
                {
                    lookAtPosition = playerHeadTransform.TransformPoint(new Vector3(0.0f, 0.0f, 0.0f));
                    fuzzyLock = 10.0f;
                }
            }
            if (currentInterest == "Pelvis")
            {
                lookAtPosition = playerPelvisController.transform.TransformPoint(new Vector3(0.0f, 0.0f, 0.0f));
                fuzzyLock = 2.0f;
            }
            if (currentInterest == "LHand")
            {
				if (playerLHandToHead < closeFaceDistance)
				{
					lookAtPosition = playerLHandTransform.TransformPoint(new Vector3(0.2f, 0.0f, 0.2f));
				}
				else
				{
					lookAtPosition = playerLHandTransform.TransformPoint(new Vector3(0.0f, 0.0f, 0.0f));
				}
                fuzzyLock = 5.0f;
            }
            if (currentInterest == "RHand")
            {
				if (playerRHandToHead < closeFaceDistance)
				{
					lookAtPosition = playerRHandTransform.TransformPoint(new Vector3(-0.2f, 0.0f, 0.2f));
				}
				else
				{
					lookAtPosition = playerRHandTransform.TransformPoint(new Vector3(0.0f, 0.0f, 0.0f));
				}
                fuzzyLock = 5.0f;
            }
            if (currentInterest == "Tip")
            {
                if (playerTipToHead < interactionDistance)// && Random.Range(0.0f,100.0f) > pExtraversion + 25.0f)
                {
                    if (headToFaceRot < lookPeripheralAngle / 2.0f && Random.Range(0.0f, 100.0f) > 70.0f)
                    {
                        lookAtPosition = playerHeadTransform.TransformPoint(new Vector3(0.0f, 0.0f, 0.0f));
                        fuzzyLock = 3.0f;
                    }
                    else
                    {
                        lookAtPosition = playerPelvisController.followWhenOff.TransformPoint(new Vector3(0.0f, 0.0f, -0.05f));
                        fuzzyLock = 0.0f;
                    }
                }
                else
                {
                    lookAtPosition = playerTipBaseController.followWhenOff.TransformPoint(new Vector3(0.0f, 0.02f, 0.0f));
                    fuzzyLock = 2.0f;
                }
            }
            if (secondClock <= 0.0f && amGlancing)
            {
                amGlancing = false;
            }
			
			if (playerHeadToFaceRot < lookDirectAngle && amGlancing == false && interestArousal < 7.0f && interestValence < 9.5f && (playerHeadToHead < personalSpaceDistance || pExtraversion < 25.0f))
			{
				//SuperController.singleton.ClearMessages();
				//SuperController.LogMessage("Eye Contact (" + (gAvoidance / 4.0) + "/" + ((100.0f-gAvoidance) / 10.0f) + ")" + gAvoidanceClock + "/" + gAvoidingClock + "/" + gAvoid);
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
					if ( mEyesClosedLeftValue < 0.5f)
					{
						tempFloat = 3.0f;
					}
					gAvoidingClock -= Time.fixedDeltaTime * ((1.5f * tempFloat) * (interestArousal / 10.0f));
				}
				else
				{
					if (gAvoidingClock <= 0.0f && gAvoid == 1.0f)
					{
						gAvoid = 0.0f;
						gAvoidanceClock = 0.0f;
						gAvoidingClock = 0.0f;
						eyesSM.Switch(eBlink);
					}
				}
				if (gAvoidanceClock < gAvoidance / 4.0f && gAvoid == 0.0f)
				{
					if (playerHeadToHead >= closeFaceDistance * 1.5f && amGlancing == false && mEyesClosedLeftTarget < 0.5f && currentLook != "Intense" && currentLook != "Playful")
					{
		  			  gAvoidanceClock += Time.fixedDeltaTime * (2.0f * (1.0f-(interestArousal / 10.0f)));
					}
					if (amGlancing == true && gAvoidanceClock > 0.0f)
					{
						gAvoidanceClock -= Time.fixedDeltaTime / 2.0f;
					}
				}
				else
				{
					if (gAvoidanceClock >= gAvoidance / 4.0f && gAvoid == 0.0f)
					{
						gAvoid = 1.0f;
						gAvoidingClock = (100.0f-gAvoidance) / Mathf.Lerp(3.0f,20.0f,pExtraversion / 100.0f);
						//eyesSM.Switch(eClosed);
						mouthSM.Switch(mSmirk);
						if (secondInterest == "RandomL" || secondInterest == "RandomR")
						{
							if (secondInterest == "RandomL")
							{
								if (lookAwaySide == "right")
								{
									eyesSM.Switch(eBlink);
								}
								lookAwaySide = "left";
							}
							else
							{
								if (lookAwaySide == "left")
								{
									eyesSM.Switch(eBlink);
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
								}
								lookAwaySide = "left";
							}
							else
							{
								if (lookAwaySide == "left")
								{
									eyesSM.Switch(eBlink);
								}
								lookAwaySide = "right";
							}
						}
					}
				}
			}
			else
			{
				gAvoid = 0.0f;
				gAvoidanceClock = 0.0f;
				gAvoidingClock = 0.0f;
			}
			
			if (playerHeadToHead > personalSpaceDistance && playerInterest <= 10.0f && amGlancing == false && interestKissing == false)
			{
				//gAvoid = 1.0f;
			}

			tempFloat = 1.0f;
			if (amGlancing == true)
			{
				tempFloat = 0.0f;
			}
            //			if (((Random.Range(0.0f,100000.0f) / 1000.0f > Mathf.Clamp(100.00f - ((100.0f-pStableness)/100.0f),99.15f,99.899f) || secondClock > 0.0f) && mainClock > Mathf.Clamp(6.0f * ((100.0f-pExtraversion)/100),1.5f,5.0f) && playerHeadToHead > closeFaceDistance) || amGlancing == true)
            if ((Random.Range(0.0f, 100.0f) > Mathf.Lerp(99.8f,99.95f,pStableness/100.0f) || secondClock > 0.0f || amGlancing == true) && mainInterest != secondInterest)// && secondInterest != "RandomL" && secondInterest != "RandomR")
            //if (1.0f == 1.0f)
			{
				
                if (secondInterest == "Face" && headToFaceRot < lookNoAwarenessAngle)
                {
                    //if (gAvoid == 0.0f)
                    //{
                    //lookAtPosition = playerHeadController.transform.TransformPoint(new Vector3(0.0f,0.0f,0.0f));
                    //}


                    eyeController.transform.position = playerHeadTransform.position;
                    eyeController.transform.rotation = playerHeadTransform.rotation;
                    eyeController.transform.Translate(0.0f, 0.03f, 0.07f);
                    amGlancing = true;
                    //fuzzyLock = 1.5f;
                }
                if (secondInterest == "LHand" && headToLHandRot < lookNoAwarenessAngle && interestLHand > 50.0f)
                {
                    //lookAtPosition = playerLHandController.transform.TransformPoint(new Vector3(0.0f,0.0f,0.0f));
					eyeController.transform.position = playerLHandTransform.TransformPoint(new Vector3(0.2f, 0.0f, 0.2f));

                    amGlancing = true;
                    //fuzzyLock = 2.5f;
                }
                if (secondInterest == "RHand" && headToRHandRot < lookNoAwarenessAngle && interestRHand > 50.0f)
                {
                    //lookAtPosition = playerRHandController.transform.TransformPoint(new Vector3(0.0f,0.0f,0.0f));
                    eyeController.transform.position = playerRHandTransform.TransformPoint(new Vector3(-0.2f, 0.0f, 0.2f));

                    amGlancing = true;
                    //fuzzyLock = 2.5f;
                }
                if (secondInterest == "Pelvis")
                {
                    //lookAtPosition = playerPelvisController.transform.TransformPoint(new Vector3(0.0f,0.0f,0.0f));
                    eyeController.transform.position = playerPelvisController.followWhenOff.position;
                    eyeController.transform.rotation = chestController.followWhenOff.rotation;
                    eyeController.transform.Translate(0.0f, 0.0f, 1.0f);
                    amGlancing = true;
                    //fuzzyLock = 3.5f;
                }
                if (secondInterest == "Tip" && headToTipRot < lookNoAwarenessAngle)
                {
                    //lookAtPosition = playerTipController.transform.TransformPoint(new Vector3(0.0f,0.0f,0.0f));
                    eyeController.transform.position = playerTipController.followWhenOff.position;
                    eyeController.transform.rotation = chestController.followWhenOff.rotation;
                    eyeController.transform.Translate(0.0f, 0.0f, 1.0f);
                    amGlancing = true;
                    //fuzzyLock = 3.5f;
                }
                if (secondInterest == "RandomL")
                {
                    //lookAtPosition = playerPelvisController.transform.TransformPoint(new Vector3(0.0f,0.0f,0.0f));
                    eyeController.transform.position = randomPointLeft;
                    amGlancing = true;
                    //fuzzyLock = 3.5f;
                }
                if (secondInterest == "RandomR")
                {
                    //lookAtPosition = playerPelvisController.transform.TransformPoint(new Vector3(0.0f,0.0f,0.0f));
                    eyeController.transform.position = randomPointRight;
                    amGlancing = true;
                    //fuzzyLock = 3.5f;
                }
                if (secondInterest == "RandomU")
                {
                    //lookAtPosition = playerPelvisController.transform.TransformPoint(new Vector3(0.0f,0.0f,0.0f));
                    eyeController.transform.position = randomPointUp;
                    amGlancing = true;
                    //fuzzyLock = 3.5f;
                }
                if (secondInterest == "RandomF")
                {
                    //lookAtPosition = playerPelvisController.transform.TransformPoint(new Vector3(0.0f,0.0f,0.0f));
                    eyeController.transform.position = randomPointForward;
                    amGlancing = true;
                    //fuzzyLock = 3.5f;
                }
                if (secondClock <= 0.0f && amGlancing)
                {
                    secondClock = Random.Range(0.55f, 1.25f + (3.0f * ((100.0f - pExtraversion) / 100.0f)));
                    ////LogError("Glancing");
                }
                if (amGlancing == false)
                {
                    secondClock = 0.0f;
                }
                else
                {
					if (tempFloat == 1.0f && eyeClock > 0.5f)
					{
						eyesSM.Switch(eBlink);
					}
					gHeadSpeed = 2.0f;
                }

            }
            else
            {
                gHeadSpeed = 1.25f;
                if (mainInterest == "Face" && (gAvoid != 1.0f || Random.Range(0.0f, 1.00f) < interestArousal / 10.0f))
                {
                    eyeController.transform.position = playerHeadTransform.position;
                    eyeController.transform.rotation = playerHeadTransform.rotation;

                }
                if (mainInterest == "Pelvis")
                {
                    eyeController.transform.position = playerPelvisController.followWhenOff.position;
                }
                if (mainInterest == "LHand")
                {
                    //if (playerLHandToHead < closeFaceDistance)
                    //{
                    //    eyeController.transform.position = playerHeadTransform.position;
                    //    eyeController.transform.rotation = playerHeadTransform.rotation;
                    //    eyeController.transform.Translate(0.0f, 0.03f, 0.07f);
                    //}
                    //else
                    //{
                        eyeController.transform.position = playerLHandTransform.position;
                        eyeController.transform.rotation = playerLHandTransform.rotation;
                        //eyeController.transform.Translate(-0.1f, 0.0f, 0.0f);
                    //}
                }
                if (mainInterest == "RHand")
                {
                    //if (playerRHandToHead < minFaceDistance)
                    //{
                    //    eyeController.transform.position = playerHeadTransform.position;
                    //    eyeController.transform.rotation = playerHeadTransform.rotation;
                    //    eyeController.transform.Translate(0.0f, 0.03f, 0.07f);
                    //}
                    //else
                    //{
                        eyeController.transform.position = playerRHandTransform.position;
                        eyeController.transform.rotation = playerRHandTransform.rotation;
                        //eyeController.transform.Translate(0.1f, 0.0f, 0.0f);
                    //}
                }
                if (mainInterest == "Tip")
                {
                    if (playerTipToHead < closeFaceDistance || headToTipRot > 45.0f)
                    {
                        if (playerTipToHead < interactionDistance || headToTipRot > 45.0f)
                        {
                            eyeController.transform.rotation = playerHeadTransform.rotation;
                            eyeController.transform.position = playerHeadTransform.position;
                        }
                        else
                        {
                            eyeController.transform.position = playerTipController.followWhenOff.position;
                            eyeController.transform.rotation = playerTipController.followWhenOff.rotation;
                            eyeController.transform.Translate(0.0f, 0.05f, -0.17f);
                        }
                    }
                    else
                    {
                        eyeController.transform.position = playerTipController.followWhenOff.position;
                        eyeController.transform.rotation = playerTipController.followWhenOff.rotation;
                    }
                }
            }
				
            Vector3 centrePoint = chestController.transform.position;
            Vector3 eyePoint = eyeController.transform.position;
            float c2eActual = Vector3.Distance(centrePoint, eyePoint);
            centrePoint.y = 0.0f;
            eyePoint.y = 0.0f;
            float c2eDist = Vector3.Distance(centrePoint, eyePoint);
            if (c2eDist < closeFaceDistance)
            {
                eyeController.transform.position = eyeController.transform.position + (chestController.transform.forward * 0.1f);
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

            //eyeController.transform.Translate(randomX,randomY,0.0f);
            if (eyeUpdateClock >= eyeUpdateTime)
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
            eyeController.transform.Translate(saccadeOffset * (Vector3.Distance(headController.transform.position, eyeController.transform.position) / 100.0f));
			
			if ((playerLHandToLBreast < interactionDistance * 1.1f || playerLHandToRBreast * 1.1f < interactionDistance || playerLHandToPelvis < interactionDistance * 2.0f) && (playerRHandToLBreast < interactionDistance * 1.1f || playerRHandToRBreast < interactionDistance * 1.1f || playerRHandToPelvis < interactionDistance * 2.0f))
			{
				if (playerHeadToFaceRot > lookDirectAngle*1.5f && playerHeadToHead > closeFaceDistance * 1.2f)// && (playerLHandMovement || playerRHandMovement))
				{
					if (currentLook == "Feel" || currentLook == "Casual")
					{
						gAvoid = 1.0f;
					}
				}
			}
			
			if (gAvoid == 1.0f)
			{
				interestArousal += 0.02f;
				//SuperController.LogMessage("Avoiding" + gAvoidance);
				if (lookAwaySide == "right")
				{
					mainInterest = "RandomR";
					lookAtPosition = randomPointRight;//playerRHandTransform.TransformPoint(new Vector3(0.0f, 0.0f, 0.0f));
					fuzzyLock = 10.0f;
					eyeController.transform.position = randomPointRight;//playerRHandTransform.position;
					gHeadRollTarget = Random.Range(-25.0f,-5.0f);
				}
				else
				{
					mainInterest = "RandomL";
					lookAtPosition = randomPointLeft;//playerLHandTransform.TransformPoint(new Vector3(0.0f, 0.0f, 0.0f));
					fuzzyLock = 10.0f;
					eyeController.transform.position = randomPointLeft;//playerLHandTransform.position;
					gHeadRollTarget = Random.Range(25.0f,5.0f);
				}
			}
			
			if (playerLHandToHead < closeFaceDistance || playerRHandToHead < closeFaceDistance)
				{
				eyeController.transform.position = playerHeadTransform.position;
                eyeController.transform.rotation = playerHeadTransform.rotation;
				}
			
			if (interestKissing)
			{
				fuzzyLock = 0.0f;
				gHeadSpeed = 0.2f;
			}
			
		    if (Vector3.Distance(oldEyePos, eyeController.transform.position) > 0.7f && eyeClock > 0.35f)
			{
                    eyesSM.Switch(eBlink);
                    eyeClock = 0.0f;
			}
			
            interestFace = Mathf.Clamp(interestFace, 0.0f, maxInterestLevel - 1.5f);
            interestLHand = Mathf.Clamp(interestLHand, 0.0f, maxInterestLevel - 1.0f);
            interestRHand = Mathf.Clamp(interestRHand, 0.0f, maxInterestLevel - 1.0f);
            interestPelvis = Mathf.Clamp(interestPelvis, 0.0f, maxInterestLevel + 5.0f);
            interestTip = Mathf.Clamp(interestTip, 0.0f, maxInterestLevel + 10.0f);
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


            float headToEyeController = Vector3.Angle(eyeController.transform.position - headController.transform.position, headController.followWhenOff.forward);
            if (headToEyeController > eyesNonDirectAngle)
            {
                eyesNonDirectClock += Time.fixedDeltaTime * Mathf.Lerp(1.5f, 6.0f, interestValence / 10.0f);
            }
            else
            {
                eyesNonDirectClock = 0.0f;
            }


            if (10.0f - ((100.0f - pExtraversion) / 10.0f) - eyesNonDirectClock < 0.0f || currentLook == "Intense" || playerHeadToHead < closeFaceDistance * 1.3f)
            {
                fuzzyLock = 0.0f;
            }
			
            // adjust angles
			
			tempFloat = peronalityAdjustH;
			if (Mathf.Abs(peronalityAdjustH - lastAdjustH) > 0.15f)
			{
				if (lastAdjustH < peronalityAdjustH)
				{
					peronalityAdjustH = lastAdjustH + 0.05f;
				}
				else
				{
					peronalityAdjustH = lastAdjustH - 0.05f;
				}
			}
			lastAdjustH = tempFloat;
			lastAdjustV = peronalityAdjustV;
			tempFloat = Mathf.Clamp(playerHeadToHead - 0.1f, 0.0f, 1.0f);
			tempFloat = Mathf.Lerp(30.00f,60.0f, tempFloat);
            targetH = Mathf.Clamp(targetH + (peronalityAdjustH * uiGazeVariation.val), -tempFloat * Mathf.Deg2Rad, tempFloat * Mathf.Deg2Rad);
			tempFloat = Mathf.Clamp(playerHeadToHead - 0.1f, 0.0f, 1.0f);
			tempFloat = Mathf.Lerp(30.00f,60.0f, tempFloat);
            targetV = Mathf.Clamp(targetV + (peronalityAdjustV * uiGazeVariation.val), -tempFloat * Mathf.Deg2Rad, tempFloat * Mathf.Deg2Rad);
            float adjustedSpeed = Mathf.Clamp((Mathf.Lerp(gHeadSpeed * 6.0f,gHeadSpeed * 2.0f,(interestArousal+interestValence)/10.0f) - 0.5f + (closeFaceDistance / playerHeadToHead)) / uiGazeSpeed.val,0.1f, 30.0f);
			if (gAvoid == 1.0f && interestValence < 5.0f)
			{
				adjustedSpeed = adjustedSpeed * 7.0f;
			}
            if (Mathf.Abs(actualH - targetH) < (30.0f * (pAgreeableness / 100.0f) * fuzzyLock) * Mathf.Deg2Rad || Mathf.Abs(actualH - targetH) > 90.0f * Mathf.Deg2Rad)// || eyesSM.CurrentState == eClosed)
            {
                targetH = actualH + (velocityH * 0.2f);
            }
            if (Mathf.Abs(actualV - targetV) < (20.0f * (pExtraversion / 100.0f) * fuzzyLock) * Mathf.Deg2Rad || Mathf.Abs(actualV - targetV) > 60.0f * Mathf.Deg2Rad)// || eyesSM.CurrentState == eClosed)
            {
                targetV = actualV + (velocityV * 0.2f);
            }
            actualH = Mathf.SmoothDamp(actualH, targetH, ref velocityH, adjustedSpeed, Mathf.Infinity, Time.fixedDeltaTime);
            actualV = Mathf.SmoothDamp(actualV, targetV, ref velocityV, adjustedSpeed, Mathf.Infinity, Time.fixedDeltaTime);

            // recombine
            actualDir = RecombineDirection(actualH, actualV);
            targetDir = RecombineDirection(targetH, targetV);
            actualDir = reference.TransformDirection(actualDir);

            //head.eulerAngles.z = curRotation.z;
            head.transform.LookAt(head.transform.position + actualDir, headController.followWhenOff.position - chestController.followWhenOff.position);
			if ((peronalityAdjustH < 0.0f && gHeadRollTarget < 0.0f) || (peronalityAdjustH > 0.0f && gHeadRollTarget > 0.0f))
			{
				gHeadRollTarget = -gHeadRollTarget;
			}
			tempFloat = (800.0f / uiGazeSpeed.val) / (Mathf.Max(Mathf.Abs(gHeadRollTarget - gHeadRoll), 1.0f) / 10.0f);
			if (interestKissing)
			{
				tempFloat = 100.0f;
			}
            if (gHeadRoll < gHeadRollTarget)
            {
                gHeadRoll = Mathf.Min(gHeadRoll + ((gHeadRollTarget - gHeadRoll) / tempFloat), 45.0f);
            }
            if (gHeadRoll > gHeadRollTarget)
            {
                gHeadRoll = Mathf.Max(gHeadRoll - ((gHeadRoll - gHeadRollTarget) / tempFloat), -45.0f);
            }

            // apply roll
            Vector3 eulerAngles = head.transform.localEulerAngles;
            eulerAngles.z += gHeadRoll;
            head.transform.localEulerAngles = eulerAngles;


            eulerAngles = head.transform.eulerAngles;
            Vector3 forwardEuler = chestController.transform.forward;
            Vector3 difference = new Vector3(0.0f, 0.0f, 0.0f);
            difference.x = (Mathf.DeltaAngle(forwardEuler.x, eulerAngles.x) * 0.45f) + sexActionNeckX;
            difference.y = Mathf.DeltaAngle(forwardEuler.y, eulerAngles.y) * 0.65f;
            difference.z = Mathf.DeltaAngle(forwardEuler.z, eulerAngles.z) * 0.05f;
            //neckController.transform.eulerAngles = difference;
            eulerAngles.x += sexActionNeckX;
            neckController.transform.eulerAngles = eulerAngles;

				if ((playerLHandToHead < closeFaceDistance && playerLHandMovement) || (playerRHandToHead < closeFaceDistance && playerRHandMovement))
					{
						if (eyeClock > 0.75f)
						{
						eyesSM.Switch(eClosed);
						}
					}
			
			SuperController.singleton.ClearMessages();
			SuperController.LogMessage("Current Emotion :" + currentLook);
			SuperController.LogMessage("Arousal : " + interestArousal.ToString());
			SuperController.LogMessage("Happiness : " + interestValence.ToString());
			SuperController.LogMessage("Main Interest : " + mainInterest);
			SuperController.LogMessage("Second Interest : " + secondInterest);
			SuperController.LogMessage("");
			SuperController.LogMessage("Face : " + interestFace.ToString());
			SuperController.LogMessage("LHand : " + interestLHand.ToString());
			SuperController.LogMessage("RHand : " + interestRHand.ToString());
			SuperController.LogMessage("Pelvis : " + interestPelvis.ToString());
			SuperController.LogMessage("Player : " + playerInterest.ToString());
			SuperController.LogMessage("Tip : " + interestTip.ToString());
			SuperController.LogMessage("");
			SuperController.LogMessage("Head : " + dbgHead);
			SuperController.LogMessage("LHand : " + dbgLHand);
			SuperController.LogMessage("RHand : " + dbgRHand);
			SuperController.LogMessage("Penis : " + dbgPenis);
			SuperController.LogMessage("Head Distance : " + playerHeadToHead.ToString());
			SuperController.LogMessage("Eye Contact : " + gAvoidanceClock + "/" + gAvoidingClock);
			if (amGlancing)
			{
				SuperController.LogMessage("Gaze : Glancing");
			}
			else
			{
				if (gAvoid == 1.0f)
				{
					SuperController.LogMessage("Gaze : Avoiding");
				}
				else
				{
					if (Mathf.Abs(peronalityAdjustH) > 0.5f || fuzzyLock > 5.0f)
					{
						SuperController.LogMessage("Gaze : Indirect");
					}
					else
					{
						SuperController.LogMessage("Gaze : Direct");
					}
				}
			}
			SuperController.LogMessage("Gaze Variation Horizontal : " + (peronalityAdjustH * Mathf.Rad2Deg));
			SuperController.LogMessage("Gaze Variation Vertical : " + (peronalityAdjustV * Mathf.Rad2Deg));
			SuperController.LogMessage("Gaze Head Roll : " + gHeadRoll);
			SuperController.LogMessage("Gaze Fuzzy Lock : " + fuzzyLock);
			SuperController.LogMessage("");
			SuperController.LogMessage("Flirt Morph : " + mFlirtingValue);
			SuperController.LogMessage("Happy Morph : " + mHappyValue);
			SuperController.LogMessage("Excitement Morph : " + mExcitementValue);
			SuperController.LogMessage("Glare Morph : " + mGlareValue);
			SuperController.LogMessage("Smile Full Face Morph : " + mSmileFullFaceValue);
			SuperController.LogMessage("Smile Open Full Face Morph : " + mSmileOpenFullFaceValue);
			SuperController.LogMessage("Smile Simple Left Morph : " + mSmileSimpleLeftValue);
			SuperController.LogMessage("Smile Simple Right Morph : " + mSmileSimpleRightValue);

			
			/*if (lookSM.CurrentState == lCasual)
			{
				SuperController.LogMessage("Look: Casual");
			}
			if (lookSM.CurrentState == lInquisitive)
			{
				SuperController.LogMessage("Look: Inquisitive");
			}
			if (lookSM.CurrentState == lPlayful)
			{
				SuperController.LogMessage("Look: Playful");
			}
			if (lookSM.CurrentState == lSex)
			{
				SuperController.LogMessage("Look: Sex");
			}
			if (lookSM.CurrentState == LKissing)
			{
				SuperController.LogMessage("Look: Kissing");
			}
			if (lookSM.CurrentState == lSucking)
			{
				SuperController.LogMessage("Look: Blowjob");
			}
			if (lookSM.CurrentState == lIntense)
			{
				SuperController.LogMessage("Look: Intense");
			}
			if (lookSM.CurrentState == lDayDream)
			{
				SuperController.LogMessage("Look: DayDream");
			}
			if (lookSM.CurrentState == lFeel)
			{
				SuperController.LogMessage("Look: Feel");
			}
			if (lookSM.CurrentState == lBored)
			{
				SuperController.LogMessage("Look: Bored");
			}*/
			
            /*if (debugClock > 10.0f)
            {
                //LogError("Current:" + currentInterest);
                //LogError(interestUpdate.ToString());
                LogError(playerInterest.ToString());
                //LogError("Main:" + mainInterest);
                //LogError("Second:" + secondInterest);
                //LogError("Face:" + interestFace.ToString());
                if (playerLHandMovement)
                {
                    //LogError("LHand(M):" + interestLHand.ToString());
                }
                else
                {
                    //LogError("LHand:" + interestLHand.ToString());
                }
                if (playerRHandMovement)
                {
                    //LogError("RHand(M):" + interestRHand.ToString());
                }
                else
                {
                    //LogError("RHand:" + interestRHand.ToString());
                }
                //LogError("Pelvis:" + interestPelvis.ToString());
                //LogError("Tip:" + interestTip.ToString());
                //LogError("Arousal:" + interestArousal.ToString());
                //LogError("Valence:" + interestValence.ToString());
                //LogError("Player:" + playerInterest.ToString());
                LogError(dbgLHand);
                if (playerHandsUsable)
                {
                    //LogError("Player Hands Usable");
                }
                else
                {
                    //LogError("Player Hands NOT Usable");
                }
                if (person2Usable)
                {
                    //LogError("Person2 Usable");
                }
                else
                {
                    //LogError("Person2 NOT Usable");
                }
                debugClock = 0.0f;
            }*/
        }

        //System States
        private static State sUpdate = new SUpdate();
        private static State sUpdatePerson = new SUpdatePerson();
        private static State sUpdatePlayer = new SUpdatePlayer();
        private static State sUpdatePlayerHands = new SUpdatePlayerHands();
        private static State sUpdatePerson2 = new SUpdatePerson2();

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
                Duration = 0.1f;
                Vector3 perp = Vector3.Cross(chestController.followWhenOff.eulerAngles, refAngle);
                float dir = Vector3.Dot(perp, chestController.followWhenOff.up);
                bool resetDir = false;
                if (Mathf.Abs(dir) > 40.0f || allSetup == false)
                {
                    resetDir = true;
                    refAngle = chestController.followWhenOff.eulerAngles;
                }

                if (Random.Range(0.0f, 100.0f) > 98.8f || resetDir)
                {
                    //generate random points
                    float rad = Random.Range(-15.0f, 15.0f) * Mathf.Deg2Rad;
                    Vector3 position = chestController.followWhenOff.right * Mathf.Sin(rad) + chestController.followWhenOff.forward * Mathf.Cos(rad);
                    randomPointForward = chestController.followWhenOff.position + (chestController.followWhenOff.right * Random.Range(-3.0f, 3.0f)) + (chestController.followWhenOff.up * Random.Range(2.0f, -1.5f)) + (chestController.followWhenOff.forward * 5.0f);
                }
                if (Random.Range(0.0f, 100.0f) > 98.8f || resetDir)
                {
                    //generate random points
                    float rad = Random.Range(15.0f, 65.0f) * Mathf.Deg2Rad;
                    Vector3 position = chestController.followWhenOff.right * Mathf.Sin(rad) + chestController.followWhenOff.up * Mathf.Cos(rad);
                    //randomPointLeft = chestController.followWhenOff.position + position * Random.Range(1.0f, 4.0f);
					randomPointLeft = chestController.followWhenOff.position + (chestController.followWhenOff.right * (-1.0f * Random.Range(0.75f, 7.0f))) + (chestController.followWhenOff.up * Random.Range(0.5f, -1.0f)) + (chestController.followWhenOff.forward * 3.0f);
                    //randomPointLeft = chestController.followWhenOff.position - (chestController.followWhenOff.right * 30.0f) - (chestController.followWhenOff.up * 7.0f) + (chestController.followWhenOff.forward * 30.0f);
                }
                if (Random.Range(0.0f, 100.0f) > 98.8f || resetDir)
                {
                    //generate random points
                    float rad = Random.Range(15.0f, 65.0f) * Mathf.Deg2Rad;
                    Vector3 position = chestController.followWhenOff.right * Mathf.Sin(rad) + chestController.followWhenOff.forward * Mathf.Cos(rad);
                    //randomPointRight = chestController.followWhenOff.position + position * Random.Range(1.0f, 4.0f) - (chestController.followWhenOff.up * 2.0f) + (chestController.followWhenOff.forward * 3.0f);
                    randomPointRight = chestController.followWhenOff.position + (chestController.followWhenOff.right * (1.0f * Random.Range(0.75f, 7.0f))) + (chestController.followWhenOff.up * Random.Range(0.5f, -1.0f)) + (chestController.followWhenOff.forward * 3.0f);
                }
                if (Random.Range(0.0f, 100.0f) > 98.8f || resetDir)
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
                Duration = 0.1f;
                //Person to Player/Person2 update
                personHeadTransform = headController.followWhenOff; //headController.transform;

                headToFaceRot = Vector3.Angle(playerFace - personHeadTransform.position, personHeadTransform.forward);
                playerHeadToFaceRot = Vector3.Angle(personHeadTransform.position - playerFace, playerHeadController.transform.forward);
                headToChestRot = Vector3.Angle(playerChest - personHeadTransform.position, personHeadTransform.forward);
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
                    headToLHandRot = Vector3.Angle(playerLHand - personHeadTransform.position, personHeadTransform.forward);
                    headToRHandRot = Vector3.Angle(playerRHand - personHeadTransform.position, personHeadTransform.forward);
                }
                if (person2Usable)
                {
                    headToPelvisRot = Vector3.Angle(playerPelvis - personHeadTransform.position, personHeadTransform.forward);
                    headToTipRot = Vector3.Angle(playerTip - personHeadTransform.position, personHeadTransform.forward);
                }
            }
            public override void OnTimeout()
            {
                systemSM.Switch(sUpdatePlayer);
            }
        }

        private class SUpdatePlayer : State
        {
            public override void OnEnter()
            {
                Duration = 0.1f;
                //Player/Person2 to Person update
                if (usePerson2)
                {
                    playerHeadTransform = playerHeadController.transform;
                }
                else
                {
                    playerHeadTransform = player;
                }

                personChestToHead = Vector3.Angle(playerHeadTransform.position - chestController.followWhenOff.position, chestController.followWhenOff.forward);

                playerToHead = Vector3.Angle(headController.followWhenOff.position - playerHeadTransform.position, playerHeadTransform.forward);
                playerToLBreast = Vector3.Angle(lBreastController.followWhenOff.position - playerHeadTransform.position, playerHeadTransform.forward);
                playerToRBreast = Vector3.Angle(rBreastController.followWhenOff.position - playerHeadTransform.position, playerHeadTransform.forward);
                playerToPelvis = Vector3.Angle(pelvisController.followWhenOff.position - playerHeadTransform.position, playerHeadTransform.forward);
                playerToLHand = Vector3.Angle(lHandController.followWhenOff.position - playerHeadTransform.position, playerHeadTransform.forward);
                playerToRHand = Vector3.Angle(rHandController.followWhenOff.position - playerHeadTransform.position, playerHeadTransform.forward);
                playerToLFoot = Vector3.Angle(lFootController.followWhenOff.position - playerHeadTransform.position, playerHeadTransform.forward);
                playerToRFoot = Vector3.Angle(rFootController.followWhenOff.position - playerHeadTransform.position, playerHeadTransform.forward);

                playerHeadToHead = Vector3.Distance(headController.followWhenOff.position, playerHeadTransform.position);
                playerHeadToLHand = Vector3.Distance(lHandController.followWhenOff.position, playerHeadTransform.position);
                playerHeadToRHand = Vector3.Distance(rHandController.followWhenOff.position, playerHeadTransform.position);
                playerHeadToLBreast = Vector3.Distance(lBreastController.followWhenOff.position, playerHeadTransform.position);
                playerHeadToRBreast = Vector3.Distance(rBreastController.followWhenOff.position, playerHeadTransform.position);
                playerHeadToPelvis = Vector3.Distance(pelvisController.followWhenOff.position, playerHeadTransform.position);
                if (person2Usable)
                {
                    playerPelvisToHead = Vector3.Distance(headController.followWhenOff.position, playerPelvisController.followWhenOff.position);
                    playerTipToHead = Vector3.Distance(headController.followWhenOff.position, playerTipController.followWhenOff.position);
                    playerTipToLHand = Vector3.Distance(lHandController.followWhenOff.position, playerTipController.followWhenOff.position);
                    playerTipToRHand = Vector3.Distance(rHandController.followWhenOff.position, playerTipController.followWhenOff.position);
                    playerTipToLBreast = Vector3.Distance(lBreastController.followWhenOff.position, playerTipController.followWhenOff.position);
                    playerTipToRBreast = Vector3.Distance(rBreastController.followWhenOff.position, playerTipController.followWhenOff.position);
                    playerTipToPelvis = Vector3.Distance(pelvisController.followWhenOff.position, playerTipController.followWhenOff.position);
                    if (Vector3.Distance(playerTip, playerTipPrev) > minTipMotion)
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
                Duration = 0.1f;
                //Player/Person2 hands to Person update

                if (playerHandsUsable || person2Usable)
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
                    personChestToLHand = Vector3.Angle(playerLHandTransform.position - chestController.followWhenOff.position, chestController.followWhenOff.forward);
                    personChestToRHand = Vector3.Angle(playerRHandTransform.position - chestController.followWhenOff.position, chestController.followWhenOff.forward);

                    playerToPLHand = Vector3.Angle(playerLHandTransform.position - playerHeadTransform.position, playerHeadTransform.forward);
                    playerToPRHand = Vector3.Angle(playerRHandTransform.position - playerHeadTransform.position, playerHeadTransform.forward);
                    playerLHandToHead = Vector3.Distance(headController.followWhenOff.position, playerLHandController.followWhenOff.TransformPoint(new Vector3(-0.1f,0.0f,0.0f)));
                    playerRHandToHead = Vector3.Distance(headController.followWhenOff.position, playerRHandController.followWhenOff.TransformPoint(new Vector3(0.1f,0.0f,0.0f)));
                    playerLHandToLHand = Vector3.Distance(lHandController.followWhenOff.position, playerLHandController.followWhenOff.TransformPoint(new Vector3(-0.1f,0.0f,0.0f)));
                    playerRHandToLHand = Vector3.Distance(lHandController.followWhenOff.position, playerRHandController.followWhenOff.TransformPoint(new Vector3(0.1f,0.0f,0.0f)));
                    playerLHandToRHand = Vector3.Distance(rHandController.followWhenOff.position, playerLHandController.followWhenOff.TransformPoint(new Vector3(-0.1f,0.0f,0.0f)));
                    playerRHandToRHand = Vector3.Distance(rHandController.followWhenOff.position, playerRHandController.followWhenOff.TransformPoint(new Vector3(0.1f,0.0f,0.0f)));
                    playerLHandToLBreast = Vector3.Distance(lBreastController.followWhenOff.position, playerLHandController.followWhenOff.TransformPoint(new Vector3(-0.1f,0.0f,0.0f)));
                    playerRHandToLBreast = Vector3.Distance(lBreastController.followWhenOff.position, playerRHandController.followWhenOff.TransformPoint(new Vector3(0.1f,0.0f,0.0f)));
                    playerLHandToRBreast = Vector3.Distance(rBreastController.followWhenOff.position, playerLHandController.followWhenOff.TransformPoint(new Vector3(-0.1f,0.0f,0.0f)));
                    playerRHandToRBreast = Vector3.Distance(rBreastController.followWhenOff.position, playerRHandController.followWhenOff.TransformPoint(new Vector3(0.1f,0.0f,0.0f)));
                    playerLHandToPelvis = Vector3.Distance(pelvisController.followWhenOff.position, playerLHandController.followWhenOff.TransformPoint(new Vector3(-0.1f,0.0f,0.0f)));
                    playerRHandToPelvis = Vector3.Distance(pelvisController.followWhenOff.position, playerRHandController.followWhenOff.TransformPoint(new Vector3(0.1f,0.0f,0.0f)));
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
                Duration = 0.1f;
                if (usePerson2 && person2Usable)
                {
                    playerFacePrev = playerFace;
                    playerFace = playerHeadController.transform.position;
                    playerFaceRotPrev = playerFaceRot;
                    playerFaceRot = playerHeadController.transform.eulerAngles;
                    playerChest = playerChestController.followWhenOff.position;
                    playerLHandPrev = playerLHand;
                    playerLHand = playerLHandController.followWhenOff.position;
                    playerRHandPrev = playerRHand;
                    playerRHand = playerRHandController.followWhenOff.position;
                    playerPelvis = playerPelvisController.followWhenOff.position;
                    playerTipPrev = playerTip;
                    playerTip = playerTipController.followWhenOff.position;
                    playerTipBase = playerTipBaseController.followWhenOff.position;
                }
                else
                {
                    playerFacePrev = playerFace;
                    playerFace = player.position;
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
                    if (playerHandsUsable && person2Usable == false)
                    {
                        playerLHand = playerVRLHand.position;
                        playerRHand = playerVRRHand.position;
                    }
                    if (person2Usable == false && playerHandsUsable == false)
                    {
                        playerLHand = playerFace;
                        playerRHand = playerFace;
                    }
                    if (person2Usable)
                    {
                        playerPelvis = playerPelvisController.followWhenOff.position;
                        playerTipPrev = playerTip;
                        playerTip = playerTipController.followWhenOff.position;
                        playerTipBase = playerTipBaseController.followWhenOff.position;
                    }
                }
                playerGround = new Vector3(playerFace.x, 0.0f, playerFace.z);
            }
            public override void OnTimeout()
            {
                systemSM.Switch(sUpdate);
				allSetup = true;
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
    }

}