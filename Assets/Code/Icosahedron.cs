using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Icosahedron : MonoBehaviour {

	public float Size = 1f;

	private List<IcoFace> _faces = new List<IcoFace>();

	public GameObject IcoFace;

	public float SphereExpansionFactor = 1f;

	public int NumSubdivisions;

	void Start() {
		// http://answers.yahoo.com/question/index?qid=20080108041441AAJCjEu
		float c = Size * 1 / Mathf.Sqrt (5);
		float s = Size * 2 / Mathf.Sqrt (5);
		float c1 = Mathf.Cos (2 * Mathf.PI / 5);
		float s1 = Mathf.Sin (2 * Mathf.PI / 5);
		float c2 = Mathf.Cos (Mathf.PI / 5);
		float s2 = Mathf.Sin (Mathf.PI / 5);

		Vector3 V1 = new Vector3 (0, Size, 0);
		Vector3 V2 = new Vector3 (s, c, 0);
		Vector3 V3 = new Vector3 (s * c1, c, s * s1);
		Vector3 V4 = new Vector3 (-1 * s * c2, c, s * s2);
		Vector3 V5 = new Vector3 (-1 * s * c2, c, -1 * s * s2);
		Vector3 V6 = new Vector3 (s * c1, c, -1 * s * s1);
		Vector3 V7 = new Vector3 (0, -1 * Size, 0);
		Vector3 V8 = new Vector3 (-1 * s, -1 * c, 0);
		Vector3 V9 = new Vector3 (-1 * s * c1, -1 * c, -1 * s * s1);
		Vector3 V10 = new Vector3 (s * c2, -1 * c, -1 * s * s2);
		Vector3 V11 = new Vector3 (s * c2, -1 * c, s * s2);
		Vector3 V12 = new Vector3 (-1 * s * c1, -1 * c, s * s1);

		CreateIcoFace (V1, V2, V3);

		CreateIcoFace (V1, V3, V4);
		CreateIcoFace (V1, V4, V5);
		CreateIcoFace (V1, V5, V6);
		CreateIcoFace (V1, V6, V2);

		CreateIcoFace (V7, V9, V8);
		CreateIcoFace (V7, V10, V9);
		CreateIcoFace (V7, V11, V10);
		CreateIcoFace (V7, V12, V11);
		CreateIcoFace (V7, V8, V12);

		CreateIcoFace (V3, V2, V11);
		CreateIcoFace (V11, V2, V10);
		CreateIcoFace (V2, V6, V10);
		CreateIcoFace (V10, V6, V9);
		CreateIcoFace (V6, V5, V9);
		CreateIcoFace (V9, V5, V8);
		CreateIcoFace (V5, V4, V8);
		CreateIcoFace (V8, V4, V12);
		CreateIcoFace (V4, V3, V12);
		CreateIcoFace (V12, V3, V11);

	}

	private void CreateIcoFace( Vector3 a, Vector3 b, Vector3 c ) {
		GameObject face = GameObject.Instantiate( IcoFace ) as GameObject;
		IcoFace icoFace = face.GetComponent<IcoFace> ();
		icoFace.A = a;
		icoFace.B = b;
		icoFace.C = c;
		icoFace.NumSubdivisions = NumSubdivisions;
		icoFace.SphereExpansionFactor = SphereExpansionFactor;
		face.transform.parent = this.transform;
		_faces.Add (icoFace);
	}

	void OnDrawGizmos() {
		// Draw a yellow sphere at the transform's position
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (transform.position, Size);
	}

	public void RefreshFaces() {
		foreach (IcoFace face in _faces) {
			face.SphereExpansionFactor = SphereExpansionFactor;
			face.NumSubdivisions = NumSubdivisions;
			face.RefreshFace();
		}
	}
}
