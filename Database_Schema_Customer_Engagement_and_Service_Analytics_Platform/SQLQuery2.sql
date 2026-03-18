
SELECT Status, COUNT(*) AS TotalTickets
FROM Tickets
GROUP BY Status;


SELECT TC.CategoryName, COUNT(*) AS TotalTickets
FROM Tickets T
JOIN TicketCategories TC
ON T.CategoryId = TC.CategoryId
GROUP BY TC.CategoryName;


SELECT A.AgentName, COUNT(*) AS TotalTickets
FROM Tickets T
JOIN Agents A
ON T.AgentId = A.AgentId
GROUP BY A.AgentName;
GO


CREATE VIEW TicketSummary AS
SELECT
T.TicketId,
C.Name AS CustomerName,
A.AgentName,
TC.CategoryName,
T.Issue,
T.Status,
T.CreatedAt
FROM Tickets T
JOIN Customers C ON T.CustomerId = C.CustomerId
JOIN Agents A ON T.AgentId = A.AgentId
JOIN TicketCategories TC ON T.CategoryId = TC.CategoryId;
GO

SELECT * FROM TicketSummary;
GO

CREATE PROCEDURE GetOpenTickets
AS
SELECT *
FROM Tickets
WHERE Status = 'Open';

EXEC GetOpenTickets;