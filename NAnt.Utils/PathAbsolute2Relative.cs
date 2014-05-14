using System;
using System.IO;
using System.Text;

using NAnt.Core;
using NAnt.Core.Attributes;

using NUnit.Framework;

namespace NAnt.Utils.Tasks
{
	[TaskName("pathabsolute2relative")]
	public class PathAbsolute2Relative : Task
	{
		private string _relativePathProp;
		private string _absPath;
		private string _baseDir;
		internal string Result;

		[TaskAttribute("absPath", Required = true)]
		[StringValidator(AllowEmpty = false)]
		public string AbsPath
		{
			get { return _absPath.Replace("/", @"\").Replace(@"file:\", ""); }
			set { _absPath = value; }
		}

		[TaskAttribute("relativePathProp", Required = true)]
		[StringValidator(AllowEmpty = false)]
		public string RelativePathProp
		{
			get { return _relativePathProp; }
			set { _relativePathProp = value; }
		}

		[TaskAttribute("baseDir", Required = true)]
		[StringValidator(AllowEmpty = false)]
		public string BaseDir
		{
			get { return _baseDir.Replace("/", @"\").Replace(@"file:\", "").TrimEnd('\\'); }
			set { _baseDir = value; }
		}

		protected override void ExecuteTask()
		{
			string absDir = Path.GetDirectoryName(AbsPath).TrimEnd('\\');

			int i = 0;
			string[] baseDirArray = BaseDir.Split('\\');
			string[] absDirArray = absDir.Split('\\');
			foreach (string s in absDirArray)
			{
				if (i >= baseDirArray.Length || baseDirArray[i] != s)
				{
					break;
				}
				i++;
			}

			StringBuilder builder = new StringBuilder();
			for (int j = i; j < baseDirArray.Length; j++)
			{
				builder.Append(@"..\");
			}
			for (int j = i; j < absDirArray.Length; j++)
			{
				builder.Append(absDirArray[j]);
				builder.Append(@"\");
			}

			builder.Append(Path.GetFileName(AbsPath));
			Result = builder.ToString();
			if (Project != null)
			{
				if (Project.Properties.Contains(RelativePathProp))
				{
					Project.Properties[RelativePathProp] = Result;
				}
				else
				{
					Project.Properties.Add(RelativePathProp, Result);
				}
			}
		}

#if UNIT_TESTS
		[TestFixture]
		public class Tests
		{
			[Test]
			public void TestAbsMoreBase1()
			{
				PathAbsolute2Relative task = new PathAbsolute2Relative();
				task.AbsPath = @"d:\1\ports.list";
				task.BaseDir = @"d:\";
				task.ExecuteTask();
				Assert.AreEqual(@"1\ports.list", task.Result);
			}

			[Test]
			public void TestAbsMoreBase2()
			{
				PathAbsolute2Relative task = new PathAbsolute2Relative();
				task.AbsPath = @"d:\1\2\ports.list";
				task.BaseDir = @"d:\1";
				task.ExecuteTask();
				Assert.AreEqual(@"2\ports.list", task.Result);
			}

			[Test]
			public void TestAbsMoreBase3()
			{
				PathAbsolute2Relative task = new PathAbsolute2Relative();
				task.AbsPath = @"d:\1\2\ports.list";
				task.BaseDir = @"c:\1";
				task.ExecuteTask();
				Assert.AreEqual(@"..\..\d:\1\2\ports.list", task.Result);
			}
			
			[Test]
			public void TestAbsSameBase1()
			{
				PathAbsolute2Relative task = new PathAbsolute2Relative();
				task.AbsPath = @"d:\1\ports.list";
				task.BaseDir = @"d:\1";
				task.ExecuteTask();
				Assert.AreEqual(@"ports.list", task.Result);
			}

			[Test]
			public void TestAbsSameBase2()
			{
				PathAbsolute2Relative task = new PathAbsolute2Relative();
				task.AbsPath = @"d:\ports.list";
				task.BaseDir = @"d:\";
				task.ExecuteTask();
				Assert.AreEqual(@"ports.list", task.Result);
			}

			[Test]
			public void TestAbsLessBase1()
			{
				PathAbsolute2Relative task = new PathAbsolute2Relative();
				task.AbsPath = @"d:\1\ports.list";
				task.BaseDir = @"d:\1\2\3";
				task.ExecuteTask();
				Assert.AreEqual(@"..\..\ports.list", task.Result);
			}

			[Test]
			public void Example1()
			{
				PathAbsolute2Relative task = new PathAbsolute2Relative();
				task.AbsPath = @"D:\sites\NumSite\Sources\Core\bin\Debug\AppConfiguration.dll";
				task.BaseDir = @"D:\sites\NumSite\Sources\Projects\Portal\Code\Portal.Business";
				task.ExecuteTask();
				Assert.AreEqual(@"..\..\..\..\Core\bin\Debug\AppConfiguration.dll", task.Result);
			}
      
		}
#endif
	}
}