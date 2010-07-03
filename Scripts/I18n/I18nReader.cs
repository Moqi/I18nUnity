using System;
using System.IO;
using System.Collections.Generic;

using UnityEngine;
using System.Text;


public class I18nReader
{
	private string _data;
	private Dictionary<string, I18nItem> _table;
	private string [] _lines;
	
	public I18nReader (string file)
	{
		_data = file;
		_table = new Dictionary<string, I18nItem> ();
		_lines= StringUtil.StrToLine(_data);
		ParseHeader ();
		ParseValues ();
	}
		
	public I18nReader (FileStream file){
		
		_data = new StreamReader (file, Encoding.UTF8).ReadToEnd ();
		_table = new Dictionary<string, I18nItem> ();
		_lines = StringUtil.StrToLine(_data);
		file.Close();
		ParseHeader ();
		ParseValues ();
	}
	
	private void ParseHeader ()
	{
		//nothing
		
	}

	private void ParseValues ()
	{	
		LineType _currentLine = LineType.EMPTY_LINE;
		_table = new Dictionary<string, I18nItem> ();
		string line = string.Empty;
		I18nItem po = new I18nItem ();
		
		for(int i=0;i<_lines.Length;i++){
			line=_lines[i];
			if (line.Trim ().Length == 0) {			
				_currentLine = LineType.EMPTY_LINE;
			} else {
				if (line[0] == '#') {
					if (line[1] == ':') {
						_currentLine = LineType.FILE_LINE;
					} else {
						_currentLine = LineType.COMMENT_LINE;
					}
				} else {
					
					if (line.IndexOf ("msgid") == 0) {
						_currentLine = LineType.MSGID_LINE;
					}
					
					if (line.IndexOf ("msgstr") == 0) {
						_currentLine = LineType.MSGSTR_LINE;
					}
				}
				
			}
		  switch (_currentLine) {
			case LineType.EMPTY_LINE:
				
				po = new I18nItem ();				
				break;
			
			case LineType.COMMENT_LINE:
				//TODO:Trazer comentario do codigo fonte
				po.comment= line.Replace("# ",string.Empty);
				break;
			
			case LineType.FILE_LINE:
				po.file = line.Substring (0, line.LastIndexOf (":")).Replace ("#:", string.Empty).TrimStart ();
				po.line = line.Substring (line.LastIndexOf (":") + 1);
				break;
			
			case LineType.MSGID_LINE:
				po.msgid = line.Replace ("msgid ", string.Empty);
				po.msgid = po.msgid.Replace ("\"", string.Empty);
				break;
			
			case LineType.MSGSTR_LINE:
				po.msgstr = line.Replace ("msgstr ", string.Empty);
				po.msgstr = po.msgstr.Replace("\"",string.Empty);
				AddI18nItem(po);
				break;
			default:
				break;
			}

		}
	}
    
    private void AddI18nItem(I18nItem po){
		_table[po.msgid]=po;
    }
        
	public bool ContainsKey (string key)
	{	
		return _table.ContainsKey (key);
	}

	public string GetText (string key)
	{
		return ContainsKey (key) ? _table[key].msgstr : "Not exist in this language";
	}

	public Dictionary<string, I18nItem> GetTable ()
	{
		return _table;
	}
	
	public static Dictionary<string, I18nItem> ParseValues(FileStream file){
	  return new I18nReader(file).GetTable();
	}
	
	public static Dictionary<string, I18nItem> ParseValues(string data){
		return new I18nReader(data).GetTable();
	}
	
}







