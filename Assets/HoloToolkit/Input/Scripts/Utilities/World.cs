using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour 
{
	public World Instance;

	private Vector3 manipulationPrevPosition;

	void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
		}
	}
	
	public void StartManipulation(Vector3 position)
	{
		manipulationPrevPosition = position;
	}

	public void UpdateManipulation(Vector3 position)
	{
		Vector3 delta = (position - manipulationPrevPosition)*2f;
		gameObject.transform.position += delta;
		manipulationPrevPosition = position;
	}
	

}

