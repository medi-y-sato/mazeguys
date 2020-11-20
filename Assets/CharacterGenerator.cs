using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGenerator : MonoBehaviour
{
  public GameObject CharacterPrefab;
  public GameObject MazeGeneratorObject;
  public GameObject GoalObject;
  public int MaxCharacter;

  // Start is called before the first frame update
  void Start()
  {
  }

  // Update is called once per frame
  void Update()
  {
    if (MazeGeneratorObject.GetComponent<MazeGenerator>().makingMazeFlug && this.transform.childCount < MaxCharacter)
    {
      makeCharacter();
    }

  }

  GameObject makeCharacter()
  {
    GameObject _character = UnityEngine.Object.Instantiate(CharacterPrefab) as GameObject;
    _character.GetComponent<characterNaviScript>().GoalObject = GoalObject;
    _character.transform.position = this.transform.position;
    _character.transform.parent = this.transform;

    return _character;
  }
}
