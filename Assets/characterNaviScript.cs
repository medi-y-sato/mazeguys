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
    setGoal();

  }

  void setGoal()
  {
    // NavMeshが準備できているなら
    if (m_navMeshAgent.pathStatus != NavMeshPathStatus.PathInvalid)
    {
      // NavMeshAgentに目的地をセット
      m_navMeshAgent.SetDestination(GoalObject.transform.position);

    }
  }

  private void OnTriggerExit(Collider other)
  {
    //離れたオブジェクトのタグが"Floor"のとき
    if (other.CompareTag("Floor"))
    {
      // NavMeshAgentを止める
      //      this.GetComponent<NavMeshAgent>().enabled = false;
    }
  }

  private void OnTriggetEnter(Collider other)
  {
    removePlayer(other);
  }

  private void OnTriggerStay(Collider other)
  {
    removePlayer(other);
  }

  void removePlayer(Collider other)
  {

    //接触したオブジェクトのタグが"Respawn"のとき
    if (other.CompareTag("Respawn"))
    {
      // 消え去る
      Destroy(this.gameObject);
    }

    //接触したオブジェクトのタグが"Finish"のとき
    if (other.CompareTag("Finish"))
    {
      // 迷路作り直して
      MazeGenerator mazeGenerator = GameObject.Find("MazeGeneratorObject").GetComponent<MazeGenerator>();
      mazeGenerator.createMaze();

      // ゴール動かして
      System.Random r1 = new System.Random();
      int rand_x = r1.Next(0, mazeGenerator.maze_x_def);
      int rand_z = r1.Next(0, mazeGenerator.maze_y_def);
      GameObject.Find("Goal").transform.position = new Vector3(rand_x * 2, 0, rand_z * 2);

      // bakeり直して
      GameObject.Find("NavMeshObject").GetComponent<realTimeBaker>().bakeNow();

// Characterの目標セットし直して
      GameObject respawnPoint = GameObject.Find("RespawnPoint");
      for (int i = 0; i < respawnPoint.transform.childCount; ++i)
      {
        respawnPoint.transform.GetChild(i).GetComponent<characterNaviScript>().setGoal();
      }

      // 消え去る
      Destroy(this.gameObject);
    }
  }
}
