using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class PlayerNetworkController : NetworkBehaviour
{
    public static PlayerNetworkController Instance { get; private set; }

    [SyncVar(hook = "AnimationStart")]
	AnimationController.ExpressionType animationTypeQueue = AnimationController.ExpressionType.Idle;
		 
    [SyncVar(hook = "UpdateMixFlag")]
    bool mixAnim = false;

	[SyncVar(hook = "FacePresetSet")]
	int serverCurrPreset = 0;

	[SyncVar(hook = "FaceColorSet")]
	int serverColorIndex = 3;

	[SyncVar(hook = "AdjustAnimationSlider")]
	float currValue = 0;

	[SyncVar(hook = "SetCurrentSelected" )]
	int currSelected = -1;

	[SyncVar(hook = "ChangeAge")]
	int ageVar = 30; 

	[SerializeField] Vector3 startingPos = new Vector3 (0, 0, 2);
	[SerializeField] Vector3 startingEuler = new Vector3 (0, 180, 0);


	void Awake()
	{
		Instance = this;
		transform.position = startingPos;
		transform.eulerAngles = startingEuler;
	}


	void Start()
    {
        if (ServerUIController.Instance != null)
        {
            if (isServer)
                ServerUIController.Instance.DefaultAnimationButtonPreset();

            serverCurrPreset = swapHeads.Instance.current;
            serverColorIndex = swapHeads.Instance.colorIndex;
        }
    }



	public override void OnStartClient ()
	{
		base.OnStartClient ();
	}
	
    public void ButtonPressed(int x)
    {
        AudioManager.Instance.playClick();
        animationTypeQueue = (AnimationController.ExpressionType)x;
		AnimationController.Instance.Queue = animationTypeQueue;
        if (isServer)
            UpdateAnimator();
    }
		
    public void ToggleMixFlag()
    {
		AnimationController.Instance.MixFlag = mixAnim = !mixAnim;
    }

	void AnimationStart(AnimationController.ExpressionType name)
    {
		if (animationTypeQueue != name) {
			animationTypeQueue = name;
			AnimationController.Instance.Queue = animationTypeQueue;
		}

		StartCoroutine(AnimationController.Instance.AnimationTransition());
    }

    void UpdateMixFlag(bool f)
    {
		AnimationController.Instance.MixFlag = mixAnim = f;
    }

    [Server]
    void UpdateAnimator()
    {
		StartCoroutine(AnimationController.Instance.AnimationTransition());
    }

	void AdjustAnimationSlider(float value)
	{
		
		AnimationController.Instance.AdjustAnimationSlider (currSelected, value);
	}


	void SetCurrentSelected(int curr)
	{
		if(currSelected != curr)
			currSelected = curr;
	}

	public int CurrentAnimationChosen
	{
		set{ currSelected = value; }
	}

	public float CurrentAnimationValue
	{
		set{ currValue = value; }
	}

	public int NextFace()
	{
		return swapHeads.Instance.next ();
	}

	public int PrevFace()
	{
		return swapHeads.Instance.prev ();
	}

	public int ServerColorIndex 
	{
		set{ serverColorIndex = (serverColorIndex + ((value < 0)? value + swapHeads.Instance.ColorLength : value)) % swapHeads.Instance.ColorLength; Debug.Log (serverColorIndex);}
	}

	public int ServerPresetIndex 
	{
		set { serverCurrPreset = (serverCurrPreset +  ((value < 0)? value + swapHeads.Instance.PresetLength : value)) % swapHeads.Instance.PresetLength; }
	}

	void FacePresetSet(int i )
	{
		if(swapHeads.Instance != null)
			swapHeads.Instance.FacePresetSet (i);
	}

	public int Age { get{ return ageVar;} set {ageVar = value; 
			if (isServer)
				ChangeAge (ageVar); }}


	void ChangeAge(int age)
	{
		float w = 100*(age - 30)/40;
		AnimationController.Instance.AdjustAnimationSlider (8, w);
	}
}
//	public void SetAnimationSlider(int i , float butt)
//	{
//		serverSliderChange.Insert(i, butt);
//	}