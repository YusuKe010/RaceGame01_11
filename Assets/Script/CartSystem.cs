using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartSystem : MonoBehaviour
{
    /// <summary>
    /// �����e�N�X�`��
    /// </summary>
    [SerializeField] Texture2D _attributeTexture = null;
    /// <summary>
    /// �ȈՃV���O���g��
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
    /// 1���[�g���ʂ̃s�N�Z����
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
        Debug.Assert(false, "�����ɂ͗��Ȃ��͂�");
        return Attribute.Max;
    }
    /// <summary>
    /// �����蔻��̏ڍ�
    /// </summary>
    /// <param name="position">�X�V��̈ʒu</param>
    /// <param name="prevPosition">���O�̈ʒu</param>
    /// <param name="newPosition">�ǂ���̂߂荞�݂��������ꂽ�ʒu</param>
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
        newPosition = position; //�ǂ���̂߂荞�݂��������ꂽ�ʒu
        position -= revVec;
        Attribute attrA = GetAttribute(position - new Vector3(PIXEL_SCALEREV, 0.0f, 0.0f));
        Attribute attrB = GetAttribute(position + new Vector3(PIXEL_SCALEREV, 0.0f, 0.0f));
        if (attrA == Attribute.Wall && attrB == Attribute.Wall)
        {
            if (revVec.z < 0.0f)
            {
                //��
                return Vector3.back;
            }
            else
            {
                //��
                return Vector3.forward;
            }
        }
        attrA = GetAttribute(position - new Vector3(0.0f, PIXEL_SCALEREV, 0.0f));
        attrB = GetAttribute(position + new Vector3(0.0f, PIXEL_SCALEREV, 0.0f));
        if (attrA == Attribute.Wall && attrB == Attribute.Wall)
        {
            if (revVec.x < 0.0f)
            {
                //��
                return Vector3.left;
            }
            else
            {
                //�E
                return Vector3.right;
            }
        }
        Debug.Assert(false, "�����ɂ͗��Ȃ��͂�");
        return Vector3.zero;
    }
}
