using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

public class DiceCatch : MonoBehaviour
{
    private Dice dice1;
    private Dice dice2;
    private void Awake()
    {
        Debug.Log("dice found");
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("dice under table!");
    }
}
