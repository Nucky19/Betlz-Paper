using UnityEngine;
public class GlobalGameManager : MonoBehaviour
{
    public static GlobalGameManager Instance;

    public int globalCraneCount;
    public int globalDeadCount;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
