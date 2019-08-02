
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Texture2D _cursorTex;

    private readonly Vector3 centerPoint = Vector3.zero;
    private readonly float rotateSpeed = 0.1f;
    private readonly float maxRotAngle = 85f; 
    private readonly float minRotAngle = 5f; // 不能小于零
    private readonly float sensitivetyKeyBoard = 2f;
    private readonly float mouseMoveSpeed = 0.2f;
    private readonly float zoomSpeed = 1f;
    private readonly float wheelSpeed = 1000;

    private float angle = 0;
    private Vector3 _lastMousePos;
    private Vector3 _lastMousePos2;
    private Vector3 _lastCameraAngle;
    private bool _leftMouseDown;
    private bool _rightMouseDown;
    
    // Start is called before the first frame update
    void Start()
    {
        if (_mainCamera == null)
        {
            _mainCamera = Camera.current;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //CameraFOV();
        KeyboardFOV();
        //CameraRotate();
        WheelRotate();
        CameraMove();
        KeydownMove();
    }
    
//--------------------- 
//作者：冰蝶zh 
//来源：CSDN 
//原文：https://blog.csdn.net/dlhcoder/article/details/85942743 
//版权声明：本文为博主原创文章，转载请附上博文链接！
    /// <summary>
    /// 滚轮控制相机视角缩放
    /// </summary>
    public void CameraFOV()
    {
        //获取鼠标滚轮的滑动量
        float wheel = Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * wheelSpeed;

        //改变相机的位置
        _mainCamera.transform.Translate(Vector3.forward * wheel);
    }

    public void KeyboardFOV()
    {
        if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.PageDown))
        { // Zoom in
            //改变相机的位置
            _mainCamera.transform.Translate(Vector3.forward * zoomSpeed);
        }
        else if(Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.PageUp))
        { // Zoom out
            //改变相机的位置
            _mainCamera.transform.Translate(-Vector3.forward * zoomSpeed);
        }
    }
    

    /// <summary>
    /// 右键控制旋转，仅绕X轴旋转，控制俯仰角
    /// </summary>
    public void CameraRotate()
    {
        //注意!!! 此处是 GetMouseButton() 表示一直长按鼠标左键；不是 GetMouseButtonDown()
        if (Input.GetMouseButton(1))
        {
            Vector3 mousePos = Input.mousePosition;
            if (!_rightMouseDown)
            {
                _lastMousePos2 = mousePos;
                _rightMouseDown = true;
            }
            Vector3 deltaMousePos = mousePos - _lastMousePos2;
            float angleX = deltaMousePos.y;
            
            //控制相机绕中心点(centerPoint)水平旋转
            Quaternion rot = _mainCamera.transform.rotation;
            //如果总角度超出指定范围，结束这一帧（！用于解决相机旋转到模型底部的Bug！）
            //（这样做其实还有小小的Bug，能发现的网友麻烦留言告知解决办法或其他更好的方法）
            if ((rot.eulerAngles.x + angleX)%360 > maxRotAngle || (rot.eulerAngles.x + angleX)%360 < minRotAngle)
            {
                _lastMousePos2 = mousePos;
                return;
            }
            
            _mainCamera.transform.Rotate(angleX * rotateSpeed, 0, 0);
            _lastMousePos2 = mousePos;
        }
        else
        {
            if (_rightMouseDown)
            {
                _rightMouseDown = false;
            }
        }
    }
    
    /// <summary>
    /// 鼠标滚轮控制旋转，仅绕X轴旋转，控制俯仰角
    /// </summary>
    public void WheelRotate()
    {
        //获取鼠标滚轮的滑动量
        float wheel = Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * wheelSpeed;
        
        //注意!!! 此处是 GetMouseButton() 表示一直长按鼠标左键；不是 GetMouseButtonDown()
        float angleX = wheel * 15;
            
        //控制相机绕中心点(centerPoint)水平旋转
        Quaternion rot = _mainCamera.transform.rotation;
        //如果总角度超出指定范围，结束这一帧（！用于解决相机旋转到模型底部的Bug！）
        //（这样做其实还有小小的Bug，能发现的网友麻烦留言告知解决办法或其他更好的方法）
        if ((rot.eulerAngles.x + angleX)%360 > maxRotAngle || (rot.eulerAngles.x + angleX)%360 < minRotAngle)
        {
            return;
        }
        
        _mainCamera.transform.Rotate(angleX * rotateSpeed, 0, 0);
    }
    
    /// <summary>
    /// 右键控制拖动
    /// </summary>
    public void CameraMove()
    {
        if (Input.GetMouseButton(1))
        {
            Vector3 mousePos = Input.mousePosition;
            if (!_leftMouseDown)
            {
                //鼠标图标换成自定义小手
                if (_cursorTex != null)
                {
                    Cursor.SetCursor(_cursorTex, Vector2.zero, CursorMode.Auto);
                }
                _lastMousePos = mousePos;
                _leftMouseDown = true;
            }

            Vector3 deltaMousePos = mousePos - _lastMousePos;
            _mainCamera.transform.position += new Vector3(-deltaMousePos.x, 0, -deltaMousePos.y) * mouseMoveSpeed;
            _lastMousePos = mousePos;
            //Debug.Log("DeltaMousePos <"+deltaMousePos.y+">");
        }
        else
        {
            if (_leftMouseDown)
            {
                //鼠标恢复默认图标，置null即可
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                _leftMouseDown = false;
            }
        }
    }

//        --------------------- 
//            作者：yongh701 
//            来源：CSDN 
//            原文：https://blog.csdn.net/yongh701/article/details/71082441 
//        版权声明：本文为博主原创文章，转载请附上博文链接！
    public void KeydownMove()
    {
        //键盘按钮←/a和→/d实现视角水平移动，键盘按钮↑/w和↓/s实现视角水平旋转
        if (Input.GetAxis("Horizontal") != 0)
        {
            transform.position += new Vector3(Input.GetAxis("Horizontal") * sensitivetyKeyBoard, 0, 0);
        }
        if (Input.GetAxis("Vertical") != 0)
        {
            transform.position += new Vector3(0, 0, Input.GetAxis("Vertical") * sensitivetyKeyBoard);
        }

    }
}
