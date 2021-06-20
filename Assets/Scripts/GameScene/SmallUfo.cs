using UnityEngine;

public class SmallUfo : Ufo
{
    private const int SCORE_VALUE = 1000;
    private const float ACCURACY_OFFSET_RANGE = 0.3f;

    private SpaceShip _spaceShip;
    private Vector3 _accuracyOffset;

    protected override void Start()
    {
        base.Start();

        _spaceShip = FindObjectOfType<SpaceShip>();

        scoreValue = SCORE_VALUE;
    }

    protected override void Shoot()
    {
        if (_spaceShip == null)
        {
            _spaceShip = FindObjectOfType<SpaceShip>();
        }

        if (_spaceShip != null)
        {
            _accuracyOffset = new Vector3(Random.Range(-ACCURACY_OFFSET_RANGE, ACCURACY_OFFSET_RANGE), Random.Range(-ACCURACY_OFFSET_RANGE, ACCURACY_OFFSET_RANGE));

            shootDirection = (_spaceShip.transform.position - _transform.position).normalized;
            shootDirection = (shootDirection + _accuracyOffset).normalized;
        }

        base.Shoot();
    }
}
