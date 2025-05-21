using MAEngine.Extention;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace MAEngine.Editor
{
    [CustomPropertyDrawer(typeof(DictionaryElement<,>))]
    public class DictionaryElementDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();
            
            var row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            row.style.justifyContent = Justify.SpaceBetween;
        
            var keyProperty = property.FindPropertyRelative("_key");
            var valueProperty = property.FindPropertyRelative("_value");
            
            var keyField = new PropertyField(keyProperty, property.displayName);
            var valueField = new PropertyField(valueProperty, "Value");
            
            keyField.style.flexGrow = 1;
            keyField.style.marginRight = 5;
            valueField.style.flexGrow = 1;

            row.Add(keyField);
            row.Add(valueField);
            container.Add(row);

            return container;
            
        }
    }
}

