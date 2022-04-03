using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using View;
using TMPro;

public class MainMenu : MonoBehaviour
{
    
    //This is info to be collected from the PlayerSelect menu
    public int numPlayers = 2;
    public Dictionary<string, Tuple<Token, bool>> playerInfo = new Dictionary<string, Tuple<Token, bool>>();
    
    //These are variables for the GamemodeSelect menu
    public int gm_index = 0;
    public int theme_index = 0;
    public TextMeshProUGUI buttonText;
    public TextMeshProUGUI gmDescription;
    public SpriteRenderer themePreview;
    public Sprite kingsley_classic, kingsley_yoda;


    MessagePopUp curr_pop;
    
    public void GoToPlayerSelect()
    {
        SceneManager.LoadScene(2);
    }

    public void GoToGameOptions()
    {
        SceneManager.LoadScene(1);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        //Go through each player panel in turn
        for (int i = 1; i <= numPlayers; i++)
        {
            //get name input
            TMP_InputField nameInput = GameObject.Find("Player" + i).GetComponentInChildren<TMP_InputField>();
            string playerName = nameInput.text;

            Toggle compToggle = GameObject.Find("Player" + i).GetComponentInChildren<Toggle>();
            bool isComp = compToggle.isOn;
            
            //find name of selected Piece from grid
            var tokenGrid = GameObject.Find("Player"+i).GetComponentInChildren<ToggleGroup>();
            IEnumerable<Toggle> playerPiece = tokenGrid.ActiveToggles();
            
            // ActiveToggles() returns a Collection (of one piece) using a foreach loop for now to access
            bool nameErrorFound = false;
            bool pieceErrorFound = false;
            foreach (var piece in playerPiece)
            {
                if (piece.name.Equals("Iron"))
                {
                    // check for duplicate name entries
                    if (playerInfo.ContainsKey(playerName))
                    {
                        nameErrorFound = true;
                        break;
                    }
                    // check for duplicate piece selection
                    if (playerInfo.Any(x => x.Value.Item1 == Token.IRON))
                    {
                        pieceErrorFound = true;
                        break;
                    }
                    playerInfo.Add(playerName, new Tuple<Token, bool>(Token.IRON, isComp));
                } 
                else if (piece.name.Equals("Hatstand"))
                {
                    // check for duplicate name entries
                    if (playerInfo.ContainsKey(playerName))
                    {
                        nameErrorFound = true;
                        break;
                    }
                    // check for duplicate piece selection
                    if (playerInfo.Any(x => x.Value.Item1 == Token.HATSTAND))
                    {
                        pieceErrorFound = true;
                        break;
                    }
                    playerInfo.Add(playerName, new Tuple<Token, bool>(Token.HATSTAND, isComp));
                } 
                else if (piece.name.Equals("Phone"))
                {
                    // check for duplicate name entries
                    if (playerInfo.ContainsKey(playerName))
                    {
                        nameErrorFound = true;
                        break;
                    }
                    // check for duplicate piece selection
                    if (playerInfo.Any(x => x.Value.Item1 == Token.SMARTPHONE))
                    {
                        pieceErrorFound = true;
                        break;
                    }
                    playerInfo.Add(playerName, new Tuple<Token, bool>(Token.SMARTPHONE, isComp));
                } 
                else if (piece.name.Equals("Boat"))
                {
                    // check for duplicate name entries
                    if (playerInfo.ContainsKey(playerName))
                    {
                        nameErrorFound = true;
                        break;
                    }
                    // check for duplicate piece selection
                    if (playerInfo.Any(x => x.Value.Item1 == Token.SHIP))
                    {
                        pieceErrorFound = true;
                        break;
                    }
                    playerInfo.Add(playerName, new Tuple<Token, bool>(Token.SHIP, isComp));
                } 
                else if (piece.name.Equals("Boot"))
                {
                    // check for duplicate name entries
                    if (playerInfo.ContainsKey(playerName))
                    {
                        nameErrorFound = true;
                        break;
                    }
                    // check for duplicate piece selection
                    if (playerInfo.Any(x => x.Value.Item1 == Token.BOOT))
                    {
                        pieceErrorFound = true;
                        break;
                    }
                    playerInfo.Add(playerName, new Tuple<Token, bool>(Token.BOOT, isComp));
                } 
                else if (piece.name.Equals("Cat"))
                {
                    // check for duplicate name entries
                    if (playerInfo.ContainsKey(playerName))
                    {
                        nameErrorFound = true;
                        break;
                    }
                    // check for duplicate piece selection
                    if (playerInfo.Any(x => x.Value.Item1 == Token.CAT))
                    {
                        pieceErrorFound = true;
                        break;
                    }
                    playerInfo.Add(playerName, new Tuple<Token, bool>(Token.CAT, isComp));
                } 
            }

            if (nameErrorFound)
            {
                if(curr_pop) { Destroy(curr_pop.gameObject); }
                curr_pop = MessagePopUp.Create(transform,"Players cannot have the same name",2);
                playerInfo.Clear();
                nameErrorFound = false;
                return;
            }

            if (pieceErrorFound)
            {
                if(curr_pop) { Destroy(curr_pop.gameObject); }
                curr_pop = MessagePopUp.Create(transform,"Players cannot have the same piece",2);
                playerInfo.Clear();
                pieceErrorFound = false;
                return;
            }
        }
        
        Debug.Log(playerInfo.Keys.Count);
        
        //state player name and piece
        foreach (var key in playerInfo.Keys)
        {
            if (playerInfo[key].Item2)
            {
                Debug.Log(key + " is a computer and is playing as the " + playerInfo[key].Item1);
            }
            else
            {
                Debug.Log(key + " is a human and is playing as the " + playerInfo[key].Item1);
            }
        }
        int e = 0; // to iterate over player_colors
        int[] colors = {0xFF0000,0x0000FF,0x00ff00,0xffea04,0xff00ff,0x00ffff};
        foreach(KeyValuePair<string, Tuple<Token, bool>> entry in playerInfo)
        {
            GameObject.Find("GameData").GetComponent<GameData>().players.Add(new Model.Player(entry.Key,entry.Value.Item1,entry.Value.Item2,colors[e]));
            e++;
        }
        SceneManager.LoadScene(3);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    public void AddPlayer()
    {
        numPlayers++;
        Debug.Log(numPlayers);
    }

    public void RemovePlayer()
    {
        numPlayers = numPlayers - 1;
        Debug.Log(numPlayers);
    }

    public void ChangeGameMode()
    {
        // gm_index 0 = classic, gm_index 1 = abridged
        if (gm_index == 0)
        {
            GameObject.Find("GameData").GetComponent<GameData>().turboGame = true;
            gm_index = 1;
            buttonText.text = "Turbo";
            gmDescription.text =
                "In Turbo Mode, players race to gather as much wealth as they can before the timer runs out!";
        } else if (gm_index == 1)
        {
            GameObject.Find("GameData").GetComponent<GameData>().turboGame = false;
            gm_index = 0;
            buttonText.text = "Classic";
            gmDescription.text = "In Classic Mode, players must trade away until only one Tycoon is left standing...";
        }
    }

    public void ChangeTheme()
    {
        kingsley_classic = Resources.Load<Sprite>("Kingsleys/kingsley monopoly man");
        kingsley_yoda = Resources.Load<Sprite>("Kingsleys/kingsley_yoda2");
        
        // theme_index 0 = classic, theme_index 1 = Star Wars
        if (theme_index == 0)
        {
            GameObject.Find("GameData").GetComponent<GameData>().starWarsTheme = true;
            theme_index = 1;
            buttonText.text = "Star Wars";
            themePreview.sprite = kingsley_yoda;
        } else if (theme_index == 1)
        {
            GameObject.Find("GameData").GetComponent<GameData>().starWarsTheme = false;
            theme_index = 0;
            buttonText.text = "Classic";
            themePreview.sprite = kingsley_classic;
        }
    } 
    public void Options()
    {
        GameObject options = GameObject.Find("InGameOptionsPopUp(Clone)");
        if(options) { Destroy(options); }
        OptionsPopUp popup = OptionsPopUp.Create(transform.parent);
        popup.btn1.gameObject.SetActive(false);
        popup.btn2.GetComponentInChildren<Text>().text = "Load Game";
        popup.btn2.onClick.AddListener(() => SaveLoadPopUp.Create(transform.parent,false));
        popup.btn3.GetComponentInChildren<Text>().text = "OK";
        popup.btn3.onClick.AddListener(popup.closePopup);
    }


}
