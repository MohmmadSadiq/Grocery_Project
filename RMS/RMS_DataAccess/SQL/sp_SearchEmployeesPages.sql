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

    IF (@PageNumber < 1)
        SET @PageNumber = 1;

    IF (@PageSize < 1)
        SET @PageSize = 20;

    SET @SearchText = NULLIF(LTRIM(RTRIM(@SearchText)), N'');

    DECLARE @SearchEmployeeID INT = TRY_CONVERT(INT, @SearchText);

    ;WITH FilteredEmployees AS
    (
        SELECT
            pos.PositionName,
            emp.EmployeeID,
            emp.HireDate,
            emp.FireDate,
            peo.FullName,
            cnt.CountryName,
            peo.Gender,
            peo.Phone,
            peo.Email,
            peo.ImagePath
        FROM Employees AS emp
        INNER JOIN People AS peo
            ON emp.PersonID = peo.PersonID
        INNER JOIN Positions AS pos
            ON emp.PositionID = pos.PositionID
        INNER JOIN Countries AS cnt
            ON peo.NationalityCountryID = cnt.CountryID
        WHERE
            (@PositionID IS NULL OR emp.PositionID = @PositionID)
            AND (@CountryID IS NULL OR peo.NationalityCountryID = @CountryID)
            AND
            (
                @SearchText IS NULL
                OR
                (
                    @SearchBy = N'FullName'
                    AND peo.FullName LIKE N'%' + @SearchText + N'%'
                )
                OR
                (
                    @SearchBy = N'EmployeeID'
                    AND @SearchEmployeeID IS NOT NULL
                    AND emp.EmployeeID = @SearchEmployeeID
                )
                OR
                (
                    @SearchBy NOT IN (N'FullName', N'EmployeeID')
                    AND
                    (
                        peo.FullName LIKE N'%' + @SearchText + N'%'
                        OR (@SearchEmployeeID IS NOT NULL AND emp.EmployeeID = @SearchEmployeeID)
                    )
                )
            )
    )
    SELECT @TotalCount = COUNT(*)
    FROM FilteredEmployees;

    SELECT
        PositionName,
        EmployeeID,
        HireDate,
        FireDate,
        FullName,
        CountryName,
        Gender,
        Phone,
        Email,
        ImagePath
    FROM FilteredEmployees
    ORDER BY EmployeeID DESC
    OFFSET ((@PageNumber - 1) * @PageSize) ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GO
