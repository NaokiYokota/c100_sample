using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class SplitMeshComponent : BaseMeshEffect
{
    [SerializeField]
    private int _horizontalMeshNum = 1;

    private RectTransform _rectTransform;

    private RectTransform RectTransform
    {
        get
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }

            return _rectTransform;
        }
    }

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive())
        {
            return;
        }

        var output = ListPool<UIVertex>.Get();
        vh.GetUIVertexStream(output);
        SplitMesh(output);
        vh.Clear();
        vh.AddUIVertexTriangleStream(output);
        ListPool<UIVertex>.Release(output);
    }

    /// <summary>
    /// Meshを分割する処理
    /// </summary>
    /// <param name="vertices">頂点情報</param>
    private void SplitMesh(List<UIVertex> vertices)
    {
        // 頂点数
        int vertexTotalCount = vertices.Count;
        // 分割後の頂点数
        int splitVertexCapacity = vertexTotalCount * _horizontalMeshNum;
        // 頂点数が元のMeshより多ければ、Listの許容数を変更する
        if (vertices.Capacity < splitVertexCapacity)
        {
            vertices.Capacity = splitVertexCapacity;
        }

        // Meshの大きさ
        Vector2 sizeDelta = RectTransform.rect.size;

        // 分割するMeshの1辺の大きさ
        Vector2 size = new Vector2(sizeDelta.x / _horizontalMeshNum, sizeDelta.y);

        Vector2 pivot = RectTransform.pivot - new Vector2(0.5f, 0.5f);

        // 頂点の移動量
        var offset = (size - sizeDelta) / 2 + new Vector2(pivot.x * size.x, pivot.y * size.y) -
                     new Vector2(sizeDelta.x * pivot.x, sizeDelta.y * pivot.y);

        Vector3 baseUvPosition = Vector3.zero;
        Vector2 uvSize = new Vector2(vertices[4].uv0.x - vertices[0].uv0.x,
            vertices[1].uv0.y - vertices[0].uv0.y);

        // 元のMeshの位置調整
        for (int i = 0; i < vertexTotalCount; i++)
        {
            var vt = vertices[i];
            var position = vt.position;
            position.x /= _horizontalMeshNum;
            position.x += offset.x;
            vt.position = position;

            var uv = vt.uv0;
            int num = i % vertexTotalCount;

            switch (num)
            {
                case 0:
                    uv = vt.uv0;
                    baseUvPosition = uv;
                    break;
                case 1:
                    uv = baseUvPosition + new Vector3(0, 1f / uvSize.y, 0);
                    break;
                case 2:
                case 3:
                    uv = baseUvPosition + new Vector3(1f / _horizontalMeshNum * uvSize.x,
                        1f / uvSize.y, 0);
                    break;
                case 4:
                    uv = baseUvPosition + new Vector3(1f / _horizontalMeshNum * uvSize.x, 0, 0);
                    break;
                case 5:
                    uv = baseUvPosition;
                    break;
            }

            vt.uv0 = uv;
            vertices[i] = vt;
        }

        for (int i = vertexTotalCount; i < splitVertexCapacity; i++)
        {
            // 頂点番号取得
            var vertexNumber = i % vertexTotalCount;

            // Meshグループ
            int meshGroup = i / vertexTotalCount;

            // 元のMesh情報を取得
            var vt = vertices[vertexNumber];

            // 位置
            var position = vt.position;
            // 左へ移動
            position.x += meshGroup * size.x;
            // 位置更新
            vt.position = position;


            // UV調整
            Vector3 uv = vt.uv0;
            var addValue = (float)meshGroup / _horizontalMeshNum * uvSize.x;
            uv += new Vector3(addValue, 0f, 0f);
            vt.uv0 = uv;

            // 頂点追加
            vertices.Add(vt);
        }
    }
}
