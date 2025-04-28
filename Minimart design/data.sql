-- Categories
INSERT INTO Categories (CategoryName, CategoryDescription)
VALUES 
('Fruits', 'Fresh fruits from local farms'),
('Dairy', 'Milk, cheese, and other dairy products'),
('Snacks', 'Chips, biscuits, and packaged snacks');

-- Suppliers
INSERT INTO Suppliers (SupplierName, SupplierPhoneNumber, SupplierAddress, SupplierEmail)
VALUES
('FreshFarm Co.', '0912345678', '123 Farm Road', 'contact@freshfarm.com'),
('DairyDelight Ltd.', '0987654321', '456 Dairy Lane', 'support@dairydelight.com');

-- Measurement Units
INSERT INTO MeasurementUnits (UnitName, UnitDescription, IsContinuous)
VALUES
('Kilogram', 'Used for weight-based items', 1),
('Piece', 'Used for countable items', 0);

-- Product Types
INSERT INTO ProductTypes (ProductName, ProductDescription, CategoryID, SupplierID, Price, Tags, StockAmount, MeasurementUnitID, ExpirationDurationDays, ImagePath)
VALUES
('Banana', 'Ripe yellow bananas', 1, 1, 1.99, 'fruit,fresh', 120, 1, 7, '/images/banana.jpg'),
('Milk 1L', 'Full cream milk 1L pack', 2, 2, 2.49, 'dairy,drink', 50, 1, 10, '/images/milk.jpg'),
('Potato Chips', 'Salted potato chips 150g', 3, 2, 1.50, 'snack,salty', 200, 2, 180, '/images/chips.jpg');

-- Employee Roles
INSERT INTO EmployeeRoles (RoleName, RoleDescription)
VALUES 
('Cashier', 'Handles transactions at the counter'),
('Stock Manager', 'Manages stock and inventory');

-- Employees
INSERT INTO Employees (FirstName, LastName, Email, PhoneNumber, Gender, BirthDate, CitizenID, Salary, RoleID, ImagePath)
VALUES
('Alice', 'Smith', 'alice@minimart.com', '0900111222', 'Female', '1990-05-01', 'C12345678', 2500.00, 1, '/images/alice.jpg'),
('Bob', 'Johnson', 'bob@minimart.com', '0900333444', 'Male', '1985-11-12', 'C87654321', 2700.00, 2, '/images/bob.jpg');

-- Admins
INSERT INTO Admins (EmployeeID, Username, PasswordHash, Salt)
VALUES
(2, 'adminbob', 0xD5E3A1B4D4DABEEF0023ABCD12345678D5E3A1B4D4DABEEF0023ABCD12345678, 0x00112233445566778899AABBCCDDEEFF);

-- Customers
INSERT INTO Customers (FirstName, LastName, Email, PhoneNumber, ImagePath, Username, PasswordHash, Salt)
VALUES
('John', 'Doe', 'john@example.com', '0911223344', '/images/john.jpg', 'johndoe', 0xDEADBEEFDEADBEEFDEADBEEFDEADBEEFDEADBEEFDEADBEEFDEADBEEFDEADBEEF, 0xAABBCCDDEEFF00112233445566778899);

-- Payment Methods
INSERT INTO PaymentMethods (MethodName)
VALUES 
('Cash'),
('Credit Card'),
('Mobile Payment');

-- Sales
INSERT INTO Sales (CustomerID, EmployeeID, PaymentMethodID, DeliveryAddress, DeliveryTime, IsPickup, OrderStatus)
VALUES
(1, 1, 2, '789 Customer Ave', GETDATE(), 0, 'Completed');

-- Sale Details
INSERT INTO SaleDetails (SaleID, ProductTypeID, Quantity, ProductPriceAtPurchase)
VALUES
(1, 1, 3.5, 1.99),  -- Bananas
(1, 2, 2, 2.49);     -- Milk
