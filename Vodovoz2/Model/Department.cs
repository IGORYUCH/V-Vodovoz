using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;

namespace Vodovoz2.Model
{
    public class Department
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual Model.Employee Head { get; set; }
        private IList<Model.Employee> _employees;
        public virtual IList<Employee> Employees
        {
            get {return _employees ?? (_employees = new List<Employee>()); }
            set { _employees = value; }
        }

        public static Department GetDepartmentById(int id)
        {
            using (ISession session = HibernateHelper.OpenSession())
            {
                var queryResult = session.QueryOver<Model.Department>().Where(d => d.Id == id).SingleOrDefault();
                return queryResult ?? null;
            }

        }

        public static IList<Department> All()
        {
            using (ISession session = HibernateHelper.OpenSession())
            {
                return session.QueryOver<Model.Department>().List<Model.Department>();
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

        public virtual Model.Employee GetDepartmentHead()
        {
            using (ISession session = HibernateHelper.OpenSession())
            {
                var queryResult = GetDepartmentById(this.Id).Head;
                return queryResult ?? null;
            }
        }

        public virtual void Delete()
        {
            using (ISession session = HibernateHelper.OpenSession())
            {
                var Employees = session.QueryOver<Model.Employee>().Where(e => e.Department == this).List<Model.Employee>();
                using (ITransaction transaction = session.BeginTransaction()) 
                {
                    foreach (Model.Employee e in Employees)
                    {
                        e.Department = null;
                        session.Update(e);
                    }
                    session.Delete(this);
                    transaction.Commit();
                }
            }
        }
    }
}
