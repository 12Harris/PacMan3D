using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Pellet : MonoBehaviour
{
	public int points = 10;

	protected virtual void Eat()
	{
		GameManager.Instance.PelletEaten(this);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
		{
			Eat();
		}
	}

}
