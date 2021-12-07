using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;                                                                    //TextMeshPro для работы с текстом интерфейса

public class GameManager : MonoBehaviour
{
    //timer
    private float startTime = 0;                                                //Точка отсчета времени
    private float currentTime = 0;                                              //Текущее время
    private float currentPlayTime = 0;                                          //Время, прошедшее с начала отсчета                                
    private float lastPlayTime = 0;                                             //Переменная для сравнения времени, прошедшего с начала отсчета 
    [SerializeField] TextMeshProUGUI timerText = null;                          //Текст таймера в интерфейсе   

    //counter
    private int lastTouchesAmount = 0;                                          //Переменная для сравнения количества соприкасаний
    public static int currentTouchesAmount = 0;                                 //Количество соприкасаний вокселей
    [SerializeField] TextMeshProUGUI touchCounterText = null;                   //Текст счетчика в интерфейсе

    //voxels
    public static int voxelsTouches = 0;                                        //Количество соприкосновений вокселей друк с другом

    private void Start()
    {
        startTime = Time.time;                                                  //Устанавливаем точку отсчета
    }

    //magnet
    public static GameObject constructionOnMagnet = null;                       //Конструкция, занявшая магнит
    public static bool isConstructionOnMagnet = false;

    private void Update()
    {
        SetTimer();
        SetTouchCounter();
    }

    private void SetTimer()
    {
        currentTime = Time.time;                                                //Текущее время
        currentPlayTime = currentTime - startTime;                              //Время, прошедшее с начала отсчета
        if (lastPlayTime != Mathf.Floor(currentPlayTime))                       //Проверяем, изменилось ли целое количество секунд
        {
            timerText.SetText("Time: " + Mathf.Floor(currentPlayTime));         //Выводим таймер в интерфес
            lastPlayTime = Mathf.Floor(currentPlayTime);                        //Меняем значение переменной для сравнения времени
        }
    }

    private void SetTouchCounter()
    {
        if (lastTouchesAmount != currentTouchesAmount)                          //Проверяем, изменилось ли количество соприкасаний конструкций
        {
            touchCounterText.SetText("Touches: " + currentTouchesAmount);       //Выводим количество соприкасаний в интерфес
            lastTouchesAmount = currentTouchesAmount;                           //Меняем значение переменной для сравнения количества соприкасаний
        }
    }

    public void ResetCounterAndTime()
    {
        startTime = currentTime;                                                //Сброс времени
        currentTouchesAmount = 0;                                               //Сброс количества соприкасаний конструкций
    }
}
