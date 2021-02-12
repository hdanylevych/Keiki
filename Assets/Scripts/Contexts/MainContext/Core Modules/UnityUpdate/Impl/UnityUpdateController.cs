public class UnityUpdateController : IUnityUpdateController
{
    private UnityUpdateAgent _updateAgent;

    public UnityUpdateAgent UpdateAgent => _updateAgent;

    public void Initialize(UnityUpdateAgent updateAgent)
    {
        _updateAgent = updateAgent;
    }
}