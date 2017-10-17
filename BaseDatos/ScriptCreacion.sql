create database GasStationPharmacy3RB
GO

Use GasStationPharmacy3RB
GO

CREATE TABLE [dbo].[ADMINISTRADORXSUCURSAL](
	[Administrador] [int] NOT NULL,
	[Sucursal] [varchar](50) NOT NULL,
	[Activo] [bit] NOT NULL,
	PRIMARY KEY (Administrador));

CREATE TABLE [dbo].[CASAFARMACEUTICA](
	[Nombre] [varchar](50) NOT NULL,
	[Sede] [varchar](50) NULL,
	[Activo] [bit] NOT NULL,
	PRIMARY KEY (Nombre));

CREATE TABLE [dbo].[CLIENTE](
	[Cedula] [int] NOT NULL,
	[Nombre1] [varchar](20) NOT NULL,
	[Nombre2] [varchar](20) NULL,
	[Apellido1] [varchar](20) NOT NULL,
	[Apellido2] [varchar](20) NULL,
	[Provincia] [varchar](10) NULL,
	[Cuidad] [varchar](30) NULL,
	[Senas] [varchar](50) NULL,
	[FechaNacimiento] [date] NOT NULL,
	[Prioridad] [int] NOT NULL,
	[Contrasena] [varchar](200) NOT NULL,
	[Activo] [bit] NOT NULL,
	PRIMARY KEY (Cedula));

CREATE TABLE [dbo].[COMPAÑIA](
	[Nombre] [varchar](50) NOT NULL,
	[Sede] [varchar](50) NULL,
	[Activo] [bit] NOT NULL,
	PRIMARY KEY (Nombre));

CREATE TABLE [dbo].[EMPLEADO](
	[Cedula] [int] NOT NULL,
	[Nombre1] [varchar](20) NOT NULL,
	[Nombre2] [varchar](20) NULL,
	[Apellido1] [varchar](20) NOT NULL,
	[Apellido2] [varchar](20) NULL,
	[Provincia] [varchar](20) NOT NULL,
	[Cuidad] [varchar](20) NULL,
	[Señas] [varchar](50) NULL,
	[FechaNacimiento] [date] NOT NULL,
	[Contraseña] [varchar](200) NOT NULL,
	[Sucursal] [varchar](50) NOT NULL,
	[Activo] [bit] NOT NULL,
	[Rol] [varchar](20) NOT NULL,
	PRIMARY KEY (Cedula));

CREATE TABLE [dbo].[MEDICAMENTO](
	[Nombre] [varchar](50) NOT NULL,
	[Prescripcion] [bit] NOT NULL,
	[Cantidad] [int] NOT NULL,
	[CasaFarmaceutica] [varchar](50) NOT NULL,
	[Activo] [bit] NOT NULL,
	[Precio] [int] NOT NULL,
	PRIMARY KEY (Nombre));

CREATE TABLE [dbo].[MEDICAMENTOXPEDIDO](
	[Pedido] [int] NOT NULL,
	[Medicamento] [varchar](50) NOT NULL,
	[Activo] [bit] NOT NULL,
	[Cantidad] [int] NOT NULL,
	[Receta] [int] NULL,
	PRIMARY KEY (Medicamento,Pedido));

CREATE TABLE [dbo].[MEDICAMENTOXRECETA](
	[Receta] [int] NOT NULL,
	[Medicamento] [varchar](50) NOT NULL,
	[Activo] [bit] NOT NULL,
	[Cantidad] [int] NOT NULL,
	PRIMARY KEY (Medicamento,Receta));

CREATE TABLE [dbo].[PADECIMIENTO](
	[Cliente] [int] NOT NULL,
	[Padecimiento] [varchar](50) NOT NULL,
	[Año] [int] NULL,
	[Activo] [bit] NOT NULL,
	PRIMARY KEY (Padecimiento,Cliente));

CREATE TABLE [dbo].[PEDIDO](
	[Numero] [int] IDENTITY(1,1) NOT NULL,
	[Estado] [char](1) NOT NULL,
	[FechaRecojo] [datetime] NOT NULL,
	[Telefono] [int] NOT NULL,
	[Sucursal] [varchar](50) NOT NULL,
	[Cliente] [int] NOT NULL,
	[Activo] [bit] NOT NULL,
	[Nombre] [varchar](20) NOT NULL,
	PRIMARY KEY (Numero));

CREATE TABLE [dbo].[RECETA](
	[Numero] [int] NOT NULL,
	[Imagen] [image] NOT NULL,
	[Activo] [bit] NOT NULL,
	[Cliente] [int] NOT NULL,
	[Usado] [bit] NOT NULL,
	[Doctor] [varchar](50) NOT NULL,
	[Nombre] [varchar](20) NOT NULL,
	PRIMARY KEY (Numero));

CREATE TABLE [dbo].[ROL](
	[Nombre] [varchar](20) NOT NULL,
	[Descripcion] [varchar](50) NULL,
	[Activo] [bit] NOT NULL,
	PRIMARY KEY (Nombre));

CREATE TABLE [dbo].[SUCURSAL](
	[Nombre] [varchar](50) NOT NULL,
	[Provincia] [varchar](20) NULL,
	[Cuidad] [varchar](20) NULL,
	[Señas] [varchar](50) NULL,
	[Descripcion] [varchar](50) NULL,
	[Compañia] [varchar](50) NOT NULL,
	[Activo] [bit] NOT NULL,
	PRIMARY KEY (Nombre));

CREATE TABLE [dbo].[TELEFONOXCLIENTE](
	[Cliente] [int] NOT NULL,
	[Telefono] [int] NOT NULL,
	[Activo] [bit] NOT NULL,
	PRIMARY KEY (Cliente,Telefono));


ALTER TABLE [ADMINISTRADORXSUCURSAL] 
ADD CONSTRAINT FK_ADMIN FOREIGN KEY (Administrador) REFERENCES EMPLEADO(Cedula)

ALTER TABLE [ADMINISTRADORXSUCURSAL] 
ADD CONSTRAINT FK_ADMIN_SUC FOREIGN KEY (Sucursal) REFERENCES Sucursal(Nombre)

ALTER TABLE EMPLEADO 
ADD CONSTRAINT FK_EMPLEADO_SUC FOREIGN KEY (Sucursal) REFERENCES Sucursal(Nombre)

ALTER TABLE MEDICAMENTO 
ADD CONSTRAINT FK_MEDICAMENTO_CASA FOREIGN KEY (CasaFarmaceutica) REFERENCES CasaFarmaceutica(Nombre)

ALTER TABLE MEDICAMENTOXPEDIDO 
ADD CONSTRAINT FK_MEDICAMENTO_PED FOREIGN KEY (Pedido) REFERENCES PEDIDO(Numero)

ALTER TABLE MEDICAMENTOXPEDIDO 
ADD CONSTRAINT FK_MEDICAMENTO_PED2 FOREIGN KEY (Medicamento) REFERENCES Medicamento(Nombre)

ALTER TABLE MEDICAMENTOXRECETA
ADD CONSTRAINT FK_MEDICAMENTO_REC FOREIGN KEY (Receta) REFERENCES Receta(Numero)

ALTER TABLE MEDICAMENTOXRECETA 
ADD CONSTRAINT FK_MEDICAMENTO_REC2 FOREIGN KEY (Medicamento) REFERENCES Medicamento(Nombre)

ALTER TABLE PADECIMIENTO 
ADD CONSTRAINT FK_PAD_CLI FOREIGN KEY (Cliente) REFERENCES Cliente(Cedula)

ALTER TABLE PEDIDO 
ADD CONSTRAINT FK_PED_SUC FOREIGN KEY (Sucursal) REFERENCES SUCURSAL(Nombre)

ALTER TABLE PEDIDO 
ADD CONSTRAINT FK_PED_CLI FOREIGN KEY (Cliente) REFERENCES CLIENTE(Cedula)

ALTER TABLE RECETA 
ADD CONSTRAINT FK_REC_CLI FOREIGN KEY (Cliente) REFERENCES CLIENTE(Cedula)

ALTER TABLE SUCURSAL 
ADD CONSTRAINT FK_SUC_COMP FOREIGN KEY (Compañia) REFERENCES COMPAÑIA(Nombre)

ALTER TABLE TELEFONOXCLIENTE 
ADD CONSTRAINT FK_TEL_CLI FOREIGN KEY (Cliente) REFERENCES CLIENTE(Cedula)