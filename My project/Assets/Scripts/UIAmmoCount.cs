using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAmmoCount : MonoBehaviour 
{
	public static UIAmmoCount Instance { get; private set;}

	public Text countText;
	
	void Awake ()
	{
		Instance = this;
	}

	public void SetAmmo(int count, int max)
	{
		countText.text = "x" + count + "/" + max;
	}
}
