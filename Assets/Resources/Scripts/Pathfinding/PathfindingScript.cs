using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathfindingScript : MonoBehaviour {
	
	const string NodeFilePath = "Objects/Pathfinding/Node";
	
	//Vector3[] MoveDirections = new Vector3[]{Vector3.forward, Vector3.back, Vector3.left, Vector3.right};
	public Vector3[] StartPathFinding(Vector4 endPos, List<Vector4> validPoints, int maxJump){
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
			foreach(Vector4 pnt in openList){
				//test for points one G-Score lower than the current point
				if(pnt.w == currentPoint.w - 1){
					//of those points, test for those 1 space away
					if((Mathf.Abs((float)(pnt.x - currentPoint.x)) + Mathf.Abs((float)(pnt.z - currentPoint.z))) == 1 && (Mathf.Abs(pnt.y - currentPoint.y) <= maxJump + 0.1f)){
						currentPoint = pnt;
						closedList.Add(currentPoint);
						break;
					}
				}
			}
		}
		while(currentPoint.w != 0);
		
		Vector3[] pathList = new Vector3[closedList.Count];
		foreach(Vector4 pnt in closedList){
			pathList[Mathf.FloorToInt(pnt.w)] = (Vector3)pnt;
		}
		
		return pathList;
		
	}
}
