using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

	AudioSource _audio;

	public AudioClip clip_One;
	public AudioClip clip_Two;
	public AudioClip clip_Three;
	public AudioClip myClick;
	public AudioClip myDenied;
	public AudioClip myGlobalWarming;
	public AudioClip myDing;

	public ErrorHandler myHandler;

	public static AudioManager Instance 
	{ 
		get; 
		private set; 
	}

	void Awake ()
	{
		Instance = this;
	}

	void OnEnable()
	{
		_audio = gameObject.AddComponent<AudioSource>();
		_audio.clip = clip_One;
		_audio.playOnAwake = false;
	}

	public void playClip(int i)
	{
		switch(i)
		{
			case 0:
				playClick();
				break;
			case 1:
				playDenied();
				break;
			case 2:
				playGlobalWarming();
				break;
		}
	}

	public void playClick()
	{
		_audio.clip = myClick;
		_audio.Play();
	}

	public void playDenied()
	{
		_audio.clip = myDenied;
		_audio.Play();

		myHandler.displayError();
	}

	public void playGlobalWarming()
	{
		
		if (!_audio.isPlaying) 
		{
			_audio.clip = myGlobalWarming;	
			_audio.Play ();
		}
	}

	public void playDing()
	{
		_audio.clip = myDing;
		_audio.Play();
	}
	public void playOne()
	{
		_audio.clip = clip_One;
		_audio.Play();
	}
	public void playTwo()
	{
		_audio.clip = clip_Two;
		_audio.Play();
	}
	public void playThree()
	{
		_audio.clip = clip_Three;
		_audio.Play();
	}

	public AudioSource AudioS {
		get{ return _audio;}
	}
}
