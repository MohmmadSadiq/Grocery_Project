/*
================================================================================
 Project   : Retail Management System (RMS)
 Module    : Core, Security, Product Catalog, Inventory, and Finance
 Script    : Database & Core DDL
 Description:
     Creates the RMS database and all core tables, constraints, indexes,
      for a portfolio‑grade RMS schema.

 Author    : Mohammed Sadiq Al-Shaepi
 CreatedOn : 2025-12-29
 Version   : 1.0
 Notes     :
   - Comment out the CREATE DATABASE / USE statements when deploying
     into an existing database or different environment.
================================================================================
*/

-- Create dedicated RMS database for development/demo
CREATE DATABASE RMS;
GO

-- Target RMS2 context for all subsequent objects
USE RMS;
GO

CREATE TABLE Countries (
    CountryID INT IDENTITY(1,1) NOT NULL,
    CountryName NVARCHAR(100) NOT NULL,
    
    CONSTRAINT PK_Countries PRIMARY KEY (CountryID)
);

CREATE TABLE People (
    -- Primary Key (surrogate identifier for a person master record)
    PersonID             INT              IDENTITY(1,1) NOT NULL,

    -- Basic Personal Information
    NationalNo           NVARCHAR(20)     NULL,               -- National/Residency ID Number (used heavily for search and deduplication)
    FirstName            NVARCHAR(50)     NOT NULL,
    SecondName           NVARCHAR(50)     NULL,
    ThirdName            NVARCHAR(50)     NULL,
    LastName             NVARCHAR(50)     NOT NULL,
    
    -- Computed column: simplifies UI and reporting by exposing a single full name
    FullName             AS (CONCAT(FirstName, ' ', SecondName, ' ', ThirdName, ' ', LastName)),

    DateOfBirth          DATE             NULL,
    Gender               TINYINT          NULL,               -- 0 = Male, 1 = Female (extendable via lookup if needed)
    Address              NVARCHAR(500)    NULL,
    Phone                NVARCHAR(20)     NULL,
    Email                NVARCHAR(100)    NULL,
    
    -- Nationality (can later be linked to a Countries lookup table)
    NationalityCountryID INT              NULL, 

    -- Personal Image Path (file system or CDN relative path)
    ImagePath            NVARCHAR(250)    NULL,

    -- Audit Columns (soft-delete pattern preferred over hard deletes)
    CreatedDate          DATETIME         DEFAULT GETDATE(),
    CreatedByUserID      INT              NULL,               -- FK to Users.UserID (who created)
    UpdatedDate          DATETIME         NULL,
    UpdatedByUserID      INT              NULL,
    IsDeleted            BIT              DEFAULT 0,          -- 1 = Logically deleted, 0 = Active

    -- Constraints
    CONSTRAINT PK_People PRIMARY KEY CLUSTERED (PersonID),
	CONSTRAINT FK_People_Countries FOREIGN KEY (NationalityCountryID) REFERENCES Countries(CountryID)
);

-- Indexes (To improve search speed on common search criteria)
-- 1. Unique index for National ID (prevents duplicate person master records)
CREATE UNIQUE NONCLUSTERED INDEX IX_People_NationalNo 
ON People(NationalNo) 
WHERE NationalNo IS NOT NULL; 

-- 2. Index for name search (frequent search on name in UI)
CREATE NONCLUSTERED INDEX IX_People_FullName 
ON People(FirstName, LastName);

-- 3. Index for search by phone number
CREATE NONCLUSTERED INDEX IX_People_Phone 
ON People(Phone);

-- =============================================
-- 2. Create Table: Users
-- =============================================
CREATE TABLE Users (
    UserID          INT              IDENTITY(1,1) NOT NULL,
    PersonID        INT              NOT NULL,

    -- Login Credentials
    UserName        NVARCHAR(50)     NOT NULL,              -- Unique username for application login
    PasswordHash    NVARCHAR(128)    NOT NULL,              -- SHA-512 (or similar) hex-encoded hash
    PasswordSalt    NVARCHAR(128)    NOT NULL,              -- Random salt used for hashing
    IsActive        BIT              DEFAULT 1,             -- 1 = Active, 0 = Disabled (cannot log in)

    -- Audit Information
    CreatedDate     DATETIME         DEFAULT GETDATE(),
    CreatedByUserID INT              NULL,                  -- Self-referencing FK to Users.UserID
    UpdatedDate     DATETIME         DEFAULT GETDATE(),
    UpdatedByUserID INT              NULL,                  -- Self-referencing FK to Users.UserID
    IsDeleted       BIT              DEFAULT 0,

    -- Primary Key Constraint
    CONSTRAINT PK_Users               PRIMARY KEY CLUSTERED (UserID),
    
    -- Unique Constraint on UserName (enforces one account per login name)
    CONSTRAINT UQ_Users_UserName      UNIQUE (UserName),
    
    -- Foreign Key to People (main 1:1 relationship to personal data)
    CONSTRAINT FK_Users_People        FOREIGN KEY (PersonID) REFERENCES People(PersonID)
);

-- Indexes for Users
CREATE NONCLUSTERED INDEX IX_Users_UserName ON Users(UserName);


-- =============================================
-- 3. Add Circular Foreign Keys (Audit Columns)
-- =============================================

-- Add FKs to People Table (Linking to Users)
ALTER TABLE People ADD CONSTRAINT FK_People_CreatedByUser 
FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID);

ALTER TABLE People ADD CONSTRAINT FK_People_UpdatedByUser 
FOREIGN KEY (UpdatedByUserID) REFERENCES Users(UserID);

-- Add FKs to Users Table (Linking to Users)
ALTER TABLE Users ADD CONSTRAINT FK_Users_CreatedByUser 
FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID);

ALTER TABLE Users ADD CONSTRAINT FK_Users_UpdatedByUser 
FOREIGN KEY (UpdatedByUserID) REFERENCES Users(UserID);

/*
 * Script: Create Positions and Employees Tables
 * Description: HR Module tables handling job titles and employee contracts.
 */

-- =============================================
-- 1. Create Table: Positions (Job Titles)
-- =============================================
CREATE TABLE Positions
(
    PositionID      INT IDENTITY(1,1) NOT NULL,   -- Surrogate key for the job/position
    PositionName    NVARCHAR(50)      NOT NULL,   -- Human-readable title (e.g., 'Cashier', 'Store Manager')
    Description     NVARCHAR(MAX)     NULL,       -- Optional long-form description of responsibilities

    -- Audit Information (Consistent with other tables)
    CreatedDate     DATETIME          DEFAULT GETDATE(),  -- When this position definition was created
    CreatedByUserID INT               NULL,               -- FK to Users(UserID) who created the record
    UpdatedDate     DATETIME          DEFAULT GETDATE(),  -- When it was last updated
    UpdatedByUserID INT               NULL,               -- FK to Users(UserID) who last updated the record
    IsDeleted       BIT               DEFAULT 0,          -- Soft delete flag for deactivating positions

    CONSTRAINT PK_Positions PRIMARY KEY CLUSTERED (PositionID)
);

-- =============================================
-- 2. Create Table: Employees
-- =============================================
CREATE TABLE Employees
(
    EmployeeID      INT IDENTITY(1, 1) NOT NULL,  -- Surrogate key for each employment record
    
    -- Link to People (The physical person)
    PersonID        INT               NOT NULL,   -- FK to People(PersonID)
    
    -- Link to Position (The job title)
    PositionID      INT               NOT NULL,   -- FK to Positions(PositionID)
    
    -- Employment Details
    HireDate        DATE              NOT NULL,   -- When the employee joined; DATE is sufficient
    FireDate        DATE              NULL,       -- Filled only when employment ends (termination/resignation)
    
    -- Audit Information
    -- Note: Here NOT NULL is acceptable IF you ensure an Admin User exists first.
    CreatedByUserID INT               NOT NULL,   -- User who created the employee record
    CreatedDate     DATETIME          DEFAULT GETDATE(),
    UpdatedByUserID INT               NOT NULL,   -- User who last updated the employee record
    UpdatedDate     DATETIME          DEFAULT GETDATE(),
    IsDeleted       BIT               DEFAULT 0,  -- Soft delete: 1 = inactive/archived employee

    -- Constraints
    CONSTRAINT PK_Employees PRIMARY KEY CLUSTERED (EmployeeID),
    
    -- 1:1 Relationship: A person can only have one active employee record
    CONSTRAINT UQ_Employees_PersonID UNIQUE (PersonID),

    -- Foreign Keys
    CONSTRAINT FK_Employees_People          FOREIGN KEY (PersonID)        REFERENCES People(PersonID),
    CONSTRAINT FK_Employees_Positions       FOREIGN KEY (PositionID)      REFERENCES Positions(PositionID),
    CONSTRAINT FK_Employees_CreatedByUserID FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID),
    CONSTRAINT FK_Employees_UpdatedByUserID FOREIGN KEY (UpdatedByUserID) REFERENCES Users(UserID)
);

-- Indexes
CREATE NONCLUSTERED INDEX IX_Employees_PositionID ON Employees(PositionID);


/*
 * Script: Create Security & Access Control Tables (RBAC)
 * Description: Defines Roles, Permissions, and their mapping to Users.
 * Architecture: User <-> UserRoles <-> Roles <-> RolePermissions <-> Permissions
 */

-- =============================================
-- 1. Create Table: Roles
-- =============================================
CREATE TABLE Roles (
    RoleID          INT IDENTITY(1,1) NOT NULL,   -- Surrogate key for each security role
    RoleName        NVARCHAR(50)      NOT NULL,   -- Unique role name (e.g., 'Admin', 'Cashier')
    Description     NVARCHAR(MAX)     NULL,       -- Optional description of permissions and purpose

    -- Audit Information
    CreatedDate     DATETIME          DEFAULT GETDATE(),
    CreatedByUserID INT               NULL,       -- FK to Users(UserID) – who created the role
    UpdatedDate     DATETIME          DEFAULT GETDATE(),
    UpdatedByUserID INT               NULL,       -- FK to Users(UserID) – who last updated the role
    IsDeleted       BIT               DEFAULT 0,  -- Soft delete flag for deactivating roles

    CONSTRAINT PK_Roles PRIMARY KEY CLUSTERED (RoleID),
    CONSTRAINT UQ_Roles_RoleName UNIQUE (RoleName), -- Role names must be unique (e.g., 'Admin', 'Cashier')
    
    -- Foreign Keys for Audit
    CONSTRAINT FK_Roles_CreatedByUser FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID),
    CONSTRAINT FK_Roles_UpdatedByUser FOREIGN KEY (UpdatedByUserID) REFERENCES Users(UserID)
);

-- Index for searching roles by name
CREATE NONCLUSTERED INDEX IX_Roles_RoleName ON Roles(RoleName);

-- =============================================
-- 2. Create Table: Permissions
-- =============================================
CREATE TABLE Permissions (
    PermissionID   INT IDENTITY(1,1) NOT NULL,    -- Surrogate key for each permission
    PermissionName NVARCHAR(100)     NOT NULL,    -- System name e.g., 'Transaction.Create'
    Description    NVARCHAR(MAX)     NULL,        -- Human readable description of what the permission allows

    -- Audit Information
    CreatedDate    DATETIME          DEFAULT GETDATE(),
    CreatedByUserID INT              NULL,        -- FK to Users(UserID) who created the permission
    UpdatedDate    DATETIME          DEFAULT GETDATE(),
    UpdatedByUserID INT              NULL,        -- FK to Users(UserID) who last updated the permission
    IsDeleted      BIT               DEFAULT 0,

    CONSTRAINT PK_Permissions PRIMARY KEY CLUSTERED (PermissionID),
    CONSTRAINT UQ_Permissions_PermissionName UNIQUE (PermissionName),
    
    -- Foreign Keys for Audit
    CONSTRAINT FK_Permissions_CreatedByUser FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID),
    CONSTRAINT FK_Permissions_UpdatedByUser FOREIGN KEY (UpdatedByUserID) REFERENCES Users(UserID)
);

-- Index for searching permissions
CREATE NONCLUSTERED INDEX IX_Permissions_PermissionName ON Permissions(PermissionName);

-- =============================================
-- 3. Create Table: UserRoles (Junction Table)
-- Description: Links Users to Roles (Many-to-Many)
-- =============================================
CREATE TABLE UserRoles (
    UserRoleID INT IDENTITY(1,1) NOT NULL, -- Optional surrogate key, useful for ORMs
    UserID INT NOT NULL,
    RoleID INT NOT NULL,

    -- Audit Information (Simplified for junction tables)
    CreatedDate DATETIME DEFAULT GETDATE(),
    CreatedByUserID INT NULL,

    -- Composite Unique Constraint: A user cannot have the same role twice
    CONSTRAINT PK_UserRoles PRIMARY KEY CLUSTERED (UserRoleID),
    CONSTRAINT UQ_UserRoles_User_Role UNIQUE (UserID, RoleID),

    -- Foreign Keys
    CONSTRAINT FK_UserRoles_Users FOREIGN KEY (UserID) REFERENCES Users(UserID),
    CONSTRAINT FK_UserRoles_Roles FOREIGN KEY (RoleID) REFERENCES Roles(RoleID),
    CONSTRAINT FK_UserRoles_CreatedByUser FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID)
);

-- Index to quickly find all roles for a specific user (Critical for Login)
CREATE NONCLUSTERED INDEX IX_UserRoles_UserID ON UserRoles(UserID);

-- =============================================
-- 4. Create Table: RolePermissions (Junction Table)
-- Description: Links Roles to Permissions (Many-to-Many)
-- =============================================
CREATE TABLE RolePermissions (
    RolePermissionID INT IDENTITY(1,1) NOT NULL,
    RoleID INT NOT NULL,
    PermissionID INT NOT NULL,

    -- Audit Information
    CreatedDate DATETIME DEFAULT GETDATE(),
    CreatedByUserID INT NULL,

    -- Composite Unique Constraint: A role cannot have the same permission twice
    CONSTRAINT PK_RolePermissions PRIMARY KEY CLUSTERED (RolePermissionID),
    CONSTRAINT UQ_RolePermissions_Role_Permission UNIQUE (RoleID, PermissionID),

    -- Foreign Keys
    CONSTRAINT FK_RolePermissions_Roles FOREIGN KEY (RoleID) REFERENCES Roles(RoleID),
    CONSTRAINT FK_RolePermissions_Permissions FOREIGN KEY (PermissionID) REFERENCES Permissions(PermissionID),
    CONSTRAINT FK_RolePermissions_CreatedByUser FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID)
);

-- Index to quickly load all permissions for a role
CREATE NONCLUSTERED INDEX IX_RolePermissions_RoleID ON RolePermissions(RoleID);


 /*
 * Script: Product Catalog & Supply Chain Master Data
 * Description: Defines Companies, Brands, Categories, and Products based on the updated diagram.
 * Hierarchy: Companies -> Brands -> Products
 */

-- =============================================
-- 1. Table: Categories
-- Description: Product families (e.g., Dairy, Electronics).
-- =============================================
CREATE TABLE Categories (
    CategoryID    INT IDENTITY(1,1) NOT NULL,  -- Surrogate key for the product category
    CategoryName  NVARCHAR(100)     NOT NULL,  -- Name shown in UI (e.g., 'Dairy', 'Electronics')
    Description   NVARCHAR(MAX)     NULL,      -- Optional description for reporting / documentation

    -- Audit
    CreatedDate   DATETIME          DEFAULT GETDATE(),
    CreatedByUserID INT             NULL,      -- FK to Users(UserID) who created the category
    IsDeleted     BIT               DEFAULT 0, -- Soft delete support (hide from UI but keep data)

    CONSTRAINT PK_Categories PRIMARY KEY CLUSTERED (CategoryID),
    CONSTRAINT FK_Categories_CreatedBy FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID)
);

CREATE NONCLUSTERED INDEX IX_Categories_CategoryName ON Categories(CategoryName);


-- =============================================
-- 2. Table: Companies
-- Description: Manufacturers or Suppliers (The parent organization).
-- Example: 'Unilever', 'Procter & Gamble'.
-- =============================================
CREATE TABLE Companies (
    CompanyID        INT IDENTITY(1,1) NOT NULL,  -- Surrogate key for the company entity
    CompanyName      NVARCHAR(150)     NOT NULL,  -- Legal or trade name of the company
    
    -- Contact Info
    ContactPersonID  INT               NULL,      -- Links to People table (the primary representative)
    Phone            NVARCHAR(20)      NULL,
    Email            NVARCHAR(100)     NULL,
    Address          NVARCHAR(500)     NULL,
    CountryID        INT               NULL,      -- Ideally links to a Countries table
    CommercialNumber NVARCHAR(50)      NULL,      -- Tax ID / commercial registration number

    -- Audit
    CreatedDate      DATETIME          DEFAULT GETDATE(),
    CreatedByUserID  INT               NULL,      -- FK to Users(UserID) who created the record
    UpdatedDate      DATETIME          DEFAULT GETDATE(),
    UpdatedByUserID  INT               NULL,      -- FK to Users(UserID) who last updated the record
    IsDeleted        BIT               DEFAULT 0, -- Soft delete flag for inactive companies

    CONSTRAINT PK_Companies PRIMARY KEY CLUSTERED (CompanyID),
    
    -- Foreign Keys
    CONSTRAINT FK_Companies_People    FOREIGN KEY (ContactPersonID) REFERENCES People(PersonID),
    CONSTRAINT FK_Companies_CreatedBy FOREIGN KEY (CreatedByUserID)   REFERENCES Users(UserID),
    CONSTRAINT FK_Companies_UpdatedBy FOREIGN KEY (UpdatedByUserID)   REFERENCES Users(UserID)
);

CREATE NONCLUSTERED INDEX IX_Companies_CompanyName ON Companies(CompanyName);


-- =============================================
-- 3. Table: Brands
-- Description: Specific brands owned by companies.
-- Example: Company 'Unilever' owns Brand 'Dove' and Brand 'Lipton'.
-- =============================================
CREATE TABLE Brands (
    BrandID INT IDENTITY(1,1) NOT NULL,
    BrandName NVARCHAR(100) NOT NULL, -- Changed 'Name' to 'BrandName' for clarity
    CompanyID INT NULL,               -- Link to the parent Company
    Description NVARCHAR(MAX) NULL,

    -- Audit
    CreatedDate DATETIME DEFAULT GETDATE(),
    CreatedByUserID INT NULL,
    IsDeleted BIT DEFAULT 0,

    CONSTRAINT PK_Brands PRIMARY KEY CLUSTERED (BrandID),
    
    -- Foreign Keys
    CONSTRAINT FK_Brands_Companies FOREIGN KEY (CompanyID) REFERENCES Companies(CompanyID),
    CONSTRAINT FK_Brands_CreatedBy FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID)
);

CREATE NONCLUSTERED INDEX IX_Brands_BrandName ON Brands(BrandName);
CREATE NONCLUSTERED INDEX IX_Brands_CompanyID ON Brands(CompanyID);


-- =============================================
-- 4. Table: Products
-- Description: The main product definition table.
-- =============================================
CREATE TABLE Products (
    ProductID     INT IDENTITY(1,1) NOT NULL,    -- Surrogate key for the product
    ProductName   NVARCHAR(200)     NOT NULL,    -- Name displayed on screens and reports
    
    -- Classification
    CategoryID    INT               NULL,        -- FK to Categories(CategoryID)
    BrandID       INT               NULL,        -- FK to Brands(BrandID)
    
    Description   NVARCHAR(MAX)     NULL,        -- Optional long description / marketing text
    IsActive      BIT               DEFAULT 1,   -- 1: Active, 0: Discontinued
    ReorderLevel  INT               DEFAULT 0,   -- Minimum stock level before reordering
    
    -- Audit
    CreatedDate   DATETIME          DEFAULT GETDATE(),
    CreatedByUserID INT             NULL,
    UpdatedDate   DATETIME          DEFAULT GETDATE(),
    UpdatedByUserID INT             NULL,
    IsDeleted     BIT               DEFAULT 0,

    CONSTRAINT PK_Products PRIMARY KEY CLUSTERED (ProductID),
    
    -- Foreign Keys
    CONSTRAINT FK_Products_Categories FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID),
    CONSTRAINT FK_Products_Brands     FOREIGN KEY (BrandID)    REFERENCES Brands(BrandID),
    
    -- Audit FKs
    CONSTRAINT FK_Products_CreatedBy  FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID),
    CONSTRAINT FK_Products_UpdatedBy  FOREIGN KEY (UpdatedByUserID) REFERENCES Users(UserID)
);

CREATE NONCLUSTERED INDEX IX_Products_ProductName ON Products(ProductName);
CREATE NONCLUSTERED INDEX IX_Products_BrandID ON Products(BrandID);

-- =============================================
-- 1. Table: Units
-- =============================================
CREATE TABLE Units (
    UnitID        INT IDENTITY(1,1) NOT NULL,  -- Surrogate key for the unit of measure
    UnitName      NVARCHAR(50)      NOT NULL,  -- Name (e.g., 'Piece', 'Box', 'Carton')
    Description   NVARCHAR (MAX)    NULL,      -- Optional explanation / conversion notes
	IsActive      BIT               DEFAULT 1, -- 1 = Active, 0 = no longer used for new products

    -- Audit
    CreatedDate   DATETIME          DEFAULT GETDATE(),
    CreatedByUserID INT             NULL,      -- FK to Users(UserID) who created the unit
    IsDeleted     BIT               DEFAULT 0, -- Soft delete support

    CONSTRAINT PK_Units PRIMARY KEY CLUSTERED (UnitID),
    
    -- Added FK for Audit
    CONSTRAINT FK_Units_CreatedBy FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID)
);

-- =============================================
-- 2. Table: ProductUnits
-- =============================================
CREATE TABLE ProductUnits (
    ProductUnitID    INT IDENTITY(1,1) NOT NULL, -- Surrogate key for the specific product+unit combination
    ProductID        INT              NOT NULL,  -- FK to Products(ProductID)
    UnitID           INT              NOT NULL,  -- FK to Units(UnitID)
    
	Description      NVARCHAR (MAX)   NULL,      -- Optional description (e.g., 'Box of 12 pieces')
    ConversionFactor DECIMAL(18, 4)  NOT NULL DEFAULT 1, -- How many base units this unit represents
    SalePrice        DECIMAL(18, 2)  NULL,      -- Retail price per this unit; DECIMAL(18,2) preferred over MONEY to avoid rounding issues
    Barcode          NVARCHAR(50)    NULL,      -- Barcode printed on packaging for POS scanning
    IsActive         BIT             DEFAULT 0, -- 1 = Currently sold, 0 = discontinued for sale

    -- Audit
    CreatedDate      DATETIME        DEFAULT GETDATE(),
    CreatedByUserID  INT             NULL,
    UpdatedDate      DATETIME        DEFAULT GETDATE(),
    UpdatedByUserID  INT             NULL,
    IsDeleted        BIT             DEFAULT 0,

    CONSTRAINT PK_ProductUnits PRIMARY KEY CLUSTERED (ProductUnitID),
    CONSTRAINT FK_ProductUnits_Units    FOREIGN KEY (UnitID)   REFERENCES Units(UnitID),
    CONSTRAINT FK_ProductUnits_ProductID FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    
    -- Added FKs for Audit
    CONSTRAINT FK_ProductUnits_CreatedBy FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID),
    CONSTRAINT FK_ProductUnits_UpdatedBy FOREIGN KEY (UpdatedByUserID) REFERENCES Users(UserID)
);

CREATE NONCLUSTERED INDEX IX_ProductUnits_Barcode ON ProductUnits(Barcode);

-- =============================================
-- 3. Table: Transactions (Financial Ledger)
-- =============================================
CREATE TABLE Transactions (
    TransactionID     INT IDENTITY(1,1) NOT NULL, -- Surrogate key for each financial transaction header
	PaymentID         INT              NULL,      -- Optional FK to Payments(PaymentID) when generated from a payment
    TransactionDate   DATETIME         DEFAULT GETDATE(), -- When the transaction occurred
    TransactionType   TINYINT          NOT NULL,  -- Business type (e.g., 1=Purchase, 2=Sale, 3=Adjustment)
    TransactionStatus TINYINT          NOT NULL,  -- 0=Draft, 1=Posted, 2=Cancelled (extendable)
    TotalAmount       DECIMAL(18, 2)   NOT NULL DEFAULT 0, -- Document total; DECIMAL preferred over MONEY for precision
	Nots              NVARCHAR (MAX)   NULL,      -- Free-form notes / internal comments

    -- Audit
    CreatedDate       DATETIME         DEFAULT GETDATE(),
    CreatedByUserID   INT              NOT NULL,  -- User who created the transaction
    UpdatedDate       DATETIME         DEFAULT GETDATE(),
    UpdatedByUserID   INT              NULL,      -- User who last updated the transaction
    IsDeleted         BIT              DEFAULT 0, -- Soft delete flag

    CONSTRAINT PK_Transactions PRIMARY KEY CLUSTERED (TransactionID),

    -- Added FK for Audit (others kept for backward compatibility with original design)
    CONSTRAINT FK_Transactions_CreatedBy FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID),
	CONSTRAINT FK_Purchases_CreatedBy   FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID),
    CONSTRAINT FK_Purchases_UpdatedBy   FOREIGN KEY (UpdatedByUserID) REFERENCES Users(UserID)
);

-- =============================================
-- 4. Table: Purchases (Inflow Header)
-- =============================================
CREATE TABLE Purchases (
    PurchaseID            INT IDENTITY(1,1) NOT NULL, -- Surrogate key for the purchase header
    TransactionID         INT               NULL,     -- FK to Transactions(TransactionID)
    SupplierID            INT               NULL,     -- FK to Suppliers(SupplierID)
    InvoiceNumber         NVARCHAR(50)      NULL,     -- Supplier invoice reference
	PurchasedByEmployeeID INT               NOT NULL, -- FK to Employees(EmployeeID) who made the purchase
    
    -- Audit (optional: could be extended later for full Created/Updated tracking)

    CONSTRAINT PK_Purchases PRIMARY KEY CLUSTERED (PurchaseID),
    CONSTRAINT FK_Purchases_Transactions FOREIGN KEY (TransactionID) REFERENCES Transactions(TransactionID)
    -- Additional audit FKs can be added here if audit columns are introduced
);

-- =============================================
-- 5. Table: PurchaseProductBatches (Inventory)
-- =============================================
CREATE TABLE PurchaseProductBatches (
    BatchID        INT IDENTITY(1,1) NOT NULL,  -- Surrogate key for the purchase batch
    PurchaseID     INT              NOT NULL,   -- FK to Purchases(PurchaseID)
    ProductUnitID  INT              NOT NULL,   -- FK to ProductUnits(ProductUnitID)
    
    TotalQuantity  DECIMAL(18, 4)   NOT NULL,   -- Quantity brought into stock for this batch
    UniteCostPrice DECIMAL(18, 2)   NOT NULL,   -- Cost per unit for this batch
    TotalCostPrice AS (TotalQuantity * UniteCostPrice), -- Computed total cost
    ProductionDate DATE             NULL,       -- Manufacturing/production date if applicable
    ExpiryDate     DATE             NULL,       -- Expiry date for perishable items
    BatchNumber    NVARCHAR(50)     NULL,       -- Supplier or internal batch number

    CONSTRAINT PK_PurchaseProductBatches PRIMARY KEY CLUSTERED (BatchID),
    CONSTRAINT FK_Batches_Purchases    FOREIGN KEY (PurchaseID)    REFERENCES Purchases(PurchaseID),
    CONSTRAINT FK_Batches_ProductUnits FOREIGN KEY (ProductUnitID) REFERENCES ProductUnits(ProductUnitID)
);

CREATE NONCLUSTERED INDEX IX_Batches_ExpiryDate ON PurchaseProductBatches(ExpiryDate);

-- =============================================
-- 6. Table: Sales (Outflow Header)
-- =============================================
CREATE TABLE Sales (
    SaleID        INT IDENTITY(1,1) NOT NULL,  -- Surrogate key for the sales header
    TransactionID INT              NULL,       -- FK to Transactions(TransactionID)
    CustomerID    INT              NULL,       -- Optional FK to Customers(CustomerID) for named customer sales
    
    CONSTRAINT PK_Sales PRIMARY KEY CLUSTERED (SaleID),
    CONSTRAINT FK_Sales_Transactions FOREIGN KEY (TransactionID) REFERENCES Transactions(TransactionID)
    -- Additional audit FKs can be added if header-level audit columns are introduced
);

-- =============================================
-- 7. Table: ProductSales (Outflow Details)
-- =============================================
CREATE TABLE ProductSales (
    ProductSaleID INT IDENTITY(1,1) NOT NULL,  -- Surrogate key for each sales line item
    SaleID        INT              NOT NULL,   -- FK to Sales(SaleID)
    ProductUnitID INT              NOT NULL,   -- FK to ProductUnits(ProductUnitID)
    Quantity      DECIMAL(18, 4)   NOT NULL,   -- Quantity sold (in the selected unit)
    UnitPrice     DECIMAL(18, 2)   NOT NULL,   -- Selling price per unit
    TotalPrice    AS (Quantity * UnitPrice),   -- Computed line total
    
    CONSTRAINT PK_ProductSales PRIMARY KEY CLUSTERED (ProductSaleID),
    CONSTRAINT FK_ProductSales_Sales       FOREIGN KEY (SaleID)        REFERENCES Sales(SaleID),
    CONSTRAINT FK_ProductSales_ProductUnits FOREIGN KEY (ProductUnitID) REFERENCES ProductUnits(ProductUnitID)
);



/*
 * Script: Financial & Accounting Module
 * Description: Defines the Chart of Accounts, Customers/Suppliers (Sub-ledgers), 
 * Payments, and the General Ledger (Double-Entry System).
 */

-- =============================================
-- 1. Table: Accounts (Chart of Accounts - COA)
-- Description: The heart of accounting. Contains Assets, Liabilities, Equity, Revenue, Expenses.
-- =============================================
CREATE TABLE Accounts (
    AccountID      INT IDENTITY(1,1) NOT NULL,   -- Surrogate key for the GL account
    AccountName    NVARCHAR(100)     NOT NULL,   -- Name shown in financial reports
    AccountCode    NVARCHAR(20)      NOT NULL,   -- Structured code, e.g., '1101' for Cash
    SubCategoryID  INT               NOT NULL,   -- FK to AccountSubCategories(SubCategoryID)
    
    Description    NVARCHAR(MAX)     NULL,       -- Optional narrative description
    IsActive       BIT               DEFAULT 1,  -- 1 = can still be posted to; 0 = closed
    IsSystemAccount BIT              DEFAULT 0,  -- 1 = protected system account (cannot be deleted)
    
    -- Audit
    CreatedDate    DATETIME          DEFAULT GETDATE(),
    CreatedByUserID INT              NULL,
    UpdatedDate    DATETIME          DEFAULT GETDATE(),
    UpdatedByUserID INT              NULL,

    CONSTRAINT PK_Accounts PRIMARY KEY CLUSTERED (AccountID),
    CONSTRAINT UQ_Accounts_AccountCode UNIQUE (AccountCode),
    CONSTRAINT FK_Accounts_CreatedBy FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID),
    CONSTRAINT FK_Accounts_UpdatedBy FOREIGN KEY (UpdatedByUserID) REFERENCES Users(UserID)
);

CREATE NONCLUSTERED INDEX IX_Accounts_AccountCode ON Accounts(AccountCode);


-- =============================================
-- 2. Table: PaymentMethods
-- Description: Cash, Credit Card, Bank Transfer, etc.
-- =============================================
CREATE TABLE PaymentMethods (
    PaymentMethodID INT IDENTITY(1,1) NOT NULL,
    MethodName NVARCHAR(50) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    
    IsActiveForSales BIT DEFAULT 1,     -- Can be used in POS?
    IsActiveForPurchases BIT DEFAULT 1, -- Can be used to pay suppliers?

    CONSTRAINT PK_PaymentMethods PRIMARY KEY CLUSTERED (PaymentMethodID)
);


-- =============================================
-- 3. Table: Customers (Receivables)
-- Description: Links a Person or Company to a Financial Account (Sub-ledger).
-- =============================================
CREATE TABLE Customers (
    CustomerID   INT IDENTITY(1,1) NOT NULL, -- Surrogate key for the customer master
    
    -- Identity Links (Can be a Person OR a Company)
    PersonID     INT              NULL,      -- FK to People(PersonID) for individual customers
    CompanyID    INT              NULL,      -- FK to Companies(CompanyID) for B2B customers
    
    -- Financial Link
    AccountID    INT              NULL,      -- The AR Account for this customer in GL
    
    IsActive     BIT              DEFAULT 1, -- 1 = Active, 0 = no longer traded with
    
    -- Audit
    CreatedDate  DATETIME         DEFAULT GETDATE(),
    CreatedByUserID INT           NULL,
    UpdatedDate  DATETIME         DEFAULT GETDATE(),
    UpdatedByUserID INT           NULL,

    CONSTRAINT PK_Customers PRIMARY KEY CLUSTERED (CustomerID),
    CONSTRAINT FK_Customers_People    FOREIGN KEY (PersonID)  REFERENCES People(PersonID),
    CONSTRAINT FK_Customers_Companies FOREIGN KEY (CompanyID) REFERENCES Companies(CompanyID),
    CONSTRAINT FK_Customers_Accounts  FOREIGN KEY (AccountID) REFERENCES Accounts(AccountID),
    CONSTRAINT FK_Customers_CreatedBy FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID),
    CONSTRAINT FK_Customers_UpdatedBy FOREIGN KEY (UpdatedByUserID) REFERENCES Users(UserID)
);


-- =============================================
-- 4. Table: Suppliers (Payables)
-- Description: Vendors we buy from.
-- =============================================
CREATE TABLE Suppliers (
    SupplierID   INT IDENTITY(1,1) NOT NULL, -- Surrogate key for the supplier master
    
    -- Identity Links
    PersonID     INT              NULL,      -- FK to People(PersonID) for individual suppliers
    CompanyID    INT              NULL,      -- FK to Companies(CompanyID) for corporate vendors
    
    -- Financial Link
    AccountID    INT              NULL,      -- The AP Account for this supplier in GL
    
    IsActive     BIT              DEFAULT 1, -- 1 = Active, 0 = do not use for new purchases

    -- Audit
    CreatedDate  DATETIME         DEFAULT GETDATE(),
    CreatedByUserID INT           NULL,
    UpdatedDate  DATETIME         DEFAULT GETDATE(),
    UpdatedByUserID INT           NULL,

    CONSTRAINT PK_Suppliers PRIMARY KEY CLUSTERED (SupplierID),
    CONSTRAINT FK_Suppliers_People     FOREIGN KEY (PersonID)  REFERENCES People(PersonID),
    CONSTRAINT FK_Suppliers_Companies  FOREIGN KEY (CompanyID) REFERENCES Companies(CompanyID),
    CONSTRAINT FK_Suppliers_Accounts   FOREIGN KEY (AccountID) REFERENCES Accounts(AccountID),
    CONSTRAINT FK_Suppliers_CreatedBy  FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID),
    CONSTRAINT FK_Suppliers_UpdatedBy  FOREIGN KEY (UpdatedByUserID) REFERENCES Users(UserID)
);


-- =============================================
-- 5. Table: Payments
-- Description: Actual movement of money (Receipts or Disbursements).
-- =============================================
CREATE TABLE Payments (
    PaymentID       INT IDENTITY(1,1) NOT NULL, -- Surrogate key for each payment
    PaymentDate     DATETIME          DEFAULT GETDATE(), -- When the payment was recorded
    
    PaymentMethodID INT               NOT NULL, -- FK to PaymentMethods(PaymentMethodID)
    PaymentAmount   DECIMAL(18, 2)    NOT NULL, -- Amount paid; DECIMAL avoids MONEY rounding issues
    
    -- Direction: 1=In (Receipt), 2=Out (Disbursement)
    PaymentType     TINYINT           NOT NULL, -- 1 = Customer receipt, 2 = Supplier / expense disbursement
    
    Notes           NVARCHAR(MAX)     NULL,     -- Optional free-form explanation
    
    -- Audit
    CreatedDate     DATETIME          DEFAULT GETDATE(),
    CreatedByUserID INT               NULL,
    UpdatedDate     DATETIME          DEFAULT GETDATE(),
    UpdatedByUserID INT               NULL,

    CONSTRAINT PK_Payments PRIMARY KEY CLUSTERED (PaymentID),
    CONSTRAINT FK_Payments_Methods   FOREIGN KEY (PaymentMethodID) REFERENCES PaymentMethods(PaymentMethodID),
    CONSTRAINT FK_Payments_CreatedBy FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID),
    CONSTRAINT FK_Payments_UpdatedBy FOREIGN KEY (UpdatedByUserID) REFERENCES Users(UserID)
);


-- =============================================
-- 6. Table: GeneralJournals (GL Header)
-- Description: The "Day Book". Every financial event becomes a Journal Entry.
-- =============================================
CREATE TABLE GeneralJournals (
    JournalID INT IDENTITY(1,1) NOT NULL,
    JournalDate DATETIME NOT NULL,
    Description NVARCHAR(MAX) NULL,
    
    -- Links to source documents
    TransactionID INT NULL, -- Link to Sales/Purchases Header
    PaymentID INT NULL,     -- Link to Payments if this is a payment entry
    
    -- Audit
    CreatedDate DATETIME DEFAULT GETDATE(),
    CreatedByUserID INT NULL,
    UpdatedDate DATETIME DEFAULT GETDATE(),
    UpdatedByUserID INT NULL,

    CONSTRAINT PK_GeneralJournals PRIMARY KEY CLUSTERED (JournalID),
    CONSTRAINT FK_Journals_Transactions FOREIGN KEY (TransactionID) REFERENCES Transactions(TransactionID),
    CONSTRAINT FK_Journals_Payments FOREIGN KEY (PaymentID) REFERENCES Payments(PaymentID),
    CONSTRAINT FK_Journals_CreatedBy FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID),
    CONSTRAINT FK_Journals_UpdatedBy FOREIGN KEY (UpdatedByUserID) REFERENCES Users(UserID)
);


-- =============================================
-- 7. Table: LedgerEntries (GL Details)
-- Description: The Double-Entry Lines (Debit/Credit). 
-- RULE: Sum(Debit) must equal Sum(Credit) for each JournalID.
-- =============================================
CREATE TABLE LedgerEntries (
    EntryID INT IDENTITY(1,1) NOT NULL,
    JournalID INT NOT NULL,
    AccountID INT NOT NULL, -- Which account is affected?
    
    EntryDate DATETIME NOT NULL, -- Usually same as JournalDate
    
    DebitAmount DECIMAL(18, 2) NOT NULL DEFAULT 0,
    CreditAmount DECIMAL(18, 2) NOT NULL DEFAULT 0,

    CONSTRAINT PK_LedgerEntries PRIMARY KEY CLUSTERED (EntryID),
    CONSTRAINT FK_Entries_Journals FOREIGN KEY (JournalID) REFERENCES GeneralJournals(JournalID),
    CONSTRAINT FK_Entries_Accounts FOREIGN KEY (AccountID) REFERENCES Accounts(AccountID)
);

-- Index for generating Account Statements (Ledger Report)
CREATE NONCLUSTERED INDEX IX_LedgerEntries_AccountID ON LedgerEntries(AccountID) INCLUDE (DebitAmount, CreditAmount);


/*
 * Script: Simplified Chart of Accounts Structure
 * Hierarchy: AccountCategories -> AccountSubCategories -> Accounts
 * Update: NormalBalance is now defined at the Category level.
 */

-- =============================================
-- 1. Table: AccountCategories
-- Description: Major grouping (e.g., Current Assets, Equity, Operating Expenses).
-- Holds the 'NormalBalance' rule (Debit/Credit).
-- =============================================
CREATE TABLE AccountCategories (
    CategoryID INT IDENTITY(1,1) NOT NULL,
    CategoryName NVARCHAR(100) NOT NULL,
    
    -- Moved here from the deleted BalanceNatures table
    NormalBalance BIT NOT NULL ,  -- 0. Debit 1. Credit 
    
    Description NVARCHAR(MAX) NULL,

    CONSTRAINT PK_AccountCategories PRIMARY KEY CLUSTERED (CategoryID)
);

-- Index for fast filtering by Balance Type (Give me all Assets?)
CREATE NONCLUSTERED INDEX IX_AccountCategories_NormalBalance ON AccountCategories(NormalBalance);


-- =============================================
-- 2. Table: AccountSubCategories
-- Description: Granular grouping (e.g., Cash, Trade Payables).
-- Linked to AccountCategories.
-- =============================================
CREATE TABLE AccountSubCategories (
    SubCategoryID INT IDENTITY(1,1) NOT NULL,
    SubCategoryName NVARCHAR(100) NOT NULL,
    
    -- Foreign Key to the Parent Category
    AccountCategoryID INT NOT NULL, 
    
    Description NVARCHAR(MAX) NULL,

    CONSTRAINT PK_AccountSubCategories PRIMARY KEY CLUSTERED (SubCategoryID),
    CONSTRAINT FK_SubCategories_Categories FOREIGN KEY (AccountCategoryID) REFERENCES AccountCategories(CategoryID)
);

-- Index for performance
CREATE NONCLUSTERED INDEX IX_SubCategories_CategoryID ON AccountSubCategories(AccountCategoryID);

-- Seed Data: Let's insert the standard 5 accounting natures immediately.
-- This saves you time and ensures standardization.
INSERT INTO AccountCategories (CategoryName, NormalBalance) VALUES 
('Assets',      0), 
('Liabilities', 1), 
('Equity',      1), 
('Revenue',     1), 
('Expenses',    0); 

/*
 * =================================================================================
 * SCRIPT: Payment Allocations (Many-to-Many Link)
 * DESCRIPTION: Links Payments to Transactions.
 * Logic: Allows one payment to cover multiple invoices, or one invoice to be paid by multiple payments.
 * =================================================================================
 */

-- =============================================
-- 1. Table: PaymentAllocations
-- Description: The Bridge Table. It maps "Money" (Payment) to "Debt" (Transaction).
-- =============================================
CREATE TABLE PaymentAllocations (
    AllocationID INT IDENTITY(1,1) NOT NULL,
    
    PaymentID INT NOT NULL,     -- Source of funds (payment)
    TransactionID INT NOT NULL,  -- Debt paid (invoice)
    
    --The amount allocated specifically for this invoice from this payment
    Amount DECIMAL(18, 2) NOT NULL, 
    
    CreatedDate DATETIME DEFAULT GETDATE(),
    CreatedByUserID INT NULL,

    CONSTRAINT PK_PaymentAllocations PRIMARY KEY CLUSTERED (AllocationID),
    
    -- Foreign Keys
    CONSTRAINT FK_Allocations_Payments FOREIGN KEY (PaymentID) REFERENCES Payments(PaymentID),
    CONSTRAINT FK_Allocations_Transactions FOREIGN KEY (TransactionID) REFERENCES Transactions(TransactionID)
);

-- =============================================
-- 2. Indexes for Performance
-- Description: These are crucial because you will query this table heavily from both sides.
-- =============================================

-- A. To find "What invoices did this payment cover?"
CREATE NONCLUSTERED INDEX IX_PaymentAllocations_PaymentID 
ON PaymentAllocations(PaymentID);

-- B. To find "What payments covered this invoice?" (Checking invoice status)
CREATE NONCLUSTERED INDEX IX_PaymentAllocations_TransactionID 
ON PaymentAllocations(TransactionID) INCLUDE (Amount);

-- =============================================
-- 2. LINK: Purchases -> Suppliers
-- Description: Links the Inventory/Purchase module to the Finance/Accounts Payable module.
-- Logic: A Purchase Invoice must belong to a valid Supplier.
-- =============================================

-- Note: SupplierID column already exists in Purchases table, we just add the constraint.
ALTER TABLE Purchases
ADD CONSTRAINT FK_Purchases_Suppliers 
FOREIGN KEY (SupplierID) REFERENCES Suppliers(SupplierID);


-- =============================================
-- 3. LINK: Sales -> Customers
-- Description: Links the POS/Sales module to the Finance/Accounts Receivable module.
-- Logic: A Sale (if not anonymous) belongs to a valid Customer.
-- =============================================

-- Note: CustomerID column already exists in Sales table, we just add the constraint.
ALTER TABLE Sales
ADD CONSTRAINT FK_Sales_Customers 
FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID);


-- =============================================
-- 4. LINK: Accounts -> AccountSubCategories
-- Description: Completes the Chart of Accounts structure.
-- Logic: Every G/L Account must belong to a SubCategory (e.g., 'Cash' belongs to 'Cash & Equivalents').
-- =============================================

-- Step B: Create the Foreign Key Constraint
ALTER TABLE Accounts
add CONSTRAINT FK_Accounts_SubCategories 
FOREIGN KEY (SubCategoryID) REFERENCES AccountSubCategories(SubCategoryID);

-- Step C: Index is crucial for generating financial reports (Balance Sheet)
CREATE NONCLUSTERED INDEX IX_Accounts_SubCategoryID ON Accounts(SubCategoryID);