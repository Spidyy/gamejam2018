using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public TMPro.TMP_Text m_text;

    public float Delay;
    public float Duration;
    public float Speed;
    public string Text
    {
        set { m_text.text = value; }
    }

    private float m_timer = 0f;
    private RectTransform m_rectTransform;

    public Color Color
    {
        set { m_text.color = value; }
    }

    // Use this for initialization
    void Awake ()
    {
        m_rectTransform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        m_timer += Time.deltaTime;
        if(m_timer > Delay)
        {
            if(m_text.gameObject.activeSelf == false)
            {
                m_text.gameObject.SetActive(true);
            }

            Vector3 position = m_rectTransform.position;
            position.y -= Speed * Time.deltaTime;
            m_rectTransform.position = position;

            float lifetimeAlpha = (m_timer - Delay) / Duration;

            Color color = m_text.color;
            color.a = Mathf.Clamp01(1f - lifetimeAlpha);
            m_text.color = color;

            if(lifetimeAlpha > 1f)
            {
                Destroy(gameObject);
            }
        }
    }
}
