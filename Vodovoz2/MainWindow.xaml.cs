using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using MySql.Data;

namespace Vodovoz2
{

    public partial class MainWindow : Window
    {
        int defaultItemIndex = 0;
        int EmployeeTabIndex = 0;
        int DepartmentTabIndex = 1;
        int GoodTabIndex = 2;
        int OrderTabIndex = 3;
        MySqlConnection Connection;
        public MainWindow()
        {
            InitializeComponent();
            InitEmployeeTab();
            InitDepartmentTab();
            InitGoodTab();
            InitOrderTab();
        }

        private ComboBoxItem createComboBoxItem(string name, object content)
        {
            ComboBoxItem newComboBoxItem = new ComboBoxItem();
            newComboBoxItem.Name = name;
            newComboBoxItem.Content = content;
            return newComboBoxItem;
        }

        private void InitOrderTab()
        {
            ComboBoxItem defaultItem = createComboBoxItem("default", "(not selected)");
            Orders_Combo.Items.Add(defaultItem);
            Orders_Combo.SelectedItem = defaultItem;

            var orders = Model.Order_.All();
            foreach (Model.Order_ order in orders)
            {
                string orderId = order.Id.ToString();
                ComboBoxItem orderItem = createComboBoxItem("item" + orderId, orderId);
                Orders_Combo.Items.Add(orderItem);
            }
            FillOrderTabAuthorCombo();
        }

        private void InitGoodTab()
        {
            ComboBoxItem defaultItem = createComboBoxItem("default", "(not selected)");
            Goods_Combo.Items.Add(defaultItem);
            Goods_Combo.SelectedItem = defaultItem;

            var goods = Model.Good.All();
            foreach (Model.Good good in goods)
            {
                string goodId = good.Id.ToString();
                string goodName = good.Name;
                ComboBoxItem goodItem = createComboBoxItem("item" + goodId, goodName);
                Goods_Combo.Items.Add(goodItem);
            }
            FillGoodTabOrderIdCombo();
        }

        private void FillGoodTabOrderIdCombo()
        {
            ComboBoxItem defaultOrderItem = new ComboBoxItem();
            Hyperlink defaultOrderItemLink = new Hyperlink();
            defaultOrderItemLink.Name = "default";
            defaultOrderItemLink.Inlines.Add("(no order)");
            defaultOrderItemLink.Click += OrderIdComboItemLinkClicked;
            defaultOrderItem.Content = defaultOrderItemLink;
            OrderIdCombo.Items.Add(defaultOrderItem);

            var orders = Model.Order_.All();
            foreach (Model.Order_ order in orders)
            {
                string orderId = order.Id.ToString();
                ComboBoxItem orderItem = new ComboBoxItem();
                Hyperlink orderItemLink = new Hyperlink();
                orderItemLink.Name = "item" + orderId;
                orderItemLink.Inlines.Add(orderId);
                orderItemLink.Click += OrderIdComboItemLinkClicked;
                orderItem.Content = orderItemLink;
                OrderIdCombo.Items.Add(orderItem);
            }
        }

        private void InitDepartmentTab()
        {
            ComboBoxItem defaultItem = createComboBoxItem("default", "(not selected)");
            Departments_Combo.Items.Add(defaultItem);
            Departments_Combo.SelectedItem = defaultItem;

            var departments = Model.Department.All();
            foreach (Model.Department department in departments)
            {
                ComboBoxItem departmentItem = createComboBoxItem("item" + department.Id, department.Name);
                Departments_Combo.Items.Add(departmentItem);
            }
            fillHeadComboBox();
        }

        private void InitEmployeeTab()
        { 
            ComboBoxItem defaultItem = createComboBoxItem("default", "(not selected)");
            Employees_Combo.Items.Add(defaultItem);
            Employees_Combo.SelectedItem = defaultItem;
            var employees = Model.Employee.All();

            foreach (Model.Employee employee in employees)
            {
                string id = employee.Id.ToString();
                string name = employee.Name;
                string surname = employee.Surname;
                string patronymic = employee.Patronymic;
                string fullName = name + " " + surname + " " + patronymic;
                ComboBoxItem employeeItem = createComboBoxItem("item" + id, fullName);
                Employees_Combo.Items.Add(employeeItem);
            }
            fillEmployeeTabDepartmentsCombo();
        }

        private void SetOrderButtonsOnSelected()
        {
            Change_button3.IsEnabled = true;
            Delete_button3.IsEnabled = true;
        }

        private void SetGoodButtonsOnSelected()
        {
            Change_button2.IsEnabled = true;
            Delete_button2.IsEnabled = true;
        }
        private void SetEmployeeButtonsOnSelected()
        {
            Change_button.IsEnabled = true;
            Delete_button.IsEnabled = true;
        }

        private void SetDepartmentButtonsOnSelected()
        {
            Change_button1.IsEnabled = true;
            Delete_button1.IsEnabled = true;
        }

        private void SetDepartmentButtonsDefault()
        {
            Change_button1.IsEnabled = false;
            Delete_button1.IsEnabled = false;
        }
        private void SetOrderButtonsDefault()
        {
            Change_button3.IsEnabled = false;
            Delete_button3.IsEnabled = false;
        }

        private void SetGoodButtonsDefault()
        {
            Change_button2.IsEnabled = false;
            Delete_button2.IsEnabled = false;
        }

        private void SetEmployeeButtonsDefault()
        {
            Change_button.IsEnabled = false;
            Delete_button.IsEnabled = false;
        }

        private void fillEmployeeTabDepartmentsCombo()
        {
            ComboBoxItem defaultItem = new ComboBoxItem();
            Hyperlink departmentDefaultItemLink = new Hyperlink();
            departmentDefaultItemLink.Name = "default";
            departmentDefaultItemLink.Inlines.Add("(no department)");
            departmentDefaultItemLink.Click += EmployeeTabDepartmentsComboItemLinkClicked;
            defaultItem.Content = departmentDefaultItemLink;
            EmployeeDepartmentCombo.Items.Add(defaultItem);
            EmployeeDepartmentCombo.SelectedItem = defaultItem;
            var departments = Model.Department.All();
            foreach (Model.Department department in departments)
            {
                string department_name = department.Name;
                ComboBoxItem combo_item = new ComboBoxItem();
                Hyperlink departmentItemLink = new Hyperlink();
                departmentItemLink.Name = "item" + department.Id.ToString();
                departmentItemLink.Inlines.Add(department_name);
                departmentItemLink.Click += EmployeeTabDepartmentsComboItemLinkClicked;
                combo_item.Content = departmentItemLink;
                EmployeeDepartmentCombo.Items.Add(combo_item);
            }
        }

        private void FillOrderTabAuthorCombo()
        {
            ComboBoxItem defaultAuthorItem = new ComboBoxItem();
            Hyperlink defaultAuthorItemLink = new Hyperlink();
            defaultAuthorItemLink.Name = "default";
            defaultAuthorItemLink.Inlines.Add("(no author)");
            defaultAuthorItemLink.Click += AuthorComboItemLinkClicked;
            defaultAuthorItem.Content = defaultAuthorItemLink;
            AuthorCombo.Items.Add(defaultAuthorItem);
            AuthorCombo.SelectedItem = defaultAuthorItem;

            var employees = Model.Employee.All();
            foreach (Model.Employee employee in employees)
            {
                string id = employee.Id.ToString();
                string fullName = employee.Name + " " + employee.Surname + " " + employee.Patronymic;
                ComboBoxItem authorItem = new ComboBoxItem();
                Hyperlink authorItemLink = new Hyperlink();
                authorItemLink.Inlines.Add(fullName);
                authorItemLink.Name = "item" + id;
                authorItemLink.Click += AuthorComboItemLinkClicked;
                authorItem.Content = authorItemLink;
                AuthorCombo.Items.Add(authorItem);
            }
        }

        private void fillHeadComboBox()
        {
            ComboBoxItem defaultHeadItem = new ComboBoxItem();
            Hyperlink defaultHeadItemLink = new Hyperlink();
            defaultHeadItemLink.Inlines.Add("(no head)");
            defaultHeadItemLink.Click += DepartmentsComboItemLinkClicked;
            defaultHeadItemLink.Name = "default";
            defaultHeadItem.Content = defaultHeadItemLink;
            Head_Combo.Items.Clear();
            Head_Combo.Items.Add(defaultHeadItem);
            Head_Combo.SelectedItem = defaultHeadItem;

            var employees = Model.Employee.All();
            foreach (Model.Employee employee in employees)
            {
                string id = employee.Id.ToString();
                string name = employee.Name;
                string surname = employee.Surname;
                string patronymic = employee.Patronymic;
                ComboBoxItem combo_item = new ComboBoxItem();
                Hyperlink link = new Hyperlink();
                link.Inlines.Add(name + " " + surname + " " + patronymic);
                link.Name = "item" + id;
                link.Click += DepartmentsComboItemLinkClicked;
                combo_item.Content = link;
                Head_Combo.Items.Add(combo_item);
            }
        }

        private void AddEmployeeButtonClick(object sender, RoutedEventArgs e)
        {
            string name = Name_box.Text;
            string surname = Surname_box.Text;
            string patronymic = Patronymic_box.Text;
            string date = Date_box.Text;
            ComboBoxItem genderItem = (ComboBoxItem)Gender_Box.SelectedItem;
            ComboBoxItem departmentItem = (ComboBoxItem)EmployeeDepartmentCombo.SelectedItem;
            Hyperlink departmentItemLink = (Hyperlink)departmentItem.Content;
            DateTime parsedDate;
            if (genderItem is null)
            {
                MessageBox.Show("Не выбран пол", "Добавление в БД");
                return;
            }

            if (!DateTime.TryParse(Date_box.Text, out parsedDate))
            {
                MessageBox.Show("Неверный формат даты", "Добавление в БД");
                return;
            }

            var newEmployee = new Model.Employee {
                Name = Name_box.Text,
                Surname = Surname_box.Text,
                Patronymic = Patronymic_box.Text,
                Gender = genderItem.Name,
                BirthDate = parsedDate
            };

            if (!(departmentItemLink.Name == "default"))
            {
                var selectedDepartment = Model.Department.GetDepartmentById(int.Parse(departmentItemLink.Name.Substring(4)));
                newEmployee.Department = selectedDepartment;
            }

            string fullName = newEmployee.Name + " " + newEmployee.Surname + " " + newEmployee.Patronymic;
            string insertedId = newEmployee.Save().ToString();

            ComboBoxItem newEmployeeItem = createComboBoxItem("item" + insertedId, fullName);
            Employees_Combo.Items.Add(newEmployeeItem);
            Employees_Combo.SelectedItem = newEmployeeItem;

            ComboBoxItem newDepartmentEmployeeItem = new ComboBoxItem();
            Hyperlink newDepartmentEmployeeItemLink = new Hyperlink();
            newDepartmentEmployeeItemLink.Click += DepartmentsComboItemLinkClicked;
            newDepartmentEmployeeItemLink.Name = "item" + insertedId;
            newDepartmentEmployeeItemLink.Inlines.Add(fullName);
            newDepartmentEmployeeItem.Content = newDepartmentEmployeeItemLink;
            Head_Combo.Items.Add(newDepartmentEmployeeItem);

            ComboBoxItem newOrderEmployeeItem = new ComboBoxItem();
            Hyperlink newOrderEmployeeItemLink = new Hyperlink();
            newOrderEmployeeItemLink.Click += AuthorComboItemLinkClicked;
            newOrderEmployeeItemLink.Name = "item" + insertedId;
            newOrderEmployeeItemLink.Inlines.Add(fullName);
            newOrderEmployeeItem.Content = newOrderEmployeeItemLink;
            AuthorCombo.Items.Add(newOrderEmployeeItem);

            MessageBox.Show("Новая запись добавлена в таблицу employees", "Добавление в БД");
        }

        private void OrdersComboSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedOrderItem = (ComboBoxItem)Orders_Combo.SelectedItem;
            if (selectedOrderItem.Name == "default")
            {
                setOrderInputsDefault();
                SetOrderButtonsDefault();
            }
            else
            {
                var selectedOrder = Model.Order_.GetOrderById(int.Parse(selectedOrderItem.Name.Substring(4)));
                var orderAuthor = selectedOrder.GetOrderAuthor();
                AgentBox.Text = selectedOrder.Agent;
                DateBox.Text = selectedOrder.Date.ToString();

                if (orderAuthor == null)
                {
                    AuthorCombo.SelectedIndex = 0;
                } else
                {
                    string authorId = "item" + orderAuthor.Id.ToString();
                    foreach (ComboBoxItem authorItem in AuthorCombo.Items)
                    {
                        Hyperlink authorItemLink = (Hyperlink)authorItem.Content;
                        if (authorItemLink.Name == authorId)
                        {
                            AuthorCombo.SelectedItem = authorItem;
                            break;
                        }
                    }
                    SetOrderButtonsOnSelected();
                }
            }
        }

        private void GoodsComboSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedGoodItem = (ComboBoxItem)Goods_Combo.SelectedItem;
            if (selectedGoodItem.Name == "default")
            {
                setGoodInputsDefault();
                SetGoodButtonsDefault();
            } 
            else
            {
                string goodId = selectedGoodItem.Name.Substring(4);
                var selectedGood = Model.Good.GetGoodById(int.Parse(goodId));
                var goodOrder = selectedGood.GetGoodOrder();
                if (goodOrder == null)
                {
                    OrderIdCombo.SelectedIndex = defaultItemIndex;
                } else
                {
                    string orderId = "item" + goodOrder.Id.ToString();
                    foreach (ComboBoxItem orderIdItem in OrderIdCombo.Items)
                    {
                        Hyperlink orderIdItemLink = (Hyperlink)orderIdItem.Content;
                        if (orderIdItemLink.Name == orderId)
                        {
                            OrderIdCombo.SelectedItem = orderIdItem;
                            break;
                        }
                    }
                }
                GoodNameBox.Text = selectedGood.Name;
                GoodAmountBox.Text = selectedGood.Amount.ToString();
                GoodPriceBox.Text = selectedGood.Price.ToString();

                SetGoodButtonsOnSelected();
            }
        }
        private void setEmployeeInputsDefault()
        {
            Name_box.Text = "";
            Surname_box.Text = "";
            Patronymic_box.Text = "";
            Date_box.Text = "";
            Gender_Box.SelectedItem = null;
            EmployeeDepartmentCombo.SelectedIndex = defaultItemIndex;
        }

        private void setDepartmentInputsDefault()
        {
            DepName_box.Text = "";
            Head_Combo.SelectedIndex = defaultItemIndex;
        }

        private void setGoodInputsDefault()
        {
            GoodNameBox.Text = "";
            GoodPriceBox.Text = "";
            GoodAmountBox.Text = "";
            OrderIdCombo.SelectedIndex = defaultItemIndex;
        }

        private void setOrderInputsDefault()
        {
            AgentBox.Text = "";
            DateBox.Text = "";
            AuthorCombo.SelectedIndex = defaultItemIndex;
        }

        private void DepartmentsComboSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem SelectedDepartmentItem = (ComboBoxItem)Departments_Combo.SelectedItem;
            string DepartmentId = SelectedDepartmentItem.Name.Substring(4);
            if (SelectedDepartmentItem.Name == "default")
            {
                setDepartmentInputsDefault();
                SetDepartmentButtonsDefault();
            } else
            {
                var selectedDepartment = Model.Department.GetDepartmentById(int.Parse(DepartmentId));
                var departmentHead = selectedDepartment.GetDepartmentHead();

                DepName_box.Text = selectedDepartment.Name;
                if (departmentHead is null)
                {
                    Head_Combo.SelectedIndex = defaultItemIndex;
                } else
                {
                    foreach (ComboBoxItem HeadComboItem in Head_Combo.Items)
                    {
                        Hyperlink HeadComboItemLink = (Hyperlink)HeadComboItem.Content;
                        if (HeadComboItemLink.Name == "item" + departmentHead.Id.ToString())
                        {
                            Head_Combo.SelectedItem = HeadComboItem;
                            break;
                        }
                    }
                }
                SetDepartmentButtonsOnSelected();
            }
        }
        private void EmployeesComboSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem SelectedEmployeeItem = (ComboBoxItem)Employees_Combo.SelectedItem;
            if (SelectedEmployeeItem.Name == "default")
            {
                SetEmployeeButtonsDefault();
                setEmployeeInputsDefault();
            }
            else
            {
                string employeeId = SelectedEmployeeItem.Name.Substring(4);
                var selectedEmployee = Model.Employee.GetEmployeeById(int.Parse(employeeId));

                Name_box.Text = selectedEmployee.Name;
                Surname_box.Text = selectedEmployee.Surname;
                Patronymic_box.Text = selectedEmployee.Patronymic;
                Date_box.Text = selectedEmployee.BirthDate.ToString();
                Model.Department department = selectedEmployee.GetEmployeeDepartment();
                if (department is null)
                {
                    EmployeeDepartmentCombo.SelectedIndex = defaultItemIndex;
                } else
                {
                    foreach (ComboBoxItem employeeDepartmentItem in EmployeeDepartmentCombo.Items)
                    {
                        Hyperlink employeeDepartmentItemLink = (Hyperlink)employeeDepartmentItem.Content;
                        if (employeeDepartmentItemLink.Name == "item" + department.Id.ToString())
                        {
                            EmployeeDepartmentCombo.SelectedItem = employeeDepartmentItem;
                            break;
                        }
                    }
                }
                foreach (ComboBoxItem genderItem in Gender_Box.Items)
                {
                    if (genderItem.Content.Equals(selectedEmployee.Gender))
                    {
                        Gender_Box.SelectedItem = genderItem;
                    }
                }
                SetEmployeeButtonsOnSelected();
            }
        }

        private void OrderIdComboItemLinkClicked(object sender, RoutedEventArgs e)
        {
            Hyperlink ClickedLink = (Hyperlink)sender;
            string ClickedLinkName = ClickedLink.Name;
            Tabs.SelectedIndex = OrderTabIndex;

            foreach (ComboBoxItem orderItem in Orders_Combo.Items)
            {
                if (orderItem.Name == ClickedLinkName)
                {
                    Orders_Combo.SelectedItem = orderItem;
                }
            }
        }

        private void EmployeeTabDepartmentsComboItemLinkClicked(object sender, RoutedEventArgs e)
        {
            Hyperlink ClickedLink = (Hyperlink)sender;
            string ClickedLinkName = ClickedLink.Name;
            Tabs.SelectedIndex = DepartmentTabIndex;

            foreach (ComboBoxItem departmentItem in Departments_Combo.Items)
            {
                if (departmentItem.Name == ClickedLinkName)
                {
                    Departments_Combo.SelectedItem = departmentItem;
                }
            }
        }

        private void DepartmentsComboItemLinkClicked(object sender, RoutedEventArgs e)
        {
            Hyperlink ClickedLink = (Hyperlink)sender;
            string ClickedLinkName = ClickedLink.Name;
            Tabs.SelectedIndex = EmployeeTabIndex;
            foreach (ComboBoxItem employeeItem in Employees_Combo.Items)
            {
                if (employeeItem.Name == ClickedLinkName)
                {
                    Employees_Combo.SelectedItem = employeeItem;
                }
            }
        }

        private void AuthorComboItemLinkClicked(object sender, RoutedEventArgs e)
        {
            Hyperlink ClickedLink = (Hyperlink)sender;
            string ClickedLinkName = ClickedLink.Name;
            Tabs.SelectedIndex = EmployeeTabIndex;
            foreach (ComboBoxItem employeeItem in Employees_Combo.Items)
            {
                if (employeeItem.Name == ClickedLinkName)
                {
                    Employees_Combo.SelectedItem = employeeItem;
                }
            }
        }

        private void AddDepartmentButtonClick(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedHeadItem = (ComboBoxItem)Head_Combo.SelectedItem;
            Hyperlink selectedHeadItemLink = (Hyperlink)selectedHeadItem.Content;
            var newDepartment = new Model.Department
            {
                Name = DepName_box.Text
            };

            if (!(selectedHeadItemLink.Name == "default"))
            {
                newDepartment.Head = Model.Employee.GetEmployeeById(int.Parse(selectedHeadItemLink.Name.Substring(4)));
            }
            int insertedId = newDepartment.Save();

            ComboBoxItem newDepartmentItem = new ComboBoxItem();
            Hyperlink newDepartmentItemLink = new Hyperlink();
            newDepartmentItemLink.Click += EmployeeTabDepartmentsComboItemLinkClicked;
            newDepartmentItemLink.Name = "item" + insertedId.ToString();
            newDepartmentItemLink.Inlines.Add(newDepartment.Name);
            newDepartmentItem.Content = newDepartmentItemLink;
            EmployeeDepartmentCombo.Items.Add(newDepartmentItem);

            newDepartmentItem = createComboBoxItem("item" + insertedId.ToString(), newDepartment.Name);
            Departments_Combo.Items.Add(newDepartmentItem);
            Departments_Combo.SelectedItem = newDepartmentItem;

            MessageBox.Show("Запись добавлена в таблицу departments", "Добавление в  БД");
        }

        private void ChangeDepartmentButtonClick(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedDepartmentItem = (ComboBoxItem)Departments_Combo.SelectedItem;
            string newDepartmentName = DepName_box.Text;

            ComboBoxItem selectedHeadItem = (ComboBoxItem)Head_Combo.SelectedItem;
            Hyperlink selectedHeadItemLink = (Hyperlink)selectedHeadItem.Content;
            string selectedHead = selectedHeadItemLink.Name;

            var selectedDepartment = Model.Department.GetDepartmentById(int.Parse(selectedDepartmentItem.Name.Substring(4)));
            string departmentId = selectedDepartment.Id.ToString();
            selectedDepartment.Name = newDepartmentName;
            if (selectedHead == "default")
            {
                selectedDepartment.Head = null;
            } else
            {
                selectedDepartment.Head = Model.Employee.GetEmployeeById(int.Parse(selectedHead.Substring(4)));
            }
            selectedDepartment.Update();
            selectedDepartmentItem.Content = newDepartmentName;

            foreach (ComboBoxItem employeeDepartmentItem in EmployeeDepartmentCombo.Items)
            {
                Hyperlink employeeDepartmentItemLink = (Hyperlink)employeeDepartmentItem.Content;
                if (employeeDepartmentItemLink.Name == "item" + departmentId)
                {
                    //If element is selected we need to delete it and paste new in order to make new department name visible
                    if (employeeDepartmentItem.IsSelected)
                    {
                        ComboBoxItem newEmployeeDepartmentItem = new ComboBoxItem();
                        Hyperlink newEmployeeDepartmentItemLink = new Hyperlink();
                        newEmployeeDepartmentItemLink.Name = "item" + departmentId;
                        newEmployeeDepartmentItemLink.Inlines.Add(newDepartmentName);
                        newEmployeeDepartmentItem.Content = newEmployeeDepartmentItemLink;
                        EmployeeDepartmentCombo.Items.Add(newEmployeeDepartmentItem);
                        EmployeeDepartmentCombo.SelectedItem = newEmployeeDepartmentItem;
                        EmployeeDepartmentCombo.Items.Remove(employeeDepartmentItem);
                    } else
                    {
                        employeeDepartmentItemLink.Inlines.Clear();
                        employeeDepartmentItemLink.Inlines.Add(newDepartmentName);
                    }
                    break;
                }
            }
            MessageBox.Show("Запись изменена в таблице departments", "Редактирование БД");
        }

        private void DeleteGoodButtonClick(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedGoodItem = (ComboBoxItem)Goods_Combo.SelectedItem;
            string goodId = selectedGoodItem.Name.Substring(4);
            var selectedGood = Model.Good.GetGoodById(int.Parse(goodId));
            selectedGood.Delete();

            Goods_Combo.SelectedIndex = defaultItemIndex;
            Goods_Combo.Items.Remove(selectedGoodItem);

            MessageBox.Show("Запись удалена из таблицы goods", "Удаление из БД");
        }

        private void AddGoodButtonClick(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedOrderItem = (ComboBoxItem)OrderIdCombo.SelectedItem;
            Hyperlink selectedOrderItemLink =(Hyperlink)selectedOrderItem.Content;
            ComboBoxItem selectedGoodItem = (ComboBoxItem)Goods_Combo.SelectedItem;
            string goodId = selectedGoodItem.Name.Substring(4);
            var newGood = new Model.Good
            {
                Name = GoodNameBox.Text,
                Amount = int.Parse(GoodAmountBox.Text),
                Price = int.Parse(GoodPriceBox.Text)
            };
            if (!(selectedOrderItemLink.Name == "default"))
            { 
                string selectedOrderId = selectedOrderItemLink.Name.Substring(4);
                var selectedOrder = Model.Order_.GetOrderById(int.Parse(selectedOrderId));
                newGood.Order = selectedOrder;
            }

            int insertedId = newGood.Save();

            ComboBoxItem newGoodItem = createComboBoxItem("item" + insertedId.ToString(), newGood.Name);
            Goods_Combo.Items.Add(newGoodItem);
            Goods_Combo.SelectedItem = newGoodItem;

            MessageBox.Show("Запись добавлена в таблицу goods", "Добавление в БД");
        }

        private void ChangeGoodButtonClick(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedGoodItem = (ComboBoxItem)Goods_Combo.SelectedItem;
            ComboBoxItem selectedOrderItem = (ComboBoxItem)OrderIdCombo.SelectedItem;
            Hyperlink selectedOrderItemLink = (Hyperlink)selectedOrderItem.Content;
            string goodId = selectedGoodItem.Name.Substring(4);
            var selectedGood = Model.Good.GetGoodById(int.Parse(goodId));
            selectedGood.Name = GoodNameBox.Text;
            selectedGood.Price = int.Parse(GoodPriceBox.Text);
            selectedGood.Amount = int.Parse(GoodAmountBox.Text);
            if (selectedOrderItemLink.Name == "default")
            {
                selectedGood.Order = null;
            } else
            {
                string selectedOrderId = selectedOrderItemLink.Name.Substring(4);
                var selectedOrder = Model.Order_.GetOrderById(int.Parse(selectedOrderId));
                selectedGood.Order = selectedOrder;
            }
            selectedGood.Update();

            selectedGoodItem.Content = selectedGood.Name;

            MessageBox.Show("Запись изменена в таблице goods", "Редактирование БД");
        }

        private void ChangeOrderButtonClick(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedOrderItem = (ComboBoxItem)Orders_Combo.SelectedItem;
            ComboBoxItem selectedAuthorItem = (ComboBoxItem)AuthorCombo.SelectedItem;
            Hyperlink selectedAuthorItemLink = (Hyperlink)selectedAuthorItem.Content;
            string selectedOrderId = selectedOrderItem.Name.Substring(4);
            var selectedOrder = Model.Order_.GetOrderById(int.Parse(selectedOrderId));
            DateTime parsedDate;
            if (!DateTime.TryParse(DateBox.Text, out parsedDate))
            {
                MessageBox.Show("Неверный формат даты", "Редактирование БД");
                return;
            }
            selectedOrder.Agent = AgentBox.Text;
            selectedOrder.Date = parsedDate;

            if (selectedAuthorItemLink.Name == "default")
            {
                selectedOrder.Author = null;
            } else
            {
                string selectedAuthorId = selectedAuthorItemLink.Name.Substring(4);
                var orderAuthor = Model.Employee.GetEmployeeById(int.Parse(selectedAuthorId));
                selectedOrder.Author = orderAuthor;
            }
            selectedOrder.Update();

               MessageBox.Show("Запись изменена в таблице orders", "Редактирование БД");
        }

        private void AddOrderButtonClick(object sender, RoutedEventArgs e)
        {
            DateTime parsedDate;
            if (!DateTime.TryParse(DateBox.Text, out parsedDate))
            {
                MessageBox.Show("Неверный формат даты", "Добавление в БД");
                return;
            }
            var newOrder = new Model.Order_ {
                Agent = AgentBox.Text,
                Date = parsedDate
            };
            ComboBoxItem selectedAuthorItem = (ComboBoxItem)AuthorCombo.SelectedItem;
            Hyperlink selectedAuthorItemLink = (Hyperlink)selectedAuthorItem.Content;
            if (!(selectedAuthorItemLink.Name == "default"))
            {
                string selectedAuthorId = selectedAuthorItemLink.Name.Substring(4);
                newOrder.Author = Model.Employee.GetEmployeeById(int.Parse(selectedAuthorId));
            }
            string lastIndex = newOrder.Save().ToString();

            ComboBoxItem newOrderItem = createComboBoxItem("item" + lastIndex, lastIndex);
            Orders_Combo.Items.Add(newOrderItem);
            Orders_Combo.SelectedItem = newOrderItem;

            ComboBoxItem newGoodOrderItem = new ComboBoxItem();
            Hyperlink newGoodOrderItemLink = new Hyperlink();
            newGoodOrderItemLink.Name = "item" + lastIndex;
            newGoodOrderItemLink.Inlines.Add(lastIndex);
            newGoodOrderItemLink.Click += OrderIdComboItemLinkClicked;
            newGoodOrderItem.Content = newGoodOrderItemLink;
            OrderIdCombo.Items.Add(newGoodOrderItem);

            MessageBox.Show("Запись добавлена в таблицу orders", "Добавление в БД");
        }

        private void DeleteOrderButtonClick(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedOrderItem = (ComboBoxItem)Orders_Combo.SelectedItem;
            string selectedOrderId = selectedOrderItem.Name;
            var selectedOrder = Model.Order_.GetOrderById(int.Parse(selectedOrderId.Substring(4)));
            selectedOrder.Delete();
            foreach (ComboBoxItem goodOrderItem in OrderIdCombo.Items)
            {
                Hyperlink gooOrderItemLink = (Hyperlink)goodOrderItem.Content;
                if (gooOrderItemLink.Name == selectedOrderId)
                {
                    if (goodOrderItem.IsSelected)
                    {
                        OrderIdCombo.SelectedIndex = defaultItemIndex;
                    }
                    OrderIdCombo.Items.Remove(goodOrderItem);
                    break;
                }
            }

            Orders_Combo.SelectedIndex = defaultItemIndex;
            Orders_Combo.Items.Remove(selectedOrderItem);

            MessageBox.Show("Запись удалена из таблицы orders", "Удаление из БД");
        }

        private void DeleteDepartmentButtonClick(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedDepartmentItem = (ComboBoxItem)Departments_Combo.SelectedItem;
            string selectedDepartmentId = selectedDepartmentItem.Name;

            var selectedDepartment = Model.Department.GetDepartmentById(int.Parse(selectedDepartmentId.Substring(4)));
            selectedDepartment.Delete();
            foreach (ComboBoxItem employeeDepartmentItem in EmployeeDepartmentCombo.Items)
            {
                Hyperlink employeeDepartmentItemLink = (Hyperlink)employeeDepartmentItem.Content;
                if (employeeDepartmentItemLink.Name == selectedDepartmentId)
                {
                    if (employeeDepartmentItem.IsSelected)
                    {
                        EmployeeDepartmentCombo.SelectedIndex = defaultItemIndex;
                    }
                    EmployeeDepartmentCombo.Items.Remove(employeeDepartmentItem);
                    break;
                }
            }

            Departments_Combo.SelectedIndex = defaultItemIndex;
            Departments_Combo.Items.Remove(selectedDepartmentItem);

            MessageBox.Show("Запись удалена из таблицы departments", "Удаление из БД");
        }

        private void DeleteEmployeeButtonClick(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedEmployeeItem = (ComboBoxItem)Employees_Combo.SelectedItem;
            string employeeId = selectedEmployeeItem.Name;

            var selectedEmployee = Model.Employee.GetEmployeeById(int.Parse(employeeId.Substring(4)));
            selectedEmployee.Delete();
            Employees_Combo.SelectedIndex = defaultItemIndex;
            Employees_Combo.Items.Remove(selectedEmployeeItem);

            foreach (ComboBoxItem departmentHeadItem in Head_Combo.Items)
            {
                Hyperlink departmentHeadItemLink = (Hyperlink)departmentHeadItem.Content;
                if (departmentHeadItemLink.Name == employeeId)
                {
                    if (departmentHeadItem.IsSelected)
                    {
                        Head_Combo.SelectedIndex = defaultItemIndex;
                    }
                    Head_Combo.Items.Remove(departmentHeadItem);
                    break;
                }
            }

            foreach (ComboBoxItem orderAuthorItem in AuthorCombo.Items){
                Hyperlink orderAuthorItemLink = (Hyperlink)orderAuthorItem.Content;
                if (orderAuthorItemLink.Name == employeeId)
                {
                    if (orderAuthorItem.IsSelected)
                    {
                        AuthorCombo.SelectedIndex = defaultItemIndex;
                    }
                    AuthorCombo.Items.Remove(orderAuthorItem);
                    break;
                }
            }
        }

        private void ChangeEmployeeButtonClick(object sender, RoutedEventArgs e)
        {
            ComboBoxItem EmployeeSelectedItem = (ComboBoxItem)Employees_Combo.SelectedItem;
            ComboBoxItem selectedDepartmentItem = (ComboBoxItem)EmployeeDepartmentCombo.SelectedItem;
            Hyperlink selectedDepartmentItemLink = (Hyperlink)selectedDepartmentItem.Content;
            ComboBoxItem genderItem = (ComboBoxItem)Gender_Box.SelectedItem;
            DateTime parsedDate;
            if (genderItem is null)
            {
                MessageBox.Show("Не выбран пол", "Редактирование БД");
                return;
            }
            if (!DateTime.TryParse(Date_box.Text, out parsedDate))
            {
                MessageBox.Show("Неверный формат даты", "Редактирование БД");
                return;
            }

            string selectedEmployeeId = EmployeeSelectedItem.Name;

            var selectedEmployee = Model.Employee.GetEmployeeById(int.Parse(selectedEmployeeId.Substring(4)));
            string fullName = Name_box.Text + " " + Surname_box.Text + " " + Patronymic_box.Text;
            selectedEmployee.Gender = genderItem.Name;
            selectedEmployee.Name = Name_box.Text;
            selectedEmployee.Surname = Surname_box.Text;
            selectedEmployee.Patronymic = Patronymic_box.Text;
            selectedEmployee.BirthDate = parsedDate;

            if (!(selectedDepartmentItemLink.Name == "default"))
            {
                var selectedDepartment = Model.Department.GetDepartmentById(int.Parse(selectedDepartmentItemLink.Name.Substring(4)));
                selectedEmployee.Department = selectedDepartment;
            }
            selectedEmployee.Update();

            EmployeeSelectedItem.Content = fullName;

            foreach (ComboBoxItem DepartmentHeadItem in Head_Combo.Items)
            {
                Hyperlink DepartmentHeadItemLink = (Hyperlink)DepartmentHeadItem.Content;
                if (DepartmentHeadItemLink.Name == selectedEmployeeId)
                {
                    if (DepartmentHeadItem.IsSelected)
                    {
                        Hyperlink newDepartmentHeadItemLink = new Hyperlink();
                        newDepartmentHeadItemLink.Name = selectedEmployeeId;
                        newDepartmentHeadItemLink.Inlines.Add(fullName);
                        newDepartmentHeadItemLink.Click += DepartmentsComboItemLinkClicked;
                        DepartmentHeadItem.Content = newDepartmentHeadItemLink;
                    } else
                    {
                        DepartmentHeadItemLink.Inlines.Clear();
                        DepartmentHeadItemLink.Inlines.Add(fullName);
                    }
                    break;
                }
            }

            foreach (ComboBoxItem orderAuthorItem in AuthorCombo.Items)
            {
                Hyperlink orderAuthorItemLink = (Hyperlink)orderAuthorItem.Content;
                if (orderAuthorItemLink.Name == selectedEmployeeId)
                {
                    if (orderAuthorItem.IsSelected)
                    {
                        Hyperlink newOrderAuthorItemLink = new Hyperlink();
                        newOrderAuthorItemLink.Name = selectedEmployeeId;
                        newOrderAuthorItemLink.Inlines.Add(fullName);
                        newOrderAuthorItemLink.Click += AuthorComboItemLinkClicked;
                        orderAuthorItem.Content = newOrderAuthorItemLink;
                    } else
                    {
                        orderAuthorItemLink.Inlines.Clear();
                        orderAuthorItemLink.Inlines.Add(fullName);
                    }
                    break;
                }
            }
            MessageBox.Show("Запись успешно изменена", "Редактирование БД");
        }
    }
}
