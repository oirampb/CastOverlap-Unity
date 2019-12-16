using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCastOverlapSpriteColor : MonoBehaviour
{

    public SpriteRenderer _spriteRenderer;
    void Update()
    {
        Fly();
        // OnCastOverlapExecute();
    }

    private void Fly()
    {
        if (_muve == 0)
        {
            Debug.Log("1");
            _direction = _position1.transform.position - this.transform.position;
            _direction = _direction.normalized;
            transform.Translate(_direction * _speed * Time.deltaTime, Space.Self);
            _distance = Vector3.Distance(this.transform.position, _position1.transform.position);
            if (_distance <= 0.1f)
            {
                this.transform.localRotation = Quaternion.Euler(0, 180, 0);
                _muve = 1;
            }
        }
        if (_muve == 1)
        {
            Debug.Log("2");
            _direction = _position2.transform.position + _position1.transform.position;
            _direction = _direction.normalized;
            transform.Translate(_direction * _speed * Time.deltaTime, Space.Self);
            _distance = Vector3.Distance(this.transform.position, _position2.transform.position);
            if (_distance <= 0.1f)
            {
                this.transform.localRotation = Quaternion.Euler(0, 0, 0);
                _muve = 0;
            }
        }
    }
    public void OnCastOverlapEnter()
    {
        _spriteRenderer.color = Color.red;
    }

    public void OnCastOverlapExit()
    {


        _spriteRenderer.color = Color.white;
    }
    public void OnCastOverlapExecute()
    {
        this.GetComponent<Rigidbody2D>().gravityScale = 5;
        _muve = 3;
        Destroy(gameObject, 1.5f);
    }
    [SerializeField]
    private GameObject _position1;
    [SerializeField]
    private GameObject _position2;


    private float _distance;
    private Vector3 _direction;
    [SerializeField]
    private float _speed = 1;
    private int _muve = 0;
}
