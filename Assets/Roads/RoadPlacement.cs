using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class RoadPlacement : MonoBehaviour {
    protected GameObject selected = null;
    protected Vector2 startBuildPos;
    protected bool isVertical = false;
    private CanvasGroup CG;
    public void setSelected(GameObject selectedIn)
    {
        selected = selectedIn;
    }

    protected void Start() {
        CG = GameObject.Find("ButtonGroup").GetComponent<CanvasGroup>();
    }
    public bool isBuilding()
    {
        return selected != null;
    }

    public void buttonsInteractable(bool val)
    {
        if (val == CG.interactable)
        {
            return;
        }
        if (val)
        {
            CG.interactable = true;
            CG.alpha = 1;
        }
        else
        {
            CG.alpha = 0;
            CG.interactable = false;
        }
    }

    abstract public void Build(Vector2 mousePos);
}
