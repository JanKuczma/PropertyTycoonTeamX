using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    /// <summary>
    /// Class used to load/save <c>CardStack</c> data from/to JSON format.
    /// </summary>
    public static class CardData
    {
        /// <summary>
        /// Wrapper for <c>Card</c> class
        /// </summary>
        [System.Serializable]
        private class CardStackWrapper
        {
            public List<CardWrapper> cards = new List<CardWrapper>();
        }
        /// <summary>
        /// Wrapper for <c>CardStack</c> class
        /// </summary>
        [System.Serializable]
        private class CardWrapper
        {
            public string description;
            public string action;

            public List<string> keys;
            public List<int> values;
        }
        /// <summary>
        /// Saves <c>CardStack</c> data to a JSON file
        /// </summary>
        /// <param name="cards">Card Stack</param>
        /// <param name="filename">JSON File name</param>
        public static void saveCardStack(CardStack cards,string filename = "custom_cardstack.json")
        {
            string cards_json = JsonUtility.ToJson(stackToWrapper(cards),true);
            System.IO.File.WriteAllText(Application.dataPath + filename+".json",cards_json);
        }
        /// <summary>
        /// Loads JSON format of a Card Stack data to <c>CardStack</c> object
        /// </summary>
        /// <param name="json_format">JSON format of the Card Stack data</param>
        /// <returns><c>CardStack</c> object</returns>
        public static CardStack loadCardStack(string json_format)
        {
            CardStackWrapper cards = JsonUtility.FromJson<CardStackWrapper>(json_format);
            return wrapperToStack(cards);
        }
        /// <summary>
        /// Loads JCard Stack data from a JSON file to <c>CardStack</c> object
        /// </summary>
        /// <param name="file_name">File name</param>
        /// <returns><c>CardStack</c> object</returns>
        public static CardStack loadCardStackFromFile(string file_name)
        {
            CardStackWrapper cards = JsonUtility.FromJson<CardStackWrapper>(System.IO.File.ReadAllText(Application.dataPath + file_name +".json"));
            return wrapperToStack(cards);
        }
        
        
        private static CardWrapper cardToWrapper(Card card)
        {
            CardWrapper cardData = new CardWrapper();
            cardData.description = card.description;
            cardData.action = card.action.ToString();
            if(card.kwargs != null)
            {
                cardData.keys = card.kwargs.Keys.ToList();
                cardData.values = card.kwargs.Values.ToList();
            }
            return cardData;
        }

        private static CardStackWrapper stackToWrapper(CardStack stack)
        {
            CardStackWrapper stackData = new CardStackWrapper();
            foreach(Card card in stack.cards)
            {
                stackData.cards.Add(cardToWrapper(card));
            }
            return stackData;
        }
        private static Card wrapperToCard(CardWrapper data)
        {
            Dictionary<string,int> dict = new Dictionary<string, int>();
            for(int i = 0 ; i < data.keys.Count ;i++)
            {
                dict.Add(data.keys[i],data.values[i]);
            }
            Card card = new Card(data.description,(CardAction)System.Enum.Parse(typeof(CardAction),data.action),dict);
            return card;
        }

        private static CardStack wrapperToStack(CardStackWrapper data)
        {
            CardStack stack = new CardStack();
            foreach(CardWrapper cdata in data.cards)
            {
                stack.cards.Add(wrapperToCard(cdata));
            }
            return stack;
        }
    }
}