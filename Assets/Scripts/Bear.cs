using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Bear : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;
	public Transform endEnd;
	public Transform endStart;
	public float lerpSpeed=1;
	public Animator anim;
	public float animSpeed;
	[Range(0,1)] public float lerp;

	public enum Type {Linear, Float};
	public Type type;

	int idleId = Animator.StringToHash("Base Layer.BearIdle");
	int walkId = Animator.StringToHash("Base Layer.BearWalkNew");

	[ShowIf("type", Type.Float)] public float sinPhase;
	[ShowIf("type", Type.Float)] public float sinAmp;
	[ShowIf("type", Type.Float)] public float sinSpeed; 
	[ShowIf("type", Type.Float)] public Vector3 additiveFloatVector;
	[ShowIf("type", Type.Float)] public float driftCooldown;
	[ShowIf("type", Type.Float)] public float driftDuration;

	public Vector3 startScale;

	private void Awake()
	{
		if(anim)
		{
			startScale = transform.localScale;
			transform.localScale = startScale * .7f;
			anim.SetBool("Idle", false);
			anim.Play(walkId);
			anim.speed=Random.Range(.24f,.4f);
		}
	}

	public void Restart()
	{
		lerp=0;
		if(anim)
		{
			anim.Play(walkId,0);
			sinPhase=0;
			additiveFloatVector= Vector3.zero;
			sinPhase=0;
			transform.localScale = startScale*.7f;
			anim.speed = Random.Range(.25f, .4f);
			driftCooldown=0;
		}
	}

	private void Update()
	{
		lerp +=Time.deltaTime*lerpSpeed;
		lerp = Mathf.Clamp01(lerp);

		switch(type)
		{
			case Type.Linear: 
				transform.position = Vector3.Lerp(startPos.position, endPos.position, lerp); 
				break;

			case Type.Float:
				if(lerp < 1)
				{
					endPos.position = Vector3.Lerp(endStart.position,endEnd.position,lerp);
					transform.position = Vector3.Lerp(startPos.position, endPos.position, lerp); 
					sinPhase=0;
					
				}
				else
				{
					sinPhase += Time.deltaTime * sinSpeed;
					transform.position= endPos.position + new Vector3(0,Mathf.Sin(sinPhase) * sinAmp, 0);
				}

			
				break;
		}

		if(lerp==1 && anim!=null)
		{
			animSpeed=1;
			anim.Play(idleId);
			transform.localScale=startScale;
		}
	}
}
