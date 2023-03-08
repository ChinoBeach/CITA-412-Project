using UnityEngine;

/// <summary>
/// Just don't destroy on load.
/// </summary>
public class ScenePersist : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
