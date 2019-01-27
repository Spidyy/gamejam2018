using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

// UI controller for the end screens 
//
public class EndScreenUIController :MonoBehaviour
{
    private const string k_mainSceneName = "main";

    public enum EndScreen
    {
        NONE,
        WIN,
        LOSE
    }

    private readonly Dictionary<Stat, string> k_deathStrings = new Dictionary<Stat, string>()
    {
       { Stat.HP, "You Died \n\nYour wounds proved too great to endure" },
       { Stat.STA, "You Died \n\nYou black out from exhaustion, never to awaken" },
       { Stat.HUN, "You Died \n\nLacking the fuel it needs, your body gives out" },
    };

    [SerializeField]
	private GameObject m_winParentGO = null;
	[SerializeField]
	private GameObject m_loseParentGO = null;
    [SerializeField]
    private TMP_Text m_deathText = null;

    private EndScreen m_activeEndScreen = EndScreen.NONE;
    private bool m_loading = false;

	// Unity Awake
	//
	private void Awake()
	{
		Debug.Assert(m_winParentGO != null, "Win parent null");
		Debug.Assert(m_loseParentGO != null, "Lose parent EndScreenUIController.null");
        Debug.Assert(m_deathText != null, "Death text is null");
	}

    public void Initialise()
    {
        m_winParentGO.SetActive(false);
        m_loseParentGO.SetActive(false);
    }

    public EndScreen ActiveScreen {  get { return m_activeEndScreen; } }

    // Show the specific end screen 
    //
    public void ShowEndScreen(EndScreen screen)
    {
        m_winParentGO.SetActive(screen == EndScreen.WIN);
        m_loseParentGO.SetActive(screen == EndScreen.LOSE);

        m_activeEndScreen = screen;
    }

    // Set the death cause and text
    //
    public void SetDeath(Stat deathStat)
    {
        string deathMsg = "You Died!";

        if(k_deathStrings.TryGetValue(deathStat, out deathMsg) == false)
        {
            Debug.LogError("No death text available for stat: " + deathStat.ToString());
        }

        m_deathText.text = deathMsg;
    }

    // Callback attached to the win screen continue button
    //
    public void OnWinContinuePressed()
    {
        if(!m_loading)
        {
            m_loading = true;
            SceneManager.LoadScene(k_mainSceneName, LoadSceneMode.Single);
        }
    }

    // Callback attached to the win screen continue button
    //
    public void OnLoseContinuePressed()
    {
        if(!m_loading)
        {
            m_loading = true;
            AsyncOperation operation = SceneManager.LoadSceneAsync(k_mainSceneName, LoadSceneMode.Single);
        }
    }
}