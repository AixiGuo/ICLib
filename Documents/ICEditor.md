[<<back](../Readme.md)
# ICEditor

这是一个类似于Odin的插件，只要简单的为成员加上修饰，就可以完成对Inspector 的自定义绘图，方便开发。
 
## 使用方法

1. 首先创建一个用户自定义的Editor类，比如，现在有一个
``` csharp
public class test : MonoBehaviour
{
    public float number = 10;
} 
```

创建一个 Editor 文件夹，在里面创建 testInspector.cs
``` csharp
[CustomEditor(typeof(test))]
public class testInsp : ICEditor
{

}

```


## 类

* ICEditor
绘制的主类