using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BorderController_old : MonoBehaviour
{
	GraphicRaycaster ray;
	EventSystem event_systems;
	PointerEventData _pointer;

	void Start()
	{
		ray = GetComponentInParent<GraphicRaycaster> ();
	}

	void Update()
	{
		if (DraggableUI.dragObject != null) 
		{
			_pointer = new PointerEventData (event_systems);
			_pointer.position = Input.mousePosition;

			List<RaycastResult> results = new List<RaycastResult> ();
			ray.Raycast (_pointer, results);

			foreach (RaycastResult r in results) 
			{
				if (!DraggableUI.dragObject.ToolBarMode && r.gameObject.CompareTag ("Border"))
					DraggableUI.dragObject.SwitchMode (true);
				else if (DraggableUI.dragObject.ToolBarMode && r.gameObject.CompareTag ("WindowArea"))
					DraggableUI.dragObject.SwitchMode (false);
			}
		}
	}
}
