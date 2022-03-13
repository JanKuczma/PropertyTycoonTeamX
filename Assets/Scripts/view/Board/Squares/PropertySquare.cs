using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public enum Group : int {BROWN=0x864c38, BLUE=0xabddf0, PURPLE=0xc53884, ORANGE=0xeb882c, RED=0xfff005, YELLOW=0xFFF005, GREEN=0x13a857, DEEPBLUE=0x0066a4}
namespace View{
public class PropertySquare : Square
{
    Group _group;
    string _price;

    public static PropertySquare Create(Transform parent, int position, string name,string price,Group group)
    {
        PropertySquare square = Instantiate(Asset.Board(SqType.PROPERTY),parent).GetComponent<PropertySquare>();
        square.transform.localScale = new Vector3(1,1,1);
        square.transform.localPosition = Square.generateCoordinates(position);
        square.transform.localRotation = getRotation(position);
        square.setName(name);
        square.assignSpots();
        square.setPrice(price);
        square.setGroup(group);
        return square;
    }
    public void setGroup(Group group)
    {
        _group = group;
        Color color;
        if ( ColorUtility.TryParseHtmlString("#"+((int)group).ToString("X")+"FF", out color))
        { GetComponent<Renderer>().materials[1].SetColor("_Color",color); }
    }
    
    public void setPrice(string price)
    {
        _price = price;
        GetComponentsInChildren<TextMeshPro>()[1].SetText("£"+price);
    }
    override protected void assignSpots()
    {
        float offsetSmall = offsetS*transform.localScale.x;
        float offsetBig = offsetB*transform.localScale.x;

        spots[0] = transform.position + transform.right*offsetSmall + transform.forward*(offsetSmall/2);
        spots[1] = transform.position - transform.right*offsetSmall + transform.forward*(offsetBig + offsetSmall/2);
        spots[2] = transform.position - transform.right*offsetSmall - transform.forward*(offsetBig - offsetSmall/2);
        spots[3] = transform.position + transform.right*offsetSmall + transform.forward*(offsetBig + offsetSmall/2);
        spots[4] = transform.position - transform.right*offsetSmall + transform.forward*(offsetSmall/2);
        spots[5] = transform.position + transform.right*offsetSmall - transform.forward*(offsetBig -offsetSmall/2);
    }
}
}