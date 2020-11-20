using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class characterNaviScript : MonoBehaviour
{
  public GameObject GoalObject; /// 目標位置
  public NavMeshAgent m_navMeshAgent; /// NavMeshAgent

  // Start is called before the first frame update
  void Start()
  {
    m_navMeshAgent = GetComponent<NavMeshAgent>();
    this.GetComponent<NavMeshAgent>().enabled = true;
  }

  // Update is called once per frame
  void Update()
  {

    // NavMeshが準備できているなら
    if (m_navMeshAgent.pathStatus != NavMeshPathStatus.PathInvalid)
    {
      // NavMeshAgentに目的地をセット
      m_navMeshAgent.SetDestination(GoalObject.transform.position);
    }

  }

  private void OnTriggetEnter(Collider other)
  {
    //接触したオブジェクトのタグが"Respawn"もしくは"Finish"のとき
    if (other.CompareTag("Respawn") || other.CompareTag("Finish"))
    {
      // 消え去る
      Destroy(this.gameObject);
    }
  }

  private void OnTriggerStay(Collider other)
  {
    //接触したオブジェクトのタグが"Respawn"もしくは"Finish"のとき
    if (other.CompareTag("Respawn") || other.CompareTag("Finish"))
    {
      // 消え去る
      Destroy(this.gameObject);
    }
  }
}
