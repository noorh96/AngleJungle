using UnityEngine;
using System.Collections;

public class AlphaFade : MonoBehaviour
{
	private SpriteRenderer sprite;

	public Color spriteColor = Color.white;
	public float fadeInTime = 1.5f;
	public float fadeOutTime = 3f;
	public float moveTime = 5f;
	public float delayToDoAgain = 5f;
	public float initialWaitingTime=3f;
	public Transform targetTrans;
	public GameObject handSprite;
	private Vector3 originalPosition;

	void Start()
	{
		originalPosition = gameObject.transform.position;
		sprite = handSprite.GetComponent<SpriteRenderer> ();
		StartCoroutine("FadeCycle");
	}
		

	IEnumerator FadeCycle()
	{
		float fade = 0f;
		float startTime;
		spriteColor.a = fade;
		sprite.color = spriteColor;
		yield return new WaitForSeconds (initialWaitingTime);
		while(true)
		{
			startTime = Time.time;
			while(fade < 1f)
			{
				fade = Mathf.Lerp(0f, 1f, (Time.time - startTime) / fadeInTime);
				spriteColor.a = fade;
				sprite.color = spriteColor;
				yield return null;
			}
			yield return new WaitForSeconds(delayToDoAgain);

			startTime = Time.time;
			while((transform.position-targetTrans.position).magnitude > 0.1f)
			{
				transform.position = Vector3.Lerp (transform.position, targetTrans.position, (Time.time - startTime) / moveTime);
				yield return null;
			}

			yield return new WaitForSeconds(delayToDoAgain);

			//Make sure it's set to exactly 1f
			fade = 1f;
			spriteColor.a = fade;
			sprite.color = spriteColor;

			startTime = Time.time;
			while(fade > 0f)
			{
				fade = Mathf.Lerp(1f, 0f, (Time.time - startTime) / fadeOutTime);
				spriteColor.a = fade;
				sprite.color = spriteColor;
				yield return null;
			}
			fade = 0f;
			spriteColor.a = fade;
			sprite.color = spriteColor;
			transform.position = originalPosition;
			yield return new WaitForSeconds(delayToDoAgain);
		}
	}
}