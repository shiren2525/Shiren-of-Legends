using System;
using System.Collections.Generic;
using UnityEngine;

public class DeckCreator : MonoBehaviour
{
    [SerializeField] private CardManager CardManager = null;
    [SerializeField] private GameObject[] FullCards = new GameObject[8];
    public List<int> deckPlayerSelf;

    private void Start()
    {
        InitDeck();
    }

    private void InitDeck()
    {
        var loadText = (Resources.Load("Decks", typeof(TextAsset)) as TextAsset).text;
        string[] spliteText = loadText.Split(',');

        var deckTemplates = new List<int>(8);
        if (deckPlayerSelf.Count == 8)
        {
            deckTemplates = deckPlayerSelf;
        }
        else
        {
            foreach (var num in spliteText)
            {
                deckTemplates.Add(Int32.Parse(num));
            }
        }

        var deckObjects = new List<GameObject>();
        foreach (var num in deckTemplates)
        {
            deckObjects.Add(FullCards[num]);
        }

        CardManager.DeckCreator(deckObjects, deckTemplates.ToArray());
    }
}