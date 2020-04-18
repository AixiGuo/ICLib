[<<back](../Readme.md)

# ICEditor

这是一个类似于Odin的插件，只要简单的为成员加上修饰，就可以完成对Inspector 的自定义绘图，方便开发。

## 使用方法

1. 继承 ICMonoBehaviour
2. 为属性加上修饰

``` csharp

public class test : ICMonoBehaviour
{  
    [ICAOffset(10,0,-10,0), ICAFloatField("1")]
    public float number = 10;

    [ICAFloatField]
    public float number2 = 10;
}

```

## 位置确定

详见 [ICLayout](/Documents/ICLayout.md)

* zOneline  组件占一行
* zSpeOneline  组件占一行的几分之几
* zFullWindow   全屏，用于Window的背景
* zToLeft   从最左边开始向右排布
* zToRight  从最右边开始向左排布

## 修饰方法

<details>
  <summary>ICAFloatField(string label = "", string tooltip = "")</summary>
 
### 绘制浮点数框
label: 要显示的标签，如果为空，显示变量名称
tooltip: tooltip
 
  
</details> 


<details>
  <summary>ICAIntField(string label = "", string tooltip = "")</summary>
  
### 绘制整数框
label: 要显示的标签，如果为空，显示变量名称
tooltip: tooltip
  
</details>  
