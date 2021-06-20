using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrappingHandler : MonoBehaviour
{
    private enum Bound
    {
        Right,
        Top,
        Left,
        Down
    }

    private GameController _gameController;
    private Transform _transform;
    private Dictionary<Bound, float> _bounds;
    private float _returnToScreenX;
    private float _returnToScreenY;

    private void Awake()
    {
        _gameController = FindObjectOfType<GameController>();
        _transform = transform;

        _bounds = new Dictionary<Bound, float>
        {
            { Bound.Right, _gameController.WorldScreenResolution.x },
            { Bound.Top, _gameController.WorldScreenResolution.y },
            { Bound.Left, -_gameController.WorldScreenResolution.x },
            { Bound.Down, -_gameController.WorldScreenResolution.y },
        };

        _returnToScreenX = _gameController.WorldScreenResolution.x * 2;
        _returnToScreenY = _gameController.WorldScreenResolution.y * 2;
    }

    private void Update()
    {
        if (_transform.position.x > _bounds[Bound.Right])
        {
            ReturnToScreen(Bound.Right);
        }
        if (_transform.position.y > _bounds[Bound.Top])
        {
            ReturnToScreen(Bound.Top);
        }
        if (_transform.position.x < _bounds[Bound.Left])
        {
            ReturnToScreen(Bound.Left);
        }
        if (_transform.position.y < _bounds[Bound.Down])
        {
            ReturnToScreen(Bound.Down);
        }
    }

    private void ReturnToScreen(Bound fromBound)
    {
        switch (fromBound)
        {
            case Bound.Right:
                _transform.position += Vector3.left * _returnToScreenX;
                break;
            case Bound.Top:
                _transform.position += Vector3.down * _returnToScreenY;
                break;
            case Bound.Left:
                _transform.position += Vector3.right * _returnToScreenX;
                break;
            case Bound.Down:
                _transform.position += Vector3.up * _returnToScreenY;
                break;
        }
    }
}
