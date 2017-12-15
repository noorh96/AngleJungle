using UnityEngine;
public class DragCamera2 : MonoBehaviour
{
	private Vector3 _screenPoint;
	private Vector3 _offset;
	private Vector3 _curScreenPoint;
	private Vector3 _curPosition;
	private Vector3 _velocity;
	private bool _underInertia;
	private float _time = 0.0f;
	public float SmoothTime;
	void Update()
	{
		if(_underInertia && _time <= SmoothTime)
		{
			transform.position += _velocity;
			_velocity = Vector3.Lerp(_velocity, Vector3.zero, _time);
			_time += Time.smoothDeltaTime;
		}
		else
		{
			_underInertia = false;
			_time = 0.0f;
		}
	}
	void OnMouseDown()
	{
		_screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		_offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z));
		_underInertia = false;
	}
	void OnMouseDrag()
	{
		Vector3 _prevPosition = _curPosition;
		_curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z);
		_curPosition = Camera.main.ScreenToWorldPoint(_curScreenPoint) + _offset;
		_velocity = _curPosition - _prevPosition;
		transform.position = _curPosition;
	}
	void OnMouseUp()
	{
		_underInertia = true;
	}
}