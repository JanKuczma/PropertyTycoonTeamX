using System;
using System.Collections.Generic;

/// <summary>
/// Class for a deck of cards
/// </summary>
namespace Model{
[System.Serializable]
public class CardStack
{
    public List<Card> cards;
    int next = 0;

    public CardStack()
    {
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

    public Card PopCard()
    {
        Card c = cards[next];
        next = (next+1)%cards.Count;
        return c;   
    }
}
}