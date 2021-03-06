//
// CreateItem.cs: Creates build item.
//
// Author:
//   Marek Sieradzki (marek.sieradzki@gmail.com)
//
// (C) 2005 Marek Sieradzki
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

#if NET_2_0

using System;
using System.Collections.Generic;
using Microsoft.Build.Framework;

namespace Microsoft.Build.Tasks {
	public class CreateItem : TaskExtension {
	
		string[]	additionalMetadata;
		ITaskItem[]	exclude;
		ITaskItem[]	include;
	
		public CreateItem ()
		{
		}

		public override bool Execute ()
		{
			if (include == null || include.Length == 0)
				return true;

			List<ITaskItem> output = new List<ITaskItem> ();
			foreach (ITaskItem item in include) {
				if (IsExcluded (item))
					continue;

				output.Add (item);
				if (AdditionalMetadata == null)
					continue;

				foreach (string metadata in AdditionalMetadata) {
					//a=1
					string [] parts = metadata.Split (new char [] {'='}, 2, StringSplitOptions.RemoveEmptyEntries);
					if (parts.Length == 2)
						item.SetMetadata (parts [0].Trim (), parts [1].Trim ());
				}
			}

			include = output.ToArray ();

			return true;
		}

		public string[] AdditionalMetadata {
			get { return additionalMetadata; }
			set { additionalMetadata = value; }
		}

		public ITaskItem[] Exclude {
			get { return exclude; }
			set { exclude = value; }
		}

		[Output]
		public ITaskItem[] Include {
			get { return include; }
			set { include = value; }
		}

		bool IsExcluded (ITaskItem eitem)
		{
			if (exclude == null) return false;

			foreach (ITaskItem item in exclude)
				if (String.Compare (eitem.ItemSpec, item.ItemSpec) == 0)
					return true;

			return false;
		}
	}
}

#endif
