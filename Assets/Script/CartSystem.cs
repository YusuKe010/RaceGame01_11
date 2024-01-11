using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartSystem : MonoBehaviour
{
    /// <summary>
    /// 属性テクスチャ
    /// </summary>
    [SerializeField] Texture2D _attributeTexture = null;
    /// <summary>
    /// 簡易シングルトン
    /// </summary>
    static CartSystem _instance = null;
    static public CartSystem Instance => _instance;

    public enum Attribute
    {
        Road,
        Dart,
        Wall,

        Max
    }
    /// <summary>
    /// 1メートル位のピクセル数
    /// </summary>
    const float PIXEL_SCALE = 10.0f;
    const float PIXEL_SCALEREV = 1.0f / PIXEL_SCALE;

    void Start()
    {
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Attribute GetAttribute(Vector3 position)
    {
        int textureX = (int)(512 + position.x * PIXEL_SCALE + 0.5f);
        int textureY = (int)(512 + position.z * PIXEL_SCALE + 0.5f);
        Color[] pixels = _attributeTexture.GetPixels(textureX, textureY, 1, 1);
        if (pixels != null && pixels.Length > 0)
        {
            int r = (int)(pixels[0].r * 255.0f + 0.5f);
            int g = (int)(pixels[0].g * 255.0f + 0.5f);
            int b = (int)(pixels[0].b * 255.0f + 0.5f);
            if (90 <= r)
                return Attribute.Road;
            else if (75 <= r)
                return Attribute.Wall;
            else
                return Attribute.Dart;
        }
        Debug.Assert(false, "ここには来ないはず");
        return Attribute.Max;
    }
    /// <summary>
    /// 当たり判定の詳細
    /// </summary>
    /// <param name="position">更新後の位置</param>
    /// <param name="prevPosition">直前の位置</param>
    /// <param name="newPosition">壁からのめり込みが解消された位置</param>
    /// <returns></returns>
    public Vector3 GetAttributeDetail(Vector3 position, Vector3 prevPosition, out Vector3 newPosition)
    {
        Vector3 revVec = prevPosition - position;
        Attribute attr = Attribute.Wall;
        revVec = revVec.normalized * PIXEL_SCALEREV;
        while (attr == Attribute.Wall)
        {
            position += revVec;
            attr = GetAttribute(position);
        }
        newPosition = position; //壁からのめり込みが解消された位置
        position -= revVec;
        Attribute attrA = GetAttribute(position - new Vector3(PIXEL_SCALEREV, 0.0f, 0.0f));
        Attribute attrB = GetAttribute(position + new Vector3(PIXEL_SCALEREV, 0.0f, 0.0f));
        if (attrA == Attribute.Wall && attrB == Attribute.Wall)
        {
            if (revVec.z < 0.0f)
            {
                //下
                return Vector3.back;
            }
            else
            {
                //上
                return Vector3.forward;
            }
        }
        attrA = GetAttribute(position - new Vector3(0.0f, PIXEL_SCALEREV, 0.0f));
        attrB = GetAttribute(position + new Vector3(0.0f, PIXEL_SCALEREV, 0.0f));
        if (attrA == Attribute.Wall && attrB == Attribute.Wall)
        {
            if (revVec.x < 0.0f)
            {
                //左
                return Vector3.left;
            }
            else
            {
                //右
                return Vector3.right;
            }
        }
        Debug.Assert(false, "ここには来ないはず");
        return Vector3.zero;
    }
}
