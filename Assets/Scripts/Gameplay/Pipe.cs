using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour {
    
    public float pipeRadius = 1;
    public float curveRadius = 3;
    public float segmentLength = 1;
    [SerializeField] private int columns, rows;

    private Mesh mesh;
	private Vector3[] vertices;
	private int[] triangles;

	private void Awake () {
		GetComponent<MeshFilter>().mesh = mesh = new Mesh();
		mesh.name = "Pipe";
		// SetVertices();
		SetTriangles();
        mesh.RecalculateNormals();
	}
	
	// private void SetVertices() {
	// 	vertices = new Vector3[rows * columns * 4];
	// 	float uStep = (2f * Mathf.PI) / columns;
	// 	CreateFirstQuadRing(uStep);
    //     int iDelta = rows * 4;
	// 	for (int u = 2, i = iDelta; u <= columns; u++, i += iDelta) {
	// 		CreateQuadRing(u * uStep, i);
	// 	}
	// 	mesh.vertices = vertices;
	// }

    // private void CreateFirstQuadRing (float u) {
	// 	float vStep = (2f * Mathf.PI) / rows;

	// 	Vector3 vertexA = GetPointOnTorus(0f, 0f);
	// 	Vector3 vertexB = GetPointOnTorus(u, 0f);
	// 	for (int v = 1, i = 0; v <= rows; v++, i += 4) {
	// 		vertices[i] = vertexA;
	// 		vertices[i + 1] = vertexA = GetPointOnTorus(0f, v * vStep);
	// 		vertices[i + 2] = vertexB;
	// 		vertices[i + 3] = vertexB = GetPointOnTorus(u, v * vStep);
	// 	}
	// }

    // private void CreateQuadRing (float u, int i) {
	// 	float vStep = (2f * Mathf.PI) / rows;
	// 	int ringOffset = rows * 4;
		
	// 	Vector3 vertex = GetPointOnTorus(u, 0f);
	// 	for (int v = 1; v <= rows; v++, i += 4) {
	// 		vertices[i] = vertices[i - ringOffset + 2];
	// 		vertices[i + 1] = vertices[i - ringOffset + 3];
	// 		vertices[i + 2] = vertex;
	// 		vertices[i + 3] = vertex = GetPointOnTorus(u, v * vStep);
	// 	}
	// }

	private void SetTriangles() {
		triangles = new int[rows * columns * 6];
        for (int t = 0, i = 0; t < triangles.Length; t += 6, i += 4) {
			triangles[t] = i;
			triangles[t + 1] = triangles[t + 4] = i + 1;
			triangles[t + 2] = triangles[t + 3] = i + 2;
			triangles[t + 5] = i + 3;
		}
		mesh.triangles = triangles;
	}

    private Vector3 GetMeshPoint (float xyAngle, float xzAngle) {
		Vector3 p;
		p.x = (pipeRadius * Mathf.Cos(xyAngle));
        p.x += (curveRadius * Mathf.Cos(xzAngle));
		p.y = pipeRadius * Mathf.Sin(xyAngle);
        p.z = (curveRadius * Mathf.Sin(xzAngle));
		return p;
	}

    private void OnDrawGizmos () {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
		mesh.name = "Pipe";
		// SetVertices();
		// SetTriangles();
        // mesh.RecalculateNormals();

		// x 
		// cstep = (2f * Mathf.PI) / columns;
		// for int i = 0; i < columns
		// 	x = cos(i * cstep)
		// 	y = sen(i * cstep)

		float uStep = (2f * Mathf.PI) / columns;
		float vStep = (2f * Mathf.PI) / rows;

		for (int u = 0; u < columns; u++) {
			for (int v = 0; v < rows; v++) {
				
				Vector3 point = GetMeshPoint(u * uStep, v * vStep);
				Gizmos.color = new Color(
					1f,
					(float)v / rows,
					(float)u / columns);
				Gizmos.DrawSphere(point, 0.1f);
			}
		}
	}

    
}
