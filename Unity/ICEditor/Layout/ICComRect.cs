
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ICLib.ICEditor
{

    //确定元素位置rect
    public class ICComRect
    {
        public float height, width;
        public ICLayout layout;
        public bool nextLine = true;
        public bool nextCol = true; 
        public Rect offset = new Rect(0,0,0,0);
        public Rect lastRect;

        public Rect GetRect()
        {
            var rect = CalculateRect();
            rect.position = rect.position + offset.position;
            rect.width = rect.width + offset.width;
            rect.height = rect.height+offset.height;
            lastRect = new Rect(rect);
            return rect;
        }
        public Rect GetRect(float padding)
        {
            var rect = GetRect(); 
            rect.position = rect.position + offset.position;
            rect.width = rect.width + offset.width;
            rect.height = rect.height + offset.height;

            //padding
            rect.x += padding;
            rect.y += padding;
            rect.width -= padding * 2;
            rect.height -= padding * 2;

            lastRect = new Rect(rect);
            return rect;
        }

        public virtual Rect CalculateRect()
        {
            return new Rect();
        }
        

        //传入当前环境
        public void Init(ICLayout layout)
        {
            this.layout = layout;
        }

        //如果超出范围，更新区域
        public void Update()
        {
            if (layout.cursor.y + layout.cursor.height > layout.field.height)
            {
                layout.field.height = layout.cursor.y + layout.cursor.height;
            }

            if (layout.cursor.x + layout.cursor.width > layout.field.width)
            {
                layout.field.width = layout.cursor.x + layout.cursor.width;
            }
        }

        public void UpdateAndMoveDown(float height)
        {
            if (nextLine)
            {
                layout.cursor.y += height;
                Update();
            }
            else
            {
                if (layout.cursor.y + layout.cursor.height + height > layout.field.height)
                {
                    layout.field.height = layout.cursor.y + layout.cursor.height + height;
                }
            }
        }
        public void UpdateAndMoveRight(float width)
        {
            if (nextCol)
            {
                layout.cursor.x += width;
                Update();
            }
            else
            {
                if (layout.cursor.x + layout.cursor.width + width > layout.field.width)
                {
                    layout.field.width = layout.cursor.y + layout.cursor.width + width;
                }
            }
        }
     
    }


    public class ICAOffset : Attribute
    {
        public Rect offset;
        public ICAOffset(int x, int y, int width, int height)
        {
            offset = new Rect(x, y, width, height);
        }
    }

    //从右向左
    public class zToRight : ICComRect
    {  
        public bool firstOne;
        public zToRight(float height, float width,
           bool firstOne = false,
           bool nextLine = false,
           bool nextCol = true)
        {
            this.firstOne = firstOne;
            this.height = height;
            this.width = width;
            this.nextLine = nextLine;
            this.nextCol = nextCol;
        }
        public override Rect CalculateRect()
        {
            if (firstOne)
            {
                layout.cursor.x = layout.field.width   - width ;
            }else if (nextCol)
            {
                layout.cursor.x -= width;
            }
            Rect rect = new Rect(layout.cursor.x + layout.field.x,
                              layout.cursor.y + layout.field.y,
                              width,
                              height);
           
            UpdateAndMoveDown(height);

            return rect;
        }
    }
    public class ICAToRight : Attribute
    {
        public zToRight pos;
        public ICAToRight(float height, float width,
           bool firstOne = false,
           bool nextLine = false,
           bool nextCol = true)
        {
            pos = new zToRight(height, width, firstOne, nextLine, nextCol);
        }
    }


    //向右排布
    public class zToLeft : ICComRect
    { 
        public bool firstOne = false;
        public zToLeft(float height, float width,
            bool firstOne = false,
            bool nextLine = false,
            bool nextCol = true)
        {
            this.firstOne = firstOne;
            this.height = height;
            this.width = width;
            this.nextLine = nextLine;
            this.nextCol = nextCol;
        }

        public override Rect CalculateRect()
        {
            if (firstOne)
            {
                layout.cursor.x = 0;
            }
            Rect rect = new Rect(layout.cursor.x + layout.field.x,
                              layout.cursor.y + layout.field.y,
                              width,
                              height);
            UpdateAndMoveRight(width);
            UpdateAndMoveDown(height);

            return rect;
        }
    }

    public class ICAToLeft : Attribute
    {
        public zToLeft pos;
        public ICAToLeft(float height, float width,
            bool firstOne = false,
            bool nextLine = false,
            bool nextCol = true)
        {
            pos = new zToLeft(height, width, firstOne, nextLine, nextCol);
        }
    }

    //一行的布局
    public class zOneline : ICComRect
    { 
        public zOneline(float height = 20, bool nextLine = true)
        {
            this.height = height;
            this.nextLine = nextLine;
        }

        public override Rect CalculateRect()
        {
            layout.cursor.x = 0;

            Rect rect = new Rect(layout.cursor.x + layout.field.x,
                                layout.cursor.y + layout.field.y,
                                layout.field.width,
                                height);

            UpdateAndMoveDown(height);

            return rect;

        }
    }
    public class ICAOneLine : Attribute
    {
        public zOneline pos;
        public ICAOneLine(float height = 20, bool nextLine = true)
        {
            pos = new zOneline(height, nextLine);
        }
    }

    //一行分割  最后一个应该让 finish 为true
    public class zSpeOneline : zOneline
    {
        public float columes = 0;
        public float colAt = 0;

        public zSpeOneline(float colAt, float columes, bool nextLine = false, float height = 17)
            : base(height, nextLine)
        {
            this.colAt = colAt;
            this.columes = columes;
        }

        public override Rect CalculateRect()
        {
            layout.cursor.x = 0;
            var oneWidth = layout.field.width / columes;

            Rect rect = new Rect(layout.cursor.x + layout.field.x + oneWidth * colAt,
                                layout.cursor.y + layout.field.y,
                                oneWidth,
                                height);

            UpdateAndMoveDown(height);
            return rect;
        }
    } 
    public class ICASpeOneline : Attribute
    {
        public zSpeOneline pos;
        public ICASpeOneline(float colAt, float columes, bool nextLine = false, float height = 17)
        {
            pos = new zSpeOneline(colAt, columes, nextLine, height);
        }
    }

    //全屏，用于Window的背景
    public class zFullWindow : ICComRect
    {
#if UNITY_EDITOR
        public UnityEditor.EditorWindow win;
        public zFullWindow(UnityEditor.EditorWindow window)
        {
            win = window;
        }
        public override Rect CalculateRect()
        {
            Vector2 windowZeroPos = layout.ToInternal(Vector2.zero);
            windowZeroPos.x = ((int)(windowZeroPos.x / 64f)) * 64;
            windowZeroPos.y = ((int)(windowZeroPos.y / 64f)) * 64;
            Rect position = win.position;
            Rect backRect = new Rect(windowZeroPos - new Vector2(64, 64), position.size + new Vector2(127, 127));
            return backRect;
        }
#endif
    }

    //全部，用于组件背景
    public class zFullCompoment: ICComRect
    {
        public override Rect CalculateRect()
        {
            return layout.field;
        }
    }

    public class ICAFullCompoment : Attribute
    {
        public zFullCompoment pos;
        public ICAFullCompoment()
        {
            pos = new zFullCompoment();
        }
    }
}