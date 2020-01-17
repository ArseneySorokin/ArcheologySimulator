using System.Collections;
using Unity.Collections;
using Unity.Jobs;
using System.Collections.Generic;
using UnityEngine;

public class PlaneManipulation : MonoBehaviour
{
    public float vertexSpeed = 1;
    public float maxVertexDistance;

    public float upDistance;
    Mesh mesh;
    public LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        Debug.Log(mesh.vertexCount);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3[] vertices = mesh.vertices;
        RaycastHit[] results = RaycastJob(mesh.vertices, -transform.up);

        for(int i = 0; i < vertices.Length; i++)
        {
            if(results[i].collider != null)
            {
                Vector3 targetPoint = transform.InverseTransformPoint(results[i].point);
                Vector3 pt = Vector3.MoveTowards(vertices[i], targetPoint, vertexSpeed);
                vertices[i] = pt;
            }
           
        }
        
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
    }

    private RaycastHit[] RaycastJob(Vector3[] vertex, Vector3 direction)
    {
        var results = new NativeArray<RaycastHit>(vertex.Length, Allocator.TempJob);
        var commands = new NativeArray<RaycastCommand>(vertex.Length, Allocator.TempJob);

        for (int i = 0; i < vertex.Length; i++)
        {
            Ray ray = new Ray(transform.TransformPoint(vertex[i]) + transform.up * upDistance, direction);
            commands[i] = new RaycastCommand(ray.origin, ray.direction, 10, layerMask);
        }
        JobHandle handle = RaycastCommand.ScheduleBatch(commands, results, 1, default(JobHandle));
        handle.Complete();
        RaycastHit[] batchedHit = results.ToArray();
        results.Dispose();
        commands.Dispose();
        return batchedHit;
    }
}
