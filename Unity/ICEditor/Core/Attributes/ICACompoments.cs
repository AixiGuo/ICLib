using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ICLib.ICEditor
{

    public class ICAFloatField : Attribute
    {
        public string label = "", tooltip = "";
        public ICAFloatField(string label = "", string tooltip = "")
        {
            this.label = label;
            this.tooltip = tooltip;
        }

        public void OnEditorGUI(ICField field, ICLayout layout, ICComRect pos, SerializedObject serializedObject)
        {
            float value = field.GetVariable<float>();
            string name = (label != "") ? label : field.name;
            layout.FloatField(ref value, pos, name, tooltip);
            serializedObject.FindProperty(field.name).floatValue = value;
        }
    }

    public class ICAIntField : Attribute
    {
        public string label = "", tooltip = "";
        public ICAIntField(string label = "", string tooltip = "")
        {
            this.label = label;
            this.tooltip = tooltip;
        }

        public void OnEditorGUI(ICField field, ICLayout layout, ICComRect pos, SerializedObject serializedObject)
        {
            int value = field.GetVariable<int>();
            string name = (label != "") ? label : field.name;
            layout.IntField(ref value, pos, name, tooltip);
            serializedObject.FindProperty(field.name).intValue = value;
        }
    }


}