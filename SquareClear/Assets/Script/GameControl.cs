using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {

	public static int STAGE_RANGE = 4;
	public Object pre_cell;

	private GameObject[,] cell_matrix = new GameObject[STAGE_RANGE*2+1,STAGE_RANGE*2+1];


//	private List<Color> m_colorList = new List<Color>();
	// Use this for initialization

	void Awake(){
		
	}

	void Start () {
		cell_matrix[STAGE_RANGE, STAGE_RANGE] = (GameObject)Instantiate(pre_cell, new Vector3(STAGE_RANGE,STAGE_RANGE,0), Quaternion.identity);
//		cell_matrix[STAGE_RANGE, STAGE_RANGE].SendMessage("changeColor",Cell.CELL_TYPE.RED);
		cell_matrix[STAGE_RANGE, STAGE_RANGE].GetComponent<Cell>().changeColor(Cell.CELL_COLOR_TYPE.RED);
//		cell_matrix[STAGE_RANGE, STAGE_RANGE].GetComponent<Renderer>().material.SetColor("_Color", (Color)m_colorList[Random.Range(0, m_colorList.Count)]);
//		for(int x=0; x<STAGE_RANGE; x++){
//			for(int y=0; y<STAGE_RANGE; y++){
//				cell_matrix[x, y] = (GameObject)Instantiate(pre_cell, new Vector3((float)x,(float)y,0), Quaternion.identity);
//			}
//		}
	}

	void OnGUI(){
		if(GUI.Button(new Rect(0,0,100,30),"click")){
			cell_matrix[STAGE_RANGE, STAGE_RANGE].GetComponent<Cell>().moveUp();
		}
	}
	
	// Update is called once per frame
	void Update () {
	}

	void gameStart(){
		
	}
}
