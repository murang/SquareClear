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
	private Vector2 touchBegin;
	private Vector2 touchOffset;
	private enum TOUCH_STATE{
		NONE,
		JUDGE_DIR,
		MOVING,
		AUTO_ACT
	}
	private TOUCH_STATE currentTouchState = TOUCH_STATE.NONE;
	private int moveDir = 0; //1:x 2:y

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
//			Debug.Log("check");
		}
		if(Input.GetMouseButtonDown(0)){
			touchRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(touchRay, out touchHit, Mathf.Infinity)){
//				Debug.Log("down");
				isMouseDown = true;
				touchBegin = new Vector2 (touchHit.point.x, touchHit.point.y);
				touchOffset = new Vector2 (touchHit.transform.position.x - touchHit.point.x, touchHit.transform.position.y - touchHit.point.y);
				currentTouchState = TOUCH_STATE.JUDGE_DIR;
			};
		}
		if(Input.GetMouseButtonUp(0)){
			resetTouchState ();
//			Debug.Log("up");
		}
	}

	void touchDrag(){
		touchRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		Physics.Raycast (touchRay, out touchHit, Mathf.Infinity);
		switch (currentTouchState) {
		case TOUCH_STATE.NONE:
			break;
		case TOUCH_STATE.JUDGE_DIR:
			if(Vector2.Distance(touchBegin, new Vector2(touchHit.point.x, touchHit.point.y))>=0.2f){
				currentTouchState = TOUCH_STATE.MOVING;
				if (Mathf.Abs (touchHit.point.x - touchBegin.x) > Mathf.Abs (touchHit.point.y - touchBegin.y)) {
					moveDir = 1;
				} else {
					moveDir = 2;
				}
			}
			break;
		case TOUCH_STATE.MOVING:
			GameObject moveObj = cell_matrix [(int)Mathf.Floor (touchHit.point.x + 0.5f), (int)Mathf.Floor (touchHit.point.y + 0.5f)];
			if (moveDir == 1) {
				moveObj.transform.position = new Vector3 (touchOffset.x + touchHit.point.x, moveObj.transform.position.y, moveObj.transform.position.z);
			} else if (moveDir == 2) {
				moveObj.transform.position = new Vector3 (moveObj.transform.position.x, touchOffset.y + touchHit.point.y, moveObj.transform.position.z);
			}
			break;
		}

//		touchRay = Camera.main.ScreenPointToRay(Input.mousePosition);
//		Physics.Raycast(touchRay, out touchHit, Mathf.Infinity);
//		cell_matrix[(int)Mathf.Floor(touchHit.point.x+0.5f), (int)Mathf.Floor(touchHit.point.y+0.5f)].transform.position = new Vector3(touchHit.point.x, touchHit.point.y, 0f);
	}

	void resetTouchState(){
		isMouseDown = false;
		touchBegin = Vector2.zero;
		touchOffset = Vector2.zero;
		currentTouchState = TOUCH_STATE.NONE;
		moveDir = 0;
	}
}
