using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightRoad : RoadPlacement {

    private Vector2 lastMousePos;
    private Stack<GameObject> buildingGhosts = new Stack<GameObject>(100);

    protected new void Start()
    {
        base.Start();
    }

    override public void Build(Vector2 mousePos)
    {
        if (Input.GetMouseButtonDown(0))
        {
            selected.transform.parent = null;
            startBuildPos = mousePos;
            buildingGhosts.Push(selected);
            lastMousePos = mousePos;
        }
        else if (Input.GetMouseButton(0))
        {
            if (mousePos == lastMousePos)
            {
                return;
            }
            if (lastMousePos == startBuildPos)
            {
                if (mousePos.x != lastMousePos.x)
                {
                    isVertical = false;
                }
                else if (mousePos.y != lastMousePos.y)
                {
                    isVertical = true;
                }
            }
            //Switch from y direction to x direction
            if (Mathf.Abs(mousePos.x - startBuildPos.x) > Mathf.Abs(mousePos.y - startBuildPos.y) && isVertical && buildingGhosts.Count > 1)
            {
                isVertical = false;
                Stack<GameObject> newOrientation = new Stack<GameObject>(buildingGhosts.Count);

                int factor = 0;
                int dir = 1;
                if (mousePos.x < startBuildPos.x)
                {
                    dir = -1;
                }
                bool firstRun = true;
                while (buildingGhosts.Count != 0)
                {
                    GameObject newPosObj = buildingGhosts.Pop();
                    if (firstRun)
                    {
                        selected = newPosObj;
                        selected.name = "Selected";
                        firstRun = false;
                    }
                    newPosObj.transform.position = startBuildPos + Vector2.right * factor * dir;
                    factor += 1;
                    newOrientation.Push(newPosObj);
                }
                buildingGhosts = newOrientation;
            }//Switch from x direction to y direction
            else if (Mathf.Abs(mousePos.y - startBuildPos.y) > Mathf.Abs(mousePos.x - startBuildPos.x) && !isVertical && buildingGhosts.Count > 1)
            {
                isVertical = true;
                Stack<GameObject> newOrientation = new Stack<GameObject>(buildingGhosts.Count);

                int factor = 0;
                int dir = 1;
                if (mousePos.y < startBuildPos.y)
                {
                    dir = -1;
                }

                bool firstRun = true;
                while (buildingGhosts.Count != 0)
                {
                    GameObject newPosObj = buildingGhosts.Pop();
                    if (firstRun)
                    {
                        selected = newPosObj;
                        selected.name = "Selected";
                        firstRun = false;
                    }
                    newPosObj.transform.position = startBuildPos + Vector2.up * factor * dir;
                    factor += 1;
                    newOrientation.Push(newPosObj);
                }
                buildingGhosts = newOrientation;
            }

            GameObject currentCheck = buildingGhosts.Peek();
            float currMousePos = mousePos.x;
            float currStartBuildPos = startBuildPos.x;
            float currCurrentCheckPos = currentCheck.transform.position.x;
            Vector3 vectorTransform = Vector3.right;

            if (isVertical)
            {
                currMousePos = mousePos.y;
                currStartBuildPos = startBuildPos.y;
                currCurrentCheckPos = currentCheck.transform.position.y;
                vectorTransform = Vector3.up;
            }
            while (currCurrentCheckPos != currMousePos)
            {

                if (currMousePos > currStartBuildPos)
                {
                    if (currCurrentCheckPos < currStartBuildPos)
                    {
                        Destroy(buildingGhosts.Pop());
                    }
                    else if (currMousePos > currCurrentCheckPos)
                    {
                        buildingGhosts.Push(Instantiate(selected, currentCheck.transform.position + vectorTransform, Quaternion.identity));
                    }
                    else
                    {
                        Destroy(buildingGhosts.Pop());
                    }
                }
                else if (currMousePos < currStartBuildPos)
                {
                    if (currCurrentCheckPos > currStartBuildPos)
                    {
                        Destroy(buildingGhosts.Pop());
                    }
                    else if (currMousePos < currCurrentCheckPos)
                    {
                        buildingGhosts.Push(Instantiate(selected, currentCheck.transform.position - vectorTransform, Quaternion.identity));
                    }
                    else
                    {
                        Destroy(buildingGhosts.Pop());
                    }
                }
                else
                {
                    Destroy(buildingGhosts.Pop());
                }
                currentCheck = buildingGhosts.Peek();
                currCurrentCheckPos = currentCheck.transform.position.x;
                if (isVertical)
                {
                    currCurrentCheckPos = currentCheck.transform.position.y;
                }
            }
            lastMousePos = mousePos;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            selected = null;
            RoadInfo prevRoad = null;
            while (buildingGhosts.Count != 0)
            {
                GameObject tempObj = buildingGhosts.Pop();
                Vector3 tempPos = tempObj.transform.position;
                tempObj.transform.position = new Vector3(Mathf.RoundToInt(tempPos.x), Mathf.RoundToInt(tempPos.y), Mathf.RoundToInt(tempPos.z));
                SpriteRenderer temp = tempObj.GetComponent<SpriteRenderer>();
                temp.color = new Color(temp.color.r, temp.color.g, temp.color.b, 1f);
                RoadInfo currRoad = tempObj.GetComponent<RoadInfo>();
                currRoad.setInteractable(true);
                if (prevRoad != null)
                {
                    currRoad.setGroupMember(0, prevRoad);
                    prevRoad.setGroupMember(1, currRoad);
                }
                prevRoad = currRoad;
            }
            buttonsInteractable(true);
        }
    }
}
