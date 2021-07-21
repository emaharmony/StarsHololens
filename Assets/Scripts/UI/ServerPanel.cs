using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerPanel : MonoBehaviour 
{
	public void Toggle()
	{
		gameObject.SetActive (!gameObject.activeInHierarchy);
	}
}
