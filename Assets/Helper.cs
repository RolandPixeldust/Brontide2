using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class Helper : MonoBehaviour
{
    public List<Loadable> myObjects;

	[Button]
	public void GetRemaining()
	{
		myObjects.Clear();
		var objs = FindObjectsOfType<Loadable>();
		bool counted=false;
		for (int i = 0; i < objs.Length; i++)
		{
			counted=false;

			if(objs[i].image!=null)
			{
				if(objs[i].image.sprite==null) counted=true;
			}

			if (objs[i].spriteRenderer != null)
			{
				if (objs[i].spriteRenderer.sprite == null) counted = true;
			}

			if(counted) myObjects.Add(objs[i]);

		}
	}

	[Button(ButtonSizes.Large)]
	void ClearSprites()
	{
		var loadables = FindObjectsOfType<Loadable>();
		foreach (var item in loadables)
		{
			if (item.image != null) item.image.sprite = null;
			if (item.spriteRenderer != null) item.spriteRenderer.sprite = null;
		}
	}

}
