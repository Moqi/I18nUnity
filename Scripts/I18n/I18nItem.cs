using System;
public struct I18nItem
{
	public string file;
	public string msgid;
	public string msgstr;
	public string line;
	public string comment;	
}

public enum LineType
{
	EMPTY_LINE = 0,
	COMMENT_LINE = 1,
	FILE_LINE = 2,
	MSGID_LINE = 3,
	MSGSTR_LINE = 4
}