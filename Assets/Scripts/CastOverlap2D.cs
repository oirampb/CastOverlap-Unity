using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastOverlap2D : MonoBehaviour
{

    public const string OnCastOverlapEnter = "OnCastOverlapEnter";
    public const string OnCastOverlapExecute = "OnCastOverlapExecute";
    public const string OnCastOverlapExit = "OnCastOverlapExit";

    public enum CastOverlap
    {
        RayCast,
        CircleCast,
        OverlapCircle,
        OverlapBox,
        BoxCastAll
    }
    public CastOverlap _castOverlap = CastOverlap.RayCast;

    public bool _drawGizmo = true;

    public float _radius = 2f;

    public Vector2 _size = Vector2.one;

    public float _angle = 1f;

    public bool _onCastOverlapExecute = false;

    // Update is called once per frame
    private void Update()
    {

        _isLeftClick = Input.GetMouseButton(0);

        if (Input.GetMouseButtonUp(0))
        {
            if (_currentHit2D.transform != null)
            {
                _currentHit2D.transform.SendMessage(OnCastOverlapExecute, true, SendMessageOptions.DontRequireReceiver);
            }
            if (_currentHit2D22 != null)
            {
                for (int i = 0; i < _currentHit2D22.Length; i++)
                {
                    _currentHit2D22[i].transform.SendMessage(OnCastOverlapExecute, true, SendMessageOptions.DontRequireReceiver);

                }
            }
            if (_currentCollider2D != null)
                _currentCollider2D.transform.SendMessage(OnCastOverlapExecute, true, SendMessageOptions.DontRequireReceiver);
            CheckHit2D(new RaycastHit2D());
            CheckCollider2D(null);
            CheckHit2DArray(null);
        }

        if (!_isLeftClick)
        {

            _onCastOverlapExecute = false;
            return;
        }

        _worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _position = this.transform.position;
        _direction = _worldMousePos - _position;

        RaycastHit2D hit2D = new RaycastHit2D();
        Collider2D collider2D = null;
        RaycastHit2D[] hit2DArray = null;

        if (_castOverlap == CastOverlap.RayCast)
        {
            hit2D = LaunchRayCast();
        }
        else if (_castOverlap == CastOverlap.CircleCast)
        {
            hit2D = LaunchCircleCast();
        }
        else if (_castOverlap == CastOverlap.BoxCastAll)
        {
            hit2DArray = LaunchBoxCastAll();
        }
        else if (_castOverlap == CastOverlap.OverlapCircle)
        {
            collider2D = LaunchOverlapCircle();
        }
        else if (_castOverlap == CastOverlap.OverlapBox)
        {
            collider2D = LaunchOverlapBox();
        }
        CheckHit2D(hit2D);
        CheckCollider2D(collider2D);
        CheckHit2DArray(hit2DArray);
    }

    private RaycastHit2D LaunchRayCast()
    {
        return Physics2D.Raycast(_position, _direction, _direction.magnitude);
    }

    private RaycastHit2D LaunchCircleCast()
    {
        return Physics2D.CircleCast(_position, _radius, _direction, _direction.magnitude);
    }

    private RaycastHit2D[] LaunchBoxCastAll()
    {
        return Physics2D.BoxCastAll(_position, _size, _angle, _direction, _direction.magnitude);
    }
    private Collider2D LaunchOverlapCircle()
    {
        return Physics2D.OverlapCircle(_worldMousePos, _radius);
    }

    private Collider2D LaunchOverlapBox()
    {
        return Physics2D.OverlapBox(_worldMousePos, _size, _angle);
    }

    private void CheckHit2D(RaycastHit2D hit2D)
    {

        if (hit2D.transform != null)
        {
            if (_currentHit2D.transform != null && _currentHit2D.transform != hit2D.transform)
            {
                _currentHit2D.transform.SendMessage(OnCastOverlapExit, null, SendMessageOptions.DontRequireReceiver);
            }
            hit2D.transform.SendMessage(OnCastOverlapEnter, null, SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            if (_currentHit2D.transform != null)
            {
                _currentHit2D.transform.SendMessage(OnCastOverlapExit, null, SendMessageOptions.DontRequireReceiver);
            }
        }
        _currentHit2D = hit2D;
    }
    private void CheckHit2DArray(RaycastHit2D[] currentHit2DBoxCastAll)
    {

        if (currentHit2DBoxCastAll != null)
        {
            if (_currentHit2D22 != null)
            {
                for (int i = 0; i < _currentHit2D22.Length; i++)
                {
                    bool exit = false;
                    for (int e = 0; e < currentHit2DBoxCastAll.Length; e++)
                    {
                        if (_currentHit2D22[i] != currentHit2DBoxCastAll[e])
                        {
                            exit = true;
                        }
                    }
                    if (!exit)
                    {
                        _currentHit2D22[i].transform.SendMessage(OnCastOverlapExit, null, SendMessageOptions.DontRequireReceiver);
                    }

                }

            }
            for (int i = 0; i < currentHit2DBoxCastAll.Length; i++)
            {
                currentHit2DBoxCastAll[i].transform.SendMessage(OnCastOverlapEnter, null, SendMessageOptions.DontRequireReceiver);
            }
        }
        else
        {
            if (_currentHit2D22 != null)
            {
                for (int i = 0; i < _currentHit2D22.Length; i++)
                {
                    _currentHit2D22[i].transform.SendMessage(OnCastOverlapExit, null, SendMessageOptions.DontRequireReceiver);
                }
            }
        }
        _currentHit2D22 = currentHit2DBoxCastAll;
    }
    private void CheckCollider2D(Collider2D collider2D)
    {
        if (collider2D != null)
        {
            if (_currentCollider2D != null && _currentCollider2D != collider2D)
            {
                _currentCollider2D.SendMessage(OnCastOverlapExit, null, SendMessageOptions.DontRequireReceiver);
            }
            collider2D.SendMessage(OnCastOverlapEnter, null, SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            if (_currentCollider2D != null)
            {
                _currentCollider2D.SendMessage(OnCastOverlapExit, null, SendMessageOptions.DontRequireReceiver);
            }
        }
        _currentCollider2D = collider2D;
    }

    private void OnDrawGizmosSelected()
    {
        if (!_drawGizmo || !_isLeftClick)
        {
            return;
        }

        switch (_castOverlap)
        {
            case CastOverlap.RayCast:

                Gizmos.DrawRay(_position, _direction);
                break;

            case CastOverlap.CircleCast:

                Gizmos.DrawWireSphere(_position, _radius);
                Gizmos.DrawRay(_position, _direction);
                Gizmos.DrawWireSphere((_position + _direction) * 0.25f, _radius);
                Gizmos.DrawWireSphere((_position + _direction) * 0.5f, _radius);
                Gizmos.DrawWireSphere((_position + _direction) * 0.75f, _radius);
                Gizmos.DrawWireSphere(_position + _direction, _radius);
                break;
            case CastOverlap.BoxCastAll:
                Gizmos.DrawWireCube(_position, _size);
                Gizmos.DrawRay(_position, _direction);
                Gizmos.DrawWireCube((_position + _direction) * 0.25f, _size);
                Gizmos.DrawWireCube((_position + _direction) * 0.5f, _size);
                Gizmos.DrawWireCube((_position + _direction) * 0.75f, _size);
                Gizmos.DrawWireCube(_position + _direction, _size);
                break;
            case CastOverlap.OverlapCircle:

                Gizmos.DrawWireSphere(_worldMousePos, _radius);
                break;
            case CastOverlap.OverlapBox:

                Gizmos.DrawWireCube(_worldMousePos, _size);
                break;
        }
    }

    // Global
    private Vector2 _worldMousePos = Vector2.zero;
    private Vector2 _position = Vector2.zero;
    private Vector2 _direction = Vector2.zero;

    private bool _isLeftClick = false;

    private RaycastHit2D _currentHit2D;
    private RaycastHit2D[] _currentHit2D22;
    private Collider2D _currentCollider2D;
}
