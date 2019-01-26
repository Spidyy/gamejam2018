using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUIController : MonoBehaviour
{
    public Slider m_slider = null;

    private float m_progress = 0.0f;

    public float Progress {  get { return m_progress; } }

    private void Awake()
    {
        Debug.Assert(m_slider != null, "Slider is null");
    }

    // Init the progress bar
    //
    public void Initialise()
    {
        UpdateProgress(1.0f);
    }

    // Update the slide progress
    //
    public void UpdateProgress(float progress)
    {
        m_progress = progress;
        m_slider.value = progress;
    }
}