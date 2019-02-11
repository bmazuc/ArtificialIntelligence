using UnityEngine;
using System.Collections.Generic;

public class Movement : MonoBehaviour, ISteeringUser {
    [SerializeField]
    private float MaxSpeed = 50f;
    public float GetMaxSpeed
    {
        get
        { if (unit.Formation != null)
                return unit.Formation.formationSpeed;
            else
                return MaxSpeed;
        }
    }
    [SerializeField]
    public float MaxAccel = 50f;

    [SerializeField]
    private float MaxForce = 50f;

    [SerializeField]
    private float Mass = 20f;

    Vector3 velocity = Vector3.zero;
	public Vector3 Velocity {get{return velocity;}}

    private float rotation = 0f;
	public float Rotation {get{return rotation;}}

    Unit unit;

    private steering.Steering steering;
    fsm.FiniteStateMachine<Movement> stateMachine;

    private void Awake()
    {
        unit = GetComponent<Unit>();
        steering = new steering.Steering();
        InitStateMachine();
    }

    public void Stop()
	{
		velocity = Vector3.zero;
	}

    public void SetTarget(Vector3 pos)
    {
        foreach (steering.SteeringBehavior behavior in GetComponents<steering.SteeringBehavior>())
            behavior.SetTarget(pos);
        Stop();
        stateMachine.SetTrigger("haveTarget");
    }

    public void SetSteering(steering.Steering behaviorSteering)
    {
        steering = behaviorSteering;
    }

    public void Update()
    {
        if (unit.IsAlive == false)
            return;
        
        transform.position += velocity * Time.deltaTime;
        transform.eulerAngles = Vector3.up * rotation;

        stateMachine.SetFloat("velocity", velocity.magnitude);
        stateMachine.Execute();
    }

    public void LateUpdate()
    {
        if (steering.linear.magnitude > MaxForce)
        {
            steering.linear.Normalize();
            steering.linear *= MaxForce;
        }

        steering.linear /= Mass;

        if (steering.linear.magnitude == 0f)
            Stop();
        else
        {
            velocity += steering.linear;
            rotation = steering.angular;


            if (velocity.magnitude > GetMaxSpeed)
            {
                velocity.Normalize();
                velocity *= GetMaxSpeed;
            }

            steering.linear = Vector3.zero;
            steering.angular = 0f;
        }
    }

    private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawRay(transform.position, velocity);
	}

    public void Attack(bool state)
    {
        stateMachine.SetBool("attack", state);
    }

    public void PreventFormationForAttack(bool state)
    {
        if (unit.Formation != null)
            foreach (Unit formationUnit in unit.Formation.unitList)
                formationUnit.movement.Attack(state);

        Attack(state);
    }

    void InitStateMachine()
    {
        stateMachine = new fsm.FiniteStateMachine<Movement>();

        fsm.SteeringBehaviorState<Movement> idle = new fsm.SteeringBehaviorState<Movement>(this, stateMachine);
        idle.name = "idle";
        fsm.SteeringBehaviorState<Movement> walk = new fsm.SteeringBehaviorState<Movement>(this, stateMachine);
        walk.name = "walk";
        fsm.SteeringBehaviorState<Movement> attack = new fsm.SteeringBehaviorState<Movement>(this, stateMachine);
        attack.name = "attack";
        stateMachine.AddInitialState(idle);

        stateMachine.AddBool("attack");
        stateMachine.AddTrigger("haveTarget");
        stateMachine.AddFloat("velocity");

        foreach (steering.SteeringBehavior behavior in GetComponents<steering.SteeringBehavior>())
            walk.AddBehavior(behavior);

        idle.AddTransition(new fsm.Transition<Movement>("haveTarget", walk));
        walk.AddTransition(new fsm.Transition<Movement>("velocity", 0.1f, fsm.E_FloatOperator.LESSER, idle));
        walk.AddTransition(new fsm.Transition<Movement>("attack", true, attack));
        attack.AddTransition(new fsm.Transition<Movement>("attack", false, walk));
    }

}
