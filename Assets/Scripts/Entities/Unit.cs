using UnityEngine;
using System.Collections;

public class Unit : SelectableEntity {

    public enum ECategory
    {
        Light,
        Medium,
        Heavy
    }

    private Formation formation = null;
    public Formation Formation { get { return formation; } set { formation = value; } }

    [SerializeField]
    private ECategory Category;
    [SerializeField]
    private int Cost = 1;
    [SerializeField]
    private int Hp;
    [SerializeField]
    private int MaxHp = 100;

    private bool isAlive = true;
    public bool IsAlive { get { return isAlive;} }

    public int GetCost { get { return Cost; } }
    public ECategory GetCategory { get { return Category; } }

    public Movement movement;
    private UnitFactory factory;

    public delegate void OnDeathEventHandler(object sender);
    public event OnDeathEventHandler OnDeadEvent;

    private float lastDamageDate = 0f;
    private GameObject hitFX;

    private bool isInitialized = false;
    [SerializeField]
    private int damages;

    public void Init(Team _team)
    {
        if (isInitialized)
            return;
        team = _team;
        Hp = MaxHp;
        OnDeadEvent += Unit_OnDead;

        isInitialized = true;
    }

    // Use this for initialization
    override protected void Awake()
    {
        base.Awake();
        hitFX = transform.Find("HitFX").gameObject;
        movement = GetComponent<Movement>();
        OnDeadEvent += Unit_OnDead;
	}

    private void Unit_OnDead(object sender)
    {
        isAlive = false;
        Destroy(gameObject);
    }

    // Update is called once per frame
    private void Update ()
    {
	}

    public void SetTargetPos(Vector3 pos)
    {
        movement.SetTarget(pos);
    }

    public void AddDamages(int damages)
    {
        Hp -= damages;
        StartCoroutine(TakeDamageFeedback());
        if (Hp <= 0)
        {
            OnDeadEvent(this);
        }
    }

    private IEnumerator TakeDamageFeedback()
    {
        hitFX.SetActive(true);
        yield return new WaitForSeconds(1);
        hitFX.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Unit unit = other.GetComponent<Unit>();
        if (unit != null)
        {
            if (unit.GetTeam != team)
                movement.PreventFormationForAttack(true);
            return;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Unit unit = other.GetComponent<Unit>();
        if (unit != null)
        {
            if (unit.GetTeam != team)
            {
                unit.AddDamages(damages);
                if (unit.Hp <= 0)
                    movement.PreventFormationForAttack(false);
            }
            return;
        }
    }
}
