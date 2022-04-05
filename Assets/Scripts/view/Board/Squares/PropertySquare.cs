using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public enum Group : int {BROWN=0x864c38, BLUE=0xabddf0, PURPLE=0xc53884, ORANGE=0xeb882c, RED=0xdb2428, YELLOW=0xFFF005, GREEN=0x13a857, DEEPBLUE=0x0066a4}
namespace View{
public class PropertySquare : Square
{
    public TextMeshPro amount;
    Group _group;
    string _price;
    List<GameObject> houses = new List<GameObject>();
    Vector3[] houses_spots = new Vector3[5];
    public GameObject ribbon;

    public static PropertySquare Create(Transform parent, int position, string name,string price,Group group)
    {
        PropertySquare square = Instantiate(Asset.Board(SqType.PROPERTY),parent).GetComponent<PropertySquare>();
        square.transform.localScale = new Vector3(1,1,1);
        square.transform.localPosition = Square.generateCoordinates(position);
        square.transform.localRotation = getRotation(position);
        square.setName(name);
        square._position = position;
        square.assignSpots();
        square.assignHousesSpots();
        square.setPrice(price);
        square.setGroup(group);
        return square;
    }
    public void setGroup(Group group)
    {
        _group = group;
        Color color;
        if ( ColorUtility.TryParseHtmlString("#"+((int)group).ToString("X6")+"FF", out color))
        { GetComponent<Renderer>().materials[1].SetColor("_Color",color); }
    }
    
    public void setPrice(string price)
    {
        _price = price;
        GetComponentsInChildren<TextMeshPro>()[1].SetText(price+"Q");
    }

    public void addHouse()
    {
        if(houses.Count < 4)
        {
            GameObject house = Instantiate(Asset.House(),transform);
            houses.Add(house);
            house.transform.localPosition = houses_spots[houses.Count-1];
        }
        else if(houses.Count == 4)
        {
            foreach(GameObject house in houses)
            {
                Destroy(house);
            }
            GameObject hotel = Instantiate(Asset.Hotel(),transform);
            houses.Add(hotel);
            hotel.transform.localPosition = houses_spots[4];
        }
    }
    public void removeHouse()
    {
        if(houses.Count <= 4)
        {
            GameObject tmp = houses[houses.Count-1];
            houses.Remove(tmp);
            Destroy(tmp);
        }
        if(houses.Count == 5)
        {
            GameObject tmp = houses[houses.Count-1];
            houses.Clear();
            Destroy(tmp);
            for(int i = 0; i < 4; i++)
            {
                GameObject house = Instantiate(Asset.House(),transform);
                houses.Add(house);
                house.transform.localPosition = houses_spots[i];
            }
        }
    }

    public void showRibbon(Color color)
    {
        ribbon.SetActive(true);
        color.a = 150f/255f;
        ribbon.GetComponent<Renderer>().material.SetColor("_Color",color);
    }

    public void removeRibbon()
    {
        ribbon.SetActive(false);
    }
    override protected void assignSpots()
    {
        float offsetSmall = offsetS*transform.localScale.x;
        float offsetBig = offsetB*transform.localScale.x;

        spots[0] = transform.position + transform.right*offsetSmall + transform.forward*(offsetSmall/2)             + transform.forward*(offsetSmall/5);
        spots[1] = transform.position + transform.right*offsetSmall + transform.forward*(offsetBig + offsetSmall/2) + transform.forward*(offsetSmall/5);
        spots[2] = transform.position + transform.right*offsetSmall - transform.forward*(offsetBig -offsetSmall/2)  + transform.forward*(offsetSmall/5);
        spots[3] = transform.position - transform.right*offsetSmall + transform.forward*(offsetSmall/2)             - transform.forward*(offsetSmall/5);
        spots[4] = transform.position - transform.right*offsetSmall + transform.forward*(offsetBig + offsetSmall/2) - transform.forward*(offsetSmall/5);
        spots[5] = transform.position - transform.right*offsetSmall - transform.forward*(offsetBig - offsetSmall/2) - transform.forward*(offsetSmall/5);
    }
    private void assignHousesSpots()
    {

        houses_spots[0] = new Vector3(.54f, .06f, -1);
        houses_spots[1] = new Vector3(.18f, .06f, -1);
        houses_spots[2] = new Vector3(-.18f, .06f, -1);
        houses_spots[3] = new Vector3(-.54f, .06f, -1);
        houses_spots[4] = new Vector3(0, .06f, -1);
    }
    
}
}