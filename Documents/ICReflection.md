[<<back](../Readme.md)
# ICReflection.cs

## Classes

* ICObjectInfo

Collect most of functions for class
``` csharp
////////////////Example:////////////////
MyClass obj = new MyClass();

ICObjectInfo objInfo = new ICObjectInfo(obj);
foreach(var one in objInfo.fields)
{
    Debug.Log(one);
}
Debug.Log(objInfo.GetVariable<float>("test"));
objInfo.SetVariable("test", 10f);
Debug.Log(objInfo.GetVariable<float>("test"));
foreach(var one in objInfo.GetAttributes("test"))
{
    MyAttribute attr = one as MyAttribute;
    if(attr!=null)Debug.Log(attr.value);
} 

objInfo.InvokeMethod("FUNCTION", null);

...

public class MyAttribute : Attribute
{
    public int value;
    public MyAttribute(int value)
    {
        this.value = value;
    }
}
public class MyClass
{
    [MyAttribute(99999999)]
    public float test = 33.65f;
    [MyAttribute(20)]
    public float test2 = 44;

    public void FUNCTION()
    {
        Debug.Log("This is function");
    }
}

```


## Extensions
1. Get list of all public members of class
``` csharp
 public static List<MemberInfo> SubMemebers(this Type parent)
 
////////////////Example:////////////////
var members = typeof(MyClass).SubMemebers();
for (int k = 0; k < members.Count; k++)
{
    var one = members[k];
    Debug.Log(members[k].Name);
}
```

2. Get list of inherited subclass 

```csharp
public static List<Type> Subtypes(this Type parent)
////////////////Example:////////////////

var subClasses = typeof(MyClass).Subtypes();
for(int i = 0; i < subClasses.Count; i++)
{
    var one = subClasses[i];
    Debug.Log(one.Name);
}
...

public class MyClass
{ 
    public float test = 0; 
}

public class subClass : MyClass
{
    public int ddd = 2;
}

```

3. Get list of Attributes
``` csharp
 public static List<T> Attributes<T>(this MemberInfo mem,Type findAttr=null) where T:Attribute
////////////////Example:////////////////

var members = typeof(MyClass).SubMemebers();
for (int k = 0; k < members.Count; k++)
{
    var one = members[k];
    Debug.Log(members[k].Name);

    var attrs = one.Attributes<MyAttribute>();
    for (int a = 0; a < attrs.Count; a++)
    {
        var att = attrs[a];
        Debug.Log(att.value);
    }
}

public class MyAttribute : Attribute
{
    public int value;
    public MyAttribute(int value)
    {
        this.value = value;
    }
}
public class MyClass
{
    [MyAttribute(10)]
    public float test = 0;
    [MyAttribute(20)]
    public float test2 = 0;
}

public class subClass : MyClass
{
    public int ddd = 2;
}
```

4. Get/Set field value
``` csharp
public static ICObjectField GetObjectField(this object target,string fieldName)
////////////////Example:////////////////

 MyClass obj = new MyClass();
ICObjectField field = obj.GetObjectField("test");

var value = field.GetFieldValue<float>();
Debug.Log(value);

field.SetFieldValue(15.3f);
value = field.GetFieldValue<float>();
Debug.Log(value);

public class MyClass
{ 
    public float test = 33.65f; 
    public float test2 = 44;
}
```