using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test5 : MonoBehaviour
{
    public float horizontalInput; // значение ввода
    public float speed = 10.0f;  //  скорость камеры
    public float xRange = 18.0f; // диапозон перемещения по x координатам
    [SerializeField] GameObject сameraMove; // камера
    [SerializeField] bool zoom; // камера приближённая к объекту или нет
    [SerializeField] Vector3 camPoz; // изначальное положение камеры до приблежения к объекту
    [SerializeField] Vector3 offset; // разница по Z оси чтобы отдалится от объекта

    // Start is called before the first frame update
    void Start()
    {
        zoom = false; // при старте игры приблежение выключено
    }
    
    // Update is called once per frame
    void Update()
    {
        CameraMove(); // метод движения камеры
        TargetZoom(); // метоб приближения камеры к выбранному мышью объекту
    }

    void TargetZoom()
    {
        if (Input.GetMouseButtonDown(0)) // если нажата левая кнопка мыши
        { 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // запуск луча в точку клика
        RaycastHit mouseHit; // объявление точки луча
            if (Physics.Raycast(ray, out mouseHit)) // если райкаст попал на объект
            {
                if (mouseHit.collider.gameObject.tag == "Target" && zoom == false) // если луч попал на объект с тегом Target и при этом приближение равно 0
                {
                    GameObject hitObject = mouseHit.transform.gameObject; // выбранный объект
                    camPoz = сameraMove.transform.position; // сохранение текущей позиции камеры
                    сameraMove.transform.position = hitObject.transform.position + offset; // приближение камеры к объекту
                    zoom = true; // приближение равно 1
                }
                else if (mouseHit.collider.gameObject.tag == "Target" && zoom == true) // если при нажатии на объект приближение равно 1 
                {
                    сameraMove.transform.position = camPoz; // возвращение камеры в исходное положение
                    zoom = false; // приближение равно 0
                }
            }
        }
    }

    void CameraMove()
    {
        if (zoom == false) // если приближение равно 0
        {
            if (сameraMove.transform.position.x < -xRange) // если позиция камеры по x меньше чем ограничение
            {
                сameraMove.transform.position = new Vector3(-xRange, transform.position.y, transform.position.z); // камера останавливается
            }
            if (сameraMove.transform.position.x > xRange) // если позиция камеры по x больше чем ограничение
            {
                сameraMove.transform.position = new Vector3(xRange, transform.position.y, transform.position.z); // камера останавливается
            }
        }

            horizontalInput = Input.GetAxis("Horizontal"); // значение ввода
            сameraMove.transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * speed); // движение камеры по оси x
        
    }
}