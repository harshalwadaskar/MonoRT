//
// PropertyInfoTest.cs - NUnit Test Cases for PropertyInfo
//
// Author:
//	Gert Driesen (drieseng@users.sourceforge.net)
//
// (C) 2004-2007 Gert Driesen
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//


using System;
using System.Reflection;
using System.Runtime.InteropServices;

using NUnit.Framework;

namespace MonoTests.System.Reflection
{
	[TestFixture]
	public class PropertyInfoTest
	{
		[Test]
		public void GetAccessorsTest ()
		{
			Type type = typeof (TestClass);
			PropertyInfo property = type.GetProperty ("ReadOnlyProperty");

			MethodInfo [] methods = property.GetAccessors (true);
			Assert.AreEqual (1, methods.Length, "#A1");
			Assert.IsNotNull (methods [0], "#A2");
			Assert.AreEqual ("get_ReadOnlyProperty", methods [0].Name, "#A3");

			methods = property.GetAccessors (false);
			Assert.AreEqual (1, methods.Length, "#B1");
			Assert.IsNotNull (methods [0], "#B2");
			Assert.AreEqual ("get_ReadOnlyProperty", methods [0].Name, "#B3");

			property = typeof (Base).GetProperty ("P");

			methods = property.GetAccessors (true);
			Assert.AreEqual (2, methods.Length, "#C1");
			Assert.IsNotNull (methods [0], "#C2");
			Assert.IsNotNull (methods [1], "#C3");
			Assert.IsTrue (HasMethod (methods, "get_P"), "#C4");
			Assert.IsTrue (HasMethod (methods, "set_P"), "#C5");

			methods = property.GetAccessors (false);
			Assert.AreEqual (2, methods.Length, "#D1");
			Assert.IsNotNull (methods [0], "#D2");
			Assert.IsNotNull (methods [1], "#D3");
			Assert.IsTrue (HasMethod (methods, "get_P"), "#D4");
			Assert.IsTrue (HasMethod (methods, "set_P"), "#D5");

			methods = property.GetAccessors ();
			Assert.AreEqual (2, methods.Length, "#E1");
			Assert.IsNotNull (methods [0], "#E2");
			Assert.IsNotNull (methods [1], "#E3");
			Assert.IsTrue (HasMethod (methods, "get_P"), "#E4");
			Assert.IsTrue (HasMethod (methods, "set_P"), "#E5");

			property = typeof (TestClass).GetProperty ("Private",
				BindingFlags.NonPublic | BindingFlags.Instance);

			methods = property.GetAccessors (true);
			Assert.AreEqual (2, methods.Length, "#F1");
			Assert.IsNotNull (methods [0], "#F2");
			Assert.IsNotNull (methods [1], "#F3");
			Assert.IsTrue (HasMethod (methods, "get_Private"), "#F4");
			Assert.IsTrue (HasMethod (methods, "set_Private"), "#F5");

			methods = property.GetAccessors (false);
			Assert.AreEqual (0, methods.Length, "#G");

			methods = property.GetAccessors ();
			Assert.AreEqual (0, methods.Length, "#H");

#if NET_2_0
			property = typeof (TestClass).GetProperty ("PrivateSetter");

			methods = property.GetAccessors (true);
			Assert.AreEqual (2, methods.Length, "#H1");
			Assert.IsNotNull (methods [0], "#H2");
			Assert.IsNotNull (methods [1], "#H3");
			Assert.IsTrue (HasMethod (methods, "get_PrivateSetter"), "#H4");
			Assert.IsTrue (HasMethod (methods, "set_PrivateSetter"), "#H5");

			methods = property.GetAccessors (false);
			Assert.AreEqual (1, methods.Length, "#I1");
			Assert.IsNotNull (methods [0], "#I2");
			Assert.AreEqual ("get_PrivateSetter", methods [0].Name, "#I3");

			methods = property.GetAccessors ();
			Assert.AreEqual (1, methods.Length, "#J1");
			Assert.IsNotNull (methods [0], "#J2");
			Assert.AreEqual ("get_PrivateSetter", methods [0].Name, "#J3");
#endif
		}

		[Test]
		public void GetCustomAttributes ()
		{
			object [] attrs;
			PropertyInfo p = typeof (Base).GetProperty ("P");

			attrs = p.GetCustomAttributes (false);
			Assert.AreEqual (1, attrs.Length, "#A1");
			Assert.AreEqual (typeof (ThisAttribute), attrs [0].GetType (), "#A2");
			attrs = p.GetCustomAttributes (true);
			Assert.AreEqual (1, attrs.Length, "#A3");
			Assert.AreEqual (typeof (ThisAttribute), attrs [0].GetType (), "#A4");

			p = typeof (Base).GetProperty ("T");

			attrs = p.GetCustomAttributes (false);
			Assert.AreEqual (2, attrs.Length, "#B1");
			Assert.IsTrue (HasAttribute (attrs, typeof (ThisAttribute)), "#B2");
			Assert.IsTrue (HasAttribute (attrs, typeof (ComVisibleAttribute)), "#B3");
			attrs = p.GetCustomAttributes (true);
			Assert.AreEqual (2, attrs.Length, "#B41");
			Assert.IsTrue (HasAttribute (attrs, typeof (ThisAttribute)), "#B5");
			Assert.IsTrue (HasAttribute (attrs, typeof (ComVisibleAttribute)), "#B6");

			p = typeof (Base).GetProperty ("Z");

			attrs = p.GetCustomAttributes (false);
			Assert.AreEqual (0, attrs.Length, "#C1");
			attrs = p.GetCustomAttributes (true);
			Assert.AreEqual (0, attrs.Length, "#C2");
		}

		[Test]
		public void GetCustomAttributes_Inherited ()
		{
			object [] attrs;
			PropertyInfo p = typeof (Derived).GetProperty ("P");

			attrs = p.GetCustomAttributes (false);
			Assert.AreEqual (0, attrs.Length, "#A1");
			attrs = p.GetCustomAttributes (true);
			Assert.AreEqual (0, attrs.Length, "#A3");

			p = typeof (Derived).GetProperty ("T");

			attrs = p.GetCustomAttributes (false);
			Assert.AreEqual (2, attrs.Length, "#B1");
			Assert.IsTrue (HasAttribute (attrs, typeof (ThisAttribute)), "#B2");
			Assert.IsTrue (HasAttribute (attrs, typeof (ComVisibleAttribute)), "#B3");
			attrs = p.GetCustomAttributes (true);
			Assert.AreEqual (2, attrs.Length, "#B41");
			Assert.IsTrue (HasAttribute (attrs, typeof (ThisAttribute)), "#B5");
			Assert.IsTrue (HasAttribute (attrs, typeof (ComVisibleAttribute)), "#B6");

			p = typeof (Derived).GetProperty ("Z");

			attrs = p.GetCustomAttributes (false);
			Assert.AreEqual (0, attrs.Length, "#C1");
			attrs = p.GetCustomAttributes (true);
			Assert.AreEqual (0, attrs.Length, "#C2");
		}

		[Test]
		public void IsDefined_AttributeType_Null ()
		{
			Type derived = typeof (Derived);
			PropertyInfo pi = derived.GetProperty ("P");

			try {
				pi.IsDefined ((Type) null, false);
				Assert.Fail ("#1");
			} catch (ArgumentNullException ex) {
				Assert.AreEqual (typeof (ArgumentNullException), ex.GetType (), "#2");
				Assert.IsNull (ex.InnerException, "#3");
				Assert.IsNotNull (ex.Message, "#4");
				Assert.IsNotNull (ex.ParamName, "#5");
				Assert.AreEqual ("attributeType", ex.ParamName, "#6");
			}
		}

		[Test]
		public void AccessorsReflectedType ()
		{
			PropertyInfo pi = typeof (Derived).GetProperty ("T");
			Assert.AreEqual (typeof (Derived), pi.GetGetMethod ().ReflectedType);
			Assert.AreEqual (typeof (Derived), pi.GetSetMethod ().ReflectedType);
		}

		[Test] // bug #399985
		public void SetValue_Enum ()
		{
			TestClass t = new TestClass ();
			PropertyInfo pi = t.GetType ().GetProperty ("Targets");
			pi.SetValue (t, AttributeTargets.Field, null);
			Assert.AreEqual (AttributeTargets.Field, t.Targets, "#1");
			pi.SetValue (t, (int) AttributeTargets.Interface, null);
			Assert.AreEqual (AttributeTargets.Interface, t.Targets, "#2");
		}

		public class ThisAttribute : Attribute
		{
		}

		class Base
		{
			[ThisAttribute]
			public virtual string P {
				get { return null; }
				set { }
			}

			[ThisAttribute]
			[ComVisible (false)]
			public virtual string T {
				get { return null; }
				set { }
			}

			public virtual string Z {
				get { return null; }
				set { }
			}
		}

		class Derived : Base
		{
			public override string P {
				get { return null; }
				set { }
			}
		}

#if NET_2_0
		public class A<T>
		{
			public string Property {
				get { return typeof (T).FullName; }
			}
		}

		public int? nullable_field;

		public int? NullableProperty {
			get { return nullable_field; }
			set { nullable_field = value; }
		}

		[Test]
		public void NullableTests ()
		{
			PropertyInfoTest t = new PropertyInfoTest ();

			PropertyInfo pi = typeof(PropertyInfoTest).GetProperty("NullableProperty");

			pi.SetValue (t, 100, null);
			Assert.AreEqual (100, pi.GetValue (t, null));
			pi.SetValue (t, null, null);
			Assert.AreEqual (null, pi.GetValue (t, null));
		}

		[Test]
		public void Bug77160 ()
		{
			object instance = new A<string> ();
			Type type = instance.GetType ();
			PropertyInfo property = type.GetProperty ("Property");
			Assert.AreEqual (typeof (string).FullName, property.GetValue (instance, null));
		}
#endif

		static bool HasAttribute (object [] attrs, Type attributeType)
		{
			foreach (object attr in attrs)
				if (attr.GetType () == attributeType)
					return true;
			return false;
		}

		static bool HasMethod (MethodInfo [] methods, string name)
		{
			foreach (MethodInfo method in methods)
				if (method.Name == name)
					return true;
			return false;
		}

		private class TestClass
		{
			private AttributeTargets _targets = AttributeTargets.Assembly;

			public AttributeTargets Targets {
				get { return _targets; }
				set { _targets = value; }
			}

			public string ReadOnlyProperty {
				get { return string.Empty; }
			}

			private string Private {
				get { return null; }
				set { }
			}

#if NET_2_0
			public string PrivateSetter {
				get { return null; }
				private set { }
			}
#endif
		}
	}
}
