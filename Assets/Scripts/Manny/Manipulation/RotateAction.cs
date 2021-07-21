using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using UnityEngine.Networking;

public class RotateAction : NetworkBehaviour
{
    public static RotateAction Instance { get; private set; }
    [SerializeField] float rotationSensitivity = 20;

    float rotationFactor;

    [SyncVar(hook = "MonnitRotation")]
    public double monnitRotation;

   //[SerializeField] bool testFlag = false; 

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Instance = this; 
    }

    // Update is called once per frame
    void Update ()
    {
        if (RotationController.Instance != null)
        {
            if (RotationController.Instance.isRotating)
            {
                PerformRotation();
            }
        }

        if (Instance == null)
            Instance = this;
        //if (testFlag)
        //{
        //    transform.RotateAround(Pivot.Instance.PivotTransform.position, transform.right, (float)monnitRotation * Time.deltaTime);
        //}
    }

    void PerformRotation()
    {
        switch (RotationController.Instance.currDirection)
        {
			case RotationController.RotationDirection.None:
				rotationFactor = 0;
				break;
            case RotationController.RotationDirection.xAxis :
                PerformXRotation();
                break;
            case RotationController.RotationDirection.yAxis:
                PerformYRotation();
                break;
            case RotationController.RotationDirection.zAxis:
                PerformZRotation();
                break;
        }
    }

    void PerformXRotation()
    {
        rotationFactor = RotationController.Instance.NavPosition.x * rotationSensitivity;
		transform.Rotate(-rotationFactor, 0, 0);
    }

    void PerformYRotation()
    {
        rotationFactor = RotationController.Instance.NavPosition.x * rotationSensitivity;
		transform.Rotate(0, -rotationFactor, 0);
    }

    void PerformZRotation()
    {
        rotationFactor = RotationController.Instance.NavPosition.x * rotationSensitivity;
        transform.Rotate(0, 0, -rotationFactor);
    }

    public void MonnitRotation(double d)
    {
        AudioManager.Instance.playClick();
        StartCoroutine(MonnitRotate(d));
    }

    IEnumerator MonnitRotate(double d)
    {
        float timer = 0;
        while (timer <= 1)
        {
            transform.Rotate(Vector3.right * ((float) d * Time.deltaTime));
            timer += Time.deltaTime;
            yield return null;
        }
    }	
}
