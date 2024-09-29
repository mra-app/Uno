using UnityEngine;

public class DontDistroy : MonoBehaviour
{
    public static DontDistroy Instance;
    // Start is called before the first frame update
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
