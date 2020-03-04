using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpeedLimitUI : MonoBehaviour {

    InputField valText;
    Slider valSlider;
    const int MAXVAL = 200;
    const int defaultVal = 40;
    private GameObject roadTarget = null;
    private RoadInfo roadTargetInfo;
    private const float scale = 10;

    private void Start()
    {
        valText = this.GetComponent<InputField>();
        valText.onEndEdit.AddListener(delegate {
            validateField();
        });
        valText.text = defaultVal.ToString();

        valSlider = transform.GetChild(1).gameObject.GetComponent<Slider>();
        valSlider.maxValue = MAXVAL;
        valSlider.value = defaultVal;
    }

    private void validateField()
    {
        int value = int.Parse(valText.text);
        if (value > MAXVAL)
        {
            value = MAXVAL;
        }
        else if (value < 1)
        {
            value = 1;
        }
        valSlider.value = value;
        valText.text = value.ToString();
        if (roadTarget != null)
        {
            roadTargetInfo.setSpeedLimit(value/scale);
        }
    }
    
    public void changeVal(float val)
    {
        valText.text = val.ToString();
        if (roadTarget != null)
        {
            roadTargetInfo.setSpeedLimit(val / scale);
        }
    }

    public void setTarget(GameObject road)
    {
        if (roadTarget != null && roadTarget != road)
        {
            roadTargetInfo.resetDefaultSprite();
        }
        roadTarget = road;
        roadTargetInfo = roadTarget.GetComponent<RoadInfo>();
        float temp = roadTargetInfo.getSpeedLimit()*scale;
        valSlider.value = temp;
        valText.text = temp.ToString();
    }

    public void interactable(bool val)
    {
        CanvasGroup temp = this.GetComponent<CanvasGroup>();
        if (val)
        {
            temp.interactable = true;
            temp.alpha = 1;
        }
        else
        {
            temp.alpha = 0;
            temp.interactable = false;
        }
    }
}
