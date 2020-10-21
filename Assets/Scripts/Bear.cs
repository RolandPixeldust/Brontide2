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

	int idleId = Animator.StringToHash("Base Layer.BearIdle");
	int walkId = Animator.StringToHash("Base Layer.BearWalk");

	public void Restart()
	{
		lerp=0;
		if(anim)
		{
			anim.speed= animSpeed;
			anim.Play(walkId);
		}
		//startPos.parent=transform.parent;
		//endPos.parent=transform.parent;
	}

	private void Update()
	{
		lerp +=Time.deltaTime*lerpSpeed;
		lerp = Mathf.Clamp01(lerp);
		transform.position = Vector3.Lerp(startPos.position, endPos.position, lerp);

		if(lerp==1 && anim!=null)
			anim.Play(idleId);
	}
}
