using System;
using UnityEngine;

public class Hex : MonoBehaviour {

	public Vector3 Center;

	public int R;
	public int Q;

	Terrain Terrain;
	
}

public enum Terrain {
	Water,
	Land
}