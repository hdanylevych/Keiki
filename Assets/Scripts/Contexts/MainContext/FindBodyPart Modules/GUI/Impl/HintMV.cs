using strange.extensions.mediation.impl;

using UnityEngine;

public class HintMV : View
{
    private RectTransform _rectTransform;

    [Inject] public CorrectAnswerSignal CorrectAnswerSignal { get; set; }

    private RectTransform RectTransform
    {
        get
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();

                if (_rectTransform == null)
                {
                    Debug.LogError("HintMV: couldn't get RectTransform component");
                }
            }

            return _rectTransform;
        }
    }

    [PostConstruct]
    private void Initialize()
    {
        CorrectAnswerSignal.AddListener(() => gameObject.SetActive(false));
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        var localScale = (Screen.width / 12f) / RectTransform.rect.width;

        RectTransform.localScale = new Vector2(localScale, localScale);
    }

    public void PointToTarget(RectTransform targetRectTransform)
    {
        RectTransform.position = targetRectTransform.position;
        
        gameObject.SetActive(true);
    }
}
