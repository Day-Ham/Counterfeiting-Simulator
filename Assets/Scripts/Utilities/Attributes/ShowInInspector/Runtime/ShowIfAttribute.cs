using UnityEngine;

namespace DaeHanKim.Utilities
{
    public class ShowIfAttribute : PropertyAttribute
    {
        public readonly bool AsEnabled;
        public readonly string[] Conditions;
        public readonly string EnumName;
        public readonly int EnumValueIndex;

        public ShowIfAttribute(bool asEnabled, params string[] conditions)
        {
            AsEnabled = asEnabled;
            Conditions = conditions;
        }

        public ShowIfAttribute(bool asEnabled, string enumName, object enumValue)
        {
            EnumName = enumName;
            EnumValueIndex = (int) enumValue;
        }
    }
}