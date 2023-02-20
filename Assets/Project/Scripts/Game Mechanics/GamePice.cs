using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GamePice : MonoBehaviour
{
    public bool waitingOnRightPlace = false;

    private Transform parentOrgin;
    private SpriteRenderer renderer;
    private Transform target;

    private PolygonCollider2D polygonCollider;

    private int gridAmount;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    public void Inithialize(int newGridAmount, Transform rightPlace)
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
        parentOrgin = transform.parent;
        renderer = GetComponent<SpriteRenderer>();
        gridAmount = newGridAmount;
        minX = GameManager.Instance.minX;
        maxX = GameManager.Instance.maxX;
        minY = GameManager.Instance.minY;
        maxY = GameManager.Instance.maxY;
        target = rightPlace;
    }
    public Transform GetParentTransform()
    {
        return parentOrgin;
    }
    public void BeginDrag()
    {
        renderer.sortingOrder = 100;
    }
    public void OnDrag(Vector2 input)
    {
        parentOrgin.transform.position = input;
    }
    public void DragEnd()
    {
        List<Collider2D> newColliders = CheckTheOverlapingPieces(false).Where(x => x.GetComponent<SpriteRenderer>() != null).
            OrderBy(x => x.GetComponent<SpriteRenderer>().sortingOrder).ToList();

        renderer.sortingOrder = newColliders.Count + 1;

        for (int i = 0; i < newColliders.Count; i++)
        {
            newColliders[i].GetComponent<SpriteRenderer>().sortingOrder = i + 1;
        }

        if (PiceIsInTheRightPlace() && isPiceInsideOfGrid())
        {
            Snap();
            waitingOnRightPlace = true;
        }
        else if (CheckTheOverlapingPieces(false).Count <= 0 && isPiceInsideOfGrid())
        {
            Snap();
            waitingOnRightPlace = PiceIsInTheRightPlace();
        }
        else
        {
            waitingOnRightPlace = false;
        }

        GameManager.Instance.onCheckGameHadDone?.Invoke();
    }
    public void ResetShortingOrrder()
    {
        renderer.sortingOrder = 1;
    }
    private List<Collider2D> CheckTheOverlapingPieces(bool inculudeThis = false)
    {
        List<Collider2D> colliders = new List<Collider2D>();

        var contactFilter2D = new ContactFilter2D
        {
            useTriggers = false,
        };

        Physics2D.OverlapCollider(polygonCollider, contactFilter2D, colliders);

        if (!inculudeThis)
        {
            foreach (var item in colliders)
            {
                if (item.gameObject == this.gameObject)
                {
                    colliders.Remove(item);
                }
            }
        }


        return colliders;
    }
    private bool isPiceInsideOfGrid()
    {
        if (parentOrgin.position.x > minX &&
            parentOrgin.position.x < maxX &&
            parentOrgin.position.y < maxY &&
            parentOrgin.position.y > minY)
        {
            return true;
        }

        return false;
    }
    public bool PiceIsInTheRightPlace()
    {
        Transform closeDot = FindCloseDot(parentOrgin.transform.position, gridAmount);

        if (closeDot == target)
        {
            return true;
        }
 
        return false;
    }
    private void Snap()
    {
        parentOrgin.transform.position = FindCloseDot(parentOrgin.transform.position, gridAmount).position;
    }
    public Transform FindCloseDot(Vector2 orgin, int gridAmount)
    {
        float oldDistance = float.MaxValue;
        Transform result = null;

        for (int y = 0; y < gridAmount; y++)
        {
            for (int x = 0; x < gridAmount; x++)
            {
                Transform newDot = GameManager.Instance.dots[x, y].transform;
                float dist = Vector2.Distance(orgin, newDot.position);
                if (dist < oldDistance)
                {
                    result = newDot;
                    oldDistance = dist;
                }
            }
        }

        return result;
    }
}
