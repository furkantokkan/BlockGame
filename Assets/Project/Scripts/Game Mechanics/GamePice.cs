using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GamePice : MonoBehaviour
{
    private Transform parentOrgin;
    private SpriteRenderer renderer;

    private PolygonCollider2D polygonCollider;

    public void Inithialize()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
        parentOrgin = transform.parent;
        renderer = GetComponent<SpriteRenderer>();
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
        List<Collider2D> newColliders = CheckTheOverlapingPieces(false).
            OrderBy(x => x.GetComponent<SpriteRenderer>().sortingOrder).ToList();

        renderer.sortingOrder = newColliders.Count + 1;

        for (int i = 0; i < newColliders.Count; i++)
        {
            newColliders[i].GetComponent<SpriteRenderer>().sortingOrder = i + 1;
        }
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
}
