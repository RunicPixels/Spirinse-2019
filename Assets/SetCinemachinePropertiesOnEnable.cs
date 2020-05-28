using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using HutongGames.PlayMaker.Actions;
using Spirinse.System;
using Spirinse.Player;
using Spirinse.System.Player;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class SetCinemachinePropertiesOnEnable : MonoBehaviour
{
    public CinemachineVirtualCamera cam;
    public CinemachineConfiner confiner;
    
    private void OnEnable()
    {
        Init();
        SceneManager.sceneLoaded += InitOnSceneChange;
        GameManager.Instance.GameStateChangeEvent += InitStateChange;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= InitOnSceneChange;
        GameManager.Instance.GameStateChangeEvent -= InitStateChange;
    }
    

    private void InitStateChange(GameManager.GameState state) {
        Init();
    }
    
    private void InitOnSceneChange(Scene scene, LoadSceneMode mode) {
        Init();
        InvokeInit();
    }
    [ClickableFunction]
    public void Init()
    {
        if(!cam) cam = GetComponent<CinemachineVirtualCamera>();
        if(!confiner) confiner = GetComponent<CinemachineConfiner>();
        if(!cam.Follow) cam.Follow = PlayerManager.Instance.GetPlayer().defender.transform;
        if(!cam.LookAt) cam.LookAt = PlayerManager.Instance.GetPlayer().defender.tracker.transform;
        if(!confiner.m_BoundingShape2D)confiner.m_BoundingShape2D = GameObject.FindWithTag("CameraBoundaries").GetComponent<Collider2D>();

        if (!cam || !confiner || cam.Follow == null || cam.LookAt == null || confiner.m_BoundingShape2D == null) InvokeInit();
    }
    public void InvokeInit() {
        Invoke("Init", 0.5f);
    } 
}