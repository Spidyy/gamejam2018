//
// Copyright (c) 2018 Tag Games Ltd. All rights reserved
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public int m_numTiles = 6;
    public bool m_invertAlternateTiles = false;
    public float m_scrollSpeed;
    public string m_prefabPath;
    public float m_overlapNormalised = 0;

    private ParallaxTile[] m_tiles;

    private void Awake()
    {
        var prefab = Resources.Load<GameObject>(m_prefabPath);

        float? width = new float?();

        m_tiles = new ParallaxTile[m_numTiles];
        for(int i = 0; i < m_numTiles; ++i)
        {
            var go = GameObject.Instantiate(prefab, transform);

            // only need to grab once
            if(width.HasValue == false)
            {
                var renderer = go.GetComponent<SpriteRenderer>();
                width = renderer.bounds.size.x;

                width = width.Value - (m_overlapNormalised * width.Value);
            }

            var rb = go.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;

            float start = -0.5f * (float)(m_numTiles - 1) * width.Value;
            go.transform.localPosition = (1f / transform.localScale.x) * new Vector3(start + (width.Value * i), 0f);

            if(m_invertAlternateTiles)
            {
                float xScale = i % 2 == 0 ? 1f : -1f;
                go.transform.localScale = new Vector2(xScale, 1f);
            }

            m_tiles[i] = go.AddComponent<ParallaxTile>();
            m_tiles[i].m_scrollSpeed = m_scrollSpeed;

            m_tiles[i].m_horizontalLength = width.Value;
            m_tiles[i].m_resetOffset = width.Value * m_numTiles;
        }
    }
    
    private void Start ()
    {

    }

    private void Update ()
    {

    }

    public void SetScrollingActive(bool active)
    {
        for(int i = 0; i < m_tiles.Length; ++i)
        {
            m_tiles[i].SetScrollingActive(active);
        }
    }
}
