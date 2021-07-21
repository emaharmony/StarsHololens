using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerUIAnimationButton : MonoBehaviour 
{
	Color onColor;
	Color offColor;
	bool io = false;
	Slider slider;
	[SerializeField]AnimationController.ExpressionType type;

	void Awake() 
	{
		onColor = Color.green;
		offColor = Color.white;
		offColor.a = .7f;
		slider = GetComponentInChildren<Slider> ();

	}

	public bool isOn
	{
		get{ return io; }
	}

	public void ToggleButtonColor()
	{
		io = !io;
		GetComponent<Image> ().color = (io ? onColor : offColor);
	}
	
	 
	public void SetButtonIO( bool b )
	{
		io = b;
		GetComponent<Image> ().color = (io ? onColor : offColor);
	}

	public Slider AnimationSlider
	{
		get { return slider;}
	}

	public AnimationController.ExpressionType AnimationName
	{
		get{ return type;}
	}
		
}
