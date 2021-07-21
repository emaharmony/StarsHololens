using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.Networking;

public class MainMenu : MonoBehaviour//NetworkBehaviour 
{
	public NetworkManager manager;



	void Start () 
	{
		if(UnityEngine.XR.XRDevice.isPresent)
		{
			manager.StartClient();
			UnityEngine.SceneManagement.SceneManager.LoadScene("FacePlate");
		}
		else
		{
			manager.StartHost();
			UnityEngine.SceneManagement.SceneManager.LoadScene("AdminScene");
		}
	}
	

}
