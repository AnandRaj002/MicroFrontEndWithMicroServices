CREATE PROCEDURE `Register_User` (
	userName varchar(100),
    firstName varchar(100),
	lastName varchar(100),
	email varchar(100),
	password varchar(255),
    passwordSalt varchar(255))
BEGIN
INSERT INTO users
VALUES
(uuid(),
userName,
firstName,
lastName,
email,
password,
passwordSalt,
now(),
now());
END