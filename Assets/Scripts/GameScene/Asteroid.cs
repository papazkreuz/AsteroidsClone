using UnityEngine;

public class Asteroid : MonoBehaviour, IExplodable
{
    private enum AsteroidSize
    {
        Large,
        Medium,
        Small
    }

    private const int SCORE_VALUE_LARGE = 20;
    private const int SCORE_VALUE_MEDIUM = 50;
    private const int SCORE_VALUE_SMALL = 100;
    private const float BASE_SPEED = 1f;
    private const float MAX_ADDITIONAL_SPEED_MEDIUM = 1f;
    private const float MAX_ADDITIONAL_SPEED_SMALL = 2f;

    private AsteroidSize _asteroidSize;
    private GameController _gameController;
    private Rigidbody2D _rigidbody;
    private float _asteroidSpeed;
    private int _scoreValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IExplodable>() != null)
        {
            Ufo collisionUfo = collision.GetComponent<Ufo>();
            Bullet collisionBullet = collision.GetComponent<Bullet>();
            Asteroid collisionAsteroid = collision.GetComponent<Asteroid>();

            if (collisionUfo != null)
            {
                _scoreValue = 0;
            }

            if (collisionBullet != null)
            {
                if (collisionBullet.GetBulletTarget() == Bullet.BulletTarget.SpaceShip)
                {
                    _scoreValue = 0;
                }
            }

            if (collisionAsteroid == null)
                Explode();
        }
    }

    private void Start()
    {
        _gameController = FindObjectOfType<GameController>();
        _rigidbody = GetComponent<Rigidbody2D>();

        switch (_asteroidSize)
        {
            case (AsteroidSize.Large):
                _asteroidSpeed = BASE_SPEED;
                _scoreValue = SCORE_VALUE_LARGE;
                break;
            case (AsteroidSize.Medium):
                _asteroidSpeed = BASE_SPEED + Random.Range(0f, MAX_ADDITIONAL_SPEED_MEDIUM);
                _scoreValue = SCORE_VALUE_MEDIUM;
                break;
            case (AsteroidSize.Small):
                _asteroidSpeed = BASE_SPEED + Random.Range(0f, MAX_ADDITIONAL_SPEED_SMALL);
                _scoreValue = SCORE_VALUE_SMALL;
                break;
        }

        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        _rigidbody.velocity = randomDirection * _asteroidSpeed;
    }

    public void Explode()
    {
        switch (_asteroidSize) {
            case (AsteroidSize.Large):
            case (AsteroidSize.Medium):
                for (int i = 0; i < 2; i++)
                {
                    GameObject createdAsteroid = Instantiate(gameObject);
                    _gameController.AddToCreatedAsteroids(createdAsteroid);
                    createdAsteroid.transform.localScale = transform.localScale / 2f;
                    createdAsteroid.GetComponent<Asteroid>()._asteroidSize = this._asteroidSize + 1;
                }
                break;
            case (AsteroidSize.Small):
                break;
        }

        _gameController.AddScore(_scoreValue);
        _gameController.RemoveFromCreatedAsteroids(gameObject);
        Destroy(gameObject);
    }
}
