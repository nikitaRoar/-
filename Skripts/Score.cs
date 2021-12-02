using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class Score
{

	[SerializeField] public int m_currentScore;

	[SerializeField] private int m_levelScoreBonus;

	[SerializeField] private int m_turnScoreBonus;
	public void AddLevelBonus()
	{
		m_currentScore += m_levelScoreBonus;
	}

	public void AddTurnBonus()
	{
		m_currentScore += m_turnScoreBonus;
	}
	public int CurrentScore
	{
		get
		{
			return m_currentScore;
		}
		set
		{

			m_currentScore = value;
			Hud.Instance.UpdateScoreValue(m_currentScore);
		}
	}
	public int Turns
	{
		get
		{
			return m_turnScoreBonus;
		}
		set
		{
			m_turnScoreBonus = value;
			Hud.Instance.UpdateTurnsValue(m_turnScoreBonus);
		}
	}


}
