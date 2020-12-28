using GibFrame.SaveSystem;
using UnityEngine;
using UnityEngine.UI;

public class SaveSystem : MonoBehaviour
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private Text desc;
    [SerializeField] private Image image;

    private void Start()
    {
        // Calling LoadOrInitialize to initialize a new data object only if not already existing
        SaveManager.LoadOrInitialize(new ExampleData(0, 100, sprite.texture.GetType(), sprite), ExampleData.PATH);
        Debug.Log("Data initialized");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SaveObject obj = SaveManager.LoadPersistentData(ExampleData.PATH);
            // Checking if the data exists and can be retrieved
            if (obj != null)
            {
                // Casting the data to its type
                ExampleData data = obj.GetData<ExampleData>();
                desc.text = data.ToString();
                image.sprite = data.sprite;
            }
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            // Laoding
            SaveObject obj = SaveManager.LoadPersistentData(ExampleData.PATH);
            if (obj != null)
            {
                // Modifying
                ExampleData data = obj.GetData<ExampleData>();
                data.points++;
                data.health--;
                // Saving
                SaveManager.SavePersistentData(data, ExampleData.PATH);
                Debug.Log("Modifying...");
            }
        }
    }
}