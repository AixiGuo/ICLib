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
        public static List<Attribute> Attributes(this MemberInfo mem,Type findAttr=null)
        {
            List<Attribute> result = new List<Attribute>();
            Type find = typeof(Attribute);
            if (findAttr != null)
            {
                find = findAttr;
            }
            foreach (var one in mem.GetCustomAttributes(find))
            {
                result.Add(one);
            }
            return result;
        }

        public static T GetFieldValue<T>(this object target, string fieldName )
        {
            Type type = target.GetType();
            var field = type.GetField(fieldName);
            if(field == null)
            {
                throw new Exception("No field:" + fieldName);
            }
            else
            {
                var value = (T) Convert.ChangeType(field.GetValue(target),typeof(T)); 
                return value;
            }
        }

    }

}