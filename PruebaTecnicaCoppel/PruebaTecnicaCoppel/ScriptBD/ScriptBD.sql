--SCRIPT DE LA PRUEBA TECNICA
IF EXISTS (SELECT name FROM master.sys.databases WHERE name = N'Rinku')
	BEGIN
	  USE master;
	  DROP DATABASE Rinku;
	END
GO
CREATE DATABASE Rinku;
GO
USE Rinku;
GO
IF  EXISTS (SELECT '' FROM SYS.OBJECTS WHERE OBJECT_ID=OBJECT_ID('Employees') AND type='U')
    DROP TABLE Employees;
GO
CREATE TABLE Employees(
    Id BIGINT IDENTITY(1, 1) NOT NULL,
    NumberEmployee BIGINT,
    Name VARCHAR(50),
    Role INT,
    Status BIT DEFAULT 1,
    CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED([Id] ASC)
    ) ON [PRIMARY]
GO
ALTER TABLE Employees ADD CONSTRAINT EC_Employee UNIQUE (NumberEmployee);
GO
IF  EXISTS (SELECT '' FROM SYS.OBJECTS WHERE OBJECT_ID=OBJECT_ID('MoveEmployees') AND type='U')
    DROP TABLE MoveEmployees
GO
CREATE TABLE MoveEmployees(
    Id BIGINT IDENTITY(1, 1) NOT NULL,
	IdEmployee BIGINT,
    DateMove DATE,
    Role INT,
    Status BIT DEFAULT 1,
	Deliver INTEGER,
    CONSTRAINT [PK_Moves] PRIMARY KEY CLUSTERED([Id] ASC)
    ) ON [PRIMARY]

--PROCEDIMIENTOS ALMACENADOS
GO
IF EXISTS (SELECT '' FROM SYS.OBJECTS WHERE OBJECT_ID=OBJECT_ID('SaveEmployee') AND type='P')
    DROP PROCEDURE SaveEmployee
GO
CREATE PROCEDURE SaveEmployee
@NumberEmployee BIGINT,
@Name VARCHAR(50),
@Role INT
-- =============================================
-- Author:  Raul Adrian Osuna Davizon
-- Create date: 26/07/2023
-- Description:Consulty to register employees
-- EXECUTION: SaveEmployee 12, 'Raul', 1
-- =============================================

AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT TOP 1 Id FROM Employees(NOLOCK) WHERE NumberEmployee=@NumberEmployee)
	BEGIN
		INSERT INTO Employees(NumberEmployee,Name,Role) VALUES(@NumberEmployee,@Name,@Role);
		SELECT 'Opreacion Realizada con exito' 'Message', 1 'ResponseCode'; 
	END
    ELSE
	BEGIN
		SELECT 'Ya existe un registro con ese numero de empleado' 'Message', 0 'ResponseCode'; 
	END

    SET NOCOUNT OFF;
END
GO
IF EXISTS (SELECT '' FROM SYS.OBJECTS WHERE OBJECT_ID=OBJECT_ID('GetEmployeeById') AND type='P')
    DROP PROCEDURE GetEmployeeById
GO
CREATE PROCEDURE GetEmployeeById
@Id BIGINT
-- =============================================
-- Author:  Raul Adrian Osuna Davizon
-- Create date: 26/07/2023
-- Description:Consulty to obtain employee by Id
-- EXECUTION: 
-- =============================================

AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT E.Id, E.NumberEmployee, E.Name, E.Role, E.Status FROM Employees E(NOLOCK)WHERE E.Status=1 AND E.Id=@Id;

    SET NOCOUNT OFF;
END
GO
IF EXISTS (SELECT '' FROM SYS.OBJECTS WHERE OBJECT_ID=OBJECT_ID('GetEmployeeByNumberEmployee') AND type='P')
    DROP PROCEDURE GetEmployeeByNumberEmployee
GO
CREATE PROCEDURE GetEmployeeByNumberEmployee
@NumberEmployee BIGINT
-- =============================================
-- Author:  Raul Adrian Osuna Davizon
-- Create date: 26/07/2023
-- Description:Consulty to obtain employee by number employee
-- EXECUTION: 
-- =============================================

AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT E.NumberEmployee, E.Name, E.Role FROM Employees E(NOLOCK)WHERE E.Status='A' AND E.Id=@NumberEmployee;

    SET NOCOUNT OFF;
END
GO
IF EXISTS (SELECT '' FROM SYS.OBJECTS WHERE OBJECT_ID=OBJECT_ID('UpdateEmployeeByNumberEmployee') AND type='P')
    DROP PROCEDURE UpdateEmployeeByNumberEmployee
GO
CREATE PROCEDURE UpdateEmployeeByNumberEmployee
@NumberEmployee BIGINT,
@Name VARCHAR(50),
@Role INT,
@Status INT
-- =============================================
-- Author:  Raul Adrian Osuna Davizon
-- Create date: 26/07/2023
-- Description:Consulty to Register a employee using Number Employee
-- EXECUTION: 
-- =============================================

AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Employees SET Name=@Name, Role=@Role, Status=1 WHERE NumberEmployee= @NumberEmployee;
    SET NOCOUNT OFF;
END
GO
IF EXISTS (SELECT '' FROM SYS.OBJECTS WHERE OBJECT_ID=OBJECT_ID('UpdateEmployeeById') AND type='P')
    DROP PROCEDURE UpdateEmployeeById
GO
CREATE PROCEDURE UpdateEmployeeById
@Id BIGINT,
@NumberEmployee BIGINT,
@Name VARCHAR(50),
@Role INT
-- =============================================
-- Author:  Raul Adrian Osuna Davizon
-- Create date: 26/07/2023
-- Description:Consulty to Update a employee using Id
-- EXECUTION: UpdateEmployeeById 1, 1234, 'Raul', 1
-- =============================================

AS
BEGIN
    SET NOCOUNT ON;
	IF NOT EXISTS(SELECT TOP 1 Id FROM Employees(NOLOCK) WHERE NumberEmployee=@NumberEmployee AND Id IN(@Id) AND Status=1)
	BEGIN
		UPDATE E  SET E.Name=@Name,E.NumberEmployee=@NumberEmployee, E.Role=@Role, E.Status=1 FROM Employees E WHERE Id= @Id;
		SELECT 'Opreacion Realizada con exito' 'Message', 1 'ResponseCode'; 
	END
	ELSE IF EXISTS(SELECT TOP 1 Id FROM Employees(NOLOCK) WHERE NumberEmployee=@NumberEmployee AND Id  != @Id AND Status=1)
	BEGIN
		UPDATE E  SET E.Name=@Name,E.NumberEmployee=@NumberEmployee, E.Role=@Role, E.Status=1 FROM Employees E WHERE Id= @Id;
		SELECT 'Opreacion Realizada con exito' 'Message', 1 'ResponseCode'; 
		
	END
	ELSE
	BEGIN
		SELECT 'El numero de empleado ya existe' 'Message', 0 'ResponseCode'; 
	END
    
    SET NOCOUNT OFF;
END
GO
IF EXISTS (SELECT '' FROM SYS.OBJECTS WHERE OBJECT_ID=OBJECT_ID('GetEmployeesWithStatusAvailable') AND type='P')
    DROP PROCEDURE GetEmployeesWithStatusAvailable
GO
CREATE PROCEDURE GetEmployeesWithStatusAvailable
-- =============================================
-- Author:  Raul Adrian Osuna Davizon
-- Create date: 26/07/2023
-- Description:Obtain Employees with status available status=1
-- EXECUTION: GetEmployeesWithStatusAvailable
-- =============================================

AS
BEGIN
    SET NOCOUNT ON;
	SELECT E.Id, E.NumberEmployee, E.Name, E.Role, E.Status FROM Employees E(NOLOCK)WHERE E.Status=1; 
    SET NOCOUNT OFF;
END
GO
IF EXISTS (SELECT '' FROM SYS.OBJECTS WHERE OBJECT_ID=OBJECT_ID('SaveMoveEmployee') AND type='P')
    DROP PROCEDURE SaveMoveEmployee
GO
CREATE PROCEDURE SaveMoveEmployee
@IdEmployee BIGINT,
@DateMove DATE,
@Role INT,
@Deliver INT
-- =============================================
-- Author:  Raul Adrian Osuna Davizon
-- Create date: 26/07/2023
-- Description:Consulty to register employees
-- EXECUTION: SaveMoveEmployee 1, '2023-07-01', 1,4
-- =============================================

AS
BEGIN
    SET NOCOUNT ON;
    
	IF NOT EXISTS(SELECT TOP 1 Id FROM MoveEmployees ME(NOLOCK) WHERE (DATEPART(MONTH,ME.DateMove)=DATEPART(MONTH,@DateMove)) AND (DATEPART(YEAR,ME.DateMove)=DATEPART(YEAR,@DateMove) ) AND ME.IdEmployee=@IdEmployee AND ME.Status=1 )
	BEGIN
		INSERT INTO MoveEmployees(IdEmployee,DateMove,Role,Deliver) VALUES(@IdEmployee,@DateMove,@Role,@Deliver);
		SELECT 'Opreacion realizada con exito' 'Message', 1 'ResponseCode'; 
	END
	ELSE
	BEGIN
		SELECT 'Ya existe un registro para este mes' 'Message', 0 'ResponseCode'; 
	END
    

    SET NOCOUNT OFF;
END
GO
IF EXISTS (SELECT '' FROM SYS.OBJECTS WHERE OBJECT_ID=OBJECT_ID('GetMovesEmployeeAvailablesByIdEmployee') AND type='P')
    DROP PROCEDURE GetMovesEmployeeAvailablesByIdEmployee
GO
CREATE PROCEDURE GetMovesEmployeeAvailablesByIdEmployee

-- =============================================
-- Author:  Raul Adrian Osuna Davizon
-- Create date: 26/07/2023
-- Description:Consulty to register employees
-- EXECUTION: GetMovesEmployeeAvailablesByIdEmployee 1,7,2023
-- =============================================
@IdEmployee BIGINT,
@Month INT,
@Year INT
AS
BEGIN
    SET NOCOUNT ON;
    CREATE TABLE #tmpResult(
		IdMove BIGINT,
		Name VARCHAR(50),
		Role INT,
		DateMove DATE,
		DateMoveStr VARCHAR(50),
		SalaryBase DECIMAL,
		SalaryPerMonth DECIMAL,
		Deliver INT,
		DeliverBonus DECIMAL,
		HourBonus DECIMAL,
		VouchersBonus DECIMAL,
		ISR DECIMAL,
	)
	INSERT INTO #tmpResult(IdMove,Name,Role,DateMove,SalaryBase,SalaryPerMonth,Deliver,DeliverBonus, HourBonus) 
	SELECT ME.Id,E.Name,ME.Role,ME.DateMove,30.00,(((30.00*8)*7)*4) 'SalaryPerMonth',ME.Deliver, ME.Deliver*5.00 'DeliverBonus',
		CASE WHEN ME.Role=1 THEN ((10*8)*6) WHEN ME.Role=2 THEN ((5*8)*6) ELSE 0 END 'HourBonus'
	FROM MoveEmployees ME(NOLOCK)
	INNER JOIN Employees E(NOLOCK) ON E.Id=ME.IdEmployee
	WHERE E.Status=1 AND E.Id=@IdEmployee AND DATEPART(MONTH,ME.DateMove)=@Month AND DATEPART(YEAR,ME.DateMove)=@Year AND ME.Status=1; 
	UPDATE #tmpResult SET VouchersBonus=SalaryPerMonth*4/100, SalaryPerMonth=(SalaryPerMonth+(SalaryPerMonth*4/100));
	UPDATE #tmpResult SET ISR=CASE WHEN(SalaryPerMonth+DeliverBonus+HourBonus)>10000 THEN 12 ELSE 9 END;
	
	SELECT IdMove,Name,Role,DateMove,FORMAT(DateMove,'MMMM/yyyy') 'DateMoveSTR',SalaryBase,SalaryPerMonth,Deliver,DeliverBonus,HourBonus,ISR, VouchersBonus,(SalaryPerMonth+DeliverBonus+HourBonus)-(SalaryPerMonth+DeliverBonus+HourBonus)*(ISR/100) 'Total'  FROM #tmpResult;
	DROP TABLE #tmpResult;
    SET NOCOUNT OFF;
END
GO
IF EXISTS (SELECT '' FROM SYS.OBJECTS WHERE OBJECT_ID=OBJECT_ID('GetMovesEmployeeAvailablesALL') AND type='P')
    DROP PROCEDURE GetMovesEmployeeAvailablesALL
GO
CREATE PROCEDURE GetMovesEmployeeAvailablesALL

-- =============================================
-- Author:  Raul Adrian Osuna Davizon
-- Create date: 26/07/2023
-- Description:Consulty to register employees
-- EXECUTION: GetMovesEmployeeAvailablesALL 
-- =============================================
AS
BEGIN
    SET NOCOUNT ON;
    CREATE TABLE #tmpResult(
		IdMove BIGINT,
		Name VARCHAR(50),
		Role INT,
		DateMove DATE,
		DateMoveStr VARCHAR(50),
		SalaryBase DECIMAL,
		SalaryPerMonth DECIMAL,
		Deliver INT,
		DeliverBonus DECIMAL,
		HourBonus DECIMAL,
		VouchersBonus DECIMAL,
		ISR DECIMAL,
	)
	INSERT INTO #tmpResult(IdMove,Name,Role,DateMove,SalaryBase,SalaryPerMonth,Deliver,DeliverBonus, HourBonus) 
	SELECT ME.Id,E.Name,ME.Role,ME.DateMove,30.00,(((30.00*8)*7)*4) 'SalaryPerMonth',ME.Deliver, ME.Deliver*5.00 'DeliverBonus',
		CASE WHEN ME.Role=1 THEN ((10*8)*6) WHEN ME.Role=2 THEN ((5*8)*6) ELSE 0 END 'HourBonus'
	FROM MoveEmployees ME(NOLOCK)
	INNER JOIN Employees E(NOLOCK) ON E.Id=ME.IdEmployee
	WHERE E.Status=1 AND ME.Status=1; 
	UPDATE #tmpResult SET VouchersBonus=SalaryPerMonth*4/100, SalaryPerMonth=(SalaryPerMonth+(SalaryPerMonth*4/100));
	UPDATE #tmpResult SET ISR=CASE WHEN(SalaryPerMonth+DeliverBonus+HourBonus)>10000 THEN 12 ELSE 9 END;
	
	SELECT IdMove,Name,Role,DateMove,FORMAT(DateMove,'MMMM/yyyy') 'DateMoveSTR',SalaryBase,SalaryPerMonth,Deliver,DeliverBonus,HourBonus,ISR, VouchersBonus,(SalaryPerMonth+DeliverBonus+HourBonus)-(SalaryPerMonth+DeliverBonus+HourBonus)*(ISR/100) 'Total'  FROM #tmpResult;
	DROP TABLE #tmpResult;
    SET NOCOUNT OFF;
END
GO
IF EXISTS (SELECT '' FROM SYS.OBJECTS WHERE OBJECT_ID=OBJECT_ID('GetMoveEmployeeById') AND type='P')
    DROP PROCEDURE GetMoveEmployeeById
GO
CREATE PROCEDURE GetMoveEmployeeById
@Id BIGINT
-- =============================================
-- Author:  Raul Adrian Osuna Davizon
-- Create date: 26/07/2023
-- Description:Consulty to register employees
-- EXECUTION: GetMoveEmployeeById 1
-- =============================================
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ME.Id,ME.IdEmployee, ME.DateMove, ME.Role, ME.Status, ME.Deliver,CAST(FORMAT(ME.DateMove,'yyyy-MM-dd') AS VARCHAR(11)) 'DateMoveSTR', E.Name
	FROM MoveEmployees ME(NOLOCK) 
	INNER JOIN Employees E(NOLOCK) ON  E.Id=ME.IdEmployee
	WHERE ME.Id=@Id
		
    SET NOCOUNT OFF;
END
GO
IF EXISTS (SELECT '' FROM SYS.OBJECTS WHERE OBJECT_ID=OBJECT_ID('UpdateMoveEmployeeById') AND type='P')
    DROP PROCEDURE UpdateMoveEmployeeById
GO
CREATE PROCEDURE UpdateMoveEmployeeById
@Id BIGINT,
@IdEmployee BIGINT,
@DateMove DATE,
@Role INT,
@Deliver INT
-- =============================================
-- Author:  Raul Adrian Osuna Davizon
-- Create date: 26/07/2023
-- Description:Consulty to register employees
-- EXECUTION: UpdateMoveEmployeeById 1
-- =============================================
AS
BEGIN
    SET NOCOUNT ON;

	IF NOT EXISTS(SELECT TOP 1 ME.Id FROM MoveEmployees ME(NOLOCK) WHERE (DATEPART(MONTH,ME.DateMove)=DATEPART(MONTH,@DateMove)) AND (DATEPART(YEAR,ME.DateMove)=DATEPART(YEAR,@DateMove) ) AND ME.IdEmployee=@IdEmployee AND ME.Status=1 )
	BEGIN
		UPDATE MoveEmployees SET IdEmployee=@IdEmployee, DateMove=@DateMove, Role=@Role, Status=1, Deliver=@Deliver WHERE Id=@Id
		SELECT 'Opreacion realizada con exito' 'Message', 1 'ResponseCode'; 
	END
	ELSE IF EXISTS(SELECT TOP 1 ME.Id FROM MoveEmployees ME(NOLOCK) WHERE (DATEPART(MONTH,ME.DateMove)=DATEPART(MONTH,@DateMove)) AND (DATEPART(YEAR,ME.DateMove)=DATEPART(YEAR,@DateMove) ) AND ME.IdEmployee=@IdEmployee AND ME.Status=1 AND ME.Id=@Id)
	BEGIN
		UPDATE MoveEmployees SET IdEmployee=@IdEmployee, DateMove=@DateMove, Role=@Role, Status=1, Deliver=@Deliver WHERE Id=@Id
		SELECT 'Opreacion realizada con exito' 'Message', 1 'ResponseCode'; 
	END
	ELSE
	BEGIN
		SELECT 'Ya se encuentra un registro en esta mes y año' 'Message', 0 'ResponseCode'; 
	END
	
    SET NOCOUNT OFF;
END
GO
IF EXISTS (SELECT '' FROM SYS.OBJECTS WHERE OBJECT_ID=OBJECT_ID('DeleteEmployeeById') AND type='P')
    DROP PROCEDURE DeleteEmployeeById
GO
CREATE PROCEDURE DeleteEmployeeById
@Id BIGINT
-- =============================================
-- Author:  Raul Adrian Osuna Davizon
-- Create date: 26/07/2023
-- Description:Consulty to Update a employee using Id
-- EXECUTION: DeleteEmployeeById 1
-- =============================================

AS
BEGIN
    SET NOCOUNT ON;
	UPDATE Employees SET Status=0 WHERE Id=@Id
    SELECT 'El registro ha sido dado de baja' 'Message', 1 'ResponseCode';  
    SET NOCOUNT OFF;
END
GO
IF EXISTS (SELECT '' FROM SYS.OBJECTS WHERE OBJECT_ID=OBJECT_ID('DeleteMovesEmployeeById') AND type='P')
    DROP PROCEDURE DeleteMovesEmployeeById
GO
CREATE PROCEDURE DeleteMovesEmployeeById
@Id BIGINT
-- =============================================
-- Author:  Raul Adrian Osuna Davizon
-- Create date: 26/07/2023
-- Description:Consulty to Update a employee using Id
-- EXECUTION: DeleteMovesEmployeeById 1
-- =============================================

AS
BEGIN
    SET NOCOUNT ON;
	UPDATE MoveEmployees SET Status=0 WHERE Id=@Id
    SELECT 'El registro ha sido dado de baja' 'Message', 1 'ResponseCode';  
    SET NOCOUNT OFF;
END

