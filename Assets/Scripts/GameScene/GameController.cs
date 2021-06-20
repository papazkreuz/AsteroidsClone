using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private const float ASTEROID_START_ZONE_WIDTH = 3f;
    private const float UFO_START_X_OFFSET = 1f;
    private const int ASTEROIDS_COUNT_START = 4;
    private const int SPACESHIP_LIVES_START = 3;
    private const int SCORE_TO_INCREASE_LIVES = 10000;
    private const int MAX_SPACESHIP_LIVES = 10;

    [SerializeField] private GameObject _spaceShipPrefab;
    [SerializeField] private GameObject _asteroidPrefab;
    [SerializeField] private GameObject _bigUfoPrefab;
    [SerializeField] private GameObject _smallUfoPrefab;
    [SerializeField] private SpawnZone _spaceShipSpawnZone;
    [SerializeField] private InputController _inputController;
    [SerializeField] private LivesDisplay _livesDisplay;
    [SerializeField] private ScoreText _scoreText;
    [SerializeField] private Button _startButton;
    private GameObject _createdSpaceShip;
    private List<GameObject> _createdAsteroids;
    private GameObject _createdUfo;
    private Vector3 _screenResolution;
    private Vector3 _worldScreenResolution;
    private int _score;
    private int _asteroidsSpawnCount;
    private int _spaceShipLives;
    private int _bonusSpaceShipLivesGot;
    private bool _isGameInProgress = false;

    private IEnumerator CreatingAsteroids()
    {
        while (_isGameInProgress)
        {
            if (_createdAsteroids.Count == 0 && _createdUfo == null && _createdSpaceShip != null)
            {
                yield return new WaitForSeconds(1.5f);
                _asteroidsSpawnCount++;
                CreateAsteroids(_asteroidsSpawnCount);
            }

            yield return null;
        }

        yield break;
    }

    private IEnumerator CreatingUfos()
    {
        yield return new WaitForSeconds(30.0f);

        while (_isGameInProgress)
        {
            if (_createdUfo == null && _createdAsteroids.Count != 0 && _createdSpaceShip != null)
            {
                yield return new WaitForSeconds(Random.Range(3f, 10f));
                CreateUfo(Random.Range(0, 2) == 0);
            }

            yield return null;
        }

        yield break;
    }

    private IEnumerator CreatingSpaceShip()
    {
        while (_isGameInProgress)
        {
            if (_spaceShipLives <= 0)
            {
                EndGame();
                break;
            }

            if (_createdSpaceShip == null)
            {
                yield return new WaitForSeconds(0.5f);
                while (!_spaceShipSpawnZone.isReadyToSpawn)
                {
                    yield return null;
                }
                CreateSpaceShip();
            }

            yield return null;
        }

        yield break;
    }

    private void Awake()
    {
        _screenResolution = new Vector3(Screen.width, Screen.height);
        _worldScreenResolution = Camera.main.ScreenToWorldPoint(_screenResolution);
    }

    private void CreateSpaceShip()
    {
        _createdSpaceShip = Instantiate(_spaceShipPrefab);
        _createdSpaceShip.GetComponent<SpaceShip>().OnSpaceShipExplodeEvent += DecreaseSpaceShipLives;
        _inputController.Enable();
    }

    private void CreateAsteroids(int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            _createdAsteroids.Add(Instantiate(_asteroidPrefab, GetRandomAsteroidStartPoint(), Quaternion.identity));
        }
    }

    private void CreateUfo(bool isBig)
    {
        if (isBig)
            _createdUfo = Instantiate(_bigUfoPrefab, GetRandomUfoStartPoint(), Quaternion.identity);
        else
            _createdUfo = Instantiate(_smallUfoPrefab, GetRandomUfoStartPoint(), Quaternion.identity);
    }

    private void IncreaseSpaceShipLives()
    {
        if (_spaceShipLives < MAX_SPACESHIP_LIVES)
        {
            _spaceShipLives++;
            _livesDisplay.UpdateLivesCount(_spaceShipLives);
        }
    }

    private void DecreaseSpaceShipLives()
    {
        _spaceShipLives--;
        _livesDisplay.UpdateLivesCount(_spaceShipLives);
    }

    public void StartGame()
    {
        _score = 0;
        _scoreText.UpdateScore(_score);
        _asteroidsSpawnCount = ASTEROIDS_COUNT_START;
        _spaceShipLives = SPACESHIP_LIVES_START;
        _bonusSpaceShipLivesGot = 0;
        _isGameInProgress = true;

        _createdAsteroids = new List<GameObject>();
        CreateAsteroids(_asteroidsSpawnCount);
        StartCoroutine(CreatingAsteroids());
        StartCoroutine(CreatingUfos());
        StartCoroutine(CreatingSpaceShip());

        _livesDisplay.UpdateLivesCount(_spaceShipLives);
        _startButton.gameObject.SetActive(false);
    }

    public void EndGame()
    {
        _isGameInProgress = false;
        StopAllCoroutines();

        if (_createdUfo != null)
        {
            Destroy(_createdUfo);
        }

        if (_createdAsteroids.Count != 0)
        {
            foreach (GameObject createdAsteroid in _createdAsteroids)
            {
                Destroy(createdAsteroid);
            }
        }

        _startButton.gameObject.SetActive(true);
    }

    public void AddScore(int value)
    {
        _score += value;

        if (_score > (_bonusSpaceShipLivesGot + 1) * SCORE_TO_INCREASE_LIVES)
        {
            IncreaseSpaceShipLives();
            _bonusSpaceShipLivesGot++;
        }

        _scoreText.UpdateScore(_score);
    }

    public void AddToCreatedAsteroids(GameObject asteroid)
    {
        if (asteroid.GetComponent<Asteroid>() == null)
            throw new System.Exception("Trying to add not Asteroid to createdAsteroids");

        _createdAsteroids.Add(asteroid);
    }

    public void RemoveFromCreatedAsteroids(GameObject asteroid)
    {
        if (asteroid.GetComponent<Asteroid>() == null)
            throw new System.Exception("Trying to remove not Asteroid from createdAsteroids");

        if (!_createdAsteroids.Contains(asteroid))
            throw new System.Exception("Trying to remove asteroid, that is not in createdAsteroids");

        _createdAsteroids.Remove(asteroid);
    }

    public Vector3 GetRandomPointOnScreen()
    {
        float randomX = Random.Range(-_worldScreenResolution.x, _worldScreenResolution.x);
        float randomY = Random.Range(-_worldScreenResolution.y, _worldScreenResolution.y);

        return new Vector3(randomX, randomY);
    }

    public Vector3 GetRandomAsteroidStartPoint()
    {
        float randomX = Random.Range(0, 2) == 0 ?
                    Random.Range(-ASTEROID_START_ZONE_WIDTH, 0f) + _worldScreenResolution.x :
                    Random.Range(0f, ASTEROID_START_ZONE_WIDTH) - _worldScreenResolution.x;
        float randomY = Random.Range(0, 2) == 0 ?
            Random.Range(-ASTEROID_START_ZONE_WIDTH, 0f) + _worldScreenResolution.y :
            Random.Range(0f, ASTEROID_START_ZONE_WIDTH) - _worldScreenResolution.y; ;

        return new Vector3(randomX, randomY);
    }

    public Vector3 GetRandomUfoStartPoint()
    {
        float randomX = Random.Range(0, 2) == 0 ? -(_worldScreenResolution.x + UFO_START_X_OFFSET) : (_worldScreenResolution.x + UFO_START_X_OFFSET);
        float randomY = Random.Range(-_worldScreenResolution.y, _worldScreenResolution.y);

        return new Vector3(randomX, randomY);
    }

    public Vector3 WorldScreenResolution => _worldScreenResolution;
}
