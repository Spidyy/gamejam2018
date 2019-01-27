using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    private const int k_staminaDecrementSpeed = -1;
    private const float k_staminaDecrementDelta = 1.0f;

    private const int k_hungerDecrementSpeed = -5;
    private const float k_hungerDecrementDelta = 25.0f;

    public float m_speedMetresPerSecond = 1f;

    public bool m_moving = false;
    public float m_currentDistance = 0f;

    private float m_homeDistance;

    private float m_prevStaminaDecementDistance = 0.0f;
    private float m_prevHungerDecementDistance = 0.0f;
    private int[] m_currentStats;
    private Animator m_animator;

    public delegate void StatChangeDelegate(Stat stat, int newTotal, int delta);
    public event StatChangeDelegate OnStatChange;

    public int Gold
    {
        get { return m_currentStats[(int)Stat.GOLD]; }
    }

    public delegate void DistanceChangedDelegate(float distance);
    public event DistanceChangedDelegate OnDistanceChanged;

    public void Awake()
    {
        m_currentStats = GenerateStartingStats();
        m_animator = GetComponent<Animator>();

        m_prevHungerDecementDistance =
            m_prevStaminaDecementDistance = 
            m_currentDistance = 0.0f;

        SetDistanceToHome(StatConsts.k_startHomeDistance);

        OnStatChange += OnStatChangeInternal;
    }

    // Clean-up
    //
    public void OnDestroy()
    {
        OnStatChange -= OnStatChangeInternal;
    }

    public int[] GenerateStartingStats()
    {
        int num = Enum.GetNames(typeof(Stat)).Length;
        var stats = new int[num];

        for(int i = 0; i < num; ++i)
        {
            stats[i] = StatConsts.k_startingValues[i];
        }

        return stats;
    }

    public void Move()
    {
        m_currentDistance += Time.deltaTime * m_speedMetresPerSecond;
        m_moving = true;

        HandleStatReductions();

        if(OnDistanceChanged != null)
        {
            OnDistanceChanged(m_currentDistance);
        }
    }

    // Set new distance until home
    //
    public void SetDistanceToHome(float distance)
    {
        m_homeDistance = distance;
    }

    // Normalized progress to 
    //
    public float GetProgressToHome()
    {
        return Mathf.Clamp01(m_currentDistance / m_homeDistance);
    }

    // Handle the reduction in stats as the player moves
    //
    private void HandleStatReductions()
    { 
        if(m_currentDistance - m_prevStaminaDecementDistance > k_staminaDecrementDelta)
        {
            AlterStat(Stat.STA, k_staminaDecrementSpeed);
            m_prevStaminaDecementDistance = m_currentDistance;
        }

        if(m_currentDistance - m_prevHungerDecementDistance > k_hungerDecrementDelta)
        {
            AlterStat(Stat.HUN, k_hungerDecrementSpeed);
            m_prevHungerDecementDistance = m_currentDistance;
        }
    }

    public void StopAt(float distance)
    {
        m_currentDistance = distance;
        m_moving = false;
    }

    public void AlterStat(Stat stat, int delta)
    {
        int statAsInt = (int)stat;
        m_currentStats[statAsInt] = Mathf.Clamp(m_currentStats[statAsInt] + delta, 0, StatConsts.k_maxValues[statAsInt]);

        if(OnStatChange != null)
        {
            OnStatChange(stat, m_currentStats[statAsInt], delta);
        }
    }

    // Return the stat value
    //
    public int CurrentStatValue(Stat stat)
    {
        return m_currentStats[(int)stat];
    }

    public bool IsDead()
    {
        return m_currentStats[(int)Stat.HP] <= 0 || m_currentStats[(int)Stat.STA] <= 0 || m_currentStats[(int)Stat.HUN] <= 0 ;
    }

    public void Update()
    {
        m_animator.SetFloat("Speed", m_moving ? 1f : 0f);
    }

    // Listener for stat changes 
    //
    private void OnStatChangeInternal(Stat stat, int newTotal, int delta)
    {
        if(stat == Stat.HUN)
        {
            if(delta > 0)
            {
                //--Reset the distance until the next decrement of hunger 
                m_prevHungerDecementDistance = m_currentDistance;
            }
        }
    }
}
