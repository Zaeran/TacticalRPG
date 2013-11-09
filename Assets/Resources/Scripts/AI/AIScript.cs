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
	TurnController Controller;
	
	List<Vector4> validPoints = new List<Vector4>();
	List<Vector4> allPoints = new List<Vector4>();
	
	Vector3[] movePath = new Vector3[0];
	Vector3 startPosition = Vector3.zero;
	int playerSelected;
	int xDistance;
	int zDistance;
	int distanceToPlayer;
	int distanceTravelled = 0;
	
	bool isMyTurn = false;
	bool actionOccuring = false;
	int remainingAP = 0;
	
	
	// Use this for initialization
	void Awake () {
		enemyList = GameObject.FindGameObjectsWithTag("Player");
		Stats = GetComponent<AttributesScript>();
		pathFind = GetComponent<PathfindingScript>();
		Draw = GetComponent<PlayerDrawScript>();
		Move = GetComponent<MovementScript>();
		findValid = GetComponent<FindValidPoints>();
		Controller = GameObject.FindGameObjectWithTag("Controller").GetComponent<TurnController>();
		playerSelected = Random.Range(0, enemyList.Length);
	}
	
	public void NextTurn(){
		isMyTurn = true;
		remainingAP = Stats.maxActions;
	}
	
	public void TakeDamage(int damage){
		if(Stats.Damage(damage)){
			gameObject.tag = "NPCDead";
			Controller.DeadCharacter(gameObject);
			Destroy(gameObject);
		}
	}
	
	private void EndTurn(){
		isMyTurn = false;
		Controller.TurnOver();
	}
	
	// Update is called once per frame
	void Update () {
		if(isMyTurn){
			if(!actionOccuring){
				TakeTurn();
			}		
		}
	}
	
	private void TakeTurn(){
		Debug.Log(remainingAP);
		if(remainingAP == 0){
			EndTurn();
		}
		else if(GetDistanceToPlayer(transform.position) < 1.1f && remainingAP >= 3){
			AIMeleeAttack();
		}
		else if(GetDistanceToPlayer(transform.position) < 1.1f){
			EndTurn();
		}
		else{
			StartCoroutine(AIMove());	
		}	
	}
	
	IEnumerator AIMove(){
		//set initial position
		startPosition = transform.position;
		actionOccuring = true;
		//find valid points
		validPoints = findValid.GetPoints(1, remainingAP, Stats.maxJump);
		allPoints = findValid.GetPoints(1, 50, 1);
		int distanceToPlayer = 100;
		int heightToPlayer;
		Vector4 endPoint = Vector4.zero;
		//find distance to closest player from current position
		xDistance = Mathf.Abs((int)(transform.position.x - enemyList[playerSelected].transform.position.x));
		zDistance = Mathf.Abs((int)(transform.position.z - enemyList[playerSelected].transform.position.z));
		heightToPlayer = Mathf.Abs((int)(transform.position.y - enemyList[playerSelected].transform.position.y));
		
		if(xDistance + zDistance < 1.1f && heightToPlayer < Stats.maxJump){//next to player
			//do nothing
		}
		else{
			int tempDistanceToPlayer = 100;
			foreach(Vector4 pnt in allPoints){
				distanceToPlayer = GetDistanceToPlayer(pnt);
				heightToPlayer = Mathf.Abs((int)(pnt.y - enemyList[playerSelected].transform.position.y));
				if(distanceToPlayer < tempDistanceToPlayer && heightToPlayer <= Stats.maxJump + 0.1f && distanceToPlayer > 0.5f){
					endPoint = pnt;
					tempDistanceToPlayer = distanceToPlayer;
				}
			}
			Draw.DrawValidSquares(validPoints);
			yield return new WaitForSeconds(1);
			Draw.DestroyValidSquares();
			movePath = pathFind.StartPathFinding(endPoint, allPoints, Stats.maxJump);
			Move.MoveToPoint(movePath, remainingAP);
		}
	}
	
	public void StopMovingConfirmation(){
		actionOccuring = false;
		distanceTravelled = Mathf.Abs((int)(startPosition.x - transform.position.x)) + Mathf.Abs((int)(startPosition.z - transform.position.z));
		remainingAP -= distanceTravelled;
		Debug.Log(remainingAP);
	}
	
	public void BattleOver(){
		Debug.Log("END BATTLE");
		remainingAP = 0;
		isMyTurn = false;
	}
	
	
	private void AIMeleeAttack(){
		actionOccuring = true;
		validPoints = findValid.GetPoints(2, 1, Stats.maxJump);
		Draw.DrawValidSquares(validPoints);
		StartCoroutine(MeleeAttack(enemyList[playerSelected]));		
	}
	
	IEnumerator MeleeAttack(GameObject target){
		yield return new WaitForSeconds(1); //replace with animation
		target.SendMessage("TakeDamage", 1);
		Draw.DestroyValidSquares();
		remainingAP -= 3;
		actionOccuring = false;
	}
	
	private int GetDistanceToPlayer(Vector4 Point){
		xDistance = Mathf.Abs((int)(Point.x - enemyList[playerSelected].transform.position.x));
		zDistance = Mathf.Abs((int)(Point.z - enemyList[playerSelected].transform.position.z));
		return xDistance + zDistance;
	}
	private int GetDistanceToPlayer(Vector3 Point){
		xDistance = Mathf.Abs((int)(Point.x - enemyList[playerSelected].transform.position.x));
		zDistance = Mathf.Abs((int)(Point.z - enemyList[playerSelected].transform.position.z));
		return xDistance + zDistance;
	}
}
