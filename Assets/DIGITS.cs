using UnityEngine;
using UnityEngine.UI;
using System;

public class DIGITS : MonoBehaviour
{
    
	public string source;
	public char [] chars;
	public Text [] digits;
	
	public float spacing = 14.48f;
	
	public float totalSpace;
	public float letterSpace;
	public float letterStartSpace;
	public float commaSpaceFactorBefore=.5f;
	public float  commaSpaceFactorAfter =.75f;
	public float iconOffset= -5;
	public float iconHeight =-10;
	public int numberSize;
	public int letterSize;

	public float letterY;
	public Transform icon;
	public float m_xOffset;

	private void Start()
	{
		spacing = 14.5f;
		commaSpaceFactorBefore = .85f;
		commaSpaceFactorAfter = .5f;
		numberSize=28;
		letterSize=12;

		letterStartSpace=16;
		letterSpace = 8.5f;
		letterY = -16.88f;

		
	}

void Update()
{
		chars = source.ToCharArray();
		totalSpace=0;

		bool isPrevComma=false;
		bool isPrevLetter=false;


		for (int i = 0; i < digits.Length; i++)
		{	
			if (chars.Length<=i)
			{
				digits[i].enabled=false;
				isPrevLetter = false;
			}
			else
			{
				digits[i].enabled = true;
				digits[i].text = chars[i].ToString();
				digits[i].fontSize = numberSize;

				if (isPrevComma)
				{
					totalSpace+= spacing * commaSpaceFactorAfter;
					isPrevLetter = false;
				}
				else if (chars[i].ToString() == ",")
				{
					totalSpace += spacing * commaSpaceFactorBefore;
					isPrevLetter = false;
				}
				else if (Char.IsLetter(chars[i]))
				{
					digits[i].fontSize = letterSize;

					if(isPrevLetter)
						totalSpace += letterSpace;
					else
						totalSpace += letterStartSpace;
					
					isPrevLetter = true;
				}
				else
				{
					totalSpace += spacing;
					isPrevLetter = false;
				}

				isPrevComma = chars[i].ToString() == "," ? true:false;
			}

			digits[i].transform.localPosition = new Vector3(totalSpace, isPrevLetter? letterY:0, 0);
		}

		if(icon)
			icon.localPosition =new Vector3(iconOffset, iconHeight,0);
		transform.localPosition= new Vector3(-(totalSpace/2)+m_xOffset,0,0);
	}
}
