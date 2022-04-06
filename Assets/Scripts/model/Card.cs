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

    public int RepairsCost(Player player)
    {
        if(action == CardAction.REPAIRS){
            int total = 0;
            foreach(Model.Space.Purchasable space in player.owned_spaces)
            {
                if(space.type == SqType.PROPERTY)
                {
                    if(((Model.Space.Property)space).noOfHouses == 5)
                    {
                        total += kwargs["hotel"];
                    } else {
                        total += ((Model.Space.Property)space).noOfHouses * kwargs["house"];
                    }
                }
            }
            return total;
        } else {
            return -1;
        }
    }
}
}