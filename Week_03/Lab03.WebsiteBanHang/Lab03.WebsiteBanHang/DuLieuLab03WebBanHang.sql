use Lab03WebBanHang;

ALTER TABLE Products DROP CONSTRAINT FK_Products_Categories_CategoryId;
INSERT INTO Categories (Name) VALUES 
(N'Nike'), 
(N'Adidas'), 
(N'New Blance'), 
(N'Puma'),
(N'Vans');

INSERT INTO Products (Name, Price, Description, ImageUrl, CategoryId) VALUES
(N'Nike Air Force 1', 3239000, N'New Nike Air Force 1 ', '/images/Nike_2.png', 1),
(N'Nike Air Force 1', 3239000, N'New Nike Air Force 1 ', '/images/Nike_3.png', 1);

INSERT INTO Products (Name, Price, Description, ImageUrl, CategoryId) VALUES
(N'Adidas Yeezy 350 V2', 3239000, N'New Adidas Yeezy 350 V2 ', '/images/Adidas_1.jpg', 2),
(N'Adidas Alphabounce', 3239000, N'New Adidas Alphabounce ', '/images/Adidas_2.jpg', 2),
(N'Adidas Yeezy 700', 3239000, N'New Adidas Yeezy 700 ', '/images/Adidas_3.jpg', 2);	

INSERT INTO Products (Name, Price, Description, ImageUrl, CategoryId) VALUES
(N'New Balance 327', 3239000, N'New New Balance 327 ', '/images/NB_1.jpg', 3),
(N'New Balance 574', 3239000, N'New New Balance 574 ', '/images/NB_2.jpg', 3),
(N'New Balance 550', 3239000, N'New New Balance 550 ', '/images/NB_3.jpg', 3);

INSERT INTO Products (Name, Price, Description, ImageUrl, CategoryId) VALUES
(N'Puma Fenty Creeper', 3239000, N'New Puma Fenty Creeper ', '/images/Puma_1.jpg', 4),
(N'Puma Skype', 3239000, N'New Puma Skype', '/images/Puma_2.jpg', 4),
(N'Puma Bari Mule', 3239000, N'Puma Bari Mule ', '/images/Puma_3.jpg', 4);

INSERT INTO Products (Name, Price, Description, ImageUrl, CategoryId) VALUES
(N'Vans Era ', 3239000, N'New Vans Era  ', '/images/Vans_1.jpg', 5),
(N'Vans Slip-on ', 3239000, N'New Vans Slip-on  ', '/images/Vans_2.jpg', 5),
(N'Vans Old skool', 3239000, N'New Vans Old skool ', '/images/Vans_3.jpg', 5);

ALTER TABLE Products ADD CONSTRAINT FK_Products_Categories_CategoryId 
FOREIGN KEY (CategoryId) REFERENCES Categories(Id);

SELECT *FROM Categories;
SELECT *FROM Products;
SELECT DISTINCT CategoryId FROM Products;
DELETE FROM Products WHERE CategoryId NOT IN (SELECT Id FROM Categories);

