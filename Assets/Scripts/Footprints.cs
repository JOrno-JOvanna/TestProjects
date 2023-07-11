using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Footprints : MonoBehaviour
{
    public GameObject _L, _R, _player;
    public Transform _parent;
    private Quaternion _LRotation, _RRotation;
    private Vector3 _position;
    private float _speed;

    public void Start()
    {
        _RRotation = _R.transform.rotation;
        _LRotation = _L.transform.rotation;
        _position = _player.transform.position;
    }

    public void FixedUpdate()
    {
        Vector3 _placement = _player.transform.position - _position;
        _speed = _placement.magnitude / Time.deltaTime;
        _position = _player.transform.position;
    }

    public void OnCollisionEnter(Collision _info)
    {
        if (_parent.childCount > 5)
        {
            Destroy(_parent.GetChild(0).gameObject);
        }

        if (_speed != 0 & _info.transform.position != Vector3.zero & _player.GetComponent<CharacterController>().isGrounded)
        {
            if (this.name == "ToesEnd_R")
            {
                GameObject _RFootprint = Instantiate(_R, _info.transform.position, _RRotation, _parent);
                _RFootprint.transform.Rotate(Vector3.back, _player.transform.eulerAngles.y);
            }
            else
            {
                GameObject _LFootprint = Instantiate(_L, _info.transform.position, _LRotation, _parent);
                _LFootprint.transform.Rotate(Vector3.forward, _player.transform.eulerAngles.y);
            }
        }
    }
}
