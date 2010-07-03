using System;


public class StringUtil
{

     public static string[] StrToLine(string input){
       string temp = input.Replace("\n","+");
       return temp.Split('+');
     }
}


