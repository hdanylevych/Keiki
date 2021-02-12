using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class GUIOverlayMV : CanvasMV
{
    private readonly string PickBarPrefabLocation = "GUI/PickBarMV";

    private GameObject _pickBarPrefab;
    private GameObject _pickBarLeft;
    private GameObject _pickBarRight;
    private List<BodyPart> _bodyParts;

    [SerializeField] private HintMV _hintMV;

    [Inject] public FindBodyPartScenario FindBodyPartScenario { get; set; } 
    [Inject] public NoInteractionsSignal NoInteractionsSignal { get; set; }

    private GameObject PickBarPrefab
    {
        get
        {
            if (_pickBarPrefab == null)
            {
                _pickBarPrefab = Resources.Load<GameObject>(PickBarPrefabLocation);

                if (_pickBarPrefab == null)
                {
                    Debug.LogError($"GUIOverlayMV: cannot locate prefab by path: {PickBarPrefabLocation}");
                }
            }

            return _pickBarPrefab;
        }
    }

    [PostConstruct]
    private void Initialize()
    {
        NoInteractionsSignal.AddListener(SpawnHint);
    }

    private void Start()
    {
        base.Start();

        _pickBarLeft = Instantiate(PickBarPrefab, transform);
        _pickBarLeft.name = "PickBarLeftMV";

        var rectTransform = _pickBarLeft.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(-(Screen.safeArea.width / 2f), rectTransform.anchoredPosition.y);

        _pickBarRight = Instantiate(PickBarPrefab, transform);
        _pickBarRight.name = "PickBarRightMV";

        rectTransform = _pickBarRight.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(Screen.safeArea.width / 2f, rectTransform.anchoredPosition.y);

        InitializePickBars();

        _bodyParts = GetComponentsInChildren<BodyPart>().ToList();
    }

    private void SpawnHint()
    {
        BodyPart bodyPart = GetOptionWithCorrectBodyPart();

        _hintMV.PointToTarget(bodyPart.GetComponent<RectTransform>());
    }

    private BodyPart GetOptionWithCorrectBodyPart()
    {
        var missingBodyPartType = FindBodyPartScenario.Model.MissingBodyPartType;
        var properUnitType = FindBodyPartScenario.Model.Type;

        BodyPart bodyPart = _bodyParts.First((part) => part.ProperUnitType == properUnitType && part.BodyPartType == missingBodyPartType);

        return bodyPart;
    }

    private void InitializePickBars()
    {
        _pickBarLeft.GetComponent<PickBarMV>().Initialize<BodyPart>(
            new BodyPartParameters()
                {
                    BodyPartType = BodyPartType.Tail,
                    ProperUnitType = UnitType.Cat
                },
            new BodyPartParameters()
                {
                    BodyPartType = BodyPartType.Tail,
                    ProperUnitType = UnitType.Pig
                },
            new BodyPartParameters()
                {
                    BodyPartType = BodyPartType.Tail,
                    ProperUnitType = UnitType.Dog
                });

        _pickBarRight.GetComponent<PickBarMV>().Initialize<BodyPart>(
            new BodyPartParameters()
                {
                    BodyPartType = BodyPartType.Tail,
                    ProperUnitType = UnitType.Cow
                },
            new BodyPartParameters()
                {
                    BodyPartType = BodyPartType.Tail,
                    ProperUnitType = UnitType.Horse
                },
            new BodyPartParameters()
                {
                    BodyPartType = BodyPartType.Tail,
                    ProperUnitType = UnitType.Mouse
                });
    }
}
