# Установка

Создайте БД и выполните `script.sql` в вашей СУБД

Откройте VisualStudio и выполните команды:

`Install-Package MySql.Data`

`Install-Package nhibernate`

Откройте файл hibernate.cfg.xml. В нем будет содержимое:
```
    <property name="connection.driver_class">NHibernate.Driver.MySqlDataDriver</property>
    <property name="connection.connection_string">
      server=localhost;
      user id=root;
      persistsecurityinfo=True;
      password=password_;
      database=vodovoz2
    </property>
```

Откредактируйте в соответствии с вашими настройками

Выполните сборку проекта
