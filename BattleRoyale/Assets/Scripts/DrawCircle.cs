using UnityEngine.Networking;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawCircle : NetworkBehaviour
{
    [SerializeField] int vertexCount = 40;
    [SerializeField] float lineWidht = 0.2f;

    [SyncVar]
    float radius;
    //[SerializeField] bool circleFillScreen;

    LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        SetUpCircle();
    }

    public void SetUpCircle()
    {
        lineRenderer.widthMultiplier = lineWidht;
        /*if (circleFillScreen)
        {
            radius = Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(0f, Camera.main.pixelRect.yMax, 0f)),
                                      Camera.main.ScreenToWorldPoint(new Vector3(0f, Camera.main.pixelRect.yMin, 0f))) * 0.5f - lineWidht;
        }*/

        float deltaTheta = (2f * Mathf.PI) / vertexCount;
        float theta = 0f;

        lineRenderer.positionCount = vertexCount;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            Vector3 pos = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0f);
            lineRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }

    }

    public void SetRadius(float _radius)
    {
        radius = _radius;
    }

/*#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        float deltaTheta = (2f * Mathf.PI) / vertexCount;
        float theta = 0f;

        Vector3 oldPos = Vector3.zero;
        for (int i = 0; i < vertexCount + 1; i++)
        {
            Vector3 pos = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0f);
            Gizmos.DrawLine(oldPos, transform.position + pos);
            oldPos = transform.position + pos;

            theta += deltaTheta;
        }
    }

#endif*/
}
