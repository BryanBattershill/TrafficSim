using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class RoadInfo : MonoBehaviour {
    private float turnRadius;
    private int turnDirection;
    private float speedLimit = 4;
    private bool isEntranceVal = true;
    private int exitAngle = 0;
    private Vector2 exitPos;
    private GameObject speedLimitChanger;
    public Sprite selectedSprite;
    public Sprite unselectedSprite;
    public bool selecting = false;
    public bool hovering = false;

    private RoadInfo[] links = new RoadInfo[2];

    private void Start()
    {
        speedLimitChanger = GameObject.Find("SpeedLimitInputField");
    }

    void OnMouseOver()
    {
        if (!selecting && Input.GetMouseButtonUp(0))
        {
            this.GetComponent<SpriteRenderer>().sprite = selectedSprite;
            Vector3 currPos = this.transform.position;
            this.transform.position = new Vector3(currPos.x, currPos.y, -1);
            Vector3 UIPos = Camera.main.WorldToScreenPoint(this.transform.position);
            float xOffset = 0;
            float yOffset = 0;

            if (UIPos.x > 904)
            {
                xOffset = 904 - UIPos.x;
            }
            if (UIPos.y > 493)
            {
                yOffset = 493 - UIPos.y;
            }

            UIPos.x += xOffset;
            UIPos.y += yOffset;

            speedLimitChanger.transform.position = UIPos;
            SpeedLimitUI temp = speedLimitChanger.GetComponent<SpeedLimitUI>();
            temp.setTarget(this.gameObject);
            temp.interactable(true);
            selecting = true;
        }
        hovering = true;
    }

    public void Update()
    {
        if (selecting && (Input.GetMouseButtonDown(1) || (!hovering && Input.GetMouseButtonDown(0) && EventSystem.current.currentSelectedGameObject == null)))
        {
            selecting = false;
            speedLimitChanger.GetComponent<SpeedLimitUI>().interactable(false);
            this.GetComponent<SpriteRenderer>().sprite = unselectedSprite;
            Vector3 currPos = this.transform.position;
            this.transform.position = new Vector3(currPos.x, currPos.y, 0);
        }
        hovering = false;
    }

    private float Truncate(float val, int digits)
    {
        float mult = Mathf.Pow(10, digits);
        float result = Mathf.Round(mult * val) / mult;
        return result;
    }

    public void setTurnRadius(float setVal)
    {
        turnRadius = setVal;
    }

    public void setTurnDirection(int setVal)
    {
        turnDirection = setVal;
    }

    public void setSpeedLimit(float setVal)
    {
        speedLimit = setVal;
    }

    public void setExitAngle(int angle)
    {
        exitAngle = angle;
        if (angle == 0) {
            exitPos = this.transform.position + new Vector3(-0.5f,0);
        }
        else if (angle == 90)
        {
            exitPos = this.transform.position + new Vector3(0, -0.5f);
        }
        else if (angle == 180)
        {
            exitPos = this.transform.position + new Vector3(0.5f, 0);
        }
        else if (angle == -90)
        {
            exitPos = this.transform.position + new Vector3(0, 0.5f);
        }
        
        exitPos.x = Truncate(exitPos.x, 1);
        exitPos.y = Truncate(exitPos.y, 1);

    }

    public void setIsEntrance(bool entVal)
    {
        isEntranceVal = entVal;
    }

    public void setInteractable(bool val)
    {
        this.GetComponent<BoxCollider2D>().enabled = val;
    }

    public float getTurnRadius()
    {
        return turnRadius;
    }

    public float getTurnDirection()
    {
        return turnDirection;
    }

    public float getTurnRadiusDir()
    {
        return turnDirection * turnRadius;
    }

    public float getSpeedLimit()
    {
        return speedLimit;
    }

    public int getExitAngle()
    {
        return exitAngle;
    } 

    public Vector3 getExitPos()
    {
        return exitPos;
    }

    public bool isEntrance()
    {
        return isEntranceVal;
    }
    
    public void resetDefaultSprite()
    {
        this.GetComponent<SpriteRenderer>().sprite = unselectedSprite;
    }
}
