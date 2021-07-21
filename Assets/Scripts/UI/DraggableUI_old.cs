using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(EventTrigger))]
public class DraggableUI_old : MonoBehaviour {

	public static DraggableUI_old dragObject = null;

	[SerializeField]GameObject toolBarObject;
	[SerializeField]GameObject windowObject;

	float dragOffsetX;
	float dragOffsetY;
	bool _toolBarMode = false;

	public void BeginDrag()
	{
		dragOffsetX = transform.position.x - Input.mousePosition.x;
		dragOffsetY = transform.position.y - Input.mousePosition.y;
		dragObject = this;
	}

	public void OnDrag()
	{
		transform.position = new Vector3 (dragOffsetX + Input.mousePosition.x, dragOffsetY + Input.mousePosition.y);
	}

	public void EndDrag()
	{
		dragObject = null;
	}

	public void SwitchMode(bool x)
	{
		_toolBarMode = x;
		toolBarObject.SetActive (x);
		windowObject.SetActive (!x);
		if (x) 
		{
			toolBarObject.transform.position = Input.mousePosition;
		}
		Debug.Log ("Called --> " + _toolBarMode);
	}

	public bool ToolBarMode
	{
		get{ return _toolBarMode;}
	}
}
