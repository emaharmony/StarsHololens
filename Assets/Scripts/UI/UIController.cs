using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour //ON CANVAS
{
	
	[Header("General Panel Variables")]
	public GameObject[] myPanelController;
	public bool SmallIncriments{ get; set; }
	public bool isCanvasEnabled { get; private set; }
	public float maxCanvasDistance = 3;

	[Header("Position Panel Variables")]
	public Text posXLabel;
	public Text posYLabel;
	public Text posZLabel;
	float valuePosX, valuePosY, valuePosZ;
	const float PRESET_MOVE_VAL = 0.025f;
	Vector3 originalPos;

	[Header("Rotation Panel Variables")]
	public Text rotXLabel;
	public Text rotYLabel;
	public Text rotZLabel;
	float valueRotX, valueRotY, valueRotZ;
	Quaternion originalRot;
	const float PRESET_ROT_VAL = 5;

	[Header("Apperance Panel Variables")]
//	public Text faceColorLabel;
//	public Text facePresetLabel;
	public Text scaleLabel;
	float valueScale;
	Vector3 originalScale;
	const float PRESET_SCALE_VAL = 0.1f;


	AudioManager myAudioManager;
	TapToPlaceParent face;
	private bool isCanvasMoving;
	GameObject activePanel;
	public GameObject importantPanel;

	public static UIController Instance{get; private set;}

	public TapToPlaceParent Face 
	{
		get { return face;  }	
		set { face = value; }
	}

	void Awake() 
	{
		Instance = this;
	}

	void Start()
	{
		isCanvasMoving = false;
		SmallIncriments = false;
		face = TapToPlaceParent.Instance;
		myAudioManager = AudioManager.Instance;
	}

	#region General Canvas
	public void GrabMenu()
	{
		myAudioManager.playClick();
		MoveCanvas ();
	}

	IEnumerator _moveCanvas(GameObject g)
	{
		Vector3 headPosition = Camera.main.transform.position;
		Vector3 gazeDirection = Camera.main.transform.forward;
		Quaternion toQuat;
		float distance = Mathf.Clamp(Vector3.Distance(transform.position, headPosition), 1 , maxCanvasDistance);
		while(isCanvasMoving)
		{
			headPosition = Camera.main.transform.position;
			gazeDirection = Camera.main.transform.forward;

			g.transform.position = headPosition + gazeDirection*(distance);
		
			toQuat = Camera.main.transform.localRotation;
			//toQuat.x = 0f;
			//toQuat.z = 0f;
			g.transform.rotation = toQuat;

			yield return null;
		}
						
	}

	public void ToggleIncriments(Text x)
	{
		myAudioManager.playClick();
		SmallIncriments = !SmallIncriments;
		x.text = SmallIncriments ? "ON" : "OFF";
	} 

	public void ResetValues()
	{
		AudioManager.Instance.playClick ();
		valueRotX = valueRotY = valueRotZ =
			valueScale = valuePosX = valuePosY = valuePosZ = 0;
		SmallIncriments = false;
		face.transform.position = originalPos;
		face.transform.rotation = originalRot;
		face.transform.localScale = originalScale;
//		FaceColorChange (3);
//		FacePresetChange (0);
		UpdateAllUI ();
	}
		
	public void SwitchPanels(GameObject panel)
	{
		myAudioManager.playClick();
		panel.SetActive (true);
		activePanel.SetActive (false);
		activePanel = panel;
		importantPanel.SetActive(activePanel.tag != "HomePanel");
	}

	public void BackButton()
	{
		myAudioManager.playClick();

		foreach(GameObject c in myPanelController)
		{
			c.SetActive(c.tag == "HomePanel");
		}

		importantPanel.SetActive (false);
	}

	public void EnableCanvas()
	{
		if(!isCanvasEnabled)
		{
			activePanel = myPanelController [0];
			activePanel.SetActive(true);
			isCanvasEnabled = true;
			myAudioManager.playClick();
			originalPos = face.transform.position;
			originalRot = face.transform.rotation;
			originalScale = face.transform.localScale;
		}
	}

	public void DisableCanvas()
	{
		GestureManager.Instance.ResetRecognizer ();
		if(isCanvasEnabled)
		{
			if(!AudioManager.Instance.AudioS.isPlaying)
				myAudioManager.playClick();

			foreach(GameObject p in myPanelController)
			{
				p.SetActive(false);
			}
			isCanvasEnabled = false;
		}
	}

	public void OpenConsole(bool a)
	{

		myAudioManager.playOne();
		GameObject.FindGameObjectWithTag ("Console_menu").SetActive(a);
	}

	public void MoveCanvas() 
	{
		if(!isCanvasMoving)
		{
			isCanvasMoving = true;
			StartCoroutine(_moveCanvas(this.gameObject));
		}
		else
			isCanvasMoving = false;
	}


	public void StopCanvas()
	{
		isCanvasMoving = false;
		StopCoroutine("_moveCanvas");
	}

	void UpdateAllUI() 
	{
		posZLabel.text = valuePosZ.ToString("0.##");
		posYLabel.text = valuePosY.ToString("0.##");
		posXLabel.text = valuePosX.ToString("0.##");
		rotXLabel.text = valueRotX.ToString ("0.##");
		rotYLabel.text = valueRotY.ToString ("0.##");
		rotZLabel.text = valueRotZ.ToString ("0.##");
		scaleLabel.text = valueScale.ToString("0.##");
	}
	#endregion

	#region Appearance Panel
//	public void FaceColorChange(int value)
//	{
//		AudioManager.Instance.playClick ();
//		if(faceColorLabel == null) {
//			ErrorHandler.Instance.errorMsg = "NO LABEL FOR FACE COLOR FOUND!";
//			updateFaceColor (value);
//			return;
//		}
//
//		faceColorLabel.text = updateFaceColor (value);
//	}
//
//	public void NextFace () 
//	{
//		FacePresetChange (1);
//	}
//
//	public void PrevFace() 
//	{
//		FacePresetChange (-1);
//	}
//
//	public void FacePresetChange(int value)
//	{
//		AudioManager.Instance.playClick ();
//		if(facePresetLabel == null){
//			ErrorHandler.Instance.errorMsg = "NO LABEL FOR FACE PrESET FOUND!";
//			FaceSwap (value);
//			return;
//		}
//
//		facePresetLabel.text = FaceSwap (value);
//	}
//
//	public void NextColor ()
//	{
//		FaceColorChange (1);
//	}
//
//	public void PrevColor ()
//	{
//		FaceColorChange (-1);
//	}
//
//	public string updateFaceColor(int value)
//	{
//		return swapHeads.Instance.faceColor(value);
//	}
//
//	public string updateFaceColor_set(int value)
//	{
//		return swapHeads.Instance.faceColor_set(value);
//	}
//
//	public string FaceSwap(int dir)
//	{
//		ErrorHandler.Instance.errorMsg = "Swapin Face" + dir;
//		if(dir > 0)
//			return swapHeads.Instance.next()+"/"+swapHeads.Instance.faces.Length;
//		return swapHeads.Instance.prev()+"/"+swapHeads.Instance.faces.Length;
//	}
//
//	public string FaceSwapSet(int i)
//	{
//		ErrorHandler.Instance.errorMsg = "Swaping";
//		return swapHeads.Instance.setFace(i) +"/"+swapHeads.Instance.faces.Length;
//	}

	public void ScaleDrag()
	{
		ScaleController.Instance.Scale ();
	}

	void Scale(float s)
	{
		TapToPlaceParent.Instance.transform.localScale += Vector3.one * ((SmallIncriments ? 0.5f : 1 ) * s);
		valueScale += (SmallIncriments ? 0.5f : 1 ) * s;
		if (scaleLabel != null)
			scaleLabel.text = valueScale.ToString("0.##");
	}

	public void ScaleUp()
	{
		Scale (PRESET_SCALE_VAL);
	}

	public void ScaleDown()
	{
		Scale (-PRESET_SCALE_VAL);
	}

	#endregion

	#region Position Panel
	public void MoveFaceNegX () 
	{
		MoveFaceX (-PRESET_MOVE_VAL);
	}
	public void MoveFacePosX () 
	{
		MoveFaceX (PRESET_MOVE_VAL);
	}
	public void MoveFaceNegY () 
	{
		MoveFaceY (-PRESET_MOVE_VAL);
	}
	public void MoveFacePosY () 
	{
		MoveFaceY (PRESET_MOVE_VAL);
	}
	public void MoveFaceNegZ () 
	{
		MoveFaceZ (-PRESET_MOVE_VAL);
	}
	public void MoveFacePosZ () 
	{
		MoveFaceZ (PRESET_MOVE_VAL);
	}

	void MoveFaceX(float x)
	{
		myAudioManager.playClick();
		ErrorHandler.Instance.errorMsg = x + " to X Position";
		TapToPlaceParent.Instance.moveOnX (x * (SmallIncriments ? 0.5f : 1));
		valuePosX += x * (SmallIncriments ? 0.5f : 1);
		if(posXLabel == null){
			Debug.LogWarning ("NO LABEL FOR POSITION X FOUND!");
			return;
		}

		posXLabel.text = valuePosX.ToString("0.##");
	}

	void MoveFaceY(float y)
	{
		myAudioManager.playClick();
		ErrorHandler.Instance.errorMsg = y + " to YPosition";
		TapToPlaceParent.Instance.moveOnY (y * (SmallIncriments ? 0.5f : 1));
		valuePosY += (y * (SmallIncriments ? 0.5f : 1));
		if(posYLabel == null){
			Debug.LogWarning ("NO LABEL FOR POSITION Y FOUND!");
			return;
		}
		posYLabel.text = valuePosY.ToString("0.##");
	}

	void MoveFaceZ(float z)
	{	
		myAudioManager.playClick();
		ErrorHandler.Instance.errorMsg = z + " to ZPosition";
		TapToPlaceParent.Instance.moveOnZ (z * (SmallIncriments ? 0.5f : 1));
		valuePosZ += (z * (SmallIncriments ? 0.5f : 1));
		if(posZLabel == null){
			Debug.LogWarning ("NO LABEL FOR POSITION Z FOUND!");
			return;
		}
		posZLabel.text = valuePosZ.ToString("0.##");
	}
	#endregion

	#region Rotation Panel

	public void  RotateXUp()
	{
		RotateX (PRESET_ROT_VAL);
	}

	public void RotateXDown()
	{
		RotateX (-PRESET_ROT_VAL);
	}

	public void  RotateYUp()
	{
		RotateY (PRESET_ROT_VAL);
	}

	public void RotateYDown()
	{
		RotateY (-PRESET_ROT_VAL);
	}

	public void  RotateZUp()
	{
		RotateZ (PRESET_ROT_VAL);
	}

	public void RotateZDown()
	{
		RotateZ (-PRESET_ROT_VAL);
	}

	public void RotateX()
	{
		RotationController.Instance.RotateX ();
	}

	void RotateX(float x)
	{
		AudioManager.Instance.playClick ();
		TapToPlaceParent.Instance.transform.Rotate((SmallIncriments ? 0.5f : 1 ) * x,0f,0f);
		valueRotX += x * (SmallIncriments ? 0.5f : 1 );
		if (rotXLabel != null)
			rotXLabel.text = valueRotX.ToString ("0.##");
	}

	public void RotateY()
	{
		RotationController.Instance.RotateY ();
	}

	void RotateY(float y)
	{
		AudioManager.Instance.playClick ();
		TapToPlaceParent.Instance.transform.Rotate(0f,(SmallIncriments ? 0.5f : 1 ) *y,0f);
		valueRotY += (SmallIncriments ? 0.5f : 1 ) * y;
		if (rotYLabel != null)
			rotYLabel.text = valueRotY.ToString ("0.##");
	}

	public void RotateZ()
	{
		RotationController.Instance.RotateZ ();
	}

	void RotateZ(float z)
	{
		AudioManager.Instance.playClick ();
		TapToPlaceParent.Instance.transform.Rotate(0f,0f,(SmallIncriments ? 0.5f : 1 ) *z);
		valueRotZ += (SmallIncriments ? 0.5f : 1 ) *z;
		if (rotZLabel != null)
			rotZLabel.text = valueRotZ.ToString ("0.##");
	}
	#endregion

}
