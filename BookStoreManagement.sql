CREATE DATABASE BookstoreDB;

USE BookstoreDB;

drop table Users

CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY,
    UserFirstName NVARCHAR(50),
    UserEmail NVARCHAR(100) UNIQUE,
    UserPassword NVARCHAR(50),
	UserPhone NVARCHAR(20)

);

UPDATE Users
SET UserPhone = '7259227183'
WHERE UserId = 8;

select * from Users


drop procedure sp_CreateUser


CREATE PROCEDURE sp_GetUserByEmail
    @Email NVARCHAR(100)
AS
BEGIN
    SELECT * FROM Users WHERE UserEmail = @Email;
END;



CREATE PROCEDURE sp_UpdatePassword
    @Email NVARCHAR(100),
    @Password NVARCHAR(100)
AS
BEGIN
    UPDATE Users SET UserPassword = @Password WHERE UserEmail = @Email;
    SELECT @@ROWCOUNT AS RowsAffected;
END;


create PROCEDURE sp_CreateUser
    @UserFirstName NVARCHAR(50),
    @UserEmail NVARCHAR(100),
    @UserPassword NVARCHAR(100),
    @UserPhone NVARCHAR(20) -- Add the parameter for UserPhone
AS
BEGIN
    INSERT INTO Users (UserFirstName, UserEmail, UserPassword, UserPhone)
    VALUES (@UserFirstName, @UserEmail, @UserPassword, @UserPhone);

    SELECT SCOPE_IDENTITY() AS UserId;
END

CREATE TABLE Books (
    BookId INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(255) NOT NULL,
    Author NVARCHAR(255) NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    Description NVARCHAR(MAX),
    Quantity INT NOT NULL,
    ImagePath NVARCHAR(1000)
);

UPDATE Books
SET ImagePath = 'https://imgs.search.brave.com/1R9RUAwu665IS_F8-Atj7E3Jj0oyLddfELagR-28nUk/rs:fit:860:0:0/g:ce/aHR0cHM6Ly9tLm1l/ZGlhLWFtYXpvbi5j/b20vaW1hZ2VzL0kv/MjExNjQrcmhQU0wu/anBn'
WHERE BookId = 5;

DECLARE @ImagePath NVARCHAR(500);
SET @ImagePath = 'https://upload.wikimedia.org/wikipedia/commons/thumb/8/89/The_Catcher_in_the_Rye_%281951%2C_first_edition_cover%29.jpg/220px-The_Catcher_in_the_Rye_%281951%2C_first_edition_cover%29.jpg';

UPDATE Books
SET ImagePath = @ImagePath
WHERE BookId = 1;


select * from Books

drop table Books

drop procedure sp_UpdateBook

CREATE PROCEDURE sp_AddBook
    @Title NVARCHAR(255),
    @Author NVARCHAR(255),
    @Price DECIMAL(18, 2),
    @Description NVARCHAR(MAX),
    @ImagePath NVARCHAR(1000),
    @Quantity INT
AS
BEGIN
    INSERT INTO Books (Title, Author, Price, Description, ImagePath, Quantity)
    VALUES (@Title, @Author, @Price, @Description, @ImagePath, @Quantity);
END


CREATE PROCEDURE sp_GetAllBooks
AS
BEGIN
    SELECT * FROM Books;
END



CREATE PROCEDURE sp_UpdateBook
    @Id INT,
    @Title NVARCHAR(255),
    @Author NVARCHAR(255),
    @Price DECIMAL(10, 2),
    @Description NVARCHAR(1000),
    @ImagePath NVARCHAR(255),
	@Quantity INT
AS
BEGIN
    UPDATE Books
    SET Title = @Title,
        Author = @Author,
        Price = @Price,
        Description = @Description,
        ImagePath = @ImagePath,
		Quantity=@Quantity
    WHERE BookId = @Id;
END




CREATE PROCEDURE sp_DeleteBook
    @Id INT
AS
BEGIN
    DELETE FROM Books
    WHERE BookId = @Id;
END


CREATE TABLE CartItems (
    Id INT PRIMARY KEY IDENTITY,
    UserId INT NOT NULL,
    BookId INT NOT NULL,
    Quantity INT NOT NULL,
    CONSTRAINT FK_UserId FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT FK_BookId FOREIGN KEY (BookId) REFERENCES Books(BookId)
);

select * from CartItems

SELECT b.BookId, b.Title, b.Author, b.Description, b.Price, b.ImagePath, ci.Quantity AS Quantity
FROM CartItems ci 
INNER JOIN Books b ON ci.BookId = b.BookId 
WHERE ci.UserId = 3;

CREATE TABLE Addresses (
    AddressId INT PRIMARY KEY IDENTITY,
    Address NVARCHAR(255) NOT NULL,
    City NVARCHAR(100) NOT NULL,
    State NVARCHAR(100) NOT NULL,
    Type INT NOT NULL,
    UserId INT NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);
ALTER TABLE Addresses
ALTER COLUMN UserPhone NVARCHAR(20) NOT NULL;
ALTER COLUMN UserName NVARCHAR(50) NOT NULL


ALTER TABLE Addresses
ALTER COLUMN UserName NVARCHAR(50) NULL,
ALTER COLUMN UserPhone NVARCHAR(20) NULL;


ALTER TABLE Addresses
ADD UserName NVARCHAR(50) NULL,
    UserPhone NVARCHAR(20) NULL;
	UPDATE Addresses SET UserName = 'harsha', UserPhone = '8892508645' where UserID=2;

select * from addresses

CREATE TABLE Wishlist (
    WishlistId INT IDENTITY(1,1) PRIMARY KEY,
    BookId INT NOT NULL,
    UserId INT NOT NULL,
    CONSTRAINT FK_Wishlist_Books FOREIGN KEY (BookId) REFERENCES Books(BookId),
    CONSTRAINT FK_Wishlist_Users FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

select * from Wishlist

SELECT a.*, u.UserFirstName, u.UserPhone 
FROM Addresses a
JOIN Users u ON a.UserId = u.UserId
WHERE a.UserId = 2;

CREATE TABLE Orders (
    OrderId INT PRIMARY KEY IDENTITY(1,1),
    AddressId INT NOT NULL,
    UserId INT NOT NULL,
    OrderDate DATETIME NOT NULL,
    FOREIGN KEY (AddressId) REFERENCES Addresses(AddressId),
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

-- Create OrderBooks table to handle many-to-many relationship
CREATE TABLE OrderBooks (
    OrderId INT NOT NULL,
    BookId INT NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
    FOREIGN KEY (BookId) REFERENCES Books(BookId),
    PRIMARY KEY (OrderId, BookId)
);

-- Create stored procedures for CartItems operations

-- GetCartBooks
CREATE PROCEDURE sp_GetCartBooks
    @UserId INT
AS
BEGIN
    SELECT b.BookId, b.Title, b.Author, b.Description, b.Price, b.ImagePath, ci.Quantity AS Quantity
    FROM CartItems ci 
    INNER JOIN Books b ON ci.BookId = b.BookId 
    WHERE ci.UserId = @UserId;
END

-- AddToCart
CREATE PROCEDURE sp_AddToCart
    @UserId INT,
    @BookId INT,
    @Quantity INT
AS
BEGIN
    INSERT INTO CartItems (UserId, BookId, Quantity)
    VALUES (@UserId, @BookId, @Quantity);
END

-- IsBookInCart
CREATE PROCEDURE sp_IsBookInCart
    @UserId INT,
    @BookId INT
AS
BEGIN
    SELECT COUNT(*) FROM CartItems WHERE UserId = @UserId AND BookId = @BookId;
END

-- UpdateQuantity
CREATE PROCEDURE sp_UpdateQuantity
    @UserId INT,
    @BookId INT,
    @Quantity INT
AS
BEGIN
    UPDATE CartItems
    SET Quantity = @Quantity
    WHERE UserId = @UserId AND BookId = @BookId;
END

-- DeleteCart
CREATE PROCEDURE sp_DeleteCart
    @UserId INT,
    @BookId INT
AS
BEGIN
    DELETE FROM CartItems WHERE UserId = @UserId AND BookId = @BookId;
END

-- GetCartItemsByUserId
CREATE PROCEDURE sp_GetCartItemsByUserId
    @UserId INT
AS
BEGIN
    SELECT * FROM CartItems WHERE UserId = @UserId;
END

-- Create stored procedures for Order operations--

-- AddOrder procedure
CREATE PROCEDURE sp_AddOrder
    @AddressId INT,
    @UserId INT,
    @OrderDate DATETIME,
    @OrderId INT OUTPUT
AS
BEGIN
    INSERT INTO Orders (AddressId, UserId, OrderDate)
    VALUES (@AddressId, @UserId, @OrderDate);
    
    SET @OrderId = SCOPE_IDENTITY();
END

-- AddOrderBook procedure
CREATE PROCEDURE sp_AddOrderBook
    @OrderId INT,
    @BookId INT
AS
BEGIN
    INSERT INTO OrderBooks (OrderId, BookId)
    VALUES (@OrderId, @BookId);
END

-- GetOrdersByUserId procedure
CREATE PROCEDURE sp_GetOrdersByUserId
    @UserId INT
AS
BEGIN
    SELECT * FROM Orders WHERE UserId = @UserId;
END

-- GetBooksByOrderIds procedure
CREATE PROCEDURE sp_GetBooksByOrderIds
    @OrderIds dbo.IntList READONLY
AS
BEGIN
    SELECT b.*, ob.OrderId
    FROM Books b
    INNER JOIN OrderBooks ob ON b.BookId = ob.BookId
    WHERE ob.OrderId IN (SELECT Id FROM @OrderIds);
END

-- GetAddressesByAddressIds procedure
CREATE PROCEDURE sp_GetAddressesByAddressIds
    @AddressIds dbo.IntList READONLY
AS
BEGIN
    SELECT * FROM Addresses WHERE AddressId IN (SELECT Id FROM @AddressIds);
END

-- GetUserByUserId procedure
CREATE PROCEDURE sp_GetUserByUserId
    @UserId INT
AS
BEGIN
    SELECT * FROM Users WHERE UserId = @UserId;
END

CREATE TYPE dbo.IntList AS TABLE
(
    Id INT
);


-- Create stored procedures for Wishlist operations--
-- GetWishlistBooks procedure
CREATE PROCEDURE sp_GetWishlistBooks
    @UserId INT
AS
BEGIN
    SELECT b.* 
    FROM Wishlist w
    JOIN Books b ON w.BookId = b.BookId
    WHERE w.UserId = @UserId;
END

-- AddToWishlist procedure
CREATE PROCEDURE sp_AddToWishlist
    @BookId INT,
    @UserId INT,
    @WishlistId INT OUTPUT
AS
BEGIN
    INSERT INTO Wishlist (BookId, UserId) 
    VALUES (@BookId, @UserId); 

    SET @WishlistId = SCOPE_IDENTITY();
END

-- GetWishlistByBookAndUser procedure
CREATE PROCEDURE sp_GetWishlistByBookAndUser
    @BookId INT,
    @UserId INT
AS
BEGIN
    SELECT TOP 1 * 
    FROM Wishlist 
    WHERE BookId = @BookId AND UserId = @UserId;
END

-- DeleteWishlist procedure
CREATE PROCEDURE sp_DeleteWishlist
    @UserId INT,
    @BookId INT
AS
BEGIN
    DELETE FROM Wishlist 
    WHERE UserId = @UserId AND BookId = @BookId;
END

-- Create stored procedures for Address operations
-- GetAddresses procedure
CREATE or alter PROCEDURE sp_GetAddresses
    @UserId INT
AS
BEGIN
    --SELECT a.*, u.UserFirstName, u.UserPhone 
    --FROM Addresses a
    --JOIN Users u ON a.UserId = u.UserId
    --WHERE a.UserId = @UserId;
	select * from Addresses where USERID=@UserId;
END

-- GetAddressById procedure
CREATE or alter PROCEDURE sp_GetAddressById
    @AddressId INT
AS
BEGIN
    SELECT * FROM Addresses WHERE AddressId = @AddressId;
END

-- AddAddress procedure
drop procedure sp_AddAddress
CREATE PROCEDURE sp_AddAddress
    @Address NVARCHAR(100),
    @City NVARCHAR(50),
    @State NVARCHAR(50),
    @Type NVARCHAR(50),
    @UserId INT,
    @UserName NVARCHAR(50),
    @UserPhone NVARCHAR(20)
AS
BEGIN
    INSERT INTO Addresses (Address, City, State, Type, UserId, UserName, UserPhone) 
    VALUES (@Address, @City, @State, @Type, @UserId, @UserName, @UserPhone);
END

ALTER PROCEDURE sp_AddAddress
    @Address NVARCHAR(100),
    @City NVARCHAR(50),
    @State NVARCHAR(50),
    @Type NVARCHAR(50),
    @UserId INT,
    @UserName NVARCHAR(50),  -- Set default value to empty string
    @UserPhone NVARCHAR(20)  -- Set default value to empty string
AS
BEGIN
    INSERT INTO Addresses (Address, City, State, Type, UserId, UserName, UserPhone) 
    VALUES (@Address, @City, @State, @Type, @UserId, @UserName, @UserPhone);
END


-- UpdateAddress procedure
CREATE or alter PROCEDURE sp_UpdateAddress
    @AddressId INT,
    @Address NVARCHAR(100),
    @City NVARCHAR(50),
    @State NVARCHAR(50),
    @Type NVARCHAR(50),
    @UserId INT,
    @UserName NVARCHAR(50),
    @UserPhone NVARCHAR(20)
AS
BEGIN
    UPDATE Addresses 
    SET Address = @Address,
        City = @City,
        State = @State,
        Type = @Type,
        UserName = @UserName,
        UserPhone = @UserPhone
    WHERE AddressId = @AddressId AND UserId = @UserId;
END


-- DeleteAddress procedure
CREATE PROCEDURE sp_DeleteAddress
    @AddressId INT
AS
BEGIN
    DELETE FROM Addresses WHERE AddressId = @AddressId;
END
select * from Addresses