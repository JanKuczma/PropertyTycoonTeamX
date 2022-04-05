using System.Collections.Generic;
using UnityEngine;
public enum CardAction {PAYTOBANK,PAYTOPLAYER,MOVEFORWARDTO,MOVEBACKTO,MOVEBACK,PAYTOPARKING,BIRTHDAY,GOTOJAIL,OUTOFJAIL,PAYORCHANCE,REPAIRS}
namespace Model{
[System.Serializable]
public class Card
{
    public string description;
    public CardAction action;
    public Dictionary<string,int> kwargs = new Dictionary<string, int>();

    public Card(string description, CardAction action, Dictionary<string,int> kwargs = null)
    {
        this.description = description;
        this.action = action;
        this.kwargs = kwargs;
    }
}
}