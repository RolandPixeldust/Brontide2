using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using System.Linq;
using System.Globalization;
using System.Runtime.InteropServices;


public class SliderControl : MonoBehaviour
{
	/*
	[DllImport("__Internal")]
	private static extern void ScrollWindow(int x, int y);

	[DllImport("__Internal")]
	private static extern void ShowMessage(string message);
*/

//#if UNITY_WEBGL && !UNITY_EDITOR

		[DllImport("__Internal")]
		private static extern void BrontideHome();

	[DllImport("__Internal")]
	private static extern void EPA();

	[DllImport("__Internal")]
	private static extern void PDF();
	//#endif



	public static SliderControl master;

	public enum State {Intro, Main};
	[EnumToggleButtons] public State state;

	[FoldoutGroup("Calculator")] public float greenHouseGas;
	[FoldoutGroup("Calculator")] public Text greenHouseGasText;
	[FoldoutGroup("Calculator")] public float emission;
	[FoldoutGroup("Calculator")] public Text emissionsText;
	[FoldoutGroup("Calculator")] public float lamps;
	[FoldoutGroup("Calculator")] public Text lampsText;
	[FoldoutGroup("Calculator")] public float carbonSequestered;
	[FoldoutGroup("Calculator")] public Text carbonSequesteredText;
	[FoldoutGroup("Calculator")] public NumberObj [] numberObjs;
	[FoldoutGroup("Calculator")] public AnimationCurve LogCurve;
	[FoldoutGroup("Calculator")] public AnimationCurve InvertLogCurve;

	[System.Serializable]
	public struct NumberObj
	{
		public Vector3 restScale;
		public Transform transformNull;
	}

	[System.Serializable]
	public struct ChartObj
	{
		public Vector3 restScale;
		public CanvasGroup canvasGroup;
		public Transform transformNull;
	}

	[FoldoutGroup("ToolTip")] public GameObject [] toolTip;
	
	[FoldoutGroup("Pools")] public GameObject dash;
	[FoldoutGroup("Pools")] public float dashStagger=.25f;
	[FoldoutGroup("Pools")] public float delay;
	[FoldoutGroup("Pools")] public Vector3 dashOffset;
	[FoldoutGroup("Pools")] public float dashDur =2;
	[FoldoutGroup("Pools")] public int dashPoolSize;
	[FoldoutGroup("Pools")] public Renderer [] dashes;

	[FoldoutGroup("FlowChart")] public bool calculatorActive;
	[FoldoutGroup("FlowChart")] public ChartObj [] chartObjs;
	[FoldoutGroup("FlowChart")] public AnimationCurve chartObjPopCurve;
	[FoldoutGroup("FlowChart")] public float flowDur=.5f;
	[FoldoutGroup("FlowChart")] public float flowStagger=.1f;
	[FoldoutGroup("FlowChart")] public float truckDelay=1;
	[FoldoutGroup("CameraTruck")] public Transform cameraStart;
	[FoldoutGroup("CameraTruck")] public Transform cameraEnd;
	[FoldoutGroup("CameraTruck")] public float cameraTruckDur =2;
	[FoldoutGroup("CameraTruck")] public AnimationCurve cameraTruckLerp;

	[FoldoutGroup("Sun")] public Transform sunStart;
	[FoldoutGroup("Sun")] public Transform sunEnd;
	[FoldoutGroup("Sun")]  public Transform sun;
	[FoldoutGroup("Sun")] public AnimationCurve sunRiseCurve;
	[FoldoutGroup("Sun")] public float sunLerpDur;
	[FoldoutGroup("Sun")] public SpriteRenderer sky;
	[FoldoutGroup("Sun")] public Transform skyScaleNull;
	[FoldoutGroup("Sun")] public Vector2 skyScaleRange = new Vector2 (.25f,1);
	[FoldoutGroup("Sun")] public Color skyCol;
	

	[FoldoutGroup("Slider")] public Slider impactSlider;
	[FoldoutGroup("Slider")] public Image leafIcon;
	float lastSliderValue;
	[FoldoutGroup("Slider")] public InputField inputField;
	string lastInputFieldValue;

	const float MAX_MT = 2000;

	[FoldoutGroup("Slider")] public float impact;
	[FoldoutGroup("Slider")] public GameObject startHere;

	[FoldoutGroup("Parallax")] public float cameraRangeX=2;
	[FoldoutGroup("Parallax")] public float cameraRangeY=2;

	[Space]
	[FoldoutGroup("Parallax")] public float cameraX;
	[FoldoutGroup("Parallax")] public float cameraY;
	[FoldoutGroup("Parallax")] public float cameraZ=75;
	[Space]
	[FoldoutGroup("Parallax")] public float mouseX;
	[FoldoutGroup("Parallax")] public float mouseY;

	[FoldoutGroup("Parallax")] public Transform camera;
	[FoldoutGroup("Parallax")] public float cameraSmoothTime=.25f;
	Vector3 smoothDampRef;


	 public enum ObjState {Hidden, Dissolving, Dissolved };
	 public enum ObjType {Static, Bear, Sun, Bee };

	[System.Serializable]
	public struct DissolveObj
	{
		[FoldoutGroup("$name")] public string name;
		[FoldoutGroup("$name")] public ObjState objState;
		[FoldoutGroup("$name")] public GameObject obj;
		[FoldoutGroup("$name")] public Renderer rend;
		[FoldoutGroup("$name")] [Range(0, MAX_MT)] public float dissoveThresh;
		[FoldoutGroup("$name")] public Vector3 startScale;
		[FoldoutGroup("$name")] public ObjType objType;
	}

	[FoldoutGroup("Objects")] public DissolveObj []  dissolveObjs;
	[FoldoutGroup("Objects")] public AnimationCurve popInCurve;
	[FoldoutGroup("Objects")] public float popInDur=1;
	[FoldoutGroup("Objects")] public AnimationCurve popOutCurve;
	[FoldoutGroup("Objects")] public float popOutDur = 1;
	[FoldoutGroup("Objects")] public string	fadeTag= "_Intensity";
	
	/*
	[Button(ButtonSizes.Large)]
	void ClearSprites()
	{
		var loadables = FindObjectsOfType<Loadable>();
		foreach (var item in loadables)
		{
			if (item.image!=null) item.image.sprite=null;
			if(item.spriteRenderer!=null) item.spriteRenderer.sprite=null;
		}
	}

	[Button(ButtonSizes.Large)]
	void LoadSprites()
	{
		var loadables = FindObjectsOfType<Loadable>();
		foreach (var item in loadables)
		{
			item.LoadSprite();
		}
	}
	*/
	void Awake()
    {
		//Screen.SetResolution(
		//Screen.currentResolution.width,
		//Screen.currentResolution.height,false);
		//WebGLInput.captureAllKeyboardInput = false;
		if (SliderControl.master==null)
		SliderControl.master=this;
		else
		Destroy(this.gameObject);

		cameraZ = 75;
		camera = Camera.main.transform;
		inputField.text="0";
		TruckCamera();

		foreach (var item in chartObjs)
			item.transformNull.gameObject.SetActive(false);

		lastInputFieldValue = inputField.text;
		lastSliderValue = impactSlider.value;

		

	}

	[Button]
	void InvertTheLogCurve()
	{
		for (int i = 0; i < InvertLogCurve.length; i++)
		{
			InvertLogCurve.RemoveKey(i);
		}
		AnimationCurve c= new AnimationCurve();

		for (float i = 0; i < 32; i++)
		{
			c.AddKey((i/32),LogCurve.Evaluate(i/32) );
		}

		for (int i = 0; i < c.keys.Length; i++)
		{
			Keyframe inverseKey = new Keyframe(c.keys[i].value, c.keys[i].time);
			InvertLogCurve.AddKey(inverseKey);
		}
	}

	private void Start()
	{
		dashes = new Renderer[dashPoolSize];
		for (int i = 0; i < dashes.Length; i++)
		{
			dashes[i]= Instantiate(dash,transform.position,transform.rotation).GetComponentInChildren<Renderer>();
			dashes[i].transform.parent.gameObject.SetActive(false);
		}
		StartCoroutine(Counters());
	}


	[FoldoutGroup("CameraTruck")]
	[Button(ButtonSizes.Large)]
	void TruckCamera()
	{
		StartCoroutine(_TruckCamera());
	}

	public void SpawnDash(Transform t)
	{
		for (int i = 0; i < dashes.Length; i++)
		{
			if(!dashes[i].transform.parent.gameObject.activeInHierarchy)
			{
				dashes[i].transform.parent.position = t.transform.position;
				dashes[i].transform.parent.rotation = t.transform.rotation;
				dashes[i].transform.parent.gameObject.SetActive(true);
				dashes[i].enabled = false;
				StartCoroutine(FadeDash(dashes[i],t));
				break;
			}
		}
	}

	IEnumerator FadeDash(Renderer d, Transform source)
	{
		yield return new WaitForSeconds(delay);
		d.enabled=true;
		for (float t = 0; t < 1; t+=Time.deltaTime/ dashDur)
		{
			d.transform.parent.localScale = Vector3.one * (1-t)*1.25f;
			yield return null;
		}
		d.transform.parent.gameObject.SetActive(false);
	}

	IEnumerator _TruckCamera()
	{
		state = State.Intro;

		HideObjects();
		sun.position = sunStart.position;
		camera.position = cameraStart.position;
		yield return new WaitForSeconds(truckDelay);
		for (float lerp = 0; lerp < 1; lerp+=Time.deltaTime/cameraTruckDur)
		{
			camera.position = Vector3.Lerp(cameraStart.position,cameraEnd.position, cameraTruckLerp.Evaluate(lerp));
			yield return null;
		}
		/*
			string message = "TEST COMPLETE";
			#if UNITY_WEBGL && !UNITY_EDITOR
					ScrollWindow(0,400);
					ShowMessage(message);
			#endif
		*/
		camera.position = cameraEnd.position;
		state = State.Main;
	}
		



	[FoldoutGroup("FlowChart")]
	[Button(ButtonSizes.Large)]
	void FlowChart()
	{
		calculatorActive=true;
		foreach (var item in chartObjs)
			item.transformNull.gameObject.SetActive(false);
		for (int i = 0; i < chartObjs.Length; i++)
		{
			StartCoroutine(_FlowChart(i));
		}	
	}

	IEnumerator _FlowChart(int index)
	{
		yield return new WaitForSeconds((float)index * flowStagger);
		chartObjs[index].transformNull.gameObject.SetActive(true);
		for (float lerp = 0; lerp < 1; lerp += Time.deltaTime / flowDur)
		{
			chartObjs[index].canvasGroup.alpha =lerp;
			chartObjs[index].transformNull.localScale = chartObjs[index].restScale * chartObjPopCurve.Evaluate(lerp);
			yield return null;
		}
		chartObjs[index].canvasGroup.alpha =1;
		chartObjs[index].transformNull.localScale = chartObjs[index].restScale;
		if(!calculatorActive) chartObjs[index].transformNull.gameObject.SetActive(false);
	}
 
    void Update()
    {
		UpdateSky();
		if (state == State.Intro) return;
		CameraParallax();
		ImpactInput();
	}

	[FoldoutGroup("FlowChart")]
	[Button(ButtonSizes.Large)]
	private void ValidateFlowChart()
	{
		for (int i = 0; i < chartObjs.Length; i++)
		{
			chartObjs[i].restScale = chartObjs[i].transformNull.localScale;
			if(chartObjs[i].canvasGroup==null)
			chartObjs[i].canvasGroup = chartObjs[i].transformNull.gameObject.AddComponent<CanvasGroup>();
		}
	}

	[FoldoutGroup("Calculator")]
	[Button(ButtonSizes.Large)]
	private void ValidateNumbers()
	{
		for (int i = 0; i < numberObjs.Length; i++)
		{
			numberObjs[i].restScale = numberObjs[i].transformNull.localScale;
			
		}
	}

	[FoldoutGroup("Objects")][Button(ButtonSizes.Large)]
	private void ValidateObjects()
	{
		for (int i = 0; i < dissolveObjs.Length; i++)
		{
			dissolveObjs[i].name = dissolveObjs[i].obj.name;
			dissolveObjs[i].rend = dissolveObjs[i].obj.GetComponentInChildren<Renderer>();
			dissolveObjs[i].startScale = dissolveObjs[i].obj.transform.localScale;
		}

		dissolveObjs = dissolveObjs.OrderBy(x => x.dissoveThresh).ToArray();
	}

	public void BrontideHomeJScript()
	{
		BrontideHome();
	}

	public void EPAJScript()
	{
		EPA();
	}

	public void PDFJScript()
	{
		PDF();
	}

	public void GoToSite()
	{
		#if UNITY_WEBGL
			Application.ExternalEval("window.open('https://www.epa.gov/energy/greenhouse-gas-equivalencies-calculator');");
		#endif
	}

	public void GoToPDF()
	{
		#if UNITY_WEBGL
			Application.ExternalEval("window.open('https://www.brontidebg.com/wp-content/uploads/2020/09/SOFW-LCA-Article.pdf');");
		#endif
	}

	public void ToolTip(int index)
	{
		toolTip[index].SetActive(true);
	}

	IEnumerator PopToolTip(int index)
	{
		for (float lerp = 0; lerp < 1; lerp += Time.deltaTime / popInDur)
		{
			dissolveObjs[index].obj.transform.localScale = dissolveObjs[index].startScale * popInCurve.Evaluate(lerp);
			yield return null;
		}
	}


	[FoldoutGroup("Objects")]
	[Button(ButtonSizes.Large)]
	private void EvenObjectThreshes()
	{
		for (int i = 0; i < dissolveObjs.Length; i++)
		{
			dissolveObjs[i].dissoveThresh = (MAX_MT/dissolveObjs.Length) *i;
		}
	}

	void TurnOffCalculator()
	{
		calculatorActive=false;
		if(!startHere.activeInHierarchy) StartCoroutine(PopObj(startHere.transform,.385f,.5f));
		startHere.SetActive(true);

		foreach (var item in chartObjs)
			item.transformNull.gameObject.SetActive(false);
	}

	IEnumerator PopObj(Transform t, float initScale, float dur)
	{
		
		for (float lerp = 0; lerp < 1; lerp+=Time.deltaTime/dur)
		{
			t.localScale = Vector3.one*initScale * popInCurve.Evaluate(lerp);
			yield return null;
		}
		t.localScale = Vector3.one* initScale;
	}

	void ShowObjects()
	{
		for (int i = 0; i < dissolveObjs.Length; i++)
		{
			dissolveObjs[i].rend.material.SetFloat(fadeTag,1);
		}
	}


	void HideObjects()
	{
		for (int i = 0; i < dissolveObjs.Length; i++)
		{
			dissolveObjs[i].rend.material.SetFloat(fadeTag, 0);
		}
	}

	void UpdateDissolveObjects()
	{
		for (int i = 0; i < dissolveObjs.Length; i++)
		{
			if(dissolveObjs[i].dissoveThresh < impact)
			{
				if(dissolveObjs[i].objState == ObjState.Hidden)
					StartCoroutine(DissolveIn(i));
			}
			else
			{
				if (dissolveObjs[i].objState == ObjState.Dissolved)
					StartCoroutine(DissolveOut(i));
			}
		}
	}

	IEnumerator DissolveIn(int index)
	{
		dissolveObjs[index].objState = ObjState.Dissolving;
		if(dissolveObjs[index].objType == ObjType.Bear)
		{
			var bear = dissolveObjs[index].obj.GetComponent<Bear>();
			if(bear) bear.Restart();
		}

		if (dissolveObjs[index].objType == ObjType.Bee)
		{
			dissolveObjs[index].obj.transform.GetChild(0).gameObject.SetActive(true);
		}

		if (dissolveObjs[index].objType == ObjType.Sun)
		{
			StartCoroutine(SunRise(index));
			yield break;
		}

		for (float	lerp = 0; lerp < 1;lerp+=Time.deltaTime/popInDur )
		{
			dissolveObjs[index].obj.transform.localScale = dissolveObjs[index].startScale * popInCurve.Evaluate(lerp);
			dissolveObjs[index].rend.material.SetFloat(fadeTag, lerp);

			yield return null;
		}

		dissolveObjs[index].objState = ObjState.Dissolved;
		UpdateDissolveObjects();
	}

	IEnumerator SunRise(int index)
	{
		Color col = sky.color;
		dissolveObjs[index].rend.material.SetFloat(fadeTag, 1);
		for (float lerp = 0; lerp <1; lerp+=Time.deltaTime/sunLerpDur)
		{
			dissolveObjs[index].obj.transform.position = Vector3.Lerp(sunStart.position,sunEnd.position,sunRiseCurve.Evaluate(lerp));
		
			yield return null;
		}
		dissolveObjs[index].objState = ObjState.Dissolved;
	}

	void UpdateSky()
	{
		var a = Mathf.Clamp01((impactSlider.value - skyScaleRange.x) / (skyScaleRange.y -skyScaleRange.x));
		skyCol.a= a;
		sky.material.SetFloat(fadeTag, a);
		sky.color = skyCol;
		skyScaleNull.localScale = Vector3.one*(.5f+a/2);
	}

	IEnumerator DissolveOut(int index)
	{
		dissolveObjs[index].objState = ObjState.Dissolving;
		for (float lerp = 0; lerp < 1; lerp += Time.deltaTime / popInDur)
		{
			if(dissolveObjs[index].objType != ObjType.Sun)
			dissolveObjs[index].obj.transform.localScale = dissolveObjs[index].startScale * popOutCurve.Evaluate(lerp);
			dissolveObjs[index].rend.material.SetFloat(fadeTag, 1-lerp);
			yield return null;
		}

		dissolveObjs[index].rend.material.SetFloat(fadeTag, 0);
		if (dissolveObjs[index].objType == ObjType.Bee)
		{
			dissolveObjs[index].obj.transform.GetChild(0).gameObject.SetActive(false);
		}
		dissolveObjs[index].objState = ObjState.Hidden;
		UpdateDissolveObjects();
	}


	void ImpactInput()
	{
		if(lastSliderValue != impactSlider.value)
			UpdateInputField();

		if(InputFieldObj.master)
		{
		if(lastInputFieldValue != inputField.text && InputFieldObj.master.selected && inputField.text!="")
			UpdateSlider();
		}

		lastInputFieldValue = inputField.text;
		lastSliderValue = impactSlider.value;
	}

	void UpdateSlider()
	{
	
		impact = float.Parse(inputField.text);
		impact = Mathf.Round(Mathf.Clamp(impact, 0, MAX_MT));
		impactSlider.value = InvertLogCurve.Evaluate(impact / MAX_MT);
		inputField.text = impact.ToString();
		Recalc();
		startHere.SetActive(false);
		UpdateDissolveObjects();
		//UpdateLeafIcon();
		
		if(impact<=0) 
		{
			TurnOffCalculator();
		}
		else if (!calculatorActive)
		{
			FlowChart();
		}
	}

	void UpdateInputField()
	{
		
		impact = Mathf.Round(LogCurve.Evaluate(impactSlider.value) * MAX_MT);
		Recalc();
		startHere.SetActive(false);
		inputField.text = impact.ToString();
		UpdateDissolveObjects();
		//UpdateLeafIcon();
		if (impact <= 0)
		{
			TurnOffCalculator();
		}
		else if (!calculatorActive) 
		{
			FlowChart();
		}
	}

	void Recalc()
	{
		greenHouseGas = Mathf.Round(impactSlider.value * MAX_MT * 1.92f);
		carbonSequestered = Mathf.Round(impactSlider.value * MAX_MT * 1.92f * 16.534f);
		emission = Mathf.Round(impactSlider.value * MAX_MT * 1.92f * .21599f);
		lamps = Mathf.Round(impactSlider.value * MAX_MT * 1.92f * 37.9899126f);
	}

	IEnumerator Counters()
	{
		float curGreenHouseGas=0;
		float curCarbonSequestered=0;
		float curEmission=0;
		float curLamps=0;

		while (true)
		{

			if (chartObjs[1].transformNull.gameObject.activeInHierarchy)
			{
				if (curGreenHouseGas < greenHouseGas)
				{
					curGreenHouseGas = Mathf.Clamp(curGreenHouseGas + Random.Range(5, 50), 0, greenHouseGas);
					if(curGreenHouseGas ==  greenHouseGas) StartCoroutine(PopNumber(0));
				}
			
				if (curGreenHouseGas > greenHouseGas)
				{
					curGreenHouseGas = Mathf.Clamp(curGreenHouseGas - Random.Range(50, 50), greenHouseGas, float.MaxValue);
				}
			}

			if (chartObjs[5].transformNull.gameObject.activeInHierarchy)
			{
				if (curEmission < emission)
				{
					curEmission = Mathf.Clamp(curEmission + Random.Range(0, 10), 0, emission);
					if (curEmission == emission) StartCoroutine(PopNumber(1));
				}

				if (curEmission > emission)
				curEmission = Mathf.Clamp(curEmission - Random.Range(0, 10), emission, float.MaxValue);
			}

			if (chartObjs[6].transformNull.gameObject.activeInHierarchy)
			{
				if (curLamps < lamps)
				{
					curLamps = Mathf.Clamp(curLamps + Random.Range(100, 1000), 0, lamps);
					if (curLamps == lamps) StartCoroutine(PopNumber(2));
				}

				if (curLamps > lamps) 
				curLamps = Mathf.Clamp(curLamps - Random.Range(100, 1000), lamps, float.MaxValue);
			}

			if(chartObjs[7].transformNull.gameObject.activeInHierarchy)
			{
				if (curCarbonSequestered < carbonSequestered)
				{
					curCarbonSequestered = Mathf.Clamp(curCarbonSequestered + Random.Range(50, 500), 0, carbonSequestered);
					if (curCarbonSequestered == carbonSequestered) StartCoroutine(PopNumber(3));
				}

				if (curCarbonSequestered > carbonSequestered)
					curCarbonSequestered = Mathf.Clamp(curCarbonSequestered - Random.Range(50, 500), carbonSequestered, float.MaxValue);
			}


			greenHouseGasText.text = curGreenHouseGas.ToString("#,#", CultureInfo.InvariantCulture);
			carbonSequesteredText.text = curCarbonSequestered.ToString("#,#", CultureInfo.InvariantCulture);
			emissionsText.text = curEmission.ToString("#,#", CultureInfo.InvariantCulture);
			lampsText.text = curLamps.ToString("#,#", CultureInfo.InvariantCulture);

			yield return null;
		}
	}

	

	IEnumerator PopNumber(int index)
	{
		for (float lerp = 0; lerp < 1; lerp += Time.deltaTime / .25f)
		{
			numberObjs[index].transformNull.localScale = numberObjs[index].restScale * chartObjPopCurve.Evaluate(lerp);
			yield return null;
		}
		numberObjs[index].transformNull.localScale = numberObjs[index].restScale;
	}

	/*
	void UpdateLeafIcon()
	{
		var col = leafIcon.color;
		col.a = impactSlider.value;
		leafIcon.color=col;
	}
	*/

	void CameraParallax()
	{
		mouseX = Mathf.Clamp01(Input.mousePosition.x / Screen.width);
		mouseY = Mathf.Clamp01(Input.mousePosition.y / Screen.height);

		//x normalized = (x – x minimum) / (x maximum – x minimum)
		cameraX = ((mouseX - .5f) * 2) * cameraRangeX;
		cameraY = ((mouseY - .5f) * 2) * cameraRangeY;

		camera.position = Vector3.SmoothDamp(camera.position, new Vector3(cameraX, cameraY, cameraZ), ref smoothDampRef, cameraSmoothTime);


	}
}
