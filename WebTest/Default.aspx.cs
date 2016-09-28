using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using System.Xml;

namespace WebTest
{
	public partial class _Default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!String.IsNullOrEmpty(Request.QueryString["id"]))
			{
				int id = int.Parse(Request.QueryString["id"]);
				UserInfo user = UserInfo.Get(id);
				Response.Write(user.ToString());
				Response.Write(user.ToJson());
			}
			btnAdd.Click += new EventHandler(btnAdd_Click);
			btnUpdate.Click += new EventHandler(btnUpdate_Click);
			btnDelete.Click += new EventHandler(btnDelete_Click);
			BindData();
		}

		void btnDelete_Click(object sender, EventArgs e)
		{
			int id = int.Parse(Request.Form["ID"]);
			UserInfo user = UserInfo.Get(id);
			user.Delete();
			user.Save();
			BindData();
		}

		void btnUpdate_Click(object sender, EventArgs e)
		{
			int id = int.Parse(Request.Form["ID"]);
			UserInfo user = UserInfo.Get(id);
			user.Password = "555555";
			user.Update();
			user.Save();
			BindData();
		}

		void btnAdd_Click(object sender, EventArgs e)
		{
			UserInfo user = new UserInfo
			{
				UserName = Request.Form["UserName"],
				Password = Request.Form["Password"],
				Question = Request.Form["Question"],
				Answer = Request.Form["Answer"],
				Email = Request.Form["Email"]
			};
			user.Insert();
			user.Save();
			BindData();
		}

		private void BindData()
		{
			gvUser.DataSource = UserInfo.FindAll().OrderBy(u => u.ID);
			gvUser.DataBind();
		}
	}
}
