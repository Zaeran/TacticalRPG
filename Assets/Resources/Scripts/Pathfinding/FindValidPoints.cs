using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FindValidPoints : MonoBehaviour {

	Vector3[] MoveDirections = new Vector3[]{Vector3.forward, Vector3.back, Vector3.left, Vector3.right};
	List<Vector4> validPoints;
	
	public List<Vector4> GetPoints(int type, int maxRange, int maxJump){ //1 - move. 		2 - melee attack.		3 - ranged attack.		4 - spell.
		if(type == 1){ //move
			validPoints = GetValidMovePoints(maxRange, maxJump);
			return validPoints;
		}
		return new List<Vector4>();
	}
	
	private List<Vector4> GetValidMovePoints(int maxMove, int maxJump){
		//declare variables
		int terrainLayerMask = 1 << 8;
		int heightDifference = 0;
		int distanceWalked = 0;
		RaycastHit currentPoint;
		Vector3 startPoint;
		Vector3 testPoint;
		Vector3 nextPoint;
		List<Vector4> pointList = new List<Vector4>();
		List<Vector4> finalPointList = new List<Vector4>();
		List<Vector3> positionList = new List<Vector3>();
		RaycastHit[] walkableTiles;
		
		Physics.Raycast(new Vector3(Mathf.Floor(transform.position.x), Mathf.Floor(transform.position.y), Mathf.Floor(transform.position.z)) + (Vector3.up * 30), Vector3.down, out currentPoint, 50, terrainLayerMask);
		startPoint = currentPoint.point;
		positionList.Add(startPoint);
		pointList.Add(new Vector4(startPoint.x, startPoint.y, startPoint.z, 0));
		testPoint = startPoint;
		
		//determine the maximum amount of possible tiles that can be moved to
		int moveTiles = 0;
		for(int i = maxMove; i > 0; i--){
			moveTiles += maxMove * 4;
		}
		int x = 0;
		//run for each tile
		for(int i = 0; i < moveTiles; i++){
			
			for(int moveDir = 0; moveDir < MoveDirections.Length; moveDir++){
				//set next point to test as adjacent
				nextPoint = testPoint + MoveDirections[moveDir];
				
				//raycast to get height of current point, then use raycasting to find if adjacent points are valid
				if(Physics.Raycast(testPoint + (Vector3.up * 30), Vector3.down, out currentPoint, 50,terrainLayerMask)){
					walkableTiles = Physics.RaycastAll(nextPoint + (Vector3.up * 30), Vector3.down, 50, terrainLayerMask);
					foreach(RaycastHit rh in walkableTiles){
						if(Physics.OverlapSphere(rh.point, 0.3f, ~terrainLayerMask).Length == 0){
							heightDifference = Mathf.FloorToInt(rh.point.y - currentPoint.point.y + 0.1f);
							if(heightDifference <= maxJump && heightDifference >= -maxJump){
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
}
