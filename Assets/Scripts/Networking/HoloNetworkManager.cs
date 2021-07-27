using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro; 
public class HoloNetworkManager : NetworkManager 
{

    const string SERVER_IPADDRESS = "10.0.1.17";
    const int SERVER_PORT = 6666;
    [Space(2)]
    [Header("Hololens Vars")]
	[SerializeField] GameObject networkUI;
	[SerializeField] GameObject connectingPanel;
	[SerializeField] UIController myCanvas;
	[SerializeField] GameObject animationCanvas;
	[SerializeField] Button serverButton;
	[SerializeField] Button clientButton;

	TMP_Text connecting = null;
	GameObject eventSystem;

	public static HoloNetworkManager Instance { get; private set;}

	void Awake()
	{
		Instance = this;
		connecting = connectingPanel.GetComponentInChildren<TMP_Text> ();
        networkPort = SERVER_PORT;
        networkAddress = SERVER_IPADDRESS;
		eventSystem = GameObject.Find ("EventSystem");
    }

	void Start()
	{
        IpScript.Instance.UpdateMyIpAddress(networkAddress);
        ErrorHandler.Instance.errorMsg = (networkAddress + " --> " + networkPort);
        connectingPanel.SetActive (false);
		if (myCanvas != null)
			animationCanvas.SetActive(false);
		else
			animationCanvas.SetActive(true);
		StartCoroutine (CheckIP ());

		if (clientButton != null)
		clientButton.enabled =  true;
		if (serverButton != null)
		serverButton.enabled = true;

		AudioManager.Instance.playDing ();
	}

	IEnumerator CheckIP()
	{
		WWW myExtIPWWW = new WWW("http://checkip.dyndns.org");
		yield return myExtIPWWW;
		networkPort = SERVER_PORT;
		string myExtIP = myExtIPWWW.text;
		myExtIP = myExtIP.Substring (myExtIP.IndexOf (":") + 1);
		myExtIP = myExtIP.Substring (0, myExtIP.IndexOf ("<"));
		myExtIP = myExtIP.TrimStart ();
		myExtIP = myExtIP.TrimEnd ();
		networkAddress = myExtIP;
		IpScript.Instance.UpdateMyIpAddress(networkAddress);
		if (clientButton != null)
			clientButton.enabled = true;
		if (serverButton != null)
			serverButton.enabled = true;
		AudioManager.Instance.playDing ();
        ErrorHandler.Instance.errorMsg = "Checking IP";
	}

	public void CreateServer(string na = "") 
	{
        if (na == "")
            networkAddress = SERVER_IPADDRESS;
        else
            networkAddress = na;

        StartServer();
		networkUI.SetActive (false);
		eventSystem.SetActive (true);
        ErrorHandler.Instance.errorMsg = "Starting Server..";
	}

	public void ConnectToServer(string na = "")
	{
        if (na == "")
            networkAddress = SERVER_IPADDRESS;
        else
            networkAddress = na;

        StartClient();
        ErrorHandler.Instance.errorMsg = "Starting Client: " + networkAddress;

    }

    IEnumerator ConnectingToServer(NetworkClient c)
	{
		connectingPanel.SetActive (true);
		string conn = "Connecting";
        int i = 0;
		while (!c.isConnected) 
		{
			for (int j = 3; i <= j  ; i++) 
			{
				conn += '.';
				yield return null;
			}
				
			if (i >= 3) 
			{
				i = 0;
				conn = "Connecting";
			}

			connecting.text = conn;
			yield return null;
		}

		connecting.text = "Connected!";
        yield return new WaitForSeconds(0.5f);

        ErrorHandler.Instance.errorMsg = "Connected to Server!";
        networkUI.SetActive (false);
		yield return null;

	}

	public UIController MyCanvas {get { return myCanvas;}}
	public override void OnStartServer ()
	{
		base.OnStartServer ();
        if(animationCanvas != null)
            animationCanvas.SetActive (true);
        if(eventSystem != null)
		    eventSystem.SetActive (PlayerNetworkController.Instance != null);
#if NET_4_6 && !NET_STANDARD_2_0 || UNITY_EDITOR || UNITY_STANDALONE
        //MonnitNetworkManager.Instance.StartMonnit ();
#endif
		Debug.Log ("Start Server");

    }

	public override void OnStartClient (NetworkClient client)
	{
        base.OnStartClient(client);
        StartCoroutine(ConnectingToServer(client));

	}

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
		if (PlayerNetworkController.Instance != null && !eventSystem.activeInHierarchy) 
		{
			eventSystem.SetActive (true);
		}

    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
		if (PlayerNetworkController.Instance != null && !eventSystem.activeInHierarchy) 
		{
			eventSystem.SetActive (true);
		}
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        networkUI.SetActive(true);
        connectingPanel.SetActive(false);
    }

    public override void OnServerDisconnect (NetworkConnection conn)
	{
		base.OnServerDisconnect (conn);
		networkUI.SetActive (true);
		connectingPanel.SetActive(false);
	}

    public override void OnServerError(NetworkConnection conn, int errorCode)
    {
        base.OnServerError(conn, errorCode);
        ErrorHandler.Instance.errorMsg = "Client Error: " + errorCode;
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        Debug.Log("Stop Server");
		myCanvas.gameObject.SetActive(false);
        networkUI.SetActive(true);
        connectingPanel.SetActive(false);
    }

    public override void OnClientError (NetworkConnection conn, int errorCode)
	{
        Debug.Log("Oh no :<");
		AudioManager.Instance.playDenied();
		base.OnClientError (conn, errorCode);
		connectingPanel.SetActive (false);
	}
}
