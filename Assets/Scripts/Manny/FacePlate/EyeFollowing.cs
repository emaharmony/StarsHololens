using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeFollowing : MonoBehaviour {

    Transform cam;

    float MAX_X_ROT = 10;
    float MAX_Y_ROT = 45;
    void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    // Update is called once per frame
    void Update () {
        FollowCamera();
	}

    void FollowCamera()
    {
        transform.LookAt(cam.transform.position);
        float y = transform.localEulerAngles.y > 45 ? transform.localEulerAngles.y - 360 : transform.localEulerAngles.y;
        y = Mathf.Clamp(y, -MAX_Y_ROT, MAX_Y_ROT);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, y, transform.localEulerAngles.z);
    }
}
