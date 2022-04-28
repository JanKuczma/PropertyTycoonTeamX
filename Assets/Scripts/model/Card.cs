using System.Collections.Generic;
/// <summary>
/// Represents the type of action that should be taken after revealing the card
/// </summary>
public enum CardAction {PAYTOBANK,PAYTOPLAYER,MOVEFORWARDTO,MOVEBACKTO,MOVEBACK,PAYTOPARKING,BIRTHDAY,GOTOJAIL,OUTOFJAIL,PAYORCHANCE,REPAIRS}
namespace Model{
    /// <summary>
    /// Represents PT Potluck/Opportunity Knocks card
    /// </summary>
[System.Serializable]
public class Card
{
    /// <summary>
    /// Whatever is written on the card
    /// </summary>
    public string description;
    /// <summary>
    /// Card's action
    /// </summary>
    public CardAction action;
    /// <summary>
    /// Optional arguments that are necessary for the action.<br/>
    /// <listheader>Possible keyword - argument pairs:</listheader>
    /// <list type="bullet">
    /// <item>
    ///     <term>"amount"</term><description> Integer value representing value of money to pay/receive.</description>
    /// </item>
    /// <item>
    ///     <term>"position"</term><description> Position of the space where token shall move.</description>
    /// </item>
    /// <item>
    ///     <term>"steps"</term><description> Number of steps the token shall move.</description>
    /// </item>
    /// <item>
    ///     <term>"house"</term><description> House repair cost.</description>
    /// </item>
    /// <item>
    ///     <term>"hotel"</term><description> Hotel repair cost.</description>
    /// </item>
    /// </list>
    /// </summary>
    public Dictionary<string,int> kwargs = new Dictionary<string, int>();

    public Card(string description, CardAction action, Dictionary<string,int> kwargs = null)
    {
        this.description = description;
        this.action = action;
        this.kwargs = kwargs;
    }
/// <summary>
/// Calculates the cost of the repairs
/// </summary>
/// <param name="player">Player that took this card.</param>
/// <returns>
/// <list type="bullet">
/// <item><i>int</i> value representing the amount due for</item>
/// <item>-1 if the card's <paramref name="action"/> is not <c>REPAIRS</c>. See <see cref="CardAction"/></item>
/// </list>
/// </returns>
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