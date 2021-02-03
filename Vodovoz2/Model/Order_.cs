using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;

namespace Vodovoz2.Model
{
    public class Order_
    {
        public virtual int Id { get; set; }
        public virtual string Agent { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual Model.Employee Author { get; set; }

        private IList<Model.Good> _goods;
        public virtual IList<Good> Goods {
            get { return _goods ?? (_goods = new List<Good>()); }
            set { _goods = value; }
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
        
        public static Order_ GetOrderById(int id)
        {
            using (ISession session = HibernateHelper.OpenSession())
            {
                var queryResult = session.QueryOver<Model.Order_>().Where(g => g.Id == id).SingleOrDefault();
                return queryResult ?? new Order_();

            }
        }

        public virtual Model.Employee GetOrderAuthor()
        {
            using (ISession session = HibernateHelper.OpenSession())
            {
                var author = GetOrderById(this.Id).Author;
                return author ?? null;
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

        public static IList<Order_> All()
        {
            using (ISession session = HibernateHelper.OpenSession())
            {
                return session.QueryOver<Model.Order_>().List<Model.Order_>();
            }
        }

        public virtual void Delete()
        {
            using (ISession session = HibernateHelper.OpenSession())
            {
                var Goods = session.QueryOver<Model.Good>().Where(g => g.Order == this).List<Model.Good>();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    foreach (Model.Good g in Goods)
                    {
                        g.Order = null;
                        session.Update(g);
                    }
                    session.Delete(this);
                    transaction.Commit();
                }
            }
        }
    }
}
