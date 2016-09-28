using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Known.Data;
using Known.Data.Mapping;
using Known.Data.Clients;

namespace Test
{
	public class Program
	{
		static void Main(string[] args)
		{
			//TestTransaction();
			TestQuery();

			Console.ReadLine();
		}

		static void AddApplyForm(Transaction da, ApplyForm form)
		{
			da.Insert(form);
			da.DeleteAll(form.Categories);
			da.InsertAll(form.Categories);
		}

		static void TestTransaction()
		{
			ApplyForm apply = new ApplyForm
			{
				FormID = 31,
				FormNo = "VT20090608001",
				ModifyDate = DateTime.Now,
				ModifyID = "902756",
				Answer = "test",
				Question = "tetsees",
				ApplyerID = "21545",
				CreateDate = DateTime.Now,
				CreatorID = "13113",
				Description = "ftwertew",
				Note = "fertetr",
				PreFormNo = "gratr",
				RD = "15614566",
				RDPM = "2343534",
				RDPrjLeader = "3535",
				RDTeamLeader = "3434",
				ReFormNo = "35353453",
				Remark = "wererere",
				IsClose = false
			};
			apply.AddCategory(new Customer
			{
				ID = "AP02",
				Name = "AP_Update",
				Type = "AP"
			});
			apply.AddCategory(new Customer
			{
				ID = "AP03",
				Name = "AP_Delete",
				Type = "AP"
			});
			apply.AddCategory(new Customer
			{
				ID = "AP04",
				Name = "AP_Select",
				Type = "AP"
			});
			Model model = new Model
			{
				Category = new Customer
				{
					ID = "ST01",
					Name = "Create",
					Type = "ST"
				},
				ApplyForm = apply,
				Categories = new List<Customer>()
				{
					new Customer
					{
						ID = "ST02",
						Name = "Update",
						Type = "ST"
					},
					new Customer
					{
						ID = "ST03",
						Name = "Delete",
						Type = "ST"
					},
					new Customer
					{
						ID = "ST04",
						Name = "Select",
						Type = "ST"
					}
				}
			};
			System.Diagnostics.Stopwatch watch;

			watch = new System.Diagnostics.Stopwatch();
			watch.Start();
			using (Transaction da = new Transaction(new AccessProvider()))
			{
				da.Insert(model.Category);
				da.Insert<Customer>(model.Category);
				da.Update<Customer>(model.Category);
				da.Delete<Customer>(model.Category);
				//AddApplyForm(da, model.ApplyForm);
				//model.ApplyForm.FormID = 32;
				//da.Update(model.ApplyForm);
				//da.InsertAll(model.Categories);
				//da.Remove<ApplyForm>(model.ApplyForm);
				Console.WriteLine(da.ToString());
			}
			watch.Stop();
			Console.WriteLine("Access runtime=" + watch.ElapsedMilliseconds + "ms");
			Console.WriteLine("--------------------------------------\n\n");

			watch = new System.Diagnostics.Stopwatch();
			watch.Start();
			using (Transaction da = new Transaction(new SqlProvider()))
			{
				da.Insert(model.Category);
				da.Insert<Customer>(model.Category);
				da.Update<Customer>(model.Category);
				da.Delete<Customer>(model.Category);
				//AddApplyForm(da, model.ApplyForm);
				//model.ApplyForm.FormID = 32;
				//da.Update(model.ApplyForm);
				//da.InsertAll(model.Categories);
				//da.Remove<ApplyForm>(model.ApplyForm);
				Console.WriteLine(da.ToString());
			}
			watch.Stop();
			Console.WriteLine("sql runtime=" + watch.ElapsedMilliseconds + "ms");
			Console.WriteLine("--------------------------------------\n\n");

			watch = new System.Diagnostics.Stopwatch();
			watch.Start();
			using (Transaction da = new Transaction(new OracleProvider()))
			{
				da.Insert(model.Category);
				da.Insert<Customer>(model.Category);
				da.Update<Customer>(model.Category);
				da.Delete<Customer>(model.Category);
				//AddApplyForm(da, model.ApplyForm);
				//model.ApplyForm.FormID = 32;
				//da.Update(model.ApplyForm);
				//da.InsertAll(model.Categories);
				//da.Remove<ApplyForm>(model.ApplyForm);
				Console.WriteLine(da.ToString());
			}
			watch.Stop();
			Console.WriteLine("Oracle runtime=" + watch.ElapsedMilliseconds + "ms");
			Console.WriteLine("--------------------------------------\n\n");
		}

		static void TestQuery()
		{
			System.Diagnostics.Stopwatch watch;
			watch = new System.Diagnostics.Stopwatch();
			watch.Start();
			KQuery query = new KQuery(new OracleProvider("Data Source=***;user=***;password=***;"));
			UserInfo user = query.Get<UserInfo>("902756", "Known_Chen");
			Console.WriteLine(query.ToString());
			//UserInfo user = null;
			//UserInfo user1 = query.Get<UserInfo>("900700");
			//UserInfo user1 = (UserInfo)query.Get(typeof(UserInfo), "900700");
			UserInfo user1 = query.Find("from acct where acctno='900201'").Get<UserInfo>();
			//UserInfo user1 = query.Find<UserInfo>("where acctno='902705'");
			Console.WriteLine(query.ToString());
			//Type t = null;
			//UserInfo user1 = (UserInfo)query.Get("Test.UserInfo", "900700");
			//query.Fetch(user, "902756");
			//query.Fetch(user1, "900700");
			//query.BindData();
			IList<Customer> customers = query.List<Customer>("Type='customer'");
			//IList<Customer> customers =
			//	query.Find("from sys_desccode where type='customer'").List<Customer>();
			//IList customers = query.Find("from sys_desccode where type='customer'")
			//	.List(typeof(Customer));
			Console.WriteLine(query.ToString());
			if (user != null)
			{
				Console.WriteLine("ID\t=" + user.ID);
				Console.WriteLine("UName\t=" + user.UserName);
				Console.WriteLine("Name\t=" + user.Name);
				Console.WriteLine("Ext\t=" + user.Ext);
				Console.WriteLine("Mobile\t=" + user.Mobile);
				Console.WriteLine("Email\t=" + user.Email);
				Console.WriteLine("Team\t=" + user.Team);
			}
			else
			{
				Console.WriteLine("user is null.");
			}
			Console.WriteLine("--------------------------------------\n\n");
			if (user1 != null)
			{
				Console.WriteLine("ID\t=" + user1.ID);
				Console.WriteLine("UName\t=" + user1.UserName);
				Console.WriteLine("Name\t=" + user1.Name);
				Console.WriteLine("Ext\t=" + user1.Ext);
				Console.WriteLine("Mobile\t=" + user1.Mobile);
				Console.WriteLine("Email\t=" + user1.Email);
				Console.WriteLine("Team\t=" + user1.Team);
			}
			else
			{
				Console.WriteLine("user is null.");
			}
			Console.WriteLine("--------------------------------------\n\n");

			Console.WriteLine("ID\tType\tName");
			if (customers != null)
			{
				Console.WriteLine("ID\tType\tName");
				foreach (Customer c in customers)
				{
					Console.WriteLine(c.ID + "\t" + c.Type + "\t" + c.Name);
				}
			}
			Console.WriteLine("--------------------------------------\n\n");

			watch.Stop();
			Console.WriteLine("runtime=" + watch.ElapsedMilliseconds + "ms");
			Console.WriteLine("--------------------------------------\n\n");
		}
	}

	[Table(Name = "acct")]
	class UserInfo
	{
		[Column(Name = "acctno", IsPrimaryKey = true)]
		public string ID { get; set; }
		[Column(Name = "acctuser")]
		public string UserName { get; set; }
		[Column(Name = "acctchinese")]
		public string Name { get; set; }
		[Column(Name = "acctext")]
		public string Ext { get; set; }
		[Column(Name = "acctcellphone")]
		public string Mobile { get; set; }
		[Column(Name = "acctemail")]
		public string Email { get; set; }
		[Column(Name = "acctteam")]
		public string Team { get; set; }
	}

	class Code
	{
		[Column(Name = "code", IsPrimaryKey = true)]
		public string ID { get; set; }
		[Column(Name = "codedesc")]
		public string Name { get; set; }
	}

	class VTCode : Code
	{
		public string SysCode { get; set; }
	}

	[Table(Name = "sys_desccode")]
	class Customer : Code
	{
		public string Type { get; set; }
	}

	[Table(Name = "t_applyformmn", History = "t_applyformmn_his")]
	class ApplyForm
	{
		[Column(IsPrimaryKey = true)]
		public string FormNo { get; set; }
		public int FormID { get; set; }
		public string RD { get; set; }
		public string RDTeamLeader { get; set; }
		public string RDPrjLeader { get; set; }
		public string RDPM { get; set; }
		public string Remark { get; set; }
		public string ModifyID { get; set; }
		public string ApplyerID { get; set; }
		public string CreatorID { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime ModifyDate { get; set; }
		public string Question { get; set; }
		public string Answer { get; set; }
		public string Description { get; set; }
		public string Note { get; set; }
		public string ReFormNo { get; set; }
		public string PreFormNo { get; set; }
		public bool IsClose { get; set; }
		public List<Customer> Categories { get; set; }

		public ApplyForm()
		{
			Categories = new List<Customer>();
		}

		public void AddCategory(Customer category)
		{
			Categories.Add(category);
		}
	}

	class Model
	{
		public Customer Category { get; set; }
		public ApplyForm ApplyForm { get; set; }
		public List<Customer> Categories { get; set; }
	}
}
