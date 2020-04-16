 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
  

namespace ICLib
{

    static public class ICReflect 
    { 
        //Get list of all public members of class   得到类的所有共有members
        public static List<MemberInfo> SubMemebers(this Type parent)
        {
            BindingFlags bindingAttr = BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static|BindingFlags.FlattenHierarchy /*| BindingFlags.NonPublic*/;
            List<MemberInfo> results = new List<MemberInfo>();
            foreach (var member in parent.GetMembers(bindingAttr))
            {
                results.Add(member as MemberInfo);
            }
            return results;
        }

        //Get list of inherited subclass 得到继承于基类的子类列表
        public static List<Type> Subtypes(this Type parent)
        {
            Assembly assembly = Assembly.GetAssembly(parent);  //Assembly[] assembleys = AppDomain.CurrentDomain.GetAssemblies();
            Type[] types = assembly.GetTypes();
            List<Type> result = new List<Type>();
            for (int t = 0; t < types.Length; t++)
            {
                Type type = types[t];
                if (type.IsSubclassOf(parent) && !type.IsInterface && !type.IsAbstract) result.Add(type);
            }
            return result;
        }

        //Get list of Attributes  得到修饰
        public static List<T> Attributes<T>(this MemberInfo mem,Type findAttr=null) where T:Attribute
        {
            List<T> result = new List<T>();
            Type find = typeof(T);
            if (findAttr != null)
            {
                find = findAttr;
            }
            foreach (var one in mem.GetCustomAttributes(find))
            {
                result.Add((T)(one) );
            }
            return result;
        }
         
       
        public static ICObjectField GetObjectField(this object target,string fieldName)
        {
            Type type = target.GetType();
            var field = type.GetField(fieldName);
            if (field == null)
            {
                throw new Exception("No field:" + fieldName);
            }
            else
            {
                ICObjectField of = new ICObjectField()
                {
                    objTarget = target,
                    objField = field
                };
                return of;
            }
        }  
    }
    public class ICObjectField
    {
        public object objTarget;
        public FieldInfo objField;

        public T GetFieldValue<T>()
        {
            var value = (T)Convert.ChangeType(objField.GetValue(objTarget), typeof(T));
            return value;
        }

        public void SetFieldValue(object value)
        {
            objField.SetValue(objTarget, value);
        }
    }

    #region Class Reflection

    public enum ICFieldType
    {
        Non,Variable,Method
    }

    public class ICField
    {
        public ICObjectInfo parent;
        public ICFieldType type;
        public string name;
        public FieldInfo fieldInfo;
        public object fieldValue;
        public MethodInfo methodInfo;
        public IEnumerable<Attribute> attributes;

        public ICField(ICObjectInfo parent)
        {
            this.parent = parent;
        }

        public void UpdataVariableValue()
        {
            if (type == ICFieldType.Variable)
            {
                fieldValue = fieldInfo.GetValue(parent.obj);
            }
        }
        public void SetVariableValue(object value)
        {
            if(type == ICFieldType.Variable)
            {
                fieldValue = value;
                fieldInfo.SetValue(parent.obj, value);
            }
        }

        public void MethodInvoke(object[] pars)
        {
            if (type == ICFieldType.Method)
            {
                if (methodInfo != null)
                {
                    methodInfo.Invoke(parent.obj, pars);
                }
            }
        }

        public override string ToString()
        {
            if (type == ICFieldType.Method)
                return name + ":" + type;
            if (type == ICFieldType.Variable)
            {
                var attrs = "";
                foreach(var one in attributes)
                {
                    attrs = attrs+ one.GetType().ToString()+"|";
                } 
                return name + ":" + type + ", " + fieldInfo.Name + "=" + fieldValue.ToString()+
                    "  |"+attrs ;
            }
            return "";
        }
    }

    public class ICObjectInfo
    {
        public object obj;
        public Type objType;
        public List<MemberInfo> members;
        public Dictionary<string, ICField> fields; 
         
        public ICObjectInfo(object target)
        {
            obj = target;
            objType = target.GetType();

            members = objType.SubMemebers();
            fields = new Dictionary<string, ICField>(); 
            for(int i = 0; i < members.Count; i++)
            {
                var name = members[i].Name;
                var attr = members[i].GetCustomAttributes(typeof(Attribute));
                var field = objType.GetField(name);
                if(field == null)
                {
                    var m = objType.GetMethod(name);
                    if(m != null)
                    {
                        //is method
                        ICField newField = new ICField(this)
                        {
                            name = name,
                            attributes = attr,
                            fieldInfo = field,
                            fieldValue = null,
                            methodInfo = m,
                            type = ICFieldType.Method
                        };
                        fields.Add(name, newField);
                    }
                }
                else
                {
                    //is variable
                    ICField newField = new ICField(this)
                    {
                        name = name,
                        attributes = attr,
                        fieldInfo = field,
                        fieldValue = null,
                        methodInfo = null,
                        type = ICFieldType.Variable
                    };
                    newField.UpdataVariableValue();
                    fields.Add(name, newField);
                }

            }
        }

        public T GetVariable<T>(string name)
        {
            ICField v;
            fields.TryGetValue(name, out v);
            if(v == null)
            {
                throw new Exception("No this variable");
            }
            else
            {
                v.UpdataVariableValue();
                return (T)Convert.ChangeType(v.fieldValue, typeof(T));
            }
        }
        public void SetVariable(string name , object value)
        {
            ICField v;
            fields.TryGetValue(name, out v);
            if (v == null)
            {
                throw new Exception("No this variable");
            }
            else
            {
                v.SetVariableValue(value);
            }
        }

        public void InvokeMethod(string name,object[] pars)
        {
            ICField v;
            fields.TryGetValue(name, out v);
            if (v == null)
            {
                throw new Exception("No this variable");
            }
            else
            {
                v.MethodInvoke(pars);
            }
        }

        public IEnumerable<Attribute> GetAttributes(string name)
        {
            ICField v;
            fields.TryGetValue(name, out v);
            if (v == null)
            {
                throw new Exception("No this variable");
            }
            else
            {
                return v.attributes;
            }
        }
        #endregion
    }
}