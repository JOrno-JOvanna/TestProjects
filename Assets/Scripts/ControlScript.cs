using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class ControlScript : MonoBehaviour
{
    public float _speed, _jump;
    public GameObject _character;
    public Joystick _joystick;
    private Vector3 _move;
    private float _gravity;

    public void FixedUpdate()
    {
        if(_character.GetComponent<CharacterController>().isGrounded)
        {
            _character.GetComponent<Animator>().ResetTrigger("Bow");
            _character.GetComponent<Animator>().ResetTrigger("Jump");
            _character.GetComponent<Animator>().SetBool("Fall", false);

            _move = Vector3.zero;
            _move.x = _joystick.Horizontal * _speed;
            _move.z = _joystick.Vertical * _speed;

            if (_move.x != 0 && _move.z != 0)
            {
                if(_joystick.Horizontal < 0.4f & _joystick.Horizontal > -0.4f & _joystick.Vertical < 0.4f & _joystick.Vertical > -0.4f)
                {
                    _character.GetComponent<Animator>().SetBool("Move", true);
                    _character.GetComponent<Animator>().SetBool("Sprint", false);
                }
                else
                {
                    _character.GetComponent<Animator>().SetBool("Sprint", true);
                }
            }
            else
            {
                _character.GetComponent<Animator>().SetBool("Sprint", false);
                _character.GetComponent<Animator>().SetBool("Move", false);
            }

            if (Vector3.Angle(_character.transform.forward, _move) > 1)
            {
                Vector3 _direction = Vector3.RotateTowards(_character.transform.forward, _move, _speed, 0);
                _character.transform.rotation = Quaternion.LookRotation(_direction);
                _character.transform.forward = _direction;
            }
        }
        else
        {
            if (_gravity < 0)
            {
                _character.GetComponent<Animator>().SetBool("Fall", true);
            }
        }

        if (!_character.GetComponent<CharacterController>().isGrounded)
        {
            _gravity -= 20 * Time.deltaTime;
        }

        _move.y = _gravity;

        _character.GetComponent<CharacterController>().Move(_move * Time.deltaTime);
    }

    public void Jump()
    {
        _character.GetComponent<Animator>().SetTrigger("Jump");
        _gravity = _jump;
    }

    public void Shoot()
    {
        _character.GetComponent<Animator>().SetTrigger("Bow");
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
