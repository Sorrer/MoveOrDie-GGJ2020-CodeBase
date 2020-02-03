using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryEnemy : Entity
{

    public Weapon weapon;

    public float RandomAngle;


    public Transform Head;

    public float MaxRange;

    public override void Pull(Vector3 Force) {
        this.GetComponent<Rigidbody>().AddForce(Force);
    }

    // Start is called before the first frame update
    void Start()
    {
        GlobalVars.Instance.AddEnemyRemaining();
    }

    private void OnDestroy() {

        GlobalVars.Instance.RemoveEnemyRemaining();
    }

    // Update is called once per frame
    void Update()
    {
        Head.LookAt(CharacterMovement.Instance.gameObject.transform);


        float randomNumberX = Random.Range(-RandomAngle, RandomAngle);
        float randomNumberY = Random.Range(-RandomAngle, RandomAngle);
        float randomNumberZ = Random.Range(-RandomAngle, RandomAngle);

        Head.transform.Rotate(randomNumberX, randomNumberY, randomNumberZ);

        if (Vector3.Distance(this.transform.position, CharacterMovement.Instance.gameObject.transform.position) < MaxRange) {
            weapon.Attack();
        }


        if (this.healthManager.IsDead()) {
            Destroy(this.gameObject);
        }
    }
}
