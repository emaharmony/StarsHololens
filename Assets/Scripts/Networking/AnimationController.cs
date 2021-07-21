using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimationController : MonoBehaviour
{
	public enum ExpressionType 
	{
		Frown,
		Smile,
		Pain,
		Serious,
		CloseEyes,
		Surpised,
		Sad,
		Suspicious,
		LeftSmirk,
		RightSmirk,
		Idle = -1
	}


	//temp var
	public int numOfAnimations = 10;

	public static AnimationController Instance { get; private set; }

	[SerializeField]float animationSpeed = 33;
	[SerializeField]bool mixAnim = false;

    ExpressionType animationTypeQueue; 
	ExpressionType currEmotion = ExpressionType.Idle;

	SkinnedMeshRenderer _sMRender;
	Mesh _skinnedMesh;

	AnimationValueType[] animationList;

	void Awake()
	{
		ExpressionType[] aList = (ExpressionType[] )Enum.GetValues (typeof(ExpressionType)) ;
		numOfAnimations = aList.Length - 1;
		animationList = new AnimationValueType[numOfAnimations];
		for (int i = 0; i < aList.Length; i++) 
		{
			if ((int)aList[i] >= 0)
				animationList [i] = new AnimationValueType ((int)aList [i], 0);	
		}

		Instance = this;
	}

	void Start()
	{
		StartCoroutine ("LateStart", 0.15f);
	}

	IEnumerator LateStart(float waitTime)
	{
		yield return new WaitForSeconds (waitTime);
		_sMRender = GetComponentInChildren<SkinnedMeshRenderer> ();
		_skinnedMesh = _sMRender.sharedMesh;
	}
		
	public IEnumerator AnimationTransition()
	{
		if (animationTypeQueue != ExpressionType.Idle) {
			float nextAnim = _sMRender.GetBlendShapeWeight ((int)animationTypeQueue);
			float currAnim = _sMRender.GetBlendShapeWeight ((int)currEmotion);
			while (nextAnim < 100) {
				if (!mixAnim && currEmotion != ExpressionType.Idle && currEmotion != animationTypeQueue && currAnim > 0) {
					currAnim -= Time.deltaTime * animationSpeed;
					currAnim = (currAnim < 0.05f ? 0 : currAnim);
					_sMRender.SetBlendShapeWeight ((int)currEmotion, currAnim);
				}

				nextAnim += Time.deltaTime * animationSpeed;
				nextAnim = (nextAnim > 100 ? 100 : nextAnim);
				_sMRender.SetBlendShapeWeight ((int)animationTypeQueue, nextAnim);
				yield return null;
			}
		} else if (currEmotion != ExpressionType.Idle) {
			if (!mixAnim) {
				float currAnim = _sMRender.GetBlendShapeWeight ((int)currEmotion);
				while (currAnim > 0) {
					currAnim -= Time.deltaTime * animationSpeed;
					currAnim = (currAnim < 0.05f ? 0 : currAnim);
					_sMRender.SetBlendShapeWeight ((int)currEmotion, currAnim);
					yield return null;
				}
			} else {
				foreach (ExpressionType e in Enum.GetValues(typeof(ExpressionType))) {
					float currAnim = _sMRender.GetBlendShapeWeight ((int)e);
					while (currAnim > 0) {
						currAnim -= Time.deltaTime * animationSpeed;
						currAnim = (currAnim < 0.05f ? 0 : currAnim);
						_sMRender.SetBlendShapeWeight ((int)e, currAnim);
						yield return null;
					}
				}
			}

		}
		currEmotion = animationTypeQueue;
		yield return null;
	}

	public bool MixFlag
	{
		get{ return mixAnim; }
		set{ mixAnim = value;}
	}

	public void AdjustAnimationSlider(int i, float val)
	{
		animationList [i].Value = val;
		_sMRender.SetBlendShapeWeight (i, val);
	}

	public void TurnAnimationOn(int i, bool b)
	{
		animationList [i].IsOn = b;

	}

	public AnimationValueType GetAnimationValue(int i)
	{
		return i >= 0 ? animationList [i] : null;
	}

	public SkinnedMeshRenderer SMesh
	{
		get{ return _sMRender; }
		set{  _sMRender = value; }
	}

	public ExpressionType Queue 
	{
		get { return animationTypeQueue;}
		set { animationTypeQueue = value;}
	}
}
