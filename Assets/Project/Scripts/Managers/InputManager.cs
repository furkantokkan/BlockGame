using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public event Action onTouchStart;
    public event Action<Vector3> onTouchStationary;
    public event Action<Vector3> onTouchMove;
    public event Action onTouchEnd;
    public event Action onTouchCancel;
    void Start()
    {

    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ScreenMouseRay();
        }
    }
    public void ScreenMouseRay()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit)
        {
            Debug.Log(hit.transform.name);
        }
    }
}