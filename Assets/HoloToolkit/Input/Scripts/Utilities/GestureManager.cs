using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GestureManager : MonoBehaviour 
{
	public static GestureManager Instance;
	public bool isManipulating;

    public UnityEngine.XR.WSA.Input.GestureRecognizer ActiveRecognizer { get; set; }

    public GameObject Face;

    //used when manipulating objects
    private UnityEngine.XR.WSA.Input.GestureRecognizer ManipulationRecognizer;

	void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
		}

		isManipulating = false;
		ManipulationRecognizer = new UnityEngine.XR.WSA.Input.GestureRecognizer();
		ManipulationRecognizer.SetRecognizableGestures(UnityEngine.XR.WSA.Input.GestureSettings.ManipulationTranslate);

		ManipulationRecognizer.ManipulationStartedEvent += manipulationStarted;
		ManipulationRecognizer.ManipulationUpdatedEvent += manipulationUpdated;
		ManipulationRecognizer.ManipulationCanceledEvent += manipulationEnded;
		ManipulationRecognizer.ManipulationCompletedEvent += manipulationEnded;
	
		ManipulationRecognizer.StartCapturingGestures();
	}

    public void Transition(UnityEngine.XR.WSA.Input.GestureRecognizer newRecognizer)
    {
        if (newRecognizer == null)
        {
            return;
        }

        if (ActiveRecognizer != null)
        {
            if (ActiveRecognizer == newRecognizer)
            {
                return;
            }

            ActiveRecognizer.CancelGestures();
            ActiveRecognizer.StopCapturingGestures();
        }

        newRecognizer.StartCapturingGestures();
        ActiveRecognizer = newRecognizer;
    }

    public void ResetRecognizer()
    {
        Transition(ManipulationRecognizer);
        RotationController.Instance.ResetRotation();
        ScaleController.Instance.ResetScale();
    }

    private void manipulationStarted(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 position, Ray headRay)
	{
		if(GazeCursor.Instance.FocusedObject != null)
		{
			isManipulating = true;
			GazeCursor.Instance.FocusedObject.SendMessage("StartManipulation",position);
		}
	}

	private void manipulationUpdated(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 position, Ray headRay)
	{
		if(GazeCursor.Instance.FocusedObject != null)
		{
			isManipulating = true;
			GazeCursor.Instance.FocusedObject.SendMessage("UpdateManipulation",position);
		}
	}

	private void manipulationEnded(UnityEngine.XR.WSA.Input.InteractionSourceKind source, Vector3 position, Ray headRay)
	{
		isManipulating = false;
	}

	public void StopManipulation()
	{
		ManipulationRecognizer.StopCapturingGestures();
	}

	private void OnDestory()
	{
		ManipulationRecognizer.ManipulationStartedEvent -= manipulationStarted;
		ManipulationRecognizer.ManipulationUpdatedEvent -= manipulationUpdated;
		ManipulationRecognizer.ManipulationCanceledEvent -= manipulationEnded;
		ManipulationRecognizer.ManipulationCompletedEvent -= manipulationEnded;
	}

}
