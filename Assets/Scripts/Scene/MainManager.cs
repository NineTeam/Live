using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mySpace;

public class MainManager : System.Object {

    private static MainManager instance;
    
    public ISceneController currentSceneController
    {
        get;
        set;
    }

    public static MainManager GetInstance()
    {
        if (instance == null)
        {
            instance = new MainManager();
        }

        return instance;
        
    }

}
