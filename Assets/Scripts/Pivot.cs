using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TapToPlace))]
public class Pivot : Singleton<Pivot>
{
    public static Pivot Instance { get; private set; }

    MeshRenderer visible;
    TapToPlace tp;

    private void Awake()
    {
        Instance = this;
        visible = GetComponent<MeshRenderer>();
        tp = GetComponent<TapToPlace>();
    }

    private void Update()
    {
       // visible.enabled = tp.IsBeingPlaced;

    }

    public TapToPlace Tap {

        get { return tp; }
    }
}
