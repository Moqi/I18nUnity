using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class I18nEditor : EditorWindow
{
	public static string LOCALIZATION_DIR=I18n.ABSOLUTE_LOCALIZATION_DIR;
	public static I18nEditor window;
	private I18nReader _reader;
	private I18nWriter _writer;
	private string _currentLanguage="pt_BR";
	private bool _generateBackup=true;
	private FileStream _file;
	private Dictionary<string, I18nItem> _table;
		
	[MenuItem("Utils/Internacinalization Editor")]
	static void Init ()
	{
		window = (I18nEditor)EditorWindow.GetWindow (typeof(I18nEditor));
		window.Show ();
	}
	
	[MenuItem("Utils/Internacinalization/Generate")]
	static void Generate(){
	   //LoadFile ();
	   
	}
	
	public string LANGUAGE_FILE{
		get{
			return  LANGUAGE_DIR  + _currentLanguage + ".txt";
		}
	}
	
	public string LANGUAGE_TEMP_FILE{
		get{
			return LANGUAGE_DIR + "temp_" + _currentLanguage + ".txt";
		}
	}
	
	public string LANGUAGE_DIR{
		get{
			if(!Directory.Exists(LOCALIZATION_DIR)) { 
				Directory.CreateDirectory(LOCALIZATION_DIR);
			}
			
			if(!Directory.Exists(LOCALIZATION_DIR + _currentLanguage)) Directory.CreateDirectory(LOCALIZATION_DIR + _currentLanguage);
			if(!Directory.Exists(LOCALIZATION_DIR + _currentLanguage +"/LC_MESSAGES")) 	Directory.CreateDirectory (LOCALIZATION_DIR + _currentLanguage +"/LC_MESSAGES");
			return LOCALIZATION_DIR + _currentLanguage + "/LC_MESSAGES/";
		}
	}
	
	void OnGUI ()
	{
		_currentLanguage=EditorGUILayout.TextField( "Language", _currentLanguage);
		_generateBackup = EditorGUILayout.Toggle ("Generate Backup ?", _generateBackup);
		if (GUILayout.Button ("Generate PO Files")) {
			LoadFile ();
		}
		
		if(window!=null) window.Repaint();
	}

	private void LoadFile ()
	{
		_table=new Dictionary<string, I18nItem>();
		if(File.Exists(LANGUAGE_FILE)){
		   _file = new FileStream (LANGUAGE_FILE, FileMode.Open);
		   ParseFile(_file);
		   _file.Close();		   
		 }else{		   
		   _table=new Dictionary<string, I18nItem>();
		 }
		 AnalyseSources();
	}
	
	private void WriteFile (Dictionary<string, I18nItem> table)
	{		
		if(File.Exists(LANGUAGE_FILE) && _generateBackup==true){
			if( File.Exists(LANGUAGE_TEMP_FILE)) File.Delete(LANGUAGE_TEMP_FILE);
			File.Move(LANGUAGE_FILE , LANGUAGE_TEMP_FILE);
		}
		_writer=new I18nWriter(table);
		_writer.Write(LANGUAGE_FILE);
		((I18nEditor)EditorWindow.GetWindow (typeof(I18nEditor))).ShowNotification( new GUIContent("PO Files Createds"));
	}

	private bool AnalyseSources ()
	{
		DirectoryInfo di = new DirectoryInfo (Application.dataPath);		
		foreach (FileInfo reference in di.GetFiles ("*.cs", SearchOption.AllDirectories)) {
			if (reference.Directory.FullName.IndexOf ("Assets/Editor") < 0 && reference.DirectoryName.IndexOf ("I18n") < 0) {
				StreamReader st = reference.OpenText ();
				string line = "";
				int counter = 0;
				while ((line = st.ReadLine ()) != null) {
					if (line.IndexOf ("I18nUtil.GetValue") > -1) {						
						line = line.Remove (0, line.IndexOf ("I18nUtil.GetValue") + 1);
						Match results = Regex.Match(line,"\"[^\"]+\"");
						I18nItem po = new I18nItem ();
						po.file = reference.FullName;
						po.line = counter.ToString();
						po.msgid = results.Captures[0].Value;
						po.msgstr="\"\"";						
						if(!_table.ContainsKey(po.msgid)){
							_table[po.msgid] = po;
						}
					}
					counter++;
				}
				st.Close();
			}
			
		}
		WriteFile (_table);
		return true;
	}

	
	void ParseFile (FileStream file)
	{ 
		_reader=new I18nReader(file);
	    _table= _reader.GetTable();
	    file.Close();
	}
}



