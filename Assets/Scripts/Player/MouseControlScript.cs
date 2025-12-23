using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseControlScript : MonoBehaviour {
		public static bool selectPosition = false;
		static RaycastHit rayhit;
		static int groundOnlyLayer = 1 << 8;

		static Vector3 clickPosition = Vector3.zero;
		static Vector4 clickPosition4D = Vector4.zero;

		static List<Vector4> validPoints = new List<Vector4>();

	public static event Vector4Event OnTileClicked;

	void Update(){
		if(Input.GetMouseButtonDown(0)){
		LeftClick();
		}
	}

	public static void SelectPosition(List<Vector4> valid){
		validPoints = valid;
	}

	public static void PositionSelected(){
		selectPosition = false;
	}

	//left mouse click
	private static void LeftClick(){
				Debug.Log ("Left clicked");
		clickPosition4D = Vector4.zero;
		//gets the in-game position of mouse, ensures that only flat ground is clicked, then uses that value for input
		if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayhit, 100, groundOnlyLayer)){ //click on ground
			if(rayhit.normal == new Vector3(0,1,0)){ //ensures that only flat ground can be clicked on
				clickPosition = new Vector3(Mathf.Floor(rayhit.point.x + 0.5f), rayhit.point.y, Mathf.Floor(rayhit.point.z + 0.5f));
				for(int i = 0; i < 20; i++){
					if(validPoints.Contains(new Vector4(clickPosition.x, clickPosition.y, clickPosition.z, i))){
						clickPosition4D = new Vector4(clickPosition.x, clickPosition.y, clickPosition.z,i);
						break;
					}
				}
				if(clickPosition4D != Vector4.zero){ //the position clicked was a valid position
					Debug.Log("LOL");
					if(OnTileClicked != null)
                    {
						OnTileClicked(clickPosition4D); //4D because different elevation can exist for a given position
                    }
										validPoints = new List<Vector4> ();
				}
			}
		}
	}
}
