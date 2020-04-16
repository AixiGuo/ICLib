using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 反射
/// 
/// ===========================
/// 最新修正  
/// 2020-4-16  初始版本
/// 
/// ===========================
/// 使用例子
///  List<Type> Subtypes(this Type parent)   
///  得到继承于基类的子类列表:
///  var subClasses = typeof(MyClass).Subtypes();
///  
/// 
/// 
/// </summary>

namespace ICLib
{

    static public class ICReflect 
    { 
        //得到类的所有共有members
        public static List<MemberInfo> SubMemebers(this Type parent)
        {
            BindingFlags bindingAttr = BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static|BindingFlags.FlattenHierarchy /*| BindingFlags.NonPublic*/;
            List<MemberInfo> results = new List<MemberInfo>();
            foreach (var member in parent.GetMembers(bindingAttr))
            {

            }
        }

        //得到继承于基类的子类列表
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