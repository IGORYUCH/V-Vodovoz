using System;

using NHibernate;
using System.Collections.Generic;

namespace Vodovoz2.Model
{
    public class Good
    {
        public virtual int Id { get; set; }
        public virtual Model.Order_ Order { get; set; }
        public virtual string Name { get; set; }
        public virtual int Amount { get; set; }
        public virtual int Price { get; set; }

        public virtual void Delete()
        {

            using (ISession session = HibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(this);
                    transaction.Commit();
                }
            }
        }

        public static IList<Good> All()
        {
            using (ISession session = HibernateHelper.OpenSession())
            {
                return session.QueryOver<Model.Good>().List<Model.Good>();
            }
        }

        public virtual void Update()
        {
            using (ISession session = HibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(this);
                    transaction.Commit();
                }
            }
        }

        public virtual int Save()
        {

            using (ISession session = HibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    int insertedId = (int)session.Save(this);
                    transaction.Commit();
                    return insertedId;
                }
            }
        }

        public virtual Model.Order_ GetGoodOrder()
        {
            using (ISession session = HibernateHelper.OpenSession())
            {
                var order = GetGoodById(this.Id).Order;
                return order ?? null;
            }
        }

        public static Good GetGoodById(int id)
        {
            using (ISession session = HibernateHelper.OpenSession())
            {
                var queryResult = session.QueryOver<Model.Good>().Where(g => g.Id == id).SingleOrDefault();
                return queryResult ?? new Good();
            }

        }
    }
}
