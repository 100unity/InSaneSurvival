using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
    public string SpriteLocation = "Sprites/PlayerHungerStates_Sprite";
    
    private int _statusValue = 100;

    private Sprite[] _barStates;

    private Image[] _images;

    private Animator _animator;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _barStates = Resources.LoadAll<Sprite>(SpriteLocation);
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    public void UpdateBar(int value)
    {
        var newValue = value;
        var oldValue = _statusValue;

        if (newValue > 100 || newValue < 0)
        {
            Debug.Log("Status bar value has to be between 0 and 100");
        }
        else if (newValue == oldValue)
        {
            Debug.Log("new status bar value has to differ from the old value");
        }
        else
        {
            ChangeBarAnimation(newValue, oldValue);
            _statusValue = newValue;
            ChangeSprite();
        }
    } 
    
    private void ChangeBarAnimation(int newValue, int oldValue)
    {
        if (newValue > oldValue)
        {
            _animator = gameObject.GetComponent<Animator>();
            if (_animator != null)
            {
                _animator.Play("UIIncreaseStat");

            }
        }
        else
        {
            _animator = gameObject.GetComponent<Animator>();
            if (_animator != null)
            {
                _animator.Play("UIDecreaseStat");

            }
        }
    }

    private void ChangeSprite()
    {
        if (_statusValue >= 0 && _statusValue < 10) gameObject.GetComponent<Image>().sprite = _barStates[0];
        if (_statusValue >= 10 && _statusValue < 20) gameObject.GetComponent<Image>().sprite = _barStates[1];
        if (_statusValue >= 20 && _statusValue < 30) gameObject.GetComponent<Image>().sprite = _barStates[2];
        if (_statusValue >= 30 && _statusValue < 40) gameObject.GetComponent<Image>().sprite = _barStates[3];
        if (_statusValue >= 40 && _statusValue < 50) gameObject.GetComponent<Image>().sprite = _barStates[4];
        if (_statusValue >= 50 && _statusValue < 60) gameObject.GetComponent<Image>().sprite = _barStates[5];
        if (_statusValue >= 60 && _statusValue < 70) gameObject.GetComponent<Image>().sprite = _barStates[6];
        if (_statusValue >= 70 && _statusValue < 80) gameObject.GetComponent<Image>().sprite = _barStates[7];
        if (_statusValue >= 80 && _statusValue < 90) gameObject.GetComponent<Image>().sprite = _barStates[8];
        if (_statusValue >= 90 && _statusValue < 100) gameObject.GetComponent<Image>().sprite = _barStates[9];
        if (_statusValue == 100) gameObject.GetComponent<Image>().sprite = _barStates[10];
    }
    
    
}
                     