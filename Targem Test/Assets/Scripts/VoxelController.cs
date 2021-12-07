using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelController : MonoBehaviour
{

    [SerializeField] public ConstructionController constructionController;
    [SerializeField] public GameObject constructionGameObject;

    //color
    private MeshRenderer meshRenderer;
    private Color markedVoxelColor;

    Vector3 savedImpulse;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        markedVoxelColor = new Color(1, 0, 0, 1);                                                                                                   //Цвет вокселя после касания с другим вокселем
    }

    private void OnTriggerEnter(Collider collision)
    {
        GameObject colliiderGO = collision.gameObject;

        if (collision.gameObject.CompareTag("Voxel"))                                                                                               //Проверяем соприкасновение вокселя с другим вокселем
        {
            VoxelController voxel = colliiderGO.GetComponent<VoxelController>();
            Rigidbody thisConstructionRigidbody = constructionController.rb;
            Rigidbody enteringConstructionRigidbody = voxel.constructionController.rb;

            if (GameManager.voxelsTouches == 0 &&                                                                                                   //Проверяем, есть ли воксели из разных конструкций, соприкасающиеся друг с другом
                thisConstructionRigidbody.velocity.magnitude < constructionController.maxSpeed &&                                                   //Проверяем скорость конструкции для избежания нескольких соприкосновений подряд
                enteringConstructionRigidbody.velocity.magnitude < voxel.constructionController.maxSpeed)
            {
                savedImpulse = thisConstructionRigidbody.velocity;                                                                                  //Сохраняем импульс для обмена

                if (savedImpulse != Vector3.zero && enteringConstructionRigidbody.velocity != Vector3.zero)                                         //Если оба импульса ненулевые, то меняем их друг с другом
                {
                    thisConstructionRigidbody.velocity = enteringConstructionRigidbody.velocity * 2;                                                //Передача импульса от другой конструкции (умножаем импульс на 2 для избежания затухания)
                    enteringConstructionRigidbody.velocity = savedImpulse * 2;                                                                      //Передача импульса другой конструкции (умножаем импульс на 2 для избежания затухания)    
                }
                else if (savedImpulse == Vector3.zero)                                                                                              //Если один из импульсов равен нулю, передаем ему второй импульс    
                {
                    thisConstructionRigidbody.velocity = enteringConstructionRigidbody.velocity * 2;                                                //Передача импульса от другой конструкции (умножаем импульс на 2 для избежания затухания)
                    enteringConstructionRigidbody.velocity /= 2;                                                                                    //уменьшение импульса    
                }
                else if (enteringConstructionRigidbody.velocity == Vector3.zero)                                                                    //Если один из импульсов равен нулю, передаем ему второй импульс    
                {
                    enteringConstructionRigidbody.velocity = thisConstructionRigidbody.velocity * 2;                                                //Передача импульса от другой конструкции (умножаем импульс на 2 для избежания затухания)
                    thisConstructionRigidbody.velocity /= 2;                                                                                        //уменьшение импульса
                }

                GameManager.currentTouchesAmount++;                                                                                                 //Фиксируем соприкосновение
            }

            if (meshRenderer.material.color != markedVoxelColor)                                                                                    //Проверяем, совпадает ли текущий цвет вокселя с цветом вокселя после касания
            {
                meshRenderer.material.color = markedVoxelColor;                                                                                     //Если цвета не совпадают, то заменям
            }

            GameManager.voxelsTouches++;                                                                                                            //Фиксируем соприконовение вокселей
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        GameObject colliiderGO = collision.gameObject;

        if (colliiderGO.CompareTag("Voxel"))
        {
            GameManager.voxelsTouches--;                                                                                                            //Фиксируем окончание соприкасновения вокселей
        }
    }
}
