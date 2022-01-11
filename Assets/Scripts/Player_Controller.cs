using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
	[SerializeField] GameObject pObject, partEff_Clash, partEff_Portal;
	private Vector3 screenPoint, offset, _mouseReference, _mouseOffset, _rotation;
	Rigidbody rb;
	[SerializeField]
	private float _sensitivity;
	private bool _isRotating;

	GameManager gameManager;
	void Awake()
	{
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		_rotation = Vector3.zero;
	}

	void Start()
	{
		rb = gameObject.GetComponent<Rigidbody>();
	}
	void Update()
	{
		rb.AddForce(0, 0, 3f);
		ClickToShoot();
		rotationTry();
	}
	void rotationTry() 
	{
		if (_isRotating)
		{
			// offset
			_mouseOffset = (Input.mousePosition - _mouseReference);

			// apply rotation
			_rotation.x = -(_mouseOffset.x + _mouseOffset.y) * _sensitivity;

			// rotate
			transform.Rotate(_rotation);

			// store mouse
			_mouseReference = Input.mousePosition;
		}
	}
	//THIS METHODS CAN BE USABLE FOR EVERY INTERACTIVE OBJECT
	void OnMouseDrag()
	{
		Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
		transform.position = cursorPosition;
	}
	void OnMouseDown()
	{
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
		// rotating flag
		_isRotating = true;
		// store mouse
		_mouseReference = Input.mousePosition;
	}
	//THIS METHODS CAN BE USABLE FOR EVERY INTERACTIVE OBJECT
	void OnMouseUp() /*Test*/
	{
		// rotating flag
		_isRotating = false;
	}

	void ClickToShoot() 
	{
		if(gameManager.shootAmmo > 0) 
		{
			if (Input.GetMouseButtonDown(0))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 100.0f) && hit.transform.gameObject.tag == "DangerObject")
				{
					GameObject.Destroy(hit.transform.gameObject);
					//Instantiate(partEff_Clash, hit.transform.gameObject, Quaternion.identity, hit.transform.parent);
					gameManager.shootAmmo--;
				}
			}
        }
        else 
		{
			//Debug.Log("No Ammo");
		}
	}
	IEnumerator _crash() 
	{
		pObject.SetActive(false);
		yield return new WaitForSeconds(0.1f);
		GameObject go = Instantiate(partEff_Clash, pObject.transform.position, Quaternion.identity);
		yield return new WaitForSeconds(3f);
		Destroy(go, 2f);
		yield return new WaitForSeconds(3f);
		gameManager.GameOver();
	}
	IEnumerator _NextToPortal() 
	{
		pObject.SetActive(false);
		yield return new WaitForSeconds(0.1f);
		GameObject go = Instantiate(partEff_Portal, pObject.transform.position, Quaternion.identity);
		yield return new WaitForSeconds(3f);
		Destroy(go, 2f);
		yield return new WaitForSeconds(3f);
		gameManager.gettingNextLevel();
		//NextToPortal();
	}
	public void Crash()
	{
		pObject.SetActive(false);
		Destroy(Instantiate(partEff_Clash, pObject.transform.position, Quaternion.identity), 2f);
		//Instantiate(partEff_Clash, pObject.transform.position, Quaternion.identity);
		//gameManager.GameOver();
	}
	public void NextToPortal() 
	{
		pObject.SetActive(false);
		//Instantiate(partEff_Portal, pObject.transform.position, Quaternion.identity);
		Destroy(Instantiate(partEff_Portal, pObject.transform.position, Quaternion.identity), 2f);
		//gameManager.gettingNextLevel();
	}
	public void resTheShip() 
	{
		pObject.SetActive(true);
	}
	public void stopTheShipForAMoment() 
	{
		//GetComponent<Rigidbody>().velocity = Vector3.zero;
		rb.velocity = Vector3.zero;
	}

	void OnCollisionEnter(Collision collision)
    {
		if (collision.gameObject.tag == "DangerObject" || collision.gameObject.tag == "Corridor")
		{
			Crash();
			//Invoke("gameManager.GameOver", 1);
			gameManager.Invoke("GameOver", 1);
			//gameManager.GameOver();
		}
		if (collision.gameObject.tag == "ScoreCoin")
		{
			gameManager.AddScore();
			Instantiate(partEff_Portal, pObject.transform.position, Quaternion.identity);
		}
	}
    void OnTriggerExit(Collider other)
    {
		if (other.gameObject.tag == "Portal")
		{
			NextToPortal();
			gameManager.Invoke("gettingNextLevel", 1);
			//Invoke("gameManager.gettingNextLevel", 1);
			//gameManager.gettingNextLevel();
		}
	}
}
