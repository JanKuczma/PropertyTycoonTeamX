using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermObject : MonoBehaviour
{
    //any stuff that we want to not be deleted when changing scenes
    public List<Model.Player> players;
    public bool starWarsTheme = false;
    public bool customData = false;
    
    void Awake()
    {
        players = new List<Model.Player>();
        GameObject[] obj = GameObject.FindGameObjectsWithTag("PreGameData");
        if(obj != null)
        {
            if (obj.Length > 1)
            {
                Destroy(obj[0]);
            }
        }
        
        DontDestroyOnLoad(this.gameObject);
    }
    //any stuff that we want to be continuous (not affected by scene changes)
}
