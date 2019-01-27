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
    private TMP_Text m_distanceText = null;

    private Player m_player = null;

    /// Unity Awake Function
	///
	private void Awake()
	{
        Debug.Assert(m_healthProgressBar != null, "Health Progress bar is null");
        Debug.Assert(m_hungerProgressBar != null, "Hunger progress bar is null");
        Debug.Assert(m_staminaProgressBar != null, "Stamina progress bar is null");
        Debug.Assert(m_goldText != null, "Gold text is null");
        Debug.Assert(m_distanceText != null, "m_distanceText is null");
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
        m_distanceText.text = string.Format("{0}m", Mathf.FloorToInt(distance));
    }

    // Update the relevate stat tracking object 
    //
    private void OnStatChanged(Stat stat, int newTotal, int delta)
    {
        Debug.Log(string.Format("OnStatChanged. stat; {0} newTotal: {1} delta: {2} ", stat, newTotal, delta));

        switch(stat)
        {
            case Stat.GOLD:
                {
                    m_goldText.text = m_player.CurrentStatValue(Stat.GOLD).ToString();

                    break;
                }
            case Stat.HP:
                {
                    float progress = Mathf.Clamp01((float)m_player.CurrentStatValue(stat) / (float)StatConsts.k_maxValues[(int)stat]);
                    m_healthProgressBar.UpdateProgress(progress);

                    break;
                }
            case Stat.HUN:
                {
                    float progress = Mathf.Clamp01((float)m_player.CurrentStatValue(stat) / (float)StatConsts.k_maxValues[(int)stat]);
                    m_hungerProgressBar.UpdateProgress(progress);

                    break;
                }
            case Stat.STA:
                {
                    float progress = Mathf.Clamp01((float)m_player.CurrentStatValue(stat) / (float)StatConsts.k_maxValues[(int)stat]);
                    m_staminaProgressBar.UpdateProgress(progress);

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