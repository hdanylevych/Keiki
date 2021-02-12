using UnityEngine;

public class InputProcessor
{
    [Inject] public IUnityUpdateController UnityUpdateController { get; set; }
    [Inject] public InitiateInteractionSignal InitiateInteractionSignal { get; set; }

    public void Initialize()
    {
        UnityUpdateController.UpdateAgent.OnUpdate += UpdateInput;
    }
    
    private void UpdateInput(float deltaTime)
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended)
            {
                TryToHitSomething(touch.position);
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            TryToHitSomething(Input.mousePosition);
        }
    }

    private void TryToHitSomething(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);

        if (hit.collider != null && hit.collider.TryGetComponent(out IInteractable interactable))
        {
            InitiateInteractionSignal.Dispatch(interactable);
        }
    }
}
