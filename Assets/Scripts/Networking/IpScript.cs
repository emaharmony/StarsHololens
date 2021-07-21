using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IpScript : MonoBehaviour 
{
	public Text myIpAddress;
	//public Text myInput;
	public InputField myInput;
	AudioManager myAudio;

	public static IpScript Instance {get; private set;}

	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		myAudio = AudioManager.Instance;
	}

	public void addChar(string a)
	{
		if(myInput!= null)
		{
			myInput.text += a;
			myAudio.playClick();
		}
		else
			myAudio.playDenied();
	}

	public void removeChar()
	{
		if(myInput.text.Length > 0)
		{
			myInput.text = myInput.text.Substring(0, myInput.text.Length-1);
			myAudio.playClick();
		}
		else
			myAudio.playDenied();
	}

	public void Clear()
	{
		myInput.text = "";
		myAudio.playClick();
	}

	public void UpdateMyIpAddress(string a)
	{
		if(myInput!= null)
			myIpAddress.text = "Your IP Address: "+a;
	}

	public void ConnectToServer()
	{
        ErrorHandler.Instance.errorMsg = "Client Button Clicked " + myInput.text;
		myAudio.playClick();
		if (myInput.text != "") {
			HoloNetworkManager.Instance.ConnectToServer (myInput.text);
			return;
		} else {
			HoloNetworkManager.Instance.ConnectToServer ();
			return;
		}
			
	}

    public void CreateServer()
    {
        ErrorHandler.Instance.errorMsg = "Server Button Clicked " + myInput.text;
        myAudio.playClick();
        if (myInput.text != "")
        {
            HoloNetworkManager.Instance.CreateServer(myInput.text);
            return;
        }
        else
        {
            HoloNetworkManager.Instance.CreateServer();
            return;
        }
    }

}
