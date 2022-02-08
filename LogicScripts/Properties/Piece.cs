using System.Security.Authentication.ExtendedProtection.Configuration;
using System.Security.Cryptography;

public class Piece
{
    private string name;
    private string modelData;

    public Piece(string name, string modelData)
    {
        this.name = name;
        this.modelData = modelData;
    }

    public void jump()
    {
        
    }
}
