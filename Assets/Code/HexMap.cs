using UnityEngine;
using System.Collections;

public class HexMap : MonoBehaviour {

	public int XChunks;

	public int YChunks;

	public GameObject HexChunk;

	// Use this for initialization
	void Start () {
		for (int y=0; y < YChunks; y++) {
			for (int x=0; x< XChunks; x++) {
				CreateHexChunk(x, y);
			}
		}
	}
	
	private void CreateHexChunk(int x, int y) {
		GameObject chunk = GameObject.Instantiate (HexChunk) as GameObject;
		chunk.name = string.Format ("HexChunk {0},{1}", x, y);
		chunk.transform.parent = this.transform;
		chunk.transform.Translate (new Vector3(x * 43, y * 37) );
	}
}
