using Spine;
using Spine.Unity;
using strange.extensions.mediation.impl;
using System.Linq;
using UnityEngine;

public class UnitView : View, IInteractable
{
    private readonly string UnitsAnimationReferencesLocation = "Configurations/UnitsAnimationReferences";

    private UnitsAnimationReferences _unitAnimationReferences;
    private IUnitModel _model;

    private float _animationLength;
    private GameObject _animationInstance;
    private SkeletonGraphic _skeletonGraphic;

    [Inject] public CorrectAnswerSignal CorrectAnswerSignal { get; set; }
    [Inject] public IncorrectAnswerSignal IncorrectAnswerSignal { get; set; }
    [Inject] public UnitSequenceCompleteSignal UnitSequenceCompleteSignal { get; set; }

    private UnitsAnimationReferences UnitsAnimationReferences
    {
        get
        {
            if (_unitAnimationReferences == null)
            {
                _unitAnimationReferences = Resources.Load<UnitsAnimationReferences>(UnitsAnimationReferencesLocation);

                if (_unitAnimationReferences == null)
                {
                    Debug.LogError($"UnitView: cannot locate object by path: {UnitsAnimationReferencesLocation}");
                }
            }

            return _unitAnimationReferences;
        }
    }

    [PostConstruct]
    private void Construct()
    {
        CorrectAnswerSignal.AddListener(OnCorrectAnswer);
        IncorrectAnswerSignal.AddListener(OnIncorrectAnswer);
    }

    public void Initialize(IUnitModel model)
    {
        _model = model;
        _model.OnTypeChanged += InstantiateNewUnitAnimation;
        
        var rectTransform = gameObject.AddComponent<RectTransform>();
        rectTransform.anchorMax = new Vector2(.7f, .8f);
        rectTransform.anchorMin = new Vector2(.3f, .2f);
        rectTransform.anchoredPosition = Vector2.zero;
        
        var collider = gameObject.AddComponent<BoxCollider2D>();
        collider.size = rectTransform.rect.size / 2;
        collider.offset = new Vector2(0, -(rectTransform.rect.size.y / 4));

        InstantiateNewUnitAnimation();
    }

    public void Interact()
    {
        if (_skeletonGraphic.SkeletonData.FindAnimation("Tap") != null)
        {
            _skeletonGraphic.AnimationState.SetAnimation(1, "Tap", false);
        }
    }

    private void InstantiateNewUnitAnimation()
    {
        if (_animationInstance != null)
        {
            Destroy(_animationInstance);
        }

        var animationPrefab = UnitsAnimationReferences.animationReferences.First((reference) => reference.UnitType == _model.Type);

        _animationInstance = Instantiate(animationPrefab.AnimationUI, transform);

        var rectTransform = _animationInstance.GetComponent<RectTransform>();
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0f, 0f);
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.localScale = new Vector3(3, 3, 3);

        _skeletonGraphic = _animationInstance.GetComponent<SkeletonGraphic>();
        _skeletonGraphic.AnimationState.Complete += SetLoopingAnimationWithUnitState;

        // scale unit by screen dimensions
        float localScale = (Screen.width / 4f) / _skeletonGraphic.SkeletonData.Width;
        rectTransform.localScale = new Vector3(localScale, localScale);

        SetSkeletonMissingBodyPartColor(0);
        SetLoopingAnimationWithUnitState();
    }

    private void Update()
    {
        _skeletonGraphic.Update(Time.deltaTime);
    }

    private void SetSkeletonMissingBodyPartColor(float color)
    {
        foreach (var slot in _skeletonGraphic.Skeleton.Slots)
        {
            if (slot.Data.Name.Contains(_model.MissingBodyPartType.ToString()))
            {
                slot.A = color;
                break;
            }
        }
    }

    private void SetLoopingAnimationWithUnitState(TrackEntry track = null)
    {
        string state = _model.State.ToString();

        if (_skeletonGraphic.SkeletonData.FindAnimation(state) != null)
        {
            _skeletonGraphic.AnimationState.SetAnimation(1, state, true);
        }
        else if (_skeletonGraphic.SkeletonData.FindAnimation(state.ToLower()) != null)
        {
            _skeletonGraphic.AnimationState.SetAnimation(1, state.ToLower(), true);

        }
    }

    private void OnIncorrectAnswer()
    {
        if (_skeletonGraphic.SkeletonData.FindAnimation("No") != null)
        {
            _skeletonGraphic.AnimationState.SetAnimation(1, "No", false);
        }
        else if (_skeletonGraphic.SkeletonData.FindAnimation("Idle_say_no") != null)
        {
            _skeletonGraphic.AnimationState.SetAnimation(1, "Idle_say_no", false);
        }
    }

    private void OnCorrectAnswer()
    {
        SetSkeletonMissingBodyPartColor(1);
        
        _skeletonGraphic.AnimationState.Complete -= SetLoopingAnimationWithUnitState;
        _skeletonGraphic.AnimationState.Complete += (track) => UnitSequenceCompleteSignal.Dispatch();
        
        if (_skeletonGraphic.SkeletonData.FindAnimation("Jump") != null)
        {
            _skeletonGraphic.AnimationState.SetAnimation(1, "Jump", false);
        }
        else if (_skeletonGraphic.SkeletonData.FindAnimation("Dance") != null)
        {
            _skeletonGraphic.AnimationState.SetAnimation(1, "Dance", false);
        }
    }
}
