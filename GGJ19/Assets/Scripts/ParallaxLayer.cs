//
// Copyright (c) 2018 Tag Games Ltd. All rights reserved
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    private const float k_globalParallaxScale = 0.28f;

    public int m_numTiles = 6;
    public bool m_invertAlternateTiles = false;
    public bool m_randomlyInvertTiles = false;
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

            bool invert = false;
            if(m_randomlyInvertTiles)
            {
                invert = (Random.Range(0,2) == 0);
                //Debug.Log(invert);
            }
            else if(m_invertAlternateTiles)
            {
                invert = (i % 2 != 0);
            }

            float xScale = invert ? -1f : 1f;
            go.transform.localScale = new Vector2(xScale, 1f);

            m_tiles[i] = go.AddComponent<ParallaxTile>();
            m_tiles[i].m_scrollSpeed = m_scrollSpeed * k_globalParallaxScale;

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
