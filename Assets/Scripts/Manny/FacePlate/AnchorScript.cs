using System;
using UnityEngine;



public class AnchorScript : MonoBehaviour 
{
	UnityEngine.XR.WSA.Persistence.WorldAnchorStore store;
	UnityEngine.XR.WSA.WorldAnchor retTrue;
	UnityEngine.XR.WSA.WorldAnchor myAnchor;
	//GameObject parent;
	TapToPlaceParent placer;
	UIController myUI_script;
	//SaveToFile DB;
	User myUser;
	public swapHeads mySwapHeads;
	ErrorHandler myError;


	void Start () 
	{
		//DB = new SaveToFile(); 

		placer = TapToPlaceParent.Instance;
		if(placer == null)
			placer = this.gameObject.GetComponent<TapToPlaceParent>();

		myUI_script = UIController.Instance;

		myError = ErrorHandler.Instance;

		//setup_myUser();
		#if UNITY_EDITOR
		LoadAnchors();
		#else
		WorldAnchorStore.GetAsync(AnchorStoreLoaded);
		#endif
	}
		

	private void AnchorStoreLoaded(UnityEngine.XR.WSA.Persistence.WorldAnchorStore stored)
	{
        if (stored == null)
        {
            throw new ArgumentNullException();
        }

        this.store = stored;
		LoadAnchors();
		myError.errorMsg = "in AnchorStoreLoaded";
	}

	private void LoadAnchors()
	{
		#if !UNITY_EDITOR

		string[] ids = store.GetAllIds();
        myError.errorMsg = "ids.length: " + ids.Length;
		for(int i = 0; i< ids.Length;i++)
			myError.errorMsg = ("id["+i+"]: "+ids[i]);

		//retTrue = store.Load(this.name.ToString(), this.gameObject);
		if(ids.Length == 0)
			return;
		
		retTrue = store.Load(ids[0], this.gameObject);
		if (!retTrue)
		{
			myError.errorMsg = "RecTrue == null";
			// Until the gameObjectIWantAnchored has an anchor saved at least once it will not be in the AnchorStore
			return;
		}
		else
		{
			myError.errorMsg = "decoding";
			decodeString(ids[0]);
			placer.isLocked = true;
		}
		myError.errorMsg = "leaving load";
		#endif
			
		/*myUser = (User)DB.LoadGame("MannyQuinn_save");
		if(myUser != null)
		{
			myError.errorMsg = "found text file";
			placer.isLocked = true;
			setValues(myUser);
		}
		else
			myError.errorMsg = "no text file";
		*/
	}

	void decodeString(string s)
	{
		string[] tokens = s.Split('@');
		for(int i =0; i< tokens.Length;i++)
			tokens[i] = tokens[i].Trim('@');

		if(tokens.Length > 3)
		{
			swapHeads.Instance.setFace( Convert.ToInt32(tokens[1]));
			swapHeads.Instance.faceColor_set( Convert.ToInt32(tokens[2]));
			Vector3 scale = Vector3.one * Convert.ToSingle(tokens[3]);
			transform.localScale = scale;
			myError.errorMsg = "decoded string successfully";
		}
		else
		{
			myError.errorMsg = "failed to decode string";
			string b = "";
			foreach(string x in tokens)
				b += x + " / ";
			myError.errorMsg = "tokens: "+b;
		}
	}

	public void SaveAnchor()
	{
		myError.errorMsg = "saving anchor";
		#if !UNITY_EDITOR
		myAnchor = this.gameObject.AddComponent<WorldAnchor>();
		// Remove any previous worldanchor saved with the same name so we can save new one
		if(store == null)
		{
			myError.errorMsg = "store == null";
			AudioManager.Instance.playDenied();
			return;
		}
		else
		{
			foreach(string s in store.GetAllIds())
				store.Delete(s); 
		}
		#endif

		string fileName = this.name.ToString()+"@"+swapHeads.Instance.current+"@"+swapHeads.Instance.colorIndex+'@'+transform.localScale.x.ToString("F");
		if (!store.Save(fileName, myAnchor))
			myError.errorMsg = "anchor save failed";
		else
			myError.errorMsg = "saving achor was successful";

		//setup_myUser();
		//DB.SaveGame(myUser);
	}

	public void RemoveAnchor()
	{
		myError.errorMsg = "removing anchor";
		myAnchor = this.GetComponent<UnityEngine.XR.WSA.WorldAnchor>();
		if (myAnchor)
		{
			// remove any world anchor component from the game object so that it can be moved
			#if !UNITY_EDITOR
			DestroyImmediate(myAnchor);
			if(store == null)
			{
				AudioManager.Instance.playDenied();
				return;
			}
			myError.errorMsg = "Removing anchors";
			foreach(string s in store.GetAllIds())
				store.Delete(s);
			#endif
		}
		//DB.deleteFile("user_pref","MannyQuinn_save.txt");
	}

	void OnLock()
	{
		if(placer.isLocked)
		{
			AudioManager.Instance.playDenied();
			myError.errorMsg = "face already locked";
			return;
		}
		placer.isLocked = true;
		myError.errorMsg = "face locked";
		SaveAnchor();
		AudioManager.Instance.playClick();
	}

	void UnLock()
	{
		if(!placer.isLocked)
		{
			AudioManager.Instance.playDenied();
			myError.errorMsg = "face already unlocked";
			return;
		}
		placer.isLocked = false;
		myError.errorMsg = "face unlocked";
		RemoveAnchor();
		AudioManager.Instance.playClick();
	}

	//store current values from face to user
	void setup_myUser()
	{
        if(mySwapHeads != null)
		    myUser = new User(mySwapHeads.current.ToString(),transform.localScale.x.ToString(),mySwapHeads.colorIndex.ToString(),
			    transform.localPosition.x.ToString(),transform.localPosition.y.ToString(),transform.localPosition.z.ToString(),
			    transform.localRotation.eulerAngles.x.ToString(),transform.localRotation.eulerAngles.y.ToString(),
			    transform.localRotation.eulerAngles.z.ToString());
	}

	//apply store user values to current face
	void setValues(User usey)
	{
//		swapHeads.Instance.setFace(Convert.ToInt16(usey.face));
//		myUI_script.updateFaceColor_set(Convert.ToInt16(usey.faceColor));

		//varibles
		float scale = Convert.ToSingle(usey.scale);
		Vector3 pos = new Vector3(Convert.ToSingle(usey.xPos),Convert.ToSingle(usey.yPos),Convert.ToSingle(usey.zPos));
		Vector3 rot = new Vector3(Convert.ToSingle(usey.xRot),Convert.ToSingle(usey.yRot),Convert.ToSingle(usey.zRot));

		//setting here
		transform.localScale = new Vector3(scale,scale,scale);
		transform.localPosition = pos;
		transform.eulerAngles = rot;
	}
}

