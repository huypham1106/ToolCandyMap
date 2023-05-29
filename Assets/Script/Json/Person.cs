using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CandyMatch3.Game.Common;

public class Person
{
    // Start is called before the first frame update

    public Dictionary<string, int> character = new Dictionary<string, int>();
    public List<BoosterResource> gift = new List<BoosterResource>();
    public List<Goal> require = new List<Goal>();
    public int skin;
}
