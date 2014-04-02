using UnityEngine;
using System.Collections;

public class BaseTile : MonoBehaviour {

	public float Size;
	public Vector3 Center;
	public Vector3 North;
	public Vector3 FaceNormal;
	public float SphereRadius;
	public float SphereExpansionFactor;

	protected Vector3[] _vertices;
	protected int[] _indices;
	protected Vector3[] _normals;
	protected Vector2[] _uvs;
	
	protected Vector3 _origin;
	protected Vector3 _normal;

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

		CreateMesh ();
		//Expand ();
		InitMesh ();
	}

	virtual protected int GetNumVerts() {
		return 1;
	}

	virtual protected int GetNumIndices() {
		return 1;
	}
	
	virtual protected void CreateMesh() {

	}

	private void InitMesh() {
		CreateMesh ();
		MeshFilter meshComp = GetComponent<MeshFilter> ();
		Mesh mesh = new Mesh ();
		meshComp.mesh = mesh;
		mesh.vertices = _vertices;
		mesh.triangles = _indices;
		mesh.normals = _normals;
		mesh.uv = _uvs;
	} 

	private void Expand() {


		Vector3[] worldVerts = new Vector3[_vertices.Length];
		for (int i=0, n=_vertices.Length; i < n; i++) {
			worldVerts[i] = this.transform.TransformPoint( _vertices[i] );
			worldVerts[i] = Vector3.Slerp(worldVerts[i], worldVerts[i].normalized * SphereRadius, SphereExpansionFactor);
		}

		this.transform.position = Vector3.Slerp(Center, Center.normalized * SphereRadius, SphereExpansionFactor);
		Vector3 sphereNormal = (Center - transform.parent.position).normalized;
		_normal = Vector3.Slerp (FaceNormal, sphereNormal, SphereExpansionFactor);
		this.transform.rotation = Quaternion.LookRotation (_normal, North);

		
		for (int i=0, n=_vertices.Length; i < n; i++) {
			_vertices[i] = this.transform.InverseTransformPoint( worldVerts[i] ); 
		}
		
	}
	
	private Vector3 ExpandToSphere(Vector3 point) {
		point = gameObject.transform.TransformPoint( point );
		point = Vector3.Slerp(point, point.normalized * SphereRadius, SphereExpansionFactor);
		return gameObject.transform.InverseTransformPoint( point);
	}
}
