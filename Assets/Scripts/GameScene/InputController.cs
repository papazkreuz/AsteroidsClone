using UnityEngine;

public class InputController : MonoBehaviour
{
    private SpaceShip _spaceShip;

    private void Start()
    {
        _spaceShip = FindObjectOfType<SpaceShip>();
        _spaceShip.OnSpaceShipExplodeEvent += Disable;
    }

    private void FixedUpdate()
    {
        if (_spaceShip != null)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                _spaceShip.AddForceForward();
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                _spaceShip.Rotate(false);
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                _spaceShip.Rotate(true);
            }
        }
    }

    private void Update()
    {
        if (_spaceShip != null)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _spaceShip.UseHyperspace();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _spaceShip.Shoot();
            }
        }
    }

    public void Enable()
    {
        this.enabled = true;
        _spaceShip = FindObjectOfType<SpaceShip>();
    }

    public void Disable()
    {
        this.enabled = false;
    }
}
