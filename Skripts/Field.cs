using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public static Field Create(int size , int emptySquares) 
    {
        //Центр поля в центре сцены
        Vector3 fieldPosition = Vector3.zero;


        //Если число клеток четное (если остаток от деления на 2 равен нулю)
        if (size % 2 == 0)
        {
            fieldPosition = new Vector3(0.5f, 0.5f, 0.0f);
        }
        var field = Instantiate(Resources.Load("Prefabs/Field") as GameObject, fieldPosition, Quaternion.identity);
        //Устанавливаем масштаб поля
        Vector3 scale = Vector3.one * size;

        scale.z = 1;

        field.transform.localScale = scale;

        //Положение камеры
        Vector3 cameraPosition = field.transform.position;

        cameraPosition.z = -10;

        Camera.main.transform.position = cameraPosition;

        //Размер камеры
        Camera.main.orthographicSize = (float)size * 0.7f;

        //Текстура сетки
        field.GetComponent<Renderer>().material.mainTextureScale = Vector2.one * size;
        //Создаем фишки
        field.gameObject.GetComponent<Field>().CreateTokens(size, emptySquares);

        return field.gameObject.GetComponent<Field>();


    }
    private void CreateTokens(int size, int emptySquares) 
    {
        //Положение первой фишки - левый нижний угол:
        var offset = (size - 1f) / 2f;

        var startPosition = new Vector3(transform.position.x - offset, transform.position.y -  offset, transform.position.z - 2);

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                //если всего фишек создано больше или столько, сколько нужно - не
                // создавать фишку, оставить пустое место
                if ((i * size) + j >= (size * size) - emptySquares)
                {
                    //Ведем подсчет пустых мест
                    emptySquares--;
                }
                //Иначе (если фишек создано меньше, чем нужно)
                else
                {
                    //Если больше не нужны пустые клетки,
                    // ИЛИ вероятность создания новой фишки больше нуля
                    if (emptySquares == 0 || Random.Range(0, size * size / emptySquares) > 0)

                    {
                        //создаем новую фишку
                        Token newToken =Instantiate(Resources.Load("Prefabs/Token"),new Vector3(startPosition.x + i, startPosition.y + j, startPosition.z), Quaternion.identity) as Token;
                    }
                    //Иначе
                    else
                    {
                        //Ведем подсчет пустых мест
                        emptySquares--;
                    }
                }
            }
        }
    


    }
}
