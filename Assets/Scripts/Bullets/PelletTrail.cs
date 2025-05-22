//using UnityEngine;
//using System.Collections.Generic;

//[RequireComponent(typeof(LineRenderer))]
//public class PelletTrail : MonoBehaviour
//{
//    private LineRenderer lineRenderer;
//    private List<Vector3> positions = new List<Vector3>();

//    void Awake()
//    {
//        lineRenderer = GetComponent<LineRenderer>();
//        positions.Add(transform.position);
//        lineRenderer.positionCount = 1;
//        lineRenderer.SetPosition(0, transform.position);
//    }

//    void Update()
//    {
//        if (Vector3.Distance(positions[positions.Count - 1], transform.position) > 0.05f)
//        {
//            positions.Add(transform.position);
//            lineRenderer.positionCount = positions.Count;
//            lineRenderer.SetPosition(positions.Count - 1, transform.position);
//        }
//    }

//    public void DetachAndDestroy(float delay)
//    {
//        // Создаём новый объект для следа
//        GameObject trailObj = new GameObject("PelletTrailDetached");
//        var lr = trailObj.AddComponent<LineRenderer>();
//        Debug.Log("Trail detached and will live for " + delay + " seconds");


//        // Копируем параметры LineRenderer
//        lr.positionCount = lineRenderer.positionCount;
//        Vector3[] points = new Vector3[lineRenderer.positionCount];
//        lineRenderer.GetPositions(points);
//        lr.SetPositions(points);
//        lr.material = lineRenderer.material;
//        lr.widthMultiplier = lineRenderer.widthMultiplier;
//        lr.startWidth = lineRenderer.startWidth;
//        lr.endWidth = lineRenderer.endWidth;
//        lr.startColor = lineRenderer.startColor;
//        lr.endColor = lineRenderer.endColor;
//        lr.numCapVertices = lineRenderer.numCapVertices;
//        lr.numCornerVertices = lineRenderer.numCornerVertices;
//        lr.shadowCastingMode = lineRenderer.shadowCastingMode;
//        lr.receiveShadows = lineRenderer.receiveShadows;
//        lr.alignment = lineRenderer.alignment;
//        lr.textureMode = lineRenderer.textureMode;
//        lr.sortingLayerID = lineRenderer.sortingLayerID;
//        lr.sortingOrder = lineRenderer.sortingOrder;
//        lr.useWorldSpace = true;

//        Destroy(trailObj, delay);

//        enabled = false;
//    }
//}
