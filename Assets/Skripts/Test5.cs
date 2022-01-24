using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test5 : MonoBehaviour
{
    public float horizontalInput; // значение ввода
    public float cameraSpeed = 10.0f;  //  скорость камеры
    public float speed = 10.0f;  //  скорость камеры
    public float camDefault = 17.8f; // изначальное положение по высоте
    public float xRange = 18.0f; // диапозон перемещения по x координатам
    [SerializeField] GameObject сameraMove; // камера
    [SerializeField] GameObject hitObject; // цель мыши
    [SerializeField] bool zoom; // камера приближённая к объекту или нет
    [SerializeField] bool zoomPos; //позиция камеры при отдалении
    [SerializeField] Vector3 camPoz; // изначальное положение камеры до приблежения к объекту
    [SerializeField] Vector3 offset; // разница по Y оси чтобы отдалится от объекта

    // Start is called before the first frame update
    void Start()
    {
        сameraMove.transform.position = new Vector3(4, 18, -30);
        zoomPos = false;
        zoom = false; // при старте игры приблежение выключено
    }
    
    // Update is called once per frame
    void Update()
    {
        TargetZoom();
        TargetMove();
        CameraMove(); // метод движения камеры
    }

    void TargetMove()
    {
        if (zoom == true)
        {
            сameraMove.transform.position = Vector3.Lerp(сameraMove.transform.position, hitObject.transform.position + offset, speed * Time.deltaTime);
        }

        if (сameraMove.transform.position.y >= camDefault && zoomPos == true)
        {
            zoomPos = false;
            сameraMove.transform.position = camPoz;
        }

        if (zoom == false && zoomPos == true)
        {
            сameraMove.transform.position = Vector3.Lerp(сameraMove.transform.position, camPoz, speed * Time.deltaTime);
        }

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
                    zoom = true;
                    hitObject = mouseHit.transform.gameObject; // выбранный объект
                    camPoz = сameraMove.transform.position; // сохранение текущей позиции камеры
                }
                else if (mouseHit.collider.gameObject.tag == "Target" && zoom == true) // если при нажатии на объект приближение равно 1 
                {
                    zoom = false;
                    zoomPos = true;
                }
            }
        }
    }

    void CameraMove()
    {
        if (zoom == false && zoomPos == false) // если приближение равно 0
        {
            if (сameraMove.transform.position.x < -xRange) // если позиция камеры по x меньше чем ограничение
            {
                сameraMove.transform.position = new Vector3(-xRange, transform.position.y, transform.position.z); // камера останавливается
            }
            if (сameraMove.transform.position.x > xRange) // если позиция камеры по x больше чем ограничение
            {
                сameraMove.transform.position = new Vector3(xRange, transform.position.y, transform.position.z); // камера останавливается
            }

            horizontalInput = Input.GetAxis("Horizontal"); // значение ввода
            сameraMove.transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * cameraSpeed); // движение камеры по оси x
        }

    }
}
