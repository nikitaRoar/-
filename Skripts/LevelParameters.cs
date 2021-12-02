using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class LevelParameters 
{
	[SerializeField] private int m_fieldSize;

	[SerializeField] private int m_freeSpace;


	[SerializeField] private int m_TokenTypes;

	[SerializeField] private int m_turns;

	public int FieldSize
	{
		get { return m_fieldSize; }
	}
	public int FreeSpace
	{
		get { return m_freeSpace; }
	}
	public int TokenTypes
	{
		get { return m_TokenTypes; }
	}
	public int Turns
	{
        get { return m_turns; }  
        set {; }
	}
    public LevelParameters(int currentLevel)
    {
        //Увеличивается на 1 каждые 4 уровня
        int fieldIncreaseStep = currentLevel / 4;

        //Увеличивается от 0 до 1 в течение 4-х уровней, пока размер поля не изменяется
        float subStep = (currentLevel / 4f) - fieldIncreaseStep;


        //Начальный размер поля - 3х3

        //Размер увеличивается на 1 каждые 4 уровня
        m_fieldSize = 3 + fieldIncreaseStep;

        //рассчитываем свободное пространство в зависимости от уровня сложности
        m_freeSpace = (int)(m_fieldSize * (1f - subStep));

        if (m_freeSpace < 1)
        {
            //минимальное число пустых клеток
            m_freeSpace = 1;
        }
        //Начальное число цветов - 2
        // Увеличивается на 1 каждые 2 уровня, увеличение начинается с 4го уровня
        m_TokenTypes = 2 + (currentLevel / 3);

        if (m_TokenTypes > 10)
        {
            //максимальное число цветов
            m_TokenTypes = 10;
        }

        //Количество ходов, за которые надо успеть закончить уровень,
        //чтобы получить бонус, зависит от остальных параметров: 
        m_turns = (((m_fieldSize * m_fieldSize / 2) - m_freeSpace) * m_TokenTypes) + m_fieldSize;

    }

}
