﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckBuild : MonoBehaviour
{
    [SerializeField] SetParentScript SetParentScript = null;
    [SerializeField] private Transform[] transforms = new Transform[8];
    private int num;
    readonly List<int> deckList = new List<int>(8);

    private void DeckBuilding()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (num == 0)
                return;

            num--;
            Transforming();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (num == 7)
                return;

            num++;
            Transforming();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (deckList.Count == 8)
                return;

            deckList.Add(num);
            SetParentScript.SetPanel(num);
            ShowListContentsInTheDebugLog(deckList);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            deckList.Clear();
            SetParentScript.DeletePanel();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.sceneLoaded += GameSceneLoaded;
            SceneManager.LoadScene("SampleScene");
        }
    }

    [SerializeField] private GameObject cursor = null;
    private void Transforming()
    {
        cursor.transform.position = transforms[num].transform.position;
    }

    private void GameSceneLoaded(Scene next, LoadSceneMode mode)
    {
        var DeckCreator = GameObject.FindWithTag("GameController").GetComponent<DeckCreator>();
        DeckCreator.deckPlayerSelf = deckList;

        SceneManager.sceneLoaded -= GameSceneLoaded;
    }

    // List -> Debug.Log
    public void ShowListContentsInTheDebugLog<T>(List<T> list)
    {
        string log = "";

        foreach (var content in list.Select((val, idx) => new { val, idx }))
        {
            if (content.idx == list.Count - 1)
                log += content.val.ToString();
            else
                log += content.val.ToString() + ", ";
        }

        Debug.Log(log);
    }

    private void Update()
    {
        DeckBuilding();
    }
}