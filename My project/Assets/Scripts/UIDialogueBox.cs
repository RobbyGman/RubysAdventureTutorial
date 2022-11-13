using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDialogueBox : MonoBehaviour
{
	public static UIDialogueBox Instance { get; private set; }
	
	public Image portrait;
	public TextMeshProUGUI text;

	void Awake()
	{
		Instance = this;
	}

	void Start () 
	{
		gameObject.SetActive(false);	
	}


	public void DisplayText(string content)
	{
		text.text = content;
	}

	public void DisplayPortrait(Sprite spr)
	{
		portrait.sprite = spr;
	}

	public void Show()
	{
		gameObject.SetActive(true);
	}
	
	public void Hide()
	{
		gameObject.SetActive(false);
	}
}
