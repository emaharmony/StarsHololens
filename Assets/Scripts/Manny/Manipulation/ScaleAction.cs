using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAction : MonoBehaviour {

	[SerializeField] float scaleSensitivity = 20;

	float scaleFactor;

	// Update is called once per frame
	void Update ()
	{
		PerformScaling();
	}

	void PerformScaling()
	{
        if (ScaleController.Instance != null)
        {
            if (ScaleController.Instance.isScaling)
            {
                scaleFactor = ScaleController.Instance.NavPosition.x * scaleSensitivity;
                transform.localScale += new Vector3(-scaleFactor, -scaleFactor, -scaleFactor);
            }
        }
	}

}
