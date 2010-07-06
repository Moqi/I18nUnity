

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
	
	void Awake ()
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
	
	private string internal_ (string id_p)
	{  
		if(_data==null) Debug.LogError("I18n no initialized");
		if (_data.ContainsKey (id_p)) return _data.GetText(id_p);
		
		return "Text with id \"" + id_p + "\" not found on language " + language;
	}
	
	private string internal_GetText (string id_p, params object[] parameters_p){
		return String.Format (internal_GetText (id_p), parameters_p);
	}

	public static string GetText (string id_p)	{
		return _instance.internal_GetText (id_p);
	}

	public static string GetText (string id_p, params object[] args)
	{
		
		return _instance.internal_GetText (id_p, args);
	}

	public static void Init (string language)
	{
	 
		_instance.internal_Init (language);
	}
	public static I18nReader GetReader(){
	   return new I18nReader( Resources.Load(I18n.LANGUAGE_FILE).ToString());
	}

}
