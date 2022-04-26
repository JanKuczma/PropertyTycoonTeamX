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

    [SetUp]
    public void Setup()
    {
        _board = Model.BoardData.LoadBoard();
        _player = new Model.Player("John1", Token.CAT, true, (int)Group.RED, 1, 1000);
    }
    [Test]
    public void PropertySpaceBuyHouseTest1()
    {
        _player.BuyProperty((Model.Space.Purchasable)_board.spaces[1]);
        _player.BuyProperty((Model.Space.Purchasable)_board.spaces[3]);
        Assert.AreEqual(((Model.Space.Property)(_player.owned_spaces[0])).buyHouse(_board),Model.Decision_outcome.SUCCESSFUL);
    }
    [Test]
    public void PropertySpaceBuyHouseTest2()
    {
        _player.BuyProperty((Model.Space.Purchasable)_board.spaces[1]);
        Assert.AreEqual(((Model.Space.Property)(_player.owned_spaces[0])).buyHouse(_board),Model.Decision_outcome.NOT_ALL_PROPERTIES_IN_GROUP);
    }
}
