using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Obi;

[RequireComponent(typeof(Obi.ObiSolver))]
public class CollisionHandler : MonoBehaviour
{
	public ObiCollider groundCollider;

	private Obi.ObiSolver solver;

	private Obi.ObiSolver.ObiCollisionEventArgs collisionEvent;


	void Awake()
    {
		solver = gameObject.GetComponent<Obi.ObiSolver>();
	}


	void OnEnable()
	{
        solver.OnCollision += Solver_OnCollision;
		solver.OnParticleCollision += Solver_OnParticleCollision;
	}

	void OnDisable()
	{
		solver.OnCollision -= Solver_OnCollision;
		solver.OnParticleCollision -= Solver_OnParticleCollision;
	}

	void Solver_OnCollision(object sender, Obi.ObiSolver.ObiCollisionEventArgs e)
	{
	}

	void Solver_OnParticleCollision(ObiSolver solver, ObiSolver.ObiCollisionEventArgs contacts)
    {
		if (!RopeCtrl.instance.isRunning)
        {
			return;
        }
		var world = Obi.ObiColliderWorld.GetInstance();
		bool hasKnot = false;
		foreach (Oni.Contact contact in contacts.contacts)
		{
			// this one is an actual collision:
			if (contact.distance < 0.01)
			{
				ObiSolver.ParticleInActor paticleA = solver.particleToActor[contact.bodyA];
                ObiSolver.ParticleInActor paticleB = solver.particleToActor[contact.bodyB];
                if (paticleA.actor == paticleB.actor)
                {
                    Debug.Log($"collide with { paticleA.actor.name}");
					// get the particle index directly, as all simplices are guaranteed to have size 1:
					int particleIndexA = solver.simplices[contact.bodyA];
					int particleIndexB = solver.simplices[contact.bodyB];

					// do something with the particle, for instance get its position:
					var positionA = solver.positions[particleIndexA];
					var positionB = solver.positions[particleIndexB];

					if (Mathf.Abs(positionA.y -positionB.y) > 0.01f)
                    {
						hasKnot = true;
						Debug.Log("has knot......................");
						break;
                    }

				}
            }
		}
		if (!hasKnot)
        {
			RopeCtrl.instance.Success();
        }
	}
}