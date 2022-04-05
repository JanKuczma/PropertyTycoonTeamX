using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public static class CardData
    {
        [System.Serializable]
        private class CardStackWrapper
        {
            public List<CardWrapper> cards = new List<CardWrapper>();
        }

        [System.Serializable]
        private class CardWrapper
        {
            public string description;
            public string action;

            public List<string> keys;
            public List<int> values;
        }
        
        public static void saveCardStack(CardStack cards,string filename = "custom_cardstack.json")
        {
            string cards_json = JsonUtility.ToJson(stackToWrapper(cards),true);
            //System.IO.File.WriteAllText(Application.persistentDataPath + filename+".json",cards_json);
            System.IO.File.WriteAllText(filename,cards_json);
        }
        public static CardStack loadCardStack(string json_format)
        {

            CardStackWrapper cards = JsonUtility.FromJson<CardStackWrapper>(json_format);
            return wrapperToStack(cards);
        }
        public static CardStack loadCardStackFromFile(string file_name)
        {
            //CardStack cards = JsonUtility.FromJson<CardStack>(System.IO.File.ReadAllText(Application.persistentDataPath + file_path +".json"));
            CardStackWrapper cards = JsonUtility.FromJson<CardStackWrapper>(System.IO.File.ReadAllText(file_name));
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