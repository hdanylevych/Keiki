using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;

public class BodyPart : View, IBodyPart, IPickOption
{
    private readonly Color SelectedColor = Color.green;
    private readonly string BodyPartSpriteLocation = "Sprites/";

    private BodyPartParameters _bodyPartParameters;
    private Image _sprite;
    private bool _isSelected;

    public BodyPartType BodyPartType => _bodyPartParameters.BodyPartType;
    public UnitType ProperUnitType => _bodyPartParameters.ProperUnitType;

    [Inject] public InitiateInteractionSignal InitiateInteractionSignal { get; set; }
    [Inject] public UnitSequenceCompleteSignal UnitSequenceCompleteSignal { get; set; }

    private bool IsSelected
    {
        get => _isSelected;

        set
        {
            _isSelected = value;

            if (_isSelected == false)
            {
                _sprite.color = Color.white;
            }
            else
            {
                _sprite.color = SelectedColor;
            }
        }
    }

    [PostConstruct]
    private void Construct()
    {
        InitiateInteractionSignal.AddListener((interactable) => IsSelected = false);
        UnitSequenceCompleteSignal.AddListener(() => IsSelected = false);
    }

    public void Initialize(object parameters)
    {
        if (parameters is BodyPartParameters bodyParams)
        {
            _bodyPartParameters = bodyParams;
        }
    }

    private void Start()
    {
        base.Start();

        if (TryGetComponent(out _sprite))
        {
            string path = BodyPartSpriteLocation + BodyPartType + "/" + ProperUnitType;
            
            _sprite.sprite = Resources.Load<Sprite>(path);
        }
        else
        {
            Debug.LogError("BodyPart: couldn't get Image component.");
        }

        var collider = gameObject.AddComponent<BoxCollider2D>();

        collider.size = GetComponent<RectTransform>().rect.size;
    }

    public void Interact()
    {
        IsSelected = true;
    }
}
