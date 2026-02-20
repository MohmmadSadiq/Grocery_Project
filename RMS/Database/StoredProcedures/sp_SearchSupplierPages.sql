CREATE OR ALTER PROCEDURE sp_SearchSupplierPages
    @SearchText NVARCHAR(100) = NULL,
    @SearchBy   NVARCHAR(50) = 'SupplierName',  -- SupplierName, Phone, Code (SupplierID)
    @SupplierType NVARCHAR(20) = NULL,  -- 'Person', 'Company', NULL for all
    @IsActive   BIT = NULL,
    @PageNumber INT = 1,
    @PageSize   INT = 20,
    @SortBy     NVARCHAR(50) = 'SupplierName' -- SupplierName, Phone, Code, Country
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT 
        SupplierID,
        SupplierType,
        SupplierName,
        Name,
        Phone,
        Email,
        Address,
        Country,
        IsActive,
        CreatedDate,
        UpdatedDate,
        COUNT(*) OVER() AS TotalCount
    FROM Suppliers_View 
    WHERE 
        (@IsActive IS NULL OR IsActive = @IsActive)
        AND (@SupplierType IS NULL OR SupplierType = @SupplierType)
        AND (@SearchText IS NULL OR @SearchText = '' OR (
            (@SearchBy = 'SupplierName' AND SupplierName LIKE '%' + @SearchText + '%')
            OR (@SearchBy = 'Name' AND Name LIKE '%' + @SearchText + '%')
            OR (@SearchBy = 'Phone' AND Phone LIKE '%' + @SearchText + '%')
            OR (@SearchBy = 'Code' AND CAST(SupplierID AS NVARCHAR(20)) LIKE '%' + @SearchText + '%')
        ))
    ORDER BY 
        CASE WHEN @SortBy = 'SupplierID' THEN SupplierID END ASC,
        CASE WHEN @SortBy = 'SupplierName' THEN SupplierName END ASC,
        CASE WHEN @SortBy = 'Phone' THEN Phone END ASC,
        CASE WHEN @SortBy = 'Country' THEN Country END ASC,
        SupplierName ASC  -- Default secondary sort
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END
