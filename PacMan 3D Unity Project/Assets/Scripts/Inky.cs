using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Security.Cryptography;
using UnityEngine;

public class Inky : Ghost
{
	// Start is called before the first frame update

	public Transform InkyTarget;

	public static Inky Instance { get; private set; }

	public override void Awake()
	{
		Instance = this;
		base.Awake();
	}
}
