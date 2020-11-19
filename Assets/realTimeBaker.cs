using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshSurface))]

public class realTimeBaker : MonoBehaviour
{

  NavMeshSurface _surface;

  // Start is called before the first frame update
  void Start()
  {
    _surface = GetComponent<NavMeshSurface>();
    Debug.Log(message: _surface.enabled);
    StartCoroutine(TimeUpdate());

  }
  IEnumerator TimeUpdate()
  {
    while (true)
    {
      _surface.BuildNavMesh();
      Debug.Log("BuildNavMesh()");

      yield return new WaitForSeconds(5.0f);
    }
  }

  // Update is called once per frame
  void Update()
  {

  }
}
