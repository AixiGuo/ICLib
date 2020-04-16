using ICMagic;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ICLib.ICEditor
{ 
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
                layout.Button(new zOneline(), one.Key);
                var field = one.Value;
                var attrs = field.attributes;
                foreach(var attr in attrs)
                {
                    layout.Label(new zOneline(), attr.GetType().ToString());
                }
            }
            layout.FinishInspector();
            // base.OnInspectorGUI();
        }
    }

}