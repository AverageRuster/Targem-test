using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionController : MonoBehaviour
{
    [HideInInspector] public Rigidbody rb;

    //magnet
    [SerializeField] private GameObject magnetGameObject = null;                                                                                //Объект, к которому притягиваются конструкции (магнит)
    private const float magnetCaptureDistance = 0.5f;                                                                                           //Расстояние, на котором действует магнит

    //movement
    public Vector3 movementVector = Vector3.zero;                                                                                               //Вектор движения конструкции
    private float acceleration = 0;                                                                                                             //Ускорение конструкции
    public float maxSpeed = 0;                                                                                                                  //Максимальная скорость конструкции           

    //rotation
    private Quaternion rotationAngle;                                                                                                           //Угол, на который будет поварачиваться конструкция (в течение одного кадра)

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        acceleration = 5000;                                                                                                                    //Задаем ускорение для объекта
        maxSpeed = 100;                                                                                                                         //Задаем максимальную скорость для объекта
        rotationAngle = Quaternion.Euler(Random.Range(0, 11), Random.Range(0, 11), Random.Range(0, 11));                                        //Задаем для конструкции случайный угол вращения
    }

    private void FixedUpdate()
    {
        transform.rotation = transform.rotation * rotationAngle;                                                                                //Вращаем конструкцию

        float distanceBetweenConstructionAndMagnet = Mathf.Abs(Vector3.Distance(transform.position, magnetGameObject.transform.position));      //Расстояние между магнитом и конструкцией

        if (GameManager.isConstructionOnMagnet &&                                                                                               //Проверяем, занят ли магнит
            gameObject == GameManager.constructionOnMagnet &&                                                                                   //Проверяем, заняла ли магнит эта конструкция  
            distanceBetweenConstructionAndMagnet > magnetCaptureDistance                                                                        //Проверяем расстояние между магнитом и конструкцией
            )
        {
            GameManager.isConstructionOnMagnet = false;                                                                                         //Освобождаем магнит, если эта конструкция занимала его и если она находится на большом расстоянии от магнита   
        }

        if (distanceBetweenConstructionAndMagnet > magnetCaptureDistance || GameManager.isConstructionOnMagnet)                                 //Проверяем, попала ли конструкция в поле действия магнита
        {
            if (rb.velocity.magnitude < maxSpeed)                                                                                               //Проверяем, больше ли текущая скорость объекта максимальной
            {
                movementVector = (magnetGameObject.transform.position - transform.position).normalized;                                         //Указываем направление для ускорения
                rb.AddForce(movementVector * acceleration * Time.fixedDeltaTime);                                                               //Добавляем объекту ускорение, если его скорость меньше максимальной
            }
        }
        else                                                                                                                                    //Если конструкция попала в зону действия магнита и магнит не занят, то она притягивается к нему
        {                                                                                                                                  
            rb.MovePosition(magnetGameObject.transform.position);                                                                               //Перемещаем конструкцию на магнит 
            rb.velocity = Vector3.zero;                                                                                                         //Обнуляем скорость    
            GameManager.constructionOnMagnet = gameObject;                                                                                      //Передаем объект конструкции магниту  
            GameManager.isConstructionOnMagnet = true;                                                                                          //Занимаем магнит
        }
    }
}
