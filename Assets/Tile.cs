using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Tile
{

    public int _xPos;
    public int _zPos;
    public bool _isbuilDabble;
    public bool _hasBeenBuilt;

    public Tile(int x_Pos,int z_Pos,bool isBuildable, bool hasBeenBuild)
    {
        _xPos = x_Pos;
        _zPos = z_Pos;
        _isbuilDabble = isBuildable;
        _hasBeenBuilt = hasBeenBuild;
    }

}
