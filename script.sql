    drop table if exists Department;

    drop table if exists Employee;

    drop table if exists Good;

    drop table if exists Order_;

    create table Department (
        Id INTEGER not null,
       Name VARCHAR(255) not null,
       HeadId INTEGER,
       primary key (Id)
    );

    create table Employee (
        Id INTEGER not null,
       Name VARCHAR(255) not null,
       Surname VARCHAR(255) not null,
       Patronymic VARCHAR(255) not null,
       BirthDate DATETIME not null,
       Gender VARCHAR(255) not null,
       DepartmentId INTEGER,
       primary key (Id)
    );

    create table Good (
        Id INTEGER not null,
       OrderId BIGINT,
       Name VARCHAR(255) not null,
       Amount INTEGER not null,
       Price INTEGER not null,
       primary key (Id)
    );

    create table Order_ (
        Id BIGINT not null,
       Agent VARCHAR(255),
       Date DATETIME,
       AuthorId INTEGER,
       primary key (Id)
    );

    alter table Department
        add index (HeadId),
        add constraint FK_1E5932E7
        foreign key (HeadId)
        references Employee (Id)
		on delete set null;

    alter table Employee
        add index (DepartmentId),
        add constraint FK_3F4A52CC
        foreign key (DepartmentId)
        references Department (Id)
		on delete set null;

    alter table Good
        add index (OrderId),
        add constraint FK_2D8D7CD6
        foreign key (OrderId)
        references Order_ (Id)
		on delete set null;

    alter table Order_
        add index (AuthorId),
        add constraint FK_A0D098E9
        foreign key (AuthorId)
        references Employee (Id)
		on delete set null;
		
INSERT INTO `department` VALUES (1,'Продажи',NULL),
								(2,'ИТ',NULL);
								
INSERT INTO `employee` VALUES 	(1,'Дарина','Дарьянова','Кирилловна','1917-07-17 00:00:00','female',1),
								(2,'Александр','Прокофьев','Эльдарович','2002-12-22 00:00:00','male',1),
								(3,'Яков','Лермонтов','Антонович','1980-11-26 00:00:00','male',2),
								(4,'Изабелла','Быкова','Леонидовна','1997-02-08 00:00:00','female',NULL);

UPDATE `department` SET `HeadId`=1 WHERE `Id`=1;

							
INSERT INTO `order_` VALUES (1,'James Bond','2020-05-10 00:00:00',1),
							(2,'Vodovoz','2019-10-10 00:00:00',1),
							(3,'PTC Inc','2020-12-20 00:00:00',2);

INSERT INTO `good` VALUES 	(1,2,'Стол',2,18000),
							(2,3,'Стул',1,2000),
							(3,3,'ШкаФ',1,100000),
							(4,2,'Горшок',12,7200),
							(5,1,'Канделябр',5,45000),
							(6,NULL,'Лампа',2,8000);


