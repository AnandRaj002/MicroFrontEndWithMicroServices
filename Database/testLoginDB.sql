CREATE DATABASE testLogin;

USE testLogin;

CREATE TABLE users (
  id int NOT NULL AUTO_INCREMENT,
  first_name varchar(100),
  last_name varchar(100),
  email varchar(100),
  password varchar(255),
  created_date datetime,
  updated_date datetime,
  PRIMARY KEY (id)
);

INSERT INTO users 
VALUES (1,'john','doe','johndoe@mail.com','12345678',now(),now());

CREATE TABLE users (
  id varchar(50) NOT NULL,
  user_name varchar(100),
  first_name varchar(100),
  last_name varchar(100),
  email varchar(100),
  password varchar(255),
  salt varchar(255),  
  created_date datetime,
  updated_date datetime,
  PRIMARY KEY (id)
);
