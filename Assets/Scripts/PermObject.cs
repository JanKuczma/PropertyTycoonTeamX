using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermObject : MonoBehaviour
{
    //any stuff that we want to not be deleted when changing scenes
    public List<Model.Player> players;
    
    void Awake()
    {
        players = new List<Model.Player>();
        DontDestroyOnLoad(this.transform.gameObject);
    }
    //any stuff that we want to be continuous (not affected by scene changes)
}
