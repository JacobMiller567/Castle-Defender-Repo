using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightClick : MonoBehaviour
{
    public BoxCollider2D towerCollider;
    public GameObject sellButton;
    public bool towerClicked = false;

    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);

            if (hitCollider == towerCollider)
            {
                towerClicked = !towerClicked;
                sellButton.SetActive(towerClicked);
            }
        }
    }
}
