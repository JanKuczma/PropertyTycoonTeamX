namespace Model{
public class Card
{
    private string name;
    private string description;

    public Card(string name, string description)
    {
        this.name = name;
        this.description = description;
    }

    public string GetName()
    {
        return name;
    }

    public string GetDescription()
    {
        return description;
    }

    public override string ToString()
    {
        return name + " " + description;
    }
}
}