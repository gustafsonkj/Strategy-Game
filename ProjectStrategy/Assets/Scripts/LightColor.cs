using UnityEngine;
using System.Collections;

public class LightColor : MonoBehaviour {

	// Use this for initialization
	void Start () {
        switch(GetComponentInParent<Unit>().UnitColor)
        {
            case 0: GetComponent<Light>().color = Color.red;
                break;
            case 1: GetComponent<Light>().color = Color.green;
                break;
            case 2: GetComponent<Light>().color = Color.blue;
                break;
            case 3: GetComponent<Light>().color = Color.yellow;
                break;
            case 4: GetComponent<Light>().color = Color.magenta;
                break;
            default: GetComponent<Light>().color = Color.white;
                break;
        }
	}
	
}
