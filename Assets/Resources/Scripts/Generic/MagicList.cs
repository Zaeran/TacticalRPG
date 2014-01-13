using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagicList : MonoBehaviour {
	
	Dictionary<string, string[]> magicInfo;

	TextAsset MagicData;

	void LoadMagicData(){
		string[] lineData = MagicData.text.Split('\n');
		foreach(string magData in lineData){
			string[] magic = magData.Split(' ');
			if(magic.Length > 2){
				string[] data = new string[magic.Length - 1];
				for(int i = 0; i < data.Length; i++){
					data[i] = magic[i+1];
				}
				magicInfo.Add(magic[0], data);
			}
		}
	}
	
	public void DestroyBlock(GameObject obj, Vector3 pos){
		Destroy(obj);
	}
	
	public void Teleport(GameObject obj, Vector3 pos){
	    transform.position = pos;
	}
	
	
	
}
