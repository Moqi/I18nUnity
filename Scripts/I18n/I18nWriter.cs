using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Text;


public class I18nWriter
{
	private Dictionary<string, I18nItem> _table;

	public I18nWriter (I18nReader reader)
	{
		_table = reader.GetTable ();
	}
	
	public I18nWriter( Dictionary<string, I18nItem> table){
		_table=table;
	}

	public void Write (string local)
	{
		
		FileStream _file = new FileStream (local, FileMode.Create);
		TextWriter tw = new StreamWriter (_file, Encoding.UTF8);
		StringBuilder st = new StringBuilder ();
		foreach (KeyValuePair<string, I18nItem> item in _table) {
			st.Append ("\n");
			st.Append ("#: " + item.Value.file + ":" + item.Value.line + "\n");
			st.Append ("msgid " + item.Value.msgid + "\n");
			st.Append ("msgstr " + item.Value.msgstr + "\n");
			
		}
		tw.Write (st.ToString ());
		tw.Close ();
		_file.Close ();
	}
	
	public static void Write(Dictionary<string, I18nItem> table, string local){
		I18nWriter writer=new I18nWriter(table);
		writer.Write(local);
		writer=null;
	}
	
	
	
}


