using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private static Controller m_instance;

    [SerializeField] private Color[] m_tokenColors;

    [SerializeField] public LevelParameters m_level;
   
    [SerializeField] public Score m_score;


    [SerializeField] private Audio m_audio = new Audio();


    public Field m_field;

    public List<List<Token>> m_tokensByTypes;

    public int m_currentLevel;

    public Audio Audio { get => m_audio; set => m_audio = value; }


    public void InitializeLevel()
    {
        m_level = new LevelParameters(m_currentLevel);

        TokenColors = MakeColors(Level.TokenTypes);

        m_tokensByTypes = new List<List<Token>>();

        for (int i = 0; i < Level.TokenTypes; i++)
        {
            m_tokensByTypes.Add(new List<Token>());
        }
        m_field = Field.Create(Level.FieldSize, Level.FreeSpace);
    }
    
    public LevelParameters Level { get => m_level; set => m_level = value; }
  

    public Color[] TokenColors
    {
        get
        {
            return m_tokenColors;
        }

        set
        {
            m_tokenColors = value;

        }
    }

   
    public static Controller Instance
    {
        get
        {
            if (m_instance == null)
            {
                var controller =Instantiate(Resources.Load("Prefabs/Controller")) as GameObject;


                m_instance = controller.GetComponent<Controller>();
            }
            return m_instance;
        }
    }

   

    private void Start()
    {
        DataStore.LoadGame();

        InitializeLevel();
        Audio.PlayMusic(true);
       
    }
    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (m_instance != this) Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        MakeColors(10);

        m_tokensByTypes = new List<List<Token>>();

        for (int i = 0; i < Level.TokenTypes; i++)
        {
            m_tokensByTypes.Add(new List<Token>());
        }

        Audio.SourceMusic = gameObject.AddComponent<AudioSource>();
        Audio.SourceRandomPitchSFX = gameObject.AddComponent<AudioSource>();
        Audio.SourceSFX = gameObject.AddComponent<AudioSource>();

        DataStore.LoadOptions();
    }
    private Color[] MakeColors(int count)
    {
        //инициализация массива
        Color[] result = new Color[count];

        //сдвиг цвета - шаг значения hue
        float colorStep = 1f / (count + 1);

        //задаем исходный цвет
        float hue = 0f;

        float saturation = 0.5f;

        float value = 1f;

        //Задаем значение цвета со смещением для каждого элемента массива
        for (int i = 0; i < count; i++)
        {
            //смещение цвета:
            float newHue = hue + (colorStep * i);

            result[i] = Color.HSVToRGB(newHue, saturation, value);
        }
        return result;
    }
    public void TurnDone()
    {
        Audio.PlaySound("Drop");
       
        if (IsAllTokensConnected())
        {
            Debug.Log("Win!");
            Audio.PlaySound("Victory");
            Debug.Log("track Pobeda");
            Hud.Instance.CountScore(m_level.Turns);
            m_currentLevel++;
            m_score.AddLevelBonus();
            Destroy(m_field.gameObject);
           
        }
        else
        {
            Debug.Log("Continue...");

            if (m_level.Turns > 0)
           {
                m_level.Turns--;
           }
        }
        
    }
    public bool IsAllTokensConnected()
    {
        //TODO:
        //Оптимизировать: проверять только тот тип, фишка которого была перемещена
        //Перебираем типы
        for (var i = 0; i < m_tokensByTypes.Count; i++)
        {
            //Соединены ли все фишки текущего типа
            if (IsTokensConnected(m_tokensByTypes[i]) == false)
            {
                return false;
            }
        }
        return true;
    }
    private bool IsTokensConnected(List<Token> tokens)
    {
        if (tokens.Count == 0)
        {
            return true;
        }

        List<Token> connectedTokens = new List<Token>();

        connectedTokens.Add(tokens[0]);

        bool moved = true;

        while (moved)
        {
            moved = false;

            for (int i = 0; i < connectedTokens.Count; i++)
            {
                for (int j = 0; j < tokens.Count; j++)
                {
                    if (IsTokensNear(tokens[j], connectedTokens[i]))
                    {
                        if (connectedTokens.Contains(tokens[j]) == false)
                        {
                            connectedTokens.Add(tokens[j]);
                            moved = true;
                        }
                    }
                }
            }
        }

        if (tokens.Count == connectedTokens.Count)
        {
            return true;
        }
        return false;
    }
    private bool IsTokensNear(Token first, Token second)
    {
        if ((int)first.transform.position.x == (int)second.transform.position.x + 1 ||  (int)first.transform.position.x == (int)second.transform.position.x - 1)
               
        {
            if ((int)first.transform.position.y == (int)second.transform.position.y)
            {
                return true;
            }
        }

        if ((int)first.transform.position.y == (int)second.transform.position.y + 1 ||  (int)first.transform.position.y == (int)second.transform.position.y - 1)
                 

        {
            if ((int)first.transform.position.x == (int)second.transform.position.x)
            {
                return true;
            }
        }

        return false;
    }

    public void Reset()
    {
        m_currentLevel = 1;

        m_score.CurrentScore = 0;

        Destroy(m_field.gameObject);
        DataStore.SaveGame();

        InitializeLevel();
    }
}
