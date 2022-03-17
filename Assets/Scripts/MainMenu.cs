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
    
    Color[] player_colors = {Color.blue,Color.red,Color.green,Color.yellow,Color.cyan,Color.magenta};
    
    public void GoToPlayerSelect()
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
                Debug.LogError("Players cannot have the same name");
                playerInfo.Clear();
                nameErrorFound = false;
                return;
            }

            if (pieceErrorFound)
            {
                Debug.LogError("Players cannot use the same piece");
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
        foreach(KeyValuePair<string, Tuple<Token, bool>> entry in playerInfo)
        {
            GameObject.Find("PersistentObject").GetComponent<PermObject>().players.Add(new Model.Player(entry.Key,entry.Value.Item1,entry.Value.Item2,player_colors[e]));
            e++;
        }
        SceneManager.LoadScene(2);
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
}
