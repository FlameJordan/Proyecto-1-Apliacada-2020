Create database bdtienda;
use bdtienda;

create table bazar(
	id int auto_increment primary key not null,
    nombreproducto varchar(200) not null,
    cantidadstock int not null,
    precio int not null,
    descripcion varchar(600) not null,
    tipo int not null,
    imagen varchar(250) not null
);

create table libreria(
	id int auto_increment primary key not null,
    nombreproducto varchar(200) not null,
    cantidadstock int not null,
    precio int not null,
    descripcion varchar(600) not null,
    tipo int not null,
    imagen varchar(250) not null
);

CREATE TABLE computacion (
    id INT AUTO_INCREMENT PRIMARY KEY NOT NULL,
    nombreproducto VARCHAR(200) NOT NULL,
    cantidadstock INT NOT NULL,
    precio INT NOT NULL,
    descripcion VARCHAR(600) NOT NULL,
    tipo INT NOT NULL,
    imagen VARCHAR(250) NOT NULL
);

/*bazar*/
DELIMITER $$
create procedure sp_obtenerProductoBazar(IN idi int)
BEGIN
	SELECT nombreproducto,cantidadstock,precio,descripcion, tipo,imagen from bazar WHERE id = idi;
END$$
DELIMITER ;

DELIMITER $$
create procedure sp_insertarProductoBazar(IN nombre varchar(200), IN cantidadStock int, IN precio int, IN descripcion varchar(600), IN tipo int, IN imagen varchar(250))
BEGIN
	INSERT INTO bazar(nombreproducto, cantidadstock,precio,descripcion,tipo,imagen) VALUES (nombre, cantidadstock,precio,descripcion,tipo,imagen);
END$$
DELIMITER ;

DELIMITER $$
create procedure sp_eliminarProductoBazar(IN idi int)
BEGIN
	DELETE FROM bazar WHERE id=idi;
END$$
DELIMITER ;

DELIMITER $$
create procedure sp_obtenerProductosBazar()
BEGIN
	SELECT id, nombreproducto,cantidadstock,precio,descripcion,tipo,imagen from bazar;
END$$
DELIMITER ;

DELIMITER $$
create procedure sp_actualizarProductoBazar(IN idi int, IN nombre varchar(200), IN cantidadStocki int,IN precioi int, IN descripcioni varchar(600), IN tipoi int, IN imageni varchar(250))
BEGIN
	UPDATE bazar set nombreproducto=nombre, cantidadstock = cantidadStocki,precio = precioi, descripcion = descripcioni, tipo = tipoi, imagen = imageni WHERE id = idi;
END$$
DELIMITER ;

/*libreria*/
DELIMITER $$
create procedure sp_obtenerProductolibreria(IN idi int)
BEGIN
	SELECT nombreproducto,cantidadstock,precio,descripcion, tipo,imagen from libreria WHERE id = idi;
END$$
DELIMITER ;

DELIMITER $$
create procedure sp_insertarProductolibreria(IN nombre varchar(200), IN cantidadStock int, IN precio int, IN descripcion varchar(600), IN tipo int, IN imagen varchar(250))
BEGIN
	INSERT INTO libreria(nombreproducto, cantidadstock,precio,descripcion,tipo,imagen) VALUES (nombre, cantidadstock,precio,descripcion,tipo,imagen);
END$$
DELIMITER ;

DELIMITER $$
create procedure sp_eliminarProductolibreria(IN idi int)
BEGIN
	DELETE FROM libreria WHERE id=idi;
END$$
DELIMITER ;

DELIMITER $$
create procedure sp_obtenerProductoslibreria()
BEGIN
	SELECT id, nombreproducto,cantidadstock,precio,descripcion,tipo,imagen from libreria;
END$$
DELIMITER ;

DELIMITER $$
create procedure sp_actualizarProductoLibreria(IN idi int, IN nombre varchar(200), IN cantidadStocki int,IN precioi int, IN descripcioni varchar(600), IN tipoi int, IN imageni varchar(250))
BEGIN
	UPDATE libreria set nombreproducto=nombre, cantidadstock = cantidadStocki,precio = precioi, descripcion = descripcioni, tipo = tipoi, imagen = imageni WHERE id = idi;
END$$
DELIMITER ;

/*computacion*/
DELIMITER $$
create procedure sp_obtenerProductoComputacion(IN idi int)
BEGIN
	SELECT nombreproducto,cantidadstock,precio,descripcion, tipo,imagen from computacion WHERE id = idi;
END$$
DELIMITER ;

DELIMITER $$
create procedure sp_insertarProductoComputacion(IN nombre varchar(200), IN cantidadStock int, IN precio int, IN descripcion varchar(600), IN tipo int, IN imagen varchar(250))
BEGIN
	INSERT INTO computacion(nombreproducto, cantidadstock,precio,descripcion,tipo,imagen) VALUES (nombre, cantidadstock,precio,descripcion,tipo,imagen);
END$$
DELIMITER ;

DELIMITER $$
create procedure sp_eliminarProductoComputacion(IN idi int)
BEGIN
	DELETE FROM computacion WHERE id=idi;
END$$
DELIMITER ;

DELIMITER $$
create procedure sp_obtenerProductosComputacion()
BEGIN
	SELECT id, nombreproducto,cantidadstock,precio,descripcion,tipo,imagen from computacion;
END$$
DELIMITER ;

DELIMITER $$
create procedure sp_actualizarProductoComputacion(IN idi int, IN nombre varchar(200), IN cantidadStocki int,IN precioi int, IN descripcioni varchar(600), IN tipoi int, IN imageni varchar(250))
BEGIN
	UPDATE computacion set nombreproducto=nombre, cantidadstock = cantidadStocki,precio = precioi, descripcion = descripcioni, tipo = tipoi, imagen = imageni WHERE id = idi;
END$$
DELIMITER ;

/*actualizar stock*/
DELIMITER $$
create procedure sp_actualizarStockBazar(IN idi int, IN cantidad int)
BEGIN
	UPDATE bazar set cantidadstock = (cantidadStock-cantidad) WHERE id = idi;
END$$
DELIMITER ;

DELIMITER $$
create procedure sp_actualizarStockLibreria(IN idi int, IN cantidad int)
BEGIN
	UPDATE libreria set cantidadstock = (cantidadStock-cantidad) WHERE id = idi;
END$$
DELIMITER ;

DELIMITER $$
create procedure sp_actualizarStockComputadora(IN idi int, IN cantidad int)
BEGIN
	UPDATE computacion set cantidadstock = (cantidadStock-cantidad) WHERE id = idi;
END$$
DELIMITER ;