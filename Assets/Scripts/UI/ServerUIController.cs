using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ServerUIController : MonoBehaviour 
{
	[Header("AnimationVariables")]
	[SerializeField]GameObject animationPanel = null;
	[SerializeField]ServerUIAnimationButton defaultOnButton  = null;
	[SerializeField]ServerUIAnimationButton	mixButton = null;

	[SerializeField] Slider animationValue;
	[SerializeField] TMP_Dropdown animationChooser;
	[SerializeField] Toggle animationSwitch;
	/**
	 * new UI script adding:
	 * Enabled Animation ([]?)
	 * Curr Animation
	 * {int of animation index
	 * bool to update wether or not this animation is on
	 * float value of animation
	 * }AnimationControllerScript? 
	 * 
	***
	* ChangeSelectedAnimation(AnimationControllerScript)
	* {
		* 		INT CURR animation  = ACS.index;
		* 		set checkmark 
		* 		set float value;
		* }
	* 
	* updateCurrAnimation(bool)
	* {
		* 		currAnimation.set = bool
			* }
	* UpdateCurrAnimation(value)
	* {
		* 		currAnimation.value = value;
		* }
	* 
	* 
	*/


	public static ServerUIController Instance { get; private set; }
	List<ServerUIAnimationButton> currActiveButtons;


	List<Button> _animationButtons = new List<Button>();
	List<GameObject> _animationSliders = new List<GameObject>();

	[Space(2)]
	[Header("FacialFeatures")]
	[SerializeField]Text faceColorLabel  = null;
	[SerializeField]Text facePresetLabel  = null;
	[SerializeField]Text ageLabel = null;

	DraggableUI dragUI = null;

	void Awake()
	{
		Instance = this;
		GameObject[] g = GameObject.FindGameObjectsWithTag ("AnimationButtons");
		foreach (GameObject x in g) 
		{
			_animationButtons.Add(x.GetComponent<Button>());
			_animationSliders.Add (x.GetComponentInChildren<Slider> ().gameObject);
		}
	}

	void Start()
	{
		currActiveButtons = new List<ServerUIAnimationButton> ();

		foreach (Button b in _animationButtons) 
		{
			b.enabled = true;
		}

		foreach (GameObject g in _animationSliders) 
		{
			g.SetActive (false);
		}

		DefaultAnimationButtonPreset ();
		LateStart();
	}

	void LateStart()
	{
		facePresetLabel.text = (swapHeads.Instance.current + 1) + "/" + swapHeads.Instance.PresetLength;
		faceColorLabel.text = (swapHeads.Instance.colorIndex + 1) + "/" + swapHeads.Instance.ColorLength;
	}
		
	public void ToggleButton(ServerUIAnimationButton button)
	{
		if (button.isOn)
			DeactivateAnimationButton (button);
		else
			ActivateAnimationButton (button);

		if (PlayerNetworkController.Instance != null) 
		{
			if (button == mixButton) 
			{
				PlayerNetworkController.Instance.ToggleMixFlag ();
				foreach (Button b in _animationButtons) 
				{
					b.enabled = !AnimationController.Instance.MixFlag;
				}

				foreach (GameObject g in _animationSliders) 
				{
					g.SetActive (AnimationController.Instance.MixFlag);
				}

				DefaultAnimationButtonPreset ();

				return;
			}

			PlayerNetworkController.Instance.ButtonPressed ((int)button.AnimationName);
		}
	}


	public void ActivateAnimationButton(ServerUIAnimationButton butt)
	{
        if (AnimationController.Instance != null)
        {
            if (!AnimationController.Instance.MixFlag && currActiveButtons.Count > 0)
            {
                TurnOffAllActiveButtons();
            }
            else if (butt != mixButton)
            {
                DeactivateAnimationButton(defaultOnButton);
            }

            if (!currActiveButtons.Contains(butt))
            {
                currActiveButtons.Add(butt);
                butt.ToggleButtonColor();
            }
        }
	}

	public void DeactivateAnimationButton(ServerUIAnimationButton butt)
	{
		if (currActiveButtons.Contains (butt)) 
		{
			currActiveButtons.Remove (butt);
			butt.ToggleButtonColor ();
		}
	}

	public void TurnOffAllActiveButtons()
	{
		foreach (ServerUIAnimationButton b in currActiveButtons) 
		{
			b.SetButtonIO (false);
		}
				
		mixButton.SetButtonIO (AnimationController.Instance.MixFlag);
		currActiveButtons.Clear ();
	}

	public void DefaultAnimationButtonPreset()
	{

//		animationPanel.BroadcastMessage ("SetButtonIO", false);

		if(currActiveButtons.Count > 0)
			currActiveButtons.Clear ();

        if(AnimationController.Instance != null)
		    mixButton.SetButtonIO (AnimationController.Instance.MixFlag);
		ActivateAnimationButton (defaultOnButton);

	}

	public void FaceColorChange(int value)
	{
		AudioManager.Instance.playClick ();
		if(faceColorLabel == null) {
			ErrorHandler.Instance.errorMsg = "NO LABEL FOR FACE COLOR FOUND!";
			updateFaceColor (value);
			return;
		}

		faceColorLabel.text = updateFaceColor (value);
	}

	public void NextColor ()
	{
		FaceColorChange (1);
	}

	public void PrevColor ()
	{
		FaceColorChange (-1);
	}

	public string updateFaceColor(int value)
	{
		PlayerNetworkController.Instance.ServerColorIndex = value;
		return swapHeads.Instance.faceColor(value);
	}

	public void NextFace () 
	{
		FacePresetChange (1);
	}

	public void PrevFace() 
	{
		FacePresetChange (-1);
	}

	public void FacePresetChange(int value)
	{
		AudioManager.Instance.playClick ();
		if(facePresetLabel == null){
			ErrorHandler.Instance.errorMsg = "NO LABEL FOR FACE PrESET FOUND!";
			FaceSwap (value);
			return;
		}
		PlayerNetworkController.Instance.ServerPresetIndex = value;
		facePresetLabel.text = FaceSwap (value);
	}

	public string FaceSwap(int dir)
 	{
		ErrorHandler.Instance.errorMsg = "Swapin Face" + dir;
		if(dir > 0)
			return swapHeads.Instance.next()+"/"+swapHeads.Instance.faces.Length;
		return swapHeads.Instance.prev()+"/"+swapHeads.Instance.faces.Length;
	}

	public void AdjustAnimationSlider(ServerUIAnimationButton butt)
	{
		AnimationController.Instance.SMesh.SetBlendShapeWeight ((int)butt.AnimationName, butt.AnimationSlider.value * 100);
		butt.SetButtonIO (butt.AnimationSlider.value > 0);
		PlayerNetworkController.Instance.CurrentAnimationChosen = (int)butt.AnimationName;
		PlayerNetworkController.Instance.CurrentAnimationValue = butt.AnimationSlider.value * 100;
	}

	public void AdjustAnimationSlider()
	{
		int index = animationChooser.value;
		float value = animationValue.value;
		bool isOn = animationSwitch.isOn;

		if (AnimationController.Instance == null || PlayerNetworkController.Instance == null)
			return;

		AnimationController.Instance.AdjustAnimationSlider (index-1, isOn ? value * 100: 0);
		PlayerNetworkController.Instance.CurrentAnimationValue = isOn ? value * 100 : 0;
	}

	public void UpdateCurrentSelection(int i)
	{
		if (AnimationController.Instance != null) {
			AnimationValueType val = AnimationController.Instance.GetAnimationValue (i - 1);
			PlayerNetworkController.Instance.CurrentAnimationChosen = i - 1;
			if (val != null) {
				animationSwitch.isOn = val.IsOn;
				animationValue.value = val.Value;
				Debug.Log ("Update Animation --> " + val.Id + " to " + val.Value);
			} else {
			
			}
		}

	}

	public void TurnOnAnimation()
	{
		if (AnimationController.Instance == null)
			return;
		
		int index = animationChooser.value - 1;
		AnimationValueType val = AnimationController.Instance.GetAnimationValue (index - 1);

		if (val == null)
			return;
		
		if (index >= 0) {
			Debug.Log ("Toggle Animation --> " + val.IsOn);
		} else {
			Debug.Log ("Idle :o");
		}
	}

	public void ChangeAge(Slider age)
	{
		if(PlayerNetworkController.Instance != null)
			PlayerNetworkController.Instance.Age = Mathf.CeilToInt(age.value);
			
		ageLabel.text = age.value.ToString ();
	}

	public void TogglePanel(ServerPanel panel)
	{
		panel.Toggle ();
	}

}
