using System;
using System.Collections;
using UnityEngine;

public class SpaceShip : MonoBehaviour, IExplodable
{
    private const float FORCE_MULTIPLIER = 2.5f;
    private const float ROTATION_SPEED = 150f;
    private const float HYPERSPACE_DELAY = 1.0f;
    private const float BULLET_SPEED = 10f;
    private const float BULLET_SPAWN_RANGE = 0.3f;

    [SerializeField] private GameObject _bulletPrefab;
    private GameController _gameController;
    private Transform _transform;
    private SpriteRenderer _spriteRenderer;
    private EdgeCollider2D _collider;
    private Rigidbody2D _rigidbody;

    public event Action OnSpaceShipExplodeEvent;

    private Coroutine hyperspaceCoroutine;
    
    private IEnumerator UsingHyperspace()
    {
        Hide();

        yield return new WaitForSeconds(HYPERSPACE_DELAY);

        _transform.position = _gameController.GetRandomPointOnScreen();
        Show();

        hyperspaceCoroutine = null;
        yield break;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IExplodable>() != null)
        {
            Explode();
        }
    }

    private void Start()
    {
        _gameController = FindObjectOfType<GameController>();
        _transform = transform;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<EdgeCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Hide()
    {
        _spriteRenderer.enabled = false;
        _collider.enabled = false;
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.simulated = false;
    }

    private void Show()
    {
        _spriteRenderer.enabled = true;
        _collider.enabled = true;
        _rigidbody.simulated = true;
    }

    public void AddForceForward()
    {
        _rigidbody.AddForce(_transform.right * FORCE_MULTIPLIER);
    }

    public void Rotate(bool clockwise)
    {
        float angle = clockwise ? -ROTATION_SPEED : ROTATION_SPEED;
        transform.Rotate(0f, 0f, angle * Time.deltaTime);
    }

    public void UseHyperspace()
    {
        if (hyperspaceCoroutine == null)
        {
            hyperspaceCoroutine = StartCoroutine(UsingHyperspace());
        }
    }

    public void Shoot()
    {
        GameObject createdBullet = Instantiate(_bulletPrefab, _transform.position + _transform.right.normalized * BULLET_SPAWN_RANGE, Quaternion.identity);
        Bullet bullet = createdBullet.GetComponent<Bullet>();
        bullet.SetVelocity(_transform.right.normalized * BULLET_SPEED);
        bullet.SetBulletTarget(Bullet.BulletTarget.Ufo);
    }

    public void Explode()
    {
        OnSpaceShipExplodeEvent?.Invoke();
        Destroy(gameObject);
    }
}
