/*
-- // This script will create the phone book database
*/

create database PhoneBookDb

use PhoneBookDb

-- // 1. Phone book table
create table [PhoneBook] (Id int not null primary key identity, 
                        [Name] varchar(100))

-- // Phone book entry table
create table [Entry] (Id int not null primary key identity, 
                      [Name] varchar(100), 
					  PhoneNumber varchar(50),
					  PhoneBookId int,
					  Foreign key (PhoneBookId) references PhoneBook (Id))
