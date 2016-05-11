using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {

	public Object pre_cell;

	private GameObject[,] cell_matrix = new GameObject[GlobalParam.g_StageRange*2+1,GlobalParam.g_StageRange*2+1];

	private Ray touchRay;
	private RaycastHit touchHit;

	//temp for mouse debug on pc
	private bool isMouseDown = false;

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

	void checkTouch(){
//		if(Input.touchCount ==1 && Input.GetTouch(0).phase == TouchPhase.Began){
//			Debug.Log("tocuh");
//		}
		if(isMouseDown){
			touchDrag();
			Debug.Log("check");
		}
		if(Input.GetMouseButtonDown(0)){
			touchRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(touchRay, out touchHit, Mathf.Infinity)){
				Debug.Log("down");
				isMouseDown = true;
			};
		}
		if(Input.GetMouseButtonUp(0)){
			isMouseDown = false;
			Debug.Log("up");
		}
	}

	void touchDrag(){
		touchRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		Physics.Raycast(touchRay, out touchHit, Mathf.Infinity);
		cell_matrix[(int)Mathf.Floor(touchHit.point.x+0.5f), (int)Mathf.Floor(touchHit.point.y+0.5f)].transform.position = new Vector3(touchHit.point.x, touchHit.point.y, 0f);
	}
}
