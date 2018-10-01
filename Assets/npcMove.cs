using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class npcMove : MonoBehaviour {

    [SerializeField]
    Transform _destination;
    Rigidbody body;
    NavMeshAgent _navMeshAgent;
	void Start () {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        if(_navMeshAgent == null)
        {
            Debug.Log("Nav mesth agent component is not attached to : " + gameObject.name);
        }
        else
        {
            SetDestination();
        }
        InvokeRepeating("SetDestination", 2, 2);
	}
    private void SetDestination()
    {
        
        if(_destination != null && _navMeshAgent!=null)
        {
            Vector3 targetVector = _destination.transform.position;
            _navMeshAgent.SetDestination(targetVector);
        }
    }
	
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="bullet")
        {
            body = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
            Destroy(_navMeshAgent);
        }
    }
}
