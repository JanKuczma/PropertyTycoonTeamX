using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using view.Sound;


public class ButtonClicker : MonoBehaviour
{
    private SoundManager soundManager;
    private Button button;
    
    private void Awake()
    {
        soundManager = GameObject.FindGameObjectWithTag("GameMusic").GetComponent<SoundManager>();
        button = GetComponent<Button>();
        button.onClick.AddListener(soundManager.PlayClickSound);
    }
}
