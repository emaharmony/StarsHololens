using UnityEngine;
using HoloToolkit.Unity.SpatialMapping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.WSA;
public class TapToPlaceParent : MonoBehaviour
{
	bool placing = false;
	Transform _cam;
	[HideInInspector]
	public bool isLocked = false;
	AudioManager myAudioManager;
	UIController myCanvas;
	public SpatialMappingManager Mapping;

	public enum tracking {faceUser = 0,faceNormal = 1,custom = 2};
	public tracking FaceTracking = 0;


	public static TapToPlaceParent Instance 
	{ 
		get; 
		private set; 
	}

	void Awake()
	{
		Instance = this;
		Mapping = GameObject.FindGameObjectWithTag ("SpatialMapping").GetComponent<SpatialMappingManager> ();
		_cam = Camera.main.transform;
	}

	void Start()
	{
		myCanvas = UIController.Instance;
		myAudioManager = AudioManager.Instance;
        Mapping = SpatialMappingManager.Instance;
	}

	public void OnSelect()
	{
		ErrorHandler.Instance.errorMsg = "Here I am";
		if(isLocked)
		{
			myAudioManager.playDenied();
			ErrorHandler.Instance.errorMsg = "face is locked";
			return;
		}
		placing = !placing;

		myAudioManager.playClick();
		
		if (placing)
		{
			GestureManager.Instance.ResetRecognizer ();

			//changeColors();
			Mapping.DrawVisualMeshes = true;
			myCanvas.DisableCanvas();
			StartCoroutine(_placing());
		}
		else
		{
			//revertColors();
			Mapping.DrawVisualMeshes = false;
		}
	}

	void UnSelect()
	{
		placing = false;
		myAudioManager.playClick();
		//revertColors();
		Mapping.DrawVisualMeshes = false;
	}



	/*void EnableCanvas()
	{
		myAudioManager.playClick();

		//myCanvas.enabled = true;
		//myCanvas.gameObject.transform.position = this.transform.position + Vector3.up*.32f;
		myCanvas.EnableCanvas();
		myCanvas.transform.position = this.transform.position + Vector3.up*.32f;
	}

	public void DisableCanvas()
	{
		//if(myCanvas.enabled)
		if(myCanvas.isCanvasEnabled)
		{
			//myCanvas.enabled = false;
			myCanvas.DisableCanvas();
			if(!myAudioManager.myClick.isPlaying)
				myAudioManager.playClick();
		}
	}*/
		

	public void scale(float i)
	{
		this.gameObject.transform.localScale += Vector3.one * i;
	}

	public void moveOnX(float i)
	{
		this.transform.position += this.transform.right*i;
	}
	public void moveOnY(float i)
	{
		this.transform.position += this.transform.up*i;
	}
	public void moveOnZ(float i)
	{
		this.transform.position += this.transform.forward*i;
	}
	public void roate_face(float i,int orintation)
	{
		switch(orintation)
		{
			case 0://x
				this.transform.Rotate(i,0f,0f);
				break;
			case 1://y
				this.transform.Rotate(0f,i,0f);
				break;
			case 2://z
				this.transform.Rotate(0f,0f,i);
				break;
			default:
				break;
		}
	}
		

	IEnumerator _placing()
	{
		this.transform.localPosition = Vector3.zero;

		while(placing)
		{
			RaycastHit hitInfo;
			Quaternion toQuat = new Quaternion();

			if (Physics.Raycast(_cam.position, _cam.forward, out hitInfo,
			100, 1 << LayerMask.NameToLayer ("SpatialMapping")))
			{
				this.transform.position = hitInfo.point;
				transform.LookAt(Camera.main.transform);
/*				switch(FaceTracking)
			
//				{
//					case tracking.faceUser:
/*					//toQuat = Camera.main.transform.localRotation;
//						break;
//					case tracking.faceNormal:
//						toQuat.SetLookRotation(-hitInfo.normal);
//						break;
//					case tracking.custom:
//						toQuat = Quaternion.Euler (new Vector3(Camera.main.transform.rotation.eulerAngles.x , 
//							Camera.main.transform.rotation.eulerAngles.y * (-hitInfo.normal.z < 0 ? -1 : 1)));	
//						break;
//				}
						
				//toQuat.x = 0f;
				//toQuat.z = 0f;
				//this.transform.rotation = toQuat;*/
			}

			yield return new WaitForEndOfFrame();
		}
	}

	void NAIIIIIILLLL()
	{
		myAudioManager.playGlobalWarming();
	}

	void OnReset()
	{
		myCanvas.ResetValues();
	}

	void Tracking()
	{
		myAudioManager.playClick();
		FaceTracking += 1;
		if((int)FaceTracking > 2)
			FaceTracking = 0;
	}
		
	void Update()
	{
		var normal = -Camera.main.transform.forward;     // Normally the normal is best set to be the opposite of the main camera's forward vector
		// If the content is actually all on a plane (like text), set the normal to the normal of the plane
		// and ensure the user does not pass through the plane
		var position = this.transform.position;
		HolographicSettings.SetFocusPointForFrame(position, normal);
	}

	void Reset()
	{
		//myCanvas.SendMessage("ResetValues");
		myCanvas.ResetValues();
	}

	void EnableMesh()
	{
		Mapping.DrawVisualMeshes = !Mapping.DrawVisualMeshes;
		myAudioManager.playClick();
	}

	void FreezeSpatialMapping()
	{
		Mapping.MappingEnabled = !Mapping.MappingEnabled;

		if(Mapping.MappingEnabled)
			myAudioManager.playOne();
		else
			myAudioManager.playTwo();
	}

	void UnLock()
	{
		placing = false;
	}
}

