using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BulletTarget
    {
        Ufo,
        SpaceShip
    }

    private const float BULLET_LIFETIME = 0.4f;

    private BulletTarget _bulletTarget;
    private CircleCollider2D _collider;
    private Rigidbody2D _rigidbody;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IExplodable>() != null)
        {
            Ufo collisionUfo = collision.GetComponent<Ufo>();
            SpaceShip collisionSpaceShip = collision.GetComponent<SpaceShip>();
            Asteroid collisionAsteroid = collision.GetComponent<Asteroid>();

            if (_bulletTarget == BulletTarget.Ufo)
            {
                if (collisionUfo != null)
                {
                    collisionUfo.Explode();
                    Destroy(gameObject);
                }
            }

            if (_bulletTarget == BulletTarget.SpaceShip)
            {
                if (collisionSpaceShip != null)
                {
                    collisionSpaceShip.Explode();
                    Destroy(gameObject);
                }
            }

            if (collisionAsteroid != null)
            {
                collisionAsteroid.Explode();
                Destroy(gameObject);
            }
        }
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(BULLET_LIFETIME);
        Destroy(gameObject);
    }

    public void SetVelocity(Vector2 _velocity)
    {
        _rigidbody.velocity = _velocity;
    }

    public BulletTarget GetBulletTarget()
    {
        return _bulletTarget;
    }

    public void SetBulletTarget(BulletTarget bulletTarget)
    {
        _bulletTarget = bulletTarget;
    }
}
