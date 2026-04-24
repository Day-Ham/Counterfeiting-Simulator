using UnityEngine;

[CreateAssetMenu(fileName = "New Bool", menuName = "Value Wrapper/Single/PersistentBoolValue")]
public class PersistentBoolValue : ValueWrapper<bool>
{
    [Header("Save Settings")]
    [SerializeField] private string saveKey;

    protected override void OnEnable()
    {
        base.OnEnable();
        Value = ES3.Load(saveKey, GetRawValue(), new ES3Settings(SaveFileUtility.SAVE_FILE_NAME));
        OnValueChanged += _ => ES3.Save(saveKey, Value, new ES3Settings(SaveFileUtility.SAVE_FILE_NAME));
    }
}