using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;

namespace Vodovoz2.Model
{
    public class Employee
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Surname { get; set; }
        public virtual string Patronymic { get; set; }
        public virtual DateTime BirthDate { get; set; }
        public virtual string Gender { get; set; }
        public virtual Model.Department Department { get; set; }

        private IList<Model.Order_> _orders;
        public virtual IList<Order_> Orders
        {
            get { return _orders ?? (_orders = new List<Order_>()); }
            set { _orders = value; }
        }

        private IList<Model.Department> _departments;
        
        public virtual IList<Department> Departments
        {
            get { return _departments ?? (_departments = new List<Department>()); }
            set { _departments = value; }
        }

        public static Employee GetEmployeeById(int id)
        {
            using (ISession session = HibernateHelper.OpenSession())
            {
                var queryResult = session.QueryOver<Model.Employee>().Where(e => e.Id == id).SingleOrDefault();
                return queryResult ?? new Employee();
            }

        }

        public virtual Model.Department GetEmployeeDepartment()
        {
            using (ISession session = HibernateHelper.OpenSession())
            {
                var department = GetEmployeeById(this.Id).Department;
                return department ?? null;
            }
        }

        public virtual int Save()
        {

            using (ISession session = HibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction()) // Создаем новую тразакцию
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

        public static IList<Employee> All()
        {
            using (ISession session = HibernateHelper.OpenSession())
            {
                return session.QueryOver<Model.Employee>().List<Model.Employee>();
            }
        }

        public virtual void Delete()
        {
            using (ISession session = HibernateHelper.OpenSession())
            {
                var Orders = session.QueryOver<Model.Order_>().Where(o => o.Author == this).List<Model.Order_>();
                var Departments = session.QueryOver<Model.Department>().Where(d => d.Head == this).List<Model.Department>();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    foreach (Model.Order_ o in Orders)
                    {
                        o.Author = null;
                        session.Update(o);
                    }
                    foreach (Model.Department d in Departments)
                    {
                        d.Head = null;
                        session.Update(d);
                    }
                    session.Delete(this);
                    transaction.Commit();
                }
            }
        }
    }
}
