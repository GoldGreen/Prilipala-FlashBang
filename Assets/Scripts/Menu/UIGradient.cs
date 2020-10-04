using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Effects/Gradient")]
public class UIGradient : BaseMeshEffect
{
    public Color Color1 { get => color1; set => color1 = value; }
    [SerializeField] private Color color1 = Color.white;

    public Color Color2 { get => color2; set => color2 = value; }
    [SerializeField] private Color color2 = Color.white;

    public float Angle { get => angle; set => angle = value; }
    [Range(-180f, 180f)]
    [SerializeField] private float angle = 0f;
    [SerializeField] private bool ignoreRatio = true;


    public override void ModifyMesh(VertexHelper vh)
    {
        if (enabled)
        {
            var rect = graphic.rectTransform.rect;
            var dir = UIGradientUtils.RotationDir(angle);

            if (!ignoreRatio)
            {
                dir = UIGradientUtils.CompensateAspectRatio(rect, dir);
            }

            var localPositionMatrix = UIGradientUtils.LocalPositionMatrix(rect, dir);

            UIVertex vertex = default;
            for (int i = 0; i < vh.currentVertCount; i++)
            {
                vh.PopulateUIVertex(ref vertex, i);
                var localPosition = localPositionMatrix * vertex.position;
                vertex.color *= Color.Lerp(color2, color1, localPosition.y);
                vh.SetUIVertex(vertex, i);
            }
        }
    }

    public void UpdateGradient()
    {
        OnValidate();
    }
}
