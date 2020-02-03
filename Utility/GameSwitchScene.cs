﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSwitchScene : MonoBehaviour
{
    
    public void SwitchToScene(int index) {
        SceneManager.LoadScene(index);
    }

    public void QuitGame() {
        Application.Quit();
    }
}