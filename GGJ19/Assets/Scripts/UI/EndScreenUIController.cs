using UnityEngine;

// UI controller for the end screens 
//
public class EndScreenUIController :MonoBehaviour
{
	[SerializeField]
	private GameObject m_winParentGO = null;
	[SerializeField]
	private GameObject m_loseParentGO = null;

	// Unity Awake
	//
	private void Awake()
	{
		Debug.Assert(m_winParentGO != null, "Win parent null");
		Debug.Assert(m_loseParentGO != null, "Lose parent EndScreenUIController.null");
	}

    public void Initialise()
    {
        m_winParentGO.SetActive(false);
        m_loseParentGO.SetActive(false);
    }
}