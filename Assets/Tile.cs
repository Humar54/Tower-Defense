using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Tile
{

    public bool _isbuilDabble;
    public bool _hasBeenBuilt;

    public Tile(bool isBuildable, bool hasBeenBuild)
    {
        _isbuilDabble = isBuildable;
        _hasBeenBuilt = hasBeenBuild;
    }

}
