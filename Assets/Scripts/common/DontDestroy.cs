using UnityEngine;

public class DontDistroy : MonoBehaviour
{
    public static DontDistroy Instance;
    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
