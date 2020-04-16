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


    }

}