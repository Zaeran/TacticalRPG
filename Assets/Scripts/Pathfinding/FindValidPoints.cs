using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FindValidPoints : MonoBehaviour {

	static int terrainLayerMask = 1 << 8;
	static Vector3[] MoveDirections = new Vector3[]{Vector3.forward, Vector3.back, Vector3.left, Vector3.right};
	static List<Vector4> validPoints;

    public static List<Vector4> GetPoints(string type, Vector3 origin, int maxRange = 0, int maxJump = 0){ //1 - move. 		2 - melee attack.		3 - ranged attack.		4 - spell.
		switch(type){
		case "Move":
			validPoints = GetValidMovePoints(maxRange, maxJump, origin);
			return validPoints;
		case "Melee": // melee
			validPoints = GetValidMeleePoints(maxRange, maxJump, origin);
			return validPoints;
		case "Ranged": // ranged
			validPoints = GetValidRangedPoints(maxRange, maxJump, origin);
			return validPoints;
		case "Magic":
			validPoints = GetValidMagicPoints(maxRange, maxJump, origin);
			return validPoints;
		default:
			return new List<Vector4>();
		}
	}
	
	private static List<Vector4> GetValidMovePoints(int maxMove, int maxJump, Vector3 origin){
		//declare variables
		int heightDifference = 0;
		int distanceWalked = 0;
		RaycastHit currentPoint = new RaycastHit();
		Vector3 startPoint = Vector3.zero;
		Vector3 testPoint;
		Vector3 nextPoint;
		List<Vector4> pointList = new List<Vector4>();
		List<Vector4> finalPointList = new List<Vector4>();
		List<Vector3> positionList = new List<Vector3>();
		RaycastHit[] walkableTiles;
		RaycastHit[] initialTile;
		
		//begin at the object's location
		initialTile = Physics.RaycastAll(new Vector3(Mathf.Floor(origin.x), Mathf.Floor(origin.y), Mathf.Floor(origin.z)) + (Vector3.up * 30), Vector3.down, 50, terrainLayerMask);
		foreach(RaycastHit r in initialTile){ //get current tile, instead of tile at same x/z but different height
			if(r.point.y == origin.y){
				startPoint = r.point;
				break;
				}
		}
		//add the starting point to the list of available positions
		positionList.Add(startPoint);
		pointList.Add(new Vector4(startPoint.x, startPoint.y, startPoint.z, 0));
		testPoint = startPoint;
		
		//determine the maximum amount of possible tiles that can be moved to
		int moveTiles = 0;
		for(int i = maxMove; i > 0; i--){
			moveTiles += (maxMove * 4) + 1;
		}
		//run for each tile
		for(int i = 0; i < moveTiles; i++){
			for(int moveDir = 0; moveDir < MoveDirections.Length; moveDir++){
				//set next point to test as adjacent
				nextPoint = testPoint + MoveDirections[moveDir];
				
				//raycast to get height of current point, then use raycasting to find if adjacent points are valid
				initialTile = Physics.RaycastAll(testPoint + (Vector3.up * 30), Vector3.down, 50,terrainLayerMask);
				foreach(RaycastHit r in initialTile){ //get current tile, instead of tile at same x/z but different height
					if(r.point.y == testPoint.y){
						currentPoint = r;
						break;
					}
				}
				//raycast the adjacent tile
				walkableTiles = Physics.RaycastAll(nextPoint + (Vector3.up * 30), Vector3.down, 50, terrainLayerMask);
				foreach(RaycastHit rh in walkableTiles){
					//ensure that tile is empty
					if(Physics.OverlapSphere(rh.point, 0.3f, ~terrainLayerMask).Length == 0){
						//calculate the height difference between current and adjacent tile
						heightDifference = Mathf.FloorToInt(rh.point.y - currentPoint.point.y + 0.1f);
						//can character reach this height?
						if(Mathf.Abs(heightDifference) <= maxJump){
							//adjacent point is within acceptable height limits
							if(!positionList.Contains(rh.point)){
								//point hasn't been travelled to before
								if(distanceWalked < maxMove){
									//haven't exceeded max move distance
									pointList.Add(new Vector4(rh.point.x, rh.point.y, rh.point.z, distanceWalked+1));
									positionList.Add(rh.point);
								}
							}
						}
					}
				}
			}
			//if more points exist, go to the first point on the list, then remove it from pointList
			if(pointList.Count > 0){
				testPoint = pointList[0];
				distanceWalked = (int)pointList[0].w;
				finalPointList.Add(pointList[0]);
				pointList.Remove(pointList[0]);				
			}				
		}
		return finalPointList;
	}
	
	//gets valid points for melee combat.
	//points are in a straight line
	private static List<Vector4> GetValidMeleePoints(int maxRange, int maxJump, Vector3 origin){
		int heightDifference = 0;
		List<Vector4> validAttacks = new List<Vector4>();
		RaycastHit currentPoint = new RaycastHit();
		RaycastHit[] initialTile;
		//test each direction
		for(int moveDir = 0; moveDir < MoveDirections.Length; moveDir++){
			initialTile = Physics.RaycastAll(origin + (Vector3.up * 30), Vector3.down, 50,terrainLayerMask);
			foreach(RaycastHit rh in initialTile){
				if(rh.point.y == origin.y){
					currentPoint = rh;
					break;
				}
			}
			Vector3 testPoint = currentPoint.point;
			int pointDistance = 1;
			//make sure that the max attack range is not exceeded
			for(int r = 1; r <= maxRange; r++){
				
				Vector3 nextPoint = testPoint + (MoveDirections[moveDir] * r);
				//raycast to get height of current point, then use raycasting to find if adjacent points are valid
				
				initialTile = Physics.RaycastAll(nextPoint + (Vector3.up * 30), Vector3.down, 50,terrainLayerMask);
				foreach(RaycastHit rh in initialTile){
					heightDifference = Mathf.FloorToInt(rh.point.y - currentPoint.point.y + 0.1f);
					if(Mathf.Abs(heightDifference) <= maxJump){
						validAttacks.Add(new Vector4(rh.point.x, rh.point.y, rh.point.z, pointDistance + 1));
						pointDistance++;
					}
				}
			}
		}
		return validAttacks;
	}
	
	//gets valid points for ranged attacks
	//points can be at any location in range. no adjacent tiles required
	private static List<Vector4> GetValidRangedPoints(int maxRange, int maxDrop, Vector3 origin){
		List<Vector4> validAttacks = new List<Vector4>();
		RaycastHit[] initialTile;
		int horDistanceToPoint = 0;
		for(int x = -maxRange; x <= maxRange; x++){ // x direction
			for(int z = -(maxRange - Mathf.Abs(x)); z <= maxRange - Mathf.Abs(x); z++){
				initialTile = Physics.RaycastAll(origin + new Vector3(x,0,z) + (Vector3.up * 30), Vector3.down, 50,terrainLayerMask);
				foreach(RaycastHit rh in initialTile){
					horDistanceToPoint = Mathf.FloorToInt(Mathf.Abs((int)(rh.point.x - origin.x))) + (Mathf.FloorToInt(Mathf.Abs((int)(rh.point.z - origin.z))));
					if(Mathf.FloorToInt(horDistanceToPoint + ((rh.point.y - origin.y)/maxDrop)) <= maxRange){ // adds range for height decrease, reduces for height increase
						validAttacks.Add(new Vector4(rh.point.x, rh.point.y, rh.point.z, horDistanceToPoint));
					}
				}
			}
		}
		
		return validAttacks;
	}
	
	private static List<Vector4> GetValidMagicPoints(int maxRange, int maxDrop, Vector3 origin){
		List<Vector4> validAttacks = new List<Vector4>();
		RaycastHit[] initialTile;
		int horDistanceToPoint = 0;
		for(int x = -maxRange; x <= maxRange; x++){ // x direction
			for (int z = -(maxRange - Mathf.Abs(x)); z <= maxRange - Mathf.Abs(x); z++)
			{
				initialTile = Physics.RaycastAll(origin + new Vector3(x, 0, z) + (Vector3.up * 30), Vector3.down, 50, terrainLayerMask);
				foreach (RaycastHit rh in initialTile){
					horDistanceToPoint = Mathf.FloorToInt(Mathf.Abs((int)(rh.point.x - origin.x))) + (Mathf.FloorToInt(Mathf.Abs((int)(rh.point.z - origin.z))));
					validAttacks.Add(new Vector4(rh.point.x, rh.point.y, rh.point.z, horDistanceToPoint));
				}
			}
		}
		
		return validAttacks;
	}
}