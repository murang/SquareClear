using UnityEngine;
using System.Collections;

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
	private CELL_COLOR_TYPE m_CellType;

	enum CELL_MOVE_STATE{
		STOP,
		UP,
		DOWN,
		LEFT,
		RIGHT
	}

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void changeColor(CELL_COLOR_TYPE type){
		m_CellType = type;
		this.GetComponent<Renderer>().material.SetColor("_Color", (Color)m_colorList[(int)m_CellType]);
	}

	public void moveUp(){
		transform.position=Vector3.MoveTowards(transform.position,transform.position+Vector3.one,2);
	}
}
