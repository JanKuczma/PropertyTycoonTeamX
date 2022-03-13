using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View {
public class Board : MonoBehaviour
{
    [System.NonSerialized] public Square[] squares; // list for squares references
    [System.NonSerialized] public JailSquare jail;    // parameter for jail square reference

    void Awake()
    {
        squares = new Square[40];
    }
    public static Board Create(Transform parent,Model2.Board boardData)
    {
        // there are 3 chance colours so we want to use all of them
        int chance_variant = 1;
        Board board = Instantiate(Asset.Board(),parent).GetComponent<Board>();
        // get the data from Model.Board and init all the squares
        foreach(Model2.Space sp in boardData.spaces)
        {
            switch(sp.type)
            {
                case SqType.PROPERTY:
                board.squares[sp.position-1] = PropertySquare.Create(board.transform, sp.position,sp.name,((Model2.Space.Property)sp).cost.ToString(),((Model2.Space.Property)sp).group);
                break;
                case SqType.STATION:
                board.squares[sp.position-1] = UtilitySquare.Create(sp.type,board.transform, sp.position,sp.name,((Model2.Space.Station)sp).cost.ToString());
                break;
                case SqType.UTILITY:
                board.squares[sp.position-1] = UtilitySquare.Create(sp.type,board.transform, sp.position,sp.name,((Model2.Space.Utility)sp).cost.ToString());
                break;
                case SqType.TAX:
                board.squares[sp.position-1] = TaxSquare.Create(board.transform, sp.position,sp.name,((Model2.Space.Tax)sp).amount.ToString());
                break;
                case SqType.GO:
                board.squares[sp.position-1] = GoSquare.Create(board.transform, sp.position,sp.name,((Model2.Space.Go)sp).amount.ToString());
                break;
                case SqType.CHANCE:
                board.squares[sp.position-1] = FullSquare.Create(sp.type,board.transform, sp.position,sp.name,chance_variant.ToString());
                chance_variant = (chance_variant % 3) + 1;
                break;
                case SqType.POTLUCK:
                board.squares[sp.position-1] = FullSquare.Create(sp.type,board.transform, sp.position,sp.name,"");
                break;
                case SqType.PARKING:
                board.squares[sp.position-1] = ParkingSquare.Create(board.transform, sp.position,sp.name);
                break;
                case SqType.GOTOJAIL:
                board.squares[sp.position-1] = GoToJailSquare.Create(board.transform, sp.position,sp.name);
                break;
                case SqType.JAILVISIT:
                board.squares[sp.position-1] = JailSquare.Create(board.transform, sp.position,sp.name);
                break;
            }
        }
        return board;
    }
}
}