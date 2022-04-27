using System;
using System.IO;
using System.Linq.Expressions;
using TMPro;
using UnityEditor;
using UnityEngine;
/// <summary>
/// Represents a space/square type
/// </summary>
public enum SqType {GO,JAILVISIT,PARKING,GOTOJAIL,PROPERTY,STATION,UTILITY,POTLUCK,TAX,CHANCE}
/// <summary>
/// Represents a token type
/// </summary>
public enum Token {CAT ,BOOT,IRON,SHIP,HATSTAND,SMARTPHONE}

/// <summary>
/// This static class is used to get the assets at the runtime.
/// </summary>
public static class Asset
{
    //board
    public static GameObject BoardPrefab = Resources.Load<GameObject>("Prefabs/Board/BoardCenter");
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
    public static GameObject HousePrefab = Resources.Load<GameObject>("Prefabs/Board/Houses/HouseWithWindows");
    public static GameObject HotelPrefab = Resources.Load<GameObject>("Prefabs/Board/Houses/HotelWithWindows");
    //pieces
    static GameObject CatPrefab = Resources.Load<GameObject>("Prefabs/Pieces/Cat");
    static GameObject BootPrefab = Resources.Load<GameObject>("Prefabs/Pieces/Boot");
    static GameObject ShipPrefab = Resources.Load<GameObject>("Prefabs/Pieces/Ship");
    static GameObject SmartphonePrefab = Resources.Load<GameObject>("Prefabs/Pieces/Smartphone");
    static GameObject HatStandPrefab = Resources.Load<GameObject>("Prefabs/Pieces/HatStand");
    static GameObject IronPrefab = Resources.Load<GameObject>("Prefabs/Pieces/Iron");
    public static GameObject DiceContainerPrefab = Resources.Load<GameObject>("Prefabs/dice/DiceContainer");
    //cursors
    public static Texture2D FingerTextureCursor = Resources.Load<Texture2D>("Textures/FINGER-CURSOR");
    public static Texture2D GrabTextureCursor = Resources.Load<Texture2D>("Textures/GRAB-CURSOR");
    //board and card data
    static TextAsset classic_board_json = Resources.Load<TextAsset>("GameDataJSON/board_data");
    static TextAsset potluck_json = Resources.Load<TextAsset>("GameDataJSON/potluck_data");
    static TextAsset opportunity_knocks_json = Resources.Load<TextAsset>("GameDataJSON/opportunity_knocks_data");

    static TextAsset custom_board_json = Resources.Load<TextAsset>("CustomData/custom_board_data");
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
    public static GameObject hud = Resources.Load<GameObject>("Prefabs/HUD/hud");
    public static GameObject PlayerTabPrefab = Resources.Load<GameObject>("Prefabs/HUD/PlayerTab/PlayerTab");
    //Pop Ups
    public static GameObject OkPopUpPrefab = Resources.Load<GameObject>("Prefabs/HUD/PopUps/OkPopUp");
    public static GameObject InJailPopUpPrefab = Resources.Load<GameObject>("Prefabs/HUD/PopUps/InJailPrefab");
    public static GameObject PayRentPopUpPrefab = Resources.Load<GameObject>("Prefabs/HUD/PopUps/PayRentPopUp");
    public static GameObject MessagePopUpPrefab = Resources.Load<GameObject>("Prefabs/HUD/PopUps/MessagePopUp");
    public static GameObject BuyPropertyPopup = Resources.Load<GameObject>("Prefabs/HUD/PopUps/BuyPropertyPopUp");
    public static GameObject GoToJailPopUpPrefab = Resources.Load<GameObject>("Prefabs/HUD/PopUps/GoToJailPopUp");
    public static GameObject PropertyManagerPrefab = Resources.Load<GameObject>("Prefabs/HUD/PopUps/PropertyManager/PropertyManagerPopUp");
    public static GameObject ManageUtilityPopUpPrefab = Resources.Load<GameObject>("Prefabs/HUD/PopUps/PropertyManager/ManageUtilityPopUp");
    public static GameObject ManagePropertyPopUpPrefab = Resources.Load<GameObject>("Prefabs/HUD/PopUps/PropertyManager/ManagePropertyPopUp");
    public static GameObject CardActionPopUp = Resources.Load<GameObject>("Prefabs/HUD/PopUps/CardActionPopUp");
    public static GameObject CardActionPopWithOptionsUp = Resources.Load<GameObject>("Prefabs/HUD/PopUps/CardActionWithOptionsPopUp");
    public static GameObject AuctionPopUpPrefab = Resources.Load<GameObject>("Prefabs/HUD/PopUps/AuctionPopUp");
    public static GameObject OptionsPopUpPreFab = Resources.Load<GameObject>("Prefabs/HUD/PopUps/InGameOptionsPopUp");
    public static GameObject LoadSavePopUpPreFab = Resources.Load<GameObject>("Prefabs/HUD/PopUps/LoadSavePopUp");
    public static GameObject HelpPopUpPreFab = Resources.Load<GameObject>("Prefabs/HUD/PopUps/HelpPopUp");
    //themes
        // Skyboxes
    public static Material StarWarsSkyBoxMaterial = Resources.Load<Material>("Materials/StarWarsTheme/StarWarsSkyBox");
    public static GameObject Kitchen = Resources.Load<GameObject>("Prefabs/Enviornment/Kitchen");
    public static GameObject Walls = Resources.Load<GameObject>("Prefabs/Enviornment/InvisibleWalls");
        // game elements
    public static Material StarWarsThemeMaterial = Resources.Load<Material>("Materials/StarWarsTheme/theme");
    public static Material StarWarsPotLuckMaterial = Resources.Load<Material>("Materials/StarWarsTheme/communityCard");
    public static Material StarWarsOppKnocksMaterial = Resources.Load<Material>("Materials/StarWarsTheme/chanceCard");
    public static Material ClassicThemeMaterial = Resources.Load<Material>("Materials/StarWarsTheme/theme");
    public static Material ClassicPotLuckMaterial = Resources.Load<Material>("Materials/StarWarsTheme/communityCard");
    public static Material ClassicOppKnocksMaterial = Resources.Load<Material>("Materials/StarWarsTheme/chanceCard");
        //chance/oppknocks card IMGs
    public static Sprite ClassicOppKnocksIMG = Resources.Load<Sprite>("Textures/Communitychestcard");
    public static Sprite ClassicChangeIMG = Resources.Load<Sprite>("Textures/chancecard");
    public static Sprite StarWarsOppKnocksIMG = Resources.Load<Sprite>("Materials/StarWarsTheme/Communitychestcard-starwars copy");
    public static Sprite StarWarsChangeIMG = Resources.Load<Sprite>("Materials/StarWarsTheme/chance-starwars copy");
    
    /// <summary>
    /// Getter for board space prefabs
    /// </summary>
    /// <param name="type">Type of the square</param>
    /// <param name="variant">Optional: variant of the square</param>
    /// <returns></returns>
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

    public static string custom_board_data()
    {
        string custom_data_string = "";
        string path = Application.dataPath + "/custom_board_data.json";
        if(File.Exists(path))
        {
            custom_data_string = File.ReadAllText(path);
        }
        if(custom_data_string != "") {
            return custom_data_string;
        } else {
            return Asset.board_data_json();
        }
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
}
