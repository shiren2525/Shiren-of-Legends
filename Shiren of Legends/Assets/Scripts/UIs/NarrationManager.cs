using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class NarrationManager : MonoBehaviour
{
    [SerializeField] List<string> narrationList = new List<string>();
    [SerializeField] Text NarrationText = null;
    [SerializeField] List<string> showSerifList = new List<string>();

    private void Start()
    {
        CreateNarrationList();
    }

    private void CreateNarrationList()
    {
        var loadText = (Resources.Load("NarrationList", typeof(TextAsset)) as TextAsset).text;
        string[] spliteText = loadText.Split('\n');

        foreach (var str in spliteText)
        {
            narrationList.Add(str);
        }
    }

    public void SetSerif(int number)
    {
        showSerifList.Add(narrationList[number]);

        if (showSerifList.Count == 1)
        {
            BeginText(0);
        }
    }

    public async void BeginText(int num)
    {
        NarrationText.text = showSerifList[num];

        await Task.Delay(TimeSpan.FromSeconds(0.5));

        if (showSerifList.Count > num + 1 && showSerifList.Count < 8)
        {
            BeginText(++num);
        }
        else
        {
            NarrationText.text = "";
            showSerifList.Clear();
        }
    }
}