USE Minimart;

-- Categories table
CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    CategoryName VARCHAR(255) NOT NULL UNIQUE,
    CategoryDescription TEXT NULL
);

-- Suppliers table
CREATE TABLE Suppliers (
    SupplierID INT PRIMARY KEY IDENTITY(1,1),
    SupplierName VARCHAR(255) NOT NULL UNIQUE,
    SupplierPhoneNumber CHAR(10) NOT NULL UNIQUE,
    SupplierAddress VARCHAR(255) NOT NULL,
    SupplierEmail VARCHAR(255) NOT NULL UNIQUE
);

-- Measurement units table
CREATE TABLE MeasurementUnits (
    MeasurementUnitID INT PRIMARY KEY IDENTITY(1,1),
    UnitName VARCHAR(50) NOT NULL UNIQUE,
    UnitDescription TEXT NULL,
    IsContinuous BIT NOT NULL  -- 1: continuous like kg, 0: discrete like pieces
);

-- Product types (catalog)
CREATE TABLE ProductTypes (
    ProductTypeID INT PRIMARY KEY IDENTITY(1,1),
    ProductName VARCHAR(255) NOT NULL UNIQUE,
    ProductDescription TEXT NOT NULL,
    CategoryID INT NOT NULL,
    SupplierID INT NOT NULL,
    Price DECIMAL(10,2) NOT NULL CHECK (Price >= 0),
    Tags VARCHAR(255) NULL,
    StockAmount DECIMAL(10,2) NOT NULL DEFAULT 0 CHECK (StockAmount >= 0),
    MeasurementUnitID INT NOT NULL,
    ExpirationDurationDays INT NULL CHECK (ExpirationDurationDays >= 0),
    IsActive BIT NOT NULL DEFAULT 1,
    DateAdded DATETIME NOT NULL DEFAULT GETDATE(),
    ImagePath VARCHAR(512) NULL,
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID) ON DELETE CASCADE,
    FOREIGN KEY (SupplierID) REFERENCES Suppliers(SupplierID) ON DELETE CASCADE,
    FOREIGN KEY (MeasurementUnitID) REFERENCES MeasurementUnits(MeasurementUnitID) ON DELETE CASCADE
);

-- Customers table
CREATE TABLE Customers (
    CustomerID INT PRIMARY KEY IDENTITY(1,1),
    FirstName VARCHAR(255) NOT NULL,
    LastName VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL UNIQUE,
    PhoneNumber CHAR(10) NOT NULL UNIQUE,
    ImagePath VARCHAR(512) NULL,
    Username VARCHAR(255) NOT NULL UNIQUE,
    PasswordHash VARBINARY(64) NOT NULL,
    Salt VARBINARY(64) NOT NULL
);

-- Employee roles
CREATE TABLE EmployeeRoles (
    RoleID INT PRIMARY KEY IDENTITY(1,1),
    RoleName VARCHAR(255) NOT NULL UNIQUE,
    RoleDescription TEXT NULL	
);

-- Employees table
CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY IDENTITY(1,1),
    FirstName VARCHAR(255) NOT NULL,
    LastName VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL UNIQUE,
    PhoneNumber CHAR(10) NOT NULL UNIQUE,
    Gender VARCHAR(20) NOT NULL CHECK (Gender IN ('Male', 'Female', 'Non-Binary', 'Prefer not to say')),
    BirthDate DATE NOT NULL,
    CitizenID VARCHAR(100) NOT NULL UNIQUE,
    Salary DECIMAL(10,2) NULL CHECK (Salary >= 0),
    HireDate DATETIME NOT NULL DEFAULT GETDATE(),
    RoleID INT NOT NULL,
    ImagePath VARCHAR(512) NULL,
    FOREIGN KEY (RoleID) REFERENCES EmployeeRoles(RoleID) ON DELETE CASCADE
);

-- Admins table
CREATE TABLE Admins (
    AdminID INT PRIMARY KEY IDENTITY(1,1),
    EmployeeID INT NOT NULL UNIQUE,
    Username VARCHAR(255) NOT NULL UNIQUE,
    PasswordHash VARBINARY(64) NOT NULL,
    Salt VARBINARY(64) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    LastLogin DATETIME NULL,
    IsActive BIT DEFAULT 1,
    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID) ON DELETE CASCADE
);

-- Payment methods
CREATE TABLE PaymentMethods (
    PaymentMethodID INT PRIMARY KEY IDENTITY(1,1),
    MethodName VARCHAR(50) NOT NULL UNIQUE
);

-- Sales table
CREATE TABLE Sales (
    SaleID INT PRIMARY KEY IDENTITY(1,1),
    SaleDate DATETIME NOT NULL DEFAULT GETDATE(),
    CustomerID INT NULL,
    EmployeeID INT NOT NULL,
    PaymentMethodID INT NOT NULL,
    DeliveryAddress VARCHAR(255) NULL,
    DeliveryTime DATETIME NULL,
    IsPickup BIT NOT NULL DEFAULT 0,
    OrderStatus VARCHAR(50) NOT NULL DEFAULT 'Pending',
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID) ON DELETE CASCADE,
    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID) ON DELETE CASCADE,
    FOREIGN KEY (PaymentMethodID) REFERENCES PaymentMethods(PaymentMethodID) ON DELETE CASCADE
);

-- Sale details (line items)
CREATE TABLE SaleDetails (
    SaleDetailID INT PRIMARY KEY IDENTITY(1,1),
    SaleID INT NOT NULL,
    ProductTypeID INT NOT NULL,
    Quantity DECIMAL(10,2) NOT NULL CHECK (Quantity > 0),
    ProductPriceAtPurchase DECIMAL(10,2) NOT NULL CHECK (ProductPriceAtPurchase >= 0),
    FOREIGN KEY (SaleID) REFERENCES Sales(SaleID) ON DELETE CASCADE,
    FOREIGN KEY (ProductTypeID) REFERENCES ProductTypes(ProductTypeID) ON DELETE CASCADE
);
