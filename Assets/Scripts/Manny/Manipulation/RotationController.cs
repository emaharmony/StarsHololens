using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RotationController : MonoBehaviour
{
    public enum RotationDirection
    {
        None,
        xAxis,
        yAxis,
        zAxis
    }

	[SerializeField]GameObject cursorRot; 
	public static RotationController Instance { get; private set;} 

    public RotationDirection currDirection = RotationDirection.None;

    public Vector3 NavPosition { get; private set; }

    bool _rotating; 
    UnityEngine.XR.WSA.Input.GestureRecognizer rotateRecognizer;

    public bool isRotating { get { return _rotating; } set {_rotating = value; } }

    
    // Use this for initialization
    void Awake ()
    {
        Instance = this;
        _rotating = false;
	}

	void Update()
	{		
		cursorRot.SetActive (_rotating);
	}

    public void RotateX()
    {
		if (_rotating) 
			SwitchAxis();

		InitRotateRecognizer();
		currDirection = RotationDirection.xAxis;
		_rotating = true;
    }

    public void RotateY()
    {
		if (_rotating) 
			SwitchAxis();
		
		InitRotateRecognizer();
        currDirection = RotationDirection.yAxis;
		_rotating = true;
    }

    public void RotateZ()
    {
		if (_rotating) 
			SwitchAxis();

		InitRotateRecognizer();
		currDirection = RotationDirection.zAxis;
        _rotating = true;
    }

    public void ResetRotation()
    {
        DestroyRotateRecognizer();
        currDirection = RotationDirection.None;
		_rotating = false;

    }

    void OnDestroy()
    {
        ResetRotation();
    }

    private void Rotate_RotateStartedEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        //Set rotating to be true.
        _rotating = true;

        //Set NavPosition to be relativePosition.
        NavPosition = relativePosition;
    }

    private void Rotate_RotateUpdatedEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        //Set rotating to be true.
        _rotating = true;

        //Set NavPosition to be relativePosition.
        NavPosition = relativePosition;
    }

    private void Rotate_RotateCompletedEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
		Debug.Log ("Complete");
        //Set rotating to be false.
        _rotating = false;

		NavPosition = Vector3.zero;
    }

    private void Rotate_RotateCanceledEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
		Debug.Log ("Canceled");
        //Set rotating to be false.
        _rotating = false;

		NavPosition = Vector3.zero;
    }

    void InitRotateRecognizer()
    {
        rotateRecognizer = new UnityEngine.XR.WSA.Input.GestureRecognizer();
        rotateRecognizer.SetRecognizableGestures(UnityEngine.XR.WSA.Input.GestureSettings.Tap | UnityEngine.XR.WSA.Input.GestureSettings.NavigationX);

        // Register the Events with their respective functions.
        rotateRecognizer.NavigationStartedEvent += Rotate_RotateStartedEvent;
        rotateRecognizer.NavigationUpdatedEvent += Rotate_RotateUpdatedEvent;
        rotateRecognizer.NavigationCompletedEvent += Rotate_RotateCompletedEvent;
        rotateRecognizer.NavigationCanceledEvent += Rotate_RotateCanceledEvent;
        GestureManager.Instance.Transition(rotateRecognizer);
    }

    void DestroyRotateRecognizer()
    {
		if (rotateRecognizer != null) 
		{
			rotateRecognizer.SetRecognizableGestures (UnityEngine.XR.WSA.Input.GestureSettings.None);

			// Deregister the Events with their respective functions.
			rotateRecognizer.NavigationStartedEvent -= Rotate_RotateStartedEvent;
			rotateRecognizer.NavigationUpdatedEvent -= Rotate_RotateUpdatedEvent;
			rotateRecognizer.NavigationCompletedEvent -= Rotate_RotateCompletedEvent;
			rotateRecognizer.NavigationCanceledEvent -= Rotate_RotateCanceledEvent;
		}
    }

	void SwitchAxis()
	{
		DestroyRotateRecognizer();
		_rotating = false;
		NavPosition = Vector3.zero;
	}

}
