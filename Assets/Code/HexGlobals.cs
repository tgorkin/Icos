using UnityEngine;
using System.Collections;

public class HexGlobals : MonoBehaviour {

	public const int NUM_VERTS_PER_HEX = 7;
	public const int NUM_INDICES_PER_HEX = 18;

	public static float Size = 0.5f;

	public static float Height { get; private set; }
	public static float Width { get; private set; }

	public static float VertDistance { get; private set; }
	public static float HorzDistance { get; private set; }

	// Use this for initialization
	void Awake () {
		Height = Size * 2f;
		Width = Mathf.Sqrt (3f) / 2f * Height;

		VertDistance = 3f / 4f * Height;
		HorzDistance = Width;
	}
}
