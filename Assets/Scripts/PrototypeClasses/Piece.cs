namespace Model{
public class Piece
{
    private string name;
    private string modelData;

    public Piece(string name, string modelData)
    {
        this.name = name;
        this.modelData = modelData;
    }

    public override string ToString()
    {
        return "Piece: " + name;
    }
}
}