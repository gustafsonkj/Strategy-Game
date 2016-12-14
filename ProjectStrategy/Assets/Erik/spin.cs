using UnityEngine;
using System.Collections;

public class spin : MonoBehaviour {


	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up,200 * Time.deltaTime );
	}
}
