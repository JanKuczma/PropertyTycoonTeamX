using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Space = Model.Space;

public class PlayerTests
{
    private GameData _gameData;

    
    [SetUp]
    public void Setup()
    {
        
    }
    
    [Test]
    public void PlayerReceiveCashTest()
    {
        // Use the Assert class to test conditions
        Model.Player player = new Model.Player("John", Token.CAT, true, (int)Group.RED, 1, 1000);
        player.ReceiveCash(100);
        Assert.AreEqual(1100,player.cash);
    }
    
    [Test]
    public void PlayerPayCashTest1()
    {
        // Use the Assert class to test conditions
        Model.Player player = new Model.Player("John", Token.CAT, true, (int)Group.RED, 1, 1000);
        Assert.AreEqual(player.PayCash(100),Model.Decision_outcome.SUCCESSFUL);
        Assert.AreEqual(900,player.cash);
    }
    [Test]
    public void PlayerPayCashTest2()
    {
        // Use the Assert class to test conditions
        Model.Player player1 = new Model.Player("John1", Token.CAT, true, (int)Group.RED, 1, 1000);
        Model.Player player2 = new Model.Player("John2", Token.SHIP, true, (int)Group.RED, 1, 1000);
        Assert.AreEqual(player1.PayCash(100,player2),Model.Decision_outcome.SUCCESSFUL);
        Assert.AreEqual(900,player1.cash);
        Assert.AreEqual(1100,player2.cash);
    }
    [Test]
    public void PlayerPayCashTest3()
    {
        // Use the Assert class to test conditions
        Model.Player player = new Model.Player("John", Token.CAT, true, (int)Group.RED, 1, 1000);
        player.owned_spaces.Add(new Space.Station(6,"station",200,new int[] {0,1,2,3}));
        Assert.AreEqual(player.PayCash(1100),Model.Decision_outcome.NOT_ENOUGH_MONEY);
        Assert.AreEqual(1000,player.cash);
    }
    [Test]
    public void PlayerPayCashTest4()
    {
        // Use the Assert class to test conditions
        Model.Player player = new Model.Player("John", Token.CAT, true, (int)Group.RED, 1, 1000);
        Assert.AreEqual(player.PayCash(1100),Model.Decision_outcome.NOT_ENOUGH_ASSETS);
        Assert.AreEqual(1000,player.cash);
    }
}
