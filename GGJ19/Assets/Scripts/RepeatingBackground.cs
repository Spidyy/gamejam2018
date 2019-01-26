using UnityEngine;
using System.Collections;

public class RepeatingBackground : MonoBehaviour 
{
    public float m_horizontalLength;

    private void Awake ()
    {
        
    }

    private void Update()
    {
        if (transform.position.x < -m_horizontalLength)
        {
            RepositionBackground ();
        }
    }

    private void RepositionBackground()
    {
        Vector2 groundOffSet = new Vector2(m_horizontalLength * 2f, 0);
        transform.position = (Vector2) transform.position + groundOffSet;
    }
}