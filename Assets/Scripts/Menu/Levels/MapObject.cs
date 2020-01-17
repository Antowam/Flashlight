using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Level", order = 1)]
public class MapObject : ScriptableObject
{
    public string levelName;

    public int levelID;

    public Sprite levelThumbnail;
}