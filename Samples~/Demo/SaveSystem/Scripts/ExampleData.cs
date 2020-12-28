using System;
using GibFrame.SaveSystem;
using GibFrame.SaveSystem.Serializables;
using UnityEngine;

[System.Serializable]
public class ExampleData : EncryptedData
{
    public static string PATH = Application.persistentDataPath + "/example.data";
    public int points;
    public int health;
    // Allow a System.Type to be serialized
    public SerializableType type;
    // Allow a Sprite to be serialized
    public SerializableSprite sprite;

    // Remember to call the base constructor in order to initialize the device Id of the encrypted data
    public ExampleData(int points, int health, Type type, Sprite sprite)
    {
        this.points = points;
        this.health = health;
        // Auto conversion properties, Type is auto converted to SerializableType and Sprite is auto converted to SerializableSprite
        this.type = type;
        this.sprite = sprite;
    }

    public override string ToString()
    {
        return "Points: " + points + ", Health: " + health;
    }
}