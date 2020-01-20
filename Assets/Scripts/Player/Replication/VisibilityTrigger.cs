using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityTrigger : Bolt.EntityBehaviour<ICustomPlayerState>
{
    private SphereCollider sc;

    [Header("Debug Sphere")]
    [SerializeField] private MeshRenderer _debugSphere;
    private bool _isDebugSphereActive = false;

    [Header("Color")]
    [SerializeField] private Material _material;
    [SerializeField] private Color _baseColor;
    [SerializeField] private Color _targetColor;
    [SerializeField] private float _alphaChangeValue;
    private Color _currentColor;
    private bool _shouldActivateVisibility = false;

    public override void Attached() {
        sc = GetComponent<SphereCollider>();
        _currentColor = _baseColor;
        _material.color = _baseColor;
    }

    private void Update() {
        UpdateMeshVisibility();
    }

    public void ActivateVisibilityChange() {
        _shouldActivateVisibility = (_shouldActivateVisibility == false) ? true : false;
    }

    public void ActivateDebugTriggerSphere() {
        _debugSphere.enabled = (_debugSphere.enabled == false) ? true : false;
    }

    private void UpdateMeshVisibility() {
        _currentColor = (_shouldActivateVisibility == false) ? _currentColor = Color.Lerp(_currentColor, _baseColor, _alphaChangeValue) : Color.Lerp(_currentColor, _targetColor, _alphaChangeValue);
        _material.color = _currentColor;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            _shouldActivateVisibility = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            print("Exit");
            _shouldActivateVisibility = false;
        }
    }
}
