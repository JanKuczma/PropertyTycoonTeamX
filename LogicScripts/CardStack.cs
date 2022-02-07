using System;
using System.Collections.Generic;

/// <summary>
/// Class for a deck of cards
/// </summary>
public class CardStack
{
private string stackName;
    private List<Card> cards;

    public CardStack(string stackName)
    {
        this.stackName = stackName;
        cards = new List<Card>();
    }

    public void AddCard(Card card)
    {
        cards.Add(card);
    }

    public void ShuffleStack()
    {
        var rng = new Random();
        var n = cards.Count;

        while (n > 1)
        {
            n--;
            var k = rng.Next(n + 1);
            var i = cards[k];
            cards[k] = cards[n];
            cards[n] = i;
        }
    }

    public void PrintCards()
    {
        foreach (var card in cards)
        {
            Console.WriteLine(card.GetName() + " " + card.GetDescription());
        }
    }
}

