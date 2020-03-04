using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class RoadPlacement : MonoBehaviour {
    protected GameObject selected = null;
    protected Vector2 startBuildPos;
    protected bool isVertical = false;

    public void setSelected(GameObject selectedIn)
    {
        selected = selectedIn;
    }

    public bool isBuilding()
    {
        return selected != null;
    }

    abstract public void Build(Vector2 mousePos);
}
