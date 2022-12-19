 using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.Networking;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;


public class SpeechManager : MonoBehaviour
{
	KeywordRecognizer keywordRecognizer = null;
	Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();


	public enum words {none, place, grabFace, grabmenu, locked, unlock, menu,close,  tracking, console, pivot};
	public words myWords;

	SpatialMappingManager _sMap;

    TapToPlace selected = null; 

	void Awake() 
	{
		_sMap = GameObject.FindGameObjectWithTag ("SpatialMapping").GetComponent<SpatialMappingManager> ();
	}

	// Use this for initialization
	void Start()
	{
		#region Keypad Speech
		keywords.Add("Zero", () =>
			{
				IpScript.Instance.addChar("0");
			});
		keywords.Add("One", () =>
			{
				IpScript.Instance.addChar("1");
			});
		keywords.Add("Two", () =>
			{
				IpScript.Instance.addChar("2");
			});
		keywords.Add("Three", () =>
			{
				IpScript.Instance.addChar("3");
			});
		keywords.Add("Four", () =>
			{
				IpScript.Instance.addChar("4");
			});
		keywords.Add("Five", () =>
			{
				IpScript.Instance.addChar("5");
			});
		keywords.Add("Six", () =>
			{
				IpScript.Instance.addChar("6");
			});
		keywords.Add("Seven", () =>
			{
				IpScript.Instance.addChar("7");
			});
		keywords.Add("Eight", () =>
			{
				IpScript.Instance.addChar("8");
			});
		keywords.Add("Nine", () =>
			{
				IpScript.Instance.addChar("9");
			});
		keywords.Add("Dot", () =>
			{
				IpScript.Instance.addChar(".");
			});
		keywords.Add("Back Space", () =>
			{
				IpScript.Instance.removeChar();
			});
		keywords.Add("Erase", () =>
			{
				IpScript.Instance.removeChar();
			});
		keywords.Add("Clear", () =>
			{
				IpScript.Instance.Clear();
			});
		keywords.Add("Connect", () =>
			{
				IpScript.Instance.ConnectToServer();
			});
		#endregion
		#region Model edit Speech
		keywords.Add("Reset All", () =>
			{
				//this.BroadcastMessage("OnReset");
				GameObject.FindGameObjectWithTag("Player").SendMessage("OnReset");
			});

		keywords.Add("Grab Face", () =>
			{
                selected = GameObject.FindGameObjectWithTag("Player").GetComponent<TapToPlace>();
                selected.VoiceSelect();
			});
		keywords.Add("Place", () =>
			{
                if(selected != null)
				    selected.VoiceSelect();
				HoloNetworkManager.Instance.MyCanvas.StopCanvas();
                selected = null;
			});
		keywords.Add("Let Go", () =>
			{
                if (selected != null)
                    selected.VoiceSelect();

                selected = null;
            });
		keywords.Add("Lock Face", () =>
			{
				GameObject.FindGameObjectWithTag("Player").SendMessage("OnLock");
			});
		keywords.Add("Unlock Face", () =>
			{
				GameObject.FindGameObjectWithTag("Player").SendMessage("UnLock");
			});
		keywords.Add("Modify Face", () =>
			{
				HoloNetworkManager.Instance.MyCanvas.EnableCanvas();
			});
		keywords.Add("Change Color", () =>
			{
				HoloNetworkManager.Instance.MyCanvas.EnableCanvas();
			});
		keywords.Add("Open Menu", () =>///
			{
				HoloNetworkManager.Instance.MyCanvas.EnableCanvas();
			});
		keywords.Add("Stop", () =>
			{
				HoloNetworkManager.Instance.MyCanvas.StopCanvas();
			});
		keywords.Add("Close", () =>////
			{
				HoloNetworkManager.Instance.MyCanvas.DisableCanvas();
			});
		keywords.Add("Global Warming", () =>
			{
				AudioManager.Instance.playGlobalWarming();
			});
		keywords.Add("Switch Tracking", () =>
			{
				GameObject.FindGameObjectWithTag("Player").SendMessage("Tracking");
			});
		keywords.Add("Back", () =>
			{
				HoloNetworkManager.Instance.MyCanvas.BackButton();
			});
		keywords.Add("Grab Menu", () =>
			{
				HoloNetworkManager.Instance.MyCanvas.GrabMenu();
			});
		keywords.Add("Reset", () =>
			{
				GameObject.FindGameObjectWithTag("Player").SendMessage("Reset");
			});
		keywords.Add("Console", () =>
			{
				HoloNetworkManager.Instance.MyCanvas.OpenConsole(true);
			});
		keywords.Add("Debug", () =>
			{
				HoloNetworkManager.Instance.MyCanvas.OpenConsole(true);
			});
		keywords.Add("Close Console", () =>
			{
				HoloNetworkManager.Instance.MyCanvas.OpenConsole(false);
			});
		keywords.Add("See Mesh", () =>
			{
				_sMap.DrawVisualMeshes = true;
			});
		keywords.Add("Disable Mesh", () =>
			{
				_sMap.DrawVisualMeshes = false;
			});
		keywords.Add("Freeze", () =>
			{
				_sMap.CleanupObserver();
				_sMap.StopObserver();
			});
        keywords.Add("Pivot", () =>
            {
                selected = Pivot.Instance.Tap;
                selected.VoiceSelect();
            });

		#endregion
		#region Animation Speech
		keywords.Add("Frown", () =>
			{
				PlayerNetworkController.Instance.ButtonPressed(0);
			});
		keywords.Add("Smile", () =>
			{
				PlayerNetworkController.Instance.ButtonPressed(1);
			});
		keywords.Add("Pain", () =>
			{
				PlayerNetworkController.Instance.ButtonPressed(2);
			});
		keywords.Add("Serious", () =>
			{
				PlayerNetworkController.Instance.ButtonPressed(3);
			});
		keywords.Add("Close Eyes", () =>
			{
				PlayerNetworkController.Instance.ButtonPressed(4);
			});
		keywords.Add("Surprised", () =>
			{
				PlayerNetworkController.Instance.ButtonPressed(5);
			});
		keywords.Add("Sad", () =>
			{
				PlayerNetworkController.Instance.ButtonPressed(6);
			});
		keywords.Add("Suspicious", () =>
			{
				PlayerNetworkController.Instance.ButtonPressed(7);
			});	
		keywords.Add("Left Smirk", () =>
			{
				PlayerNetworkController.Instance.ButtonPressed(8);
			});
		keywords.Add("Right Smirk", () =>
			{
				PlayerNetworkController.Instance.ButtonPressed(9);
			});
		keywords.Add("Idle", () =>
			{
				PlayerNetworkController.Instance.ButtonPressed(-1);
			});
		#endregion

		// Tell the KeywordRecognizer about our keywords.
		keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

		// Register a callback for the KeywordRecognizer and start recognizing!
		keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
		keywordRecognizer.Start();
	}

	private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
	{
		System.Action keywordAction;
		if (keywords.TryGetValue(args.text, out keywordAction))
		{
			keywordAction.Invoke();
		}
	}

	#if UNITY_EDITOR
	void FixedUpdate()
	{
		if (GameObject.FindGameObjectWithTag("Player") != null) {
			GameObject face = GameObject.FindGameObjectWithTag("Player");
			UIController myCanvas = HoloNetworkManager.Instance.MyCanvas;
			switch (myWords) {
			case words.grabFace:
				face.GetComponent<TapToPlace>().VoiceSelect();
				myWords = words.none;
				break;
			case words.grabmenu:
				HoloNetworkManager.Instance.MyCanvas.SendMessage("GrabMenu");
				break;
			case words.place:
				face.GetComponent<TapToPlace>().VoiceSelect();
				myWords = words.none;
				break;
			case words.locked:
				face.SendMessage ("OnLock");
				myWords = words.none;
				break;
			case words.unlock:
				face.SendMessage ("UnLock");
				myWords = words.none;
				break;
			case words.menu:
				myCanvas.EnableCanvas ();
				myWords = words.none;
				break;
			case words.close:
				myCanvas.DisableCanvas ();
				myWords = words.none;
				break;
			case words.tracking:
				face.SendMessage ("Tracking");
				myWords = words.none;
				break;
			case words.console:
				myCanvas.OpenConsole(true);
				myWords = words.none;
				break;
            case words.pivot:

			default:
				break;
			}
		}
	}
	#endif
}