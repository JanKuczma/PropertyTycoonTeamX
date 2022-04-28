using System.Collections;
using System.Collections.Generic;
using Model;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CardTests
{
    private Model.Board _board;
    private Model.Player _player;
    private SoundManager _soundManager;

    [SetUp]
    public void Setup()
    {
        _board = Model.BoardData.LoadBoard();
        _player = new Model.Player("John1", Token.CAT, true, (int)Group.RED, 1, 1000);
        _soundManager = new SoundManager();
    }
    [Test]
    public void CardRepairCostsTest()
    {
        
        Dictionary<string, int> kwargs = new Dictionary<string, int>();
        _player.BuyProperty((Model.Space.Purchasable)_board.spaces[1]);
        _player.BuyProperty((Model.Space.Purchasable)_board.spaces[3]);
        ((Model.Space.Property)(_player.owned_spaces[0])).buyHouse(_board);
        ((Model.Space.Property)(_player.owned_spaces[1])).buyHouse(_board);
        ((Model.Space.Property)(_player.owned_spaces[0])).buyHouse(_board);
        ((Model.Space.Property)(_player.owned_spaces[1])).buyHouse(_board);
        ((Model.Space.Property)(_player.owned_spaces[0])).buyHouse(_board);
        ((Model.Space.Property)(_player.owned_spaces[1])).buyHouse(_board);
        ((Model.Space.Property)(_player.owned_spaces[0])).buyHouse(_board);
        ((Model.Space.Property)(_player.owned_spaces[1])).buyHouse(_board);
        ((Model.Space.Property)(_player.owned_spaces[0])).buyHouse(_board);

        kwargs.Add("house",10);
        kwargs.Add("hotel",20);
        Model.Card card = new Card("Lol",CardAction.REPAIRS, kwargs);
        Assert.AreEqual(card.RepairsCost(_player),60);
    }
    
}
