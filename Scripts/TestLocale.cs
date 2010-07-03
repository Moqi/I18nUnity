using UnityEngine;
using System.Collections;

public class TestLocale : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		I18nUtil.Init ("pt_BR");
		gameObject.guiText.text = I18nUtil.GetValue ("congratulations",1,"2","3","4");
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
