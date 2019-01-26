using UnityEngine;
using UnityEngine.UI;

public class HudUIController : MonoBehaviour
{
    ProgressBarUIController m_healthProgressBar = null;
    ProgressBarUIController m_hungerProgressBar = null;
    ProgressBarUIController m_staminaProgressBar = null;

    /// Unity Awake Function
	///
	private void Awake()
	{
        Debug.Assert(m_healthProgressBar != null, "Health Progress bar is null");
        Debug.Assert(m_hungerProgressBar != null, "Hunger progress bar is null");
        Debug.Assert(m_staminaProgressBar != null, "Stamina progress bar is null");
	}

    /// Clean-up before destriction
    /// 
    private void OnDestroy()
    {
        if(GameDirector.Instance != null)
        {
            GameDirector.Instance.Player.OnStatChange -= OnStatChanged;
        }
    }

    public void Initialise()
    {
        GameDirector.Instance.Player.OnStatChange += OnStatChanged;
    }

    // Update the relevate stat tracking object 
    //
    private void OnStatChanged(Stat stat, int newTotal, int delta)
    {
        switch(stat)
        {
            case Stat.GOLD:
                {
                    break;
                }
            case Stat.HP:
                {
                    break;
                }
            case Stat.HUN:
                {
                    break;
                }
            case Stat.STA:
            {
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