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
            default: GetComponent<Light>().color = Color.white;
                break;
        }
	}
	
}
