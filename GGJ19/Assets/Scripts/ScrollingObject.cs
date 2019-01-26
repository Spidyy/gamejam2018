using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour 
{
    private Rigidbody2D rb2d;

    public float m_scrollspeed = 1f;

    private void Awake () 
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void SetScrollingActive(bool active)
    {
        rb2d.velocity = active ? new Vector2 (-m_scrollspeed, 0) : Vector2.zero;
    }
}