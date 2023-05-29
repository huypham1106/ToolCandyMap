using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CandyMatch3.Game.Common;

public class PersonVip
{
    public int ratio;
    public List<int> times = new List<int>();
    public int turn;
    public Dictionary<string, int> character = new Dictionary<string, int>();
    public List<BoosterResource> gift = new List<BoosterResource>();
    public List<List<Goal>> random_required = new List<List<Goal>>();
    public int skin;
}
