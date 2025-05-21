using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Orders
{
    [CustomPropertyDrawer(typeof(OrderText))]
    public class OrderTextDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();
            
            var textProperty = property.FindPropertyRelative("_text");
            var descriptionProperty = property.FindPropertyRelative("_description");

            var textField = new TextField("Text")
            {
                multiline = true
            };
            textField.BindProperty(textProperty);
            textField.style.minHeight = 60;

            var descriptionField = new TextField("Description")
            {
                multiline = true
            };
            descriptionField.BindProperty(descriptionProperty);
            descriptionField.style.minHeight = 60;

            container.Add(textField);
            container.Add(descriptionField);

            return container;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 8;
        }
    }
}

