using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerConfig", menuName = "Configs/Player")]
public class PlayerConfig : ScriptableObject
{
    public float speed = 5f;
    public int maxHealth = 10;
}