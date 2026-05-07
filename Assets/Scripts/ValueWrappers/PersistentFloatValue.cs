using UnityEngine;

[CreateAssetMenu(fileName = "New Persistent Float", menuName = "Value Wrapper/Single/PersistentFloatValue")]
public class PersistentFloatValue : FloatValue
{
    [SerializeField] private string saveKey;

    protected override void OnEnable()
    {
        base.OnEnable();
        var settings = new ES3Settings(SaveFileUtility.SAVE_FILE_NAME);
        Value = ES3.Load(saveKey, GetRawValue(), settings);
    }

    public void Save()
    {
        var settings = new ES3Settings(SaveFileUtility.SAVE_FILE_NAME);
        ES3.Save(saveKey, Value, settings);     
    }
}
