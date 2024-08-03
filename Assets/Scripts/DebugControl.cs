using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public static class DebugControl 
{

    public static void Log(string msg, int priority) { 
        //TODO: make it enum  
        //0:temp log  1:testing function 2:log error  
        if(priority > 0)
            {
                Debug.Log(msg);
            }
    
    }
}
