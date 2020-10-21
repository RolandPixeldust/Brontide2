using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Bee : MonoBehaviour
{
   
	[EnumToggleButtons] public enum State{Fly,Hold,Disabled};
	public State state;

	
	public WayPoint [] wayPoints;

	[System.Serializable]
	public struct WayPoint
	{
		public Transform pointNull;
		public int [] branchablePoints;
	}


	public Vector2 holdRange = new Vector2(2,4);
	public float maxMoveSpeed=1;
	public float moveSpeed=1;
	public float turnSpeed=1;
	public float distAway=0;
	public int waypointIndex=0;
	public float waypointThresh=1;
	public Transform child;
	public Transform camera;
	public float childOffset =180;

	public bool lockX;
	public bool lockY;
	public bool lockZ;
	public Renderer rend;
	Vector3 moveRef;

    void Start()
    {
		child = transform.GetChild(0);
		rend = GetComponentInChildren<Renderer>();
		rend.gameObject.SetActive(false);
		waypointIndex =  wayPoints[waypointIndex].branchablePoints[Random.Range(0, wayPoints[waypointIndex].branchablePoints.Length)];
		camera = Camera.main.transform;
		state = State.Fly;
		StartCoroutine(Fly());   
		StartCoroutine(Dashes());
	}

   IEnumerator Fly()
	{
		while(true)
		{
			switch(state)
			{
				case State.Disabled:
				break;

				case State.Fly:
				var moveDir = Quaternion.LookRotation(wayPoints[waypointIndex].pointNull.position - transform.position);
				transform.rotation = Quaternion.Slerp(transform.rotation, moveDir, Time.deltaTime*turnSpeed);
				transform.position += transform.forward * Time.deltaTime * moveSpeed;
				distAway = Vector3.Distance(transform.position, wayPoints[waypointIndex].pointNull.position);
				var pos = transform.position + transform.forward;
				var dir =new Vector3(pos.x, pos.y, child.position.z) - child.position;
				var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
				child.rotation = Quaternion.AngleAxis(angle+childOffset, Vector3.forward);

				if (distAway<waypointThresh) state= State.Hold;
				break;

				case State.Hold:
				yield return new WaitForSeconds(Random.Range(holdRange.x, holdRange.y));
				waypointIndex = wayPoints[waypointIndex].branchablePoints[Random.Range(0, wayPoints[waypointIndex].branchablePoints.Length)];
				state = State.Fly;
				break;

			}
			yield return null;
		}
	}

	IEnumerator Dashes()
	{
		while(true)
		{
			if(state == State.Fly && rend.gameObject.activeInHierarchy)
			{
				SliderControl.master.SpawnDash(this.transform);
				yield return new WaitForSeconds(SliderControl.master.dashStagger);
			}
			yield return null;
		}
	}
	/*
	# if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		
		for (int i = 0; i < wayPoints.Length; i++)
		{

			for (int w = 0; w < wayPoints[i].branchablePoints.Length; w++)
			{
				if(wayPoints[i].branchablePoints.Length>0)
				{
					if (wayPoints[i].pointNull && wayPoints[wayPoints[i].branchablePoints[w]].pointNull)
					Debug.DrawLine(wayPoints[i].pointNull.position, wayPoints[wayPoints[i].branchablePoints[w]].pointNull.position, Color.blue);
				}
			}
		}
	}
	#endif
	*/
}
