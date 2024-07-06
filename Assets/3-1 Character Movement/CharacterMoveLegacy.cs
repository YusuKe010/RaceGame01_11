using UnityEngine;

/// <summary>
/// 古いやり方でのキャラクター操作スキーマを実現する。
/// （いわゆる「ラジコン操作」）
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class CharacterMoveLegacy : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 3f;
    [SerializeField] float _rotateSpeed = 3f;
    Rigidbody _rb = default;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        this.transform.Rotate(this.transform.up, _rotateSpeed * h * Time.deltaTime);
        _rb.velocity = this.transform.forward * v * _moveSpeed;
    }
}
