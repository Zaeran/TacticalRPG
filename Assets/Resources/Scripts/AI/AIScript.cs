using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIScript : MonoBehaviour {
	
	GameObject[] enemyList;
	
	PathfindingScript pathFind;
	MovementScript Move;
	PlayerDrawScript Draw;
	FindValidPoints findValid;
	AttributesScript Stats;
	
	List<Vector4> validPoints = new List<Vector4>();
	List<Vector4> allPoints = new List<Vector4>();
	
	Vector3[] movePath = new Vector3[0];
	int playerSelected;
	int xDistance;
	int zDistance;
	
	
	// Use this for initialization
	void Start () {
		enemyList = GameObject.FindGameObjectsWithTag("Player");
		Stats = GetComponent<AttributesScript>();
		pathFind = GetComponent<PathfindingScript>();
		Draw = GetComponent<PlayerDrawScript>();
		Move = GetComponent<MovementScript>();
		findValid = GetComponent<FindValidPoints>();
		playerSelected = Random.Range(0, enemyList.Length);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			StartCoroutine(AIMove());		
		}
	}
	
	IEnumerator AIMove(){
		validPoints = findValid.GetPoints(1, Stats.maxMove, Stats.maxJump);
		allPoints = findValid.GetPoints(1, 30, 1);
		int distanceToPlayer = 100;
		int heightToPlayer;
		Vector4 endPoint = Vector4.zero;
		//if not next to player, select point closest to a player
		xDistance = Mathf.Abs((int)(transform.position.x - enemyList[playerSelected].transform.position.x));
		zDistance = Mathf.Abs((int)(transform.position.z - enemyList[playerSelected].transform.position.z));
		heightToPlayer = Mathf.Abs((int)(transform.position.y - enemyList[playerSelected].transform.position.y));
		if(xDistance + zDistance < 1.1f && heightToPlayer < Stats.maxJump){
			//do nothing
		}
		else{
			foreach(Vector4 pnt in allPoints){
				xDistance = Mathf.Abs((int)(pnt.x - enemyList[playerSelected].transform.position.x));
				zDistance = Mathf.Abs((int)(pnt.z - enemyList[playerSelected].transform.position.z));
				heightToPlayer = Mathf.Abs((int)(pnt.y - enemyList[playerSelected].transform.position.y));
				if((xDistance + zDistance) < distanceToPlayer && heightToPlayer <= Stats.maxJump + 0.1f){
					endPoint = pnt;
					distanceToPlayer = xDistance + zDistance;
				}
			}
			Debug.Log(endPoint);
			Debug.Log(distanceToPlayer);
			Draw.DrawValidSquares(validPoints);
			yield return new WaitForSeconds(1);
			Draw.DestroyValidSquares();
			movePath = pathFind.StartPathFinding(endPoint, allPoints, Stats.maxJump);
			Move.MoveToPoint(movePath, Stats.maxMove);
		}
	}
}
