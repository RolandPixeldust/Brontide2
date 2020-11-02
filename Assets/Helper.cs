using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class Helper : MonoBehaviour
{
/*
	[System.Serializable]
	public struct LoadableAsset
	{
		public Loadable loadable;
		public Sprite sprite;
	}

	public List<Loadable> myObjects;
	public LoadableAsset [] myAssets;

	

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
	public void GetAssets()
	{

		var objs = FindObjectsOfType<Loadable>();
		myAssets = new LoadableAsset[objs.Length];
		for (int i = 0; i < objs.Length; i++)
		{
			myAssets[i].loadable = objs[i];
			Sprite s = null;
			if (myAssets[i].loadable.image != null) s = myAssets[i].loadable.image.sprite;
			if (myAssets[i].loadable.spriteRenderer != null) s = myAssets[i].loadable.spriteRenderer.sprite;

			myAssets[i].sprite = s;

		}
	}

	[Button(ButtonSizes.Large)]
	public void ReAssignSprites()
	{
		for (int i = 0; i < myAssets.Length; i++)
		{
			if (myAssets[i].loadable.image != null) myAssets[i].loadable.image.sprite = myAssets[i].sprite;
			if (myAssets[i].loadable.spriteRenderer != null) myAssets[i].loadable.spriteRenderer.sprite = myAssets[i].sprite;
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
	*/
}
