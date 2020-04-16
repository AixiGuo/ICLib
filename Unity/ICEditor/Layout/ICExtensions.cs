using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ICMagic
{

 static public class ICExtensions 
{

		public static IEnumerable<Type> Subtypes(this Type parent)
		{
			Assembly assembly = Assembly.GetAssembly(parent);  //Assembly[] assembleys = AppDomain.CurrentDomain.GetAssemblies();
			Type[] types = assembly.GetTypes();
			for (int t = 0; t < types.Length; t++)
			{
				Type type = types[t];
				if (type.IsSubclassOf(parent) && !type.IsInterface && !type.IsAbstract) yield return type;
			}
		}

		 
			public static TValue GetAttributeValue<TAttribute, TValue>(
				this Type type,
				Func<TAttribute, TValue> valueSelector)
				where TAttribute : Attribute
			{
				var att = type.GetCustomAttributes(
					typeof(TAttribute), true
				).FirstOrDefault() as TAttribute;
				if (att != null)
				{
					return valueSelector(att);
				}
				return default(TValue);
			}
		 

		//水平分割矩形
		public static Rect SplitRectHorizontal(this Rect r,float i,float all)
		{
			var newRect = new Rect();
			var eachWidth = r.width / all;
			newRect.x = r.x+eachWidth * i;
			newRect.width = eachWidth;
			newRect.y = r.y;
			newRect.height = r.height;

			return newRect;
		}

		public static Keyframe[] Copy(this Keyframe[] src)
		{
			Keyframe[] dst = new Keyframe[src.Length];
			for (int k = 0; k < src.Length; k++)
			{
				dst[k].value = src[k].value;
				dst[k].time = src[k].time;
				dst[k].inTangent = src[k].inTangent;
				dst[k].outTangent = src[k].outTangent;
			}
			return dst;
		}

		public static AnimationCurve Copy(this AnimationCurve src)
		{
			AnimationCurve dst = new AnimationCurve();
			dst.keys = src.keys.Copy();
			return dst;
		}


		public static bool IdenticalTo(this AnimationCurve c1, AnimationCurve c2)
		{
			if (c1 == null || c2 == null) return false;
			if (c1.keys.Length != c2.keys.Length) return false;

			int numKeys = c1.keys.Length;
			for (int k = 0; k < numKeys; k++)
			{
				if (c1.keys[k].time != c2.keys[k].time ||
					c1.keys[k].value != c2.keys[k].value ||
					c1.keys[k].inTangent != c2.keys[k].inTangent ||
					c1.keys[k].outTangent != c2.keys[k].outTangent)
					return false;
			}

			return true;
		}
	}

}