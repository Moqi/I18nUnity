using System;
using  UnityEngine;

	public class I18n:MonoBehaviour
	{
	    private static string _language = "pt_BR";
		private static string _LANGUAGE_DIR = "languages/{0}/";
	   	public static string LOCALIZATION_DIR = "Resources/languages/";
		public static string ABSOLUTE_LOCALIZATION_DIR = Application.dataPath+"/" +LOCALIZATION_DIR;
		public static string ABSOLUTE_LANGUAGE_DIR = Application.dataPath + "Assets/Resources/" + LANGUAGE_DIR;
		public static string ABSOLUTE_LANGUAGE_FILE = ABSOLUTE_LANGUAGE_DIR + LANGUAGE_FILE + ".txt"; 
		public static string LANGUAGE_DIR{
		   get{
			 return String.Format(_LANGUAGE_DIR, _language );
		   }  
		}
		
		public static string LANGUAGE_FILE{
		   get{
		  	return LANGUAGE_DIR +"LC_MESSAGES/"+ _language;
		   }
		}
		
		public static void setLanguage( string language){
		  _language=language;
		}
	}


