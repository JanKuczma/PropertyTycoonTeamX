using System;
using System.Collections.Generic;

namespace Model{
/// <summary>
/// Class for a deck of cards
/// </summary>
[System.Serializable]
public class CardStack
{
    /// <summary>
    /// List of cards in the deck
    /// </summary>
    public List<Card> cards;
    /// <summary>
    /// Index of the card to be taken from the deck
    /// </summary>
    int next = 0;

    public CardStack()
    {
        cards = new List<Card>();
    }
    /// <summary>
    /// Adds card to the deck
    /// </summary>
    /// <param name="card"></param>
    public void AddCard(Card card)
    {
        cards.Add(card);
    }

    /// <summary>
    /// Shuffles the deck of cards
    /// </summary>
    public void ShuffleStack()
    {
        var rng = new Random();
        var n = cards.Count;
        next = 0;
        while (n > 1)
        {
            n--;
            var k = rng.Next(n + 1);
            var i = cards[k];
            cards[k] = cards[n];
            cards[n] = i;
        }
    }
    /// <summary>
    /// Pops the card from the top.
    /// </summary>
    /// <returns>The card from the top.</returns>
    public Card PopCard()
    {
        Card c = cards[next];
        next = (next+1)%cards.Count;
        return c;   
    }
}
}