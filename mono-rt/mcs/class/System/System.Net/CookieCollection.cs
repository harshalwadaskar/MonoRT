//
// System.Net.CookieCollection
//
// Authors:
// 	Lawrence Pit (loz@cable.a2000.nl)
//	Gonzalo Paniagua Javier (gonzalo@ximian.com)
//
// (c) Copyright 2004 Novell, Inc. (http://www.novell.com)
//

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
using System.Collections;
using System.Globalization;
using System.Runtime.Serialization;

namespace System.Net 
{
	[Serializable]
#if NET_2_1
	public sealed class CookieCollection : ICollection, IEnumerable {
#else
	public class CookieCollection : ICollection, IEnumerable {
#endif
		sealed class CookieCollectionPathComparer : IComparer
		{
			int IComparer.Compare (object p1, object p2)
			{
				Cookie c1 = p1 as Cookie;
				Cookie c2 = p2 as Cookie;

				if (c1 == null || c2 == null)
					return 0;
				
				return (c2.Path.Length - c1.Path.Length);
			}
		}
		
		ArrayList list = new ArrayList (4);

		internal ArrayList List {
			get { return list; }
		}
		// ICollection
		public int Count {
			get { return list.Count; }
		}

		public bool IsSynchronized {
			get { return false; }
		}

		public Object SyncRoot {
			get { return this; }
		}

		public void CopyTo (Array array, int arrayIndex)
		{
			list.CopyTo (array, arrayIndex);
		}

#if NET_2_0
		public void CopyTo (Cookie [] array, int index)
		{
			list.CopyTo (array, index);
		}
#endif

		// IEnumerable
		public IEnumerator GetEnumerator ()
		{
			return list.GetEnumerator ();
		}

		// This

		// LAMESPEC: So how is one supposed to create a writable CookieCollection 
		// instance?? We simply ignore this property, as this collection is always
		// writable.
		public bool IsReadOnly {
			get { return true; }
		}		

		public void Add (Cookie cookie) 
		{
			if (cookie == null)
				throw new ArgumentNullException ("cookie");

			int pos = SearchCookie (cookie);
			if (pos == -1)
				list.Add (cookie);
			else
				list [pos] = cookie;
		}

		internal void SortByPath ()
		{
			if (list == null || list.Count == 0)
				return;

			list.Sort (new CookieCollectionPathComparer ());
		}
		
		int SearchCookie (Cookie cookie)
		{
			string name = cookie.Name;
			string domain = cookie.Domain;
			string path = cookie.Path;

			for (int i = list.Count - 1; i >= 0; i--) {
				Cookie c = (Cookie) list [i];
				if (c.Version != cookie.Version)
					continue;

				if (0 != String.Compare (domain, c.Domain, true, CultureInfo.InvariantCulture))
					continue;

				if (0 != String.Compare (name, c.Name, true, CultureInfo.InvariantCulture))
					continue;

				if (0 != String.Compare (path, c.Path, true, CultureInfo.InvariantCulture))
					continue;

				return i;
			}

			return -1;
		}

		public void Add (CookieCollection cookies) 
		{
			if (cookies == null)
				throw new ArgumentNullException ("cookies");

			foreach (Cookie c in cookies)
				Add (c);
		}

		public Cookie this [int index] {
			get {
				if (index < 0 || index >= list.Count)
					throw new ArgumentOutOfRangeException ("index");

				return (Cookie) list [index];
			}
		}

		public Cookie this [string name] {
			get {
				foreach (Cookie c in list) {
					if (0 == String.Compare (c.Name, name, true, CultureInfo.InvariantCulture))
						return c;
				}
				return null;
			}
		}

	} // CookieCollection
} // System.Net
