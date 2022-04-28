using System.Collections;
using System.Collections.Generic;
using Model;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.TestTools;
using Space = Model.Space;

public class PurchasableSpaceTests
{
    private Model.Board _board;
    private Model.Player _player;
    private SoundManager _soundManager;

    [SetUp]
    public void Setup()
    {
        _board = Model.BoardData.LoadBoard();
        _player = new Model.Player("John1", Token.CAT, true, (int)Group.RED, 1, 3000);
    }
    // for successful purchase
    [Test]
    public void PropertySpaceBuyHouseTest1()
    {
        _player.BuyProperty((Model.Space.Purchasable)_board.spaces[1]);
        _player.BuyProperty((Model.Space.Purchasable)_board.spaces[3]);
        Assert.AreEqual(Model.Decision_outcome.SUCCESSFUL, ((Model.Space.Property)(_player.owned_spaces[0])).buyHouse(_board));
    }
    // if not all properties in group owned
    [Test]
    public void PropertySpaceBuyHouseTest2()
    {
        _player.BuyProperty((Model.Space.Purchasable)_board.spaces[1]);
        Assert.AreEqual(Model.Decision_outcome.NOT_ALL_PROPERTIES_IN_GROUP, ((Model.Space.Property)(_player.owned_spaces[0])).buyHouse(_board));
    }
    // house limit
    [Test]
    public void PropertySpaceBuyHouseTest3()
    {
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
        ((Model.Space.Property)(_player.owned_spaces[1])).buyHouse(_board);
        Assert.AreEqual(Model.Decision_outcome.MAX_HOUSES,((Model.Space.Property)(_player.owned_spaces[0])).buyHouse(_board));
    }
    // if difference in houses is invalid
    [Test]
    public void PropertySpaceBuyHouseTest4()
    {
        _player.BuyProperty((Model.Space.Purchasable)_board.spaces[1]);
        _player.BuyProperty((Model.Space.Purchasable)_board.spaces[3]);
        Assert.AreEqual(Model.Decision_outcome.SUCCESSFUL, ((Model.Space.Property)(_player.owned_spaces[0])).buyHouse(_board));
        Assert.AreEqual(Model.Decision_outcome.DIFFERENCE_IN_HOUSES, ((Model.Space.Property)(_player.owned_spaces[0])).buyHouse(_board));
    }
    // regular sale
    [Test]
    public void PropertySpaceSellHouseTest1()
    {
        _player.BuyProperty((Model.Space.Purchasable)_board.spaces[1]);
        _player.BuyProperty((Model.Space.Purchasable)_board.spaces[3]);
        ((Model.Space.Property)(_player.owned_spaces[0])).buyHouse(_board);
        Assert.AreEqual(Model.Decision_outcome.SUCCESSFUL,((Model.Space.Property)(_player.owned_spaces[0])).sellHouse(_board));
    }
    // if there are no houses
    [Test]
    public void PropertySpaceSellHouseTest2()
    {
        _player.BuyProperty((Model.Space.Purchasable)_board.spaces[1]);
        
        Assert.AreEqual(Model.Decision_outcome.NO_HOUSES,((Model.Space.Property)(_player.owned_spaces[0])).sellHouse(_board));
    }
    //if difference in houses is invalid
    [Test]
    public void PropertySpaceSellHouseTest3()
    {
        _player.BuyProperty((Model.Space.Purchasable)_board.spaces[1]);
        _player.BuyProperty((Model.Space.Purchasable)_board.spaces[3]);
        ((Model.Space.Property)(_player.owned_spaces[0])).buyHouse(_board);
        ((Model.Space.Property)(_player.owned_spaces[1])).buyHouse(_board);
        ((Model.Space.Property)(_player.owned_spaces[0])).buyHouse(_board);
        Assert.AreEqual(Model.Decision_outcome.DIFFERENCE_IN_HOUSES,((Model.Space.Property)(_player.owned_spaces[1])).sellHouse(_board));
    }
}
