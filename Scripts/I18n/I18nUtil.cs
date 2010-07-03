

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class I18nUtil:MonoBehaviour {

	// Use this for initialization

	public string[] languages;
	//
	protected string language;
	protected Dictionary<String,String> _dictionary;
	//
	private I18nReader _data;
	private static I18nUtil _instance;
	
	void Start ()
	{
		_instance = this;
		GameObject.DontDestroyOnLoad (this);
	}
	
	private void internal_Init (string language_p)
	{
		_dictionary = new Dictionary<string, string> ();
		language = language_p;
		I18n.setLanguage(language);
		_data=new I18nReader( Resources.Load(I18n.LANGUAGE_FILE).ToString());
	}
	
	private string internal_GetValue (string id_p)
	{  
		if (_data.ContainsKey (id_p))
			return _data.GetText(id_p);
		    return "Text with id \"" + id_p + "\" not found on language " + language;
	}
	
	private string internal_GetValue (string id_p, params object[] parameters_p){
		return String.Format (internal_GetValue (id_p), parameters_p);
	}

	public static string GetValue (string id_p)	{
		return _instance.internal_GetValue (id_p);
	}

	public static string GetValue (string id_p, params object[] args)
	{
		
		return _instance.internal_GetValue (id_p, args);
	}

	public static void Init (string language)
	{
		_instance.internal_Init (language);
	}

}
