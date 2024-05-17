using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDataBase 
{

    private static UnitDataBase instance = new();
    public static UnitDataBase Instance { get { return instance; } private set { } }
    //  初始化每一个部队：  算子编号  师的编号   进场回合  行位置  列位置
    public UnitElement[] units =
    {
        new UnitElement(0,  "5", 0, 23, 35),
        new UnitElement(1,  "12", 0, 24, 33),
        new UnitElement(1,  "12", 0, 27, 31),
        new UnitElement(2,  "58", 0, 10, 31),
        new UnitElement(3,  "81", 0, 12, 34),
        new UnitElement(4,  "79", 0, 13, 33),
        new UnitElement(4,  "76", 0, 17, 32),
        new UnitElement(4,  "80", 0, 14, 33),
        new UnitElement(4,  "60", 0, 11, 30),
        new UnitElement(4,  "59", 0, 11, 31),
        new UnitElement(5,  "78", 0, 17, 27),
        new UnitElement(5,  "88", 0, 17, 28),
        new UnitElement(5,  "89", 0, 11, 31),
        new UnitElement(5,  "90", 0, 15, 33),
        new UnitElement(5,  "77", 0, 17, 33),
        new UnitElement(6,  "45", 0, 11, 30),
        new UnitElement(6,  "47", 0, 11, 31),
        new UnitElement(6,  "48", 0, 11, 31),
        new UnitElement(7,  "44", 0, 11, 30),
        new UnitElement(8,  "1", 0, 4, 37),
        new UnitElement(9,  "1Mar", 0, 10, 34),
        new UnitElement(9,  "1Mar", 0, 11, 32),
        new UnitElement(10,  "1Mar", 0, 10, 32),
        new UnitElement(11,  "1Mar", 0, 10, 34),
        new UnitElement(12,  "Grp", 0, 3, 38),
        new UnitElement(13,  "1Mar", 0, 5, 37),
        new UnitElement(14,  "3", 0, 3, 38),
        new UnitElement(14,  "7", 0, 10, 46),
        new UnitElement(15,  "3", 0, 7, 32),
        new UnitElement(15,  "3", 0, 1, 32),
        new UnitElement(15,  "3", 0, 1, 34),
        new UnitElement(15,  "7", 0, 12, 35),
        new UnitElement(15,  "7", 0, 22, 44),
        new UnitElement(15,  "7", 0, 19, 46),
        new UnitElement(16,  "3", 0, 3, 38),
        new UnitElement(17,  "Air1", 0, 4, 37),
        new UnitElement(18,  "Air2", 0, 4, 37),
    };
}
