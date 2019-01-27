
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxTile : MonoBehaviour
{
    public float m_horizontalLength;
    public float m_resetOffset;
    public float m_scrollSpeed;
    private Rigidbody2D rb2d;

    private void Awake () 
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void SetScrollingActive(bool active)
    {
        rb2d.velocity = active ? new Vector2 (-m_scrollSpeed, 0) : Vector2.zero;
    }

    private void Update()
    {
        if (transform.position.x < (-0.5f * m_resetOffset))
        {
            RepositionBackground ();
        }
    }

    private void RepositionBackground()
    {
        Vector2 groundOffSet = new Vector2(m_resetOffset, 0);
        transform.position = (Vector2) transform.position + groundOffSet;
    }
}
