using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// enum types
public enum SqType {GO,JAILVISIT,PARKING,GOTOJAIL,PROPERTY,STATION,BULB,WATER,POTLUCK,SUPERTAX,INCOMETAX,CHANCE1,CHANCE2,CHANCE3}
public enum Token {CAT ,BOOT,IRON,SHIP,HATSTAND,SMARTPHONE}
public enum CursorType {FINGER,GRAB}
/*
    This static class is used to get the assets at the runtime
    it seems to be the most efficient way to load the assets to the game atm
*/
public static class Asset
{
    //board
    static GameObject BoardPrefab = Resources.Load<GameObject>("Prefabs/Board/BoardCenter");
    static GameObject GoPrefab = Resources.Load<GameObject>("Prefabs/Board/elements/Go");
    static GameObject JailVisitPrefab = Resources.Load<GameObject>("Prefabs/Board/elements/JailVisit");
    static GameObject ParkingPrefab = Resources.Load<GameObject>("Prefabs/Board/elements/Parking");
    static GameObject GoToJailPrefab = Resources.Load<GameObject>("Prefabs/Board/elements/GoToJail");
    static GameObject PropertyPrefab = Resources.Load<GameObject>("Prefabs/Board/elements/Property");
    static GameObject StationPrefab = Resources.Load<GameObject>("Prefabs/Board/elements/Station");
    static GameObject BulbPrefab = Resources.Load<GameObject>("Prefabs/Board/elements/Bulb");
    static GameObject WaterPrefab = Resources.Load<GameObject>("Prefabs/Board/elements/Water");
    static GameObject PotLuckPrefab = Resources.Load<GameObject>("Prefabs/Board/elements/PotLuck");
    static GameObject SuperTaxPrefab = Resources.Load<GameObject>("Prefabs/Board/elements/SuperTax");
    static GameObject IncomeTaxPrefab = Resources.Load<GameObject>("Prefabs/Board/elements/IncomeTax");
    static GameObject Chance1Prefab = Resources.Load<GameObject>("Prefabs/Board/elements/Chance(1)");
    static GameObject Chance2Prefab = Resources.Load<GameObject>("Prefabs/Board/elements/Chance(2)");
    static GameObject Chance3Prefab = Resources.Load<GameObject>("Prefabs/Board/elements/Chance(3)");
    //house and hotel
    static GameObject HousePrefab = Resources.Load<GameObject>("Prefabs/Board/Houses/House");
    static GameObject HotelPrefab = Resources.Load<GameObject>("Prefabs/Board/Houses/Hotel");
    //pieces
    static GameObject CatPrefab = Resources.Load<GameObject>("Prefabs/Pieces/Cat");
    static GameObject BootPrefab = Resources.Load<GameObject>("Prefabs/Pieces/Boot");
    static GameObject ShipPrefab = Resources.Load<GameObject>("Prefabs/Pieces/Ship");
    static GameObject SmartphonePrefab = Resources.Load<GameObject>("Prefabs/Pieces/Smartphone");
    static GameObject HatStandPrefab = Resources.Load<GameObject>("Prefabs/Pieces/HatStand");
    static GameObject IronPrefab = Resources.Load<GameObject>("Prefabs/Pieces/Iron");
    static GameObject DiceContainerPrefab = Resources.Load<GameObject>("Prefabs/dice/DiceContainer");
    //cursors
    static Texture2D FingerTexture = Resources.Load<Texture2D>("Textures/FINGER-CURSOR");
    static Texture2D GrabTexture = Resources.Load<Texture2D>("Textures/GRAB-CURSOR");


    public static GameObject Board(SqType type)
    {

        return  type == SqType.GO ? GoPrefab :
                type == SqType.JAILVISIT ? JailVisitPrefab :
                type == SqType.PARKING ? ParkingPrefab :
                type == SqType.GOTOJAIL ? GoToJailPrefab :
                type == SqType.PROPERTY ? PropertyPrefab :
                type == SqType.STATION ? StationPrefab :
                type == SqType.BULB ? BulbPrefab :
                type == SqType.WATER ? WaterPrefab :
                type == SqType.POTLUCK ? PotLuckPrefab :
                type == SqType.SUPERTAX ? SuperTaxPrefab :
                type == SqType.INCOMETAX ? IncomeTaxPrefab :
                type == SqType.CHANCE1 ? Chance1Prefab :
                type == SqType.CHANCE2 ? Chance2Prefab :
                type == SqType.CHANCE3 ? Chance3Prefab :
                null;
    }
    public static GameObject Board()
    {
        return BoardPrefab;
    }

    public static GameObject Piece(Token token)
    {
        return  token == Token.CAT ? CatPrefab :
                token == Token.SHIP ? ShipPrefab :
                token == Token.IRON ? IronPrefab :
                token == Token.SMARTPHONE ? SmartphonePrefab :
                token == Token.HATSTAND ? HatStandPrefab :
                token == Token.BOOT ? BootPrefab :
                null;
    }

    public static GameObject Dice()
    {
        return DiceContainerPrefab;
    }
    
    public static Texture2D Cursor(CursorType type)
    {
        return  type == CursorType.FINGER ? FingerTexture :
                type == CursorType.GRAB ? GrabTexture :
                null;
    }

    public static GameObject House()
    {
        return HousePrefab;
    }

    public static GameObject Hotel()
    {
        return HotelPrefab;
    }
}
