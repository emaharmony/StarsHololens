using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleOnStart : MonoBehaviour 
{
	public GameObject[] EnableOnStart;
	public GameObject[] DisableOnStart;
	//public bool disableCanvasOnStart;
	//public bool resetCameraOnStart;
	//public Canvas c;
	//public GameObject cube;

	void Start()
	{
		/*#if !UNITY_EDITOR
			Camera.main.transform.eulerAngles = Vector3.zero;
			AudioManager.Instance.playThree();
		#else
			AudioManager.Instance.playGlobalWarming();
		#endif*/
		/*if(disableCanvasOnStart)
			c.enabled = false;*/

		#if UNITY_EDITOR
		ErrorHandler.Instance.errorMsg = "moving camera foward";
		Camera.main.transform.localPosition = (Vector3.forward * 1.36f) + (Vector3.up * .04f);
		#endif

		StartCoroutine(_enableOnStart());
	}


	IEnumerator _enableOnStart()
	{
		foreach(GameObject g in EnableOnStart)
			g.SetActive(true);
		yield return null;

		StartCoroutine(_DisableOnStart());
	}

	IEnumerator _DisableOnStart()
	{
		foreach(GameObject g in DisableOnStart)
			g.SetActive(false);
		
		yield return null;
	}

}
