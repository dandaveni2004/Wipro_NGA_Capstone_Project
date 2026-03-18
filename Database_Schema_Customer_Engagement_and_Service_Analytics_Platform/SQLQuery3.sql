ALTER TABLE Tickets
ADD CONSTRAINT FK_Tickets_Customers
FOREIGN KEY (CustomerId)
REFERENCES Customers(CustomerId);

ALTER TABLE Tickets
ADD CONSTRAINT FK_Tickets_Agents
FOREIGN KEY (AgentId)
REFERENCES Agents(AgentId);

ALTER TABLE Tickets
ADD CONSTRAINT FK_Tickets_Categories
FOREIGN KEY (CategoryId)
REFERENCES TicketCategories(CategoryId);

ALTER TABLE Tickets
ADD CreatedAt DATETIME DEFAULT GETDATE();

SELECT * FROM Tickets;

CREATE INDEX idx_ticket_status
ON Tickets(Status);

EXEC sp_help 'Tickets';