using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour {

	private const string SFX_VOL = "sfxVol";
	private const string MUSIC_VOL = "musicVol";
	private const string MASTER_VOL = "masterVol";

	[Header("Volume Sliders")]
	[SerializeField] private Slider _masterVolSlider;
	[SerializeField] private Slider _musicVolSlider;
	[SerializeField] private Slider _sfxVolSlider;

	[Header("Audio Sources")]
	[SerializeField] private AudioMixer _masterMixer;
	[SerializeField] private AudioSource _sfxSource;

	[Header("Audio Clips")]
	[SerializeField] private AudioClip _buttonPressClip;

	[Header("Canvas")]
	[SerializeField] private Canvas _audioCan;

	[Header("Mute Buttons Text")]
	[SerializeField] private Text _masterText;
	[SerializeField] private Text _musicText;
	[SerializeField] private Text _SFXText;

	private float _tempMusicVol;
	private float _tempSFXVol;

	// Use this for initialization
	void Start () 
	{
		_tempMusicVol = PlayerPrefs.GetFloat(MUSIC_VOL, 0.0f);
		_tempSFXVol = PlayerPrefs.GetFloat(SFX_VOL, 0.0f);

		if(_tempMusicVol < 0.0f)
			_musicText.text = "Unmute";
		if(_tempSFXVol < 0.0f)
			_SFXText.text = "Unmute";
		if (AudioListener.pause)
			_masterText.text = "Unmute";

		//DontDestroyOnLoad(this);
		Load();
		_audioCan.enabled = false;
	}

	public void MuteSound(string pSound)
	{
		switch(pSound)
		{
		case "Master":
			if (AudioListener.pause)
			{
				AudioListener.pause = false;
				_masterText.text = "Mute";
			}
			else
			{
				AudioListener.pause = true;
				_masterText.text = "Unmute";
			}
			Save();
			Load();
			break;

		case "Music":
			if(_tempMusicVol == 0.0f)
			{
				SetMusicVolume(-80.0f);
				_tempMusicVol = -80.0f;
				_musicText.text = "Unmute";
			}
			else
			{
				SetMusicVolume(0.0f);
				_tempMusicVol = 0.0f;
				_musicText.text = "Mute";
			}
			Save();
			Load();
			break;

		case "SFX":
			if(_tempSFXVol == 0.0f)
			{
				SetSFXVolume(-80.0f);
				_tempSFXVol = -80.0f;
				_SFXText.text = "Unmute";
			}
			else
			{
				SetSFXVolume(0.0f);
				_tempSFXVol = 0.0f;
				_SFXText.text = "Mute";
			}
			Save();
			Load();
			break;
		}
	}

	public void SetMasterVolume(float pMasterVol)
	{
		_masterMixer.SetFloat(MASTER_VOL, pMasterVol);
	}

	public void SetMusicVolume(float pMusicVol)
	{
		_masterMixer.SetFloat(MUSIC_VOL, pMusicVol);
	}

	public void SetSFXVolume(float pSFXVol)
	{
		_masterMixer.SetFloat(SFX_VOL, pSFXVol);
	}

	public void OnAudioSettings()
	{
		_audioCan.enabled = !_audioCan.enabled;
		Save();
	}

	public void OnButtonSound()
	{
		PlaySound(_buttonPressClip);
	}

	public void PlaySound(AudioClip pSound, float pVolume = 1.0f)
	{
		_sfxSource.PlayOneShot(pSound, pVolume);
	}

	public void Save()
	{
		float masterVol = 0.0f;
		float musicVol = 0.0f;
		float sfxVol = 0.0f;

		_masterMixer.GetFloat(MASTER_VOL, out masterVol);
		PlayerPrefs.SetFloat(MASTER_VOL, masterVol);

		_masterMixer.GetFloat(MUSIC_VOL, out musicVol);
		PlayerPrefs.SetFloat(MUSIC_VOL, musicVol);

		_masterMixer.GetFloat(SFX_VOL, out sfxVol);
		PlayerPrefs.SetFloat(SFX_VOL, sfxVol);
	}

	public void Load()
	{
		_masterVolSlider.value = PlayerPrefs.GetFloat(MASTER_VOL, 0.0f);
		_musicVolSlider.value = PlayerPrefs.GetFloat(MUSIC_VOL, 0.0f);
		_sfxVolSlider.value = PlayerPrefs.GetFloat(SFX_VOL, 0.0f);
	}

	public void Clear()
	{
		PlayerPrefs.DeleteAll();
		Load();
	}
}
