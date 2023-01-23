using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance { get; private set; }

    [SerializeField] private RectTransform canvasRectTransform;

    private TextMeshProUGUI textMeshPro;
    private RectTransform backgroundRectTransform;
    private RectTransform rectTransform;
    private TooltipTimer toolTipTimer;

    private Vector2 textPadding = new Vector2(8, 8);

    private void Awake()
    {
        Instance = this;
        rectTransform = GetComponent<RectTransform>();
        textMeshPro = transform.Find("text").GetComponent<TextMeshProUGUI>();
        backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();

        SetText("This is a default tooltip.");
        Hide();
    }

    private void Update()
    {
        rectTransform.anchoredPosition = HandleFollowMouse();

        if (toolTipTimer != null)
        {
            toolTipTimer.timer -= Time.deltaTime;
            if (toolTipTimer.timer <= 0)
            {
                Hide();
            }
        }
    }

    private Vector2 HandleFollowMouse()
    {
        // canvasRectTransform.localScale is uniform so use any of its Vector3 values. This is used to handle the screen size ratio to keep this locked to the mouse position.
        Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;

        // check that the entire tooltip is visible on the right side of the screen
        if (anchoredPosition.x + backgroundRectTransform.rect.width / canvasRectTransform.localScale.x > canvasRectTransform.rect.width)
        {
            anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width / canvasRectTransform.localScale.x;
        }
        // check that the entire tooltip is visible on the top of the screen
        if (anchoredPosition.y + backgroundRectTransform.rect.height / canvasRectTransform.localScale.x > canvasRectTransform.rect.height)
        {
            anchoredPosition.y = canvasRectTransform.rect.height - backgroundRectTransform.rect.height / canvasRectTransform.localScale.x;
        }

        return anchoredPosition;
    }

    private void SetText(string tooltipText)
    {
        textMeshPro.SetText(tooltipText);
        textMeshPro.ForceMeshUpdate();

        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        backgroundRectTransform.sizeDelta = textSize + textPadding;
    }

    public void Show(string tipTipText, TooltipTimer toolTipTimer = null)
    {
        this.toolTipTimer = toolTipTimer;
        gameObject.SetActive(true);
        SetText(tipTipText);
        HandleFollowMouse();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }



    public class TooltipTimer
    {
        public float timer;
    }
}
