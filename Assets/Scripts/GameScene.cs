using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class GameScene : MonoBehaviour
{
    private CanvasGroup fadeGroup;
    private float fadeInDuration = 2;
    private bool gameStarted;

    public Transform arrow;
    private Transform playerTransform;
    public Objective objective;

    private void Start()
    {
        bool canshowad;
        canshowad = true;
        if (canshowad)
        {
            if (Advertisement.IsReady())
            {
                Advertisement.Show();
                canshowad = false;
            }

        }

        playerTransform = FindObjectOfType<PlayerMotor>().transform;

        SceneManager.LoadScene(Manager.Instance.currentLevel.ToString(), LoadSceneMode.Additive);

        fadeGroup = FindObjectOfType<CanvasGroup>();

        fadeGroup.alpha = 1;

        Manager.Instance.startMusic.Pause();
    }

    private void Update()
    {
        if(objective != null)
        {
            Vector3 dir = playerTransform.InverseTransformPoint(objective.GetCurrentRing().position);
            float a = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            a += 180;
            arrow.transform.localEulerAngles = new Vector3(0, 180, a);
        }

        if(Time.timeSinceLevelLoad <= fadeInDuration)
        {
            fadeGroup.alpha = 1 - (Time.timeSinceLevelLoad / fadeInDuration);
        }
        else if(!gameStarted)
        {
            fadeGroup.alpha = 0;
            gameStarted = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitScene();
        }
    }

    public void CompleteLevel()
    {
        SaveManager.Instance.CompleteLevel(Manager.Instance.currentLevel);
        Manager.Instance.menuFocus = 1;
        ExitScene();
    }

    public void ExitScene()
    {
        SceneManager.LoadScene("Menu");
        Manager.Instance.startMusic.UnPause();
    }
}
