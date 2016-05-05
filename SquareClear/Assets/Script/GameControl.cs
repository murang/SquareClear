using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {

	public Object pre_cell;

	private GameObject[,] cell_matrix = new GameObject[GlobalParam.g_StageRange*2+1,GlobalParam.g_StageRange*2+1];


//	private List<Color> m_colorList = new List<Color>();
	// Use this for initialization

	void Awake(){
		
	}

	void Start () {
		cell_matrix[GlobalParam.g_StageRange, GlobalParam.g_StageRange] = (GameObject)Instantiate(pre_cell, new Vector3(GlobalParam.g_StageRange,GlobalParam.g_StageRange,0), Quaternion.identity);
		cell_matrix[GlobalParam.g_StageRange, GlobalParam.g_StageRange].GetComponent<Cell>().changeColor(Cell.CELL_COLOR_TYPE.RED);

	}

	void OnGUI(){
		if(GUI.Button(new Rect(0,0,100,30),"click")){
			cell_matrix[GlobalParam.g_StageRange, GlobalParam.g_StageRange].GetComponent<Cell>().moveLeft();
		}
	}
	
	// Update is called once per frame
	void Update () {
		checkTouch();
	}

	void gameStart(){
		
	}

	private void checkTouch(){
//		if(Input.touchCount ==1 && Input.GetTouch(0).phase == TouchPhase.Began){
//			Debug.Log("tocuh");
//		}
		if(Input.GetMouseButton(0)){
			Ray  ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit = new RaycastHit();
			Physics.Raycast(ray, out hit, 100);
			if(hit.transform != null){
				print(hit.point);
			}
		}
	}
}
