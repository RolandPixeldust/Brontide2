using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Cloud : MonoBehaviour
{

    public SpriteRenderer sprite;
    public Color col;
    public Transform start;
    public Transform end;
    public float distAway;
    public float thresh=.5f;
    public float speed;
    public float fadeThresh=3;
    public Vector2 speedRange =new Vector2(.2f,.5f);
   public Vector3 startPos;

	private void Awake()
	{
		startPos=transform.position;
        sprite=GetComponent<SpriteRenderer>();
        col=Color.white;
        col.a=.8f;
        thresh=2;
        fadeThresh=3;
        speedRange = new Vector2(.1f, .25f);
        speed =Random.Range(speedRange.x,speedRange.y);
	}

	private void OnEnable()
	{
		Restart();
	}

	void Restart()
    {
        transform.position = startPos;
        col.a =.8f;
        speed = Random.Range(speedRange.x, speedRange.y);
    }
    void Update()
    {
        transform.position += transform.right*Time.deltaTime*speed;
        distAway=Vector3.Distance(end.position,transform.position);

        if(distAway>fadeThresh)
        {
            col.a += Time.deltaTime;

        }
        else
        {
            col.a-=Time.deltaTime;
        }
        col.a = Mathf.Clamp(col.a,0,.6f);
        sprite.color = col;

        if (distAway<thresh)
        {
            transform.position = new Vector3(start.position.x, startPos.y, startPos.z);
            col.a=0;
        }
        
     
    }
}
