create database BDKLINUX
go

use BDKLINUX
go

create table CatDispositivo(
	DispositivoId int Primary Key Identity(1,1),
	DispositivoNombre varchar(50) Not Null,
	DispositivoState bit Default 1
)
go

CREATE TABLE Roles (
    RolId INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL
);
GO

CREATE TABLE Usuario (
    UsuarioId INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL,
    Correo VARCHAR(100) NOT NULL,
    RolId INT,
    FOREIGN KEY (RolId) REFERENCES Roles(RolId)
);
GO

CREATE TABLE Producto (
    ProductoId INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL,
    Precio DECIMAL(10, 2) NOT NULL,
    CategoriaId INT,
    FOREIGN KEY (CategoriaId) REFERENCES Categoria(CategoriaId)
);
GO

CREATE TABLE Categoria (
    CategoriaId INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL
);
GO

select * from Categoria
select * from Producto
select * from Roles
select * from Usuario