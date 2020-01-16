﻿using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public const string MouseXInput = "Mouse X";
    public const string MouseYInput = "Mouse Y";

    [SerializeField] public float sensitivity = 5.0f;
    [SerializeField] public float smoothing = 2.0f;

    public GameObject player;

    private Vector2 mouseLook;

    private Vector2 smoothV;


    private void Start()
    {
        player = this.transform.parent.gameObject;
    }

    private void Update()
    {
        var md = new Vector2(Input.GetAxis(MouseXInput), Input.GetAxis(MouseYInput));
        md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));

        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);

        mouseLook += smoothV;

        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        player.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, player.transform.up);
    }
}
