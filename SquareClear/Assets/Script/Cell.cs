using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Cell : MonoBehaviour {
	

	private ArrayList m_colorList = new ArrayList(){
		Color.black,
		Color.blue,
		Color.cyan,
		Color.gray,
		Color.green,
		Color.magenta,
		Color.red,
		Color.white,
		Color.yellow
	};
	public enum CELL_COLOR_TYPE{
		BLACK = 0,
		BLUE,
		CYAN,
		GRAY,
		GREEN,
		MAGENTA,
		RED,
		WHITE,
		YELLOW
	};
	private CELL_COLOR_TYPE m_CellType; //use to identify the kind of the cell
//	private float m_MoveTimeCheck = .0f;


//	enum CELL_MOVE_STATE{
//		STOP,
//		UP,
//		DOWN,
//		LEFT,
//		RIGHT
//	}

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
//		m_MoveTimeCheck += Time.deltaTime;
	}

	public void changeColor(CELL_COLOR_TYPE type){
		m_CellType = type;
		this.GetComponent<Renderer>().material.SetColor("_Color", (Color)m_colorList[(int)m_CellType]);
	}

	public void moveUp(){
//		if(m_MoveTimeCheck <= GlobalParam.g_MoveDuration)return;
//		m_MoveTimeCheck = 0;
		transform.DOMoveY(transform.position.y+1, GlobalParam.g_MoveDuration);
	}
	public void moveDown(){
//		if(m_MoveTimeCheck <= GlobalParam.g_MoveDuration)return;
//		m_MoveTimeCheck = 0;
		transform.DOMoveY(transform.position.y-1, GlobalParam.g_MoveDuration);
	}
	public void moveLeft(){
//		if(m_MoveTimeCheck <= GlobalParam.g_MoveDuration)return;
//		m_MoveTimeCheck = 0;
		transform.DOMoveX(transform.position.x-1, GlobalParam.g_MoveDuration);
	}
	public void moveRight(){
//		if(m_MoveTimeCheck <= GlobalParam.g_MoveDuration)return;
//		m_MoveTimeCheck = 0;
		transform.DOMoveX(transform.position.x+1, GlobalParam.g_MoveDuration);
	}
}
