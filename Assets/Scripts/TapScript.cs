using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapScript : MonoBehaviour
{
    public GameObject _gem;
    private int _speed = 35;

    //public void Update()
    //{
    //    _gem.transform.Rotate(Vector3.up * _speed * Time.deltaTime, Space.World); //второй способ зацикленного вращения
    //}

    public void OnMouseDown()
    {
        _gem.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f, 1f, 1f);
        _gem.GetComponent<Animator>().SetTrigger("Gem");
        _gem.GetComponent<AudioSource>().Play();
    }
}
