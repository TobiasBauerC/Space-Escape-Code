using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameData;

[RequireComponent(typeof(Text))]

public class TextLangSetter : MonoBehaviour {

	[SerializeField] private bool _initializeWithText = true;
	[SerializeField] private Text _text;
	[SerializeField] private string _key;

	// Use this for initialization
	void Start () 
	{
		if(_initializeWithText)
			SetText(_key);
	}
	
	public void SetText()
	{
		_text.text = _key.Localize();
	}

	public void SetText(string pKey)
	{
		_text.text = pKey.Localize();
	}

	public string GetText()
	{
		return _key.Localize();
	}

	public string GetText(string pKey)
	{
		return pKey.Localize();
	}
}
