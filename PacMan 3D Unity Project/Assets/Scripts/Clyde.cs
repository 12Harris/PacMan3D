using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clyde : Ghost
{
    public static Clyde Instance { get; private set; }

    public override void Awake()
    {
        Instance = this;
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
