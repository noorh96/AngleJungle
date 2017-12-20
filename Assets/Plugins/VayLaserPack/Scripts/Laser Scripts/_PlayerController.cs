using UnityEngine;
using System.Collections;

public class _PlayerController : MonoBehaviour {


    public float playerVelocity;
    public float playerRotationVelocity; 

    private CharacterController _characterController;
    private Camera _playerCamera;
    private float _rotateX;
    private float _rotateY; 


    void Start() {
        //Cursor.lockState = CursorLockMode.Locked;

        _playerCamera = Camera.main; 
        gameObject.AddComponent<CharacterController>();
        _characterController = GetComponent<CharacterController>();
    }

    void Update() {
        Move();
        //Rotate(); 
    }


    private void Move()
    {
        float h = Input.GetAxis("Horizontal") * playerVelocity;
        float v = Input.GetAxis("Vertical") * playerVelocity;
        Vector3 _movement = new Vector3(h, 0, v);
        _movement = transform.rotation * _movement; 
        _characterController.Move(_movement * Time.deltaTime);
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
