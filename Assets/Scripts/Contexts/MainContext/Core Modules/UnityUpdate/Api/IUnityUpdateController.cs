public interface IUnityUpdateController
{
    UnityUpdateAgent UpdateAgent { get; }

    void Initialize(UnityUpdateAgent updateAgent);
}
