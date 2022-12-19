using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceCommandManager : MonoBehaviour
{
    TapToPlace selected = null;

    public void GrabFace() 
    {
        selected = GameObject.FindGameObjectWithTag("Player").GetComponent<TapToPlace>();
        selected.VoiceSelect();
    }

    public void Reset()
    {
        GameObject.FindGameObjectWithTag("Player").SendMessage("Reset");
    }
     
    public void GrabMenu() 
    {
        HoloNetworkManager.Instance.MyCanvas.GrabMenu();
    }

    public void Place() 
    {
        if (selected != null)
            selected.VoiceSelect();
        HoloNetworkManager.Instance.MyCanvas.StopCanvas();
        selected = null;
    }

    public void GrabPivot() 
    {
        selected = Pivot.Instance.Tap;
        selected.VoiceSelect();
    }
}
