using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Known.DLite.Mapping;

namespace WebTest
{
	[Table("ks_User")]
	public class UserInfo : BaseEntity<UserInfo>
	{
		[Column(IsDbGenerated = true, IsPrimaryKey = true)]
		public int ID { get; set; }

		[Column(IsChangeable = false)]
		public string UserName { get; set; }

		public string Password { get; set; }

		[Column(IsNullable = true)]
		public string Question { get; set; }

		[Column(IsNullable = true)]
		public string Answer { get; set; }

		public string Email { get; set; }

		public override string ToString()
		{
			return String.Format(
				"ID={0}<br />UserName={1}<br />Password={2}<br />Question={3}<br />Answer={4}<br />Email={5}<br />",
				ID, UserName, Password, Question, Answer, Email
			);
		}
	}
}
