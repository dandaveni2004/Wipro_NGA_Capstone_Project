CREATE DATABASE CustomerSupportDB;
GO

USE CustomerSupportDB;
GO


CREATE TABLE Customers(
CustomerId INT IDENTITY(1,1) PRIMARY KEY,
Name VARCHAR(100) NOT NULL,
Email VARCHAR(100) UNIQUE
);


CREATE TABLE Agents(
AgentId INT IDENTITY(1,1) PRIMARY KEY,
AgentName VARCHAR(100) NOT NULL
);


CREATE TABLE TicketCategories(
CategoryId INT IDENTITY(1,1) PRIMARY KEY,
CategoryName VARCHAR(100) NOT NULL
);

CREATE TABLE Tickets(
TicketId INT IDENTITY(1,1) PRIMARY KEY,
CustomerId INT NOT NULL,
AgentId INT NOT NULL,
CategoryId INT NOT NULL,
Issue VARCHAR(255) NOT NULL,
Status VARCHAR(50) NOT NULL,
CreatedAt DATETIME DEFAULT GETDATE(),

FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId),
FOREIGN KEY (AgentId) REFERENCES Agents(AgentId),
FOREIGN KEY (CategoryId) REFERENCES TicketCategories(CategoryId)
);


INSERT INTO Customers(Name,Email) VALUES
('Ganesh','ganesh@gmail.com'),
('Praveen','praveen@gmail.com'),
('Raju','raju@gmail.com'),
('Karthik','karthik@gmail.com'),
('Pradeep','pradeep@gmail.com');


INSERT INTO Agents(AgentName) VALUES
('Agent Sanvith'),
('Agent Dhruvan'),
('Agent Madhukar'),
('Agent Nithin'),
('Agent Rohith');


INSERT INTO TicketCategories(CategoryName) VALUES
('Billing'),
('Software'),
('Account Access'),
('Technical Issue'),
('Login Problem');


INSERT INTO Tickets(CustomerId,AgentId,CategoryId,Issue,Status) VALUES
(1,1,5,'Cannot login','Open'),
(2,2,1,'Payment failed','Closed'),
(3,3,3,'Forgot password','Open'),
(4,4,2,'Application crash','Open'),
(5,5,4,'Website slow','In Progress');

SELECT * FROM Customers;
SELECT * FROM Agents;
SELECT * FROM TicketCategories;
SELECT * FROM Tickets;
GO