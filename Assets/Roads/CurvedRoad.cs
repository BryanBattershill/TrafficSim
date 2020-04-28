using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvedRoad : RoadPlacement {


    private Vector2[] diagonalVectors;
    private GameObject dirIndicator;
    private GameObject firstRoad;
    private GameObject secondRoad;
    private int quadrant = 0;
    private int buildingState = 0;

    protected new void Start()
    {
        base.Start();
        diagonalVectors = new Vector2[] {
            (Vector2.right + Vector2.up).normalized,
            (Vector2.left + Vector2.up).normalized,
            (Vector2.left + Vector2.down).normalized,
            (Vector2.right + Vector2.down).normalized};
    }

    override public void Build(Vector2 mousePos)
    {
        if (buildingState == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                selected.transform.parent = null;
                firstRoad = selected;
                startBuildPos = mousePos;
                selected = Instantiate(selected, mousePos, Quaternion.identity);
                secondRoad = selected;
                selected.name = "Selected";
                buildingState = 1;
                isVertical = false;
            }
        }
        else if (buildingState == 1)
        {
            Vector2 temp = (mousePos - startBuildPos);
            Vector2 placement = new Vector2(0, 0);
            if (temp.x >= 0 && temp.y > 0)
            {
                quadrant = 0;
                placement = Vector3.Project(temp, diagonalVectors[0]);
            }
            else if (temp.x < 0 && temp.y >= 0)
            {
                quadrant = 1;
                placement = Vector3.Project(temp, diagonalVectors[1]);
            }
            else if (temp.x <= 0 && temp.y < 0)
            {
                quadrant = 2;
                placement = Vector3.Project(temp, diagonalVectors[2]);
            }
            else if (temp.x > 0 && temp.y <= 0)
            {
                quadrant = 3;
                placement = Vector3.Project(temp, diagonalVectors[3]);
            }
            placement = new Vector2(Mathf.RoundToInt(placement.x), Mathf.RoundToInt(placement.y));
            selected.transform.position = placement + startBuildPos;
            if (Input.GetMouseButtonDown(0) && mousePos.x != startBuildPos.x && mousePos.y != startBuildPos.y)
            {
                buildingState = 2;
                Vector2 vecOffset = Vector2.left;
                if (quadrant == 0 || quadrant == 3)
                {
                    vecOffset = Vector2.right;
                }
                dirIndicator = Instantiate(selected, startBuildPos + vecOffset, Quaternion.identity);
            }
        }
        else if (buildingState == 2)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RoadInfo tempRoad = firstRoad.GetComponent<RoadInfo>();
                tempRoad.setSpeedLimit(4);
                tempRoad.setTurnRadius(Mathf.Abs(secondRoad.transform.position.x - firstRoad.transform.position.x) - 0.5f);
                if ((isVertical && (quadrant == 0 || quadrant == 2)) ||
                    !isVertical && (quadrant == 3 || quadrant == 1))
                {
                    tempRoad.setTurnDirection(-1);
                }
                else if ((isVertical && quadrant == 1 || quadrant == 3) ||
                    !isVertical && quadrant == 0 || quadrant == 2)
                {
                    tempRoad.setTurnDirection(1);
                }
                tempRoad.setInteractable(true);
                tempRoad = secondRoad.GetComponent<RoadInfo>();
                tempRoad.setIsEntrance(false);
                if (isVertical && (quadrant == 0 || quadrant == 3))
                {
                    tempRoad.setExitAngle(0);
                }
                else if (isVertical && (quadrant == 1 || quadrant == 2))
                {
                    tempRoad.setExitAngle(180);
                }
                else if (!isVertical && (quadrant == 0 || quadrant == 1))
                {
                    tempRoad.setExitAngle(90);
                }
                else if (!isVertical && (quadrant == 2 || quadrant == 3))
                {
                    tempRoad.setExitAngle(-90);
                }
                tempRoad.setInteractable(true);
                foreach (GameObject gameIter in new GameObject[] { firstRoad, secondRoad })
                {
                    SpriteRenderer temp = gameIter.GetComponent<SpriteRenderer>();
                    Vector3 tempPos = gameIter.transform.position;
                    temp.color = new Color(temp.color.r, temp.color.g, temp.color.b, 1f);
                }
                Destroy(dirIndicator);
                selected = null;
                buildingState = 0;
                buttonsInteractable(true);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                isVertical = !isVertical;
                Vector2 tempPos = (Vector2)dirIndicator.transform.position - startBuildPos;
                if (quadrant == 0 || quadrant == 2)
                {
                    dirIndicator.transform.position = new Vector2(tempPos.y, tempPos.x) + startBuildPos;
                }
                else if (quadrant == 1 || quadrant == 3)
                {
                    dirIndicator.transform.position = new Vector2(tempPos.y * -1, tempPos.x * -1) + startBuildPos;
                }
            }
        }
    }
}
