using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class swapHeads : MonoBehaviour
{
	public GameObject[] faces;

	[HideInInspector]
	public int current = 0;

	[HideInInspector]
	public int colorIndex = 2;
	//int faceIndex = 0;

	Color[] defaultColors;
	Color[] faceColors;
	Material[] myMaterial;


	public static swapHeads Instance 
	{ 
		get; 
		private set; 
	}

	void Awake()
	{
		Instance = this;
	}

	void Start()
	{

		colorSetup();
		int i =0;
		for(i=0;i < faces.Length;i++)
		{
			if(faces[i].activeSelf)
			{
				current = i;
				break;
			}
		}
		for(++i;i<faces.Length;i++)
			faces[i].SetActive(false);
	}

	void colorSetup()
	{
		Renderer[] r = new Renderer[faces.Length];
		defaultColors = new Color[faces.Length];
		myMaterial = new Material[faces.Length];


		for(int i =0; i< faces.Length;i++)
		{
			r[i] = faces[i].GetComponentInChildren<Renderer>();
			myMaterial[i] = (r[i].materials.Length > 1 ? r[i].materials[1] : r[i].material);
			defaultColors[i] = myMaterial[i].color;
		}

		faceColors = new Color[5];
		faceColors[0] = new Color(131f/255f,104f/255f,69f/255f);
		faceColors[1] = new Color(234f/255f,226f/255f,160f/255f);
		faceColors[2] = new Color(1f,1f,1f);
		faceColors[3] = new Color(197f/255f,164f/255f,164f/255f);
		faceColors[4] = new Color(179f/255f,221f/255f,179f/255f);
	}

	void revertColors()
	{
		for(int i =0;i<faces.Length;i++)
		{
			myMaterial[i].color = defaultColors[i];
		}
	}

	void changeColors()
	{
		float r,g,b;
		r = Random.Range(0f,1f);
		g = Random.Range(0f,1f);
		b = Random.Range(0f,1f);

		for(int i =0;i<faces.Length;i++)
		{
			defaultColors[i] = myMaterial[i].color;
			myMaterial[i].color = new Color(r,g,b);
		}

		ErrorHandler.Instance.errorMsg = "Color Change (" + r + ", " + g + ", " + b + ")";
	}

	public string faceColor(int i)//
	{
		int a = (((i < 0 ? i + faceColors.Length : i) + colorIndex) % faceColors.Length);
		if(a < faceColors.Length && a > -1)
		{
			foreach(Material m in myMaterial)
 				m.color = faceColors[a];
			colorIndex = a;
		}

		return (colorIndex+1)+"/"+faceColors.Length;
	}

	public string faceColor_set(int i)
	{
		if(i < faceColors.Length && i > -1)
		{
			foreach(Material m in myMaterial)
				m.color = faceColors[i];
			colorIndex = i;
		}
			
		return (colorIndex+1)+"/"+faceColors.Length;
	}
		
	public void FaceColorSet(int i)
	{
		if(i < faceColors.Length && i > -1)
		{
			foreach(Material m in myMaterial)
				m.color = faceColors[i];
			colorIndex = i;
		}
	}

	public int prev()
	{

		faces[current].SetActive(false);
		current = (current + (faces.Length - 1)) % faces.Length;
		faces[current].SetActive(true);
		AnimationController.Instance.SMesh = faces [current].GetComponentInChildren<SkinnedMeshRenderer> ();


		return current+1;
	}

	public int next()
	{
		faces[current].SetActive(false);
		current = (current + 1) % faces.Length;
		faces[current].SetActive(true);
		AnimationController.Instance.SMesh = faces [current].GetComponentInChildren<SkinnedMeshRenderer> ();

		return current+1;
	}

	public int setFace(int i)
	{
		if(!(i < faces.Length) || i < 0)
			return -1;

		foreach(GameObject a in faces)
			a.SetActive(false);
		faces[current = i].SetActive(true);

		return current+1;
	}

	public void FacePresetSet(int i )
	{
		faces [current].SetActive(false);
		faces [i].SetActive (true);
		AnimationController.Instance.SMesh = faces [i].GetComponentInChildren<SkinnedMeshRenderer> ();
		current = i;
	}

	public void Reset()
	{
		AudioManager.Instance.playThree();
		transform.localPosition = Vector3.zero;
		transform.eulerAngles = Vector3.up*180f;
		setFace(0);
		faceColor_set(2);
	}

	public int ColorLength
	{
		get{ return faceColors.Length; }
	}

	public int PresetLength
	{
		get{ return faces.Length; }
	}
		
}
