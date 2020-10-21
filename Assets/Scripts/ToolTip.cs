using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
   
	public float distAwayFromPointer;
	public float distAwayThresh =400;
	public float cooldown;
	public bool active;
	public CanvasGroup canvasGroup;
	public float fadeDur=1;

	private void Awake()
	{
		canvasGroup=GetComponentInChildren<CanvasGroup>();
		distAwayThresh = 400;
		fadeDur = .25f;
	}

	private void OnEnable()
	{
		StartCoroutine(FadeIn());
	}

	IEnumerator FadeIn()
	{
		active = true;
		for (float lerp = 0; lerp < 1; lerp += Time.deltaTime / fadeDur)
		{
			canvasGroup.alpha = lerp;
			yield return null;
		}
		canvasGroup.alpha=1;
	}

	IEnumerator FadeOut()
	{
		active=false;
		for (float lerp = 0; lerp < 1; lerp += Time.deltaTime / fadeDur)
		{
			canvasGroup.alpha = 1 - lerp;
			yield return null;
		}
		canvasGroup.alpha = 0;
		this.gameObject.SetActive(false);
	}


	void Update()
    {
        distAwayFromPointer = Vector3.Distance(transform.position, Input.mousePosition);
		if(distAwayFromPointer>distAwayThresh)
		{
			cooldown+=Time.deltaTime;
			if(active) StartCoroutine(FadeOut());
		}
		else
		{
			if(!active)
			{
				canvasGroup.alpha=1;
				active=true;
			}
			cooldown=0;
		}
	}


}
