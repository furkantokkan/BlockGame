using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    Vector3 offset;

    bool isHitPice = false;

    private GamePice piceOnDrag;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ScreenMouseRay();

            if (piceOnDrag != null && isHitPice)
            {
                piceOnDrag.ResetShortingOrrder();
                piceOnDrag.BeginDrag();
                offset = piceOnDrag.GetParentTransform().transform.position - MouseWorldPosition();
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (piceOnDrag != null && isHitPice)
            {
                piceOnDrag.OnDrag(MouseWorldPosition() + offset);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            piceOnDrag.DragEnd();
            isHitPice = false;
        }
    }
    public void ScreenMouseRay()
    {

        RaycastHit2D[] hits = Physics2D.RaycastAll(MouseWorldPosition(), Vector2.zero, 1  << LayerMask.NameToLayer("Pice"));

        List<RaycastHit2D> hitsList = new List<RaycastHit2D>();
        hitsList.AddRange(hits);
        hitsList = hitsList.OrderByDescending(x => x.transform.GetComponent<SpriteRenderer>().sortingOrder).ToList();

        if (hitsList[0].transform.TryGetComponent<GamePice>(out GamePice pice) && !isHitPice)
        {
            Debug.Log(hits[0].transform.name);
            piceOnDrag = pice;
            isHitPice = true;
        }

    }

    Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }
}