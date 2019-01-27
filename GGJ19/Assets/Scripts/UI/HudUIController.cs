using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudUIController : MonoBehaviour
{
    [SerializeField]
    private ProgressBarUIController m_healthProgressBar = null;
    [SerializeField]
    private ProgressBarUIController m_hungerProgressBar = null;
    [SerializeField]
    private ProgressBarUIController m_staminaProgressBar = null;
    [SerializeField]
    private TMP_Text m_goldText = null;
    [SerializeField]
    private Slider m_homeBarSlider = null;

    private Player m_player = null;

    /// Unity Awake Function
	///
	private void Awake()
	{
        Debug.Assert(m_healthProgressBar != null, "Health Progress bar is null");
        Debug.Assert(m_hungerProgressBar != null, "Hunger progress bar is null");
        Debug.Assert(m_staminaProgressBar != null, "Stamina progress bar is null");
        Debug.Assert(m_goldText != null, "Gold text is null");
        Debug.Assert(m_homeBarSlider != null, "home bar slider is null");
	}

    /// Clean-up before destriction
    /// 
    private void OnDestroy()
    {
        if(GameDirector.Instance != null)
        {
            GameDirector.Instance.Player.OnStatChange -= OnStatChanged;
            GameDirector.Instance.Player.OnDistanceChanged -= OnDistanceChanged;
        }
    }

    // Init the controller 
    //
    public void Initialise()
    {
        m_player = GameDirector.Instance.Player;

        m_player.OnStatChange += OnStatChanged;
        m_player.OnDistanceChanged += OnDistanceChanged;

        m_healthProgressBar.Initialise();
        m_hungerProgressBar.Initialise();
        m_staminaProgressBar.Initialise();
        m_goldText.text = m_player.CurrentStatValue(Stat.GOLD).ToString();
    }

    /// Callbacke attached to a distance change
    /// 
    private void OnDistanceChanged(float distance)
    {
        m_homeBarSlider.value = m_player.GetProgressToHome();
    }

    // Update the relevate stat tracking object 
    //
    private void OnStatChanged(Stat stat, int newTotal, int delta)
    {
        Debug.Log(string.Format("OnStatChanged. stat; {0} newTotal: {1} delta: {2} ", stat, newTotal, delta));

        float progress = 0.0f;
        switch(stat)
        {
            case Stat.GOLD:
                {
                    m_goldText.text = m_player.CurrentStatValue(Stat.GOLD).ToString();

                    break;
                }
            case Stat.HP:
                {
                    progress = Mathf.Clamp01((float)m_player.CurrentStatValue(stat) / (float)StatConsts.k_maxValues[(int)stat]);
                    m_healthProgressBar.SetTargetProgress(progress);

                    break;
                }
            case Stat.HUN:
                {
                    progress = Mathf.Clamp01((float)m_player.CurrentStatValue(stat) / (float)StatConsts.k_maxValues[(int)stat]);
                    m_hungerProgressBar.SetTargetProgress(progress);

                    break;
                }
            case Stat.STA:
                {
                    progress = Mathf.Clamp01((float)m_player.CurrentStatValue(stat) / (float)StatConsts.k_maxValues[(int)stat]);
                    m_staminaProgressBar.SetTargetProgress(progress);

                    break;
                }
            default:
                {
                    Debug.LogWarning("No UI element tracking stat: " + stat.ToString());
                    break;
                }
        }
    }
}