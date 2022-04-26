using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BoardTests
{
    private Model.Board board;
    // A Test behaves as an ordinary method
    [SetUp]
    public void Setup()
    {
        board = Model.BoardData.LoadBoard();
    }
    [Test]
    public void BoardTestAllStations()
    {
        Assert.AreEqual(4,board.allStations().Count);
    }
    [Test]
    public void BoardTestAllUtilities()
    {
        Assert.AreEqual(2,board.allUtilities().Count);
    }
    [Test]
    public void BoardTestAllPropertiesInGroup()
    {
        Assert.AreEqual(2,board.allPropertiesInGroup(Group.BROWN).Count);
        Assert.AreEqual(2,board.allPropertiesInGroup(Group.DEEPBLUE).Count);
        Assert.AreEqual(3,board.allPropertiesInGroup(Group.RED).Count);
        Assert.AreEqual(3,board.allPropertiesInGroup(Group.GREEN).Count);
        Assert.AreEqual(3,board.allPropertiesInGroup(Group.BLUE).Count);
        Assert.AreEqual(3,board.allPropertiesInGroup(Group.PURPLE).Count);
        Assert.AreEqual(3,board.allPropertiesInGroup(Group.YELLOW).Count);
        Assert.AreEqual(3,board.allPropertiesInGroup(Group.ORANGE).Count);
    }
}
