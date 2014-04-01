using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
public class HexChunk : MonoBehaviour {

	public float Size;
	public Vector3 Center;
	public Vector3 North;
	public Vector3 FaceNormal;
	public float SphereRadius;
	public float SphereExpansionFactor = 1f;	

	private Vector3[] _vertices;
	private int[] _indices;
	private Vector3[] _normals;
	private Vector2[] _uvs;

	private Vector3 _origin;
	private Vector3 _normal;

	// Use this for initialization
	void Start () {
		int totalNumVerts = 7;
		int totalIndices = 18;

		_vertices = new Vector3[ totalNumVerts ];
		_indices = new int[ totalIndices ];
		_normals = new Vector3[ totalNumVerts ];
		_uvs = new Vector2[ totalNumVerts ];

		_origin = transform.parent.position;

		this.transform.position = Center;
		this._normal = FaceNormal;
		this.transform.rotation = Quaternion.LookRotation (_normal, North);

		CreateHexMesh();
		Expand ();

		MeshFilter meshComp = GetComponent<MeshFilter> ();
		Mesh mesh = new Mesh ();
		meshComp.mesh = mesh;
		mesh.vertices = _vertices;
		mesh.triangles = _indices;
		mesh.normals = _normals;
		mesh.uv = _uvs;
	}

	private void CreateHexMesh() {

		int vertOffset = 0;
		float x, y, angle;
		_vertices[vertOffset++] = new Vector3(0, 0, 0);
		for(int i=0; i < 6; i++) {
			angle = 2f * Mathf.PI / 6f * i;
			x = Size * Mathf.Cos(angle);
			y = Size * Mathf.Sin(angle);
			_vertices[vertOffset++] = new Vector3(x, y, 0);
		}
		vertOffset = 0;

		int indexOffset = 0;
		for(int i=0; i < 6; i++) {
			_indices[indexOffset++] = vertOffset;
			_indices[indexOffset++] = (i % 6) + 1 + vertOffset;
			_indices[indexOffset++] = ((i+1) % 6) + 1 + vertOffset;
		}

		vertOffset = 0;
		for (int i=0; i < 7; i++) {
			_normals[vertOffset++] = Vector3.up;
		}

		vertOffset = 0;
		_uvs [vertOffset++] = new Vector2 (0, 0);
		for (int i=1; i < 7; i++) {
			_uvs[vertOffset++] = new Vector2(1f, 0);
		}
	}

	private void Expand() {

		Vector3[] worldVerts = new Vector3[_vertices.Length];
		for (int i=0, n=_vertices.Length; i < n; i++) {
			worldVerts[i] = gameObject.transform.TransformPoint( _vertices[i] );
			worldVerts[i] = Vector3.Slerp(worldVerts[i], worldVerts[i].normalized * SphereRadius, SphereExpansionFactor);
		}

		this.transform.position = Vector3.Slerp(Center, Center.normalized * SphereRadius, SphereExpansionFactor);
		Vector3 sphereNormal = (Center - transform.parent.position).normalized;
		_normal = Vector3.Slerp (FaceNormal, sphereNormal, SphereExpansionFactor);
		this.transform.rotation = Quaternion.LookRotation (_normal, North);

		for (int i=0, n=_vertices.Length; i < n; i++) {
			_vertices[i] = gameObject.transform.InverseTransformPoint( worldVerts[i] ); 
		}
	}

	private Vector3 ExpandToSphere(Vector3 point) {
		point = gameObject.transform.TransformPoint( point );
		point = Vector3.Slerp(point, point.normalized * SphereRadius, SphereExpansionFactor);
		return gameObject.transform.InverseTransformPoint( point);
	}
}
