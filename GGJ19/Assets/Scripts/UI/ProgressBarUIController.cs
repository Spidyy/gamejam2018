using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUIController : MonoBehaviour
{
    private const float k_progressUpdateDelta = 1.0f;
    private const float k_minUpdateDistance = 0.02f;

    public Slider m_slider = null;

    private float m_currentProgress = 0.0f;
    private float m_targetProgress = 1.0f;

    public float Progress {  get { return m_currentProgress; } }

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

    // Set the new target progress
    //
    public void SetTargetProgress(float progress)
    {
        m_targetProgress = Mathf.Clamp01(progress);
    }

    // Called once per frame
    //
    private void Update()
    {
        if(Mathf.Abs(m_targetProgress - m_currentProgress) > k_minUpdateDistance)
        {
            int sign = (m_targetProgress - m_currentProgress) > 0 ? 1 : -1;
            m_currentProgress += k_progressUpdateDelta * Time.smoothDeltaTime * sign;
            m_slider.value = m_currentProgress;
        }
    }

    // Update the slide progress
    //
    private void UpdateProgress(float progress)
    {

    }
}