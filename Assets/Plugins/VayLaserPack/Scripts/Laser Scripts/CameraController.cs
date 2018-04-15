using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public float cameraVelocity;
    public float playerRotationVelocity;

    private CharacterController _characterController;
    private Camera _playerCamera;
    private float _rotateX;
    private float _rotateY;

    private bool pause; 


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        _playerCamera = transform.Find("Main Camera").GetComponent<Camera>();
        gameObject.AddComponent<CharacterController>();
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause = !pause; 
        }

        if (!pause)
        {
            Move();
            Rotate();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true; 

        }

    }


    private void Move()
    {

        if (Input.GetKey(KeyCode.W))
        {
            transform.position = transform.position + (_playerCamera.transform.forward * Time.deltaTime) * cameraVelocity; 
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position = transform.position + (_playerCamera.transform.right * Time.deltaTime) * cameraVelocity;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position = transform.position + (-_playerCamera.transform.right * Time.deltaTime) * cameraVelocity;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position = transform.position + (-_playerCamera.transform.forward * Time.deltaTime) * cameraVelocity;
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            transform.position = transform.position + (_playerCamera.transform.up * Time.deltaTime) * cameraVelocity;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.position = transform.position + (-_playerCamera.transform.up * Time.deltaTime) * cameraVelocity;
        }
    }

    private void Rotate()
    {
        _rotateX = Input.GetAxis("Mouse X") * playerRotationVelocity;
        _rotateY -= Input.GetAxis("Mouse Y") * playerRotationVelocity;
        transform.Rotate(new Vector3(0, _rotateX, 0));
        _rotateY = Mathf.Clamp(_rotateY, -60, 60);
        _playerCamera.transform.localRotation = Quaternion.Euler(new Vector3(_rotateY, 0, 0));

    }
}
