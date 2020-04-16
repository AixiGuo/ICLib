[<<back](../Readme.md)
# ICLayout

方便的用户自定义版图

## 用于Inspector


``` csharp
    public class ICEditor : Editor
    { 
        private ICLayout layout;
        
        public override void OnInspectorGUI()
        {
            if (layout == null)
            {
                layout = new ICLayout();
            }
            layout.InitInspector();
            
            //Add details

            layout.FinishInspector();
           
        }
    }
```

## 绘制

绘制方法首先是调用IClayout其中的方法，比如
``` csharp
layout.label(...)
```

大部分的绘制都需要一个确定位置的对象：
* zOneline  组件占一行
* zSpeOneline  组件占一行的几分之几
* zFullWindow   全屏，用于Window的背景
* zToLeft   从最左边开始向右排布
* zToRight  从最右边开始向左排布

具体例子：
``` csharp
layout.Button(new zOneline(), "Hello");
```

所有的排布可以额外加偏移，比如
``` csharp
layout.Button(new zOneline(){ offset = new Rect(0, 0, 0, 0) }, "Hello");
```
