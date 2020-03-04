using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class GridBuilder : MonoBehaviour {
	private int[,] map;
	private enum cellVal
    {
        EMPTY,
        ROAD
    };
    
    private const int cellSize = 1;
	private const int gridSizeX = 300;
	private const int gridSizeY = 300;
    private bool ignoreMouseUp = false;
    
    private StraightRoad SR;
    private CurvedRoad CR;

    private RoadPlacement currentPlacement;

    void Start () {
        SR = this.GetComponent<StraightRoad>();
        CR = this.GetComponent<CurvedRoad>();
        currentPlacement = SR;
		map = new int[gridSizeX,gridSizeY];

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
				map[i,j] = (int)cellVal.EMPTY;
			}
		}
        selectCell();
    }

	void renderMap(){
		
	}

    private void changeSprite(GameObject roadType)
    {
        GameObject tempGO = Instantiate(roadType, this.GetComponent<Transform>());
        tempGO.name = "Selected";
        SpriteRenderer tempSR = tempGO.GetComponent<SpriteRenderer>();
        tempSR.color = new Color(tempSR.color.r, tempSR.color.g, tempSR.color.b, .5f);
        currentPlacement.setSelected(tempGO);
        ignoreMouseUp = true;
    }

    public void straightRoad(GameObject roadType)
    {
        currentPlacement = SR;
        changeSprite(roadType);
    }

    public void curvedRoad(GameObject roadType)
    {
        currentPlacement = CR;
        changeSprite(roadType);
    }

    private void selectCell()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        mousePos.x = Mathf.RoundToInt(mousePos.x);
        mousePos.y = Mathf.RoundToInt(mousePos.y);

        this.transform.position = mousePos;
        if (!currentPlacement.isBuilding())
        {
            return;
        }

        if (Input.GetMouseButtonUp(0) && ignoreMouseUp)
        {
            ignoreMouseUp = false;
        }
        else
        {
            currentPlacement.Build(mousePos);
        }
    }

    

    void Update ()
    {
        selectCell();
    }
}
