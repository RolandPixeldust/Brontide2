
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;



public class Loadable : MonoBehaviour
{
	
	public SpriteRenderer spriteRenderer;
	public Image image;
	public Sprite sprite;
	public AssetReferenceSprite spriteRef;

	private void OnValidate()
	{
		if(!image) image = GetComponent<Image>();
		if(!spriteRenderer) spriteRenderer =GetComponent<SpriteRenderer>();
	}

	private void Awake()
	{
		if (!image) image = GetComponent<Image>();
		if (!spriteRenderer) spriteRenderer = GetComponent<SpriteRenderer>();

		LoadSprite();
	}


	private void OnEnable()
	{
		LoadSprite();
	}

	void LoadSprite()
	{
		if (sprite)
		{
			image.sprite = sprite;
		}
		else
		{
			spriteRef.LoadAssetAsync().Completed += SpriteLoaded;
		}
	}


	private void SpriteLoaded(AsyncOperationHandle<Sprite> obj)
	{
		if (obj.Status == AsyncOperationStatus.Succeeded)
		{
			sprite = obj.Result;
			if (image) image.sprite = sprite;
			if(spriteRenderer) spriteRenderer.sprite= sprite;
		}
	}

}
