using TMPro;
using UnityEngine;

/// enum types
public enum SqType {GO,JAILVISIT,PARKING,GOTOJAIL,PROPERTY,STATION,UTILITY,POTLUCK,TAX,CHANCE}
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
    //board and card data
    static TextAsset classic_board_json = Resources.Load<TextAsset>("GameDataJSON/board_data");
    static TextAsset potluck_json = Resources.Load<TextAsset>("GameDataJSON/potluck_data");
    static TextAsset opportunity_knocks_json = Resources.Load<TextAsset>("GameDataJSON/opportunity_knocks_data");
    //token IMGs
    static Sprite catIMG = Resources.Load<Sprite>("tokenIMGs/catIMG");
    static Sprite ironIMG = Resources.Load<Sprite>("tokenIMGs/ironIMG");
    static Sprite bootIMG = Resources.Load<Sprite>("tokenIMGs/bootIMG");
    static Sprite boatIMG = Resources.Load<Sprite>("tokenIMGs/boatIMG");
    static Sprite smartphoneIMG = Resources.Load<Sprite>("tokenIMGs/phoneIMG");
    static Sprite hatstandIMG = Resources.Load<Sprite>("tokenIMGs/hatstandIMG");
    //token spriteSheets
    static Sprite[] catAnim = Resources.LoadAll<Sprite>("TokenSpriteSheets/cat");
    static Sprite[] ironAnim = Resources.LoadAll<Sprite>("TokenSpriteSheets/iron");
    static Sprite[] bootAnim = Resources.LoadAll<Sprite>("TokenSpriteSheets/boot");
    static Sprite[] shipAnim = Resources.LoadAll<Sprite>("TokenSpriteSheets/ship");
    static Sprite[] phoneAnim = Resources.LoadAll<Sprite>("TokenSpriteSheets/phone");
    static Sprite[] standAnim = Resources.LoadAll<Sprite>("TokenSpriteSheets/stand");
    //property cards
    public static GameObject PropertyCard = Resources.Load<GameObject>("Prefabs/HUD/PropertyCards/PropertyCard");
    static GameObject WaterCard = Resources.Load<GameObject>("Prefabs/HUD/PropertyCards/WaterCard");
    static GameObject BulbCard = Resources.Load<GameObject>("Prefabs/HUD/PropertyCards/BulbCard");
    public static GameObject StationCard = Resources.Load<GameObject>("Prefabs/HUD/PropertyCards/StationCard");

    //HUD components
    static GameObject playerTabPrefab = Resources.Load<GameObject>("Prefabs/HUD/PlayerTab/PlayerTab");
    //Pop Ups
    public static GameObject okPopup = Resources.Load<GameObject>("Prefabs/HUD/PopUps/okPopup");
    public static GameObject MessagePopUpPrefab = Resources.Load<GameObject>("Prefabs/HUD/PopUps/MessagePopUp");
    public static GameObject BuyPropertyPopup = Resources.Load<GameObject>("Prefabs/HUD/PopUps/BuyPropertyPopUp");
    public static GameObject GoToJailPopUpPrefab = Resources.Load<GameObject>("Prefabs/HUD/PopUps/GoToJailPopUp");
    static GameObject PropertyManagerPrefab = Resources.Load<GameObject>("Prefabs/HUD/PopUps/PropertyManager/PropertyManagerPopUp");
    public static GameObject ManageUtilityPopUpPrefab = Resources.Load<GameObject>("Prefabs/HUD/PopUps/PropertyManager/ManageUtilityPopUp");
    public static GameObject ManagePropertyPopUpPrefab = Resources.Load<GameObject>("Prefabs/HUD/PopUps/PropertyManager/ManagePropertyPopUp");


    public static GameObject Board(SqType type,string variant = "")
    {

        return  type == SqType.GO ? GoPrefab :
                type == SqType.JAILVISIT ? JailVisitPrefab :
                type == SqType.PARKING ? ParkingPrefab :
                type == SqType.GOTOJAIL ? GoToJailPrefab :
                type == SqType.PROPERTY ? PropertyPrefab :
                type == SqType.STATION ? StationPrefab :
                type == SqType.UTILITY ? variant == "TESLA POWER CO" ? BulbPrefab : WaterPrefab :
                type == SqType.POTLUCK ? PotLuckPrefab :
                type == SqType.TAX ? variant == "INCOME TAX" ? IncomeTaxPrefab : SuperTaxPrefab :
                type == SqType.CHANCE ? variant == "1" ? Chance1Prefab : variant == "2" ? Chance2Prefab : Chance3Prefab :
                null;
    }
    public static GameObject Board()
    {
        return BoardPrefab;
    }

    public static GameObject UtilityCard(string variant)
    {
        return variant == "TESLA POWER CO" ? BulbCard : WaterCard;
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

    public static string board_data_json()
    {
        return classic_board_json.ToString();
    }

    public static string potluck_data_json()
    {
        return potluck_json.ToString();
    }

    public static string opportunity_knocks_data_json()
    {
        return opportunity_knocks_json.ToString();
    }

        public static GameObject House()
    {
        return HousePrefab;
    }

    public static GameObject Hotel()
    {
        return HotelPrefab;
    }

    public static Sprite TokenIMG(Token token)
    {
        return  token == Token.CAT ? catIMG :
                token == Token.SHIP ? boatIMG :
                token == Token.IRON ? ironIMG :
                token == Token.SMARTPHONE ? smartphoneIMG :
                token == Token.HATSTAND ? hatstandIMG :
                token == Token.BOOT ? bootIMG :
                null;
    }

    public static Sprite[] TokenAnim(Token token)
    {
        return  token == Token.CAT ? catAnim :
                token == Token.SHIP ? shipAnim :
                token == Token.IRON ? ironAnim :
                token == Token.SMARTPHONE ? phoneAnim :
                token == Token.HATSTAND ? standAnim :
                token == Token.BOOT ? bootAnim :
                null;
    }

    public static GameObject playerTab()
    {
        return playerTabPrefab;
    }

    public static GameObject propertyManager()
    {
        return PropertyManagerPrefab;
    }
}
