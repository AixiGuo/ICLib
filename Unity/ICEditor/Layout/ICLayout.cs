using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using System;
using System.Dynamic;

namespace ICMagic
{
	[System.Serializable]
    public class ICLayout
    { 
        public int margin = 0;
        public Rect field;  
		public Rect cursor=new Rect();

		private ICComRect defaultPos = new ICComRect();
		 
		//将指针向下移动
		public void Space(float height=10)
		{ 
			 
		}

		#region Init

		public void InitInspector(int margin=0)
		{
			cursor = new Rect();
			this.margin = margin;
			field = ICEditorFunc.GetInspectorRect();
		}

		#endregion

		#region Compoments
		private Rect com_rect;
		private GUIStyle com_style;
		private GUIContent com_content;

		private void Com_Init(ICComRect position,
			string label = "", string tooltip = "", GUIStyle style = null,
			GUIStyle defaultStyle = null)
		{
			position.Init(this);
			com_rect = ToDisplay( position.GetRect(margin));   //确定位置

			Style_Init(style, defaultStyle);

			com_content = new GUIContent(label, tooltip);
		}

		private void Style_Init(GUIStyle style = null,
			GUIStyle defaultStyle = null)
		{
			if (style != null) com_style = new GUIStyle(style);
			else if (defaultStyle != null) com_style = new GUIStyle(defaultStyle);
			else com_style = new GUIStyle();

			var worldHeight = com_rect.height * (0.8f / zoom);
			com_style.fontSize = Mathf.RoundToInt(worldHeight * zoom);
		}

		//空元素，用于新的定位
		public void Empty(ICComRect position)
		{
			Com_Init(position, "", "", null, null); 
		} 

		//Toggle样式的button
		public void CheckButton(ref bool value, ICComRect position,
			string label = "", string tooltip = "",GUIStyle style=null)
		{
			Com_Init(position, label, tooltip, style,ICEditorFunc.buttonStyle);

			value = GUI.Toggle(com_rect, value, com_content, com_style); 
		}

		//button
		public bool Button(ICComRect position,
			string label = "", string tooltip = "", GUIStyle style = null)
		{
			Com_Init(position, label, tooltip, style, ICEditorFunc.buttonStyle);

			return GUI.Button(com_rect, com_content, com_style);
		}

		//Label
		public void Label(ICComRect position,
			string label , string tooltip = "", GUIStyle style = null,
			TextAnchor textAnchor = TextAnchor.UpperLeft)
		{
			Com_Init(position, label, tooltip, style, ICEditorFunc.labelStyle);
			com_style.alignment = textAnchor;
			GUI.Label(com_rect, com_content, com_style);
		}

		//box
		public void Box(ICComRect position,
			string label = "", 
			string tooltip = "", GUIStyle style = null
			)
		{
			Com_Init(position, label, tooltip, style, ICEditorFunc.boxStyle);
			 
			GUI.Box(com_rect, com_content, com_style);
		}

		//ColorBox
		public void ColorBox(ICComRect position,Color color)
		{
			Com_Init(position, "", "", null, ICEditorFunc.labelStyle);
			com_style.normal.background = Texture2D.whiteTexture;
			var back = GUI.backgroundColor;
			GUI.backgroundColor = color;
			GUI.Box(com_rect, com_content,com_style );

			GUI.backgroundColor = back;
		}



		//Image 
		public enum IconAligment { resize, min, max, center }

		public void Icon(ICComRect position, string texture,
			 IconAligment horizontalAlign = IconAligment.resize,
			 IconAligment verticalAlign = IconAligment.resize,
			 bool tile = false)
		{
			var text = ICEditorFunc.GetIcon(texture);
			Icon(position, text, horizontalAlign, verticalAlign, tile);
		}

		public void Icon(ICComRect position,Texture2D texture,
			 IconAligment horizontalAlign = IconAligment.resize, 
			 IconAligment verticalAlign = IconAligment.resize ,
			 bool tile = false)
		{
#if UNITY_EDITOR
			if (texture == null) return;

			position.Init(this); 
			var rect = position.GetRect(margin);

			//aligning texture if the rect width or height is more than icon size
			if (rect.width > texture.width)
			{
				switch (horizontalAlign)
				{
					case IconAligment.min: rect.width = texture.width; break;
					case IconAligment.center: rect.x += rect.width / 2; rect.x -= texture.width / 2; rect.width = texture.width; break;
					case IconAligment.max: rect.x += rect.width; rect.x -= texture.width; rect.width = texture.width; break;
				}
			}
			if (rect.height > texture.height)
			{
				switch (verticalAlign)
				{
					case IconAligment.min: rect.height = texture.height; break;
					case IconAligment.center: rect.y += rect.height / 2; rect.y -= texture.height / 2; rect.height = texture.height; break;
					case IconAligment.max: rect.y += rect.height; rect.y -= texture.height; rect.height = texture.height; break;
				}
			}
			if (!tile)
			{
				 GUI.DrawTexture(ToDisplay(rect), texture, ScaleMode.ScaleAndCrop);
 
			}
			else
			{
				//Debug.Log(zoom);
				Rect localRect = ToDisplay(rect);
				for (float x = 0; x < rect.width; x += texture.width * zoom)
					for (float y = 0; y < rect.height+texture.height; y += texture.height * zoom)
					{
					 GUI.DrawTexture(new Rect(x + localRect.x, y + localRect.y, texture.width * zoom+1, texture.height * zoom+1), texture, ScaleMode.StretchToFill);
 
					}
			}
#endif
		}

		//Element 
		public void Element(string textureName,ICComRect position, RectOffset borders, RectOffset offset)
		{

			if (Event.current.type != EventType.Repaint) return;

			position.Init(this);
			Rect rect = position.GetRect();

			GUIStyle elementStyle = new GUIStyle();
			elementStyle.normal.background = ICEditorFunc.GetIcon(textureName);
			elementStyle.hover.background = ICEditorFunc.GetIcon(textureName);

			if (borders != null)
				elementStyle.border = borders;
			Rect paddedRect = ToDisplay(rect);
			if (offset != null)
				paddedRect = new Rect(paddedRect.x - offset.left, paddedRect.y - offset.top, paddedRect.width + offset.left + offset.right, paddedRect.height + offset.top + offset.bottom);

#if UNITY_EDITOR
			elementStyle.Draw(paddedRect, UnityEditor.EditorGUIUtility.isProSkin, false, false, false);
#endif
		}



		//Float Input
		public void FloatField(ref float value, ICComRect position,
			string label = "", string tooltip = "Float Value", GUIStyle style = null)
		{
			Icon(new zSpeOneline(1, 4, false, position.height) { offset = new Rect(2,1,0,0)}
			, "ICMagic_Slider",IconAligment.max,IconAligment.center);

			Com_Init(position, label, tooltip, style, ICEditorFunc.InputTextStyle);
#if UNITY_EDITOR
			UnityEditor.EditorGUIUtility.labelWidth= com_rect.width/2 ;
			 value = UnityEditor.EditorGUI. FloatField(com_rect," ", value, com_style);
			UnityEditor.EditorGUIUtility.labelWidth = 0;
			

			Style_Init(ICEditorFunc.labelStyle);  
			GUI.Label(com_rect, com_content, com_style);
			 
#endif
		}

		public void Vector2Field(ref Vector2 value, ICComRect position,
			string label = "", string tooltip = "Vector2 Value" )
		{
			Com_Init(position, label, tooltip, null, ICEditorFunc.InputTextStyle);
			Rect LabelRect = com_rect.SplitRectHorizontal(0, 3);
			Rect XRect = com_rect.SplitRectHorizontal(1, 3);
			Rect YRect = com_rect.SplitRectHorizontal(2, 3);

#if UNITY_EDITOR
			Style_Init(ICEditorFunc.labelStyle);
			GUI.Label(LabelRect, com_content, com_style);
			Style_Init(ICEditorFunc.InputTextStyle);
			UnityEditor.EditorGUIUtility.labelWidth = 3;
			value.x = UnityEditor.EditorGUI.FloatField(XRect, " ", value.x, com_style);
			value.y = UnityEditor.EditorGUI.FloatField(YRect, " ", value.y, com_style);
			UnityEditor.EditorGUIUtility.labelWidth = 0;
#endif

			//value = UnityEditor.EditorGUI.Vector2Field(com_rect, "ss", value);
		}

		System.Type curveWindowType;
		AnimationCurve windowCurveRef = null;
		public void Curve(ref AnimationCurve src, ICComRect position,
				string label = "", string tooltip = "Float Value", GUIStyle style = null)
		{
			Com_Init(position, label, tooltip, null, ICEditorFunc.InputTextStyle);

#if UNITY_EDITOR
			//recording undo on change if the curve editor window is opened (and this current curve is selected)
			if (curveWindowType == null) curveWindowType = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.CurveEditorWindow");
			if (UnityEditor.EditorWindow.focusedWindow != null && UnityEditor.EditorWindow.focusedWindow.GetType() == curveWindowType)
			{
				AnimationCurve windowCurve = curveWindowType.GetProperty("curve").GetValue(UnityEditor.EditorWindow.focusedWindow, null) as AnimationCurve;
				if (windowCurve == src)
				{
					if (windowCurveRef == null) windowCurveRef = windowCurve.Copy();
					if (!windowCurve.IdenticalTo(windowCurveRef))
					{

						Keyframe[] tempKeys = windowCurve.keys;
						windowCurve.keys = windowCurveRef.keys;
						//SetChange(true);
						//SetChange(true);

						windowCurve.keys = tempKeys;

						windowCurveRef = windowCurve.Copy();
					}
				}
			}else windowCurveRef = null;

			UnityEditor.EditorGUI.CurveField( com_rect , src, Color.white,new Rect(0,0,1,1));


#endif
		}
		#endregion


		#region Zoom Scroll
		public Vector2 scroll = new Vector2(0, 0);
		public float zoom = 1;
		public float zoomStep = 0.0625f;
		public float minZoom = 0.25f;
		public float maxZoom = 1f;
		  
		public void Zoom()
		{
			if (Event.current == null) return;

			//reading control
			bool control = Event.current.control;
			 
			float delta = 0;
			if (Event.current.type == EventType.ScrollWheel) delta = Event.current.delta.y / 3f;
			//else if (Event.current.type == EventType.MouseDrag && Event.current.button == 0 && control)
			//	delta = Event.current.delta.y / 15f;
			 
			if (Mathf.Abs(delta) < 0.001f) return;

			float zoomChange = -zoom * zoomStep * delta; //progressive step
			 
			//clamping zoom change so it will never be out of range
			if (zoom + zoomChange > maxZoom) zoomChange = maxZoom - zoom;
			if (zoom + zoomChange < minZoom) zoomChange = minZoom - zoom;

			//record mouse position in worldspace
			Vector2 worldMousePos = (Event.current.mousePosition - scroll) / zoom;

			//changing zoom
			zoom += zoomChange;

			if (zoom >= minZoom && zoom <= maxZoom) scroll -= worldMousePos * zoomChange;
			//zoom = Mathf.Clamp(zoom, minZoom, maxZoom); //returning on out-of-range instead

#if UNITY_EDITOR
			if (UnityEditor.EditorWindow.focusedWindow != null) UnityEditor.EditorWindow.focusedWindow.Repaint();
#endif
		}

		public void Scroll()
		{
			if (Event.current == null || Event.current.type != EventType.MouseDrag) return;
			if (!(Event.current.button == 2 || (Event.current.button == 0 && Event.current.alt))) return;

			scroll += Event.current.delta;

#if UNITY_EDITOR
			if (UnityEditor.EditorWindow.focusedWindow != null)
				UnityEditor.EditorWindow.focusedWindow.Repaint();
#endif
		}

		public Rect ToDisplay(Rect rect)
		{ return new Rect(rect.x * zoom + scroll.x, rect.y * zoom + scroll.y, rect.width * zoom, rect.height * zoom); }
		public float ToDisplay(float value)
		{
			return value * zoom;
		}

		public Vector2 ToInternal(Vector2 pos) { return (pos - scroll) / zoom; } //return new Vector2( (pos.x-scroll.x)/zoom, (pos.y-scroll.y)/zoom); }
		public Vector2 ToDisplay(Vector2 pos) { return pos * zoom + scroll; }


		#endregion

		#region DragAndDrop
		public enum DragState { Pressed, Drag, Released }
		public DragState dragState;
		public Vector2 dragOffset;
		public Vector2 dragTo;
		  
		public bool DragDrop()
		{
			return DragDrop(field);
		}

		public bool DragDrop(Rect initialRect)
		{
			Vector2 mousePos = ToInternal(Event.current.mousePosition);

			//点击
			if(Event.current.type == EventType.MouseDown && Event.current.button == 0)
			{
				if (initialRect.Contains(mousePos))
				{
					dragOffset = mousePos- initialRect.position;
					dragState = DragState.Pressed;
					dragTo = initialRect.position;
					return true;
				}
			}

			if (Event.current.rawType == EventType.MouseUp)
			{
				dragState = DragState.Released;
				return false;
			}

			if (Event.current.type == EventType.MouseDrag && 
				(dragState == DragState.Pressed || dragState == DragState.Drag))
			{
				dragState = DragState.Drag;
				 
#if UNITY_EDITOR
				if (Event.current.isMouse) Event.current.Use();
				if (UnityEditor.EditorWindow.focusedWindow != null) UnityEditor.EditorWindow.focusedWindow.Repaint();
#endif
				dragTo = mousePos - dragOffset;
			 

				if (!Event.current.shift)
				{ 
					var x = dragTo.x;
					var y = dragTo.y;
					dragTo.x = ((int)(x / 10)) * 10;
					dragTo.y = ((int)(y / 10)) * 10;
					  
				}
				return true;
			} 

			return false;
			 
		}

		public void MoveTo(Vector2 pos)
		{
			field.position = pos; 
		}
		#endregion

		//结束绘制，把区域反应到inpector上
		public void FinishInspector()
		{
			ICEditorFunc.SetInspectorRect(this.field);
		}


	}

}