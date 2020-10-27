using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
		distAwayThresh = 200;
		fadeDur = .25f;
	}

	private void OnEnable()
	{
		//StartCoroutine(FadeIn());
		canvasGroup.alpha = 0;
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
		//this.gameObject.SetActive(false);
	}

	/*
	void Update()
    {
		if(SliderControl.master.state == SliderControl.State.Intro || SliderControl.master.impactSlider.value==0) return;
        distAwayFromPointer = Vector3.Distance(transform.position, Input.mousePosition);
		if(distAwayFromPointer<distAwayThresh)
		{
			//cooldown+=Time.deltaTime;
			//if(active) 
			//{
			canvasGroup.alpha+=Time.deltaTime*2;
			canvasGroup.alpha = Mathf.Clamp01(canvasGroup.alpha);
			active = true;
			//StartCoroutine(FadeOut());
			//}
		}
		else
		{
			//if(!active)
			//{
			canvasGroup.alpha -= Time.deltaTime*2;
			canvasGroup.alpha = Mathf.Clamp01(canvasGroup.alpha);
			active =false;
			//}
			//cooldown=0;
		}
	}
	*/

	public void OnPointerEnter(PointerEventData pointerEventData)
	{
		if (SliderControl.master.state == SliderControl.State.Intro || SliderControl.master.impactSlider.value == 0) return;
		if (!active) StartCoroutine(FadeIn());
		active = true;
		
	}

	//Detect when Cursor leaves the GameObject
	public void OnPointerExit(PointerEventData pointerEventData)
	{
		if (SliderControl.master.state == SliderControl.State.Intro || SliderControl.master.impactSlider.value == 0) return;
		if (active) StartCoroutine(FadeOut());
		active = false;
	}

}
