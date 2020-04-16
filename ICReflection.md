
# ICReflection.cs

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
 public static List<Attribute> Attributes(this MemberInfo mem,Type findAttr=null)
////////////////Example:////////////////

var members = typeof(MyClass).SubMemebers();
for (int k = 0; k < members.Count; k++)
{
    var one = members[k];
    Debug.Log(members[k].Name);

    var attrs = one.Attributes();
    for(int a = 0; a < attrs.Count; a++)
    {
        var att = attrs[a] as MyAttribute;
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

4. Set field value
``` csharp
public static T GetFieldValue<T>(this object target, string fieldName )
////////////////Example:////////////////

MyClass obj = new MyClass();
var value = obj.GetFieldValue<float>("test");
Debug.Log(value);

public class MyClass
{ 
    public float test = 33.65f; 
    public float test2 = 44;
}
```