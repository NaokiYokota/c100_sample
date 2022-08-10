using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class UiWaveComponent : BaseMeshEffect
{
    [SerializeField]
    private float _swing = 20;

    [SerializeField]
    private float _waveSpeed = 2;

    [SerializeField]
    private float _positionOffset = 20;

    private float _totalTime = 0f;

    private void Update()
    {
        if (graphic != null)
        {
            graphic.SetVerticesDirty();
        }
    }

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive()) return;

        var vertices = ListPool<UIVertex>.Get();

        // 頂点情報を取得する
        vh.GetUIVertexStream(vertices);
        // 頂点を操作する
        MoveVertex(vertices);
        // 頂点情報をクリア
        vh.Clear();
        // 頂点情報を設定する
        vh.AddUIVertexTriangleStream(vertices);

        ListPool<UIVertex>.Release(vertices);
    }

    private void MoveVertex(List<UIVertex> vertices)
    {
        _totalTime += Time.deltaTime;
        for (int i = 0; i < vertices.Count; i++)
        {
            int vertexNum = i % 6;
            if (vertexNum == 1 || vertexNum == 2 || vertexNum == 3)
            {
                UIVertex vertex = vertices[i];

                float moveValue = _swing *
                                  Mathf.Sin((_totalTime * _waveSpeed) +
                                            (vertex.position.x * _positionOffset) * Mathf.Deg2Rad);

                vertex.position.y += moveValue;
                vertices[i] = vertex;
            }
        }
    }
}
