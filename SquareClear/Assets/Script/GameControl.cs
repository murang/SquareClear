﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour {

	public Object pre_cell;

	private int m_level = GlobalParam.g_ColorLevel;

	private GameObject[,] cell_matrix = new GameObject[GlobalParam.g_StageRange*2+1,GlobalParam.g_StageRange*2+1];

	private Ray touchRay;
	private RaycastHit touchHit;
	private Vector2 touchHitIndex;

	//temp for mouse debug on pc
	private bool isMouseDown = false;
	private Vector2 touchBegin;
	private Vector2 touchOffset;
	private enum TOUCH_STATE{
		NONE,
		JUDGE_DIR,
		MOVING,
		AUTO
	}
	private enum MOVE_DIR
	{
		NONE,
		X,
		Y
	}
	private TOUCH_STATE currentTouchState = TOUCH_STATE.NONE;
	private MOVE_DIR moveDir = MOVE_DIR.NONE;

//	private List<Color> m_colorList = new List<Color>();
	// Use this for initialization

	void Awake(){
		
	}

	void Start () {
		insertCube (GlobalParam.g_StageRange, GlobalParam.g_StageRange);
	}

	void OnGUI(){
		if(GUI.Button(new Rect(0,0,100,30),"Level-"+m_level)){
//			SceneManager.LoadSceneAsync("ActionPhase");
			if (m_level >= 9) {
				m_level = GlobalParam.g_ColorLevel;
			} else {
				m_level++;
			}
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
				touchHitIndex = new Vector2 (touchHit.transform.position.x, touchHit.transform.position.y);
				currentTouchState = TOUCH_STATE.JUDGE_DIR;
			};
		}
		if(Input.GetMouseButtonUp(0)){
			if (currentTouchState == TOUCH_STATE.AUTO) {
				resetTouchState ();
			} else {
				resetTouchState ();
				for (int x = 0; x < GlobalParam.g_StageRange * 2 + 1; x++) {
					for (int y = 0; y < GlobalParam.g_StageRange * 2 + 1; y++) {
						if (cell_matrix [x, y] != null) {
							cell_matrix [x, y].GetComponent<Cell> ().moveToTarget ();
						}
					}
				}
			}
//			Debug.Log("up");
		}
	}

	void touchDrag(){
		Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		switch (currentTouchState) {
		case TOUCH_STATE.NONE:
			break;
		case TOUCH_STATE.JUDGE_DIR:
			if(Vector2.Distance(touchBegin, new Vector2(touchPos.x, touchPos.y))>=0.1f){
				if (Mathf.Abs (touchPos.x - touchBegin.x) > Mathf.Abs (touchPos.y - touchBegin.y)) {
					moveDir = MOVE_DIR.X;
				} else {
					moveDir = MOVE_DIR.Y;
				}
				currentTouchState = TOUCH_STATE.MOVING;
			}
			break;
		case TOUCH_STATE.MOVING:
			MOVE_DIR t_move = MOVE_DIR.NONE;
			int t_index = 0;
			bool t_aORr = false;
			if (moveDir == MOVE_DIR.X) {
				int line_index = (int)Mathf.Floor (touchHit.point.y + 0.5f);
				if ((cell_matrix [0, line_index] != null && (touchPos.x - touchBegin.x) < 0.0f)
					|| (cell_matrix [GlobalParam.g_StageRange*2, line_index] != null && (touchPos.x - touchBegin.x) > 0.0f)
				) {
					break;
				}
				for (int i = 0; i < GlobalParam.g_StageRange * 2 + 1; i++) {
					if (cell_matrix [i, line_index] != null) {
						GameObject moveObj = cell_matrix [i, line_index];
						cell_matrix [i, line_index].transform.position = new Vector3 (touchOffset.x + touchPos.x + (i-touchHitIndex.x), moveObj.transform.position.y, moveObj.transform.position.z);
						if (touchPos.x - touchBegin.x > 0.5f) {
//							cell_matrix[GlobalParam.g_StageRange, GlobalParam.g_StageRange].GetComponent<Cell>().moveRight();
							moveObj.GetComponent<Cell> ().moveRight ();
//							changeMatrixIndex (MOVE_DIR.X, line_index, true);
							t_move = MOVE_DIR.X;
							t_index = line_index;
							t_aORr = true;
							currentTouchState = TOUCH_STATE.AUTO;
//							Debug.Log ("move right");
						} else if (touchPos.x - touchBegin.x < -0.5f) {
//							cell_matrix[GlobalParam.g_StageRange, GlobalParam.g_StageRange].GetComponent<Cell>().moveLeft();
							moveObj.GetComponent<Cell> ().moveLeft ();
//							changeMatrixIndex (MOVE_DIR.X, line_index, false);
							t_move = MOVE_DIR.X;
							t_index = line_index;
							t_aORr = false;
							currentTouchState = TOUCH_STATE.AUTO;
//							Debug.Log ("move left");
						}
					}
				}
			} else if (moveDir == MOVE_DIR.Y) {
				int line_index = (int)Mathf.Floor (touchHit.point.x + 0.5f);
				if ((cell_matrix [line_index, 0] != null && (touchPos.y - touchBegin.y) < 0.0f)
					|| (cell_matrix [line_index, GlobalParam.g_StageRange*2] != null && (touchPos.y - touchBegin.y) > 0.0f)
				) {
					break;
				}
				for (int i = 0; i < GlobalParam.g_StageRange * 2 + 1; i++) {
					if (cell_matrix [line_index, i] != null) {
						GameObject moveObj = cell_matrix [line_index, i];
						cell_matrix [line_index, i].transform.position = new Vector3 (moveObj.transform.position.x, touchOffset.y + touchPos.y + (i - touchHitIndex.y), moveObj.transform.position.z);
						if (touchPos.y - touchBegin.y > 0.5f) {
//							cell_matrix[GlobalParam.g_StageRange, GlobalParam.g_StageRange].GetComponent<Cell>().moveUp();
							moveObj.GetComponent<Cell> ().moveUp ();
							t_move = MOVE_DIR.Y;
							t_index = line_index;
							t_aORr = true;
//							changeMatrixIndex (MOVE_DIR.Y, line_index, true);
							currentTouchState = TOUCH_STATE.AUTO;
//							Debug.Log ("move up");
						} else if (touchPos.y - touchBegin.y < -0.5f) {
//							cell_matrix[GlobalParam.g_StageRange, GlobalParam.g_StageRange].GetComponent<Cell>().moveDown();
							moveObj.GetComponent<Cell> ().moveDown ();
//							changeMatrixIndex (MOVE_DIR.Y, line_index, false);
							t_move = MOVE_DIR.Y;
							t_index = line_index;
							t_aORr = false;
							currentTouchState = TOUCH_STATE.AUTO;
//							Debug.Log ("move down");
						}
					}
				}
			}
			if (t_move != MOVE_DIR.NONE) {
				changeMatrixIndex (t_move, t_index, t_aORr);
				StartCoroutine (checkClear (t_move));
			}
			break;
		}

//		touchRay = Camera.main.ScreenPointToRay(Input.mousePosition);
//		Physics.Raycast(touchRay, out touchHit, Mathf.Infinity);
//		cell_matrix[(int)Mathf.Floor(touchHit.point.x+0.5f), (int)Mathf.Floor(touchHit.point.y+0.5f)].transform.position = new Vector3(touchHit.point.x, touchHit.point.y, 0f);
	}

	IEnumerator checkClear(MOVE_DIR dir){
		yield return new WaitForSeconds (GlobalParam.g_MoveDuration);
		List<Vector2> clearList = new List<Vector2>();
		for (int i = 0; i < GlobalParam.g_StageRange * 2; i++) {
			for (int j = 0; j < GlobalParam.g_StageRange * 2; j++) {
				if (cell_matrix [i, j] == null
					||cell_matrix[i+1, j] == null
					||cell_matrix[i, j+1] == null
					||cell_matrix[i+1, j+1] == null
				) {
					continue;
				}
				if (cell_matrix [i, j].GetComponent<Cell> ().getColorIndex () == cell_matrix [i + 1, j].GetComponent<Cell> ().getColorIndex ()
				   && cell_matrix [i, j].GetComponent<Cell> ().getColorIndex () == cell_matrix [i, j + 1].GetComponent<Cell> ().getColorIndex ()
				   && cell_matrix [i, j].GetComponent<Cell> ().getColorIndex () == cell_matrix [i + 1, j + 1].GetComponent<Cell> ().getColorIndex ()) {
					clearList.Add (new Vector2 (i, j));
				}
			}	
		}
		foreach (Vector2 index in clearList) {
			clearCubes (index, dir);
		}
	}

	void resetTouchState(){
		isMouseDown = false;
		touchBegin = Vector2.zero;
		touchOffset = Vector2.zero;
		touchHitIndex = Vector2.zero;
		currentTouchState = TOUCH_STATE.NONE;
		moveDir = MOVE_DIR.NONE;
	}

	void changeMatrixIndex(MOVE_DIR dir, int index, bool addOrReduce){
		switch (dir) {
		case MOVE_DIR.X:
			{
				if (addOrReduce) {
					int insert_index = -1;
					for (int i = GlobalParam.g_StageRange * 2; i > 0; i--) {
						cell_matrix [i, index] = cell_matrix [i - 1, index];
						if (cell_matrix [i, index] != null) {
							insert_index = i - 1;
						}
					}
					cell_matrix [0, index] = null;
					if (insert_index != -1) {
						insertCube (insert_index, index);
					}
				} else {
					int insert_index = -1;
					for (int i = 0; i < GlobalParam.g_StageRange * 2; i++) {
						cell_matrix [i, index] = cell_matrix [i + 1, index];
						if (cell_matrix [i, index] != null) {
							insert_index = i + 1;
						}
					}
					cell_matrix [GlobalParam.g_StageRange * 2, index] = null;
					if (insert_index != -1) {
						insertCube (insert_index, index);
					}
				}
				break;
			}
		case MOVE_DIR.Y:
			{
				if (addOrReduce) {
					int insert_index = -1;
					for (int i = GlobalParam.g_StageRange * 2; i > 0; i--) {
						cell_matrix [index, i] = cell_matrix [index, i - 1];
						if (cell_matrix [index, i] != null) {
							insert_index = i - 1;
						}
					}
					cell_matrix [index, 0] = null;
					if (insert_index != -1) {
						insertCube (index, insert_index);
					}
				} else {
					int insert_index = -1;
					for (int i = 0; i < GlobalParam.g_StageRange * 2; i++) {
						cell_matrix [index, i] = cell_matrix [index, i + 1];
						if (cell_matrix [index, i] != null) {
							insert_index = i + 1;
						}
					}
					cell_matrix [index, GlobalParam.g_StageRange * 2] = null;
					if (insert_index != -1) {
						insertCube (index, insert_index);
					}
				}
				break;
			}
		}
	}

	void insertCube(int index_x, int index_y){
		cell_matrix[index_x, index_y] = (GameObject)Instantiate(pre_cell, new Vector3(index_x, index_y,0), Quaternion.identity);
		cell_matrix[index_x, index_y].GetComponent<Cell>().setOrder(index_x, index_y);
		int r = ((int)Random.Range (0, 100))%GlobalParam.g_ColorLevel;
		cell_matrix[index_x, index_y].GetComponent<Cell>().changeColor(r);
	}

	void clearCubes(Vector2 startLoc, MOVE_DIR dir){
		int startLoc_x = (int)startLoc.x;
		int startLoc_y = (int)startLoc.y;
		DestroyImmediate (cell_matrix [startLoc_x, startLoc_y]);
		DestroyImmediate (cell_matrix [startLoc_x+1, startLoc_y]);
		DestroyImmediate (cell_matrix [startLoc_x, startLoc_y+1]);
		DestroyImmediate (cell_matrix [startLoc_x+1, startLoc_y+1]);
		switch (dir) {
		case MOVE_DIR.X:
			{
				for (int i = startLoc_x; i >0; i--) {
					cell_matrix [i, startLoc_y] = cell_matrix [i - 1, startLoc_y];
					if (cell_matrix [i, startLoc_y] != null) {
						cell_matrix [i, startLoc_y].GetComponent<Cell> ().moveRight ();
					}
					cell_matrix [i, startLoc_y+1] = cell_matrix [i - 1, startLoc_y+1];
					if (cell_matrix [i, startLoc_y+1] != null) {
						cell_matrix [i, startLoc_y+1].GetComponent<Cell> ().moveRight ();
					}
				}
				cell_matrix [0, startLoc_y] = null;
				cell_matrix [0, startLoc_y+1] = null;
				for (int i = startLoc_x+1; i <GlobalParam.g_StageRange*2; i++) {
					cell_matrix [i, startLoc_y] = cell_matrix [i + 1, startLoc_y];
					if (cell_matrix [i, startLoc_y] != null) {
						cell_matrix [i, startLoc_y].GetComponent<Cell> ().moveLeft ();
					}
					cell_matrix [i, startLoc_y+1] = cell_matrix [i + 1, startLoc_y+1];
					if (cell_matrix [i, startLoc_y+1] != null) {
						cell_matrix [i, startLoc_y+1].GetComponent<Cell> ().moveLeft ();
					}
				}
				cell_matrix [GlobalParam.g_StageRange*2, startLoc_y] = null;
				cell_matrix [GlobalParam.g_StageRange*2, startLoc_y+1] = null;
				break;
			}
		case  MOVE_DIR.Y:
			{
				for (int i = startLoc_y; i >0; i--) {
					cell_matrix [startLoc_x, i] = cell_matrix [startLoc_x, i-1];
					if (cell_matrix [startLoc_x, i] != null) {
						cell_matrix [startLoc_x, i].GetComponent<Cell> ().moveUp ();
					}
					cell_matrix [startLoc_x+1, i] = cell_matrix [startLoc_x+1, i-1];
					if (cell_matrix [startLoc_x+1, i] != null) {
						cell_matrix [startLoc_x+1, i].GetComponent<Cell> ().moveUp ();
					}
				}
				cell_matrix [startLoc_x, 0] = null;
				cell_matrix [startLoc_x+1, 0] = null;
				for (int i = startLoc_y+1; i <GlobalParam.g_StageRange*2; i++) {
					cell_matrix [startLoc_x, i] = cell_matrix [startLoc_x, i+1];
					if (cell_matrix [startLoc_x, i] != null) {
						cell_matrix [startLoc_x, i].GetComponent<Cell> ().moveDown ();
					}
					cell_matrix [startLoc_x+1, i] = cell_matrix [startLoc_x+1, i+1];
					if (cell_matrix [startLoc_x+1, i] != null) {
						cell_matrix [startLoc_x+1, i].GetComponent<Cell> ().moveDown ();
					}
				}
				cell_matrix [startLoc_x, GlobalParam.g_StageRange*2] = null;
				cell_matrix [startLoc_x+1, GlobalParam.g_StageRange*2] = null;
				break;
			}
		}
	}

	void autoMoveMatrix(){
		
	}
}
