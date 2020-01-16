using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityMeter : MonoBehaviour
{
    public Transform target;
    public PixelationEffect pe;

    [SerializeField] private int _distanceToTarget;
    [SerializeField] private int _amountToAdd;
    [SerializeField] private int _startValue;
    [SerializeField] private int _targetValue;
    private int  _currentValue;

    void Start() {
        pe.pixelDensity = _startValue;
        _currentValue = _startValue;
    }

    void Update() {
        if (Vector3.Distance(target.position, transform.position) <= _distanceToTarget) {
            if (_currentValue >= _targetValue)
                _currentValue -= _amountToAdd;
            else
                _currentValue = _targetValue;
        }
        else {
            if (_currentValue <= _startValue)
                _currentValue += _amountToAdd;
            else
                _currentValue = _startValue;
        }
        pe.pixelDensity = _currentValue;
    }
}
