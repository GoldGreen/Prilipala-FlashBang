using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private RectTransform upper;
    [SerializeField] private RectTransform lower;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetProgress(float progress)
    {
        float doorOpenLen = GetScaledValue(progress);
        upper.offsetMin = upper.offsetMin.Change(y: doorOpenLen);
        lower.offsetMax = lower.offsetMax.Change(y: -doorOpenLen);
    }

    private float NormilizeFunc(float progress) => Mathf.Pow(progress, 2.3f);

    private float GetScaledValue(float progress)
    {
        progress = NormilizeFunc(progress);

        float len = progress * rectTransform.sizeDelta.y;//0-350 175-350

        float scale = rectTransform.sizeDelta.y / 700;
        float offset = -175;


        return len * scale - offset;
    }
}
