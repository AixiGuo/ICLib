using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace ICLib.ICEditor
{

    public class ICEditorFunc
    {
        //得到inspector的范围
        public static Rect GetInspectorRect()
        {
#if UNITY_EDITOR

            UnityEditor.EditorGUI.indentLevel = 0;      //https://docs.unity3d.com/ScriptReference/EditorGUI-indentLevel.html
                                                        //左侧缩进=0
            return UnityEditor.EditorGUILayout.GetControlRect(GUILayout.Height(0));
#else
			return new Rect();
#endif
        }


        //设置inspector的范围
        public static void SetInspectorRect(Rect rect)
        {
            if (Event.current.type == EventType.Layout) GUILayoutUtility.GetRect(1, rect.height, "TextField");
        }

        #region Styles
        public static GUIStyle buttonStyle = null; 
        public static GUIStyle labelStyle = null; 
        public static GUIStyle boldStyle = null; 
        public static GUIStyle boxStyle = null; 
        public static GUIStyle InputTextStyle = null; 
        public static void CheckStyles(bool fouce=false)
        {
#if UNITY_EDITOR
            if (buttonStyle == null &&!fouce)
            {
                buttonStyle = new GUIStyle("Button");
                boxStyle = new GUIStyle(GUI.skin.box);
                labelStyle = new GUIStyle(UnityEditor.EditorStyles.label); labelStyle.active.textColor = Color.black; labelStyle.focused.textColor = labelStyle.active.textColor = labelStyle.normal.textColor;
                boldStyle = new GUIStyle(UnityEditor.EditorStyles.label); boldStyle.fontStyle = FontStyle.Bold;
                InputTextStyle = new GUIStyle(UnityEditor.EditorStyles.numberField);

            }
#endif
        }


        #endregion

        #region ICON
        public static Dictionary<string, Texture2D> icons = new Dictionary<string, Texture2D>();
        public static Texture2D GetIcon(string textureName)
        { 
            Texture2D texture = null;
            if (!icons.ContainsKey(textureName))
            {
                texture = Resources.Load(textureName) as Texture2D;

                icons.Add(textureName, texture);
            }
            else texture = icons[textureName];
            return texture;
        }

        #endregion

        #region Extensions
       

        #endregion
    }
}