using System;
using UnityEngine;

public class UnityUpdateAgent : MonoBehaviour
{
    public event Action<float> OnUpdate; 
    public event Action<float> OnFixedUpdate;
    public event Action<float> OnLateUpdate;

    public static UnityUpdateAgent Create(GameObject root = null)
    {
        var gameObject = new GameObject("Unity Update Agent");

        if (root != null)
        {
            gameObject.transform.parent = root.transform;
            gameObject.transform.localPosition = Vector3.zero;
        }

        var updateAgent = gameObject.AddComponent<UnityUpdateAgent>();

        return updateAgent;
    }

    private void Update()
    {
        OnUpdate?.Invoke(Time.deltaTime);
    }

    private void LateUpdate()
    {
        OnLateUpdate?.Invoke(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        OnFixedUpdate?.Invoke(Time.fixedDeltaTime);
    }
}
