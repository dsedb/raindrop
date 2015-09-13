using UnityEngine;
using System.Collections;

public class Debris : MonoBehaviour
{
	const int POINT_MAX = 4096;
	private Vector3[] vertices_;
	private int[] indices_;
	private Color[] colors_;
	private Vector2[] uvs_;
	private float range_;
	private float rangeR_;
	private float move_ = 0f;
	private Matrix4x4 prev_view_matrix_;

	void Start ()
	{
		range_ = 32f;
		rangeR_ = 1.0f/range_;
		vertices_ = new Vector3[POINT_MAX*3];
		for (var i = 0; i < POINT_MAX; ++i) {
			float x = Random.Range (-range_, range_);
			float y = Random.Range (-range_, range_);
			float z = Random.Range (-range_, range_);
			var point = new Vector3(x, y, z);
			vertices_ [i*3+0] = point;
			vertices_ [i*3+1] = point;
			vertices_ [i*3+2] = point;
		}
		indices_ = new int[POINT_MAX*3];
		for (var i = 0; i < POINT_MAX*3; ++i) {
			indices_ [i] = i;
		}
		colors_ = new Color[POINT_MAX*3];
		for (var i = 0; i < POINT_MAX; ++i) {
			colors_ [i*3+0] = new Color (1f, 1f, 1f, 0f);
			colors_ [i*3+1] = new Color (1f, 1f, 1f, 1f);
			colors_ [i*3+2] = new Color (1f, 1f, 1f, 0f);
		}
		uvs_ = new Vector2[POINT_MAX*3];
		for (var i = 0; i < POINT_MAX; ++i) {
			uvs_ [i*3+0] = new Vector2 (1f, 0f);
			uvs_ [i*3+1] = new Vector2 (1f, 0f);
			uvs_ [i*3+2] = new Vector2 (0f, 1f);
		}
		Mesh mesh = new Mesh ();
		mesh.name = "debris";
		mesh.vertices = vertices_;
		mesh.colors = colors_;
		mesh.uv = uvs_;
		mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 99999999);
		var mf = GetComponent<MeshFilter> ();
		mf.sharedMesh = mesh;
		mf.mesh.SetIndices (indices_, MeshTopology.Lines, 0);
		prev_view_matrix_ = Camera.main.worldToCameraMatrix;
	}
	
	// Update is called once per frame
	void Update ()
	{
		var target_position = Camera.main.transform.TransformPoint(Vector3.forward * range_);
		var matrix = prev_view_matrix_ * Camera.main.cameraToWorldMatrix; // prev-view * inverted-cur-view
		var mr = GetComponent<Renderer> ();
		const float raindrop_speed = -1f;
		mr.material.SetFloat ("_Range", range_);
		mr.material.SetFloat ("_RangeR", rangeR_);
		mr.material.SetFloat ("_MoveTotal", move_);
		mr.material.SetFloat ("_Move", raindrop_speed);
		mr.material.SetVector ("_TargetPosition", target_position);
		mr.material.SetMatrix ("_PrevInvMatrix", matrix);
		move_ += raindrop_speed;
		move_ = Mathf.Repeat(move_, range_ * 2f);
		prev_view_matrix_ = Camera.main.worldToCameraMatrix;
	}
}
