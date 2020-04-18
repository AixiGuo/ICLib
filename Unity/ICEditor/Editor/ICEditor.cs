 
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ICLib.ICEditor
{ 
    [System.Serializable]
    [CustomEditor(typeof(ICMonoBehaviour),editorForChildClasses:true)]
    public class ICEditor : Editor
    {  
        private ICObjectInfo targetInfo;
       
        private ICLayout layout;
        public void OnEnable()
        {
            targetInfo = new ICObjectInfo(target); 
        }
        public override void OnInspectorGUI()
        {
            if (layout == null)
            {
                layout = new ICLayout();
            }
            layout.InitInspector();
            foreach (var one in targetInfo.fields)
            {
                ICField field = one.Value;
                var attrs = field.attributes;
                ICComRect pos=new zOneline();
                foreach(var attr in attrs)
                {
                    if (attr is ICAOneLine) { var at = attr as ICAOneLine; pos = at.pos; continue; }
                    if (attr is ICASpeOneline) { var at = attr as ICASpeOneline; pos = at.pos; continue; }
                    if (attr is ICAToLeft) { var at = attr as ICAToLeft; pos = at.pos; continue; }
                    if (attr is ICAToRight) { var at = attr as ICAToRight; pos = at.pos; continue; }
                    if (attr is ICAFullCompoment) { var at = attr as ICAFullCompoment; pos = at.pos; continue; }

                    if (attr is ICAOffset){var at = attr as ICAOffset;pos.offset = at.offset;continue;}
                    if(attr is ICAFloatField){var at = attr as ICAFloatField; at.OnEditorGUI(field,layout,pos,serializedObject);continue;}
                    if(attr is ICAIntField){var at = attr as ICAIntField; at.OnEditorGUI(field,layout,pos,serializedObject);continue;}

                }
                serializedObject.ApplyModifiedProperties();
            }
            layout.FinishInspector();
           
        }
    }

}