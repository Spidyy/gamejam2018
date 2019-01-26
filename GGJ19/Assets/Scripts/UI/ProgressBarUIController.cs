using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUIController : MonoBehaviour
{
    public Image m_foregroundImage = null;
    public Image m_backgroundImage = null;

    private float m_progress = 0.0f;

    public float Progress {  get { return m_progress; } }

    public void Initialise()
    {

    }


}