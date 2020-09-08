--TABLAS--
create table Clientes (
	idCliente serial PRIMARY KEY,
	nombre Varchar (100)  NOT NULL,
	email VARCHAR ( 255 ) NOT NULL,
	contrasena VARCHAR ( 50 ) NOT NULL
);
create table Producto (
	idCont serial NOT NULL,
	idProducto int NOT NULL,
	idEmpresa int NOT NULL,
	nombre Varchar (100)  NOT NULL,
	precio INT NOT NULL,
	descripcion Varchar (100)  NOT NULL,
	tipo INT NOT NULL,
	imagen VARCHAR(200) NOT NULL,
	cantStock int NOT NULL,
	estado int NOT NULL,
	PRIMARY KEY (idProducto,idEmpresa)
);

--Agregué hoy
create table Carrito (
	idCliente int  NOT NULL,
	idProducto int NOT NULL,
	idEmpresa int NOT NULL,
	cant int NOT NULL,
	PRIMARY KEY (idProducto,idEmpresa,idCliente)
);
--table tipoProducto
create table tipoProducto(
	idProducto serial NOT NULL PRIMARY KEY,
	nombreTipo VARCHAR(100) NOT NULL
)
---------------------------------------
create table empresa (
	idempresa serial NOT NULL PRIMARY KEY,
	nombre VARCHAR(100) NOT NULL,
	clave VARCHAR(60) NOT NULL,
	correo VARCHAR(200) NOT NULL
);
---------------------------------------
--TABLE SUGERENCIAS CLIENTE--
CREATE TABLE sugerencias(
	idproducto INT NOT NULL,
	idempresa INT NOT NULL,
	idcliente INT NOT NULL,
	cantidadClicks INT NOT NULL,
	PRIMARY KEY(idproducto,idempresa,idcliente)
);

---------------------------------------
--INSERTS--
Insert into Clientes (nombre, contrasena, email) values ('Graciela', 'gp@gmail.com', '123' );
INSERT INTO PRODUCTO (idProducto, idEmpresa, nombre, precio, descripcion, tipo, imagen, cantStock, estado) 
	VALUES(7, 2,'Samsung Galaxy Fold3',1675000,'Movil 166hz 4k', 3, 'fold3.png', 5,1);
INSERT INTO tipoProducto (nombreTipo) VALUES ('Arma'),('Ropa'),('Tecnología'),('Alimento'),('Fármaco'),('Servicio')
							
update Producto set  imagen='SG10.png' where idProducto='2'
--FUNCIONES--
create or replace FUNCTION registrarClientes(nombreC varchar(100), emailC VARCHAR ( 255 ), contrasenaC VARCHAR ( 50 ))
RETURNS SETOF INTEGER 
AS $BODY$
declare existee varchar (50);
begin
	existee := (select (exists (select * FROM Clientes where nombre=nombreC AND email=emailC)) as existe);
	if (existee='true') then
		return query select 0 as resultado;
	else 
		Insert into Clientes (nombre, email, contrasena) values (nombreC, emailC, contrasenaC);
		return query select 1 as resultado;
	end if;
end
$BODY$
LANGUAGE plpgsql;
select registrarClientes ('GraciPorras', 'delosangeles@gmail.com', '1234');
--INICIARSESION--

create or replace FUNCTION iniciarSesion(nombreC varchar(100),contrasenaC VARCHAR (50))
RETURNS SETOF INTEGER 
AS $BODY$
declare existee varchar (50);
begin
	existee := (select (exists (select * FROM Clientes where nombre=nombreC AND contrasena=contrasenaC)));
	if (existee='true') then
		return query select idCliente as idCliente from Clientes where nombre=nombreC AND contrasena=contrasenaC;
		
	else 
		return query select 0 as resultado;
	end if;
end
$BODY$
LANGUAGE plpgsql;
select iniciarSesion ('GraciPorras', '1234');

--OBTENERPRODUCTOS--
create or replace FUNCTION obtenerProductos()
RETURNS TABLE(idproductoT INT ,idempresaT INT,nombreT VARCHAR(100),precioT INT,descripcionT VARCHAR(100),cantstockT INT ,estadoT INT) AS
$BODY$
DECLARE
    reg RECORD;
BEGIN
    FOR REG IN SELECT idproducto,idempresa,nombre,precio,descripcion,cantstock,estado
FROM producto LOOP
        idproductoT := reg.idproducto;
        idempresaT   := reg.idempresa;
		nombreT := reg.nombre;
		precioT := reg.precio;
		descripcionT := reg.descripcion;
		cantstockT := reg.cantstock;
		estadoT := reg.estado;
        RETURN NEXT;
    END LOOP;
    RETURN;
END
$BODY$
LANGUAGE plpgsql;
select obtenerProductos();

--DETALLEPRODUCTO--
create or replace FUNCTION detalleProducto(idProducto_ int, idEmpresa_ int,idclienteE INT)
RETURNS TABLE(idproductoT INT ,idempresaT INT,nombreT VARCHAR(100),precioT INT,descripcionT VARCHAR(100),
			  tipoT INT, imagenT VARCHAR(200), cantstockT INT ,estadoT INT) AS
$BODY$
DECLARE
    reg RECORD;
	existe INT := 0;
BEGIN
	existe := (select idcliente from sugerencias WHERE idcliente=idclienteE and idproducto=idProducto_ and idEmpresa_=idempresa);
	if (existe<>0) then
		UPDATE sugerencias set cantidadclicks = (cantidadclicks+1) 
		WHERE idcliente=idclienteE and idproducto=idProducto_ and idEmpresa_=idempresa;
	else 
		INSERT INTO sugerencias VALUES (idProducto_,idEmpresa_,idclienteE,1);
	end if;
    FOR REG IN SELECT idproducto,idempresa,nombre,precio,descripcion,tipo,imagen,cantstock,estado
FROM producto WHERE idproducto=idProducto_ AND idempresa=idEmpresa_ LOOP
        idproductoT := reg.idproducto;
        idempresaT   := reg.idempresa;
		nombreT := reg.nombre;
		precioT := reg.precio;
		descripcionT := reg.descripcion;
		tipoT := reg.tipo;
		imagenT := reg.imagen;
		cantstockT := reg.cantstock;
		estadoT := reg.estado;
        RETURN NEXT;
    END LOOP;
    RETURN;
END
$BODY$
LANGUAGE plpgsql;
select *from detalleProducto(7, 2,2);
select * from producto
select * from sugerencias
--OBTENERPRODUCTOS--
create or replace FUNCTION obtenerProductos()
RETURNS TABLE(idproductoT INT ,idempresaT INT,nombreT VARCHAR(100),precioT INT,descripcionT VARCHAR(100),cantstockT INT ,estadoT INT) AS
$BODY$
DECLARE
    reg RECORD;
BEGIN
    FOR REG IN SELECT idproducto,idempresa,nombre,precio,descripcion,cantstock,estado
FROM producto LOOP
        idproductoT := reg.idproducto;
        idempresaT   := reg.idempresa;
		nombreT := reg.nombre;
		precioT := reg.precio;
		descripcionT := reg.descripcion;
		cantstockT := reg.cantstock;
		estadoT := reg.estado;
        RETURN NEXT;
    END LOOP;
    RETURN;
END
$BODY$
LANGUAGE plpgsql;
select * from obtenerProductos();

-- CAMBIARESTADOPRODUCTO

create or replace FUNCTION cambiarEstadoProducto(idProductoE int,idEmpresaE int)
RETURNS SETOF INTEGER 
AS $BODY$
declare estadoActual int;
begin
	estadoActual := (select estado FROM producto where idProducto=idProductoE AND idEmpresa=idEmpresaE);
	if (estadoActual=1) then
		UPDATE producto set estado=0 where idProducto=idProductoE AND idEmpresa=idEmpresaE;
		return query select 0 as resultado;
	else 
		UPDATE producto set estado=1 where idProducto=idProductoE AND idEmpresa=idEmpresaE;
		return query select 1 as resultado;
	end if;
end
$BODY$
LANGUAGE plpgsql;
select cambiarEstadoProducto(1,2)

-- CAMBIARESTADOTODALAEMPRESA--
create or replace FUNCTION cambiarEstadoTodosLosProductos(idEmpresaE int,estadoE int)
RETURNS SETOF INTEGER 
AS $BODY$
begin
	if (estadoE=1) then
		UPDATE producto set estado=1 where idEmpresa=idEmpresaE;
		return query select 1 as resultado;
	else 
		UPDATE producto set estado=0 where idEmpresa=idEmpresaE;
		return query select 0 as resultado;
	end if;
end
$BODY$
LANGUAGE plpgsql;
SELECT cambiarEstadoTodosLosProductos(2,0)

--OBTENERPRODUCTOSAPROBADOS--
--Agregué hoy
create or replace FUNCTION obtenerProductosAprobados()
RETURNS TABLE(idproductoT INT,idempresaT INT,nombreT VARCHAR(100),precioT INT,imagenT VARCHAR(200)) AS
$BODY$
DECLARE
    reg RECORD;
BEGIN
    FOR REG IN SELECT idproducto,idempresa,nombre,precio,imagen
FROM producto where estado=1 LOOP
        idproductoT := reg.idproducto;    
		idempresaT := reg.idempresa;
		nombreT := reg.nombre;
		precioT := reg.precio;
		imagenT := reg.imagen;
        RETURN NEXT;
    END LOOP;
    RETURN;
END
$BODY$
LANGUAGE plpgsql;

select AgregarCarrito(2, 2, 2, 1);
select * from clientes;
select * from producto;
select * from carrito;


create or replace FUNCTION AgregarCarrito(idproductoT INT,idempresaT INT, idClienteT INT, cantidad INT)
RETURNS SETOF INTEGER 
AS $BODY$
begin
	if exists (Select idCliente, idproducto, idempresa from Carrito 
			   where idCliente=idClienteT AND idproducto=idproductoT AND idempresa=idempresaT ) then
		UPDATE Carrito set cant=cant+cantidad where idCliente=idClienteT AND idproducto=idproductoT AND idempresa=idempresaT;
	else 
		Insert into Carrito values (idClienteT, idproductoT, idempresaT, cantidad);
	end if;
	
end
$BODY$
LANGUAGE plpgsql;
select * from AgregarCarrito(3,2,2,1)
select * from producto
-------------------------------------------------------------------
--OBTENER CARRITO DE UN CLIENTE
create or replace FUNCTION obtenerProductosCarritos(idClienteE INT)
RETURNS TABLE(idcontT INT,idclienteT INT,idproductoT INT,idempresaT INT,nombreT VARCHAR(100),precioT INT,imagenT VARCHAR(200),cantidadProductosT INT,cantidadStockT INT) AS
$BODY$
DECLARE
    reg RECORD;
BEGIN
    FOR REG IN select ca.idcliente,ca.cant,ca.idproducto,ca.idempresa,p.nombre,p.precio,p.imagen, p.cantstock from carrito as ca 
			join producto as p ON ca.idproducto = p.idproducto
				join clientes as c ON ca.idcliente = c.idcliente
					where ca.idcliente = idClienteE LOOP
					idclienteT 	:= reg.idcliente;
					idproductoT := reg.idproducto;
					idempresaT := reg.idempresa;
					nombreT := reg.nombre;
					precioT := reg.precio;
					imagenT := reg.imagen;
					cantidadStockT := reg.cantstock;
					cantidadProductosT := reg.cant;
					RETURN NEXT;
					END LOOP;
    RETURN;
END
$BODY$
LANGUAGE plpgsql; 
select * from obtenerProductosCarritos(2)
------------------------------------------------------------------
--detalleCompraProducto esta en veremos
create or replace FUNCTION detalleCompraCarrito(idClienteE INT,idEmpresaE INT,idProductoE INT,cantidadProductosE INT)
RETURNS TABLE(idproductoT INT,idempresaT INT,nombreT VARCHAR(100),precioT INT,montoTotalT INT) AS
$BODY$
DECLARE
	cantidadStock INT;
	montoTotal INT;
    reg RECORD;
BEGIN
    FOR REG IN select idproducto,idempresa,nombre,precio
		ca.idcliente,ca.cant,ca.idproducto,ca.idempresa,p.nombre,p.precio,p.imagen from carrito as ca 
			join producto as p ON ca.idproducto = p.idproducto
				join clientes as c ON ca.idcliente = c.idcliente
					where ca.idcliente = idClienteE LOOP
					idclienteT 	:= reg.idcliente;
					idproductoT := reg.idproducto;
					idempresaT := reg.idempresa;
					nombreT := reg.nombre;
					precioT := reg.precio;
					imagenT := reg.imagen;
					cantidadProductosT := reg.cant;
					RETURN NEXT;
					END LOOP;
    RETURN;
END
$BODY$
LANGUAGE plpgsql; 
------------------------------------------------------------------
--AUMENTAR O DISMUNIR CANTIDAD CARRITO
create or replace FUNCTION cantDelCarrito(idproductoE INT,idempresaE INT, idClienteE INT, accionE INT)
RETURNS SETOF INTEGER 
AS $BODY$
begin
	if (accionE=1) then
		UPDATE Carrito set cant=cant+1 where idCliente=idClienteE AND idproducto=idproductoE AND idempresa=idempresaE;
	else 
		UPDATE Carrito set cant=cant-1 where idCliente=idClienteE AND idproducto=idproductoE AND idempresa=idempresaE;
	end if;
end
$BODY$
LANGUAGE plpgsql;
select from cantDelCarrito(4,2,2,1)
------------------------------------------------------------------
--eliminarDelCarrito
create or replace FUNCTION eliminarDelCarrito(idproductoE INT,idempresaE INT, idClienteE INT)
RETURNS SETOF INTEGER 
AS $BODY$
begin
	DELETE FROM Carrito where idCliente=idClienteE AND idproducto=idproductoE AND idempresa=idempresaE;
end
$BODY$
LANGUAGE plpgsql;
select from eliminarDelCarrito(3,2,2)
------------------------------------------------------------------
select * from clientes;
select * from producto;
select * from carrito;
------------------------------------------------------------------
--REALIZARCOMPRA--
create or replace FUNCTION realizarCompra(idClienteE INT)
RETURNS SETOF INTEGER 
AS $BODY$
DECLARE
	idProductoo INT;
	cantidadCompra INT;
	cantidadCarrito INT := (SELECT COUNT(idcliente) from carrito where idcliente = idClienteE);
	i int :=0;
begin
	WHILE (i < cantidadCarrito)
		LOOP
		idProductoo := (select idproducto from carrito limit 1);
		cantidadCompra:= (select cant from carrito where idcliente = idClienteE and idProductoo = idproducto);
		UPDATE producto set cantstock = (cantstock-cantidadCompra) where idproducto = idProductoo;
		DELETE FROM carrito where idcliente = idClienteE and idproducto = idProductoo;
		Raise Notice 'ID: %',i;
		i := i+1;
		END LOOP;
end
$BODY$
LANGUAGE plpgsql;
------------------------------------------------------------------
--OBTENER CORREO EMPRESA
create or replace FUNCTION obtenerCorreo(idempresaE INT)
RETURNS TABLE (correoS VARCHAR(100))
AS $BODY$
DECLARE
reg RECORD;
begin
	FOR REG IN SELECT correo from empresa 
		where idempresa = idempresaE 
		LOOP
		correoS:= reg.correo;
		RETURN NEXT;
		END LOOP;
    RETURN;
end
$BODY$
LANGUAGE plpgsql;
select correos from obtenerCorreo(2)
------------------------------------------------------------------
--AUMENTAR O DISMUNIR CANTIDAD CARRITO
create or replace FUNCTION aumentarDisminuir(idClienteE INT,idProductoE INT,accionE INT, cantidadE INT)
RETURNS SETOF INTEGER 
AS $BODY$
begin

	UPDATE carrito set cant =
end
$BODY$
LANGUAGE plpgsql;
------------------------------------------------------------------
--AUMENTA CONTAR DE LOS PRODUCTOS MAS VISTOS
create or replace FUNCTION aumentarSugerencias(tipoProductoE INT,idClienteE INT)
RETURNS SETOF INTEGER 
AS $BODY$
begin
	UPDATE sugerencias set cantidadclicks = (cantidadclicks+1) 
		where tipoproducto = tipoProductoE 
			AND idcliente = idClienteE;
end
$BODY$
LANGUAGE plpgsql;
select from realizarCompra (2);
------------------------------------------------------------------
--OBTENERCATEGORIAS
create or replace FUNCTION obtenerCategorias()
RETURNS TABLE(idproductoT INT ,nombretipoT VARCHAR(100)) AS
$BODY$
DECLARE
    reg RECORD;
BEGIN
    FOR REG IN SELECT idproducto,nombretipo
FROM tipoproducto where estado =1 LOOP
        idproductoT := reg.idproducto;
        nombretipoT := reg.nombretipo;
        RETURN NEXT;
    END LOOP;
    RETURN;
END
$BODY$
LANGUAGE plpgsql;
select * from obtenerCategorias();
-----------------------------------------------------------------
update producto set estado = 1 where idproducto<>0
select * from sugerencias
------------------------------------------------------------------
--OBTENERPRODUCTOS SEGUN EL TIPO--
create or replace FUNCTION obtenerProductosPorTipo(idtipo INT)
RETURNS TABLE(idproductoT INT ,idempresaT INT,nombreT VARCHAR(100),precioT INT,imagenT VARCHAR(100)) AS
$BODY$
DECLARE
    reg RECORD;
BEGIN
    FOR REG IN SELECT idproducto,idempresa,nombre,precio,imagen
FROM producto WHERE tipo = idtipo LOOP
        idproductoT := reg.idproducto;
        idempresaT   := reg.idempresa;
		nombreT := reg.nombre;
		precioT := reg.precio;
		imagenT := reg.imagen;
        RETURN NEXT;
    END LOOP;
    RETURN;
END
$BODY$
LANGUAGE plpgsql;
select * from obtenerProductosPorTipo(1);
---------------------------------------------------------
create or replace FUNCTION obtenerProductosSugeridos(idclienteE INT)
RETURNS TABLE(idproductoT INT ,idempresaT INT,nombreT VARCHAR(100),precioT INT,imagenT VARCHAR(100)) AS
$BODY$
DECLARE
    reg RECORD;
BEGIN
    FOR reg IN 
	SELECT s.idproducto,s.idempresa,p.nombre,p.precio,p.imagen  
		FROM sugerencias as s 
			JOIN producto as p ON p.idproducto = s.idproducto
				WHERE idcliente =idclienteE ORDER BY(cantidadclicks) DESC LIMIT 5 LOOP
					idproductoT := reg.idproducto;
					idempresaT := reg.idempresa;
					nombreT := reg.nombre;
					precioT := reg.precio;
					imagenT := reg.imagen;
				RETURN NEXT;
    END LOOP;
    RETURN;
END
$BODY$
LANGUAGE plpgsql;
select * from producto
select * from empresa 	
SELECT * FROM tipoproducto

INSERT INTO PRODUCTO (idProducto, idEmpresa, nombre, precio, descripcion, tipo, imagen, cantStock, estado) 
	VALUES(12, 1,'Jabon protex',2000,'Jabón para lavar tus manos', 6, 'jaboncito.png', 115,1);
update producto set idempresa=4 where tipo <>1
update producto set nombre='Tennis Puma',precio= 35000,descripcion='Tennis para correr',imagen='tennisPuma.jpg' where idproducto = 5
select * from obtenerProductosSugeridos(2);
------------------------------------------------------------------
--OBTENERPRODUCTOS SUGERIDOS--
create or replace FUNCTION obtenerTiposSugeridos(idclienteE INT)
RETURNS TABLE(tipoproductoT INT ,nombretipoT VARCHAR(100),cantidadclicksT INT,idclienteT INT) AS
$BODY$
DECLARE
    reg RECORD;
BEGIN
    FOR reg IN 
	SELECT t.tipoproducto,idcliente,cantidadclicks,p.nombretipo FROM (
		SELECT tipoproducto,MAX(cantidadclicks) as cliks
			FROM sugerencias
				GROUP BY tipoproducto
		) t JOIN sugerencias m ON m.tipoproducto = t.tipoproducto
				JOIN tipoproducto p ON p.idproducto = m.tipoproducto
					WHERE idcliente = idclienteE
						ORDER BY (cantidadclicks) desc limit 3 LOOP
        tipoproductoT := reg.tipoproducto;
        nombretipoT   := reg.nombretipo;
		cantidadclicksT := reg.cantidadclicks;
		idclienteT := reg.idcliente;
        RETURN NEXT;
    END LOOP;
    RETURN;
END
$BODY$
LANGUAGE plpgsql;
select * from obtenerTiposSugeridos(2);
--HAY QUE HACER OTRO SOLO POR CANTIDAD DE CLICKS--
---------------------------------------------------------
create or replace FUNCTION obtenerProductosPorTipoSuge(tipoE INT,idclienteE INT)
RETURNS TABLE(idproductoT INT ,idempresaT INT,nombreT VARCHAR(100),precioT INT,descripcionT VARCHAR(100),cantstockT INT ,estadoT INT) AS
$BODY$
DECLARE
    reg RECORD;
	existe INT := 0;
BEGIN
	existe := (select idcliente from sugerencias WHERE idcliente=idclienteE and tipoproducto=tipoE);
	if (existe<>0) then
		UPDATE sugerencias set cantidadclicks = (cantidadclicks+1) 
		where tipoproducto = tipoE 
			AND idcliente = idclienteE;
	else 
		INSERT INTO sugerencias VALUES (tipoE,idclienteE,1);
	end if;
    FOR REG IN SELECT idproducto,idempresa,nombre,precio,descripcion,cantstock,estado
FROM producto WHERE tipo = tipoE LOOP
        idproductoT := reg.idproducto;
        idempresaT   := reg.idempresa;
		nombreT := reg.nombre;
		precioT := reg.precio;
		descripcionT := reg.descripcion;
		cantstockT := reg.cantstock;
		estadoT := reg.estado;
        RETURN NEXT;
    END LOOP;
    RETURN;
END
$BODY$
LANGUAGE plpgsql;
select * from obtenerProductosPorTipoSuge(3,2);
--HAY QUE HACER OTRO SOLO POR CANTIDAD DE CLICKS--
---------------------------------------------------------
