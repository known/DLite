using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Known.DLite.Mapping
{
	[Serializable]
	public abstract class BaseEntity<TEntity> : IComparable<TEntity>
		where TEntity : BaseEntity<TEntity>, new()
	{
		private static List<TEntity> allEntities;
		private static object asyncLick = new object();
		private Transaction trans;

		protected BaseEntity() { }

		public void Insert()
		{
			LoadTransaction();
			TEntity entity = GetEntity();
			AddToList(entity);
			trans.Insert<TEntity>(entity);
		}

		public void Update()
		{
			LoadTransaction();
			TEntity entity = GetEntity();
			RemoveFromList(entity);
			AddToList(entity);
			trans.Update<TEntity>(entity);
		}

		public void Delete()
		{
			LoadTransaction();
			TEntity entity = GetEntity();
			RemoveFromList(entity);
			trans.Delete<TEntity>(entity);
		}

		public void Save()
		{
			if (trans != null)
			{
				trans.SubmitChanges();
				trans.Dispose();
			}
		}

		public static TEntity Get(params object[] id)
		{
			TEntity entity = GetFromList(id);
			if (entity == null)
			{
				KQuery query = new KQuery();
				entity = query.Get<TEntity>(id);
				AddToList(entity);
			}
			return entity;
		}

		public static List<TEntity> FindAll()
		{
			if (allEntities == null)
			{
				lock (asyncLick)
				{
					if (allEntities == null)
					{
						KQuery query = new KQuery();
						allEntities = query.FindAll<TEntity>();
					}
				}
			}
			allEntities.Sort();
			return allEntities;
		}

		public virtual string ToJson()
		{
			TEntity entity = GetEntity();
			StringBuilder sb = new StringBuilder();
			sb.Append("{");
			foreach (var property in typeof(TEntity).GetProperties())
			{
				string name = property.Name;
				object value = property.GetValue(entity, null);
				sb.Append("'" + name + "':'" + value + "',");
			}
			return sb.ToString().TrimEnd(',') + "}";
		}

		public int CompareTo(TEntity other)
		{
			return GetHashCode().CompareTo(other.GetHashCode());
		}

		public override string ToString()
		{
			return GetKeyValueString(GetEntity());
		}

		public override int GetHashCode()
		{
			return GetKeyValueString(GetEntity()).GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj.GetType() == typeof(TEntity))
			{
				return obj.GetHashCode() == GetEntity().GetHashCode();
			}
			return false;
		}

		private void LoadTransaction()
		{
			if (trans == null)
			{
				trans = new Transaction();
			}
		}

		private TEntity GetEntity()
		{
			TEntity entity = new TEntity();
			entity = this as TEntity;
			return entity;
		}

		private static TEntity GetFromList(params object[] id)
		{
			if (allEntities != null)
			{
				foreach (TEntity item in allEntities)
				{
					if (GetKeyValueString(id) == GetKeyValueString(item))
					{
						return item;
					}
				}
			}
			return null;
		}

		private static void AddToList(TEntity entity)
		{
			if (allEntities == null)
			{
				allEntities = new List<TEntity>();
			}
			allEntities.Add(entity);
		}

		private static void RemoveFromList(TEntity entity)
		{
			if (allEntities != null)
			{
				//foreach (TEntity item in allEntities)
				//{
				//    TEntity temp = item;
				//    if (GetKeyValueString(entity) == GetKeyValueString(item))
				//    {
				//        allEntities.Remove(temp);
				//    }
				//}
				for (int i = 0; i < allEntities.Count; i++)
				{
					if (allEntities[i].Equals(entity))
					{
						allEntities.RemoveAt(i);
					}
				}
			}
		}

		private static string GetKeyValueString(TEntity entity)
		{
			MetaType type = MetaManager.GetMetaType(typeof(TEntity));
			object[] keys = type.MetaParameters.Where(p => p.IsPrimaryKey).Select(
				p => type.EntityType.GetProperty(p.PropertyName).GetValue(entity, null)
			).ToArray();
			return GetKeyValueString(keys);
		}

		private static string GetKeyValueString(params object[] id)
		{
			StringBuilder sb = new StringBuilder();
			foreach (object key in id)
			{
				if (key != null)
				{
					sb.Append(key.ToString());
				}
			}
			return sb.ToString();
		}
	}

	//public class Validator<TEntity>
	//{
	//    private TEntity entity;
	//    private List<string> errorMessages;

	//    public List<string> ErrorMessages
	//    {
	//        get
	//        {
	//            if (errorMessages == null)
	//            {
	//                errorMessages = new List<string>();
	//            }
	//            return errorMessages;
	//        }
	//    }

	//    public Validator() { }

	//    public Validator(TEntity entity)
	//    {
	//        this.entity = entity;
	//    }

	//    public Validator<TEntity> Validate(Predicate<TEntity> predicate, string errorMessage)
	//    {
	//        if (!predicate(entity))
	//        {
	//            errorMessages.Add(errorMessage);
	//        }
	//        return this;
	//    }
	//}
}
