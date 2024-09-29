using UnityEngine;

public static class DebugControl 
{

    public static void Log(string msg, int priority) {  
        if(priority > 0)
            {
                Debug.Log(msg);
            }
    
    }
}
