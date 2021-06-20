using System.Collections.Generic;
using UnityEngine;

public class BigUfo : Ufo
{
    private const int SCORE_VALUE = 500;

    private List<Vector3> _shootDirections;

    protected override void Start()
    {
        base.Start();

        _shootDirections = new List<Vector3>
        {
            Vector3.right,
            (Vector3.right + Vector3.up).normalized,
            Vector3.up,
            (Vector3.up + Vector3.left).normalized,
            Vector3.left,
            (Vector3.left + Vector3.down).normalized,
            Vector3.down,
            (Vector3.down + Vector3.right).normalized
        };

        scoreValue = SCORE_VALUE;
    }

    protected override void Shoot()
    {
        int randomDirectionIndex = Random.Range(0, _shootDirections.Count);
        shootDirection = _shootDirections[randomDirectionIndex];

        base.Shoot();
    }
}
