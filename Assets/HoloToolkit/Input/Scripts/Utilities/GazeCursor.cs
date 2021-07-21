using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeCursor : MonoBehaviour 
{
	public static GazeCursor Instance;
	public GameObject FocusedObject;


	void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
		}
	}


	void Update () 
	{
		Vector3 headPosition = Camera.main.transform.position;
		Vector3 gazeDirection = Camera.main.transform.forward;

		RaycastHit hitInfo;
		if(Physics.Raycast(headPosition,gazeDirection,out hitInfo))
		{	//if the cursor collides with the mesh, store the access in the focusedObject vatible for other scripts to access
							
			FocusedObject = hitInfo.collider.gameObject;
			this.transform.position = hitInfo.point;
		}
		else
		{
			FocusedObject = null;
			this.transform.position = headPosition + gazeDirection *2f;
		}

	}
}
