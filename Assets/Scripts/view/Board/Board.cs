using UnityEngine;
using TMPro;

namespace View {
public class Board : MonoBehaviour
{
    [System.NonSerialized] public Square[] squares; // list for squares references
    [System.NonSerialized] public JailSquare jail;    // parameter for jail square reference
    public GameObject opp_knock_stack;
    public GameObject potluck_stack;

    void Awake()
    {
        squares = new Square[40];
    }
    public static Board Create(Transform parent,Model.Board boardData)
    {
        // there are 3 chance colours so we want to use all of them
        int chance_variant = 1;
        Board board = Instantiate(Asset.Board(),parent).GetComponent<Board>();
        // get the data from Model.Board and init all the squares
        foreach(Model.Space sp in boardData.spaces)
        {
            switch(sp.type)
            {
                case SqType.PROPERTY:
                board.squares[sp.position-1] = PropertySquare.Create(board.transform, sp.position,sp.name,((Model.Space.Property)sp).cost.ToString(),((Model.Space.Property)sp).group);
                break;
                case SqType.STATION:
                board.squares[sp.position-1] = UtilitySquare.Create(sp.type,board.transform, sp.position,sp.name,((Model.Space.Station)sp).cost.ToString());
                break;
                case SqType.UTILITY:
                board.squares[sp.position-1] = UtilitySquare.Create(sp.type,board.transform, sp.position,sp.name,((Model.Space.Utility)sp).cost.ToString());
                break;
                case SqType.TAX:
                board.squares[sp.position-1] = TaxSquare.Create(board.transform, sp.position,sp.name,((Model.Space.Tax)sp).amount.ToString());
                break;
                case SqType.GO:
                board.squares[sp.position-1] = GoSquare.Create(board.transform, sp.position,sp.name,((Model.Space.Go)sp).amount.ToString());
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
                board.jail = (View.JailSquare)board.squares[sp.position-1];
                break;
            }
        }
        return board;
    }

    public void loadTheme(string theme = "classic")
    {
        if(theme == "starwars")
        {
            GetComponent<MeshRenderer>().material = Asset.StarWarsThemeMaterial;
            opp_knock_stack.GetComponent<MeshRenderer>().material = Asset.StarWarsOppKnocksMaterial;
            potluck_stack.GetComponent<MeshRenderer>().material = Asset.StarWarsPotLuckMaterial;
            foreach(Square square in squares)
            {
                square.GetComponent<MeshRenderer>().material = Asset.StarWarsThemeMaterial; //237f/255f,238f/255f,181f/255f
                if(square is JailSquare) { square.GetComponent<MeshRenderer>().materials[1] = Asset.StarWarsThemeMaterial; }
                square.GetComponentsInChildren<TextMeshPro>()[0].color = new Color(237f/255f,238f/255f,181f/255f);
                if(square.GetComponentsInChildren<TextMeshPro>().Length > 1) { square.GetComponentsInChildren<TextMeshPro>()[1].color = new Color(237f/255f,238f/255f,181f/255f); }
            }
        } else {
            GetComponent<MeshRenderer>().material = Asset.ClassicThemeMaterial;
            opp_knock_stack.GetComponent<MeshRenderer>().material = Asset.ClassicOppKnocksMaterial;
            potluck_stack.GetComponent<MeshRenderer>().material = Asset.ClassicPotLuckMaterial;
            foreach(Square square in squares)
            {
                square.GetComponent<MeshRenderer>().materials[0] = Asset.ClassicThemeMaterial;
                if(square is JailSquare) { square.GetComponent<MeshRenderer>().materials[1] = Asset.ClassicThemeMaterial; }
                square.GetComponentsInChildren<TextMeshPro>()[0].color = new Color(31f/255f, 31f/255f,31f/255f);
                if(square.GetComponentsInChildren<TextMeshPro>().Length > 1) { square.GetComponentsInChildren<TextMeshPro>()[1].color = new Color(31f/255f,31f/255f,31f/255f); }
            }
        }
    }
}
}