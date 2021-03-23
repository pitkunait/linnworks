IF NOT EXISTS(SELECT *
              FROM sys.objects
              WHERE object_id = OBJECT_ID(N'dbo.SalesRecords')
                AND type in (N'U'))
    BEGIN
        CREATE TABLE SalesRecords
        (
            Id            INT            NOT NULL IDENTITY PRIMARY KEY,
            Region        VARCHAR(50)    NOT NULL,
            Country       VARCHAR(50)    NOT NULL,
            ItemType      VARCHAR(50)    NOT NULL,
            SalesChannel  VARCHAR(50)    NOT NULL,
            OrderPriority CHAR           NOT NULL,
            OrderDate     DATE           NOT NULL,
            OrderId       INT            NOT NULL,
            ShipDate      DATE           NOT NULL,
            UnitsSold     INT            NOT NULL,
            UnitPrice     DECIMAL(25, 2) NOT NULL,
            UnitCost      DECIMAL(25, 2) NOT NULL,
            TotalRevenue  DECIMAL(25, 2) NOT NULL,
            TotalCost     DECIMAL(25, 2) NOT NULL,
            TotalProfit   DECIMAL(25, 2) NOT NULL,
        )
    END;