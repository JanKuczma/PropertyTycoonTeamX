using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceTests : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        /*Model2.Space[] spaces = new Model2.Space[40];
        spaces[0] = new Model2.Space.Go(1,"GO",200);
        int[] rents = {2,10,30,90,160,250};
        spaces[1] = new Model2.Space.Property(2,"THE OLD CREEK",60,Group.BROWN,rents,50,250);
        spaces[2] = new Model2.Space.PotLuck(3,"POT LUCK");
        int[] rents2 = {4,20,60,180,320,450};
        spaces[3] = new Model2.Space.Property(4,"GANGSTER PARADISE",60,Group.BROWN,rents2,50,250);
        spaces[4] = new Model2.Space.Tax(5,"INCOME TAX",100);
        int[] srents = {25,50,100,200};
        spaces[5] = new Model2.Space.Station(6,"BRIGHTON STATION",200,srents);
        int[] rents4 = {6,30,90,270,400,550};
        spaces[6] = new Model2.Space.Property(7,"THE ANGELS DELIGHT",60,Group.BLUE,rents4,50,250);
        spaces[7] = new Model2.Space.Chance(8,"OPPORTUNITY KNOCKS");
        spaces[8] = new Model2.Space.Property(9,"POTTER AVENUE",100,Group.BLUE,rents4,50,250);
        int[] rents5 = {8,40,100,300,450,600};
        spaces[9] = new Model2.Space.Property(10,"GRANGER DRIVE",100,Group.BLUE,rents5,50,250);
        spaces[10] =new Model2.Space.VisitJail(11,"JUST VISITING");
        int[] rents6 = {10,50,150,450,625,750};
        spaces[11] =new Model2.Space.Property(12,"SKYWALKER DRIVE",140,Group.PURPLE,rents6,100,500);
        int[] urents = {4,10};
        spaces[12] =new Model2.Space.Utility(13,"TESLA POWER CO",150,urents);
        spaces[13] =new Model2.Space.Property(14,"WOOKIE HOLE",140,Group.PURPLE,rents6,100,500);
        int[] rents8 = {12,60,180,500,700,900};
        spaces[14] =new Model2.Space.Property(15,"REY LANE",160,Group.PURPLE,rents8,100,500);
        spaces[15] =new Model2.Space.Station(16,"HOVE STATION",200,srents);
        int[] rents9 = {14,70,200,550,750,950};
        spaces[16] =new Model2.Space.Property(17,"BISHOP DRIVE",180,Group.ORANGE,rents9,100,500);
        spaces[17] =new Model2.Space.PotLuck(18,"POT LUCK");
        spaces[18] =new Model2.Space.Property(19,"DUNHAM STREET",180,Group.ORANGE,rents9,100,500);
        int[] rents10 = {16,80,220,600,800,1000};
        spaces[19] =new Model2.Space.Property(20,"BROYLES LANE",200,Group.ORANGE,rents10,100,500);
        spaces[20] =new Model2.Space.FreeParking(21,"FREE PARKING");
        int[] rents11 = {18,90,250,700,875,1050};
        spaces[21] =new Model2.Space.Property(22,"YUE FEI SQUARE",220,Group.RED,rents11,150,750);
        spaces[22] =new Model2.Space.Chance(23,"OPPORTUNITY KNOCKS");
        spaces[23] =new Model2.Space.Property(24,"MILAN ROGUE",220,Group.RED,rents11,150,750);
        int[] rents12 = {20,100,300,750,925,1100};
        spaces[24] =new Model2.Space.Property(25,"HAN XIN GARDENS",240,Group.RED,rents12,150,750);
        spaces[25] =new Model2.Space.Station(26,"FALMER STATION",200,srents);
        int[] rents13 = {22,110,330,800,975,1150};
        spaces[26] =new Model2.Space.Property(27,"SHATNER CLOSE",260,Group.YELLOW,rents13,150,750);
        spaces[27] =new Model2.Space.Property(28,"PICARD AVENUE",260,Group.YELLOW,rents13,150,750);
        spaces[28] =new Model2.Space.Utility(29,"EDISON WATER",150,urents);
        spaces[29] =new Model2.Space.Property(30,"CRUSHER CREEK",280,Group.YELLOW,rents13,150,750);
        spaces[30] =new Model2.Space.GoToJail(31,"GO TO JAIL");
        int[] rents14 = {26,130,390,900,1100,1275};
        spaces[31] =new Model2.Space.Property(32,"SIRAT MEWS",300,Group.GREEN,rents14,200,100);
        spaces[32] =new Model2.Space.Property(33,"GENGHIS CRESCENT",300,Group.GREEN,rents14,200,1000);
        spaces[33] =new Model2.Space.PotLuck(34,"POT LUCK");
        int[] rents15 = {28,150,450,1000,1200,1400};
        spaces[34] =new Model2.Space.Property(35,"IBIS CLOSE",320,Group.GREEN,rents15,200,1000);
        spaces[35] =new Model2.Space.Station(36,"PORTSLADE STATION",200,srents);
        spaces[36] =new Model2.Space.Chance(37,"OPPORTUNITY KNOCKS");
        int[] rents16 = {35,175,500,1100,1300,1500};
        spaces[37] =new Model2.Space.Property(38,"JAMES WEBB WAY",350,Group.DEEPBLUE,rents16,200,1000);
        spaces[38] =new Model2.Space.Tax(39,"SUPER TAX",100);
        int[] rents17 = {50,200,600,1400,1700,2000};
        spaces[39] =new Model2.Space.Property(40,"TURING HEIGHTS",400,Group.DEEPBLUE,rents17,200,1000);
        Model2.Board tmp = new Model2.Board(spaces);
        Model2.BoardData.saveBoard(tmp);*/
        View.Board.Create(transform,Model2.BoardData.loadBoard(Asset.board_data_json()));
    }
}
