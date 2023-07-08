using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.EventSystems;
using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}

public class CarControls : MonoBehaviour
{
    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    private float _speed, _angle, _speedNum;
    private Vector3 _dist;
    public GameObject _door, _prefab, _background, _parent;

    public void Start()
    {
        _dist = _background.transform.position - this.transform.position;
    }

    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    public void FixedUpdate()
    {
        _speedNum = this.GetComponent<Rigidbody>().velocity.magnitude;
        if(_speedNum > 10)
        {
            this.GetComponent<AudioSource>().pitch = 1;
        }
        else
        {
            this.GetComponent<AudioSource>().pitch = -0.5f;
        }

        Vector3 targetPosition = this.transform.position + _dist.normalized * 500;
        _background.transform.position = new Vector3(_background.transform.position.x, _background.transform.position.y, targetPosition.z);

        float motor = maxMotorTorque * _speed;
        float steering = maxSteeringAngle * _angle;

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }

    public void SpeedButtonUp()
    {
        _speed = 0;
    }

    public void SteeringButtonUp()
    {
        _angle = 0;
    }

    public void Right()
    {
        _angle = 1;
    }

    public void Left()
    {
        _angle = -1;
    }

    public void Front()
    {
        _speed = 1;
    }

    public void Back()
    {
        _speed = -1;
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.name == "Trigger1")
        {
            _door.GetComponent<Animator>().SetBool("Open", true);
        }
        else if(other.name == "Trigger2")
        {
            Instantiate(_prefab, new Vector3(0, 0, _parent.transform.GetChild(_parent.transform.childCount - 1).position.z + 477), new Quaternion(), _parent.transform);
            if (_parent.transform.childCount > 3)
            {
                Destroy(_parent.transform.GetChild(0).gameObject);
            }
        }
    }
}
