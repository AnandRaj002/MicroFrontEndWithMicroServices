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


