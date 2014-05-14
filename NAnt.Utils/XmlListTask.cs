using System.IO;
using System.Text;
using System.Xml;

using NAnt.Core;
using NAnt.Core.Attributes;

namespace NAnt.Utils.Tasks
{
	[TaskName("xmllist")]
	public class XmlListTask : Task
	{
		private string _xmldoc;
		private string _property;
		private string _selection;

		[TaskAttribute("file", Required = true)]
		[StringValidator(AllowEmpty = false)]
		public string XmlDocument
		{
			get { return _xmldoc; }
			set { _xmldoc = value; }
		}

		[TaskAttribute("xpath", Required = true)]
		[StringValidator(AllowEmpty = false)]
		public string Selection
		{
			get { return _selection; }
			set { _selection = value; }
		}

		[TaskAttribute("property", Required = true)]
		[StringValidator(AllowEmpty = false)]
		public string Property
		{
			get { return _property; }
			set { _property = value; }
		}

		protected override void ExecuteTask()
		{
			//Check the file exists
			string docPath = Project.ExpandProperties(_xmldoc, Location).Replace(@"file:\", "");
			if (!File.Exists(docPath))
			{
				throw new BuildException("The xml document specified does not exist");
			}
			// Load the document and run the selection
			XmlDocument doc = new XmlDocument();
			doc.Load(docPath);
			XmlNodeList list = doc.SelectNodes(_selection);
			Project.Log(Level.Info, "Found " + list.Count.ToString() + " nodes");
			StringBuilder builder = new StringBuilder();
			foreach (XmlNode node in list)
			{
				builder.Append(node.InnerText);
				builder.Append(",");
			}
			if (builder.Length > 0)
			{
				builder.Length -= 1;
			}
			if (Project.Properties.Contains(_property))
			{
				Project.Properties[_property] = builder.ToString();
			}
			else
			{
				Project.Properties.Add(_property, builder.ToString());
			}
		}
	}
}