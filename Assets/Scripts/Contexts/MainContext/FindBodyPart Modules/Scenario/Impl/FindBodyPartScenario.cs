using System;
using System.Collections.Generic;
using UnityEngine;

public class FindBodyPartScenario : Scenario
{
    private const string FindBodyPartGameLocation = "MiniGames/FindBodyPartGame";
    private const string MainQuestionClipLocation = "Sounds/Voices/Units/";
    private const int NumberOfIncorrectAnswersToSad = 2;
    private const float SecondsWithoutInteractions = 7f;

    private Queue<UnitType> _unitsQueue;
    private UnitModel _model;
    private int _incorrectAnswersCount = 0;
    private float _timer;

    public override string MiniGameLocation => FindBodyPartGameLocation;

    public int IncorrectAnswersCount
    {
        get => _incorrectAnswersCount;

        set
        {
            _incorrectAnswersCount = value;

            if (_incorrectAnswersCount > NumberOfIncorrectAnswersToSad)
            {
                _model.State = UnitState.Sad;
            }
            else
            {
                _model.State = UnitState.Idle;
            }
        }
    }

    public IUnitModel Model => _model;

    [Inject] public IAudioManager AudioManager { get; set; }
    [Inject] public IUnityUpdateController UnityUpdateController { get; set; }
    [Inject] public BodyPartFoundSignal BodyPartFoundSignal { get; set; }
    [Inject] public CorrectAnswerSignal CorrectAnswerSignal { get; set; }
    [Inject] public IncorrectAnswerSignal IncorrectAnswerSignal { get; set; }
    [Inject] public UnitSequenceCompleteSignal UnitSequenceCompleteSignal { get; set; }
    [Inject] public NoInteractionsSignal NoInteractionsSignal { get; set; }
    [Inject] public InitiateInteractionSignal InitiateInteractionSignal { get; set; }

    [PostConstruct]
    private void Construct()
    {
        BodyPartFoundSignal.AddListener(OnBodyPartFound);
        UnitSequenceCompleteSignal.AddListener(MoveToTheNextUnit);
        InitiateInteractionSignal.AddListener((interactable) => _timer = 0f);

        UnityUpdateController.UpdateAgent.OnUpdate += Update;
    }

    public override void Start()
    {
        _isActive = true;

        InitializeUnitsQueue();

        _model = new UnitModel(_unitsQueue.Dequeue(), BodyPartType.Tail);
        AskMainQuestion();
    }

    private void Update(float deltaTime)
    {
        if (_timer > SecondsWithoutInteractions)
        {
            _timer = 0;
            NoInteractionsSignal.Dispatch();
        }

        _timer += deltaTime;
    }

    private void InitializeUnitsQueue()
    {
        Array unitTypeValues = Enum.GetValues(typeof(UnitType));
        
        _unitsQueue = new Queue<UnitType>(unitTypeValues.Length);

        foreach (UnitType value in unitTypeValues)
        {
            _unitsQueue.Enqueue(value);
        }
    }

    private void OnBodyPartFound(BodyPartParameters parameters)
    {
        if (parameters.ProperUnitType == Model.Type && parameters.BodyPartType == Model.MissingBodyPartType)
        {
            IncorrectAnswersCount = 0;
            CorrectAnswerSignal.Dispatch();
        }
        else
        {
            IncorrectAnswersCount += 1;
            IncorrectAnswerSignal.Dispatch();
        }
    }

    private void MoveToTheNextUnit()
    {
        if (_unitsQueue.Count <= 0)
        {
            Debug.Log("End of the game");
            InvokeOnComplete();
            return;
        }

        _model.Type = _unitsQueue.Dequeue();
        _model.State = UnitState.Idle;
        
        AskMainQuestion();
    }

    private void AskMainQuestion()
    {
        var clip = Resources.Load<AudioClip>(MainQuestionClipLocation + Model.Type.ToString().ToUpperInvariant() + "_09");

        AudioManager.PlayOneShot(clip);
    }
}
