using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScreen : MonoBehaviour {

    public GameDirector gameDirector;

	public void OnFadeOutDone()
    {
        gameDirector.BeginPlaythrough();
        gameObject.SetActive(false);
    }
}
