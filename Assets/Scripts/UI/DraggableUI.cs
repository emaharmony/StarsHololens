using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {

	public static DraggableUI dragObject = null;


	RectTransform panelTransform;
	RectTransform canvasTransform;
	float dragOffsetX;
	float dragOffsetY;
	bool _toolBarMode = false;


	void Awake()
	{
		Canvas can = GetComponentInParent<Canvas> ();
		if (can != null) {
			Debug.Log ("No Canvas");
		}
	}

	public void BeginDrag()
	{
		dragOffsetX = transform.parent.position.x - Input.mousePosition.x;
		dragOffsetY = transform.parent.position.y - Input.mousePosition.y;
		dragObject = this;
	}

	#region IPointerDownHandler implementation

	public void OnPointerDown (PointerEventData eventData)
	{
		BeginDrag ();
	}

	#endregion

	#region IDragHandler implementation

	public void OnDrag (PointerEventData eventData)
	{
		OnDrag ();
	}

	#endregion

	#region IPointerUpHandler implementation

	public void OnPointerUp (PointerEventData eventData)
	{
		EndDrag ();
	}
	#endregion

	public void OnDrag()
	{
//		Vector3 pos = panelRectTransform.localPosition;
//
//		Vector3 minPosition = parentRectTransform.rect.min - panelRectTransform.rect.min;
//		Vector3 maxPosition = parentRectTransform.rect.max - panelRectTransform.rect.max;
//
//		pos.x = Mathf.Clamp(panelRectTransform.localPosition.x, minPosition.x, maxPosition.x);
//		pos.y = Mathf.Clamp(panelRectTransform.localPosition.y, minPosition.y, maxPosition.y);
//
//		panelRectTransform.localPosition = pos;
		transform.parent.position = new Vector3 (dragOffsetX + Input.mousePosition.x, dragOffsetY + Input.mousePosition.y);
	}

	public void EndDrag()
	{
		AdjustDraggedPanelPosition ();
		dragObject = null;
	}

	public void SwitchMode(bool x)
	{
		_toolBarMode = x;
	
	}

	public bool ToolBarMode
	{
		get{ return _toolBarMode;}
	}

	public void AdjustDraggedPanelPosition()
	{
		RectTransform rect = transform.parent as RectTransform;
		float x = Mathf.Clamp (rect.localPosition.x, -630, 730);
		float y = Mathf.Clamp (rect.localPosition.y, -225, 170);
		transform.parent.localPosition = new Vector3 (x, y, 0);
	}
}
