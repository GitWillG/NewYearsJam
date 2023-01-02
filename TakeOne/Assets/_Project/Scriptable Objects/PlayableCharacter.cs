using UnityEngine;

[CreateAssetMenu(fileName = "CreateCharacter", menuName = "Assets/Create/CreateCharacter", order = 1)]
public class PlayableCharacter : ScriptableObject
{
    [SerializeField] private string characterName;
    [SerializeField] private int numOfDice;
    [SerializeField] private int lifeMod;

    [SerializeField] private GameObject diePrefab;

    public string CharacterName => characterName;

    public int NumOfDice
    {
        get => numOfDice;
        set => numOfDice = value;
    }

    public int LifeMod => lifeMod;

    //TODO: Refactor this to work with multiple types of dice and new dice
    public GameObject DiePrefab => diePrefab;
}
