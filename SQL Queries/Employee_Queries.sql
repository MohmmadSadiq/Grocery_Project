USE [RMS];
GO

CREATE PROCEDURE spEmployee_AddNew
    @PersonID INT,
    @PositionID INT,
    @HireDate DATE,
    @FireDate DATE,
    @CreatedByUserID INT,
    @NewEmployeeID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Employees (
        PersonID, PositionID, HireDate, FireDate, CreatedByUserID
        , CreatedDate , IsDeleted
    )
    VALUES (
        @PersonID, @PositionID, @HireDate, @FireDate, @CreatedByUserID
        , GETDATE() , 0
    );

    SET @NewEmployeeID = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE spEmployee_GetAll AS
BEGIN
    SET NOCOUNT ON;

    SELECT EmployeeID, PersonID, PositionID, HireDate, FireDate, CreatedByUserID, CreatedDate, UpdatedByUserID, UpdatedDate FROM Employees WHERE IsDeleted = 0;
END
GO

CREATE PROCEDURE spEmployee_GetByID
    @EmployeeID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT EmployeeID, PersonID, PositionID, HireDate, FireDate, CreatedByUserID, CreatedDate, UpdatedByUserID, UpdatedDate FROM Employees WHERE EmployeeID = @EmployeeID AND IsDeleted = 0;
END
GO

CREATE PROCEDURE spEmployee_Update
    @EmployeeID INT,
    @PersonID INT,
    @PositionID INT,
    @HireDate DATE,
    @FireDate DATE,
    @UpdatedByUserID INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Employees SET
        PersonID = @PersonID,
        PositionID = @PositionID,
        HireDate = @HireDate,
        FireDate = @FireDate,
        UpdatedByUserID = @UpdatedByUserID
,
        UpdatedDate = GETDATE()
    WHERE EmployeeID = @EmployeeID;
    IF @@ROWCOUNT > 0 RETURN 1 ELSE RETURN 0
END
GO

CREATE PROCEDURE spEmployee_Delete
    @EmployeeID INT,
    @UpdatedByUserID INT

AS 
BEGIN
    SET NOCOUNT ON;
    DECLARE @IsCompleted INT;

    BEGIN TRY
        BEGIN TRANSACTION

        -- Attempt to delete (Intercepted by Trigger)
        DELETE FROM Employees
        WHERE EmployeeID = @EmployeeID AND IsDeleted != 1

        SET @IsCompleted = @@ROWCOUNT;
        -- If ID didn't exist or was already deleted
        IF @@ROWCOUNT = 0
            THROW 51000, 'No record found to delete', 1;

        -- Update audit info
        UPDATE Employees
        SET UpdatedByUserID = @UpdatedByUserID,
            UpdatedDate = GETDATE()
        WHERE EmployeeID = @EmployeeID

        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
    END CATCH

    IF @IsCompleted > 0 RETURN 1; ELSE RETURN 0;
END
GO

CREATE TRIGGER EmployeeSoftDelete
    ON Employees
    INSTEAD OF DELETE
AS 
BEGIN
    SET NOCOUNT ON;
    UPDATE Employees
    SET IsDeleted = 1
    WHERE EmployeeID IN (SELECT EmployeeID FROM deleted)
      AND IsDeleted != 1
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_SearchEmployeesPages
    @SearchText NVARCHAR(100) = NULL,
    @SearchBy NVARCHAR(50) = N'FullName',
    @PositionID INT = NULL,
    @CountryID INT = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 20,
    @TotalCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- 1.  ÃÂÌ“ «·„⁄«ÌÌ— (Cleaning inputs)
    IF (@PageNumber < 1) SET @PageNumber = 1;
    IF (@PageSize < 1) SET @PageSize = 20;

    SET @SearchText = NULLIF(LTRIM(RTRIM(@SearchText)), N'');
    DECLARE @SearchEmployeeID INT = TRY_CONVERT(INT, @SearchText);

    -- 2. ≈‰‘«¡ «·ÃœÊ· «·„ƒﬁ  Ê ⁄»∆ Â ›Ì ŒÿÊ… Ê«Õœ… »«” Œœ«„ INTO
    -- ”ÌﬁÊ„ SQL Server »≈‰‘«¡ «·ÃœÊ·  ·ﬁ«∆Ì« »‰›” √‰Ê«⁄ »Ì«‰«  «·√⁄„œ… «·„Œ «—…
    SELECT
        pos.PositionName,
        emp.EmployeeID,
        emp.HireDate,
        emp.FireDate,
        peo.FullName,
        cnt.CountryName,
        Gender = CASE Gender
		WHEN 0 Then 'Male'
		WHEN 1 THEN 'Female'
		ELSE 'Unknown'
		END,
        peo.Phone,
        peo.Email,
        peo.ImagePath
    INTO #TempEmployees
    FROM Employees AS emp
    INNER JOIN People AS peo ON emp.PersonID = peo.PersonID
    INNER JOIN Positions AS pos ON emp.PositionID = pos.PositionID
    INNER JOIN Countries AS cnt ON peo.NationalityCountryID = cnt.CountryID
    WHERE
        (@PositionID IS NULL OR emp.PositionID = @PositionID)
        AND (@CountryID IS NULL OR peo.NationalityCountryID = @CountryID)
        AND (
            @SearchText IS NULL
            OR (@SearchBy = N'FullName' AND peo.FullName LIKE N'%' + @SearchText + N'%')
            OR (@SearchBy = N'EmployeeID' AND @SearchEmployeeID IS NOT NULL AND emp.EmployeeID = @SearchEmployeeID)
            OR (
                @SearchBy NOT IN (N'FullName', N'EmployeeID')
                AND (peo.FullName LIKE N'%' + @SearchText + N'%' OR (@SearchEmployeeID IS NOT NULL AND emp.EmployeeID = @SearchEmployeeID))
            )
        );

    -- 3. «” Œ—«Ã «·⁄œœ «·≈Ã„«·Ì „‰ «·ÃœÊ· «·„ƒﬁ  Ê≈”‰«œÂ ··„ €Ì— «·„Œ—Ã
    SELECT @TotalCount = COUNT(*) FROM #TempEmployees;

    -- 4. Ã·» »Ì«‰«  «·’›Õ… «·„ÿ·Ê»… „⁄ «· — Ì»
    SELECT *
    FROM #TempEmployees
    ORDER BY EmployeeID DESC
    OFFSET ((@PageNumber - 1) * @PageSize) ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    -- 5. Õ–› «·ÃœÊ· «·„ƒﬁ  («Œ Ì«—Ì ·√‰ SQL ÌÕ–›Â  ·ﬁ«∆Ì« ⁄‰œ «‰ Â«¡ «·‹ Procedure)
    DROP TABLE #TempEmployees;
END;
GO