
using UnityEngine;

public class GhostFrightened : GhostBehavior
{
	/*public SpriteRenderer body;
	public SpriteRenderer eyes;
	public SpriteRenderer blue;
	public SpriteRenderer white;*/

	[SerializeField]
	private LayerMask nodeLayer;

	private bool eaten;

	public override void Enable(float duration)
	{
		base.Enable(duration);

		/*body.enabled = false;
		eyes.enabled = false;
		blue.enabled = true;
		white.enabled = false;*/

		Invoke(nameof(Flash), duration / 2f);
	}

	public override void Disable()
	{
		base.Disable();

		/*body.enabled = true;
		eyes.enabled = true;
		blue.enabled = false;
		white.enabled = false;*/
	}

	private void Eaten()
	{
		eaten = true;
		ghost.eaten.Enable();
		//ghost.SetPosition(ghost.home.inside.position);
		//ghost.home.Enable(duration);

		//Disable();

		/*body.enabled = false;
		eyes.enabled = true;
		blue.enabled = false;
		white.enabled = false;*/
	}

	private void Flash()
	{
		if (!eaten)
		{
			/*blue.enabled = false;
			white.enabled = true;
			white.GetComponent<AnimatedSprite>().Restart();*/
		}
	}

	private void OnEnable()
	{
		//blue.GetComponent<AnimatedSprite>().Restart();
		ghost.movement.speedMultiplier = 0.5f;
		ghost.movement.direction *= -1;
		eaten = false;
	}

	private void OnDisable()
	{
		ghost.movement.speedMultiplier = 1f;
		eaten = false;
	}

	public virtual void Update()
	{

		RaycastHit hit;
		UnityEngine.Debug.DrawRay(transform.position, ghost.movement.direction, Color.red);
		if (Physics.Raycast(transform.position, ghost.movement.direction, out hit, 1f, nodeLayer))
		{
			Node node = hit.collider.gameObject.GetComponent<Node>();

			if (node != null && enabled && !ghost.eaten.enabled)
			{
				Vector3 direction = Vector3.zero;
				float maxDistance = float.MinValue;

				// Find the available direction that movesfarthes  to the target
				foreach (Node nnode in node.neighborNodes)
				{
					var nodeDir = (nnode.transform.position - node.transform.position).normalized;

					if (nodeDir == -ghost.movement.direction)
						continue;

					Vector3 newPosition = transform.position + nodeDir;

					float distance = (Pacman.Instance.transform.position - newPosition).sqrMagnitude;

					if (distance > maxDistance)
					{
						direction = nodeDir;
						maxDistance = distance;
					}
				}

				ghost.movement.nextDirection = direction;
				ghost.movement.SetExitLocation(node.transform.position);
				ghost.movement._reachedExitLocation = false;
			}
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
		{
			if (enabled)
			{
				Eaten();
			}
		}
	}

}
