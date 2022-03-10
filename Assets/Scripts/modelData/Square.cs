using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model{
public class Square
{
    public SqType type;
    public int position;
    public string name;
    public int price;   // PurchasableSquareData class?
    public Group group; //PropertySquareData class?

        //public int amount; TaxSquareData class?
        //public SquareCard card;???

    public Square(SqType type, int position, string name, int price, Group group)
    {
       this.type = type;
       this.position = position;
       this.name = name;
       this.price = price;
       this.group = group;
    }
}
}
