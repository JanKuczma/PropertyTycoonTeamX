using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Board : MonoBehaviour
{
    [System.NonSerialized] public SquareArrangement[] squares; // list for squares references
    [System.NonSerialized] public JailArrangement jail;    // parameter for jail square reference
    public GameObject   GoPrefab, JailVisitPrefab, ParkingPrefab, GoToJailPrefab,
                        PropertyPrefab, StationPrefab, BulbPrefab, WaterPrefab, PotLuckPrefab,
                        Chance1Prefab, Chance2Prefab, Chance3Prefab, SuperTaxPrefab, IncomeTaxPrefab;
    private Vector3[] squareCoordinates;

    void Awake()
    {
        squareCoordinates = new Vector3[40];
        generateSquareCoordinates();
        squares = new SquareArrangement[40];
    }

    public void initProperty(int position, string name, string price, string group)
    {
        GameObject square = Instantiate(PropertyPrefab,transform);
        square.transform.localScale = new Vector3(1,1,1);
        square.transform.localPosition = squareCoordinates[position-1];
        square.transform.localRotation = getRotation(position);
        squares[position-1] = square.GetComponent<SquareArrangement>();
        squares[position-1].assignSpots(position);
        //square.GetComponent<CustomisableSquareProp>().setName(name);
        //square.GetComponent<CustomisableSquareProp>().setPrice(price);
        //square.GetComponent<CustomisableSquareProp>().setGroup(group);
    }
    public void initBulb(int position, string name, string price)
    {
        GameObject square = Instantiate(BulbPrefab,transform);
        square.transform.localScale = new Vector3(1,1,1);
        square.transform.localPosition = squareCoordinates[position-1];
        square.transform.localRotation = getRotation(position);
        squares[position-1] = square.GetComponent<SquareArrangement>();
        squares[position-1].assignSpots(position);
        //square.GetComponent<CustomisableSquare>().setName(name);
        //square.GetComponent<CustomisableSquare>().setPrice(price);
    }
    public void initWater(int position, string name, string price)
    {
        GameObject square = Instantiate(WaterPrefab,transform);
        square.transform.localScale = new Vector3(1,1,1);
        square.transform.localPosition = squareCoordinates[position-1];
        square.transform.localRotation = getRotation(position);
        squares[position-1] = square.GetComponent<SquareArrangement>();
        squares[position-1].assignSpots(position);
        //square.GetComponent<CustomisableSquare>().setName(name);
        //square.GetComponent<CustomisableSquare>().setPrice(price);
    }
    public void initStation(int position, string name, string price)
    {
        GameObject square = Instantiate(StationPrefab,transform);
        square.transform.localScale = new Vector3(1,1,1);
        square.transform.localPosition = squareCoordinates[position-1];
        square.transform.localRotation = getRotation(position);
        squares[position-1] = square.GetComponent<SquareArrangement>();
        squares[position-1].assignSpots(position);
        //square.GetComponent<CustomisableSquare>().setName(name);
        //square.GetComponent<CustomisableSquare>().setPrice(price);
    }
    public void initPotLuck(int position)
    {
        GameObject square = Instantiate(PotLuckPrefab,transform);
        square.transform.localScale = new Vector3(1,1,1);
        square.transform.localPosition = squareCoordinates[position-1];
        square.transform.localRotation = getRotation(position);
        squares[position-1] = square.GetComponent<SquareArrangement>();
        squares[position-1].assignSpots(position);
    }
    public void initChance1(int position)
    {
        GameObject square = Instantiate(Chance1Prefab,transform);
        square.transform.localScale = new Vector3(1,1,1);
        square.transform.localPosition = squareCoordinates[position-1];
        square.transform.localRotation = getRotation(position);
        squares[position-1] = square.GetComponent<SquareArrangement>();
        squares[position-1].assignSpots(position);
    }
    public void initChance2(int position)
    {
        GameObject square = Instantiate(Chance2Prefab,transform);
        square.transform.localScale = new Vector3(1,1,1);
        square.transform.localPosition = squareCoordinates[position-1];
        square.transform.localRotation = getRotation(position);
        squares[position-1] = square.GetComponent<SquareArrangement>();
        squares[position-1].assignSpots(position);
    }
    public void initChance3(int position)
    {
        GameObject square = Instantiate(Chance3Prefab,transform);
        square.transform.localScale = new Vector3(1,1,1);
        square.transform.localPosition = squareCoordinates[position-1];
        square.transform.localRotation = getRotation(position);
        squares[position-1] = square.GetComponent<SquareArrangement>();
        squares[position-1].assignSpots(position);
    }
    public void initSuperTax(int position, string amount)
    {
        GameObject square = Instantiate(SuperTaxPrefab,transform);
        square.transform.localScale = new Vector3(1,1,1);
        square.transform.localPosition = squareCoordinates[position-1];
        square.transform.localRotation = getRotation(position);
        squares[position-1] = square.GetComponent<SquareArrangement>();
        squares[position-1].assignSpots(position);
        //square.GetComponent<CustomisableTax>().setAmount(amount);
    }
    public void initIncomeTax(int position, string amount)
    {
        GameObject square = Instantiate(IncomeTaxPrefab,transform);
        square.transform.localScale = new Vector3(1,1,1);
        square.transform.localPosition = squareCoordinates[position-1];
        square.transform.localRotation = getRotation(position);
        squares[position-1] = square.GetComponent<SquareArrangement>();
        squares[position-1].assignSpots(position);
        //square.GetComponent<CustomisableTax>().setAmount(amount);
    }
    public void initGo(int position = 1)
    {
        GameObject square = Instantiate(GoPrefab,transform);
        square.transform.localScale = new Vector3(1,1,1);
        square.transform.localPosition = squareCoordinates[position-1];
        square.transform.localRotation = getRotation(position);
        squares[position-1] = square.GetComponent<SquareArrangement>();
        squares[position-1].assignSpots(position);
    }
    public void initJailVisit(int position = 11)
    {
        GameObject square = Instantiate(JailVisitPrefab,transform);
        square.transform.localScale = new Vector3(1,1,1);
        square.transform.localPosition = squareCoordinates[position-1];
        square.transform.localRotation = getRotation(position);
        jail = square.GetComponent<JailArrangement>(); 
        squares[position-1] = jail;
        jail.assignSpots(position);
        jail.assignCells();
        
    }
    public void initParking(int position = 21)
    {
        GameObject square = Instantiate(ParkingPrefab,transform);
        square.transform.localScale = new Vector3(1,1,1);
        square.transform.localPosition = squareCoordinates[position-1];
        square.transform.localRotation = getRotation(position);
        squares[position-1] = square.GetComponent<SquareArrangement>();
        squares[position-1].assignSpots(position);
    }
    public void initGoToJail(int position = 31)
    {
        GameObject square = Instantiate(GoToJailPrefab,transform);
        square.transform.localScale = new Vector3(1,1,1);
        square.transform.localPosition = squareCoordinates[position-1];
        square.transform.localRotation = getRotation(position);
        squares[position-1] = square.GetComponent<SquareArrangement>();
        squares[position-1].assignSpots(position);
    }

    private void generateSquareCoordinates()
    {
        float displacement = .0064f;
        for(int i = 1; i < squareCoordinates.Length/4; i++)
        {
            squareCoordinates[i] = new Vector3(-displacement,0,.0085f);
            squareCoordinates[i+squareCoordinates.Length/4] = new Vector3(.0085f,0,displacement);
            squareCoordinates[i+squareCoordinates.Length/2] = new Vector3(displacement,0,-.0085f);
            squareCoordinates[i+(int)(squareCoordinates.Length*0.75f)] = new Vector3(-.0085f,0,-displacement);
            displacement -= .0016f;
        }
        squareCoordinates[0] = new Vector3(-.0085f,0,.0085f);
        squareCoordinates[10] = new Vector3(.0085f,0,.0085f);
        squareCoordinates[20] = new Vector3(.0085f,0,-.0085f);
        squareCoordinates[30] = new Vector3(-.0085f,0,-.0085f);
    }

    private Quaternion getRotation(int position)
    {
        return  position > 30 ? Quaternion.Euler(0,-90,0) :
                position > 20 ? Quaternion.Euler(0,180,0) :
                position > 10 ? Quaternion.Euler(0,90,0) :
                                Quaternion.Euler(0,0,0);
    }
}
