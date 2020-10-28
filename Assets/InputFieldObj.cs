using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class InputFieldObj : MonoBehaviour, ISelectHandler, IDeselectHandler 
{

	public static InputFieldObj master;
    public AnimationCurve selectedCurve;
	public float selectDur;
	public AnimationCurve selectedFieldCurve;

	public Transform frameTransform;
	public Vector3 frameStartScale;
	public Transform inputFieldTransform;
	public Vector3 inputFieldStartScale;


	public Image selectionFill;
	public Gradient selectionFillColor;
	public bool selected;
	

	private void Awake()
	{
		if(InputFieldObj.master ==null) {InputFieldObj.master=this; }else {Destroy(this.gameObject); }
		frameStartScale = frameTransform.localScale;
		inputFieldStartScale = inputFieldTransform.localScale;
	}

	public void OnSelect(BaseEventData eventData)
	{
		if(!selected)
		{
			StopAllCoroutines();
			StartCoroutine(SelectAnim());
		}
		selected=true;
	}

	public void OnDeselect(BaseEventData eventData)
	{
		//Debug.Log("De-Selected");
		selected=false;
	}


	IEnumerator SelectAnim()
	{
		selectionFill.gameObject.SetActive(true);
		for (float lerp = 0; lerp < 1; lerp+=Time.deltaTime/selectDur)
		{
			selectionFill.color = selectionFillColor.Evaluate(lerp);
			frameTransform.localScale = new Vector3(frameStartScale.x*selectedCurve.Evaluate(lerp), frameStartScale.y, frameStartScale.z);
			inputFieldTransform.localScale = inputFieldStartScale* selectedFieldCurve.Evaluate(lerp);
			yield return null;
		}
		selectionFill.gameObject.SetActive(false);
		inputFieldTransform.localScale = inputFieldStartScale;
		frameTransform.localScale = frameStartScale;
	}


}
