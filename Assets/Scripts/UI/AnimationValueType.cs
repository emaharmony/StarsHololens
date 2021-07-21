using UnityEngine;
using System;

public class AnimationValueType
{
    int _id;
    float _value;
	bool _isOn = false;

    public int Id { get {return _id; }  set { _id = value; } }

    public float Value { get {return _value; } set { _value = value; } }

	public bool IsOn{ get{ return _isOn; }	set{ _isOn = value;} }

    public AnimationValueType()
    {
        _id = 0;
        _value = 0;
		_isOn = false;
    }
    
    public AnimationValueType(int id, float value)
    {
        _id = id;
        _value = value;
		_isOn = false;
    }


}