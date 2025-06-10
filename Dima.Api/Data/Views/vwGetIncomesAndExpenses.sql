CREATE OR ALTER VIEW [vwGetIncomesAndExpenses] AS
    SELECT
        [Transaction].[UserId],
        MONTH([Transaction].[PaidOrReceivedAt]) AS [Month],
        YEAR([Transaction].[PaidOrReceivedAt]) AS [Year],
        SUM(IIF([Transaction].[Type] = 1, [Transaction].[Amount], 0)) AS [Incomes],
        SUM(IIF([Transaction].[Type] = 2, [Transaction].[Amount], 0)) AS [Expenses]
    FROM
        [Transaction]
    WHERE
        [Transaction].[PaidOrReceivedAt] >= DATEADD(MONTH, -11, CAST(GETDATE() AS DATE))
      AND
        [Transaction].[PaidOrReceivedAt] < DATEADD(MONTH, 1, CAST(GETDATE() AS DATE))
    GROUP BY
        [Transaction].[UserId],
        MONTH([Transaction].[PaidOrReceivedAt]),
        YEAR([Transaction].[PaidOrReceivedAt])