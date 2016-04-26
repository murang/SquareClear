using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.position = new Vector3(GlobalParam.g_StageRange,GlobalParam.g_StageRange, -10);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
