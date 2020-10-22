using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Bear : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;
	public float lerpSpeed=1;
	public Animator anim;
	public float animSpeed=.25f;
	[Range(0,1)] public float lerp;

	public enum Type {Linear, Float };
	public Type type;

	int idleId = Animator.StringToHash("Base Layer.BearIdle");
	int walkId = Animator.StringToHash("Base Layer.BearWalk");

	Vector3 additiveFloatVector;
	[ShowIf("type", Type.Float)] public Vector3 additiveFloatVectorDir;

	public void Restart()
	{
		lerp=0;
		if(anim)
		{
			anim.speed= animSpeed;
			anim.Play(walkId);
			additiveFloatVector=Vector3.zero;
		}
	}

	private void Update()
	{
		lerp +=Time.deltaTime*lerpSpeed;
		lerp = Mathf.Clamp01(lerp);
		switch(type)
		{
			case Type.Linear: transform.position = Vector3.Lerp(startPos.position, endPos.position, lerp); break;
			case Type.Float:
				transform.position = Vector3.Lerp(startPos.position, endPos.position, lerp)+ additiveFloatVector; 
				additiveFloatVector += additiveFloatVectorDir *Time.deltaTime;
				break;
		}

		if(lerp==1 && anim!=null)
			anim.Play(idleId);
	}
}
