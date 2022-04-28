using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Space = Model.Space;

public class PlayerTests
{
    private GameData _gameData;
    private Model.Player player1;
    private Model.Player player2;
    

    
    [SetUp]
    public void Setup()
    {
        player1 = new Model.Player("John1", Token.CAT, true, (int)Group.RED, 1, 1000);
        player2 = new Model.Player("John2", Token.SHIP, true, (int)Group.RED, 1, 1000);
    }
    
    [Test]
    public void PlayerReceiveCashTest()
    {
        // simple receive 100
        player1.ReceiveCash(100);
        Assert.AreEqual(1100,player1.cash);
    }
    
    [Test]
    public void PlayerPayCashTest1()
    {
        // pay to bank
        Assert.AreEqual(player1.PayCash(100),Model.Decision_outcome.SUCCESSFUL);
        Assert.AreEqual(900,player1.cash);
    }
    [Test]
    public void PlayerPayCashTest2()
    {
        // pay to other player
        Assert.AreEqual(player1.PayCash(100,player2),Model.Decision_outcome.SUCCESSFUL);
        Assert.AreEqual(900,player1.cash);
        Assert.AreEqual(1100,player2.cash);
    }
    [Test]
    public void PlayerPayCashTest3()
    {
        // pay not enough money
        player1.owned_spaces.Add(new Space.Station(6,"station",200,new int[] {0,1,2,3}));
        Assert.AreEqual(player1.PayCash(1100),Model.Decision_outcome.NOT_ENOUGH_MONEY);
        Assert.AreEqual(1000,player1.cash);
    }
    [Test]
    public void PlayerPayCashTest4()
    {
        // pay not enough assets
        Assert.AreEqual(player1.PayCash(1100),Model.Decision_outcome.NOT_ENOUGH_ASSETS);
        Assert.AreEqual(1000,player1.cash);
    }
    [Test]
    public void PlayerPayCashTest5()
    {
        // pay to parking fees
        Model.Board board = new Model.Board(null);
        Assert.AreEqual(player1.PayCash(50,board: board),Model.Decision_outcome.SUCCESSFUL);
        Assert.AreEqual(950,player1.cash);
        Assert.AreEqual(50,board.parkingFees);
    }
    // assets with one property
    [Test]
    public void PlayerTotalAssetsTest1()
    {
        Model.Board board = Model.BoardData.LoadBoard();
        player1.BuyProperty((Model.Space.Purchasable)board.spaces[1]);
        Assert.AreEqual(((Model.Space.Purchasable)(board.spaces[1])).cost + player1.cash,player1.totalValueOfAssets());
    }
    // assets with two properties and one house
    [Test]
    public void PlayerTotalAssetsTest2()
    {
        Model.Board board = Model.BoardData.LoadBoard();
        player1.BuyProperty((Model.Space.Purchasable)board.spaces[1]);
        player1.BuyProperty((Model.Space.Purchasable)board.spaces[3]);
        ((Model.Space.Property)(player1.owned_spaces[0])).buyHouse(board);
        player1.ReceiveCash(70);
        Assert.AreEqual(((Model.Space.Purchasable)(player1.owned_spaces[0])).cost + ((Model.Space.Purchasable)(player1.owned_spaces[1])).cost + ((Model.Space.Property)(player1.owned_spaces[0])).house_cost + player1.cash,player1.totalValueOfAssets());
    }
    // assets with one mortgaged property
    [Test]
    public void PlayerTotalAssetsTest3()
    {
        Model.Board board = Model.BoardData.LoadBoard();
        player1.BuyProperty((Model.Space.Purchasable)board.spaces[1]);
        player1.owned_spaces[0].mortgage();
        player1.ReceiveCash(70);
        Assert.AreEqual(((Model.Space.Property)(player1.owned_spaces[0])).cost/2 + player1.cash,player1.totalValueOfAssets());
    }
}
