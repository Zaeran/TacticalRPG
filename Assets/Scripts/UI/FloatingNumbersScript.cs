using UnityEngine;
using System.Collections;

public class FloatingNumbersScript : MonoBehaviour {

	public static FloatingNumbersScript instance;
	public GameObject floatingNumberObj;

    private void Awake()
    {
        instance = this;
    }

    public static void CreateFloatingNumber(Vector3 position, string val, Color c)
    {
		GameObject number = Instantiate(instance.floatingNumberObj, Camera.main.WorldToScreenPoint(position) + new Vector3(Random.Range(-25,25), Random.Range(-25,25), 0), Quaternion.identity, instance.transform);
		number.GetComponent<FloatingNumber>().Initialize(val, c);
    }
}
