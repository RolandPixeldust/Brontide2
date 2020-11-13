using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class JSTesting : MonoBehaviour
{

	[DllImport("__Internal")]
	private static extern void ScrollWindow(int x, int y);

	[DllImport("__Internal")]
	private static extern void ShowMessage(string message);


	public void JSScroll()
	{
		ScrollWindow(0,100);		
	}

	public void JSMessage()
	{
		string message ="MESSAGE TEST";
		ShowMessage(message);	
	}
}
