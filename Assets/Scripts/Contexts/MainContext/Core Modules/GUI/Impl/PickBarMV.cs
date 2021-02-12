using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class PickBarMV : CanvasMV
{
    private List<Image> _options;

    [SerializeField] private RectTransform _optionsPanelRectTransform;

    public void Initialize<T>(params object[] parameters) where T : MonoBehaviour, IPickOption
    {
        _options = _optionsPanelRectTransform.gameObject.GetComponentsInChildren<Image>().ToList();

        for (int i = 0; i < _options.Count; i++)
        {
            var newComponent = _options[i].gameObject.AddComponent<T>();

            if (newComponent is IPickOption pickOption && parameters.Length > i)
            {
                pickOption.Initialize(parameters[i]);
            }
        }
    }

    private void Start()
    {
        base.Start();

        var rect = GetComponent<RectTransform>();

        if (rect.anchoredPosition.x < 0)
        {
            _optionsPanelRectTransform.anchoredPosition = new Vector2(rect.rect.width / 4, 0);
        }
        else
        {
            _optionsPanelRectTransform.anchoredPosition = new Vector2(-(rect.rect.width / 5), 0);
        }
    }
}
