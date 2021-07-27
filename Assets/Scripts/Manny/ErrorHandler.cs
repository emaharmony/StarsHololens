using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorHandler : MonoBehaviour 
{

	public GameObject sprite;
	public Text errorText;
	string myString="";
	public string errorMsg
	{
		get{
			return myString;
		}
		set
		{
			myString += "-"+value+"\n";
			outputToConsole();
		}
	}


	public static ErrorHandler Instance 
	{ 
		get; 
		private set; 
	}

	void Awake()
	{
		Instance = this;

	}

	public void displayError()
	{
		StopCoroutine(_flashError());
		StartCoroutine(_flashError());
	}

	IEnumerator _flashError()
	{
		int count = 10;
		while(count > 0)
		{
			if (sprite == null) break;

			sprite.SetActive(!sprite.activeSelf);
			yield return new WaitForSeconds(.35f);
			--count;
		}
	}
		
	public void outputToConsole()
	{
		if (errorText == null) return;
		errorText.text = errorMsg;
	}

	public void ClearErrorMsg()
	{
		myString="";
		outputToConsole();
	}

}
