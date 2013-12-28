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
	
	int atkType;
	
	bool isMyTurn = false;
	bool actionOccuring = false;
	bool isReacting = false;
	bool targetReactionOccuring = false;
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
		//atkType = Random.Range(1,3);
		atkType = 1;
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
		else if(atkType == 2 && GetDistanceToPlayer(transform.position) < 4.01f && GetDistanceToPlayer(transform.position) > 1.1f && remainingAP >= 3){
		    AIRangedAttack();	
		}
		else if(atkType == 1 && GetDistanceToPlayer(transform.position) < 1.1f && remainingAP >= 3){
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
			int rangeDistToPlayer = Random.Range(2,5);
			foreach(Vector4 pnt in allPoints){
				distanceToPlayer = GetDistanceToPlayer(pnt);
				heightToPlayer = Mathf.Abs((int)(pnt.y - enemyList[playerSelected].transform.position.y));
				if(atkType == 1){
					if(distanceToPlayer < tempDistanceToPlayer && heightToPlayer <= Stats.maxJump + 0.1f && distanceToPlayer > 0.5f){
						endPoint = pnt;
						tempDistanceToPlayer = distanceToPlayer;
					}
				}
				else if(atkType == 2){
					if(distanceToPlayer < rangeDistToPlayer && distanceToPlayer > 1.5f && heightToPlayer <= Stats.maxJump){
						endPoint = pnt;
						break;
					}
					else if(distanceToPlayer < tempDistanceToPlayer && heightToPlayer <= Stats.maxJump + 0.1f){
					    endPoint = pnt;
						tempDistanceToPlayer = distanceToPlayer;
					}
				}
			}
			Draw.DrawValidSquares(validPoints);
			yield return new WaitForSeconds(1);
			Draw.DestroyValidSquares();
			movePath = pathFind.StartPathFinding(endPoint, allPoints, Stats.maxJump);
			Move.MoveToPoint(movePath, remainingAP);
		}
	}
	//movement over. reset things and remove AP
	public void StopMovingConfirmation(int squaresMoved){
		actionOccuring = false;
		remainingAP -= squaresMoved;
		Debug.Log(remainingAP);
	}
	
		//an action has been performed. allow new action to occur and remove all marker squares
	private void ActionComplete(){
		actionOccuring = false;
		Draw.DestroyValidSquares();
	}
	
	public void BattleOver(){
		Debug.Log("END BATTLE");
		remainingAP = 0;
		isMyTurn = false;
	}
	
	//enable reaction
	public void Reaction(){
		
	}
	//enemy has finished reaction
	public void ContinueFromReaction(){
		if(atkType == 1){
			StartCoroutine(MeleeAttackEnd(enemyList[playerSelected]));
			targetReactionOccuring = false;
		}
	}
	
	
	private void AIMeleeAttack(){
		actionOccuring = true;
		validPoints = findValid.GetPoints(2, 2, Stats.maxJump);
		Draw.DrawValidSquares(validPoints);
		StartCoroutine(MeleeAttackStart(enemyList[playerSelected]));		
	}
	
	IEnumerator MeleeAttackStart(GameObject target){
		yield return new WaitForSeconds(0.5f); //replace with animation (halfway)
		target.SendMessage("Reaction", gameObject);
		Draw.DestroyValidSquares();
		targetReactionOccuring = true;
	}
	
	IEnumerator MeleeAttackEnd(GameObject target){
		yield return new WaitForSeconds(0.5f);
		target.SendMessage("TakeDamage", 1);
		remainingAP -= 3;
		ActionComplete();
	}
	
	private void AIRangedAttack(){
		actionOccuring = true;
		validPoints = findValid.GetPoints(4, 3, Stats.maxJump);
		Draw.DrawValidSquares(validPoints);
		StartCoroutine(RangedAttack(enemyList[playerSelected]));	
	}
	
	IEnumerator RangedAttack(GameObject target){
		const float projectileHeight = 0.4f;
		yield return new WaitForSeconds(1); //replace with animation
		//create 'arrow', and fire it at the selected square
		GameObject cheese = Instantiate(Resources.Load("Objects/Arrow"), transform.position + new Vector3(0,0.4f,0), Quaternion.identity) as GameObject;
		ProjectileScript Projectile = cheese.GetComponent<ProjectileScript>();
		Projectile.Initialise(60, enemyList[playerSelected].transform.position, projectileHeight, 1);
		Debug.Log(enemyList[playerSelected].transform.position);
		
		yield return new WaitForSeconds(1); //allow time for arrow to hit
		remainingAP -= 3;
		ActionComplete();
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
