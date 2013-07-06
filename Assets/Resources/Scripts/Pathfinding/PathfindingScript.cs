using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathfindingScript : MonoBehaviour {
	
	const string NodeFilePath = "Objects/Pathfinding/Node";
	
	//Vector3[] MoveDirections = new Vector3[]{Vector3.forward, Vector3.back, Vector3.left, Vector3.right};
	public Vector3[] StartPathFinding(Vector4 endPos, List<Vector4> validPoints, int maxJump){
		//fixes a glitch where a player would move to (0,0,0) is they clicked their own square
		if(endPos.w == 0){
			return new Vector3[]{transform.position};
		}
		return FindPath(endPos, validPoints, maxJump);
	}
	
	Vector3[] FindPath(Vector4 end, List<Vector4> openList, int maxJump){
		List<Vector4> closedList = new List<Vector4>();
		Vector4 currentPoint = Vector4.zero;
		//error checking. if end position isn't on list, return empty path list
		if(!openList.Contains(end)){
			return new Vector3[0];
		}
		else{
			closedList.Add(end);
			currentPoint = end;
		}
		
		do{
			int lowestPntHeight = 100;
			Vector4 tempCurrentPoint = Vector4.zero;
			foreach(Vector4 pnt in openList){
				//test for points one G-Score lower than the current point
				if(pnt.w == currentPoint.w - 1){
					int pntHeight = Mathf.Abs((int)(pnt.y - currentPoint.y));
					int xDistance = Mathf.Abs((int)(pnt.x - currentPoint.x));
					int zDistance = Mathf.Abs((int)(pnt.z - currentPoint.z));
					//of those points, test for those 1 space away
					if(xDistance + zDistance == 1 && pntHeight <= (maxJump + 0.05f)){
						//prioritise points that don't require jumping
						if(pntHeight < lowestPntHeight){
							tempCurrentPoint = pnt;
							lowestPntHeight = pntHeight;
						}
						
					}
				}
			}
			currentPoint = tempCurrentPoint;
			closedList.Add(currentPoint);
		}
		while(currentPoint.w != 0);
		
		Vector3[] pathList = new Vector3[closedList.Count];
		foreach(Vector4 pnt in closedList){
			pathList[Mathf.FloorToInt(pnt.w)] = (Vector3)pnt;
		}
		
		return pathList;
		
	}
}
