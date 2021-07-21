using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloToolkit;

public class ScaleController : MonoBehaviour 
{

	public static ScaleController Instance;

	public Vector3 NavPosition{ get; private set;}

	bool _scaling = true;

	UnityEngine.XR.WSA.Input.GestureRecognizer scaleRecognizer;

	void Awake() 
	{
		Instance = this;
	}

	// Use this for initialization
	void Start () 
	{
		_scaling = false;		
	}

	public void Scale()
	{
		InitScaleRecognizer ();
		_scaling = true;
	}

	public bool isScaling { get{ return _scaling;} set { _scaling = value; }}

	public void ResetScale()
	{
		DestroyScaleRecognizer();
		_scaling = false;
	}

	void OnDestroy()
	{
		ResetScale();
	}

	void InitScaleRecognizer()
	{
		scaleRecognizer = new UnityEngine.XR.WSA.Input.GestureRecognizer();
		scaleRecognizer.SetRecognizableGestures(UnityEngine.XR.WSA.Input.GestureSettings.Tap | UnityEngine.XR.WSA.Input.GestureSettings.NavigationX);

		// Register the Events with their respective functions.
		scaleRecognizer.NavigationStartedEvent += ScaleStartedEvent;
		scaleRecognizer.NavigationUpdatedEvent += ScaleUpdatedEvent;
		scaleRecognizer.NavigationCompletedEvent += ScaleCompletedEvent;
		scaleRecognizer.NavigationCanceledEvent += ScaleCanceledEvent;
		GestureManager.Instance.Transition(scaleRecognizer);
	}

	void DestroyScaleRecognizer()
	{		 
		if (scaleRecognizer != null) 
		{
			scaleRecognizer.SetRecognizableGestures (UnityEngine.XR.WSA.Input.GestureSettings.None);

			// Deregister the Events with their respective functions.
			scaleRecognizer.NavigationStartedEvent -= ScaleStartedEvent;
			scaleRecognizer.NavigationUpdatedEvent -= ScaleUpdatedEvent;
			scaleRecognizer.NavigationCompletedEvent -= ScaleCompletedEvent;
			scaleRecognizer.NavigationCanceledEvent -= ScaleCanceledEvent;
		}
	}

	private void ScaleStartedEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 relativePosition, Ray ray)
	{
		//Set rotating to be true.
		_scaling = true;

		//Set NavPosition to be relativePosition.
		NavPosition = relativePosition;
	}

	private void ScaleUpdatedEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 relativePosition, Ray ray)
	{
		//Set rotating to be true.
		_scaling = true;

		//Set NavPosition to be relativePosition.
		NavPosition = relativePosition;
	}

	private void ScaleCompletedEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 relativePosition, Ray ray)
	{
		//Set rotating to be false.
		_scaling = false;
	}

	private void ScaleCanceledEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 relativePosition, Ray ray)
	{
		//Set rotating to be false.
		_scaling = false;
	}
}
