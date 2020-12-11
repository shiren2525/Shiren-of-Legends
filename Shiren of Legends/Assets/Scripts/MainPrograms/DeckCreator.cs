using System;
using System.Collections.Generic;
using UnityEngine;

public class DeckCreator : MonoBehaviour
{
    [SerializeField] private CardManager CardManager = null;
    [SerializeField] private InfoPanel InfoPanel = null;
    [SerializeField] private GameObject[] FullCards = new GameObject[(int)EnumNumbers.FullCards];
    public List<int> deckPlayerSelf;

    private void Start()
    {
        InitDeck();
    }

    private void InitDeck()
    {
        var deckIDs = new List<int>((int)EnumNumbers.Cards);
        if (deckPlayerSelf.Count == (int)EnumNumbers.Cards)
        {
            deckIDs = deckPlayerSelf;
        }
        else
        {
            // use Template file
            var loadText = (Resources.Load("Decks", typeof(TextAsset)) as TextAsset).text;
            string[] spliteText = loadText.Split(',');
            foreach (var num in spliteText)
            {
                deckIDs.Add(Int32.Parse(num));
            }
        }

        var deckObjects = new List<GameObject>();
        foreach (var num in deckIDs)
        {
            deckObjects.Add(FullCards[num]);
        }

        CardManager.DeckCreator(deckObjects.ToArray(), deckIDs.ToArray());
        InfoPanel.Init(deckIDs.ToArray());
    }
}