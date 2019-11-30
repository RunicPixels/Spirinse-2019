using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
[ExecuteInEditMode]
public class SetEdgeColliderPoints : MonoBehaviour
{
    public List<Transform> points;

    public EdgeCollider2D edgy;
    // Start is called before the first frame update
    private void Start()
    {
        edgy = GetComponent<EdgeCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        SetCollision();
    }

    [ClickableFunction]
    
    public void EditorSet()
    {
        SetCollision();
    }

    public void SetCollision()
    {
        Vector2[] newPoints = new Vector2[points.Count];
        for (int i = 0; i < points.Count; i++)
        {
            newPoints[i] = points[i].position- transform.position;
        }
        edgy.points = newPoints;
    }
    public void OnDrawGizmos()
    {
        
        Gizmos.color = new Color(255, 0, 255);
        for (int i = 0; i < points.Count - 1; i++)
        {
            var pos = points[i].position;
            var pos2 = points[i + 1].position;

            Gizmos.DrawLine(new Vector3(pos.x, pos.y), new Vector3(pos2.x,pos2.y));
        }
        Gizmos.color = new Color(255, 255, 0);
        for (int i = 0; i < edgy.pointCount - 1; i++)
        {
            var pos = edgy.points[i];
            var pos2 = edgy.points[i + 1];

            Gizmos.DrawLine(new Vector3(pos.x, pos.y), new Vector3(pos2.x, pos2.y));
        }
    }
}
