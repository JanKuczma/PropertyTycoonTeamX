using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model{
public class PlayerData
{
public string name;
public Token token;
    public PlayerData(string name, Token token)
    {
        this.name = name;
        this.token = token;
    }
}
}