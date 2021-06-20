using System.Collections;
using UnityEngine;

public abstract class Ufo : MonoBehaviour, IExplodable
{
    private const float SHOOT_RATE = 1.0f;
    private const float BULLET_SPEED = 5f;
    private const float BULLET_SPAWN_RANGE = 0.5f;
    private const float PROBABILITY_OF_CHANGE_Y = 0.001f;
    private const float UFO_SPEED = 2f;

    [SerializeField] protected GameObject _bulletPrefab;
    protected int scoreValue;
    protected Transform _transform;
    protected Vector3 shootDirection;
    private GameController _gameController;
    private Vector3 _endPosition;

    private IEnumerator Moving()
    {
        while (_transform.position.x != _endPosition.x)
        {
            if (_transform.position.x > _endPosition.x)
            {
                _transform.position += Vector3.left * Mathf.Min(Time.deltaTime * UFO_SPEED, _transform.position.x - _endPosition.x);
            }
            else
            {
                _transform.position += Vector3.right * Mathf.Min(Time.deltaTime * UFO_SPEED, _endPosition.x - _transform.position.x);
            }

            if (_transform.position.y > _endPosition.y)
            {
                _transform.position += Vector3.down * Mathf.Min(Time.deltaTime * UFO_SPEED, _transform.position.y - _endPosition.y);
            }
            else if (_transform.position.y < _endPosition.y)
            {
                _transform.position += Vector3.up * Mathf.Min(Time.deltaTime * UFO_SPEED, _endPosition.y - _transform.position.y);
            }
            else
            {
                float randomProbability = Random.Range(0f, 1f);

                if (randomProbability < PROBABILITY_OF_CHANGE_Y)
                {
                    ChangeEndY();
                }
            }

            yield return null;
        }

        Destroy(gameObject);
        yield break;
    }

    private IEnumerator Shooting()
    {
        while (true)
        {
            yield return new WaitForSeconds(SHOOT_RATE);
            Shoot();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IExplodable>() != null)
        {
            Asteroid collisionAsteroid = collision.GetComponent<Asteroid>();

            if (collisionAsteroid != null)
            {
                scoreValue = 0;
            }

            Explode();
        }
    }

    protected virtual void Start()
    {
        _gameController = FindObjectOfType<GameController>();
        _transform = transform;

        _endPosition = new Vector3(-_transform.position.x, _transform.position.y);

        StartCoroutine(Moving());
        StartCoroutine(Shooting());
    }

    private void ChangeEndY()
    {
        if ((_endPosition + Vector3.up).y > _gameController.WorldScreenResolution.y)
        {
            _endPosition += Vector3.down;
            return;
        }

        if ((_endPosition + Vector3.down).y < -_gameController.WorldScreenResolution.y)
        {
            _endPosition += Vector3.up;
            return;
        }

        _endPosition += Random.Range(0, 2) == 0 ? Vector3.up : Vector3.down;
    }

    protected virtual void Shoot()
    {
        GameObject createdBullet = Instantiate(_bulletPrefab, _transform.position + shootDirection * BULLET_SPAWN_RANGE, Quaternion.identity);
        Bullet bullet = createdBullet.GetComponent<Bullet>();
        bullet.SetVelocity(shootDirection * BULLET_SPEED);
        bullet.SetBulletTarget(Bullet.BulletTarget.SpaceShip);
    }

    public void Explode()
    {
        _gameController.AddScore(scoreValue);
        Destroy(gameObject);
    }
}
