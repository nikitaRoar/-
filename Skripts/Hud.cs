﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    private static Hud m_instance;

    [SerializeField] private Text[] m_scoreValue;

    [SerializeField] private Text m_turnsValue;

    [SerializeField] private Slider m_musicSlider;

    [SerializeField] private Slider m_soundSlider;

    [SerializeField] private CanvasGroup m_LevelCompletedWindow;

    private GraphicRaycaster m_raycaster;

   

    public void UpdateOptions()
    {
        m_musicSlider.value = Controller.Instance.Audio.MusicVolume;

        m_soundSlider.value = Controller.Instance.Audio.SfxVolume;
    }

    public void Next()
    {
        Controller.Instance.InitializeLevel();
    }


    public void UpdateTurnsValue(int value)
    {
        m_turnsValue.text = value.ToString();
    }

    public void UpdateScoreValue(int value)
    {
        for (int i = 0; i < m_scoreValue.Length; i++)
        {
            m_scoreValue[i].text = value.ToString();
        }

    }

    public static Hud Instance
    {
        get
        {
            return m_instance;
        }

    }

    private void Awake()
    {
        m_instance = this;
        m_raycaster = gameObject.GetComponent<GraphicRaycaster>();
        Controller.Instance.InitializeLevel();
    }
    public void ShowWindow(CanvasGroup window)
    {
        window.alpha = 1f;

        window.blocksRaycasts = true;

        window.interactable = true;
    }
    public void HideWindow(CanvasGroup window)
    {
        window.alpha = 0f;

        window.blocksRaycasts = false;

        window.interactable = false;
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void Reset()
    {
        Controller.Instance.Reset();
    }
    public void SetMusicVolume(float volume)
    {
        Controller.Instance.Audio.MusicVolume = volume;
    }

    public void SetSoundVolume(float volume)
    {
        Controller.Instance.Audio.SfxVolume = volume;
    }
    private IEnumerator Count(int to, float delay)
    {
        m_raycaster.enabled = false;

        for (int i = 1; i <= to; i++)
        {
            yield return new WaitForSeconds(delay);

            Controller.Instance.m_score.AddTurnBonus();
        }
        DataStore.SaveGame();

        m_raycaster.enabled = true;

    }
    public void CountScore(int to)
    {
        ShowWindow(m_LevelCompletedWindow);

        StartCoroutine(Count(to, 0.3f));
    }
    public void PlayPreviewSound()
    {
        Controller.Instance.Audio.PlaySound("Drop");
    }

}
