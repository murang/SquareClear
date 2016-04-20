using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.position = new Vector3(GameControl.STAGE_RANGE,GameControl.STAGE_RANGE, -10);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
